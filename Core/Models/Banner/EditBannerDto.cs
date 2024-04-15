using Microsoft.AspNetCore.Http;

namespace Core.Models.Banner
{
    public class EditBannerDto
    {
        public IFormFile Image { get; set; }
    }
}
