using JobSity.API.Hubs;
using JobSity.API.Mapping;
using JobSity.API.Services.Abstract;
using JobSity.API.Services.Implementations;
using JobSity.DAO;
using JobSity.Messaging.Receiver;
using JobSity.Messaging.Sender;
using JobSity.Model.Models;
using JobSity.Model.Models.Messaging;
using JobSity.Repositories.Abstract;
using JobSity.Repositories.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;

services.AddControllers();
services.AddDbContext<JobSityContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<JobSityContext>().AddDefaultTokenProviders();

services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:4200")
        .WithOrigins("http://localhost:5000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobSity.API", Version = "v1" });
});

services.AddSignalR(conf =>
{
    conf.MaximumReceiveMessageSize = null;
    conf.ClientTimeoutInterval = TimeSpan.FromMinutes(3);
});

services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("SecretKey").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && (path.Value.Contains("chatHub")))
            {
                context.Token = accessToken;

            }

            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        }
    };
});

services.AddAutoMapper(typeof(MappingProfile));
services.AddScoped(typeof(IEntityBaseRepositoryAsync<>), typeof(EntityBaseRepositoryAsync<>));
services.AddScoped<IUserService, UserService>();
services.AddTransient<ChatHubService<ChatHub>>();

var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();
services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);
if (serviceClientSettings.Enabled)
{
    services.AddScoped<IStockRequestSender, StockRequestSender>();
    services.AddHostedService<StockResponseReceiver>();
}


var app = builder.Build();




using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JobSityContext>();  
    //db.Database.EnsureCreated();
    db.Database.Migrate();
}



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobSity.API v1"));
}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/api/chatHub");
    endpoints.MapControllers();
});

app.Run();
