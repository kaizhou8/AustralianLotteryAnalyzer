using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace LottoAnalyzer
{
    public class LottoResult
    {
        public DateTime DrawDate { get; set; }
        public int DrawNumber { get; set; }
        public List<int> WinningNumbers { get; set; } = new();
        public List<int> SupplementaryNumbers { get; set; } = new();
    }

    public class LottoScraper
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://australia.national-lottery.com/saturday-lotto/past-results/";

        public LottoScraper()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        }

        public async Task<List<LottoResult>> GetHistoricalResults(int year)
        {
            var results = new List<LottoResult>();
            try
            {
                var url = $"{BaseUrl}{year}";
                var html = await _client.GetStringAsync(url);
                
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // 查找所有结果行
                var rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
                if (rows != null)
                {
                    foreach (var row in rows.Skip(1)) // 跳过表头
                    {
                        var cells = row.SelectNodes("td");
                        if (cells != null && cells.Count >= 4)
                        {
                            var result = new LottoResult
                            {
                                DrawDate = ParseDate(cells[0].InnerText.Trim()),
                                DrawNumber = int.Parse(Regex.Replace(cells[1].InnerText.Trim(), "[^0-9]", "")),
                                WinningNumbers = ParseNumbers(cells[2].InnerText.Trim()),
                                SupplementaryNumbers = ParseNumbers(cells[3].InnerText.Trim())
                            };
                            results.Add(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching results for {year}: {ex.Message}");
            }

            return results;
        }

        private DateTime ParseDate(string dateStr)
        {
            // 假设日期格式为 "Saturday 27th Jan 2024"
            return DateTime.Parse(dateStr);
        }

        private List<int> ParseNumbers(string numbersStr)
        {
            return numbersStr.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(n => int.Parse(Regex.Replace(n.Trim(), "[^0-9]", "")))
                           .ToList();
        }

        public async Task<List<LottoResult>> GetLastNYearsResults(int years)
        {
            var results = new List<LottoResult>();
            var currentYear = DateTime.Now.Year;
            
            for (int i = 0; i < years; i++)
            {
                var yearResults = await GetHistoricalResults(currentYear - i);
                results.AddRange(yearResults);
                // 添加延迟以避免过快请求
                await Task.Delay(1000);
            }

            return results;
        }
    }
}
