using AlugaOffice.Database;
using AlugaOffice.Models;
using AlugaOffice.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private AlugaOfficeContext _banco;
        public NewsletterRepository(AlugaOfficeContext banco)
        {
            _banco = banco;
        }

        public void Cadastrar(NewsletterEmail newsletter)
        {
            _banco.NewsletterEmails.Add(newsletter);
            _banco.SaveChanges();
        }

        public IEnumerable<NewsletterEmail> ObterTodasNewsletter()
        {
            return _banco.NewsletterEmails.ToList();
        }
    }
}
