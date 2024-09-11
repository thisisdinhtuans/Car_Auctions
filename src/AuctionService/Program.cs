using AuctionService.Consumers;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    x.AddEntityFrameworkOutbox<AuctionDbContext>(o=>{
        o.QueryDelay=TimeSpan.FromSeconds(10);

        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    x.UsingRabbitMq((context, cfg)=>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host => {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });
        
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>{
        options.Authority=builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata=false;
        options.TokenValidationParameters.ValidateAudience=false;
        options.TokenValidationParameters.NameClaimType="username";
    });

var app = builder.Build();
app.UseAuthentication();
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
