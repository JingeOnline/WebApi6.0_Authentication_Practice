using DbServiceLib;
using DbServiceLib.ModelDtos;
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
        public IEnumerable<StudentDto> Get()
        {
            return _dbService.GetStudentsAll().Select(x=>x.ToDto());
        }

        // GET api/<StudentController>/5
        [HttpGet("{pkid}")]
        public StudentDto Get(int pkId)
        {
            return _dbService.GetStudent(pkId).ToDto();
        }

        // POST api/<StudentController>
        [HttpPost]
        public StudentDto Post([FromBody] StudentDto studentDto)
        {
            return _dbService.AddStudent(studentDto.ToStudent()).ToDto();
        }

        // PUT api/<StudentController>/5
        [HttpPut("{pkid}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{pkid}")]
        public void Delete(int pkid)
        {
            _dbService.RemoveStudent(pkid);
        }
    }
}
