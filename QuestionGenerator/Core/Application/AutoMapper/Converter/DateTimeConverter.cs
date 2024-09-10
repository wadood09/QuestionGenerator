using AutoMapper;
using System.Globalization;

namespace QuestionGenerator.Core.Application.AutoMapper.Converter
{
    public class DateTimeConverter : IValueConverter<DateTime, string>
    {
        public string Convert(DateTime sourceMember, ResolutionContext context)
        {
            return sourceMember.ToString("dddd, MMMM d, yyyy h:mmtt", CultureInfo.InvariantCulture);
        }
    }
}
