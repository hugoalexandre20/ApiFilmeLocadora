using ApiLocadora.Model;
using ApiLocadora.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Context
{
	public class ValidacoesCliente : IValidacoes<Cliente>
	{
		private readonly AppDbContext _context;


		public ValidacoesCliente(AppDbContext context)
		{
			_context = context;

		}

		//optei validar pelo nome pois no momento do cadastro ainda não teremos o ID do Cliente.
		public bool ValidacaoInicial(Cliente generico)
		{
			return _context.Cliente.Any(c => c.Nome == generico.Nome);
		}
		public void Inativar(Cliente generico)
		{
			generico.Desativado = true;
		}
	}
}
