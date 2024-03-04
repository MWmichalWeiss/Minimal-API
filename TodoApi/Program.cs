using TodoApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("toDoDb"), new MySqlServerVersion(new Version(8, 0, 36)));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

 builder.Services.AddSwaggerGen();
 builder.Services.AddControllers();
 builder.Services.AddEndpointsApiExplorer();
 app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

 if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/items", async (ToDoDbContext context) => {
    var tasks=await context.Items.ToListAsync();
    return tasks;
});

app.MapPost("/items", async (ToDoDbContext context, Item newItem) =>
{
    context.Items.Add(newItem);
    await context.SaveChangesAsync();
    return Results.Created($"/items/{newItem.Id}", newItem);
});

app.MapPut("/items/{id}", async (ToDoDbContext context, int id, Item updatedItem) =>
{
    var existingItem = await context.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }

    existingItem.Name = updatedItem.Name;
    existingItem.IsComplete = updatedItem.IsComplete;

    await context.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("/items/{id}", async (ToDoDbContext context, int id) =>
{
    var existingItem = await context.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound("item not found!!");
    }

    context.Items.Remove(existingItem);
    await context.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
