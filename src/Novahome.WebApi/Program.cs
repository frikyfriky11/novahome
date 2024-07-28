WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

WebApplication app = builder.Build();
app.UseWebApiServices(app.Configuration);

app.Run();
