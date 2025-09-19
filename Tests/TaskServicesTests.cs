using Xunit;
using ApplicationLayer.Services.TaskServices;
using DomainLayer.Models;
using System.Threading.Tasks;

public class TaskServicesTests
{
    [Fact]
    public async Task AddTaskAllAsync_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var mockProcess = new MockCommonProcess();
        var service = new TaskServices(mockProcess);
        var tarea = new Tareas { Description = "Test", DueDate = System.DateTime.Now.AddDays(1) };

        // Act
        var response = await service.AddTaskAllAsync(tarea);

        // Assert
        Assert.True(response.Succesful);
    }
}