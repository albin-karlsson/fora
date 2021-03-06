using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<MessagesController>
        [HttpGet]
        public List<MessageModel> Get([FromQuery] string token)
        {
            // First: Validate token

            return _context.Messages.ToList();
        }

        // GET api/<MessagesController>/5
        [HttpGet("{id}")]
        public MessageModel Get([FromRoute] int id)
        {
            // First: Validate token

            return _context.Messages.FirstOrDefault(m => m.Id == id);
        }

        // This method gets all messages in a specific thread by passing the thread id as a query string parameter
        // We're constructing the result by projecting the data into the form we want to avoid circular references
        [HttpGet]
        [Route("thread")]
        public List<MessageModel> GetThreadMessages([FromQuery] int id, [FromQuery] string token)
        {
            // First: Validate token

            // Project the user in each message object into a user with the data we want and need (without circular references)
            // If the user would have messages that in turn have users (that we get by default), we would get a circular reference error
            var messages = _context.Messages.Include(m => m.User).Where(m => m.ThreadId == id).Select(m => new MessageModel
            {
                Message = m.Message,
                User = new UserModel()
                {
                    Id = m.User.Id,
                    Username = m.User.Username,
                    Banned = m.User.Banned,
                    Deleted = m.User.Deleted,
                }
            }).ToList();

            return messages;
        }


        // MessageDto is a simplified MessageModel with Message and ThreadId only
        // POST api/<MessagesController>
        [HttpPost]
        public async Task Post([FromBody] MessageDto message, [FromQuery] string token)
        {
            // First: Validate token

            // var identityUser = _signInManager.UserManager.FirstOrDefault(u => u.Token == token);
            // var user = _context.Users.FirstOrDefault(u => u.Username == identityUser.UserName);
            var user = _context.Users.FirstOrDefault(u => u.Username == "Test"); // Dummy for rows above since no identity

            // If a user with the specified token was found
            if (user != null)
            {
                // Get the specific thread in the MessageDto object (sent from client)
                var thread = _context.Threads.FirstOrDefault(t => t.Id == message.ThreadId);

                // If thread was found
                if (thread != null)
                {
                    // Construct the message object
                    var messageToAdd = new MessageModel()
                    {
                        Message = message.Message,
                        User = user,
                        Thread = thread,
                    };

                    // Add message to the db
                    _context.Messages.Add(messageToAdd);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // PUT api/<MessagesController>/5
        [HttpPut("{id}")]
        public async Task Put([FromRoute] int id, [FromBody] MessageModel updatedMessage, [FromQuery] string token)
        {
            // First: Validate token

            // Get the message we want to update from the db
            var messageToUpdate = _context.Messages.FirstOrDefault(m => m.Id == id);

            // If a message was found
            if (messageToUpdate != null)
            {
                // Update the content of that message
                messageToUpdate.Message = updatedMessage.Message;
                // Todo: Set the a message bool property "Edited" to True

                // Save the updated message to the database
                _context.Messages.Update(messageToUpdate);
                await _context.SaveChangesAsync();
            }
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id, [FromQuery] string token)
        {
            // Todo: Set the message bool property "Deleted" to True
        }
    }
}
