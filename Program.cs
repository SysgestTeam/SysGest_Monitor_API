using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static SistemasdeTarefas.Repository.DividasRepository;

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
            builder.Services.AddScoped<ISaldoConsumo, SaldoConsumoRepository>();
            builder.Services.AddScoped<IDividasRepository, DividasRepository>();
            builder.Services.AddScoped<IAnoLectivo, AnoLectivoRepository>();

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
            });

            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();


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

            // Configuração do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins(allowedOrigins) 
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API MONITOR", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

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
            app.UseCors("AllowSpecificOrigin");


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

            // Aplicação do CORS
            app.UseCors("AllowAll");

            app.MapControllers();
            app.Run();
        }
    }
}