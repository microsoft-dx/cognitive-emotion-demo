using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EmotionDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;

namespace EmotionDemo.Controllers
{
    public class DemoSentimentAnalyzeController : Controller
    {
        private readonly IConfiguration configuration;
        private int _requestCount = 0;

        public IActionResult Index()
        {

            return RedirectToAction("SentimentAnalyzeResult", new SentimentAnalyzeModel { TextStr = "I am so happy about this demo!"});
        }


        public DemoSentimentAnalyzeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IActionResult> SentimentAnalyzeResult(SentimentAnalyzeModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.TextStr))
            {
                ITextAnalyticsClient client = this.GetTextAnalyzeClient(new MyHandler());
                model.EmotionResult = await client.SentimentAsync(true,
                    new MultiLanguageBatchInput(new List<MultiLanguageInput> { new MultiLanguageInput(null, $"{_requestCount++}", model.TextStr) }));
            }
            return View(model);
        }

        [HttpPost("Analyze")]
        public IActionResult Analyze(SentimentAnalyzeModel model)
        {
            return RedirectToAction("SentimentAnalyzeResult", model);
        }

        // Using the ServiceKey from the configuration file,
        // get an instance of the Text Analytics client.
        private ITextAnalyticsClient GetTextAnalyzeClient(DelegatingHandler handler)
        {

            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials(configuration))
            {
                Endpoint = configuration.GetSection("CognitiveServices")["TextAnalytics:ServiceBaseUrl"]
            };

            return client;
        }
    }

    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        IConfiguration _configuration;

        public ApiKeyServiceClientCredentials(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.GetSection("CognitiveServices")["TextAnalytics:ServiceKey"]);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }

    class MyHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}