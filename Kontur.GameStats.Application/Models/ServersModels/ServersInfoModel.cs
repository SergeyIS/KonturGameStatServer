using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kontur.GameStats.Application.Models
{
    [DataContract]
    class ServerInfoModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "gameModes")]
        public List<string> GameModes { get; set; }
    }
}