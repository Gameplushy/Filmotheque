using Microsoft.AspNetCore.Mvc;
using Filmotheque.Models;
using Filmotheque.Data;
using Filmotheque.Models.Requests;

namespace Filmotheque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly APIContext _context;

        public ActorController(APIContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public JsonResult Create(ActorRequest actor)
        {
            var res = _context.Actors.Add(actor.ToActor());
            _context.SaveChanges();
            return new JsonResult(Ok(res.Entity));
        }

        [HttpGet]
        public JsonResult GetAll([FromQuery] int page = 1, [FromQuery] int number = 20)
        {
            List<Actor> actorsInPage = _context.Actors.Skip((page - 1) * number).Take(number).ToList();
            var res = new Dictionary<string, object>();
            if (page != 1) 
                res.Add("previousPage", Url.Link(null, new { page = page - 1, number = number }));
            if(_context.Actors.Count() > page * number)
                res.Add("nextPage", Url.Link(null,new { page = page + 1, number = number }));
            res.Add("actors", actorsInPage);
            return new JsonResult(Ok(res));
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            Actor? a = _context.Actors.Find(id);
            if (a == null)
                return new JsonResult(NotFound($"Actor of id {id} not found."));
            return new JsonResult(Ok(a));
        }

        [HttpPatch]
        [Route("{id}")]
        public JsonResult Patch(int id,ActorRequest actor)
        {
            Actor? oldActor = _context.Actors.Find(id);
            if (oldActor == null)
                return new JsonResult(NotFound($"Actor of id {id} not found."));
            if(actor.FirstName != oldActor.FirstName)
                oldActor.FirstName = actor.FirstName;
            if(actor.LastName != oldActor.LastName)
                oldActor.LastName = actor.LastName;
            if(actor.BirthDate != oldActor.BirthDate)
                oldActor.BirthDate = actor.BirthDate;
            _context.SaveChanges();
            return new JsonResult(Ok(actor));
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            Actor? oldActor = _context.Actors.Find(id);
            if (oldActor == null) 
                return new JsonResult(NotFound($"Actor of id {id} not found."));
            _context.Actors.Remove(oldActor);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
    }
}
