namespace Domain.Models;
public class Noticia
{
    public string Titulo { get; init; }
    public string Descricao { get; init; }
    public DateTime? DataPublicacao { get; init; }
    public string Autor { get; init; }
}