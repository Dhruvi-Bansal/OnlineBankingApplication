namespace OnlineBankingApplication.AI
{

    public class ChatRequest
    {
        public string model { get; set; } = string.Empty;

        public List<ChatMessage> messages { get; set; } = new();

        public double temperature { get; set; } = 0.1;

        public int max_tokens { get; set; } = 500;
    }
}