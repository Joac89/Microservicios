using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace userinterface
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnv;
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = new[] { "text/plain", "text/json", "application/json" };
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "User Interface",
                    Version = "v1.0",
                    Description = "Interfaz estandar para la comunicación de clientes y convenions",
                    Contact = new Contact()
                    {
                        Name = ": Especialización AES - PUJ",
                        Email = "aguilarcjesus@javeriana.edu.co"
                    },
                    License = new License()
                    {
                        Name = "MIT",
                        Url = "http://opensource.org/licenses/MIT"
                    },
                });
                c.CustomSchemaIds(type => type.Name);
                c.DescribeAllEnumsAsStrings();
                c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);*/
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();
            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Interface v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
