using Core.Interfaces;
using Core.Models;
using Core.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsService productsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await productsService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await productsService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto product)
        {
            await productsService.Create(product);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] EditProductDto product)
        {
            await productsService.Edit(id, product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await productsService.Delete(id);
            return Ok();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute] int categoryId)
        {
            var sortedProducts = await productsService.GetProductsByCategory(categoryId);
            return Ok(sortedProducts);
        }
        [HttpGet("Frequently/{productId}")]
        public async Task<IActionResult> GetFrequentlyBoughtTogether([FromRoute] int productId)
        {
            var sortedProducts = await productsService.GetFrequentlyBoughtTogether(productId);
            return Ok(sortedProducts);
        }

        [HttpGet("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromQuery] ItemsFilter itemsFilter)
        {
            return Ok(await productsService.GetByFilter(itemsFilter));
        }

        [HttpGet("getPromotionalOffersWithFavorites/{count}/{userId?}")]
        public async Task<IActionResult> GetPromotionalOffersWithFavorites(int count, string userId)
        {
            return Ok(await productsService.GetPromotionalOffersWithFavorites(count, userId));
        }

        [HttpGet("getPromotionalOffers/{count}")]
        public async Task<IActionResult> GetPromotionalOffers(int count)
        {
            return Ok(await productsService.GetPromotionalOffers(count));
        }
        
        [HttpGet("getPopularProducts/{count}")]
        public async Task<IActionResult> GetPopularProducts([FromRoute] int count)
        {
            return Ok(await productsService.GetPopularProducts(count));
        }

        [HttpGet("getPopularProductsWithFavorites/{count}/{userId}")]
        public async Task<IActionResult> GetPopularProductsWithFavorites(int count, string userId)
        {
            return Ok(await productsService.GetPopularProductsWithFavorites(count, userId));
        }

        [HttpGet("getPopularProductsByUserId/{userId}/{count}")]
        public async Task<IActionResult> GetPopularProductsByUserId(string userId, [FromRoute] int count)
        {
            var popularProducts = await productsService.GetPopularProductsByUserId(userId, count);
            return Ok(popularProducts);
        }

        [HttpGet("getPopularProductsByCategory/{categoryId}/{count}")]
        public async Task<IActionResult> GetPopularProductsByCategory(int categoryId, [FromRoute] int count)
        {
            var popularProducts = await productsService.GetPopularProductsByCategory(categoryId, count);
            return Ok(popularProducts);
        }
    }
}
