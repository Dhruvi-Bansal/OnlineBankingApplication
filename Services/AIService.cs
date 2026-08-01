using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OnlineBankingApplication.AI;

namespace OnlineBankingApplication.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly AISettings _settings;

        public AIService(HttpClient httpClient,
                         IOptions<AISettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

      
        public async Task<string> AskAI(string applicationContext, string userQuestion)
        {
            
            string systemPrompt = @"
You are an intelligent AI Banking Assistant.

Follow the application context exactly.

Do not invent features that are not mentioned.

Always answer professionally and concisely.

If the customer asks about a feature that is not available,
politely inform them that it is not implemented in this version.

If the customer asks for personal banking information that is
not provided in the application context, politely tell them to
check the relevant section of the Secure Online Banking System
or contact customer support.

Never mention OTP, SMS verification, UPI, loans,
credit cards, or any feature unless it exists
in the application context.
";

            string finalPrompt = $@"
Application Context

{applicationContext}

------------------------------------

Customer Question

{userQuestion}

------------------------------------

Instructions

- Answer only according to the application context.
- Do not make assumptions.
- If information is unavailable, politely say so.
";

          
            var request = new ChatRequest
            {
                model = _settings.Model,

                messages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        role = "system",
                        content = systemPrompt
                    },

                    new ChatMessage
                    {
                        role = "user",
                        content = finalPrompt
                    }
                },

                temperature = 0.2,

                max_tokens = 500
            };

            var json = JsonSerializer.Serialize(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _settings.ApiKey);

            HttpResponseMessage response =
                await _httpClient.PostAsync(
                    _settings.BaseUrl,
                    content);

            if (!response.IsSuccessStatusCode)
            {
                return "Unable to contact the AI assistant at the moment. Please try again later.";
            }

           
            string responseJson =
                await response.Content.ReadAsStringAsync();

            var aiResponse =
                JsonSerializer.Deserialize<ChatResponse>(responseJson);

            return aiResponse?
                .choices?
                .FirstOrDefault()?
                .message?
                .content
                ?.Trim()
                ?? "Sorry, I couldn't generate a response.";
        }
    }
}