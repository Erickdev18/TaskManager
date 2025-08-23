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
                DueDate = DateTime.Now.AddDays(1), // Vencimiento cercano
                Status = "Pending",
                AdditionalData = additionalData
            };
        }

        public static Tareas CreateLowPriorityTask(string description, string additionalData = "")
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(7), // Vencimiento lejano
                Status = "Pending",
                AdditionalData = additionalData
            };
        }

        // Puedes agregar más métodos para otros tipos de tareas
    }
}