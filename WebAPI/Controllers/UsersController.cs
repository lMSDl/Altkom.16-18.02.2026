using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ICrudService<User> _service;

        public UsersController(ICrudService<User> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _service.ReadAsync(id);
            if(user is null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _service.ReadAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(User user)
        {
            if(user is null)
                return BadRequest();

            var userId = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = userId }, userId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, User user)
        {
            var localUser = await _service.ReadAsync(id);
            if(localUser is null)
                return NotFound();

            var updated = await _service.UpdateAsync(user);
            if(!updated)
                return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var localUser = await _service.ReadAsync(id);
            if (localUser is null)
                return NotFound();
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return BadRequest();
            return NoContent();
        }
    }
}
