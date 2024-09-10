using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayStack.Net;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Extensions;
using QuestionGenerator.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wadood Question-Generator", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please Enter a Valid Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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
            new string[] {}
        }
    });
});

builder.Services.AddCors(cors =>
{
    cors.AddPolicy("question_Generator", pol =>
    {
        pol.WithOrigins("http://127.0.0.1:5500")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddContext(builder.Configuration.GetConnectionString("QuestionGenString")!);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
            options.MapInboundClaims = false;
        });

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("CohereApi", client =>
{
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", builder.Configuration["Cohere:ApiKey"]);
    client.Timeout = TimeSpan.FromSeconds(100);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("BrevoApi", client =>
{
    client.DefaultRequestHeaders.Add("api-key", builder.Configuration["Brevo:ApiKey"]);
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("PaystackApi", client =>
{
    client.BaseAddress = new Uri("https://api.paystack.co/transaction");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", builder.Configuration["Paystack:ApiSecretKey"]);
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpContextAccessor();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddScoped(provider =>
{
    var paystackSecretKey = builder.Configuration["Paystack:ApiSecretKey"];
    return new PayStackApi(paystackSecretKey);
});

builder.Services.AddSingleton<CohereService>();

builder.Services.Configure<StorageConfig>(builder.Configuration.GetSection("FileStorage"));

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

builder.Services.Configure<OpenAiConfig>(builder.Configuration.GetSection("OpenAi"));

builder.Services.Configure<GoogleAuthConfig>(builder.Configuration.GetSection("GoogleAuths"));

builder.Services.Configure<BrevoConfig>(builder.Configuration.GetSection("Brevo"));

builder.Services.Configure<PaystackConfig>(builder.Configuration.GetSection("Paystack"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("question_Generator");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
