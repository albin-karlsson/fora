using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInterestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserInterestsController(AppDbContext context)
        {
            _context = context;
        }

        // Get all UserInterests (interests a user has) for a specific user (by the user's id)
        // GET api/<UserInterestsController>/5
        [HttpGet("{id}")]
        public List<InterestModel> Get(int id, [FromQuery] string token)
        {
            // First: Validate token

            return _context.Interests.Where(i => i.UserInterests.Any(ui => ui.UserId == id)).ToList();
        }

        // Add UserInterest (interests a user has) to user (by the user's id)
        // PUT api/<UserInterestsController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] InterestModel interest, [FromQuery] string token)
        {
            // First: Validate token

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            // Get the full interest from the db, in case we sent an incomplete one that could cause an error
            var interestToAdd = _context.Interests.FirstOrDefault(i => i.Id == interest.Id);

            if (user != null && interestToAdd != null)
            {
                _context.UserInterests.Add(new UserInterestModel()
                {
                    Interest = interestToAdd,
                    User = user
                });

                await _context.SaveChangesAsync();
            }
        }

        // Remove UserInterest (interests a user has) from user
        // id is interest id
        // DELETE api/<UserInterestsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, [FromQuery] string token)
        {
            // First: Validate token

            // Get user
            // var identityUser = _signInManager.UserManager.FirstOrDefault(u => u.Token == token);
            // var user = _context.Users.FirstOrDefault(u => u.Username == identityUser.UserName);
            var user = _context.Users.FirstOrDefault(u => u.Username == "Test"); // Dummy for rows above since no identity

            // If a user was found
            if (user != null)
            {
                // Get UserInterest to remove
                var userInterestToRemove = _context.UserInterests
                    .FirstOrDefault(ui => ui.InterestId == id && ui.UserId == user.Id);

                // If a UserInterest was found
                if (userInterestToRemove != null)
                {
                    _context.UserInterests.Remove(userInterestToRemove);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
