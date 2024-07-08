using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Infrastructure.Context;

namespace QuestionGenerator.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<QuestionGeneratorContext>(a => a.UseMySQL(connectionString));
        }
    }
}
