using Domain.Models;
using Shared.DTO;

namespace Shared.Mapping
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category category) => new(category.Id, category.Name);

        public static IEnumerable<CategoryDto> ToDto(IEnumerable<Category> categories) => categories.Select(ToDto);

        public static Category ToEntity(CreateCategoryDto dto) => new() { Name = dto.Name };

        public static void UpdateEntity(UpdateCategoryDto dto, Category entity) => entity.Name = dto.Name;
    }
}
