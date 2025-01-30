using Microsoft.AspNetCore.Mvc;
using LottoAnalyzer.Models;
using LottoAnalyzer.Services;

namespace LottoAnalyzer.Controllers
{
    /// <summary>
    /// Main controller for the lottery analysis application
    /// Handles user interactions and data presentation
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the HomeController
        /// </summary>
        /// <param name="logger">Logger instance for recording application events</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the home page with a list of available lottery games
        /// </summary>
        /// <returns>View containing all supported lottery games and their rules</returns>
        public IActionResult Index()
        {
            var games = LottoGameRules.GetAllGames();
            return View(games);
        }

        /// <summary>
        /// Displays detailed analysis for a specific lottery game
        /// </summary>
        /// <param name="gameType">Type of lottery game to analyze</param>
        /// <returns>
        /// View containing game statistics, predictions, and recent results
        /// </returns>
        /// <remarks>
        /// This action retrieves historical data, performs statistical analysis,
        /// generates predictions, and presents the information in an organized format
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GameAnalysis(LottoType gameType)
        {
            var scraper = new LottoScraper(gameType);
            var results = await scraper.GetLastNYearsResults(2); // Get last 2 years of data
            var analyzer = new LottoAnalyzer(results);
            var nextDrawDate = await scraper.GetNextDrawDate();
            
            var viewModel = new GameAnalysisViewModel
            {
                GameType = gameType,
                GameRules = LottoGameRules.GetAllGames()[gameType],
                Statistics = analyzer.GetStatistics(),
                Prediction = analyzer.GetPrediction(nextDrawDate),
                LastResults = results.OrderByDescending(r => r.DrawDate).Take(10).ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Retrieves historical lottery data for analysis
        /// </summary>
        /// <param name="gameType">Type of lottery game</param>
        /// <param name="years">Number of years of historical data to retrieve (default: 5)</param>
        /// <returns>
        /// JSON result containing historical lottery results or error message
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(LottoType gameType, int years = 5)
        {
            try
            {
                var scraper = new LottoScraper(gameType);
                var results = await scraper.GetLastNYearsResults(years);
                return Json(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching historical data");
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Generates predictions for the next draw of a specific lottery game
        /// </summary>
        /// <param name="gameType">Type of lottery game to generate predictions for</param>
        /// <returns>
        /// JSON result containing predicted numbers and confidence levels,
        /// or error message if prediction generation fails
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetPrediction(LottoType gameType)
        {
            try
            {
                var scraper = new LottoScraper(gameType);
                var results = await scraper.GetLastNYearsResults(2);
                var analyzer = new LottoAnalyzer(results);
                var nextDrawDate = await scraper.GetNextDrawDate();
                var prediction = analyzer.GetPrediction(nextDrawDate);
                
                return Json(new { success = true, data = prediction });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating prediction");
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
