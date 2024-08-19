using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PayStack.Net;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Extensions;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddContext(builder.Configuration.GetConnectionString("QuestionGenString")!);

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
        });

builder.Services.AddHttpClient();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
