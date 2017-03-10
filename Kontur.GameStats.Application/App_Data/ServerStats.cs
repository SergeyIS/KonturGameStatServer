namespace Kontur.GameStats.Application.App_Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServerStats
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServerStats()
        {
            ServerStatGameMaps = new HashSet<ServerStatGameMaps>();
            ServerStatGameModes = new HashSet<ServerStatGameModes>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int serverId { get; set; }

        public int? totalMatchesPlayed { get; set; }

        public int? maximumMatchesPerDay { get; set; }

        public float? averageMatchesPerDay { get; set; }

        public int? maximumPopulation { get; set; }

        public float? averagePopulation { get; set; }

        public virtual Servers Servers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServerStatGameMaps> ServerStatGameMaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServerStatGameModes> ServerStatGameModes { get; set; }
    }
}
