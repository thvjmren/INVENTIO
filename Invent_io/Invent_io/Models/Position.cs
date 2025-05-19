using Invent_io.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Invent_io.Models
{
    public class Position:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
