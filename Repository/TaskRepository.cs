using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByColumnAsync(int boardColumnId)
        {
            // Retrieve tasks ordered alphabetically, with favorites on top
            return await _context.Tasks
                .Where(t => t.BoardColumnId == boardColumnId)
                .OrderByDescending(t => t.IsFavorite)
                .ThenBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<TaskItem> GetTaskAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItem> AddTaskAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task MoveTaskAsync(int taskId, int newColumnId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                task.BoardColumnId = newColumnId;
                await _context.SaveChangesAsync();
            }
        }
    }
}
