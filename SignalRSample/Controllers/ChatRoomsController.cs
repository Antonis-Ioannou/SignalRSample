using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;
using SignalRSample.Models;

namespace SignalRSample.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatRooms
        [HttpGet]
        [Route("/[controller]/GetChatRoom")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetChatRoom()
        {
          if (_context.ChatRooms == null)
          {
              return NotFound();
          }
            return await _context.ChatRooms.ToListAsync();
        }

        [HttpGet]
        [Route("/[controller]/GetChatUser")]
        public async Task<ActionResult<IEnumerable<Object>>> GetChatUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var users = await _context.Set<IdentityUser>().ToListAsync();  same as line 44
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users.Where(x => x.Id != userId).Select(y => new { y.Id, y.UserName }).ToList();
        }

        // GET: api/ChatRooms/5
        //[HttpGet("{id}")]
        //[Route("/[controller]/GetChatRoom/{id}")]
        //public async Task<ActionResult<ChatRoom>> GetChatRoom(int id)
        //{
        //  if (_context.ChatRooms == null)
        //  {
        //      return NotFound();
        //  }
        //    var chatRoom = await _context.ChatRooms.FindAsync(id);

        //    if (chatRoom == null)
        //    {
        //        return NotFound();
        //    }

        //    return chatRoom;
        //}

        // PUT: api/ChatRooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChatRoom(int id, ChatRoom chatRoom)
        //{
        //    if (id != chatRoom.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(chatRoom).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ChatRoomExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/ChatRooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("/[controller]/PostChatRoom")]
        public async Task<ActionResult<ChatRoom>> PostChatRoom(ChatRoom chatRoom)
        {
          if (_context.ChatRooms == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ChatRooms'  is null.");
          }
            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatRoom", new { id = chatRoom.Id }, chatRoom);
        }

        // DELETE: api/ChatRooms/5
        [HttpDelete("{id}")]
        [Route("/[controller]/DeleteChatRoom/{id}")]  
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            if (_context.ChatRooms == null)
            {
                return NotFound();
            }
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            _context.ChatRooms.Remove(chatRoom);
            await _context.SaveChangesAsync();

            var room = await _context.ChatRooms.FirstOrDefaultAsync();

            return Ok(new {deleted = id, selected = (room == null ? 0 : room.Id)});
        }

        //private bool ChatRoomExists(int id)
        //{
        //    return (_context.ChatRooms?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
