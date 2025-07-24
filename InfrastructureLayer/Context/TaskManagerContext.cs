using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Context
{
    public class TaskManagerContext: DbContext 
    {
        public TaskManagerContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Tareas> Tarea { get; set; } 
    }
}
