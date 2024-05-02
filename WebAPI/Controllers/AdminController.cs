using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using BackendLab01;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApIa.DTO;

namespace WebApIa.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IQuizAdminService _adminservice;
        private readonly IMapper _mapper;

        public AdminController(IQuizAdminService adminservice, IMapper mapper)
        {
            _adminservice = adminservice;
            _mapper = mapper;   
        }
        [HttpPost]
        public ActionResult<object> AddQuiz(LinkGenerator link, NewQuizDto dto)
        {
            var quiz = _adminservice.AddQuiz(_mapper.Map<Quiz>(dto));
            return Created(
                link.GetPathByAction(HttpContext, nameof(GetQuiz), null, new { quiId = quiz.Id }),
                quiz
            );
        }

        [HttpGet]
        [Route("{quizId}")]
        public ActionResult<Quiz> GetQuiz(int quizId)
        {
            var quiz = _adminservice.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            return quiz is null ? NotFound() : quiz;
        }
        [HttpPatch]
        [Route("{quizId}")]
        [Consumes("application/json-patch+json")]
        public ActionResult<Quiz> AddQuizItem(int quizId, JsonPatchDocument<Quiz>? patchDoc)
        {
            var quiz = _adminservice.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            if (quiz is null || patchDoc is null)
            {
                return NotFound(new
                {
                    error = $"Quiz width id {quizId} not found"
                });
            }
            
            int previousCount = quiz.Items.Count;
            patchDoc.ApplyTo(quiz, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (previousCount < quiz.Items.Count)
            {
                QuizItem item = quiz.Items[^1];
                quiz.Items.RemoveAt(quiz.Items.Count - 1);
                _adminservice.AddQuizItemToQuiz(quizId, item);
            }
            return Ok(_adminservice.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
        }
    }
}
    

