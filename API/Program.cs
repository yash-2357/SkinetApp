using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddDbContext<StoreContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build(); //things before this line are services and things after this line are middlewares

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

//the below code applies the remaining migrations to DB and creates a new DB if DB doednot exists.
try {
  // using is used : any code that we create using this variable scope once this is finished executing then framework will dispose any services that we have used.
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;
  //create context
  var context = services.GetRequiredService<StoreContext>();
  
  //applies migrations
  await context.Database.MigrateAsync();

  //seeds data into DB
  await StoreContextSeed.SeedDataAsync(context);

} catch (Exception ex) {
  Console.WriteLine(ex);
  throw;
}

app.Run();
