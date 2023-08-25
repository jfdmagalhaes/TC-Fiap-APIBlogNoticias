using Domain.Interfaces;
using Domain.Models;

namespace Domain.Entities;

public class NoticiaDto : Noticia, IAggregateRoot
{
    public int Id { get; set; }
}