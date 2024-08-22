using AutoMapper;
using QuestionGenerator.Core.Domain.Entities;

namespace QuestionGenerator.Core.Application.AutoMapper.Resolver
{
    public class AssessmentGradeResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, double?>
    {
        public double? Resolve(TSource source, TDestination destination, double? destMember, ResolutionContext context)
        {
            if (source == null)
                return null;

            if (source is Assessment assessment)
            {
                var latestSubmission = assessment.AssessmentSubmissions.Count != 0 ? assessment.AssessmentSubmissions.OrderByDescending(r => r.DateCreated).First() : null;
                return CalculateGrade(latestSubmission);
            }
            else if(source is AssessmentSubmission assesment)
            {
                return CalculateGrade(assesment);
            }
            return null;
        }

        private static double CalculateGrade(AssessmentSubmission assesment)
        {
            if (assesment.Results.Count == 0)
                return 0;

            var score = assesment.Results.Count(x => x.UserAnswer.Equals(x.Question.Answer, StringComparison.OrdinalIgnoreCase));
            var grade = (double)score / assesment.Results.Count * 100;
            return grade;
        }
    }
}
