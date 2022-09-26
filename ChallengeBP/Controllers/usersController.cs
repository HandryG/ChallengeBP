using ChallengeBP.DataAccess;
using ChallengeBP.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBP.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> list(){

            return Ok(await _userRepository.getUsers());
        }

        [HttpGet("[action]")]
        public IActionResult Get(int id) {

            return Ok(_userRepository.getUserById(id));
        }
    }
}
