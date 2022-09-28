using ChallengeBP.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ChallengeBP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public usersController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {

            return Ok(await _userApplication.GetUserById(id));
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetResumen(int id)
        {
            return Ok(await _userApplication.GetResumenById(id));
        }

        [HttpGet("{id}/summary/{date}")]
        public async Task<IActionResult> GetResumenDate(int id, string date)
        {

            if (Regex.IsMatch(date, "\\d{2}-\\d{2}-\\d{4}"))
            {
                DateOnly dateonly = DateOnly.Parse(date);
                return Ok(await _userApplication.GetResumenByIdAndDate(id, dateonly));
            }
            else
                return Ok("Por favor ingresar formato 'dd-mm-yyyy' para la fecha");
            
        }

        [HttpGet("{id}/goals")]
        public async Task<IActionResult> GetGoals(int id)
        {
            return Ok(await _userApplication.GetMetasById(id));
        }

        [HttpGet("{id}/goals/{goalid}")]
        public async Task<IActionResult> GetGoals(int id, int goalid)
        {
            return Ok(await _userApplication.GetMetaDetail(id, goalid));
        }

    }
}
