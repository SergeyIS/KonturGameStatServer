using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.ControllersCore.Types;

namespace Kontur.GameStats.Controllers
{
    class ReportsController
    {
       [HttpGet]
        public string RecentMatches(string count)
        {
            return "public string RecentMatches(int count)";
        }

        [HttpGet]
        public string BestPlayers(string count)
        {
            return "public string BestPlayers(int count)";
        }

        [HttpGet]
        public string PopularServers(string count)
        {
            return "public string PopularServers(int count)";
        }
    }
}
