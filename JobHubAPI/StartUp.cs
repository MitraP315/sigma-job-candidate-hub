using JobHubAPI.DbSetup;
using JobHubAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace JobHubAPI
{
    public class Startup
        {
            private readonly IWebHostEnvironment _hostingEnvironment;
            public Startup(IWebHostEnvironment env, IConfiguration configuration)
            {
                _hostingEnvironment = env;
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(Configuration);
            services.AddScoped<ICandidateServices, CandidateServices>(); // Assuming `CandidateServices` is the implementation

            var serviceProvider = services.BuildServiceProvider();
            IDatabaseFactory dbFactory = DatabaseFactories.SetFactory(serviceProvider);
            services.AddSingleton(dbFactory);
            services.AddControllers()
                .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddHttpContextAccessor();

            //     services.AddControllers()
            //    .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("JobCandidateHub", new OpenApiInfo { Title = "Job Candidate Hub ", Version = "v1" });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    string groupName = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiExplorerSettingsAttribute>()
                        .Select(attr => attr.GroupName).FirstOrDefault();
                    if (groupName == docName) { return true; } else { return false; }

                });

            });
                }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting(); // Essential for routing

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://example.com", "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/JobCandidateHub/swagger.json", "Online Class API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Essential for mapping controller routes
            });
        }

    }

}
