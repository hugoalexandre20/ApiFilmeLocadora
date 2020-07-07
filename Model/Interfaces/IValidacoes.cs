using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLocadora.Model.Interfaces
{
	 public interface IValidacoes<T>
	{
		 bool ValidacaoInicial(T generico);
		 void Inativar(T generico);	
	}
}
