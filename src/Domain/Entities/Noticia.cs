using Domain.Interfaces;

namespace Domain.Entities;

public class Noticia : IAggregateRoot
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime DataPublicacao { get; set; }
    public string Autor { get; set; }
}