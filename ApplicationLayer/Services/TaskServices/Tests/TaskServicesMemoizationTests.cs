using Xunit;
using ApplicationLayer.Services.TaskServices;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TaskServicesMemoizationTests
{
    [Fact]
    public async Task CalculateTaskCompletionRateAsync_ShouldUseCache()
    {
        // Arrange
        var mockProcess = new MockCommonProcess();
        var service = new TaskServices(mockProcess);

        // Act: Primera llamada (calcula y guarda en caché)
        double rate1 = await service.CalculateTaskCompletionRateAsync();
        // Modifica el mock para simular cambio de datos
        mockProcess.Tareas.Add(new Tareas { Status = "Completed" });
        // Segunda llamada (debe devolver el valor memorizado, no recalcular)
        double rate2 = await service.CalculateTaskCompletionRateAsync();

        // Assert
        Assert.Equal(rate1, rate2);
    }

    [Fact]
    public async Task FilterTasksAsync_ShouldUseCache()
    {
        // Arrange
        var mockProcess = new MockCommonProcess();
        var service = new TaskServices(mockProcess);
        string status = "Completed";

        // Act: Primera llamada (calcula y guarda en caché)
        var result1 = await service.FilterTasksAsync(status);
        // Modifica el mock para simular cambio de datos
        mockProcess.Tareas.Add(new Tareas { Status = status });
        // Segunda llamada (debe devolver el valor memorizado, no recalcular)
        var result2 = await service.FilterTasksAsync(status);

        // Assert
        Assert.Equal(result1.Count, result2.Count);
    }
}

// Mock simple para pruebas
public class MockCommonProcess : InfrastructureLayer.Repository.Commons.ICommonProcess<Tareas>
{
    public List<Tareas> Tareas = new()
    {
        new Tareas { Status = "Completed" },
        new Tareas { Status = "Pending" }
    };

    public Task<IEnumerable<Tareas>> GetAllAsync() => Task.FromResult<IEnumerable<Tareas>>(Tareas);
    public Task<Tareas?> GetByIdAsync(int id) => Task.FromResult(Tareas.Count > id ? Tareas[id] : null);
    public Task<(bool IsSuccess, string Message)> AddAsync(Tareas entry) { Tareas.Add(entry); return Task.FromResult((true, "")); }
    public Task<(bool IsSuccess, string Message)> UpdateAsync(Tareas entry) => Task.FromResult((true, ""));
    public Task<(bool IsSuccess, string Message)> DeleteAsync(int id) { if (Tareas.Count > id) Tareas.RemoveAt(id); return Task.FromResult((true, "")); }
}