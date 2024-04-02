namespace srv1.ServerData
{
    // Класс для передачи информации об разрешении экрана
    public record DisplayInfo
    {
        public int Width { get; init; }
        public int Height { get; init; }
    }
}