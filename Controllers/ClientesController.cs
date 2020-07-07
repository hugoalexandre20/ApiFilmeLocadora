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
	public class ClientesController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly ValidacoesCliente _validacoes;

		public ClientesController(AppDbContext context, ValidacoesCliente validacoes)
		{
			_context = context;
			_validacoes = validacoes;
		}
		
		// GET: api/Cliente
		[HttpGet]
		public IEnumerable<Cliente> GetClientes()
		{
			return _context.Cliente;
		}

		// GET: api/Cliente/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCliente([FromRoute] int id)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var cliente = await _context.Cliente.FindAsync(id);

			if (cliente == null)
			{
				return NotFound();
			}

			return Ok(cliente);
		}
		
		// PUT: api/Cliente/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCliente([FromRoute] int id, [FromBody] Cliente cliente)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != cliente.ID)
			{
				return BadRequest();
			}

			_context.Entry(cliente).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ClienteExists(id))
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

		// POST: api/Cliente
		[HttpPost]
		public async Task<IActionResult> PostCliente([FromBody] Cliente cliente)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			
			//validação para não cadastrar cliente já existente.
			if (_validacoes.ValidacaoInicial(cliente))
				return BadRequest();

			
			_context.Cliente.Add(cliente);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCliente", new { id = cliente.ID }, cliente);
		}

		// DELETE: api/Cliente/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCliente([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var cliente = await _context.Cliente.FindAsync(id);
			if (cliente == null)
			{
				return NotFound();
			}

			//validaçao para não permitir excluir fisicamente um cliente do banco de dados
			_validacoes.Inativar(cliente);
			await PutCliente(id,cliente);
			

			return Ok(cliente);
		}

		private bool ClienteExists(int id)
		{
			return _context.Cliente.Any(e => e.ID == id);
		}

		//buscar clientes pelo nome
		public async Task<IActionResult> GetClienteForName([FromRoute] string nome)
		{

			if (!ModelState.IsValid)
			{
				return null;
			}

			var cliente =  _context.Cliente.FirstOrDefault(c => c.Nome == nome);

			if (cliente == null)
			{
				return null;
			}
			if (_validacoes.ValidacaoInicial(cliente))
				return null;

			return Ok(cliente);
		}
	}
}