using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kontur.GameStats.Server.Models
{
    class ServerInfoModel
    {
        public string Name { get; set; }
        public List<string> GameModes { get; set; }
    }
}