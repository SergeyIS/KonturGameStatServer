using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kontur.GameStats.ControllersCore.Types
{
    class RequestHandler : IDisposable
    {
        private static Dictionary<string, string> routes;
        private HttpListenerContext context;
        private RequestMap requestmap;
        private Type contrtype;
        private MethodInfo exemethod;

        /// <summary>
        /// Регистрирует маршруты в приложении.
        /// </summary>
        public static void RegisterRoutes()
        {
            routes = new Dictionary<string, string>();
            /*
             Регистрация маршрутов. Маршрут представляет собой строку вида: 
             "/{controller:name}/{param:regular_expression}/{method:name}". 
             Такой маршрут сопоставляется с http запросом, при учитывается последовательность 
             указания элементов {key:value}; 
             */

            routes.Add("/servers/endpoint/info", "/{controller:servers}/{param:[a-zA-Z0-9_]+}/{method:info}");
            routes.Add("/servers/endpoint/matches/timestamp", "/{controller:servers}/{param:[a-zA-Z0-9_]+}/{method:matches}/{param:[a-zA-Z0-9_]+}");
            routes.Add("/servers/info", "/{controller:servers}/{method:info}");
            routes.Add("/servers/endpoint/stats", "/{controller:servers}/{param:[a-zA-Z0-9_]+}/{method:stats}");
            routes.Add("/players/name/stats", "/{controller:players}/{param:[a-zA-Z0-9_]+}/{method:stats}");
            routes.Add("/reports/recent-matches/count", "/{controller:reports}/{method:recent-matches}/{param:[0-9]*}");
            routes.Add("/reports/best-players/count", "/{controller:reports}/{method:best-players}/{param:[0-9]*}");
            routes.Add("/reports/popular-servers/count", "/{controller:reports}/{method:popular-servers}/{param:[0-9]*}");
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
            if (!Mapping()) //Не найден метод контроллера.(Получены значения contrtype и exemethod).
            {
                WriteResponse("Не найден метод контроллера");
                return;
            }

            object obj = contrtype.GetConstructor(new Type[0]).Invoke(null);
            object result = null;
            try
            {
                if (context.Request.HttpMethod.Equals("PUT"))
                {
                    string data = ReadRequestBody();
                    requestmap.Param.Add(data);
                }
               
                result = exemethod.Invoke(obj, requestmap.Param.ToArray());

                if(result != null)
                    WriteResponse(result.ToString());

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            } 
        }

        /// <summary>
        /// Возвращает объект RequestMap, 
        /// которые представляет собой карту запроса. С помощью объекта RequestMap
        /// можно получить вызываемый(е) методы(ы) контроллера. Если соответствий не найдено, то возвращает null.
        /// </summary>
        private RequestMap GetRequestMap()
        {
            string req = context.Request.Url.AbsolutePath;
            RequestMap map = null;
            foreach (var rte in routes)
            {
                map = new RequestMap(rte.Value);
                if (map.ExecuteMapping(req))
                    return map;
            }
            return null;
        }

        /// <summary>
        /// Получает значения contrtype и exemethod.
        /// </summary>
        private bool Mapping()
        {
            if (requestmap == null) return false;

            contrtype = Type.GetType(
            "Kontur.GameStats.Controllers." + requestmap.Controller + "Controller", false, true);
            if (contrtype == null) return false;

            MemberInfo[] members = contrtype.FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, (m, c) => m.Name.ToLower().Equals(c), requestmap.Method.ToLower());
            Type attr = Type.GetType(
                "Kontur.GameStats.ControllersCore.Types.Http" + context.Request.HttpMethod + "Attribute", false, true);
            foreach (MethodInfo method in members)
            {
                if (method.IsDefined(attr))
                {
                    exemethod = method;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Записывет сообщение в выходной поток.
        /// </summary>
        private void WriteResponse(string resp)
        {
            using (var output = context.Response.OutputStream)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(resp);
                output.Write(bytes, 0, bytes.Length);
            }
        }
        
        private string ReadRequestBody()
        {
            string data = String.Empty;
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
