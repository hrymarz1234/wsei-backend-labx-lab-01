using ApplicationCore.Interfaces.Criteria;
using ApplicationCore.Models.QuizAggregate;

namespace ApplicationCore.Interfaces.AdminService;

public interface IQuizAdminService
{
    public QuizItem AddQuizItemToQuiz(int quizId, QuizItem item);
    public Quiz AddQuiz(Quiz quiz);
    public void UpdateQuiz(Quiz quiz);
    public IQueryable<QuizItem> FindAllQuizItems();
    public IQueryable<Quiz> FindAllQuizzes();

    public IEnumerable<Quiz> FindBySpecification(ISpecification<Quiz> specification);
}