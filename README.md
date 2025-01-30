# Australian Lottery Analyzer

A web-based analysis tool for Australian lottery games, providing statistical analysis, predictions, and historical data visualization.

## Features

- **Multi-Game Support**
  - Monday Lotto
  - Oz Lotto (Tuesday)
  - Wednesday Lotto
  - Powerball (Thursday)
  - Saturday TattsLotto

- **Statistical Analysis**
  - Historical data analysis
  - Hot and cold numbers identification
  - Number pair correlation analysis
  - Trend visualization
  - Win probability calculations

- **Predictions**
  - Number recommendations based on multiple strategies
  - Confidence ratings for predictions
  - Historical accuracy tracking
  - Detailed reasoning for recommendations

- **Data Visualization**
  - Interactive charts using Chart.js
  - Historical trend analysis
  - Frequency distribution graphs
  - Pattern recognition displays

## Technology Stack

- ASP.NET Core 8.0
- Entity Framework Core
- Chart.js for data visualization
- Bootstrap 5 for responsive design
- HTML Agility Pack for data scraping
- SQL Server for data storage

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (Optional)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/LottoAnalyzer.git
```

2. Navigate to the project directory:
```bash
cd LottoAnalyzer
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Run the application:
```bash
dotnet run
```

5. Access the application at:
```
https://localhost:5001
```

## Project Structure

```
LottoAnalyzer/
├── Controllers/          # MVC Controllers
├── Models/              # Data models and view models
├── Services/            # Business logic and data services
├── Views/               # Razor views
│   ├── Home/           # Main views
│   └── Shared/         # Shared layouts and partials
├── wwwroot/            # Static files (CSS, JS, images)
└── Data/               # Data access layer
```

## Key Components

### LottoScraper
Handles data collection from official lottery websites:
- Historical results retrieval
- Real-time data updates
- Error handling and retry logic

### LottoAnalyzer
Core analysis engine providing:
- Statistical calculations
- Pattern recognition
- Prediction generation
- Historical trend analysis

### Web Interface
Responsive web interface featuring:
- Real-time updates
- Interactive charts
- Mobile-friendly design
- Intuitive navigation

## Development

### Code Style
- Follow Microsoft's C# coding conventions
- Use meaningful variable and method names
- Include XML documentation for public methods
- Keep methods focused and concise

### Testing
Run tests using:
```bash
dotnet test
```

### Contributing
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## Disclaimer

This tool is for entertainment purposes only. Lottery games are based on random chance, and no prediction system can guarantee wins. Please gamble responsibly.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please open an issue in the repository or contact the development team.
