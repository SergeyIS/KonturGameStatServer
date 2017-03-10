namespace Kontur.GameStats.Application.App_Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlayerStat")]
    public partial class PlayerStat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlayerStat()
        {
            PlayerStatGameMaps = new HashSet<PlayerStatGameMaps>();
            PlayerStatGameModes = new HashSet<PlayerStatGameModes>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int playerId { get; set; }

        public int? totalMatchesPlayed { get; set; }

        public int? totalMatchesWon { get; set; }

        public float? averageScoreboardPercent { get; set; }

        public int? maximumMatchesPerDay { get; set; }

        public float? averageMatchesPerDay { get; set; }

        public DateTime? lastMatchPlayed { get; set; }

        public float? killToDeathRatio { get; set; }

        public virtual Players Players { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerStatGameMaps> PlayerStatGameMaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerStatGameModes> PlayerStatGameModes { get; set; }
    }
}
