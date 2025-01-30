using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using XUnity.AutoTranslator.Plugin.Core.Endpoints;
using XUnity.AutoTranslator.Plugin.Core.Endpoints.Http;
using XUnity.AutoTranslator.Plugin.Core.Web;

internal class DeepSeekTranslatorEndpoint : HttpEndpoint
{
    private string? _apiKey;
    private string? _model;
    private string? _prompt;
    private const string _url = "https://api.deepseek.com/chat/completions";

    public override string Id => "DeepSeekTranslate";
    public override string FriendlyName => "DeepSeek Translate";
    public override int MaxTranslationsPerRequest => 1;
    public override int MaxConcurrency => 15;

    public override void Initialize(IInitializationContext context)
    {
        _apiKey = context.GetOrCreateSetting("DeepSeek", "APIKey", "");
        _model = context.GetOrCreateSetting("DeepSeek", "Model", "deepseek-chat");
        _prompt = context.GetOrCreateSetting("DeepSeek", "Prompt", "You are a professional translator. Translate the following text to English accurately. Maintain original context and nuance. Keep character names original.");

        // Remove artificial delays
        context.SetTranslationDelay(0.1f);
        context.DisableSpamChecks();

        if (string.IsNullOrEmpty(_apiKey))
            throw new Exception("The DeepSeek endpoint requires an API key which has not been provided.");
    }

    public override void OnCreateRequest(IHttpRequestCreationContext context)
    {
        var requestData = GetRequestData(context);

        var request = new XUnityWebRequest("POST", _url, requestData);
        request.Headers[HttpRequestHeader.Authorization] = $"Bearer {_apiKey}";
        request.Headers[HttpRequestHeader.ContentType] = "application/json";

        context.Complete(request);
    }

    private string GetRequestData(IHttpRequestCreationContext context)
    {
        var messages = new List<object>
        {
            new { role = "system", content = _prompt }
        };

        foreach (var text in context.UntranslatedTexts)
        {
            messages.Add(new { role = "user", content = text });
        }

        var requestBody = new
        {
            model = _model,
            // temperature = 0.1,
            max_tokens = 1000,
            // top_p = 1,
            // frequency_penalty = 0,
            // presence_penalty = 0,
            messages
        };

        return JsonConvert.SerializeObject(requestBody);
    }

    public override void OnExtractTranslation(IHttpTranslationExtractionContext context)
    {
        var data = context.Response.Data;

        var jsonResponse = JObject.Parse(data);
        if (MaxTranslationsPerRequest == 1)
            context.Complete(GetTranslatedText(jsonResponse, 0));
    }

    private static string GetTranslatedText(JObject jsonResponse, int index)
    {
        var rawString = jsonResponse["choices"]?[index]?["message"]?["content"]?.ToString() ?? string.Empty;
        return rawString.Trim();
    }
}