
using System.Drawing;
using System.Runtime.InteropServices;

// Класс для работы с экраном
public class Screen
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;    // x верхнего левого угла
        public int Top;     // y верхнего левого угла
        public int Right;   // x нижнего правого угла
        public int Bottom;  // y нижнего правого угла
    }
    // Подключение системных библиотек Windows
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);
    [DllImport("user32.dll", SetLastError = false)]
    private static extern IntPtr GetDesktopWindow();
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindowDC(IntPtr window);
    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern uint GetPixel(IntPtr dc, int x, int y);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int ReleaseDC(IntPtr window, IntPtr dc);

    private const int LOGPIXELSX = 88;
    private const int LOGPIXELSY = 90;
    private static readonly object SyncRoot = new object();
    // Функция для получения размера экрана
    public static Size GetSize(){
        var desktop = GetDesktopWindow();
        RECT rect;
        GetWindowRect(new HandleRef(SyncRoot, desktop), out rect);
        int width = rect.Right;
        int height = rect.Bottom;
        return new Size() { Width = width, Height = height };
    }
    // Функция для получения цвета в пикселе
    public static Color GetColorAt(Point point)
    {
        IntPtr desktop = GetDesktopWindow();
        IntPtr dc = GetWindowDC(desktop);
        int a = (int) GetPixel(dc, point.X, point.Y);
        ReleaseDC(desktop, dc);
        return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
    }
}
