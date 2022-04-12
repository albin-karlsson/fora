using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public List<UserModel> Get([FromQuery] string token)
        {
            // First: Validate token

            return _context.Users.ToList();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public UserModel Get([FromRoute] int id, [FromQuery] string token)
        {
            // First: Validate token

            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task Post([FromBody] UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] UserModel updatedUser, [FromQuery] string token)
        {
            // First: Validate token
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string token)
        {
            // First: Validate token
        }
    }
}
