using System.Drawing;

namespace srv1.ServerData
{
    // Класс для передачи информации о цвете пикселя
    public record PixelColorInfo
    {
        public Color PixelColor { get; init; }
    }
}