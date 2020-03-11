using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleStore.GraphQLApi.Options;
using SimpleStore.Infrastructure.Common;
using System;
using System.Threading.Tasks;
using SimpleStore.Infrastructure.Common.Extensions;

namespace SimpleStore.GraphQLApi
{
    public class Startup
    {
        private readonly ServiceOptions _serviceOptions;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this._serviceOptions = this.Configuration.GetOptions<ServiceOptions>("Services");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()
                .AddHttpClient(this._serviceOptions.ProductCatalogApi.ServiceName, (sp, client) =>
                {
                    client.BaseAddress = new Uri($"{this._serviceOptions.ProductCatalogApi.RestUri}/graphql");
                });

            services
                .AddGraphQLSubscriptions()
                .AddStitchedSchema(stitchingBuilder =>
                    stitchingBuilder.AddSchemaFromHttp(this._serviceOptions.ProductCatalogApi.ServiceName));

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Listen(this._serviceOptions.GraphQLApi);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseGraphQL("/graphql");
            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/graphql",
                Path = "/playground",
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/playground");
                    return Task.CompletedTask;
                });
            });
        }
    }
}