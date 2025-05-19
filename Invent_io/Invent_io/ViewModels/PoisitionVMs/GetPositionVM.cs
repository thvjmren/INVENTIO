using Invent_io.Models;
using System.ComponentModel.DataAnnotations;

namespace Invent_io.ViewModels
{
    public class GetPositionVM
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
