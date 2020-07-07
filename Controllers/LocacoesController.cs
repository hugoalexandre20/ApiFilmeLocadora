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
	public class LocacoesController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly ValidacoesLocacao _validacoes;

		public LocacoesController(AppDbContext context, ValidacoesLocacao validacoes)
		{
			_context = context;
			_validacoes = validacoes;

		}

		// GET: api/Locacoes
		[HttpGet]
		public IEnumerable<Locacao> GetLocacoes()
		{
			return _context.Locacao;
		}

		// GET: api/Locacoes/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetLocacao([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var locacao = await _context.Locacoes.FindAsync(id);

			if (locacao == null)
			{
				return NotFound();
			}

			return Ok(locacao);
		}

		// PUT: api/Locacoes/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutLocacao([FromRoute] int id, [FromBody] Locacao locacao)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (locacao.Cliente != null)
				_context.Entry(locacao.Cliente).State = EntityState.Modified;

			if (locacao.LocacoesFilmes != null)
			{
				foreach (var item in locacao.LocacoesFilmes)
					_context.Entry(item.Filme).State = EntityState.Modified;
			}


			if (id != locacao.ID)
			{
				return BadRequest();
			}



			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!LocacaoExists(id))
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

		// POST: api/Locacoes
		[HttpPost]
		public async Task<IActionResult> PostLocacao([FromBody] Locacao locacao)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}



			//Aqui estou validando se há algum filme indisponivel 
			if (!_validacoes.ValidacaoInicial(locacao))
				return BadRequest();


			_context.Locacao.Add(locacao);
			_context.Entry(locacao.Cliente).State = EntityState.Modified;

			foreach (var item in locacao.LocacoesFilmes)
				_context.Entry(item.Filme).State = EntityState.Modified;

			await _context.SaveChangesAsync();


			return CreatedAtAction("GetLocacao", new { id = locacao.ID }, locacao);
		}

		// DELETE: api/Locacoes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLocacao([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var lc = await _context.Locacao.FindAsync(id);

			if (lc == null)
			{
				return NotFound();
			}

			//validação adicionada para não excluir fisicamente registros no banco de dados
			_validacoes.Inativar(lc);
			await this.PutLocacao(id, lc);

			return Ok(lc);
		}

		private bool LocacaoExists(int id)
		{
			return _context.Locacao.Any(e => e.ID == id);
		}

		public async Task<IActionResult> GetLocacaoForCliente([FromRoute] Cliente cliente)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var locacao =  _context.Locacoes.Where(l => l.Locacao.Cliente.ID == cliente.ID);

			if (locacao == null)
			{
				return null;
			}

			return Ok(locacao);
		}
	}
}