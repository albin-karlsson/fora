using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InterestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<InterestsController>
        [HttpGet]
        public List<InterestModel> Get()
        {
            return _context.Interests.ToList();
        }

        // GET api/<InterestsController>/5
        [HttpGet("{id}")]
        public InterestModel Get([FromRoute] int id)
        {
            return _context.Interests.FirstOrDefault(i => i.Id == id);
        }

        // POST api/<InterestsController>
        [HttpPost]
        public async Task Post([FromBody] InterestModel interest, [FromQuery] string token)
        {
            // First: Validate token
            // var identityUser = _signInManager.UserManager.FirstOrDefault(u => u.Token == token);
            // var user = _context.Users.FirstOrDefault(u => u.Username == identityUser.UserName);
            var user = _context.Users.FirstOrDefault(u => u.Username == "Test"); // Dummy for rows above since no identity

            if (user != null)
            {
                interest.User = user;

                _context.Interests.Add(interest);
                await _context.SaveChangesAsync();
            }
        }

        // PUT api/<InterestsController>/5
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] InterestModel updatedInterest, [FromQuery] string token)
        {
            // First: Validate token
        }

        // DELETE api/<InterestsController>/5
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string token)
        {
            // First: Validate token
        }
    }
}
