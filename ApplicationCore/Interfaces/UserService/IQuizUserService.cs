using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models.QuizAggregate;

namespace BackendLab01;

public interface IQuizUserService
{
    Quiz CreateAndGetQuizRandom(int count);

    Quiz? FindQuizById(int id);

    IQueryable<Quiz> FindAll();

    void SaveUserAnswerForQuiz(int quizId, int userId, int quizItemId, string answer);

    IQueryable<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId);

    int CountCorrectAnswersForQuizFilledByUser(int quizId, int userId)
    {
        return GetUserAnswersForQuiz(quizId, userId)
            .Count(e => e.IsCorrect());
    }
    IEnumerable<Quiz> FindAllQuizzes();
    
}