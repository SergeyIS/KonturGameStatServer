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
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public List<string> gameModes { get; set; }
    }
}