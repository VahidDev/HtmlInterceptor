using System.Net;
using System.Text;

namespace HtmlInterceptor.NewTab
{
    public static class NotificationServer
    {
        public const int PORT = 45678;
        private static string currentCouponCode = "SAVE20NOW";
        private static bool isRunning = true;
        private static HashSet<int> notifiedProcessIds = new HashSet<int>();

        public static void Start()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{PORT}/");
            listener.Start();

            Console.WriteLine($"Notification server started on port {PORT}");

            while (isRunning)
            {
                try
                {
                    var context = listener.GetContext();
                    ThreadPool.QueueUserWorkItem((_) => HandleNotificationRequest(context));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Server error: {ex.Message}");
                }
            }

            listener.Stop();
        }

        private static void HandleNotificationRequest(HttpListenerContext context)
        {
            string pidStr = context.Request.QueryString["pid"];

            if (int.TryParse(pidStr, out int pid))
            {
                lock (notifiedProcessIds)
                {
                    notifiedProcessIds.Add(pid);
                }
            }

            string html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Amazon Coupon Available</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; background: #f4f4f4; padding: 20px; }}
                        .coupon-box {{ 
                            background: white; 
                            border: 2px dashed #e47911; 
                            border-radius: 10px; 
                            padding: 20px; 
                            text-align: center;
                            box-shadow: 0 0 10px rgba(0,0,0,0.1);
                            max-width: 400px;
                            margin: 0 auto;
                        }}
                        .code {{ 
                            font-size: this24px; 
                            font-weight: bold; 
                            color: #e47911; 
                            padding: 10px;
                            background: #f7f7f7;
                            border-radius: 5px;
                            margin: 10px 0;
                        }}
                        .apply-btn {{
                            background: #e47911;
                            color: white;
                            border: none;
                            padding: 10px 20px;
                            border-radius: 5px;
                            cursor: pointer;
                            font-size: 16px;
                            margin-top: 10px;
                        }}
                        .close-btn {{
                            background: #888;
                            color: white;
                            border: none;
                            padding: 10px 20px;
                            border-radius: 5px;
                            cursor: pointer;
                            font-size: 16px;
                            margin-top: 10px;
                            margin-left: 10px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='coupon-box'>
                        <h2>Amazon Coupon Available!</h2>
                        <p>We found a coupon that might save you money on your purchase:</p>
                        <div class='code'>{currentCouponCode}</div>
                        <p>Copy this code and apply it at checkout.</p>
                        <button class='apply-btn' onclick='copyCode()'>Copy Code</button>
                        <button class='close-btn' onclick='window.close()'>Close</button>
                    </div>
                    <script>
                        function copyCode() {{
                            navigator.clipboard.writeText('{currentCouponCode}');
                            alert('Coupon code copied to clipboard!');
                            window.close();
                        }}
                    </script>
                </body>
                </html>";

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.Close();
        }
    }

}
