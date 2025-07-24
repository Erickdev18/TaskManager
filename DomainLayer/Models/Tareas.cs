using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Tareas
    {
        [Key] // Assuming you are using System.ComponentModel.DataAnnotations for the Key attribute
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Unique identifier for the task
        public string Description { get; set; } // Description of the task
        public DateTime DueDate { get; set; } // Due date for the task
        public string Status { get; set; } //  "Pending", "Completed"
        public string AdditionalData { get; set; } // Generic type for additional data


    }
}
