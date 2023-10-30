using Microsoft.AspNetCore.Mvc;
using Filmotheque.Models;
using Filmotheque.Data;
using Filmotheque.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Filmotheque.Models.Responses;

namespace Filmotheque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly APIContext _context;

        public MovieController(APIContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieResponse))]
        public IActionResult Create(MovieCreator movie)
        {
            var checker = CheckActorsAndDirectorsIds(movie.Actors, movie.Directors);
            if(checker.errorMessage != null) 
                return BadRequest(checker.errorMessage);
            if (!DateTime.TryParse(movie.ReleaseDate, out _))
                return BadRequest("Release date given is not a valid date.");
            Movie res = new Movie()
            {
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Actors = checker.actors!,
                Directors = checker.directors!           
            };
            var newId = _context.Movies.Add(res);
            _context.SaveChanges();
            return Created(Url.Action("Get",new {id = newId.Entity.Id})!,GivePersonLinksToMovie(newId.Entity));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MovieResponse>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(void))]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] List<int>? actors = null, [FromQuery] List<int>? directors = null)
        {
            if (pageSize > 20) return BadRequest("Page size must be lower than 20");
            IEnumerable<Movie> movieList = _context.Movies.Include(m => m.Actors).Include(m => m.Directors);
            var checker = CheckActorsAndDirectorsIds(actors, directors, true);
            if(checker.errorMessage != null) 
                return BadRequest(checker.errorMessage);
            if (checker.actors != null) movieList = movieList.Where(m => checker.actors.All(a => m.Actors.Contains(a)));
            if (checker.directors != null) movieList = movieList.Where(m => checker.directors.All(d => m.Directors.Contains(d)));
            List<Movie> moviesInPage = movieList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            if (moviesInPage.Count == 0)
                return NoContent();
            var res = new Dictionary<string, object>();
            if (page != 1)
                res.Add("previousPage", Url.Action(null, new { page = page - 1, number = pageSize })!);
            if (_context.Actors.Count() > page * pageSize)
                res.Add("nextPage", Url.Action(null, new { page = page + 1, number = pageSize })!);
            List<MovieResponse> formattedRes = moviesInPage.Select(GivePersonLinksToMovie).ToList();
            res.Add("movies", formattedRes);
            return Ok(res);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(MovieResponse))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult Get(int id)
        {
            Movie? a = _context.Movies.Include(m=>m.Actors).Include(m=>m.Directors).SingleOrDefault(m=>m.Id==id);
            if (a == null)
                return NotFound($"Movie of id {id} not found.");
            return Ok(GivePersonLinksToMovie(a));
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(MovieResponse))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Patch(int id, MovieEditor movie)
        {
            Movie? oldMovie = _context.Movies.Include(m=>m.Actors).Include(m=>m.Directors).SingleOrDefault(m=>m.Id==id);
            if (oldMovie == null)
                return NotFound($"Movie of id {id} not found.");
            var checker = CheckActorsAndDirectorsIds(movie.Actors, movie.Directors,true);
            if(checker.errorMessage != null) 
                return BadRequest(checker.errorMessage);
            if (movie.ReleaseDate != null && !DateTime.TryParse(movie.ReleaseDate, out _))
                return BadRequest("Release date given is not a valid date.");

            if (movie.Title != null && movie.Title != oldMovie.Title)
                oldMovie.Title = movie.Title;
            if (movie.Description != null && movie.Description != oldMovie.Description)
                oldMovie.Description = movie.Description;
            if (movie.ReleaseDate != null && movie.ReleaseDate != oldMovie.ReleaseDate)
                oldMovie.ReleaseDate = movie.ReleaseDate;
            if (checker.actors != null && !movie.Actors!.SequenceEqual(oldMovie.Actors.Select(a => a.Id)))
                oldMovie.Actors = checker.actors;
            if (checker.directors != null && !movie.Directors!.SequenceEqual(oldMovie.Directors.Select(d => d.Id)))
                oldMovie.Directors = checker.directors;
            _context.SaveChanges();
            return Ok(GivePersonLinksToMovie(oldMovie));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            Movie? oldMovie = _context.Movies.Find(id);
            if (oldMovie == null)
                return NotFound($"Movie of id {id} not found.");
            _context.Movies.Remove(oldMovie);
            _context.SaveChanges();
            return Ok();
        }

        private (List<Actor>? actors, List<Director>? directors, string? errorMessage) CheckActorsAndDirectorsIds(List<int>? actorIds,  List<int>? directorIds, bool editingMode = false)
        {
            List<Actor>? actors = null; List<Director>? directors = null;
            if(!(editingMode && actorIds == null))
            {
                var res = CheckActorIds(actorIds!);
                if(res.errorMessage != null)
                    return (null,null,res.errorMessage);
                actors = res.actors;
            }
            if(!(editingMode && directorIds == null))
            {
                var res = CheckDirectorIds(directorIds!);
                if (res.errorMessage != null)
                    return (null, null, res.errorMessage);
                directors = res.directors;
            }
            return (actors, directors, null);
        }

        private (List<Actor>? actors, string? errorMessage) CheckActorIds(List<int> actorIds)
        {
            if (actorIds.Distinct().Count() != actorIds.Count)
                return (null, "All actor ids must be unique.");
            List<Actor?> actors;
            actors = actorIds.Select(a => _context.Actors.SingleOrDefault(ra => ra.Id == a)).ToList();
            if (actors.Any(a => a == null))
                return (null, "Invalid actor id detected.");
            return (actors, null);
        }

        private (List<Director>? directors, string? errorMessage) CheckDirectorIds(List<int> directorIds)
        {
            if (directorIds.Distinct().Count() != directorIds.Count)
                return (null, "All director ids must be unique.");
            List<Director?> directors;
            directors = directorIds.Select(d => _context.Directors.SingleOrDefault(rd => rd.Id == d)).ToList();
            if (directors.Any(d => d == null))
                return (null, "Invalid director id detected.");
            return (directors, null);
        }

        private MovieResponse GivePersonLinksToMovie(Movie mov)
        {
            return new MovieResponse()
            {
                Id = mov.Id,
                Description = mov.Description,
                ReleaseDate = mov.ReleaseDate,
                Title = mov.Title,
                Actors = mov.Actors.Select(ac => new ActorWithLink() { Link = Url.Action("Get", "Actor", new { Id = ac.Id })!, Actor = ac }).ToList(),
                Directors = mov.Directors.Select(dir => new DirectorWithLink() { Link = Url.Action("Get", "Director", new { Id = dir.Id })!, Director = dir }).ToList(),
            };
        }
    }
}
