using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Application.Services;
using QuestionGenerator.Infrastructure.Context;
using QuestionGenerator.Infrastructure.Repositories;

namespace QuestionGenerator.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<QuestionGeneratorContext>(a => a.UseMySQL(connectionString));
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IAssessmentRepository, AssessmentRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<IOptionRepository, OptionRepository>()
                .AddScoped<IQuestionRepository, QuestionRepository>()
                .AddScoped<IRevisitedAssessmentRepository, RevisitedAssessmentRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAssessmentService, AssessmentService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
