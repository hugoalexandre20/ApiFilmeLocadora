using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Model
{
	public class Cliente
	{
		public int ID { get; set; }
		[MaxLength(50)]
		public string Nome { get; set; }

		public bool Desativado { get; set; }
	}
}
