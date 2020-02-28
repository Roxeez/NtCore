using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Moonlight.Core.Enums;
using Moonlight.Database.DAL;

namespace Moonlight.Database.Entities
{
    [Table("items")]
    internal class Item : IEntity<int>
    {
        [Required]
        public string NameKey { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int SubType { get; set; }

        [Required]
        public BagType BagType { get; set; }

        [Required]
        public string Data { get; set; }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
    }
}