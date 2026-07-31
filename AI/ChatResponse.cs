namespace OnlineBankingApplication.AI
{
    
    public class ChatResponse
    {
        public List<Choice> choices { get; set; } = new();
    }

    public class Choice
    {
        public ChatMessage message { get; set; } = new();
    }
}