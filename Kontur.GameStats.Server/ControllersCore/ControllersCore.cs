using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kontur.GameStats.ControllersCore.Types
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
    class HttpGetAttribute : Attribute
    {

    }
    class HttpPostAttribute : Attribute
    {

    }
    class HttpPutAttribute : Attribute
    {

    }
    class RequestMap
    {

        private string pattern;
        private string controllername;
        private string methodname;
        private List<object> param;
        public string Pattern { get { return (pattern!=null) ? pattern : null; } }
        public string Controller { get { return (controllername != null) ? controllername : null; } }
        public string Method { get { return (methodname != null) ? methodname : null; } }
        public List<object> Param { get { return param; } }
        public RequestMap(string pattern)
        {
            this.pattern = pattern;
            param = new List<object>();
        }

        public bool ExecuteMapping(string reqstr)
        {
            if (pattern == "") return false;
            pattern.Trim('/');
            reqstr = reqstr.Replace(" ", "");
            reqstr = reqstr.Trim('/');
            var partsreq = reqstr.Split('/');

            Regex convreg = ConvertPattern();
            if (!convreg.IsMatch(reqstr)) return false;

            pattern = pattern.Replace("/","");

            string buff = String.Empty;
            int partreqindex = 0;
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '{')
                {
                    buff = String.Empty;
                }
                else if (pattern[i] == '}')
                {
                    var values = buff.Split(':');
                    if (values[0] == "controller")
                    {
                        controllername = partsreq[partreqindex];
                    }
                    else if (values[0] == "method")
                    {
                        methodname = partsreq[partreqindex].Replace("-","");
                    }
                    else
                    {
                        param.Add(partsreq[partreqindex]);
                    }
                    partreqindex++;
                }
                else
                {
                    buff += pattern[i];
                }
            }
            return true;
        }

        private Regex ConvertPattern()
        {
            string regpat = pattern;
            regpat = regpat.Replace(" ", "");
            regpat = regpat.Replace("{", "");
            regpat = regpat.Replace("}", "");
            regpat = regpat.Replace("controller:", "");
            regpat = regpat.Replace("param:", "");
            regpat = regpat.Replace("method:", "");
            regpat = regpat.Trim('/');
            return new Regex(regpat);
        }
    }
}
