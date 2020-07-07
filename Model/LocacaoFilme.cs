using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Model
{
	public class LocacaoFilme
	{
		public int FilmeId { get; set; }
		public Filme Filme { get; set; }
		public int LocacaoId { get; set; }
		public Locacao Locacao { get; set; }
	}
}
