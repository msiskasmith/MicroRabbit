using MediatR;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Infrastructure.InversionOfControl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MicroRabbit.Transfer.Application.Services;
using System.Reflection;
using MicroRabbit.Contracts;
using MicroRabbit.Workers;

namespace MicroRabbit.Transfer.Api
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

            services.AddDbContext<TransferDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TransferDbConnection"));
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroRabbit.Banking.Api", Version = "v1" });
            });

            services.AddMassTransit(config =>
            {
                 
                config.SetKebabCaseEndpointNameFormatter();
                config.AddConsumers(Assembly.GetEntryAssembly());

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", h => {
                        h.Username("guest");
                        h.Password("guest");

                        cfg.ConfigureEndpoints(ctx);
                    });
                });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "TransferApi_";
            });

            DependencyContainer.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroRabbit.Transfer.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
