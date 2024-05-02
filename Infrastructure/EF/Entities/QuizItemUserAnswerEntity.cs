using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    [PrimaryKey(nameof(UserId), nameof(QuizId), nameof(QuizItemId))]
    public class QuizItemUserAnswerEntity
    {
        public int UserId { get; set; }
        public int QuizItemId { get; set; }
        public int QuizId { get; set; }
        public string UserAnswer { get; set; }
        public QuizItemEntity QuizItem { get; set; }
    }
}
