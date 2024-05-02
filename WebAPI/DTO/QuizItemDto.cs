using ApplicationCore.Models.QuizAggregate;
using BackendLab01;

namespace WebApIa.DTO
{
    public class QuizItemDto
    {
        public int Id { get; set; }
        public string Question{ get; set; }
        public List<string> Options { get;}
        public static QuizItemDto of(QuizItem quiz)
        {
            QuizItemDto qidto = new QuizItemDto();
            qidto.Id = quiz.Id;
            qidto.Question = quiz.Question;
            List<string> answers = new List<string>();
            answers.Add(quiz.CorrectAnswer);
            answers.AddRange(quiz.IncorrectAnswers);
            
            Random rnd = new Random();
            answers = answers.OrderBy(x => rnd.Next()).ToList();

            return qidto;

            
        }   
    }
}
