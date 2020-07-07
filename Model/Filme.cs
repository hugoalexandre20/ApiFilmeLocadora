using ApiLocadora.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Model
{
	public class Filme
	{
		
		public int ID { get; set; }
		
		public DateTime DataLocacao { get; set; }
		public DateTime DataEntrega { get; set; }
		[Required]
		public bool EstaDisponivel { get; set; }
		[MaxLength(50)]
		public string Nome { get; set; }
		public virtual ICollection<LocacaoFilme> LocacoesFilmes { get; set; }
		public bool Desativado { get; set; }
		public Filme()
		{
			LocacoesFilmes = new HashSet<LocacaoFilme>();
		}

		//Método com intutito de validar o atraso do filme.

		public bool EstaAtrasado()
		{
			if (DataEntrega != null)
				return this.DataEntrega < DateTime.Now;

			else
				return false;
		}

	}
}
