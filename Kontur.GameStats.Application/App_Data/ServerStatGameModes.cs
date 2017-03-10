namespace Kontur.GameStats.Application.App_Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServerStatGameModes
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int serverId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string gamemode { get; set; }

        public float frecuency { get; set; }

        public virtual ServerStats ServerStats { get; set; }
    }
}
