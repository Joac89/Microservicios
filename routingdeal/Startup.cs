using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using routingdeal.Business;
using Swashbuckle.AspNetCore.Swagger;

namespace routingdeal
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

        // This method gets called by the runtime. Use this method to add services to the container.
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
            services.Add(new ServiceDescriptor(typeof(RoutingDao),
                new RoutingDao(Configuration.GetConnectionString("DefaultConnection"), _hostingEnv.WebRootPath + "/database.txt"))
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Routing Deal",
                    Version = "v1.0",
                    Description = "Enrutador para resolver URL de convenio y XSLT de transformación",
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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Routing Deal v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
