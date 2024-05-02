using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class QuizItemEntity
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public ISet<QuizEntity> Quizzes { get; set; } = new HashSet<QuizEntity>();
        public string CorrectAnswer { get; set; }
        public ISet<QuizItemAnswerEntity> IncorrectAnswers { get; set; } = new HashSet<QuizItemAnswerEntity>();
    }
}
