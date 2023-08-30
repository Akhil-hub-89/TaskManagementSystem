using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Tests
{
    public class TaskControllerTests
    {
        private AppDbContext _context;
        private TaskController _taskController;

        [SetUp]
        public void Setup()
        {
            // Configure in-memory database for testing
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated(); // Create the in-memory database

            // Initialize the controller with the in-memory database context
            _taskController = new TaskController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateTask_ValidTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var task = new TaskModel
            {
                Name = "Sample Task",
                Description = "Task description",
                Deadline = DateTime.UtcNow.AddDays(7),
                IsFavorite = false
            };

            // Act
            var result = await _taskController.CreateTask(task);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task GetTask_ExistingTaskId_ReturnsTask()
        {
            // Arrange
            var taskToAdd = new TaskModel
            {
                Name = "Sample Task",
                Description = "Task description",
                Deadline = DateTime.UtcNow.AddDays(7),
                IsFavorite = false
            };
            _context.Tasks.Add(taskToAdd);
            _context.SaveChanges();

            // Act
            var result = await _taskController.GetTask(taskToAdd.Id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var task = okResult.Value as TaskModel;
            Assert.IsNotNull(task);
            Assert.AreEqual(taskToAdd.Name, task.Name);
        }

        [Test]
        public async Task UpdateTask_ExistingTask_ReturnsNoContent()
        {
            // Arrange
            var existingTask = new TaskModel
            {
                Name = "Task to update",
                Description = "Description",
                Deadline = DateTime.UtcNow.AddDays(7),
                IsFavorite = false
            };
            _context.Tasks.Add(existingTask);
            _context.SaveChanges();

            var updatedTask = new TaskModel
            {
                Id = existingTask.Id,
                Name = "Updated Task",
                Description = "Updated Description",
                Deadline = DateTime.UtcNow.AddDays(14),
                IsFavorite = true
            };

            // Act
            var result = await _taskController.UpdateTask(existingTask.Id, updatedTask);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteTask_ExistingTask_ReturnsNoContent()
        {
            // Arrange
            var existingTask = new TaskModel
            {
                Name = "Task to delete",
                Description = "Description",
                Deadline = DateTime.UtcNow.AddDays(7),
                IsFavorite = false
            };
            _context.Tasks.Add(existingTask);
            _context.SaveChanges();

            // Act
            var result = await _taskController.DeleteTask(existingTask.Id);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
