using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiLocadora.Context;
using ApiLocadora.Model;

namespace ApiLocadora.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FilmesController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly ValidacoesFilme _validacoes;
		public FilmesController(AppDbContext context, ValidacoesFilme validacoes)
		{
			_context = context;
			_validacoes = validacoes;
		}
		

		// GET: api/Filmes
		[HttpGet]
		public IEnumerable<Filme> GetFilmes()
		{
			return _context.Filme;
		}

		// GET: api/Filmes/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetFilme([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var filme = await _context.Filme.FindAsync(id);

			if (filme == null)
			{
				return NotFound();
			}

			return Ok(filme);
		}

		// PUT: api/Filmes/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFilme([FromRoute] int id, [FromBody] Filme filme)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != filme.ID)
			{
				return BadRequest();
			}

			_context.Entry(filme).State = EntityState.Modified;

			var ok = _validacoes.ValidacaoInicial(filme);
			

			try
			{
				await _context.SaveChangesAsync();
			
				//validação para o caso em que o filme estiver em atraso.
				if (ok)
					return CreatedAtAction("Filme está sendo devolvido em atraso.", new { id = filme.ID }, filme);
				
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FilmeExists(id))
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

		// POST: api/Filmes
		[HttpPost]
		public async Task<IActionResult> PostFilme([FromBody] Filme filme)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Filme.Add(filme);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetFilme", new { id = filme.ID }, filme);
		}

		// DELETE: api/Filmes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFilme([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var filme = await _context.Filme.FindAsync(id);
			if (filme == null)
			{
				return NotFound();
			}
		
			//validação para não deletar fisicamente o registro do filme
			_validacoes.Inativar(filme);
			await this.PutFilme(id, filme);
		
			return Ok(filme);
		}

		private bool FilmeExists(int id)
		{
			return _context.Filme.Any(e => e.ID == id);
		}
		//Buscar filme por nome
		public async Task<IActionResult> GetFilmeForName([FromRoute] string name)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var filme =  _context.Filme.FirstOrDefault(f => f.Nome == name);

			if (filme == null)
			{
				return null;
			}

			return Ok(filme);
		}
	}
}