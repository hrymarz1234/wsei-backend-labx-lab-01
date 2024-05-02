using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class QuizEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ISet<QuizItemEntity> Items { get; set; } = new HashSet<QuizItemEntity>();
    }
}
