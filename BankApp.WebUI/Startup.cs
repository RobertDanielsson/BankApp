using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankApp.Application.AutoMapper.Profiles;
using BankApp.Application.Customers.Queries.GetCustomersListSearch;
using BankApp.Application.Customers.Queries.GetIndexStatistics;
using BankApp.Application.Interfaces;
using BankApp.Persistence;
using BankApp.Persistence.Identity;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReflectionIT.Mvc.Paging;

namespace BankApp.WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string securityKey = Configuration["securityKey"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            services.AddScoped(typeof(IBankAppDbContext), typeof(BankAppDbContext));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //What to validate
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        //Setup validate data
                        ValidIssuer = "robband.se",
                        ValidAudience = "readers",
                        IssuerSigningKey = symmetricSecurityKey
                    };
                });

            services.AddDbContext<BankAppDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IBankAppDbContext>(sp => sp.GetRequiredService<BankAppDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<BankAppDbContext>();

            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddEntityFrameworkStores<BankAppDbContext>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policy => policy
                    .RequireClaim("Admin"));

                options.AddPolicy(
                    "Cashier",
                    policy => policy
                    .RequireClaim("Cashier"));
            });

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetCustomersListQueryValidator>());

            services.AddMediatR(typeof(GetIndexStatisticsQueryHandler).GetTypeInfo().Assembly);
            services.AddPaging(options =>
            {
                options.ViewName = "CustomPager";
            });

            services.AddAutoMapper(typeof(CustomerProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                      name: "customer_details",
                      template: "customer/{customerId}",
                      defaults: new
                      {
                          Controller = "Customer",
                          Action = "Index"
                      });

                routes.MapRoute(
                      name: "customer_manageAccounts",
                      template: "customer/{customerId}/manageaccounts",
                      defaults: new
                      {
                          Controller = "Customer",
                          Action = "manageaccounts"
                      });

                routes.MapRoute(
                      name: "customer_transfer",
                      template: "customer/{customerId}/transfer",
                      defaults: new
                      {
                          Controller = "transfer",
                          Action = "transfer"
                      });

                routes.MapRoute(
                      name: "customer_deposit",
                      template: "customer/{customerId}/deposit",
                      defaults: new
                      {
                          Controller = "transfer",
                          Action = "deposit"
                      });

                routes.MapRoute(
                      name: "customer_withdraw",
                      template: "customer/{customerId}/withdraw",
                      defaults: new
                      {
                          Controller = "transfer",
                          Action = "withdraw"
                      });

                routes.MapRoute(
                      name: "account_details",
                      template: "customer/{customerId}/account/{accountId}",
                      defaults: new
                      {
                          Controller = "customer",
                          Action = "AccountDetails"
                      });

                routes.MapRoute(
                      name: "customer_create",
                      template: "createcustomer",
                      defaults: new
                      {
                          Controller = "customer",
                          Action = "createcustomer"
                      });

                routes.MapRoute(
                      name: "customer_edit",
                      template: "customer/{customerId}/edit",
                      defaults: new
                      {
                          Controller = "customer",
                          Action = "editcustomer"
                      });

                routes.MapRoute(
                      name: "search_index",
                      template: "Search",
                      defaults: new
                      {
                          Controller = "Search",
                          Action = "Index"
                      });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
