using System.Diagnostics;

namespace HtmlInterceptor.NewTab
{
    public static class BrowserMonitor
    {
        private static bool isRunning = true;
        private static Dictionary<int, DateTime> lastNotificationTimes = new Dictionary<int, DateTime>();
        private static TimeSpan notificationCooldown = TimeSpan.FromMinutes(30);

        public static void Install()
        {
            // In a complete implementation, this would:
            // 1. Create a URL monitoring helper script
            // 2. Install browser helper extensions if user approves
            // 3. Set up communication channels
            // 
            // For now, just log that it's enabled
            Console.WriteLine("Browser monitoring enabled.");
        }

        public static void StartMonitoring()
        {
            Task.Run(() =>
            {
                while (isRunning)
                {
                    try
                    {
                        var browsers = Process.GetProcesses().Where(p =>
                            p.ProcessName.ToLower().Contains("chrome") ||
                            p.ProcessName.ToLower().Contains("firefox") ||
                            p.ProcessName.ToLower().Contains("edge") ||
                            p.ProcessName.ToLower().Contains("iexplore"));

                        foreach (var browser in browsers)
                        {
                            try
                            {
                                bool isActive = WindowUtils.IsActiveWindow(browser.MainWindowHandle);
                                bool isAmazon = IsVisitingAmazon(browser);
                                string windowTitle = WindowUtils.GetWindowTitle(browser.MainWindowHandle);

                                Console.WriteLine($"Browser: {browser.ProcessName} (PID: {browser.Id})");
                                Console.WriteLine($"  Window title: '{windowTitle}'");
                                Console.WriteLine($"  Is active: {isActive}");
                                Console.WriteLine($"  Is Amazon: {isAmazon}");

                                if (isActive && isAmazon)
                                {
                                    if (ShouldShowNotification(browser.Id))
                                    {
                                        Console.WriteLine($"  Showing notification for browser {browser.ProcessName} (PID: {browser.Id})");
                                        ShowCouponNotification(browser);

                                        lastNotificationTimes[browser.Id] = DateTime.Now;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error checking browser {browser.ProcessName} (PID: {browser.Id}): {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error monitoring browsers: {ex.Message}");
                    }

                    Thread.Sleep(2000);
                }
            });
        }

        private static bool ShouldShowNotification(int processId)
        {
            if (lastNotificationTimes.TryGetValue(processId, out DateTime lastTime))
            {
                return (DateTime.Now - lastTime) > notificationCooldown;
            }

            return true;
        }

        private static bool IsVisitingAmazon(Process browserProcess)
        {
            // In a real implementation, this would use browser-specific methods:
            // - Chrome: Chrome Debugging Protocol
            // - Firefox: Firefox Remote Debugging Protocol
            // - Edge: Microsoft Edge DevTools Protocol

            try
            {
                string windowTitle = WindowUtils.GetWindowTitle(browserProcess.MainWindowHandle);

                if (string.IsNullOrEmpty(windowTitle))
                    return false;

                return windowTitle.ToLower().Contains("amazon");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking window title: {ex.Message}");
                return false;
            }
        }

        private static void ShowCouponNotification(Process browserProcess)
        {
            string notificationUrl = $"http://localhost:{NotificationServer.PORT}/notify?pid={browserProcess.Id}";

            try
            {
                string browserName = browserProcess.ProcessName.ToLower();

                if (browserName.Contains("chrome"))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "chrome.exe",
                        Arguments = $"--app=\"{notificationUrl}\" --window-size=450,400",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else if (browserName.Contains("firefox"))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "firefox.exe",
                        Arguments = $"-new-window \"{notificationUrl}\"",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = $"{browserName}.exe",
                        Arguments = $"\"{notificationUrl}\"",
                        UseShellExecute = true
                    };

                    try
                    {
                        Process.Start(psi);
                    }
                    catch
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = notificationUrl,
                            UseShellExecute = true
                        });
                    }
                }

                Console.WriteLine($"Showed notification for browser PID: {browserProcess.Id} at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to show notification: {ex.Message}");

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = notificationUrl,
                        UseShellExecute = true
                    });
                }
                catch (Exception fallbackEx)
                {
                    Console.WriteLine($"Even fallback notification failed: {fallbackEx.Message}");
                }
            }
        }
    }
}
