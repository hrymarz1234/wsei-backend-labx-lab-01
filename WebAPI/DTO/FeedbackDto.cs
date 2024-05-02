using ApplicationCore.Models;
using WebApIa.DTO;
namespace WebAPI.Dto;

public class FeedbackDto
{
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public int TotalQuestions { get; set; }
    public IEnumerable<QuizItemAnswerDto> Answers { get; set; }
}
