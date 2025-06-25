namespace NewsApi.Dtos
{
    public class Insight
    {
        public string Ticker { get; set; } = null!;
        public string Sentiment { get; set; } = null!;
        public string SentimentReasoning { get; set; } = null!;
    }
}
