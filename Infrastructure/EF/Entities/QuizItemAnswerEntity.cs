using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class QuizItemAnswerEntity
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public ISet<QuizItemEntity> QuizItems { get; set; } = new HashSet<QuizItemEntity>();
    }
}
