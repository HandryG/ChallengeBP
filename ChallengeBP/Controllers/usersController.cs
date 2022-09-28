using ChallengeBP.DataAccess;
using ChallengeBP.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly challengeContext _context;
        private readonly IUserRepository _userRepository;

        public usersController(challengeContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id) {

            return Ok(_userRepository.getUserById(id));
        }

        [HttpGet("{id}/summary")]
        public IActionResult GetResumen(int id)
        {
            return Ok(_userRepository.getResumenById(id));
        }

        [HttpGet("{id}/summary/{dd-mm-yyyy}")]
        public IActionResult GetResumenDate(int id, DateTime date)
        {
            return Ok(_userRepository.getResumenByIdAndDate(id, date));
        }

        [HttpGet("{id}/goals")]
        public IActionResult GetGoals(int id)
        {
            return Ok(_userRepository.getMetas(id));
        }

        [HttpGet("{id}/goals/{goalid}")]
        public IActionResult GetGoals(int id, int goalid)
        {
            return Ok(_userRepository.getMetaDetail(id, goalid));
        }

    }
}
