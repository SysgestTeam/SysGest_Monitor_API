using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            // Configuração de autenticação JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ChaveSuperSecreta12345678901234567890"))
                };
            });

            // Configuração do Swagger

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API MONITOR", Version = "v1" });

                // Configuração de autenticação via JWT Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

                // Adiciona o parâmetro de cabeçalho de autorização globalmente
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            var app = builder.Build();

                // Middlewares
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                // Configuração do Swagger no pipeline de requisições
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API MONITOR");
                    c.RoutePrefix = "swagger";
                });


            // Configuração do CORS
            app.UseCors(corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin();
                corsBuilder.AllowAnyMethod();
                corsBuilder.AllowAnyHeader();
            });

            app.MapControllers();
            app.Run();
        }
    }
}
