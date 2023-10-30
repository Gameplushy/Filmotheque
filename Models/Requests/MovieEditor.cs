using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Filmotheque.Models.Requests
{
    public class MovieEditor
    {
        [MaxLength(128)]
        public string? Title { get; set; }
        [MaxLength(2048)]
        public string? Description { get; set; }
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$")]
        [DefaultValue("2023-10-22")]
        public string? ReleaseDate { get; set; }
        public List<int>? Actors { get; set; }
        public List<int>? Directors { get; set; }
    }
}
