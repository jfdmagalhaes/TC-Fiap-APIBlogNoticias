using AutoFixture;
using Domain.Entities;

namespace Infrastructure.UnitTests.Repositories.Utils;
public class UnitTestTools
{
    public static IList<NoticiaDto> CreateNoticias()
        => new Fixture().Create<IList<NoticiaDto>>();

    public static NoticiaDto CreateNoticia()
        => new Fixture().Create<NoticiaDto>();
}
