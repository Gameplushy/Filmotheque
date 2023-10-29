using Microsoft.AspNetCore.Mvc;
using Filmotheque.Models;
using Filmotheque.Data;
using Filmotheque.Models.Requests;

namespace Filmotheque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly APIContext _context;

        public DirectorController(APIContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public JsonResult Create(DirectorCreator director)
        {
            var res = _context.Directors.Add(director.ToDirector());
            _context.SaveChanges();
            return new JsonResult(Ok(res.Entity));
        }

        [HttpGet]
        public JsonResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (pageSize > 20) return new JsonResult(BadRequest("Page size must be lower than 20"));
            List<Director> directorsInPage = _context.Directors.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            if(directorsInPage.Count == 0)
                return new JsonResult(BadRequest("This page is empty."));
            var res = new Dictionary<string, object>();
            if (page != 1) 
                res.Add("previousPage", Url.Link(null, new { page = page - 1, number = pageSize }));
            if(_context.Directors.Count() > page * pageSize)
                res.Add("nextPage", Url.Link(null,new { page = page + 1, number = pageSize }));
            res.Add("directors", directorsInPage);
            return new JsonResult(Ok(res));
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            Director? a = _context.Directors.Find(id);
            if (a == null)
                return new JsonResult(NotFound($"Director of id {id} not found."));
            return new JsonResult(Ok(a));
        }

        [HttpPatch]
        [Route("{id}")]
        public JsonResult Patch(int id, DirectorEditor director)
        {
            Director? oldDirector = _context.Directors.Find(id);
            if (oldDirector == null)
                return new JsonResult(NotFound($"Director of id {id} not found."));
            if(director.FirstName != null && director.FirstName != oldDirector.FirstName)
                oldDirector.FirstName = director.FirstName;
            if(director.LastName != null && director.LastName != oldDirector.LastName)
                oldDirector.LastName = director.LastName;
            if(director.BirthDate != null && director.BirthDate != oldDirector.BirthDate)
                oldDirector.BirthDate = (DateTime)director.BirthDate;
            _context.SaveChanges();
            return new JsonResult(Ok(director));
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            Director? oldDirector = _context.Directors.Find(id);
            if (oldDirector == null) 
                return new JsonResult(NotFound($"Director of id {id} not found."));
            _context.Directors.Remove(oldDirector);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
    }
}
