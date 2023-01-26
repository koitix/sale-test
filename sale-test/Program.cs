using Microsoft.EntityFrameworkCore;
using sale_test.Infrastucture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<SaleApiDbContext>(options => options.UseInMemoryDatabase("CustomerDb"));
//builder.Services.AddDbContext<CustomersApiDbContext>(options => options.UseInMemoryDatabase("CustomerDb"));

builder.Services.AddDbContext<CustomersApiDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SalesApiConnectionString")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
