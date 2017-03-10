using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kontur.GameStats.Application.Models
{
    [DataContract]
    class ServersInfoModel
    {
        [DataMember]
        public string endpoint { get; set; }

        [DataMember]
        public ServerInfoModel info { get; set; }
    }
}