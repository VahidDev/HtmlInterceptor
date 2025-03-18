using HtmlInterceptor.Message;
namespace AmazonJsInjector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Html Interceptor Starting...");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WARNING: CLOSE THE APPLICATION ONLY THROUGH COMMAND LINE(CTRL+C OR PRESS SOME KEY)");
            Console.ResetColor();

            await using var proxyManager = new ProxyManager();
            try
            {
                await proxyManager.StartProxy();
                Console.WriteLine("Proxy server started on port 8000");
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

                await proxyManager.StopProxy();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await proxyManager.LogStatus($"error: {ex.Message}");
            }
        }
    }
}