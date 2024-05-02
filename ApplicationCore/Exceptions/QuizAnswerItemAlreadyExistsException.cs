using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Exceptions
{
    public class QuizAnswerItemAlreadyExistsException : Exception
    {
        private int UserId;
        private int QuizId;
        private int QuizItemId;

        public QuizAnswerItemAlreadyExistsException(int quizId, int quizItemId, int userId)
            : base($"Answer for quiz item {quizItemId} in quiz {quizId} by user {userId} already exists!")
        {
        }
    }
}
