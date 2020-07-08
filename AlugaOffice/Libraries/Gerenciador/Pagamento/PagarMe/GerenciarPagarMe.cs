﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.Login;
using AlugaOffice.Libraries.Texto;
using AlugaOffice.Models;
using Microsoft.Extensions.Configuration;
using PagarMe;

namespace AlugaOffice.Libraries.Gerenciador.Pagamento.PagarMe
{
    public class GerenciarPagarMe
    {
        private IConfiguration _configuration;
        private LoginCliente _loginCliente;
        public GerenciarPagarMe(IConfiguration configuration, LoginCliente loginCliente)
        {
            _configuration = configuration;
            _loginCliente = loginCliente;
        }
        public object GerarBoleto(double valor)
        {
            try { 
                Cliente cliente = _loginCliente.GetCliente();

                PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");
                PagarMeService.DefaultEncryptionKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey:EncryptionKey");

                Transaction transaction = new Transaction();

                transaction.Amount = Convert.ToInt32(valor);
                transaction.PaymentMethod = PaymentMethod.Boleto;
                transaction.Customer = new Customer
                {
                    ExternalId = cliente.Id.ToString(),
                    Name = cliente.Nome,
                    Type = CustomerType.Individual,
                    Country = "br",
                    Email = cliente.Email,
                    Documents = new[] {
                        new Document {
                            Type = DocumentType.Cpf,
                            Number = Mascara.Remover(cliente.CPF)
                        },
                    },
                    PhoneNumbers = new string[] {
                        "+55" + Mascara.Remover(cliente.Telefone)
                    },
                    Birthday = cliente.Nascimento.ToString("yyyy-MM-dd")
                };
                transaction.Save();

                return new { BoletoURL = transaction.BoletoUrl, BarCode = transaction.BoletoBarcode, Expiration = transaction.BoletoExpirationDate };
                }
            catch(Exception e)
            {
                return new { Erro = e.Message };
            }
        }

        /*public object GerarPagCartaoCredito(CartaoCredito cartao)
        {
            Cliente cliente = _loginCliente.GetCliente();

            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");
            PagarMeService.DefaultEncryptionKey = _configuration.GetValue<String>("Pagamento:PagarMe:EncryptionKey");

            Card card = new Card();
            card.Number = cartao.NumeroCartao;
            card.HolderName = cartao.NomeNoCartao;
            card.ExpirationDate = cartao.Vecimento;
            card.Cvv = cartao.CodigoSeguranca;

            card.Save();

            Transaction transaction = new Transaction();

            //TODO - Valor Total - Pendênte
            transaction.Amount = 2100;
            transaction.Card = new Card
            {
                Id = card.Id
            };

            transaction.Customer = new Customer
            {
                ExternalId = cliente.Id.ToString(),
                Name = cliente.Nome,
                Type = CustomerType.Individual,
                Country = "br",
                Email = cliente.Email,
                Documents = new[] {
                        new Document{
                            Type = DocumentType.Cpf,
                            Number = Mascara.Remover(cliente.CPF)
                        }
                    },
                PhoneNumbers = new string[]
                    {
                        "+55" + Mascara.Remover( cliente.Telefone )
                    },
                Birthday = cliente.Nascimento.ToString("yyyy-MM-dd")
            };

            transaction.Billing = new Billing
            {
                Name = cliente.Nome,
                Address = new Address()
                {
                    Country = "br",
                    State = cliente.Estado,
                    City = cliente.Cidade,
                    Neighborhood = cliente.Bairro,
                    Street = cliente.Endereco + " " + cliente.Complemento,
                    StreetNumber = cliente.Numero,
                    Zipcode = Mascara.Remover(cliente.CEP)
                }
            };

            var Today = DateTime.Now;

            transaction.Shipping = new Shipping
            {
                Name = "Rick",
                Fee = 100,
                DeliveryDate = Today.AddDays(4).ToString("yyyy-MM-dd"),
                Expedited = false,
                Address = new Address()
                {
                    Country = "br",
                    State = "sp",
                    City = "Cotia",
                    Neighborhood = "Rio Cotia",
                    Street = "Rua Matrix",
                    StreetNumber = "213",
                    Zipcode = "04250000"
                }
            };

            transaction.Item = new[] {
                  new Item()
                  {
                    Id = "1",
                    Title = "Little Car",
                    Quantity = 1,
                    Tangible = true,
                    UnitPrice = 1000
                  },
                  new Item()
                  {
                    Id = "2",
                    Title = "Baby Crib",
                    Quantity = 1,
                    Tangible = true,
                    UnitPrice = 1000
                  }
                };

            transaction.Save();
        }*/
    }
}
