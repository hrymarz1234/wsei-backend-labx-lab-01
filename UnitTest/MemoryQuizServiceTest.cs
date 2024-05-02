using ApplicationCore.Interfaces.Criteria;
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models.QuizAggregate;
using Infrastructure.Memory;
using Infrastructure.Memory.Repositories;


namespace UnitTest;

public class GenericRepositoryTest
{
    private readonly IGenericRepository<QuizItem, int> quizItemRepository;
    private readonly QuizItem item1;
    private readonly QuizItem item2;
    private readonly QuizItem item3;

    public GenericRepositoryTest()
    {
        quizItemRepository = new MemoryGenericRepository<QuizItem, int>(new IntGenerator());
        item1 = quizItemRepository.Add(new QuizItem()
        {
            Question = "Litera?",
            IncorrectAnswers = new List<string>() { "B", "C", "D" },
            CorrectAnswer = "A",
            Id = 0
        }
        );
        item2 = quizItemRepository.Add(new QuizItem()
        {
            Question = "Planeta?",
            IncorrectAnswers = new List<string>() { "Mars", "Wenus", "Pluton" },
            CorrectAnswer = "Jowisz",
            Id = 0
        });
        item3 = quizItemRepository.Add(new QuizItem()
        {
            Question = "Miasto?",
            IncorrectAnswers = new List<string>() { "Kielce", "Kraków", "Katowice" },
            CorrectAnswer = "Ko³obrzeg",
            Id = 0
        });
    }

    [Fact]
    public void CreateTest()
    {
        Assert.Equal(item1.Id, quizItemRepository.FindById(item1.Id).Id);
        Assert.Equal(item2.Id, quizItemRepository.FindById(item2.Id).Id);
        Assert.Equal(item3.Id, quizItemRepository.FindById(item3.Id).Id);
    }

    [Fact]
    public void DeleteTest()
    {
        var newItem = quizItemRepository.Add(new QuizItem()
        {
            Id = 1,
            CorrectAnswer = "x",
            IncorrectAnswers = new List<string>() { "1", "2", "3" },
            Question = "?"
        }
        );
        Assert.Contains(quizItemRepository.FindAll(), item => item.Id == newItem.Id);
        Assert.Equal(4, quizItemRepository.FindAll().Count());
        quizItemRepository.RemoveById(newItem.Id);
        Assert.Equal(3, quizItemRepository.FindAll().Count());
        Assert.Contains(quizItemRepository.FindAll(), item => item.Id == item2.Id);
        Assert.Contains(quizItemRepository.FindAll(), item => item.Id == item3.Id);
        Assert.DoesNotContain(quizItemRepository.FindAll(), item => item.Id == newItem.Id);
    }

    [Fact]
    public void UpdateTest()
    {
        var updatedQuiz = new QuizItem()
        {
            Id = item1.Id,
            CorrectAnswer = item1.CorrectAnswer,
            IncorrectAnswers = item1.IncorrectAnswers,
            Question = "question"
        };
        quizItemRepository.Update(item1.Id, updatedQuiz);
        var item = quizItemRepository.FindById(item1.Id);
        Assert.Equal(updatedQuiz.Question, item.Question);
    }

    [Fact]
    public void FindBySpecification()
    {
        IEnumerable<QuizItem> items = quizItemRepository.FindBySpecification(new QuizItemByQuestion("Miasto?"));
        Assert.Contains(items, item => item.Question == "Miasto?");
        Assert.Single(items);
    }
}