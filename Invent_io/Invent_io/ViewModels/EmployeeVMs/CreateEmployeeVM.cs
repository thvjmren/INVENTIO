using Invent_io.Models;

namespace Invent_io.ViewModels
{
    public class CreateEmployeeVM
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string Description { get; set; }
        public string X { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
