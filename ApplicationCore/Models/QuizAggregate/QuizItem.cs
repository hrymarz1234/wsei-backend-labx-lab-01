using System.Collections;
using System.Collections.Specialized;
using ApplicationCore.Interfaces.Repository;
using Microsoft.VisualBasic;

namespace ApplicationCore.Models.QuizAggregate;

public class QuizItem : IIdentity<int>
{
    public int Id { get; set; }
    public string Question { get; init; } = "";
    public List<string> IncorrectAnswers { get; init; } = new();
    public string CorrectAnswer { get; init; } = "";
}