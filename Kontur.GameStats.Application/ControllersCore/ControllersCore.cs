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

        public string Pattern { get; private set; }
        public int PatternLength { get; private set; }
        public string[] PatternCollecton { get; private set; }
        public string[] RequestCollection { get; private set; }
        public string ControllerName { get; private set; }
        public string MethodName { get; private set; }
        public IDictionary<string, object> Parametrs { get; private set; }

        public RequestMap(string pattern, string[] request_collection)
        {
            Pattern = pattern;
            PatternLength = Pattern.Length;
            Parametrs = new Dictionary<string, object>();
            PatternCollecton = Pattern.Replace("{", "").Replace("}", "").Split('/');
            RequestCollection = request_collection;
        }

        public bool ExecuteMapping(string reqstr)
        {

            if (reqstr.Equals(String.Empty)) return false;

            Regex RegEx = GetRegExFromPattern();
            Match m = RegEx.Match(reqstr);
            if (!m.Value.Equals(reqstr)) return false;

            for (int i = 0; i < PatternCollecton.Length; i++)
            {
                var obj = PatternCollecton[i].Split(':');
                if (obj.Length >= 2)
                {
                    if(obj[0] == "controller")
                    {
                        ControllerName = obj[1];
                    }
                    else if(obj[0] == "method")
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
