using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public int Key { get; set; }
        public RPTestPartType Part { get; set; }
    }
}