namespace client.Models;
public record FormServer1Display
{
    public string? address { get; init; }
    public int xpos { get; init; }
    public int ypos { get; init; }
    public string? color { get; init; }
    public string? screenX { get; init; }
    public string? screenY { get; init; }
    public bool isError { get; init; }

}