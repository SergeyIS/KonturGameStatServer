using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Models
{
    class MatchModel
    {
        public string Server { get; set; }
        public DateTime Timestamp { get; set; }
        public MatchInfoModel Results { get; set; }
    }
}
