using Domain.Models;
using Shared.DTO;

namespace Shared.Mapping
{
    public static class TagMapper
    {
        public static TagDto ToDto(Tag tag) => new(tag.Id, tag.Name, tag.ColorHex);

        public static IEnumerable<TagDto> ToDto(IEnumerable<Tag> tags) => tags.Select(ToDto);

        public static Tag ToEntity(CreateTagDto dto) => new() { Name = dto.Name, ColorHex = dto.ColorHex };

        public static void UpdateEntity(UpdateTagDto dto, Tag entity)
        {
            entity.Name = dto.Name;
            entity.ColorHex = dto.ColorHex;
        }
    }
}
