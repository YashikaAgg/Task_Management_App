using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models;
using TaskManagementApp.Repository;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        // GET api/tasks/{boardColumnId}
        [HttpGet("ByColumn/{boardColumnId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByColumn(int boardColumnId)
        {
            var tasks = await _repository.GetTasksByColumnAsync(boardColumnId);
            return Ok(tasks);
        }

        // GET api/tasks/task/5
        [HttpGet("Task/{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _repository.GetTaskAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // POST api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem task)
        {
            var createdTask = await _repository.AddTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> UpdateTask(int id, [FromBody] TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            var updatedTask = await _repository.UpdateTaskAsync(task);
            return Ok(updatedTask);
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            bool deleted = await _repository.DeleteTaskAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        // PATCH api/tasks/move/5
        [HttpPatch("move/{id}")]
        public async Task<IActionResult> MoveTask(int id, [FromQuery] int newColumnId)
        {
            await _repository.MoveTaskAsync(id, newColumnId);
            return NoContent();
        }
    }

}
