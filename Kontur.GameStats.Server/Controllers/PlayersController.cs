using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.ControllersCore.Types;
namespace Kontur.GameStats.Controllers
{
    class PlayersController
    {
        [HttpGet]
        public string Stats(string endpoint)
        {
            return "public string Stats(string endpoint)";
        }
    }
}
