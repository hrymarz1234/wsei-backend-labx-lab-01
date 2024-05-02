using ApplicationCore.Interfaces.Criteria;
using BackendLab01;
using ApplicationCore.Interfaces.Repository;
using Infrastructure.Memory;
using ApplicationCore.Models.QuizAggregate;
using ApplicationCore.Interfaces.AdminService;
using Infrastructure.Memory.Repositories;

namespace UnitTest;

public class MemoryQuizServiceTest
{
    private IGenericRepository<Quiz, int> quizRepository = new MemoryGenericRepository<Quiz, int>(new IntGenerator());

    private IGenericRepository<QuizItem, int> itemRepository =
        new MemoryGenericRepository<QuizItem, int>(new IntGenerator());

    private IGenericRepository<QuizItemUserAnswer, string> answerRepository =
        new MemoryGenericRepository<QuizItemUserAnswer, string>(null);

    private IQuizAdminService _aservice;
    private IQuizUserService _uservice;
    private User _user = new User() { Id = 1, Username = "Testowy" };
    private Quiz _quiz;

    public MemoryQuizServiceTest()
    {
        _aservice = new QuizAdminService(quizRepository, itemRepository);
        _uservice = new QuizUserService(quizRepository, answerRepository, itemRepository);

        _quiz = _aservice.AddQuiz(
            new Quiz()
            {
                Title = "Litery alfabetu",
            }
        );
        var item1 = _aservice.AddQuizItemToQuiz(_quiz.Id,
            new QuizItem()
            {
                CorrectAnswer = "A",
                IncorrectAnswers = new List<string>() { "B", "C", "D" },
                Question = "Pierwsza litera alfabetu?"
            });

        var item2 = _aservice.AddQuizItemToQuiz(_quiz.Id,
            new QuizItem()
            {
                CorrectAnswer = "B",
                IncorrectAnswers = new List<string>() { "A", "C", "E" },
                Question = "Druga litera alfabetu?"
            });

        var item3 = _aservice.AddQuizItemToQuiz(_quiz.Id,
            new QuizItem()
            {
                CorrectAnswer = "C",
                IncorrectAnswers = new List<string>() { "A", "B", "F" },
                Question = "Trzecia litera alfabetu?"
            });
    }

    [Fact]
    public void CreateItemsTest()
    {
        int previousCount = _aservice.FindAllQuizItems().Count();
        _aservice.AddQuizItemToQuiz(
            1,
            new QuizItem()
            {
                CorrectAnswer = "A",
                IncorrectAnswers = new List<string>() { "B", "C", "D" },
                Question = "Pierwsza litera alfabetu?"
            });
        var items = _aservice.FindAllQuizItems();
        Assert.Equal(previousCount + 1, items.Count());
    }

    [Fact]
    public void CreateQuizTest()
    {
        var findQuiz = _uservice.FindQuizById(_quiz.Id);
        Assert.Equal(_quiz.Id, findQuiz.Id);
    }

    [Fact]
    public void AddUserAnswerTest()
    {
        var quiz = _uservice.FindQuizById(_quiz.Id);
        _uservice.SaveUserAnswerForQuiz(quizId: quiz.Id, userId: _user.Id, quizItemId: quiz.Items[0].Id, "x");
        _uservice.SaveUserAnswerForQuiz(quizId: quiz.Id, userId: _user.Id, quizItemId: quiz.Items[1].Id,
            quiz.Items[1].CorrectAnswer);
        _uservice.SaveUserAnswerForQuiz(quizId: quiz.Id, userId: _user.Id, quizItemId: quiz.Items[2].Id,
            quiz.Items[2].CorrectAnswer);
        IQueryable<QuizItemUserAnswer> userAnswers = _uservice.GetUserAnswersForQuiz(quiz.Id, userId: _user.Id);
        Assert.Equal(3, userAnswers.Count());
        int count = _uservice.CountCorrectAnswersForQuizFilledByUser(quiz.Id, _user.Id);
        Assert.Equal(2, count);
    }
}




