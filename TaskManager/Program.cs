using Microsoft.EntityFrameworkCore;
using InfrastructureLayer.Context;
using InfrastructureLayer.Repository.Commons;
using DomainLayer.Models;
using InfrastructureLayer.Repository.TaskRepository;
using ApplicationLayer.Services.TaskServices;
using ApplicationLayer.Services.Security;
using TaskManager.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaskManagerContext>(options =>
{ 
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagerDB"));
});

builder.Services.AddScoped<ICommonProcess<Tareas>, TaskRepository>();
builder.Services.AddScoped<TaskServices>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSignalR();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var Context = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();
//    // Ensure the database is created and apply any pending migrations
//    Context.Database.Migrate();
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<TaskHub>("/taskHub");

app.Run();
