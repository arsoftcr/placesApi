using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace placesApi
{
    public class Startup
    {

        public static noef.models.Connection Conexion = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Conexion = new noef.models.Connection();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           

            Conexion.Server = Configuration["Contexto:Server"];

            Conexion.Bd = Configuration["Contexto:Bd"];

            Conexion.User = Configuration["Contexto:User"];

            Conexion.Password = Configuration["Contexto:Password"];

            Conexion.Port = "5432";

            Conexion.TypeConnection = noef.models.typeConnection.Postgres;

            services.AddControllers();

            services.AddMvc().AddMvcOptions(options=> {

                options.EnableEndpointRouting = false;
            });

            services.AddCors();


            string key = "AASFGHJKLKJYTEREWSDF6654ESEE4W33EER45R5TG";

            SymmetricSecurityKey security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidateLifetime = true,
                       ValidIssuer = "some",
                       ValidAudience = "some",
                       IssuerSigningKey = security,
                   };
               });

            var filePath = System.IO.Path.Combine(AppContext.BaseDirectory, "placesApi.xml");
            
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Places V1", Version = "v1" });

                config.DocInclusionPredicate((docName, description) => true);

                config.IncludeXmlComments(filePath);


                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                   
                    Description =
         "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });

            });
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

             app.UseDeveloperExceptionPage();
            
            app.UseHttpsRedirection();


            app.UseCors(builder =>
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Places V1");
               
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
