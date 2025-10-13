using System.Security.Cryptography;
using System.Text;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Repositories;
using medical_appointment_scheduling_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Supabase configuration will be handled by the SupabaseTokenService

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var keyTrial = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)); // 32 bytes = 256 bits
Console.WriteLine($"Secret Key: {keyTrial}");

// Add services to the container.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure PostgreSQL database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ensure the following NuGet package is installed in your project:
// Microsoft.EntityFrameworkCore.SqlServer
// Configure JWT authentication
var key = Encoding.ASCII.GetBytes("ja9LS02MnUixI/NNYYvjgQbtAg210NK9jrg53yg+fwY="); // Replace with your secret key
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuers = new[] { "https://localhost:44384", "https://your-other-issuer.com" }, // Replace with your valid issuers
        ValidAudiences = new[] { "https://localhost:44384", "https://your-other-audience.com" }, // Replace with your valid audiences
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSingleton(new JwtTokenService("ja9LS02MnUixI/NNYYvjgQbtAg210NK9jrg53yg+fwY=", "https://localhost:44384", "https://localhost:44384"));
builder.Services.AddSingleton<SupabaseTokenService>();
builder.Services.AddScoped<EmailRepository>();
builder.Services.AddScoped<IAnamneseRepository, AnamneseRepository>();
builder.Services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
builder.Services.AddScoped<IClientHealthPlansRepository, ClientHealthPlansRepository>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IClinicsRepository, ClinicsRepository>();
builder.Services.AddScoped<IClinicUsersRepository, ClinicUsersRepository>();
builder.Services.AddScoped<IDoctorHealthPlansRepository, DoctorHealthPlansRepository>();
builder.Services.AddScoped<IDoctorsRepository, DoctorsRepository>();
builder.Services.AddScoped<IHealthPlansRepository, HealthPlansRepository>();
builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped<ISchedulesRepository, SchedulesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IWaitlistRepository, WaitlistRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
