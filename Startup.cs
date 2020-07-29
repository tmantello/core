using AspNetCoreNHibernate.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreNHibernate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Chamado em tempo de execu��o. 
        /// Adiciona servi�os ao container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {                
                options.CheckConsentNeeded = context => true; // avisar usu�rio sobre os cookies e pedir consentimento.
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();

            services.AddNHibernate(Configuration.GetConnectionString("DefaultConnection"));
            services.AddControllersWithViews();
        }


        /// <summary>
        /// Chamado em tempo de execu��o. 
        /// Configura o HTTP request pipeline.
        /// </summary>        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())            
                app.UseDeveloperExceptionPage();            
            else
            {
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts(); // pol�tica de seguran�a, informa que deve usar conex�o segura e https p/ acessar os recursos (padr�o 30 dias)
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}");
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
