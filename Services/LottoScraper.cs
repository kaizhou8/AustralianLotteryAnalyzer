using HtmlAgilityPack;
using LottoAnalyzer.Models;
using System.Text.RegularExpressions;

namespace LottoAnalyzer.Services
{
    /// <summary>
    /// Service responsible for scraping lottery results from official websites
    /// Handles data retrieval and parsing for all Australian lottery games
    /// </summary>
    public class LottoScraper
    {
        private readonly HttpClient _client;
        private readonly Dictionary<LottoType, string> _baseUrls;
        private readonly LottoGameRules _gameRules;

        /// <summary>
        /// Initializes a new instance of the LottoScraper for a specific lottery game
        /// </summary>
        /// <param name="gameType">The type of lottery game to scrape data for</param>
        public LottoScraper(LottoType gameType)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            
            // Initialize base URLs for different lottery games
            _baseUrls = new Dictionary<LottoType, string>
            {
                { LottoType.MondayLotto, "https://australia.national-lottery.com/monday-lotto/past-results/" },
                { LottoType.WednesdayLotto, "https://australia.national-lottery.com/wednesday-lotto/past-results/" },
                { LottoType.SaturdayLotto, "https://australia.national-lottery.com/saturday-lotto/past-results/" },
                { LottoType.OzLotto, "https://australia.national-lottery.com/oz-lotto/past-results/" },
                { LottoType.Powerball, "https://australia.national-lottery.com/powerball/past-results/" }
            };

            _gameRules = LottoGameRules.GetAllGames()[gameType];
        }

        /// <summary>
        /// Retrieves historical lottery results for a specific year
        /// </summary>
        /// <param name="year">The year to get results for</param>
        /// <returns>A list of lottery results for the specified year</returns>
        /// <remarks>
        /// The method scrapes data from the official lottery website and parses the HTML content
        /// to extract draw numbers, dates, winning numbers, and prize information
        /// </remarks>
        public async Task<List<LottoResult>> GetHistoricalResults(int year)
        {
            var results = new List<LottoResult>();
            try
            {
                var url = $"{_baseUrls[_gameRules.Type]}{year}";
                var html = await _client.GetStringAsync(url);
                
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Find all result rows in the table
                var rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
                if (rows != null)
                {
                    foreach (var row in rows.Skip(1)) // Skip header row
                    {
                        var cells = row.SelectNodes("td");
                        if (cells != null && cells.Count >= 4)
                        {
                            var result = new LottoResult
                            {
                                GameType = _gameRules.Type,
                                DrawDate = ParseDate(cells[0].InnerText.Trim()),
                                DrawNumber = int.Parse(Regex.Replace(cells[1].InnerText.Trim(), "[^0-9]", "")),
                                WinningNumbers = ParseNumbers(cells[2].InnerText.Trim()),
                                Division1Prize = ParsePrize(cells[4].InnerText.Trim())
                            };

                            // Handle Powerball or supplementary numbers based on game type
                            if (_gameRules.Type == LottoType.Powerball)
                            {
                                result.PowerballNumber = ParseNumbers(cells[3].InnerText.Trim()).FirstOrDefault();
                            }
                            else
                            {
                                result.SupplementaryNumbers = ParseNumbers(cells[3].InnerText.Trim());
                            }

                            results.Add(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching results for {_gameRules.Name} {year}: {ex.Message}");
            }

            return results;
        }

        /// <summary>
        /// Parses a date string from the lottery website format
        /// </summary>
        /// <param name="dateStr">Date string to parse (e.g., "Saturday 27th Jan 2024")</param>
        /// <returns>Parsed DateTime object</returns>
        private DateTime ParseDate(string dateStr)
        {
            return DateTime.Parse(dateStr);
        }

        /// <summary>
        /// Parses a string of lottery numbers into a list of integers
        /// </summary>
        /// <param name="numbersStr">String containing lottery numbers</param>
        /// <returns>List of parsed numbers</returns>
        private List<int> ParseNumbers(string numbersStr)
        {
            return numbersStr.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(n => int.Parse(Regex.Replace(n.Trim(), "[^0-9]", "")))
                           .ToList();
        }

        /// <summary>
        /// Parses a prize amount string into a decimal value
        /// </summary>
        /// <param name="prizeStr">Prize string (e.g., "$1,000,000")</param>
        /// <returns>Prize amount as decimal</returns>
        private decimal ParsePrize(string prizeStr)
        {
            // Remove currency symbols and commas
            var cleanStr = Regex.Replace(prizeStr, @"[^0-9\.]", "");
            return decimal.TryParse(cleanStr, out var prize) ? prize : 0;
        }

        /// <summary>
        /// Retrieves lottery results for the last N years
        /// </summary>
        /// <param name="years">Number of years of historical data to retrieve</param>
        /// <returns>List of lottery results from the specified period</returns>
        public async Task<List<LottoResult>> GetLastNYearsResults(int years)
        {
            var results = new List<LottoResult>();
            var currentYear = DateTime.Now.Year;
            
            for (int i = 0; i < years; i++)
            {
                var yearResults = await GetHistoricalResults(currentYear - i);
                results.AddRange(yearResults);
                await Task.Delay(1000); // Add delay to avoid overwhelming the server
            }

            return results;
        }

        /// <summary>
        /// Calculates the date and time of the next draw
        /// </summary>
        /// <returns>DateTime of the next scheduled draw</returns>
        /// <remarks>
        /// Takes into account the game's draw day and time, and adjusts for
        /// current time to ensure the returned date is always in the future
        /// </remarks>
        public async Task<DateTime> GetNextDrawDate()
        {
            var now = DateTime.Now;
            var nextDraw = new DateTime(now.Year, now.Month, now.Day, 
                                      _gameRules.DrawTime.Hours, 
                                      _gameRules.DrawTime.Minutes, 0);

            // If today is draw day but time has passed, or if it's not draw day,
            // keep adding days until we reach the next valid draw day
            while (nextDraw <= now || nextDraw.DayOfWeek != _gameRules.DrawDay)
            {
                nextDraw = nextDraw.AddDays(1);
            }

            return nextDraw;
        }
    }
}
