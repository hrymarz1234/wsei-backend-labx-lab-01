using System.Data;
using ApplicationCore.Interfaces.Criteria;
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;

namespace ApplicationCore.Interfaces.AdminService;

public class QuizAdminService : IQuizAdminService
{
    private readonly IGenericRepository<Quiz, int> quizRepository;
    private readonly IGenericRepository<QuizItem, int> itemRepository;

    public QuizAdminService(IGenericRepository<Quiz, int> quizRepository,
        IGenericRepository<QuizItem, int> itemRepository)
    {
        this.quizRepository = quizRepository;
        this.itemRepository = itemRepository;
    }

    public QuizItem AddQuizItemToQuiz(int quizId, QuizItem item)
    {
        var quiz = quizRepository.FindById(quizId);
        if (quiz is null)
        {
            throw new Exception();
        }
        var newItem = itemRepository.Add(item);
        quiz.Items.Add(newItem);
        quizRepository.Update(quizId, quiz);
        return newItem;
    }

    public void UpdateQuizItem(QuizItem item)
    {
        itemRepository.Update(item.Id, item);
    }

    public QuizItem? FindQuizItemById(int id)
    {
        return itemRepository.FindById(id);
    }

    public Quiz AddQuiz(Quiz quiz)
    {
        return quizRepository.Add(quiz);
    }

    public void UpdateQuiz(Quiz quiz)
    {
        quizRepository.Update(quiz.Id, quiz);
    }

    public IQueryable<QuizItem> FindAllQuizItems()
    {
        return itemRepository.FindAll().AsQueryable();
    }

    public IQueryable<Quiz> FindAllQuizzes()
    {
        return quizRepository.FindAll().AsQueryable();
    }

    public IEnumerable<Quiz> FindBySpecification(ISpecification<Quiz> specification)
    {
        return quizRepository.FindBySpecification(specification);
    }
}