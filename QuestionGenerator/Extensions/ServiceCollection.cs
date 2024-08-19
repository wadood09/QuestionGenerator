using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Application.Services;
using QuestionGenerator.Core.Domain.Entities;
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
                .AddScoped<IAssessmentSubmissionRepository, AssessmentSubmissionRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<IOptionRepository, OptionRepository>()
                .AddScoped<IQuestionRepository, QuestionRepository>()
                .AddScoped<IQuestionResultRepository, QuestionResultRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<ITokenRepository, TokenRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
                .AddScoped<IAssessmentService, AssessmentService>()
                .AddScoped<IAssessmentSubmissionService, AssessmentSubmissionService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IOptionService, OptionService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<IQuestionService, QuestionService>()
                .AddScoped<ITokenService, Tokenservice>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
