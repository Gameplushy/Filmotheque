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
        public JsonResult Create(ActorCreator actor)
        {
            var res = _context.Actors.Add(actor.ToActor());
            _context.SaveChanges();
            return new JsonResult(Ok(res.Entity));
        }

        [HttpGet]
        public JsonResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if(pageSize>20) return new JsonResult(BadRequest("Page size must be lower than 20"));
            List<Person> actorsInPage = _context.Actors.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            if (actorsInPage.Count == 0)
                return new JsonResult(BadRequest("This page is empty."));
            var res = new Dictionary<string, object>();
            if (page != 1) 
                res.Add("previousPage", Url.Link(null, new { page = page - 1, number = pageSize }));
            if(_context.Actors.Count() > page * pageSize)
                res.Add("nextPage", Url.Link(null,new { page = page + 1, number = pageSize }));
            res.Add("actors", actorsInPage);
            return new JsonResult(Ok(res));
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            Person? a = _context.Actors.Find(id);
            if (a == null)
                return new JsonResult(NotFound($"Actor of id {id} not found."));
            return new JsonResult(Ok(a));
        }

        [HttpPatch]
        [Route("{id}")]
        public JsonResult Patch(int id,ActorEditor actor)
        {
            Person? oldActor = _context.Actors.Find(id);
            if (oldActor == null)
                return new JsonResult(NotFound($"Actor of id {id} not found."));
            if(actor.FirstName != null && actor.FirstName != oldActor.FirstName)
                oldActor.FirstName = actor.FirstName;
            if(actor.LastName != null && actor.LastName != oldActor.LastName)
                oldActor.LastName = actor.LastName;
            if(actor.BirthDate != null && actor.BirthDate != oldActor.BirthDate)
                oldActor.BirthDate = (DateTime)actor.BirthDate;
            _context.SaveChanges();
            return new JsonResult(Ok(actor));
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            Person? oldActor = _context.Actors.Find(id);
            if (oldActor == null) 
                return new JsonResult(NotFound($"Actor of id {id} not found."));
            _context.Actors.Remove(oldActor);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
    }
}
