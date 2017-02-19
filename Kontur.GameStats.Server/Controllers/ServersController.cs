using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.ControllersCore.Types;

namespace Kontur.GameStats.Controllers
{
    class ServersController
    {
        [HttpGet]
        public string Info()
        {
            return "public string Info()";
        }
        [HttpPut]
        public string Info(string endpoin, string data)
        {
            return "public string Info(string endpoin, string data)";
        }
        [HttpGet]
        public string Matches(string endpoin, string timestamp)
        {
            return "public string Matches(string endpoin, DateTime timestamp)";
        }
        [HttpPut]
        public string Matches(string endpoin, string timestamp, string data)
        {
            return "public string Matches(string endpoin, DateTime timestamp, string data)";
        }
        [HttpGet]
        public string Stats(string endpoint)
        {
            return "public string Stats(string endpoint)";
        }

    }
}

