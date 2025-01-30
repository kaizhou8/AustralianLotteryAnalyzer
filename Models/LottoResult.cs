namespace LottoAnalyzer.Models
{
    /// <summary>
    /// Represents a single lottery draw result
    /// </summary>
    public class LottoResult
    {
        /// <summary>
        /// Type of lottery game
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Date and time when the draw occurred
        /// </summary>
        public DateTime DrawDate { get; set; }

        /// <summary>
        /// Draw number/identifier
        /// </summary>
        public int DrawNumber { get; set; }

        /// <summary>
        /// Main winning numbers drawn
        /// </summary>
        public List<int> WinningNumbers { get; set; } = new();

        /// <summary>
        /// Supplementary numbers (if applicable)
        /// </summary>
        public List<int> SupplementaryNumbers { get; set; } = new();

        /// <summary>
        /// Powerball number (if applicable)
        /// </summary>
        public int? PowerballNumber { get; set; }

        /// <summary>
        /// Division 1 prize pool amount in AUD
        /// </summary>
        public decimal Division1Prize { get; set; }

        /// <summary>
        /// Number of Division 1 winners
        /// </summary>
        public int Division1Winners { get; set; }

        /// <summary>
        /// Indicates whether there were any Division 1 winners
        /// </summary>
        public bool HasWinners => Division1Winners > 0;
    }

    /// <summary>
    /// Contains statistical analysis of draw history
    /// </summary>
    public class DrawStatistics
    {
        /// <summary>
        /// Type of lottery game analyzed
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Date of the most recent draw analyzed
        /// </summary>
        public DateTime LastDrawDate { get; set; }

        /// <summary>
        /// Average Division 1 prize amount over analyzed period
        /// </summary>
        public decimal AverageDivision1Prize { get; set; }

        /// <summary>
        /// Highest Division 1 prize recorded in analyzed period
        /// </summary>
        public decimal HighestDivision1Prize { get; set; }

        /// <summary>
        /// Date when the highest prize was awarded
        /// </summary>
        public DateTime HighestPrizeDate { get; set; }

        /// <summary>
        /// Total number of draws analyzed
        /// </summary>
        public int TotalDraws { get; set; }

        /// <summary>
        /// Total number of Division 1 winners in analyzed period
        /// </summary>
        public int TotalDivision1Winners { get; set; }

        /// <summary>
        /// Frequency distribution of drawn numbers
        /// </summary>
        public Dictionary<int, int> NumberFrequency { get; set; } = new();

        /// <summary>
        /// Frequency distribution of Powerball numbers (if applicable)
        /// </summary>
        public Dictionary<int, int> PowerballFrequency { get; set; } = new();

        /// <summary>
        /// Most common number pairs that appear together
        /// </summary>
        public List<(int Number1, int Number2, int Frequency)> CommonPairs { get; set; } = new();
    }

    /// <summary>
    /// Analyzes probability and patterns for individual numbers
    /// </summary>
    public class NumberProbability
    {
        /// <summary>
        /// The number being analyzed
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// How many times this number has been drawn
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// When this number was last drawn
        /// </summary>
        public DateTime LastAppearance { get; set; }

        /// <summary>
        /// Probability of this number being drawn (Frequency/TotalDraws)
        /// </summary>
        public double Probability { get; set; }

        /// <summary>
        /// Indicates if this is a frequently drawn number (>15% probability)
        /// </summary>
        public bool IsHot => Probability > 0.15;

        /// <summary>
        /// Indicates if this number is overdue (not drawn for >60 days)
        /// </summary>
        public bool IsDue => (DateTime.Now - LastAppearance).TotalDays > 60;

        /// <summary>
        /// Numbers that frequently appear together with this number
        /// </summary>
        public List<int> CommonCompanions { get; set; } = new();
    }

    /// <summary>
    /// Represents predictions and recommendations for future draws
    /// </summary>
    public class LottoPrediction
    {
        /// <summary>
        /// Type of lottery game being predicted
        /// </summary>
        public LottoType GameType { get; set; }

        /// <summary>
        /// Date and time of the next draw
        /// </summary>
        public DateTime NextDrawDate { get; set; }

        /// <summary>
        /// Recommended numbers to play
        /// </summary>
        public List<int> RecommendedNumbers { get; set; } = new();

        /// <summary>
        /// Recommended Powerball number (if applicable)
        /// </summary>
        public int? RecommendedPowerball { get; set; }

        /// <summary>
        /// Explanations for why these numbers were recommended
        /// </summary>
        public List<string> Reasoning { get; set; } = new();

        /// <summary>
        /// Estimated Division 1 prize for next draw
        /// </summary>
        public decimal EstimatedDivision1Prize { get; set; }

        /// <summary>
        /// Confidence level for each recommended number (0-1)
        /// </summary>
        public Dictionary<int, double> NumberConfidence { get; set; } = new();
    }
}
