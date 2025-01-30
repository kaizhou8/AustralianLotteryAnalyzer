namespace LottoAnalyzer.Models
{
    /// <summary>
    /// Represents different types of Australian lottery games
    /// </summary>
    public enum LottoType
    {
        /// <summary>
        /// Monday Lotto (also known as Monday X Lotto in SA, Monday Gold Lotto in QLD)
        /// </summary>
        MondayLotto,     // Monday X Lotto (SA), Monday Gold Lotto (QLD)
        /// <summary>
        /// Wednesday Lotto (also known as Wednesday X Lotto in SA, Wednesday Gold Lotto in QLD)
        /// </summary>
        WednesdayLotto,  // Wednesday X Lotto (SA), Wednesday Gold Lotto (QLD)
        /// <summary>
        /// Saturday TattsLotto (also known as Saturday X Lotto, Saturday Gold Lotto)
        /// </summary>
        SaturdayLotto,   // Saturday TattsLotto, Saturday X Lotto, Saturday Gold Lotto
        /// <summary>
        /// Oz Lotto, drawn on Tuesdays
        /// </summary>
        OzLotto,         // Tuesday
        /// <summary>
        /// Powerball, drawn on Thursdays
        /// </summary>
        Powerball,       // Thursday
        /// <summary>
        /// Set for Life, drawn daily
        /// </summary>
        SetForLife,      // Daily
        /// <summary>
        /// Strike (NSW only, supplementary game)
        /// </summary>
        Strike           // NSW only, supplementary game
    }

    /// <summary>
    /// Defines the rules and characteristics of each lottery game
    /// </summary>
    public class LottoGameRules
    {
        /// <summary>
        /// The type of lottery game
        /// </summary>
        public LottoType Type { get; set; }

        /// <summary>
        /// The display name of the game
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Number of main numbers drawn in each game
        /// </summary>
        public int NumbersDrawn { get; set; }         // 主要号码数量

        /// <summary>
        /// Maximum number that can be drawn (range is 1 to MaxNumber)
        /// </summary>
        public int MaxNumber { get; set; }            // 主要号码范围

        /// <summary>
        /// Number of supplementary numbers drawn
        /// </summary>
        public int SupplementaryCount { get; set; }   // 补充号码数量

        /// <summary>
        /// Number of Powerball numbers (if applicable)
        /// </summary>
        public int? PowerballCount { get; set; }      // Powerball数量（如果有）

        /// <summary>
        /// Maximum Powerball number (range is 1 to PowerballMaxNumber)
        /// </summary>
        public int? PowerballMaxNumber { get; set; }  // Powerball号码范围

        /// <summary>
        /// Cost of a standard game in AUD
        /// </summary>
        public decimal StandardGameCost { get; set; } // 标准投注成本

        /// <summary>
        /// Day of the week when the draw occurs
        /// </summary>
        public DayOfWeek DrawDay { get; set; }        // 开奖日

        /// <summary>
        /// Time of day when the draw occurs (AEST/AEDT)
        /// </summary>
        public TimeSpan DrawTime { get; set; }        // 开奖时间

        /// <summary>
        /// Minimum guaranteed Division 1 prize pool in AUD
        /// </summary>
        public int MinimumDivision1Prize { get; set; }// 最低一等奖金额

        /// <summary>
        /// Gets a dictionary containing all supported lottery games and their rules
        /// </summary>
        /// <returns>Dictionary mapping LottoType to LottoGameRules</returns>
        public static Dictionary<LottoType, LottoGameRules> GetAllGames()
        {
            return new Dictionary<LottoType, LottoGameRules>
            {
                {
                    LottoType.MondayLotto,
                    new LottoGameRules
                    {
                        Type = LottoType.MondayLotto,
                        Name = "Monday Lotto",
                        NumbersDrawn = 6,
                        MaxNumber = 45,
                        SupplementaryCount = 2,
                        StandardGameCost = 0.60M,
                        DrawDay = DayOfWeek.Monday,
                        DrawTime = new TimeSpan(19, 30, 0), // 7:30 PM AEST
                        MinimumDivision1Prize = 1000000
                    }
                },
                {
                    LottoType.OzLotto,
                    new LottoGameRules
                    {
                        Type = LottoType.OzLotto,
                        Name = "Oz Lotto",
                        NumbersDrawn = 7,
                        MaxNumber = 47,
                        SupplementaryCount = 2,
                        StandardGameCost = 1.30M,
                        DrawDay = DayOfWeek.Tuesday,
                        DrawTime = new TimeSpan(19, 30, 0),
                        MinimumDivision1Prize = 2000000
                    }
                },
                {
                    LottoType.WednesdayLotto,
                    new LottoGameRules
                    {
                        Type = LottoType.WednesdayLotto,
                        Name = "Wednesday Lotto",
                        NumbersDrawn = 6,
                        MaxNumber = 45,
                        SupplementaryCount = 2,
                        StandardGameCost = 0.60M,
                        DrawDay = DayOfWeek.Wednesday,
                        DrawTime = new TimeSpan(19, 30, 0),
                        MinimumDivision1Prize = 1000000
                    }
                },
                {
                    LottoType.Powerball,
                    new LottoGameRules
                    {
                        Type = LottoType.Powerball,
                        Name = "Powerball",
                        NumbersDrawn = 7,
                        MaxNumber = 35,
                        PowerballCount = 1,
                        PowerballMaxNumber = 20,
                        StandardGameCost = 1.35M,
                        DrawDay = DayOfWeek.Thursday,
                        DrawTime = new TimeSpan(19, 30, 0),
                        MinimumDivision1Prize = 4000000
                    }
                },
                {
                    LottoType.SaturdayLotto,
                    new LottoGameRules
                    {
                        Type = LottoType.SaturdayLotto,
                        Name = "Saturday TattsLotto",
                        NumbersDrawn = 6,
                        MaxNumber = 45,
                        SupplementaryCount = 2,
                        StandardGameCost = 0.70M,
                        DrawDay = DayOfWeek.Saturday,
                        DrawTime = new TimeSpan(19, 30, 0),
                        MinimumDivision1Prize = 5000000
                    }
                }
            };
        }
    }
}
