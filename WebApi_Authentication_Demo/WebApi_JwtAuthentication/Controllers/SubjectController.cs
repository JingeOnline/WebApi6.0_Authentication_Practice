using DbServiceLib.ModelDtos;
using DbServiceLib.Models;
using DbServiceLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi_JwtAuthentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IDbService _dbService;

        public SubjectController(IDbService dbService)
        {
            _dbService = dbService;
        }
        // GET: api/<SubjectController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<SubjectWithIdDto> subjectDtos = _dbService.GetSubjectsAll().Select(x => x.ToDtoWithId());
                return Ok(subjectDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/<SubjectController>/5
        //这里的Name为该路由命名，方便之后在其他Action中调用。
        [HttpGet("{pkid}", Name = "GetSubjectById")]
        public IActionResult Get(int pkid)
        {
            try
            {
                Subject subject = _dbService.GetSubject(pkid);
                if (subject is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(subject.ToDtoWithId());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<SubjectController>
        [HttpPost]
        public IActionResult Post([FromBody] SubjectDto subjectDto)
        {

            try
            {
                if (subjectDto is null)
                {
                    return BadRequest("SubjectDto object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                Subject subject = _dbService.AddSubject(subjectDto.ToSubject());

                //三个参数：
                //第一个参数指定路由的名称。（在每个Action上通过Name属性来设置）
                //第二个参数指定路由中要传入的参数。(参数名大小写不敏感)
                //指定好之后，会在Response Headers中创建一条location字段，【location: http://localhost:5000/api/Subject/6 】
                //这样用户就能从header中获取刚刚创建的新对象的URL，方便他之后访问该对象。
                //第三个参数才是指定当前Response body中返回的对象。
                return CreatedAtRoute("GetSubjectById", new { PKID = subject.PkId }, subject.ToDtoWithId()); //Code:201
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int pkid, [FromBody] SubjectDto subjectDto)
        {
            try
            {
                if (subjectDto is null)
                {
                    return BadRequest("Subject object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }
                Subject subject = _dbService.GetSubject(pkid);
                if (subject is null)
                {
                    return NotFound();
                }
                _dbService.UpdateSubject(subjectDto.ToSubject(pkid));
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{pkid}")]
        public IActionResult Delete(int pkid)
        {
            try
            {
                Subject subject = _dbService.GetSubject(pkid);
                if (subject is null)
                {
                    return NotFound(); //Code:404
                }
                else
                {
                    _dbService.RemoveSubject(subject);
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
