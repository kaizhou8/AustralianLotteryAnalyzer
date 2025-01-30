using System.Collections.Concurrent;

namespace LottoAnalyzer
{
    /// <summary>
    /// Represents statistical information about a lottery number
    /// </summary>
    public class NumberStats
    {
        /// <summary>
        /// The lottery number
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// The frequency of the number in historical draws
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// The date of the last appearance of the number
        /// </summary>
        public DateTime LastAppearance { get; set; }
        /// <summary>
        /// The probability of the number appearing in a draw
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// Whether the number is considered 'hot' (high frequency)
        /// </summary>
        public bool IsHot => Probability > 0.15; // 频率高于15%认为是热门号码
        /// <summary>
        /// Whether the number is considered 'due' (has not appeared recently)
        /// </summary>
        public bool IsDue => (DateTime.Now - LastAppearance).TotalDays > 60; // 60天未出现认为是应该出现的号码
    }

    /// <summary>
    /// Performs statistical analysis and generates predictions for lottery games
    /// </summary>
    public class LottoAnalyzer
    {
        private readonly List<LottoResult> _historicalResults;
        private readonly Dictionary<int, NumberStats> _numberStats;
        private const int MaxNumber = 45; // 澳洲Saturday Lotto的最大号码
        private const int NumbersPerDraw = 6; // 每次抽取的号码数

        /// <summary>
        /// Initializes a new instance of the LottoAnalyzer
        /// </summary>
        /// <param name="historicalResults">Historical lottery results to analyze</param>
        public LottoAnalyzer(List<LottoResult> historicalResults)
        {
            _historicalResults = historicalResults;
            _numberStats = CalculateNumberStats();
        }

        /// <summary>
        /// Calculates statistical information for each lottery number
        /// </summary>
        /// <returns>Dictionary mapping numbers to their statistical information</returns>
        private Dictionary<int, NumberStats> CalculateNumberStats()
        {
            var stats = new Dictionary<int, NumberStats>();
            var totalDraws = _historicalResults.Count;

            // 初始化统计
            for (int i = 1; i <= MaxNumber; i++)
            {
                stats[i] = new NumberStats { Number = i, Frequency = 0, LastAppearance = DateTime.MinValue };
            }

            // 计算频率和最后出现时间
            foreach (var result in _historicalResults)
            {
                foreach (var number in result.WinningNumbers)
                {
                    stats[number].Frequency++;
                    if (result.DrawDate > stats[number].LastAppearance)
                        stats[number].LastAppearance = result.DrawDate;
                }
            }

            // 计算概率
            foreach (var stat in stats.Values)
            {
                stat.Probability = (double)stat.Frequency / totalDraws;
            }

            return stats;
        }

        /// <summary>
        /// Generates recommended numbers for the next draw
        /// </summary>
        /// <param name="count">The number of recommended numbers to generate</param>
        /// <returns>List of recommended numbers</returns>
        public List<int> GetRecommendedNumbers(int count = 6)
        {
            var recommended = new List<int>();
            var random = new Random();

            // 策略1：选择2个热门号码
            var hotNumbers = _numberStats.Values
                .Where(s => s.IsHot)
                .OrderByDescending(s => s.Probability)
                .Take(2)
                .Select(s => s.Number)
                .ToList();
            recommended.AddRange(hotNumbers);

            // 策略2：选择2个应该出现的号码
            var dueNumbers = _numberStats.Values
                .Where(s => s.IsDue && !recommended.Contains(s.Number))
                .OrderByDescending(s => DateTime.Now - s.LastAppearance)
                .Take(2)
                .Select(s => s.Number)
                .ToList();
            recommended.AddRange(dueNumbers);

            // 策略3：随机选择剩余号码
            while (recommended.Count < count)
            {
                var number = random.Next(1, MaxNumber + 1);
                if (!recommended.Contains(number))
                {
                    recommended.Add(number);
                }
            }

            return recommended.OrderBy(n => n).ToList();
        }

        /// <summary>
        /// Gets the top numbers by frequency
        /// </summary>
        /// <param name="count">The number of top numbers to return</param>
        /// <returns>List of top numbers</returns>
        public List<NumberStats> GetTopNumbers(int count = 10)
        {
            return _numberStats.Values
                .OrderByDescending(s => s.Probability)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Gets the least drawn numbers
        /// </summary>
        /// <param name="count">The number of least drawn numbers to return</param>
        /// <returns>List of least drawn numbers</returns>
        public List<NumberStats> GetLeastDrawnNumbers(int count = 10)
        {
            return _numberStats.Values
                .OrderBy(s => s.Probability)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Analyzes pairs of numbers
        /// </summary>
        /// <returns>Dictionary mapping pairs to their frequency</returns>
        public Dictionary<int, List<int>> AnalyzePairs()
        {
            var pairs = new ConcurrentDictionary<(int, int), int>();

            // 分析所有号码对
            Parallel.ForEach(_historicalResults, result =>
            {
                var numbers = result.WinningNumbers;
                for (int i = 0; i < numbers.Count; i++)
                {
                    for (int j = i + 1; j < numbers.Count; j++)
                    {
                        var pair = (Math.Min(numbers[i], numbers[j]), Math.Max(numbers[i], numbers[j]));
                        pairs.AddOrUpdate(pair, 1, (key, oldValue) => oldValue + 1);
                    }
                }
            });

            // 找出每个号码最常出现的搭配
            var commonPairs = new Dictionary<int, List<int>>();
            for (int i = 1; i <= MaxNumber; i++)
            {
                var numberPairs = pairs.Where(p => p.Key.Item1 == i || p.Key.Item2 == i)
                                     .OrderByDescending(p => p.Value)
                                     .Take(3)
                                     .Select(p => p.Key.Item1 == i ? p.Key.Item2 : p.Key.Item1)
                                     .ToList();
                commonPairs[i] = numberPairs;
            }

            return commonPairs;
        }

        /// <summary>
        /// Prints statistical information to the console
        /// </summary>
        public void PrintStatistics()
        {
            Console.WriteLine("\n=== Lotto Statistics ===");
            Console.WriteLine($"Total Draws Analyzed: {_historicalResults.Count}");
            Console.WriteLine($"Date Range: {_historicalResults.Min(r => r.DrawDate):d} to {_historicalResults.Max(r => r.DrawDate):d}");

            Console.WriteLine("\nTop 10 Most Frequent Numbers:");
            foreach (var stat in GetTopNumbers(10))
            {
                Console.WriteLine($"Number {stat.Number}: {stat.Frequency} times ({stat.Probability:P2})");
            }

            Console.WriteLine("\nTop 10 Least Frequent Numbers:");
            foreach (var stat in GetLeastDrawnNumbers(10))
            {
                Console.WriteLine($"Number {stat.Number}: {stat.Frequency} times ({stat.Probability:P2})");
            }

            Console.WriteLine("\nRecommended Numbers for Next Draw:");
            var recommended = GetRecommendedNumbers();
            Console.WriteLine(string.Join(", ", recommended));
        }
    }
}
