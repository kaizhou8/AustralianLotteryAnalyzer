namespace LottoAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// Main entry point for the LottoAnalyzer application
            /// Configures and starts the web application
            /// </summary>
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Configure caching for better performance
            builder.Services.AddMemoryCache();

            // Configure HTTP client factory for web scraping
            builder.Services.AddHttpClient();

            // Configure logging
            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            // Build the application
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                // Use exception handler page in production
                app.UseExceptionHandler("/Home/Error");
                // Enable HSTS for security
                app.UseHsts();
            }

            // Configure HTTPS redirection
            app.UseHttpsRedirection();

            // Enable serving static files (CSS, JavaScript, images)
            app.UseStaticFiles();

            // Enable routing
            app.UseRouting();

            // Enable authorization
            app.UseAuthorization();

            // Configure default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Start the application
            app.Run();
        }
    }
}
