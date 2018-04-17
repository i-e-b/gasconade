using ExternalLogDefinitions;
using Gasconade.UI.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tag;

namespace SampleCoreWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseGasconadeUI(config => {
                config.AddAssembly(typeof(InsultMessage).Assembly);
                config.AddSwaggerLink();
                config.AddHeaderHtml(T.g("p")[
                    "This is a ", T.g("b")["sample"], " of Gasconade output, running under AspnetCore MVC. Looks like it's working."
                ]);
            });
        }
    }
}
