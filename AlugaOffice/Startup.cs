using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using AlugaOffice.Repositories;
using AlugaOffice.Repositories.Contracts;
using AlugaOffice.Libraries.Sessao;
using AlugaOffice.Libraries.Login;
using System.Net.Mail;
using System.Net;
using AlugaOffice.Libraries.Email;
using AlugaOffice.Libraries.Middleware;
using AlugaOffice.Libraries.CarrinhoCompra;
using AutoMapper;
using AlugaOffice.Libraries.AutoMapper;

namespace AlugaOffice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * Padrão Repository
             */
            services.AddAutoMapper(typeof(MapeamentoProfile));
            services.AddHttpContextAccessor();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IImagemRepository, ImagemRepository>();



            services.AddScoped<SmtpClient>(options =>
            {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = Configuration.GetValue<string>("Email:ServerSMTP"),
                    Port = Configuration.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Configuration.GetValue<string>("Email:UserName"),
                    Configuration.GetValue<string>("Email:Password")),
                    EnableSsl = true
                };
                return smtp;
            });
            services.AddScoped<GerenciarEmail>();
            services.AddScoped<Libraries.Cookie.Cookie>();
            services.AddScoped<CarrinhoCompra>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            /*
             * Session - Configuração
             */
            services.AddMemoryCache(); //Guardar os dados na memória
            services.AddSession(options =>
            {

            });

            services.AddScoped<Sessao>();
            services.AddScoped<Libraries.Cookie.Cookie>();
            services.AddScoped<LoginCliente>();
            services.AddScoped<LoginColaborador>();

            services.AddMvc(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O campo deve ser preenchido!");
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddSessionStateTempDataProvider();

            services.AddSession(options => { options.Cookie.IsEssential = true; });

            services.AddDbContext<AlugaOfficeContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("AlugaOfficeContext"), builder =>
                        builder.MigrationsAssembly("AlugaOffice")
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
