using AIEnterpriseCommandCenter.Application.DTOs.AI;
using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace AIEnterpriseCommandCenter.Web.Controllers;

public class AIController : Controller
{
    private readonly IAIService _aiService;
    private readonly IAIToolsService _tools;
    private readonly IAITicketAssistant _ticketAssistant; 
    public AIController(
        IAIService aiService,
        IAIToolsService tools,
        IAITicketAssistant ticketAssistant)
    {
        _aiService = aiService;
        _tools = tools;
        _ticketAssistant = ticketAssistant;
    }


    public IActionResult Index()
    {
        return View("Chat");
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        var history = new List<string>
    {
"You are AI Enterprise Command Center Assistant. Introduce yourself in two professional sentences."    };

        var response = await _aiService.AskAsync(history);

        return Content(response);
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] ChatRequestDto request)
    {

        request.Prompt = request.Prompt?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return Json(new ChatResponseDto
            {
                Response = "Please enter your question."
            });
        } 

        // 1. Check Enterprise Tools first
        var toolAnswer = await _tools.ExecuteAsync(request.Prompt);

        if (!string.IsNullOrEmpty(toolAnswer))
        {
            return Json(new ChatResponseDto
            {
                Response = toolAnswer
            });
        }

        // 2. Otherwise ask groq ai
        var historyJson = HttpContext.Session.GetString("ChatHistory");

        List<string> history;

        if (string.IsNullOrEmpty(historyJson))
        {
            history = new List<string>();
        }
        else
        {
            history = JsonSerializer.Deserialize<List<string>>(historyJson)!;
        }

        history.Add("User: " + request.Prompt);

        string answer;

        try
        {
            answer = await _aiService.AskAsync(history);
        }
        catch
        {
            answer = "Sorry, the AI service is temporarily unavailable.";
        }
        history.Add("AI: " + answer);
        const int MaxMessages = 20;

        if (history.Count > MaxMessages)
        {
            history = history.Skip(history.Count - MaxMessages).ToList();
        }

        HttpContext.Session.SetString(
            "ChatHistory",
            JsonSerializer.Serialize(history));

        return Json(new ChatResponseDto
        {
            Response = answer
        });
    }

    [HttpPost]
    public async Task<IActionResult> AnalyzeTicket([FromBody] TicketRequestDto request)
    {
        var result = await _ticketAssistant.AnalyzeAsync(request.Issue);

        return Json(result);
    }

    [HttpPost]
    public IActionResult ClearChat()
    {
        HttpContext.Session.Remove("ChatHistory");

        return Ok();
    }
}