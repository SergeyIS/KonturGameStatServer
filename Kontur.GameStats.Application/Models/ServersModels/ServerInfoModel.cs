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
        [DataMember(Name = "endpoint")]
        public string Endpoint { get; set; }

        [DataMember(Name = "info")]
        public ServerInfoModel Info { get; set; }
    }
}