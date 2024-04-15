using Core.Interfaces;
using Core.Models;
using Core.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoriesService categoriesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await categoriesService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await categoriesService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto category)
        {
            await categoriesService.Create(category);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] EditCategoryDto category)
        {
            await categoriesService.Edit(id, category);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await categoriesService.Delete(id);
            return Ok();
        }

        [HttpGet("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromQuery] ItemsFilter itemsFilter)
        {
            return Ok(await categoriesService.GetByFilter(itemsFilter));
        }

        [HttpGet("getByParentCategoryId/{categoryId}")]
        public async Task<IActionResult> GetByParentCategoryId([FromRoute] int categoryId)
        {
            return Ok(await categoriesService.GetByParentCategoryId(categoryId));
        }

        [HttpGet("getByParentCategoryName/{categoryName}")]
        public async Task<IActionResult> GetByParentCategoryName([FromRoute] string categoryName)
        {
            return Ok(await categoriesService.GetByParentCategoryName(categoryName));
        }

        [HttpGet("getHead")]
        public async Task<IActionResult> GetHead()
        {
            IEnumerable<GetCategoryDto> categories = await categoriesService.GetHead();
            return Ok(categories);
        }
    }
}
