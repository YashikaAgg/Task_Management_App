using TaskManagementApp.Models;

namespace TaskManagementApp.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetTasksByColumnAsync(int boardColumnId);
        Task<TaskItem> GetTaskAsync(int id);
        Task<TaskItem> AddTaskAsync(TaskItem task);
        Task<TaskItem> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
        Task MoveTaskAsync(int taskId, int newColumnId);
    }
}
