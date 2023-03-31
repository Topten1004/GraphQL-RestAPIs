using GraphiQl;
using GraphQL.Server;
using GraphQL.Types;
using GraphQLProject.Data;
using GraphQLProject.Interfaces;
using GraphQLProject.Mutations;
using GraphQLProject.Query;
using GraphQLProject.Schema;
using GraphQLProject.Services;
using GraphQLProject.Type;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GraphQLProject
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
            //SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GraphQLProject", Version = "v1" });
            });

            services.AddControllers();

            //// GraphQL OLD
            //services.AddTransient<IProduct, ProductService>();
            //services.AddSingleton<ProductType>();
            //services.AddSingleton<ProductQuery>();
            //services.AddSingleton<ProductMutation>();
            //services.AddSingleton<ProductSchema>();
            //services.AddSingleton<ISchema, ProductSchema>();


            // GraphQL NEW
            services.AddTransient<IProduct, ProductService>();
            services.AddTransient<ProductType>();
            services.AddTransient<ProductQuery>();
            services.AddTransient<ProductMutation>();
            services.AddTransient<ProductSchema>();
            services.AddTransient<ISchema, ProductSchema>();




            // DATABASE CONNECTION
            //==============================================
            services.AddDbContext<GraphQLDbContext>(option => option.UseSqlServer(@"Data Source= (localdb)\MSSQLLocalDB; Initial Catalog=GraphQLDb; Integrated Security = True"));


            //GRAPHQL
            //==============================================
            services.AddGraphQL(options => {
                options.EnableMetrics = false;
            }).AddSystemTextJson();


            //REDIS
            //==============================================
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

            //services.AddDistributedRedisCache(option =>
            //{
            //    option.Configuration = "localhost:6379";
            //});

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GraphQLDbContext dbContext)
        {
            // REMOVE
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphQLProject v1"));
            }

            // Verifica de o database existe
            // Define que o banco será o que está definido em InitialCatalog
            // Pressupondo que para ser modificado pelo projeto será necessário usar o MIGRATIONS
            dbContext.Database.EnsureCreated();

            app.UseGraphiQl("/graphql");
            app.UseGraphQL<ISchema>();
            

            
        }
    }
}
