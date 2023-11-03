using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
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
   opt.UseNpgsql(builder.Configuration.GetConnectionString("TodoContext") ?? throw new InvalidOperationException("Connection string 'TodooContext' not found.")));

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