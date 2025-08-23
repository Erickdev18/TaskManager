using System;

namespace DomainLayer.Models
{
    public static class TaskFactory
    {
        public static Tareas CreateHighPriorityTask(string description, string additionalData = "")
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(1),
                Status = "Pending",
                AdditionalData = additionalData
            };
        }

        public static Tareas CreateLowPriorityTask(string description, string additionalData = "")
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(7),
                Status = "Pending",
                AdditionalData = additionalData
            };
        }

        public static Tareas CreateCompletedTask(string description, DateTime dueDate, string additionalData = "")
        {
            return new Tareas
            {
                Description = description,
                DueDate = dueDate,
                Status = "Completed",
                AdditionalData = additionalData
            };
        }

        // tarea personalizada con parámetros flexibles
        public static Tareas CreateCustomTask(string description, DateTime dueDate, string status, string additionalData = "")
        {
            return new Tareas
            {
                Description = description,
                DueDate = dueDate,
                Status = status,
                AdditionalData = additionalData
            };
        }
    }
}