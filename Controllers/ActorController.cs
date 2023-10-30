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
        [ProducesResponseType(201, Type = typeof(Actor))]
        public IActionResult Create(ActorCreator actor)
        {
            var res = _context.Actors.Add(actor.ToActor());
            _context.SaveChanges();
            return Created(Url.Action("Get",new { id = res.Entity.Id }),res.Entity);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Actor>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(void))]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if(pageSize>20) return BadRequest("Page size must be lower than 20");
            List<Actor> actorsInPage = _context.Actors.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            if (actorsInPage.Count == 0)
                return NoContent();
            var res = new Dictionary<string, object>();
            if (page != 1) 
                res.Add("previousPage", Url.Link(null, new { page = page - 1, number = pageSize }));
            if(_context.Actors.Count() > page * pageSize)
                res.Add("nextPage", Url.Link(null,new { page = page + 1, number = pageSize }));
            res.Add("actors", actorsInPage);
            return Ok(res);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Actor))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult Get(int id)
        {
            Actor? a = _context.Actors.Find(id);
            if (a == null)
                return NotFound($"Actor of id {id} not found.");
            return Ok(a);
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Actor))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult Patch(int id,ActorEditor actor)
        {
            Actor? oldActor = _context.Actors.Find(id);
            if (oldActor == null)
                return NotFound($"Actor of id {id} not found.");
            if(actor.FirstName != null && actor.FirstName != oldActor.FirstName)
                oldActor.FirstName = actor.FirstName;
            if(actor.LastName != null && actor.LastName != oldActor.LastName)
                oldActor.LastName = actor.LastName;
            if(actor.BirthDate != null && actor.BirthDate != oldActor.BirthDate)
                oldActor.BirthDate = (DateTime)actor.BirthDate;
            _context.SaveChanges();
            return Ok(actor);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            Actor? oldActor = _context.Actors.Find(id);
            if (oldActor == null) 
                return NotFound($"Actor of id {id} not found.");
            _context.Actors.Remove(oldActor);
            _context.SaveChanges();
            return Ok();
        }
    }
}
