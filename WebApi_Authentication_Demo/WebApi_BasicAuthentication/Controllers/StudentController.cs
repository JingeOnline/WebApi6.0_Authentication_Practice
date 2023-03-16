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
        public IActionResult Get()
        {
            try
            {
                IEnumerable<StudentWithIdDto> studentDtos=_dbService.GetStudentsAll().Select(x => x.ToDtoWithId());
                return Ok(studentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        // GET api/<StudentController>/5
        //这里的Name为该Action命名，方便之后在其他Action中执行跳转。
        [HttpGet("{pkid}",Name ="GetStudentById")]
        public IActionResult Get(int pkid)
        {
            try
            {
                Student student = _dbService.GetStudent(pkid);
                if (student is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(student.ToDtoWithId());
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<StudentController>
        [HttpPost]
        public IActionResult Post([FromBody] StudentDto studentDto)
        {
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
                return StatusCode(500, "Internal server error");
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
            try
            {
                Student student = _dbService.GetStudent(pkid);
                if (student is null)
                {
                    return NotFound();//404
                }
                else
                {
                    _dbService.RemoveStudent(student);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
