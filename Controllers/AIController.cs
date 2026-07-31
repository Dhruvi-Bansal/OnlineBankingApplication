using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Services;
using OnlineBankingApplication.AI;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AIController : Controller
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

    
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Ask(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return Json(new
                {
                    response = "Please enter a question."
                });
            }


            string applicationContext = AIApplicationContext.Context;

            string answer = await _aiService.AskAI(applicationContext, question);

            return Json(new
            {
                response = answer
            });
        }
    }
}