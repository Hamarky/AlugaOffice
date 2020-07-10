using AlugaOffice.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AlugaOffice.Libraries.Email
{
    public class GerenciarEmail
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;
        public GerenciarEmail (SmtpClient smtp, IConfiguration configuration)
        {
            _smtp = smtp;
            _configuration = configuration;
        }

        public void EnviarContatoPorEmail(Contato contato)
        {

            string corpoMsg = string.Format("<h2>Contato - AlugaOffice</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>E-mail: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                "<br /> E-mail enviado automaticamente do site AlugaOffice.",
                contato.Nome,
                contato.Email,
                contato.Texto
            );


            /*
             * MailMessage -> Construir a mensagem
             */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:UserName"));
            mensagem.To.Add("alugaoffice@gmail.com");
            mensagem.Subject = "Contato - AlugaOffice - E-mail: " + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via SMTP
            _smtp.Send(mensagem);
        }

        public void EnviarSenhaParaColaboradorPorEmail (Colaborador colaborador)
        {
            string corpoMsg = string.Format("<h2>Colaborador - AlugaOffice</h2>" +
                "Sua senha é: " +
                "<h3>{0}</h3>", colaborador.Senha
                );
            /*
             * MailMessage -> Construir a mensagem
             */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:UserName"));
            mensagem.To.Add(colaborador.Email);
            mensagem.Subject = "Colaborador - AlugaOffice - Senha do Colaborador - " + colaborador.Nome;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via SMTP
            _smtp.Send(mensagem);
        }
        public void EnviarDadosDoPedido(Cliente cliente, Pedido pedido)
        {
            string corpoMsg = string.Format("<h2>Pedido - Aluga Office</h2>" +

                "Pedido realizado com sucesso!<br />" +
                "<h3>Nº {0}</h3>" +
                "<br /> Acompanhe o andamento em nossa loja.",
                pedido.Id + "-" + pedido.TransactionId

            );


            /*
             * MailMessage -> Construir a mensagem
             */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add(cliente.Email);
            mensagem.Subject = "AlugaOffice - Pedido - " + pedido.Id + "-" + pedido.TransactionId;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via SMTP
            _smtp.Send(mensagem);
        }
    }
}
