using MongoDB.Bson;
using MongoDB.Driver;
using CatalogApi.Settings;
using CatalogApi.Repositories;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CatalogApi.Repositories.Interfaces;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogApi
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
            // MongoDb Serialization for Guid and DateTimeOffset
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            // Services Registration
            services.AddControllers();
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                return new MongoClient(settings.ConnectionString);
            });
            services.AddSingleton<IMongoDbItemsRepository, MongoDbItemsRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogApi v1"));
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
