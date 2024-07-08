using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddContext(builder.Configuration.GetConnectionString("QuestionGenString")!);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.Configure<StorageConfig>(builder.Configuration.GetSection("FileStorage"));

builder.Services.Configure<OpenAiConfig>(builder.Configuration.GetSection("OpenAi"));

builder.Services.Configure<GoogleAuthConfig>(builder.Configuration.GetSection("GoogleAuths"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
