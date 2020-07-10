using AlugaOffice.Models;
using AlugaOffice.Models.TodosProdutos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Database
{
    public class AlugaOfficeContext : DbContext
    {

        public AlugaOfficeContext(DbContextOptions<AlugaOfficeContext> options) : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<NewsletterEmail> NewsletterEmails { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<EnderecoEntrega> EnderecosEntrega { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoSituacao> PedidoSituacoes { get; set; }

    }
}
