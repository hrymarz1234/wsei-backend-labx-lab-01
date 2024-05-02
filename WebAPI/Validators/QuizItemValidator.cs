using ApplicationCore.Models.QuizAggregate;
using FluentValidation;

namespace WebAPI.Validators
{
    public class QuizItemValidator : AbstractValidator<QuizItem>
    {
        public QuizItemValidator()
        {
            RuleFor(q => q.Question)
                .MaximumLength(200).WithMessage("Pytanie nie może być dłuższe niż 200 znaków.")
                .MinimumLength(3).WithMessage("Pytanie nie może być krótsze od 3 znaków!");
            RuleForEach(q => q.IncorrectAnswers)
                .MaximumLength(200)
                .MinimumLength(1);
            RuleFor(q => q.CorrectAnswer)
                .MinimumLength(1)
                .MaximumLength(200);
            RuleFor(q => new { q.CorrectAnswer, q.IncorrectAnswers })
                .Must(t => !t.IncorrectAnswers.Contains(t.CorrectAnswer))
                .WithMessage("Poprawna odpowiedź nie powinna występować w liście niepoprawnych odpowiedzi!");
            RuleFor(q => q.IncorrectAnswers)
                .Must(i => i.Count > 0);
        }
    }
}
