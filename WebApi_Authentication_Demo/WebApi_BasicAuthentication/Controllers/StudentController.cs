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
            return _dbService.GetStudentsAll().Select(x => x.ToDto());
        }

        // GET api/<StudentController>/5
        [HttpGet("{pkid}")]
        public StudentDto Get(int pkId)
        {
            return _dbService.GetStudent(pkId).ToDto();
        }

        // POST api/<StudentController>
        [HttpPost]
        public IActionResult Post([FromBody] StudentDto studentDto)
        {
            ModelState.Remove("PkId");
            try
            {
                StudentDto dto = _dbService.AddStudent(studentDto.ToStudent()).ToDto();
                if (dto != null)
                {
                    return Ok(dto);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        // PUT api/<StudentController>/5
        [HttpPut("{pkid}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{pkid}")]
        public IActionResult Delete(int pkid)
        {
            string error = _dbService.RemoveStudent(pkid);
            if (error == null)
            {
                return Ok();
            }
            else
            {
                return BadRequest(error);
            }
        }
    }
}
