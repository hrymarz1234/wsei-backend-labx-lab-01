namespace WebAPI.DTO
{
    public class NewQuizItemDto
    {
        public string Qestion { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }
    }
}
