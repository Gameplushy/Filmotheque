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
            if (movie.Actors.Distinct().Count() != movie.Actors.Count)
                return new JsonResult(BadRequest("All actor ids must be unique."));
            if (movie.Actors.Distinct().Count() != movie.Actors.Count)
                return new JsonResult(BadRequest("All actor ids must be unique."));
            List<Actor?> actors; List<Director?> directors;
            actors = movie.Actors.Select(a => _context.Actors.SingleOrDefault(ra => ra.Id == a)).ToList();//_context.Actors.Where(a => movie.Actors.Contains(a.Id)).ToList();
            if (actors.Any(a => a == null))
                return new JsonResult(BadRequest("Invalid actor id detected."));
            directors = movie.Directors.Select(d => _context.Directors.SingleOrDefault(rd => rd.Id == d)).ToList();//_context.Actors.Where(a => movie.Actors.Contains(a.Id)).ToList();
            if (directors.Any(d => d == null))
                return new JsonResult(BadRequest("Invalid director id detected."));
            if (!DateTime.TryParse(movie.ReleaseDate, out _))
                return new JsonResult(BadRequest("Release date given is not a valid date."));
            //var res = _context.Movies.Add(movie.ToMovie());
            Movie res = new Movie()
            {
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Actors = actors!,
                Directors = directors!           
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
    }
}
