using ApplicationCore.Models.QuizAggregate;
using BackendLab01;

namespace WebApIa.DTO
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuizItemDto> Items { get; set; }
        public static QuizDto of(Quiz quiz)
        {
            QuizDto dto = new QuizDto();
            dto.Id = quiz.Id;
            dto.Title = quiz.Title;
            foreach (var quizItem in quiz.Items)
            {
                dto.Items.Add(QuizItemDto.of(quizItem));
            }
            return dto;

        }
    }
}
