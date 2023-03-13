using DbServiceLib;
using DbServiceLib.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_BasicAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDbService _dbService;
        public StudentController(IDbService dbService)
        {
            _dbService = dbService;
        }


        // GET: api/<StudentController>
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _dbService.GetStudentsAll();
        }

        // GET api/<StudentController>/5
        [HttpGet("{pkid}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudentController>
        [HttpPost]
        public Student Post([FromBody] Student student)
        {
            return _dbService.AddStudent(student);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{pkid}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{pkid}")]
        public void Delete(int id)
        {
        }
    }
}
