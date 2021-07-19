using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MMLib.Ocelot.Provider.AppConfiguration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MediatR;
using System.Reflection;
using SagaExample.Gateways.Handlers;
using MassTransit;
using SagaExample.Gateways.Configurations;
using RabbitMQ.Client;

namespace SagaExample.Gateways
{
    public class Message
    {
        public string Text { get; set; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            //services.Configure<HealthCheckPublisherOptions>(options =>
            //{
            //    options.Delay = TimeSpan.FromSeconds(2);
            //    options.Predicate = (check) => check.Tags.Contains("ready");
            //});

            // swagger + ocelot
            var ocelotConfigSection = Configuration.GetSection("Ocelot");
            services.AddOcelot(ocelotConfigSection).AddAppConfiguration();
            services.AddSwaggerForOcelot(ocelotConfigSection);
            // MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // bus settings
            var busConfig = Configuration.GetSection("Bus").Get<BusConfig>();
            services.AddTransient<INotifierMediatorService, NotifierMediatorService>();

            // MassTransit registration
            services.AddMassTransit(mtc =>
            {
                mtc.AddConsumer<EventConsumer>();
                mtc.AddConsumer<SuperEventConsumer>();
                mtc.AddConsumer<SuperMegaEventConsumer>();
                mtc.SetKebabCaseEndpointNameFormatter();

                mtc.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(busConfig.Host, busConfig.VirtualHost, h =>
                    {
                        h.Username(busConfig.Username);
                        h.Password(busConfig.Password);
                        //h.UseSsl();
                    });
                    // register consumer' and hub' endpoints
                    cfg.ConfigureEndpoints(context);

                    //cfg.ReceiveEndpoint("event-listener", e =>
                    //{
                    //    e.ConfigureConsumer<EventConsumer>(context);
                    //});
                });
                ////mtc.AddSignalRHub<ChatHub>(cfg => {/*Configure hub lifetime manager*/});
                //mtc.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                //{                    
                //    cfg.Host(busConfig.Host, busConfig.VirtualHost, h =>
                //    {
                //        h.Username(busConfig.Username);
                //        h.Password(busConfig.Password);
                //        //h.UseSsl();
                //    });
                //    // register consumer' and hub' endpoints
                //    cfg.ConfigureEndpoints(context);
                //}));
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            if (env.IsDevelopment())
            {
                app.UseSwaggerForOcelotUI().UseOcelot().Wait();
            }
        }
    }


    class EventConsumer : IConsumer<Message>
    {
        ILogger<SuperEventConsumer> _logger;

        public EventConsumer(ILogger<SuperEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.Text);
        }
    }

    class SuperEventConsumer : IConsumer<Message>
    {
        ILogger<SuperEventConsumer> _logger;

        public SuperEventConsumer(ILogger<SuperEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.Text);
        }
    }

    class SuperMegaEventConsumer : IConsumer<Message>
    {
        ILogger<SuperEventConsumer> _logger;

        public SuperMegaEventConsumer(ILogger<SuperEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.Text);
        }
    }
}
