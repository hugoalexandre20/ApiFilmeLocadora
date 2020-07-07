# ApiLocadora
Api construída para controle de locações de filmes de uma locadora.

# Questões de SQL

## 1)
Com base no modelo acima, escreva um comando SQL que liste a quantidade de registros por Status com sua descrição

```SQL
select
	count(*) as "Qtde de Registros",
	coalesce(proc.idstatus,0) as "Id Status",
	coalesce((select 
			  	dsstatus 
			  from 
			  	tb_status st 
			  where
			  	st.idstatus = proc.idstatus),
			 'Sem Status') as "Descrição"
from 
	tb_processo proc
	left join tb_andamento anda
		on proc.idprocesso = anda.idprocesso
group by proc.idstatus
```
## 2)
Com base no modelo acima, construa um comando SQL que liste a maior data de andamento por número de processo, com processos encerrados no ano de 2013

```SQL
select 
	proc.nroprocesso as "Numero do Processo", 
	max(anda.dtAndamento) as "Maior data de andamento" 
from 
	tb_andamento anda
	inner join tb_processo proc
		on proc.idprocesso = anda.idprocesso
where 
	extract(year from dtEncerramento) = 2013
group by 
	proc.nroprocesso
```

## 3)
Com base no modelo acima, construa um comando SQL que liste a quantidade de Data de Encerramento agrupada por ela mesma com a quantidade da contagem seja maior que 5

```SQL
select 
	dtEncerramento as "Data de Encerramento", 
	count(dtEncerramento) as "Qtde Processos Encerrados" 
from 
	tb_processo
group by 
	dtEncerramento
having 
	count(dtEncerramento) > 5
```

## 4) 
Possuímos um número de identificação do processo, onde o mesmo contém 12 caracteres com zero á esquerda, contudo nosso modelo e dados ele é apontado como bigint. Como fazer para apresenta-lo com 12 caracteres considerando os zeros a esquerda?

```SQL
select 
	lpad(nroprocesso::text,12,'0') 
from 
	tb_processo; 
```

# API

Foi criado um serviço REST(.NET Core) com Entity FrameWork para o gerenciamento de atividades de uma Locadora. O Banco de Dados usado foi o PostgreSQL

Logo abaixo, estão as solicitações de regras de negócios e onde determinadas validações foram aplicadas no código fonte, para facilitar o acesso para verificação do mesmo.

### - Um locador não pode se repetir.

Classe ClientesController.cs

linha 99

```C#
//validação para não cadastrar cliente já existente.
			if (_validacoes.ValidacaoInicial(cliente))
				return BadRequest();
```

### - Não é permitido excluir fisicamente um registro.

Classe ClientesController.cs

linha 125

```C#
			//validaçao para não permitir excluir fisicamente um cliente do banco de dados
			_validacoes.Inativar(cliente);
			await PutCliente(id,cliente);
```
Classe FilmesController.cs

linha 127

```C#
			//validação para não deletar fisicamente o registro do filme
			_validacoes.Inativar(filme);
			await this.PutFilme(id, filme);
 ```
 Classe LocacoesController.cs
 
 linha 142
 
 ```C#
 //validação adicionada para não excluir fisicamente registros no banco de dados
			_validacoes.Inativar(lc);
			await this.PutLocacao(id, lc);
  ```
  
  
  ### - Não permitir alugar um filme que não está disponivel
  
  Classe LocacoesController.cs
  
  linha 109
  
  ```C#
  if (!_validacoes.ValidacaoInicial(locacao))
				return BadRequest();
 ```
 
 ### - Alertar na devolução se o filme estiver com atraso
 
 Classe FilmesController.cs
 
 linha 68
 
 ```C#
 var ok = _validacoes.ValidacaoInicial(filme);
  ```
  
 
  
 
