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
