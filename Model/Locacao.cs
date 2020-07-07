using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Model
{
	public class Locacao
	{
		
		public int ID { get; set; }
		[ForeignKey("ClienteId")]
		public int ClienteId { get; set; }
		public virtual Cliente Cliente { get; set; }

		public virtual ICollection<LocacaoFilme> LocacoesFilmes { get; set; }

		public bool Desativado { get; set; }
		public Locacao()
		{
			LocacoesFilmes = new HashSet<LocacaoFilme>();
		}
	}
}
