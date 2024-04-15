using Microsoft.AspNetCore.Http;

namespace Core.Models.Category
{
    public class EditCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
