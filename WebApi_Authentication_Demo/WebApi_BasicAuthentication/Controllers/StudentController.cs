using DbServiceLib;
using DbServiceLib.ModelDtos;
using DbServiceLib.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
                if (studentDto is null)
                {
                    return BadRequest("StudentDto object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                Student student = _dbService.AddStudent(studentDto.ToStudent());
                //三个参数：
                //第一个参数指定路由的名称。（在每个Action上通过Name属性来设置）
                //第二个参数指定路由中要传入的参数。(参数名大小写不敏感)
                //指定好之后，会在Response Headers中创建一条location字段，【location: http://localhost:5000/api/Student/6 】
                //这样用户就能从header中获取刚刚创建的新对象的URL，方便他之后访问该对象。
                //第三个参数才是指定当前Response body中返回的对象。
                return CreatedAtRoute("GetStudentById", new { pkid = student.PkId }, student.ToDtoWithId());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        // PUT api/<StudentController>/5
        [HttpPut("{pkid}")]
        public IActionResult Put(int pkid, [FromBody] StudentDto studentDto)
        {
            try
            {
                if (studentDto is null)
                {
                    return BadRequest("Student object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }
                Student student = _dbService.GetStudent(pkid);
                if (student is null)
                {
                    return NotFound(); //404
                }
                _dbService.UpdateStudent(studentDto.ToStudent(pkid));
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
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
