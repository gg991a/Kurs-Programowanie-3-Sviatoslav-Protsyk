using Microsoft.AspNetCore.Mvc;
using SportsStore.Api.Models;

namespace SportsStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Piłka nożna", Category = "Sporty drużynowe", Price = 800 },
            new Product { Id = 2, Name = "Rakieta tenisowa", Category = "Tenis", Price = 1500 }
        };

        [HttpGet] 
        public ActionResult<IEnumerable<Product>> Get() => Ok(_products);

        [HttpPost] 
        public IActionResult Post([FromBody] Product product)
        {
            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            return Ok(product);
        }

        [HttpPut("{id}")] 
        public IActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            product.Name = updatedProduct.Name;
            product.Category = updatedProduct.Category;
            product.Price = updatedProduct.Price;
            return Ok(product);
        }

        [HttpDelete("{id}")] 
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            _products.Remove(product);
            return Ok();
        }
    }
}