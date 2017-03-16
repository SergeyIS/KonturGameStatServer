using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Kontur.GameStats.Application.Core
{
    public class RequestHandler : IDisposable
    {
        private static IList<string> routes;
        private HttpListenerContext context;
        private RequestMap requestmap;
        private Type ControllerType;
        private MethodInfo ExeMethod;

        /// <summary>
        /// Регистрирует маршруты в приложении.
        /// </summary>
        public static void RegisterRoutes()
        {
            routes = new List<string>();
            /*
             Регистрация маршрутов. Маршрут представляет собой строку вида: 
             "{controller:name}/{param:name}/{method:name}". Здесь предполагается, что названия параметров будут соответствовать
             названию методов в контролленре. Контроллеры и методы, в свою очередь, 'помечаются' атрибутом [Name("name")].  
             Такой маршрут сопоставляется с http запросом, при этом учитывается последовательность 
             указания элементов {key:value}; 
             */

            routes.Add("{controller:servers}/{param:endpoint}/{method:info}");
            routes.Add("{controller:servers}/{param:endpoint}/{method:matches}/{param:timestamp}");
            routes.Add("{controller:servers}/{method:info}");
            routes.Add("{controller:servers}/{param:endpoint}/{method:stats}");
            routes.Add("{controller:players}/{param:name}/{method:stats}");
            routes.Add("{controller:reports}/{method:recent-matches}/{param:count}");
            routes.Add("{controller:reports}/{method:best-players}/{param:count}");
            routes.Add("{controller:reports}/{method:popular-servers}/{param:count}");
            routes.Add("{controller:reports}/{method:recent-matches}");
            routes.Add("{controller:reports}/{method:best-players}");
            routes.Add("{controller:reports}/{method:popular-servers}");
        } 
        public RequestHandler(HttpListenerContext context)
        {
            this.context = context;
            this.requestmap = GetRequestMap();

        }
        /// <summary>
        /// Вызывает метот контроллера и передает ему параметры.
        /// </summary>
        public void Execute()
        {
            if (context.Request.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                object data = ReadRequestBody();
                requestmap.Parametrs.Add("data", data);
            }

            if (!Mapping()) //Не найден метод контроллера, соответствующий трубуемым параметрам
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Response.Close();
                return;
            }
            object obj = ControllerType.GetConstructor(new Type[0]).Invoke(null);
            object result = null;
            try
            {
               
                ParameterInfo[] parameters = ExeMethod.GetParameters();
                object[] order_param = new object[parameters.Count()];
                //Определим тип параметров для передачи
                for (int i = 0; i < parameters.Count(); i++)
                {
                    var StrParamValue = requestmap.Parametrs[parameters[i].Name];
                    Type ParamType = parameters[i].ParameterType;
                    if (ParamType.Name.Equals(typeof(String).Name))
                    {
                        order_param[i] = Convert.ToString(StrParamValue);
                    }
                    else if (ParamType.Name.Equals(typeof(Int32).Name))
                    {
                        order_param[i] = Convert.ToInt32(StrParamValue);
                    }
                    else if (ParamType.Name.Equals(typeof(DateTime).Name))
                    {
                        order_param[i] = Convert.ToDateTime(StrParamValue).ToUniversalTime();
                    }
                    else if (ParamType.Name.Equals(typeof(Boolean).Name))
                    {
                        order_param[i] = Convert.ToBoolean(StrParamValue);
                    }
                    else//Указан тип модели
                    {
                        DataContractJsonSerializer jresp = new DataContractJsonSerializer(ParamType);
                        using (MemoryStream mswrriter = new MemoryStream())
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes(Convert.ToString(StrParamValue));
                            mswrriter.Write(bytes, 0, bytes.Length);
                            mswrriter.Position = 0;
                            order_param[i] = jresp.ReadObject(mswrriter);
                            //

                        //
                        }
                    }
                }
                result = ExeMethod.Invoke(obj, order_param);
                if (result != null)
                    WriteResponse(result);
            }
            catch(Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                Logger.Log.WriteErrorLog(e);
            }
            finally
            {
                context.Response.Close();
            }
        }

        /// <summary>
        /// Возвращает объект RequestMap, 
        /// который представляет собой карту запроса. С помощью объекта RequestMap
        /// можно получить вызываемый(е) методы(ы) контроллера. Если соответствий не найдено, то возвращает null.
        /// </summary>
        private RequestMap GetRequestMap()
        {
            string request = context.Request.Url.AbsolutePath;
            string[] request_collection;
            request = request.Replace(" ", "");
            request = request.Trim('/');
            request_collection = request.Split('/');
            RequestMap map = null;
            foreach (var rte in routes)
            {
                map = new RequestMap(rte, request_collection);
                if (map.ExecuteMapping(request))
                    return map;
            }
            return null;
        }

        /// <summary>
        /// Получает значения ControllerType и ExeMethod, являющиеся типами контроллера и метода соответственно.
        /// </summary>
        private bool Mapping()
        {
            try
            {
                if (requestmap == null) return false; //Не нашлось соответствий маршруту.

                ControllerType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(n => n.IsClass
                && n.GetCustomAttribute<NameAttribute>() != null &&
                n.GetCustomAttribute<NameAttribute>().Name.Equals(requestmap.ControllerName.ToLower()));
                if (ControllerType == null) return false;

                ExeMethod = ControllerType.GetMethods().Where(
                 n => n.GetCustomAttribute<NameAttribute>() != null && n.GetCustomAttribute<MethodAttribute>() != null &&
                 n.GetCustomAttribute<NameAttribute>().Name.Equals(requestmap.MethodName.ToLower()) &&
                 n.GetCustomAttribute<MethodAttribute>().Method.ToString().Equals(context.Request.HttpMethod,
                 StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault(
                    m => m.GetParameters().Where(
                        p => requestmap.Parametrs.Keys.Contains(p.Name)).Count().Equals(m.GetParameters().Count()) &&
                        m.GetParameters().Count().Equals(requestmap.Parametrs.Count()));
                if (ExeMethod == null) return false;
            }
            catch(Exception e)
            {

            }

            return true;
        }
        /// <summary>
        /// Записывет сообщение в выходной поток.
        /// </summary>
        private void WriteResponse(object resp)
        {
            try
            {
                if (resp is HttpStatusCode)//был ли передан статус код.
                {
                    context.Response.StatusCode = (int)resp;
                    return;
                }
                else if (resp is string)
                {
                    using (var output = context.Response.OutputStream)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(resp.ToString());
                        output.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    //выполнить сериализацию, и записать ее в result
                    DataContractJsonSerializer jresp = new DataContractJsonSerializer(resp.GetType());
                    jresp.WriteObject(context.Response.OutputStream, resp);
                }
            }
            catch(Exception ex)
            {
                //Log.Write(ex);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            
        }
        /// <summary>
        /// Считывает тело запроса.
        /// </summary>
        private object ReadRequestBody()
        {
            object data;
            using (StreamReader reader = new StreamReader(context.Request.InputStream))
            {
                data = reader.ReadToEnd();
            }
            return data;
        }
        public void Dispose()
        {
            if (context != null)
                context.Response.OutputStream.Close();
        }
    }
}
