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
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get(
            [FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        [HttpPost]
        [Route("")]

        public async Task<ActionResult<Category>> Post(
            [FromServices] DataContext context, [FromBody] Category model)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(model);
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

            var categorie = await context.Categories.Where(categorie => categorie.Id == id).FirstOrDefaultAsync();
            context.Categories.Remove(categorie);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Update(
            [FromServices] DataContext context, int id, Category req)
        {

            if (id != req.Id)
            {
                return BadRequest();
            }

            var categorie = await context.Categories.Where(categorie => categorie.Id == id).FirstOrDefaultAsync();
            categorie.Title = req.Title;

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}

