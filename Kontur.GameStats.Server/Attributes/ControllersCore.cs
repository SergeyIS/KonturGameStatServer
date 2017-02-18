using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kontur.GameStats.ControllersCore.Attributes
{
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
}
namespace Kontur.GameStats.ControllersCore.Types
{
    enum HttpMethods { GET, PUT, POST, DELETE };

    class RequestMap
    {

        private string pattern;
        private Type controller;
        private MethodInfo method;
        private List<object> param;
        public string Pattern { get { return (pattern!=null) ? pattern : null; } }
        public Type Controller { get { return (controller != null) ? controller : null; } }
        public MethodInfo Method { get { return (method != null) ? method : null; } }
        public List<object> Param { get { return (param.Count > 0) ? param : null; } }
        public RequestMap(string pattern)
        {
            this.pattern = pattern;
            param = new List<object>();
        }

        public bool ExecuteMapping(string reqstr)
        {
            //pattern: /{controller:servers}/{param:}/{method:info}
            if (pattern == "") return false;
            reqstr = reqstr.Replace(" ", "");
            reqstr = reqstr.Trim('/');
            var partsreq = reqstr.Split('/');

            pattern = pattern.Replace("/", "");

            string buff = String.Empty;
            int partreqindex = 0;
            string controllername;
            string methodname;

            for (int i = 0; i < pattern.Length; i++)
            {
                if(pattern[i] == '{')
                {
                    buff = String.Empty;
                }
                else if(pattern[i] == '}')
                {
                    var values = buff.Split(':');
                    Regex regexp = new Regex(values[1]);
                    Match match = regexp.Match(partsreq[partreqindex]);
                    if (match.Success)
                    {
                        if(values[0] == "controller")
                        {
                            controllername = partsreq[partreqindex];
                        }
                        else if(values[0] == "method")
                        {
                            methodname = partsreq[partreqindex];
                        }
                        else
                        {
                            param.Add(partsreq[partreqindex]);
                        }
                        partreqindex++;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    buff += pattern[i];
                }
            }
            return true;
        }
    }
    class MapPattern
    {
        
        public MapPattern(string pattern)
        {

        }
    }
}
