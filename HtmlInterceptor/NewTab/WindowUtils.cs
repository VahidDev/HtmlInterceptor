using System.Runtime.InteropServices;
using System.Text;

namespace HtmlInterceptor.NewTab
{
    public static class WindowUtils
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static bool IsActiveWindow(IntPtr handle)
        {
            return handle == GetForegroundWindow();
        }

        public static string GetWindowTitle(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder sb = new StringBuilder(nChars);

            if (GetWindowText(handle, sb, nChars) > 0)
            {
                return sb.ToString();
            }
            return string.Empty;
        }
    }
}
