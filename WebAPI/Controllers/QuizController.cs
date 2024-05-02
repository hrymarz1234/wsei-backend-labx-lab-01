using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApIa.DTO;

namespace WebApIa.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("/api/v1/quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizUserService _service;
        private readonly IQuizAdminService _adminservice;

        public QuizController(IQuizUserService service, IQuizAdminService adminservice)
        {
            _service = service;
            _adminservice = adminservice;
        }

        [HttpGet("{id}")]
        public ActionResult<QuizDto> FindById(int id)
        {
            Quiz quiz = _service.FindQuizById(id);
            if (quiz != null)
            {
                QuizDto quizDto = QuizDto.of(quiz);
                return Ok(quizDto);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IEnumerable<QuizDto> FindAll()
        {
            IEnumerable<Quiz> quizzes = (IEnumerable<Quiz>)_adminservice.FindAllQuizItems();
            List<QuizDto> list = new List<QuizDto>();
            foreach(var quiz in quizzes) 
            {
                QuizDto quizDto = QuizDto.of(quiz);
                list.Add(quizDto);
            }
            return list;
        }
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("{quizId}/items/{itemId}")]
        public void SaveAnswer([FromBody] QuizItemAnswerDto dto, int quizId, int quizItemId)
        {
            int userId = dto.UserId;
            string answer = dto.Answer;
            _service.SaveUserAnswerForQuiz(quizId, userId, quizItemId, answer);
        }
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("{quizId}/user/{userId}/correct-answers-count")]
        public ActionResult<QuizUserAnswersDto> GetCorrectAnswersCount(int quizId, int userId)
        { 
            int correctAnswersCount = _service.CountCorrectAnswersForQuizFilledByUser(quizId, userId);

            QuizUserAnswersDto userAnswersDto = new QuizUserAnswersDto
            {
                QuizId = quizId,
                UserId = userId,
                CorrectAnswersCount = correctAnswersCount
            };

            return userAnswersDto;
        }

    }
}
