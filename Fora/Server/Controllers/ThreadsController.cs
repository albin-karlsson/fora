using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThreadsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<ThreadsController>
        [HttpGet]
        public List<ThreadModel> Get([FromQuery] string token)
        {
            // First: Validate token

            return _context.Threads.ToList();
        }

        // GET api/<ThreadsController>/5
        [HttpGet("{id}")]
        public ThreadModel Get([FromRoute] int id, [FromQuery] string token)
        {
            // First: Validate token

            return _context.Threads.FirstOrDefault(t => t.Id == id);
        }

        // POST api/<ThreadsController>
        [HttpPost]
        public async Task Post([FromBody] ThreadDto thread, [FromQuery] string token)
        {
            // First: Validate token
            // var identityUser = _signInManager.UserManager.FirstOrDefault(u => u.Token == token);
            // var user = _context.users.FirstOrDefault(u => u.Username == identityUser.UserName);
            var user = _context.Users.FirstOrDefault(u => u.Username == "Test"); // Dummy for rows above since no identity

            if (user != null)
            {
                var interest = _context.Interests.FirstOrDefault(i => i.Id == thread.InterestId);

                if (interest != null)
                {
                    var threadToAdd = new ThreadModel()
                    {
                        Name = thread.Name,
                        Interest = interest,
                        User = user,
                    };

                    _context.Threads.Add(threadToAdd);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // PUT api/<ThreadsController>/5
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] ThreadModel updatedThread, [FromQuery] string token)
        {
            // First: Validate token
            var thread = _context.Threads.FirstOrDefault(t => t.Id == id);


        }

        // DELETE api/<ThreadsController>/5
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string token)
        {
            // First: Validate token
        }
    }
}
