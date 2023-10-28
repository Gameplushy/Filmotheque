using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Filmotheque.Models.Requests
{
    public class MovieCreator
    {
        [MaxLength(128)]
        public required string Title { get; set; }
        [MaxLength(2048)]
        public required string Description { get; set; }
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$")]
        [DefaultValue("2023-10-22")]
        public required string ReleaseDate { get; set; }
        public required List<int> Actors { get; set; }
        public required List<int> Directors { get; set; }
    }
}
