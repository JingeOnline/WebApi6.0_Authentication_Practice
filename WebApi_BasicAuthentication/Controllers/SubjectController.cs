using DbServiceLib;
using DbServiceLib.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_BasicAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IDbService _dbService;

        public SubjectController(IDbService dbService) { 
            _dbService= dbService;
        }
        // GET: api/<SubjectController>
        [HttpGet]
        public IEnumerable<Subject> Get()
        {
            return _dbService.GetSubjectsAll();
        }

        // GET api/<SubjectController>/5
        [HttpGet("{id}")]
        public Subject Get(int id)
        {
            return _dbService.GetSubject(id);
        }

        // POST api/<SubjectController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
