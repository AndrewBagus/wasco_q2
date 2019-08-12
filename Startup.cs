using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appglobal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace wasco_q2 {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
            DbInitializer.Initialize ();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.Configure<CookiePolicyOptions> (options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc (config => {
                    var policy = new AuthorizationPolicyBuilder ()
                        .RequireAuthenticatedUser ()
                        .Build ();
                    config.Filters.Add (new AuthorizeFilter (policy));
                })
                .SetCompatibilityVersion (CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions (options => {
                    options.Conventions.AllowAnonymousToPage ("/Index");
                    options.Conventions.AllowAnonymousToPage ("/Register");
                    options.Conventions.AllowAnonymousToPage ("/Login");
                });
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<login_db> (options => options.UseMySql (AppGlobal.MYSQL_CS)); //set DBContext - used in migration

            services.AddAuthentication (o => {
                    o.DefaultScheme = "appcore_cookie_instance";
                    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie (o => {
                    o.LoginPath = new PathString ("/login");
                    //o.ReturnUrlParameter = null;
                    o.AccessDeniedPath = new PathString ("/main/forbidden");
                    o.ExpireTimeSpan = TimeSpan.FromMinutes (10);
                });
            services.AddAuthorization (opts => {
                //opts.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            services.Configure<IISOptions> (options => {
                options.ForwardClientCertificate = true; //IIS cannot access kestrel session claims by default, this code made it possible.
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Error");
                app.UseHsts ();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles ();
            app.UseCookiePolicy ();
            app.UseAuthentication();

            app.UseMvc ();
        }
    }
}