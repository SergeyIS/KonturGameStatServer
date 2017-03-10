namespace Kontur.GameStats.Application.App_Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Matches
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Matches()
        {
            MatchesPlayers = new HashSet<MatchesPlayers>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int matchId { get; set; }

        public int serverId { get; set; }

        public DateTime timestamp { get; set; }

        public int? fraglimit { get; set; }

        public int? timelimit { get; set; }

        public int? timeelapsed { get; set; }

        [Required]
        [StringLength(10)]
        public string gamemode { get; set; }

        [Required]
        [StringLength(50)]
        public string map { get; set; }

        public virtual Servers Servers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MatchesPlayers> MatchesPlayers { get; set; }
    }
}
