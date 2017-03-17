using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kontur.GameStats.Application.Core
{
    enum HttpMethods { GET, PUT, POST, DELETE };
    class NameAttribute : Attribute
    {
        private string name;
        public string Name { get { return name; } }

        public NameAttribute(string name)
        {
            this.name = name;
        }
    }
    class MethodAttribute : Attribute
    {
        public HttpMethods Method { get; private set; }
        public MethodAttribute(HttpMethods method)
        {
            Method = method;
        }

    }
    class RequestMap
    {
        public string Pattern { get; private set; }//Шаблон маршрута
        public string[] PatternCollecton { get; private set; }//Коллекция элементов "key:value"
        public string[] RequestCollection { get; private set; }//Коллекция элементов "request-part"
        public string ControllerName { get; private set; }//Имя контроллера
        public string MethodName { get; private set; }//Имя вызываемого метода
        public IDictionary<string, object> Parametrs { get; private set; }//Параметры вызываемого метода



        public RequestMap(string pattern, string[] request_collection)
        {
            Pattern = pattern;
            Parametrs = new Dictionary<string, object>();
            PatternCollecton = Pattern.Replace("{", "").Replace("}", "").Split('/');
            RequestCollection = request_collection;
        }

        public bool ExecuteMapping(string reqstr)
        {
            try
            {
                if (reqstr.Equals(String.Empty)) return false;

                Regex RegEx = GetRegExFromPattern();
                Match m = RegEx.Match(reqstr);
                if (!m.Value.Equals(reqstr)) return false;

                //Парсинг запроса
                for (int i = 0; i < PatternCollecton.Length; i++)
                {
                    var obj = PatternCollecton[i].Split(':');
                    if (obj.Length >= 2)
                    {
                        if (obj[0] == "controller")
                        {
                            ControllerName = obj[1];
                        }
                        else if (obj[0] == "method")
                        {
                            MethodName = obj[1];
                        }
                        else
                        {
                            Parametrs.Add(obj[1], RequestCollection[i]);
                        }
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Logger.Log.WriteErrorLog(e);
                return false;
            }
        }

        private Regex GetRegExFromPattern()
        {
            string regpat = String.Empty;

            for (int i = 0; i < PatternCollecton.Length; i++)
            {
                var obj = PatternCollecton[i].Split(':');
                if (obj.Length >= 2)
                {
                    if (obj[0] == "param") obj[1] = "[^/*!]+";
                    regpat += obj[1] + "/";
                }
                else
                {
                    regpat += obj[0] + "/";
                }
                
            }
            return new Regex(regpat.Trim('/'), RegexOptions.IgnoreCase);
        }
    }
}
