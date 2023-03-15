using DbServiceLib;
using DbServiceLib.ModelDtos;
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
                IEnumerable<SubjectDto> subjectDtos = _dbService.GetSubjectsAll().Select(x => x.ToDto());
                return Ok(subjectDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/<SubjectController>/5
        [HttpGet("{pkid}")]
        public SubjectDto Get(int pkid)
        {
            return _dbService.GetSubject(pkid).ToDto();
        }

        // POST api/<SubjectController>
        [HttpPost]
        public SubjectDto Post([FromBody] SubjectDto subjectDto)
        {
            return _dbService.AddSubject(subjectDto.ToSubject()).ToDto();
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{pkid}")]
        public IActionResult Delete(int pkid)
        {
            string error = _dbService.RemoveSubject(pkid);
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
