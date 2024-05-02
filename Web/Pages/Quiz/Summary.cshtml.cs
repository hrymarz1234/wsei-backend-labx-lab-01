using BackendLab01;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class Summary : PageModel
{
    private readonly IQuizUserService _userService;

    public int CorrectAnswerCount { get; set; }
    public Summary(IQuizUserService userService)
    {
        _userService = userService;
    }

    public void OnGet(int quizId, int userId)
    {
        CorrectAnswerCount = _userService.CountCorrectAnswersForQuizFilledByUser(quizId, userId);
    }
}