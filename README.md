

# ASP .NET - Working with Cognitive Services


## Prerequisites

 - An Azure subscription. If you do not have one, you can sign up for a
   [free account](https://azure.microsoft.com/pricing/free-trial/).
 - Visual Studio 2017+, with the Web Development workload
   installed. [Download it now](https://aka.ms/vsdownload?utm_source=mscom&amp;utm_campaign=msdocs).



## Install the Cognitive Services VSIX Extension[](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/vs-text-connected-service#install-the-cognitive-services-vsix-extension)

1.  With your web project open in Visual Studio, choose the  **Connected Services**  tab. The tab is available on the welcome page that appears when you open a new project. If you don't see the tab, select  **Connected Services**  in your project in Solution Explorer.
    
    ![Screenshot of Connected Services tab](https://docs.microsoft.com/en-us/azure/includes/media/vs-install-cognitive-services-vsix/connected-services-tab.png)
    
2.  Scroll down to the bottom of the list of services, and select  **Find more services**.
    
    ![Screenshot of Find more services link](https://docs.microsoft.com/en-us/azure/includes/media/vs-install-cognitive-services-vsix/find-more-services.png)
    
    The  **Extensions and Updates**  dialog box appears.
    
3.  In the  **Extensions and Updates**  dialog box, search for  **Cognitive Services**, and then download and install the Cognitive Services VSIX package.
    
    ![Screenshot of Extensions and Updates dialog box](https://docs.microsoft.com/en-us/azure/includes/media/vs-install-cognitive-services-vsix/install-cognitive-services-vsix.png)
    
    Installing an extension requires a restart of the integrated development environment (IDE).
    
4.  Restart Visual Studio. The extension installs when you close Visual Studio, and is available next time you launch the IDE.


## Add support to your project for the Text Analytics Service[](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/vs-text-connected-service#add-support-to-your-project-for-the-text-analytics-service)

1.  Create a new ASP.NET Core web project called TextAnalyticsDemo. Use the Web Application (Model-View-Controller) project template with all the default settings. It’s important to name the project MyWebApplication, so the namespace matches when you copy code into the project. The example in this articles uses MVC, but you can use the Text Analytics Connected Service with any ASP.NET project type.
    
2.  In  **Solution Explorer**, double-click on the  **Connected Service**  item. The Connected Service page appears, with services you can add to your project.
    
    ![Screenshot of Connected Service in Solution Explorer](https://docs.microsoft.com/en-us/azure/cognitive-services/media/vs-common/connected-services-solution-explorer.png)
    
3.  In the menu of available services, choose  **Evaluate Sentiment with Text Analytics**.
    
    ![Screenshot of Connected Services screen](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/media/vs-text-connected-service/cog-text-connected-service-0.png)
    
    If you've signed into Visual Studio, and have an Azure subscription associated with your account, a page appears with a dropdown list with your subscriptions.
    
    ![Screenshot of Text Analytics Connected Service screen](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/media/vs-text-connected-service/cog-text-connected-service-1.png)
    
4.  Select the subscription you want to use, and then choose a name for the Text Analytics Service, or choose the  **Edit**  link to modify the automatically generated name, choose the resource group, and the Pricing Tier.
    
    ![Screenshot of resource group and pricing tier fields](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/media/vs-text-connected-service/cog-text-connected-service-2.png)
    
    Follow the link for details on the pricing tiers.
    
5.  Choose  **Add**  to add support for the Connected Service. Visual Studio modifies your project to add the NuGet packages, configuration file entries, and other changes to support a connection to the Text Analytics Service. The  **Output Window**shows the log of what is happening to your project. You should see something like the following:
    
    
    ```
     [6/1/2018 3:04:02.347 PM] Adding Text Analytics to the project.
     [6/1/2018 3:04:02.906 PM] Creating new Text Analytics...
     [6/1/2018 3:04:06.314 PM] Installing NuGet package 'Microsoft.Azure.CognitiveServices.Language' version 1.0.0-preview...
     [6/1/2018 3:04:56.759 PM] Retrieving keys...
     [6/1/2018 3:04:57.822 PM] Updating appsettings.json setting: 'ServiceKey' = '&lt;service key=""&gt;'
     [6/1/2018 3:04:57.827 PM] Updating appsettings.json setting: 'ServiceEndPoint' = 'https://westus.api.cognitive.microsoft.com/text/analytics/v2.0'
     [6/1/2018 3:04:57.832 PM] Updating appsettings.json setting: 'Name' = 'TextAnalyticsDemo'
     [6/1/2018 3:05:01.840 PM] Successfully added Text Analytics to the project.
    
    ```


## Install the NuGet SDK Package[](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/quickstarts/csharp#install-the-nuget-sdk-package)

1. Right click on the solution and click  **Manage NuGet Packages for Solution** or on the Project and click **Manage NuGet Packages**
2. Select the **Browse**  tab, and Search for  **Microsoft.Azure.CognitiveServices.Language.TextAnalytics** 
3. Select the NuGet package and install it.


## Use the Text Analytics Service to detect the Emition for a text sample[](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/vs-text-connected-service#use-the-text-analytics-service-to-detect-the-language-for-a-text-sample)

  
1.  Add a class file in the Controllers folder called DemoSentimentAnalyzeController and replace its contents with the following code:

```
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

        public async Task&lt;iactionresult&gt; SentimentAnalyzeResult(SentimentAnalyzeModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.TextStr))
            {
                ITextAnalyticsClient client = this.GetTextAnalyzeClient(new MyHandler());
                model.EmotionResult = await client.SentimentAsync(true,
                    new MultiLanguageBatchInput(new List&lt;multilanguageinput&gt; { new MultiLanguageInput(null, $"{_requestCount++}", model.TextStr) }));
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
}
```
    
The code includes GetTextAnalyzeClient to get the client object which you can use to call the Text Analytics API, and a request handler that calls SentimentAsync on a given text.
    
2.  Add the MyHandler helper class which is used by the preceding code.   
    
```
        class MyHandler : DelegatingHandler
        {
            protected async override Task&lt;httpresponsemessage&gt; SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // Call the inner handler.
                var response = await base.SendAsync(request, cancellationToken);
    
                return response;
            }
        }
  ```
    
3. Add the ApiKeyServiceClientCredentials  class that is also used by the initial code
```
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
```

4.  In the Models folder, add a class for the model.
```
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace EmotionDemo.Models
{
    public class SentimentAnalyzeModel
    {        
        public string TextStr { get; set; }
        public string SentimentValue { get; set; } = "100%";
        public string SentimentText { get; set; } = "Neutral";
        public string SentimentType { get; set; } = "neutral";

        public SentimentBatchResult EmotionResult { get; set; }
    }
}
```
    
5.  Add a View to show the analyzed text, the language determined, and the score that represents the confidence level in the analysis. To do this, right-click on the  **Views**  folder, choose  **Add**, then  **View**. In the dialog box that appears, provide a name (We used _SentimentAnalyzeResult_), accept the defaults to add a new file called _SentimentAnalyzeResult.cshtml_  in the  **Views**  folder and copy the following contents into it:       
 
 ```     
@using System
@model EmotionDemo.Models.SentimentAnalyzeModel

@{
    ViewData["Title"] = "SentimentAnalyzeResult";
    double sentimentScore = Model.EmotionResult.Documents[0].Score ?? 0.5;

    if (sentimentScore <= 0.3)
    {
        Model.SentimentValue = ((0.3 - sentimentScore) / 0.3).ToString("P2");
        Model.SentimentType = "negative";
        Model.SentimentText = "Negative";
    }
    else
    {
        if (sentimentScore >= 0.7)
        {
            Model.SentimentValue = ((sentimentScore - 0.7) / 0.3).ToString("P2");
            Model.SentimentType = "positive";
            Model.SentimentText = "Positive";
        }
        else
        {
            if (sentimentScore <= 0.5)
            {
                Model.SentimentValue = (1 - ((0.5 - sentimentScore) / 0.2)).ToString("P2");
            }
            else
            {
                Model.SentimentValue = (1 - ((sentimentScore - 0.5) / 0.2)).ToString("P2");
            }
            Model.SentimentType = "neutral";
            Model.SentimentText = "Neutral";
        }
    }
}

<h2>Sentiment Analyze</h2>

<div class="row">
    <section>
        <form asp-controller="DemoSentimentAnalyze" asp-action="Analyze" method="POST"
              class="form-horizontal" enctype="multipart/form-data">
            <table>
                <tr>
                    <td>
                        <input type="text" name="TextStr" class="form-control" />
                    </td>
                    <td>
                        <button type="submit" class="btn btn-default">Analyze</button>
                    </td>
                </tr>
            </table>
        </form>
    </section>
</div>

<h2>Result</h2>
<div>
    <dl class="dl-horizontal">
        <dt>
            Text :
        </dt>
        <dd>
            @Html.DisplayFor(model => model.TextStr)
        </dd>
        <dt>
            Sentiment Type :
        </dt>
        <dd>
            @Model.SentimentText
        </dd>
        <dt>
            Sentiment Score :
        </dt>
        <dd>
            @Model.SentimentValue
        </dd>
    </dl>
    <div class="container">
        <div class="sentiment @Model.SentimentType" style="width:@Model.SentimentValue">
            @Model.SentimentValue
        </div>
    </div>
</div>
<div>
    <hr />
    <p>
        <a asp-controller="Home" asp-action="Index">Return to Index</a>
    </p>
</div>

<style>
    /* Make sure that padding behaves as expected */
    * {
        box-sizing: border-box
    }

    /* Container for skill bars */
    .container {
        width: 100%; /* Full width */
        background-color: #ddd; /* Grey background */
    }

    .sentiment {
        text-align: right; /* Right-align text */
        padding: 10px; /* Add some padding */
        color: white; /* White text color */
    }

    .positive {
        background-color: #4CAF50;
    }
    /* Green */
    .neutral {
        background-color: #2196F3;
    }
    /* Blue */
    .negative {
        background-color: #f44336;
    }
    /* Red */
</style>
```
    
6.  Build and run the example locally. Enter some text and see what  Sentiment Text Analytics detects.

# Sources
 - Base Tutorial: [Microsoft Docs](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/vs-text-connected-service)

