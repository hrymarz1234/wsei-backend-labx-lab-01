using BackendLab01;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Web.Pages
{

    public class QuizItemModel : PageModel
    {
        private readonly IQuizUserService _userService;

        private readonly ILogger _logger;
        public QuizItemModel(IQuizUserService userService, ILogger<QuizItemModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [BindProperty]
        public string Question { get; set; }
        [BindProperty]
        public List<string> Answers { get; set; }

        [BindProperty]
        public String UserAnswer { get; set; }

        [BindProperty]
        public int QuizId { get; set; }
        [BindProperty]
        public int ItemId { get; set; }
        [BindProperty]
        public int? NextItemIndex { get; set; }

        public IActionResult OnGet(int quizId, int itemIndex)
        {
            QuizId = quizId;
            var quiz = _userService.FindQuizById(quizId);
            if (quiz is null)
            {
                return BadRequest();
            }
            var items = quiz.Items;
            if (items.Count <= itemIndex)
            {
                return NotFound();
            }
            var quizItem = quiz.Items[itemIndex];
            ItemId = quizItem.Id;
            NextItemIndex = items.Count > itemIndex + 1 ? itemIndex + 1 : null;
            Question = quizItem.Question;
            Answers = new List<string>() { quizItem.CorrectAnswer };
            Answers.AddRange(quizItem?.IncorrectAnswers);
            return Page();
        }
        
        public IActionResult OnPost()
        {
            _userService.SaveUserAnswerForQuiz(QuizId, 1, ItemId, UserAnswer);
            if (NextItemIndex is null)
            {
                return RedirectToPage("Summary", new { quizId = QuizId, userId = 1 });
            }
            return RedirectToPage("Item", new { quizId = QuizId, itemIndex = NextItemIndex });
        }
    }
}

