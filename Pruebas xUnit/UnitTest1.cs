using Xunit;
using DomainLayer.Models;
using System;

namespace Pruebas_xUnit
{
    public class TareasDescriptionTests
    {
        [Fact]
        public void Description_ShouldNotBeNullOrEmpty()
        {
            var tarea = new Tareas { Description = "Test" };
            Assert.False(string.IsNullOrWhiteSpace(tarea.Description));
        }
    }

    public class TareasDueDateTests
    {
        [Fact]
        public void DueDate_ShouldBeInFuture()
        {
            var tarea = new Tareas { DueDate = DateTime.Now.AddDays(1) };
            Assert.True(tarea.DueDate > DateTime.Now);
        }
    }

    public class TareasStatusTests
    {
        [Fact]
        public void Status_CanBeSetAndChanged()
        {
            var tarea = new Tareas { Status = "Pending" };
            tarea.Status = "Completed";
            Assert.Equal("Completed", tarea.Status);
        }
    }

    public class TareasAdditionalDataTests
    {
        [Fact]
        public void AdditionalData_CanBeSetAndChanged()
        {
            var tarea = new Tareas { AdditionalData = "Info" };
            tarea.AdditionalData = "NewInfo";
            Assert.Equal("NewInfo", tarea.AdditionalData);
        }
    }

    public class TareasIdTests
    {
        [Fact]
        public void Id_CanBeSetAndIsInt()
        {
            var tarea = new Tareas { Id = 42 };
            Assert.Equal(42, tarea.Id);
        }
    }

    public class TareasDefaultValuesTests
    {
        [Fact]
        public void Default_Id_IsZero()
        {
            var tarea = new Tareas();
            Assert.Equal(0, tarea.Id);
        }
    }

    public class TareasChangeDescriptionTests
    {
        [Fact]
        public void Description_CanBeChanged()
        {
            var tarea = new Tareas { Description = "Old" };
            tarea.Description = "New";
            Assert.Equal("New", tarea.Description);
        }
    }

    public class TareasChangeDueDateTests
    {
        [Fact]
        public void DueDate_CanBeChanged()
        {
            var tarea = new Tareas { DueDate = DateTime.Now.AddDays(1) };
            var newDate = DateTime.Now.AddDays(5);
            tarea.DueDate = newDate;
            Assert.Equal(newDate, tarea.DueDate);
        }
    }

    public class TareasChangeStatusTests
    {
        [Fact]
        public void Status_CanBeChanged()
        {
            var tarea = new Tareas { Status = "Pending" };
            tarea.Status = "InProgress";
            Assert.Equal("InProgress", tarea.Status);
        }
    }

    public class TareasChangeAdditionalDataTests
    {
        [Fact]
        public void AdditionalData_CanBeChanged()
        {
            var tarea = new Tareas { AdditionalData = "OldData" };
            tarea.AdditionalData = "UpdatedData";
            Assert.Equal("UpdatedData", tarea.AdditionalData);
        }
    }
}