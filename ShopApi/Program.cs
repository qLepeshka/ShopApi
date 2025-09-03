using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using ShopApi.Data;
using ShopApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI();
app.MapHub<OrderHub>("/orderHub");
app.MapControllers();
app.MapGet("/admin", () => Results.File("admin.html", "text/html"));
app.Run();