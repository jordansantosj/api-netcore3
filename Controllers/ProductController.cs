using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeef.Data;
using testeef.Models;

namespace testeef.Controllers
{
    [ApiController]
    [Route("v1/products")]

    public class ProductController : ControllerBase
    {
        [HttpGet]

        public async Task<ActionResult<List<Product>>> Get(
            [FromServices] DataContext context)
        {

            var products = await context.Products
                .Include(x => x.Category)
                .ToListAsync();
            return products;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Product>> GetById(
            [FromServices] DataContext context, int id)
        {

            var product = await context.Products.Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        [HttpGet("categories/{id:int}")]

        public async Task<ActionResult<List<Product>>> GetByCategory(
            [FromServices] DataContext context, int id)
        {

            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();
            return products;
        }

        [HttpPost]

        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] Product model)
        {

            if (ModelState.IsValid)
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(
            [FromServices] DataContext context, int id)
        {
            var product = await context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Update([FromServices] DataContext context, int id, Product req)
        {
            if (id != req.Id)
            {
                return BadRequest();
            }

            var product = await context.Products.Where(product => product.Id == id).FirstOrDefaultAsync();
            product.Title = req.Title;
            product.Description = req.Description;
            product.Price = req.Price;
            product.CategoryId = req.CategoryId;

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}