using Microsoft.AspNetCore.Mvc;
using Filmotheque.Models;
using Filmotheque.Data;
using Filmotheque.Models.Requests;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Create(MovieCreator movie)
        {
            var checker = CheckActorsAndDirectorsIds(movie.Actors, movie.Directors);
            if(checker.errorMessage != null) 
                return new JsonResult(BadRequest(checker.errorMessage));
            if (!DateTime.TryParse(movie.ReleaseDate, out _))
                return new JsonResult(BadRequest("Release date given is not a valid date."));
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
            return new JsonResult(Ok(newId.Entity));
        }

        [HttpGet]
        public JsonResult GetAll([FromQuery] int page = 1, [FromQuery] int number = 20)
        {
            if (number > 20) return new JsonResult(BadRequest("number must be lower than 20"));
            List<Movie> moviesInPage = _context.Movies.Include(m => m.Actors).Include(m=>m.Directors).Skip((page - 1) * number).Take(number).ToList();
            var res = new Dictionary<string, object>();
            if (page != 1)
                res.Add("previousPage", Url.Link(null, new { page = page - 1, number = number }));
            if (_context.Actors.Count() > page * number)
                res.Add("nextPage", Url.Link(null, new { page = page + 1, number = number }));
            res.Add("movies", moviesInPage);
            return new JsonResult(Ok(res));
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            Movie? a = _context.Movies.Include(m=>m.Actors).Include(m=>m.Directors).SingleOrDefault(m=>m.Id==id);
            if (a == null)
                return new JsonResult(NotFound($"Movie of id {id} not found."));
            return new JsonResult(Ok(a));
        }

        [HttpPatch]
        [Route("{id}")]
        public JsonResult Patch(int id, MovieEditor movie)
        {
            Movie? oldMovie = _context.Movies.Include(m=>m.Actors).Include(m=>m.Directors).SingleOrDefault(m=>m.Id==id);
            if (oldMovie == null)
                return new JsonResult(NotFound($"Movie of id {id} not found."));
            var checker = CheckActorsAndDirectorsIds(movie.Actors, movie.Directors,true);
            if(checker.errorMessage != null) 
                return new JsonResult(BadRequest(checker.errorMessage));
            if (movie.ReleaseDate != null && !DateTime.TryParse(movie.ReleaseDate, out _))
                return new JsonResult(BadRequest("Release date given is not a valid date."));

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
            return new JsonResult(Ok(oldMovie));
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            Movie? oldMovie = _context.Movies.Find(id);
            if (oldMovie == null)
                return new JsonResult(NotFound($"Movie of id {id} not found."));
            _context.Movies.Remove(oldMovie);
            _context.SaveChanges();
            return new JsonResult(Ok());
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


    }
}
