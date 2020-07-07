using ApiLocadora.Controllers;
using ApiLocadora.Model;
using ApiLocadora.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Context
{
	public class ValidacoesLocacao : IValidacoes<Locacao>
	{

		private readonly AppDbContext _context;

		public ValidacoesLocacao(AppDbContext context)
		{
			_context = context;
		}

		
		//No seguinte contexto, uma locação não pode ser completada se houver um filme já locado em sua lista selecionada pelo cliente,
		//de acordo com a regra de negocios da Locadora.
		public bool ValidacaoInicial(Locacao generico)
		{
			var ids = new List<int>();
			_context.Entry(generico).State = EntityState.Added;
			
			var filmesFront = generico.LocacoesFilmes;

			foreach (var f in filmesFront)
				ids.Add(f.Filme.ID);
			
			var filmesSalvos = _context.Filme.Where(f	 => ids.Contains(f.ID)).ToList();
			var loc = filmesSalvos.FirstOrDefault(fs => !fs.EstaDisponivel);

			if (loc == null)
				return true;

			else
				return false;

		}
		public void Inativar(Locacao generico)
		{
			generico.Desativado = true;
		}
	}
}
