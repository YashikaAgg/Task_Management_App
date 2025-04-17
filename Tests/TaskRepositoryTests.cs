using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TaskManagementApp.Data;
using TaskManagementApp.Models;
using TaskManagementApp.Repository;


namespace TaskManagementApp.Tests
{
    [TestFixture]
    public class TaskRepositoryTests
    {
        private TaskManagementContext _context;
        private TaskRepository _repository;

        [SetUp]
        public void Setup()
        {
            // Use the in-memory provider for testing
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TaskManagementContext(options);

            // Seed a default board column
            _context.BoardColumns.Add(new BoardColumn { Id = 1, Name = "ToDo" });
            _context.BoardColumns.Add(new BoardColumn { Id = 2, Name = "In Progress" });
            _context.SaveChanges();

            _repository = new TaskRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddTaskAsync_Should_Add_Task()
        {
            var newTask = new TaskItem
            {
                BoardColumnId = 1,
                Name = "Test Task",
                Description = "Test Description",
                Deadline = DateTime.Now.AddDays(1),
                IsFavorite = false
            };

            var task = await _repository.AddTaskAsync(newTask);

            Assert.IsNotNull(task);
            Assert.AreEqual(1, _context.Tasks.Count());
        }

        [Test]
        public async Task UpdateTaskAsync_Should_Update_Task()
        {
            var task = new TaskItem
            {
                BoardColumnId = 1,
                Name = "Initial Task",
                Description = "Initial Desc",
                Deadline = DateTime.Now.AddDays(1),
                IsFavorite = false
            };

            await _repository.AddTaskAsync(task);

            // Modify task details
            task.Name = "Updated Task";
            task.IsFavorite = true;
            await _repository.UpdateTaskAsync(task);

            var updatedTask = await _repository.GetTaskAsync(task.Id);
            Assert.AreEqual("Updated Task", updatedTask.Name);
            Assert.IsTrue(updatedTask.IsFavorite);
        }

        [Test]
        public async Task DeleteTaskAsync_Should_Delete_Task()
        {
            var task = new TaskItem
            {
                BoardColumnId = 1,
                Name = "Task to Delete",
                Description = "Desc",
                Deadline = DateTime.Now.AddDays(1)
            };

            await _repository.AddTaskAsync(task);
            bool result = await _repository.DeleteTaskAsync(task.Id);

            Assert.IsTrue(result);
            Assert.IsNull(await _repository.GetTaskAsync(task.Id));
        }

        [Test]
        public async Task MoveTaskAsync_Should_Change_Task_BoardColumn()
        {
            var task = new TaskItem
            {
                BoardColumnId = 1,
                Name = "Movable Task",
                Description = "Desc",
                Deadline = DateTime.Now.AddDays(1)
            };

            await _repository.AddTaskAsync(task);
            await _repository.MoveTaskAsync(task.Id, 2);

            var movedTask = await _repository.GetTaskAsync(task.Id);
            Assert.AreEqual(2, movedTask.BoardColumnId);
        }

        [Test]
        public async Task GetTasksByColumnAsync_Should_Sort_Favorites_On_Top_Then_Alphabetically()
        {
            // Add tasks with various names and favorite flags
            await _repository.AddTaskAsync(new TaskItem { BoardColumnId = 1, Name = "Beta Task", IsFavorite = false });
            await _repository.AddTaskAsync(new TaskItem { BoardColumnId = 1, Name = "Alpha Task", IsFavorite = true });
            await _repository.AddTaskAsync(new TaskItem { BoardColumnId = 1, Name = "Gamma Task", IsFavorite = false });
            await _repository.AddTaskAsync(new TaskItem { BoardColumnId = 1, Name = "Delta Task", IsFavorite = true });

            var tasks = (await _repository.GetTasksByColumnAsync(1)).ToList();

            // Favorites come first, then sorted by name
            // Expected order: Alpha Task, Delta Task, Beta Task, Gamma Task.
            Assert.AreEqual("Alpha Task", tasks[0].Name);
            Assert.AreEqual("Delta Task", tasks[1].Name);
            Assert.AreEqual("Beta Task", tasks[2].Name);
            Assert.AreEqual("Gamma Task", tasks[3].Name);
        }
    }
}
