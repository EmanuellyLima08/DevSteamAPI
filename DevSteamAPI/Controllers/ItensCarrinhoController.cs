using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevSteamAPI.Data;
using DevSteamAPI.Models;

namespace DevSteamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensCarrinhoController : ControllerBase
    {
        private readonly DevSteamAPIContext _context;

        public ItensCarrinhoController(DevSteamAPIContext context)
        {
            _context = context;
        }

        // GET: api/ItensCarrinho
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCarrinho>>> GetItemCarrinho()
        {
            return await _context.ItemCarrinho.ToListAsync();
        }

        // GET: api/ItensCarrinho/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCarrinho>> GetItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);

            if (itemCarrinho == null)
            {
                return NotFound();
            }

            return itemCarrinho;
        }

        // PUT: api/ItensCarrinho/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemCarrinho(Guid id, ItemCarrinho itemCarrinho)
        {
            if (id != itemCarrinho.ItemCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(itemCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await UpdateCarrinhoTotal(itemCarrinho.CarrinhoId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCarrinhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItensCarrinho
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemCarrinho>> PostItemCarrinho(ItemCarrinho itemCarrinho)
        {
            itemCarrinho.Total = itemCarrinho.Quantidade * itemCarrinho.Valor;
            _context.ItemCarrinho.Add(itemCarrinho);
            await _context.SaveChangesAsync();
            await UpdateCarrinhoTotal(itemCarrinho.CarrinhoId);

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItensCarrinho/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            _context.ItemCarrinho.Remove(itemCarrinho);
            await _context.SaveChangesAsync();
            await UpdateCarrinhoTotal(itemCarrinho.CarrinhoId);

            return NoContent();
        }

        private bool ItemCarrinhoExists(Guid id)
        {
            return _context.ItemCarrinho.Any(e => e.ItemCarrinhoId == id);
        }

        private async Task UpdateCarrinhoTotal(Guid carrinhoId)
        {
            var carrinho = await _context.Carrinho.FindAsync(carrinhoId);
            if (carrinho != null)
            {
                carrinho.Total = await _context.ItemCarrinho
                    .Where(i => i.CarrinhoId == carrinhoId)
                    .SumAsync(i => i.Total);
                await _context.SaveChangesAsync();
            }
        }
    }
}
