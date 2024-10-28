using FluentAssertions.Common;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace SistemasdeTarefas
{
    public class Program
    {

        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Registre a dependencia IAlunoRepository e AlunoRepository.
            builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
            builder.Services.AddScoped<IExistenciaCardRepository, ExistenciaCardRepository>();
            builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();
            builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();
            builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            builder.Services.AddScoped<IProfessoresRepository, ProfessoresRepository>();
            builder.Services.AddScoped<IloginRepository, loginRepository>();

            // Configure o Swagger/OpenAPI
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API MONITOR", Version = "v1" });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API MONITOR");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Configura��o do CORS
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin(); // Permitir qualquer origem (n�o recomendado em produ��o)
                builder.AllowAnyMethod(); // Permitir qualquer m�todo HTTP
                builder.AllowAnyHeader(); // Permitir qualquer cabe�alho
            });

            app.MapControllers();

            app.Run();
        }

        // M�todo Configure
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
