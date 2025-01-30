namespace LottoAnalyzer.Models
{
    /// <summary>
    /// View model for the game analysis page
    /// Contains all necessary data for displaying lottery game analysis
    /// </summary>
    public class GameAnalysisViewModel
    {
        /// <summary>
        /// Type of lottery game being analyzed
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Rules and configuration for the lottery game
        /// </summary>
        public LottoGameRules GameRules { get; set; }

        /// <summary>
        /// Statistical analysis of historical draw data
        /// </summary>
        public DrawStatistics Statistics { get; set; }

        /// <summary>
        /// Predictions for the next draw
        /// </summary>
        public LottoPrediction Prediction { get; set; }

        /// <summary>
        /// List of most recent draw results
        /// </summary>
        public List<LottoResult> LastResults { get; set; }
    }

    /// <summary>
    /// View model for displaying historical data analysis
    /// </summary>
    public class HistoricalDataViewModel
    {
        /// <summary>
        /// Type of lottery game
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Start date of the analysis period
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the analysis period
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// List of all draw results in the period
        /// </summary>
        public List<LottoResult> Results { get; set; }

        /// <summary>
        /// Statistical analysis for the period
        /// </summary>
        public DrawStatistics Statistics { get; set; }
    }

    /// <summary>
    /// View model for displaying number frequency charts
    /// </summary>
    public class NumberFrequencyViewModel
    {
        /// <summary>
        /// Type of lottery game
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Frequency data for main numbers
        /// </summary>
        public Dictionary<int, int> MainNumberFrequency { get; set; }

        /// <summary>
        /// Frequency data for supplementary numbers
        /// </summary>
        public Dictionary<int, int> SupplementaryFrequency { get; set; }

        /// <summary>
        /// Frequency data for Powerball numbers (if applicable)
        /// </summary>
        public Dictionary<int, int> PowerballFrequency { get; set; }

        /// <summary>
        /// Time period covered by the frequency analysis
        /// </summary>
        public string TimePeriod { get; set; }
    }

    /// <summary>
    /// View model for displaying prize trend analysis
    /// </summary>
    public class PrizeTrendViewModel
    {
        /// <summary>
        /// Type of lottery game
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Historical Division 1 prize amounts
        /// </summary>
        public List<(DateTime DrawDate, decimal PrizeAmount)> PrizeHistory { get; set; }

        /// <summary>
        /// Average Division 1 prize amount
        /// </summary>
        public decimal AveragePrize { get; set; }

        /// <summary>
        /// Highest recorded Division 1 prize
        /// </summary>
        public decimal HighestPrize { get; set; }

        /// <summary>
        /// Date when the highest prize was awarded
        /// </summary>
        public DateTime HighestPrizeDate { get; set; }

        /// <summary>
        /// Trend analysis of prize amounts
        /// </summary>
        public string TrendAnalysis { get; set; }
    }
}
