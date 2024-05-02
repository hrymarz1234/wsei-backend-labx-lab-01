using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.EF.Mappers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Quiz, QuizEntity>()
                 .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<QuizEntity, Quiz>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<QuizItem, QuizItemEntity>()
                .ForMember(dest => dest.IncorrectAnswers, opt => opt.MapFrom(src => src.IncorrectAnswers.Select(answer => new QuizItemAnswerEntity { Answer = answer })));

            CreateMap<QuizItemEntity, QuizItem>()
                .ForMember(dest => dest.IncorrectAnswers, opt => opt.MapFrom(src => src.IncorrectAnswers.Select(answer => answer.Answer)));

            CreateMap<QuizItemAnswerEntity, string>()
                .ConvertUsing(src => src.Answer);

            CreateMap<string, QuizItemAnswerEntity>()
                .ForMember(dest => dest.QuizItems, opt => opt.Ignore());
        }
    }
}
