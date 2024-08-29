using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

//builder khởi tạo một ứng dụng WebApplication, nơi các dịch vụ(services) như controller và DbContext được thêm vào
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt=>{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//đăng ký AutoMapper trong 'Service'
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x=>
{
    x.UsingRabbitMq((context, cfg)=>
    {
        cfg.ConfigureEndpoints(context);
    });
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
