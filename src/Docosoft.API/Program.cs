using Docosoft.API.Core.Data;
using Docosoft.API.Core.Managers;
using Docosoft.API.Core.ResourceAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("defaultDbDSN"), b => b.MigrationsAssembly("Docosoft.API"));
});

builder.Services.AddTransient<ISQLiteResourceAccess, SQLiteResourceAccess>();
builder.Services.AddTransient<IDocosoftUserManager, DocosoftUserManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.AllowAnyOrigin()
    .WithOrigins("http://localhost:7237")
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
