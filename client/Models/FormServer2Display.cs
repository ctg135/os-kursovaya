namespace client.Models;
// Данные для отображения формы сервера 2
public record FormServer2Display
{
    public string? address { get; init; }
    public string? pid { get; init; }
    public string? did { get; init; }
    public bool isError { get; init; }

}