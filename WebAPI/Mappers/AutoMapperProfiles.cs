using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using BackendLab01;
using WebAPI.Dto;
using WebApIa.DTO;

namespace WebAPI.Mappers
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<QuizItem, QuizItemDto>()
                .ForMember(
                    q => q.Options,
                    op => op.MapFrom(i => new List<string>(i.IncorrectAnswers) { i.CorrectAnswer }));
            CreateMap<Quiz, QuizDto>()
                .ForMember(
                    q => q.Items,
                    op => op.MapFrom<List<QuizItem>>(i => i.Items)
                );
            CreateMap<NewQuizDto, Quiz>();

            CreateMap<QuizItemUserAnswer, FeedbackDto>();
        }
    }
}
