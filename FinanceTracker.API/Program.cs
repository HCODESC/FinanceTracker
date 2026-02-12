using System.Text;
using FinanceTracker.API;
using FinanceTracker.API.Data;
using FinanceTracker.API.Services.Auth;
using FinanceTracker.API.Services.Budget;
using FinanceTracker.API.Services.Category;
using FinanceTracker.API.Services.Report;
using FinanceTracker.API.Services.Transaction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using FinanceTracker.API.Middleware;
using FinanceTracker.API.Services.UserProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });

    
});

builder.Services.AddDbContext<FinanceTrackerDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("FinanceTrackerDb")); 
});

//Register Auth Service with JWT
var supabaseUrl = builder.Configuration["supabase:url"]!.TrimEnd('/');
var authIssuer = $"{supabaseUrl}/auth/v1";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>
{
    options.Authority = authIssuer;
    options.Audience = "authenticated";
    options.MetadataAddress = $"{authIssuer}/.well-known/openid-configuration";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authIssuer,

        ValidateAudience = true,
        ValidAudience = "authenticated",

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        ClockSkew = TimeSpan.FromMinutes(2), 
    }; 
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IReportService, ReportService>(); 
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

//AutoMapper config
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingConfig>());

builder.Services.AddAuthorization(); 

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "My API",
        Version = "v1",
        Description = "API with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
});

var app = builder.Build();

// Apply Migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at root URL
    });
}

app.UseHttpsRedirection();
app.UseRouting(); 
app.UseCors("AllowAngularDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
