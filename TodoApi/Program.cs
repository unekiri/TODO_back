using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: MyAllowSpecificOrigins,
                  policy =>
                  {
                      policy.AllowAnyOrigin()
                      .AllowAnyHeader();
                  });
});

// services.AddResponseCashing();

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();