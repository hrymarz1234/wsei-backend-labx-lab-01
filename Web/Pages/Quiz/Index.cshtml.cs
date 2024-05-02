using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Quiz;

public class IndexModel : PageModel
{
    private readonly IQuizUserService _quizUserService;

    [BindProperty]
    public List<ApplicationCore.Models.QuizAggregate.Quiz> Quizzes { get; set; }


    public IndexModel(IQuizUserService quizUserService)
    {
        _quizUserService = quizUserService;
    }


    public void OnGet()
    {
        Quizzes = _quizUserService.FindAll().ToList();

    }
}