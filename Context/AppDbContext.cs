using ApiLocadora.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiLocadora.Context
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{ }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<LocacaoFilme>().HasKey(lf => new { lf.FilmeId, lf.LocacaoId });

			modelBuilder.Entity<LocacaoFilme>()
								.HasOne<Filme>(lf => lf.Filme)
								.WithMany(f => f.LocacoesFilmes)
								.HasForeignKey(lf => lf.FilmeId);


			modelBuilder.Entity<LocacaoFilme>()
					.HasOne<Locacao>(lf => lf.Locacao)
					.WithMany(l => l.LocacoesFilmes)
					.HasForeignKey(lf => lf.LocacaoId);


		}
		public DbSet<Cliente> Cliente { get; set; }
		public DbSet<Filme> Filme { get; set; }
		public DbSet<Locacao> Locacao { get; set; }
		public DbSet<LocacaoFilme> Locacoes { get; set; }



	}
}
