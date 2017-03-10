using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Application.Core;

namespace Kontur.GameStats.Application.Controllers
{
    [Name("players")]
    class PlayersController
    {
        [Method(HttpMethods.GET)][Name("stats")]
        public string Stats(string name)
        {
            return "public string Stats(string endpoint)";
        }
    }
}
