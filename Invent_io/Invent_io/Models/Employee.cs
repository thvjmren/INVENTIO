using Invent_io.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Invent_io.Models
{
    public class Employee:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string X { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public int PositionId { get; set; }
        public Position? Position { get; set; }
    }
}
