using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

//builder khởi tạo một ứng dụng WebApplication, nơi các dịch vụ(services) như controller và DbContext được thêm vào
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt=>{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

//khởi tạo cơ sở dữ liệu và seed dữ liệu ban đầu nếu cần thiết
try
{
    DbInitializer.InitDb(app);
} catch(Exception e)
{
    Console.WriteLine(e);
}

app.Run();
