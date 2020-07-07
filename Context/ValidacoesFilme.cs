using ApiLocadora.Model;
using ApiLocadora.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Context
{
	public class ValidacoesFilme : IValidacoes<Filme>
	{
		private readonly AppDbContext _context;

		public ValidacoesFilme(AppDbContext context)
		{
			_context = context;
		}

		public bool ValidacaoInicial(Filme generico)
		{
			var ok = generico.EstaAtrasado();
			if (ok)
				return true;

			else
				return false;
		}
	
		public void Inativar(Filme generico)
		{
			generico.Desativado = true;
		}
	}
}
