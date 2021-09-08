using IdylAPI;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Services.Interfaces.Dashboard;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Services.Interfaces.WO;
using IdylAPI.Services.Interfaces.WR;
using IdylAPI.Services.Repository.Authorize;
using IdylAPI.Services.Repository.Dashboard;
using IdylAPI.Services.Repository.Img;
using IdylAPI.Services.Repository.Master;
using IdylAPI.Services.Repository.Notify;
using IdylAPI.Services.Repository.Syst;
using IdylAPI.Services.Repository.WO;
using IdylAPI.Services.Repository.WR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Contexts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IDYL.API
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
            services.CorsConfiguration();

            services.AddSignalR();
            services.AddTransient<IAuthorizeRepository, AuthorizeRepository>();
            services.AddTransient<IWORepository, WORepository>();
            services.AddTransient<IWRRepository, WRRepository>();
            services.AddTransient<ISiteRepository, SiteRepository>();
            services.AddTransient<IEQRepository, EQRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IProblemTypeRepository, ProblemTypeRepository>();
            services.AddTransient<ISystemRepository, SystemRepository>();
            services.AddTransient<IUploadRespository, UploadRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IResourceRepository, ResourceRepository>();
            services.AddTransient<IWOResRepository, WOResRepository>();
            services.AddTransient<ISectionRepository, SectionRepository>();
            services.AddTransient<IFailureObjectRepository, FailureObjectRepository>();
            services.AddTransient<IEvaluateRepository, EvaluateRepository>();
            services.AddTransient<IWOTaskRepository, WOTaskRepository>();
            services.AddTransient<IEQTypeRepository, EQTypeRepository>();
            services.AddTransient<IVendorRepository, VendorRepository>();
            services.AddTransient<INotifyRepository, NotifyRepository>();
            services.AddTransient<INotifyMsgDefaultRepository, NotifyMsgDefaultRepository>();
            services.AddTransient<INotifyMsgRoleRepository, NotifyMsgRoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFormVideoRepository, FormVideoRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<IWOTypeRepository, WOTypeRepository>();
            services.AddTransient<IUserGroupPermissionRepository, UserGroupPermissionRepository>();
            services.AddTransient<IUserGroupRepository, UserGroupRepository>();
            services.AddControllers(options => options.MaxIAsyncEnumerableBufferLimit = 1000000);
                //.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);


            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IDYLConnection")));

            services.AddSwaggerGen(c =>
            {
                //c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IDYL.API", Version = "v1" });
                //c.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo
                //{
                //    Title = "Swagger With Asp.net core",
                //    Version = "v1",
                //    //Contact = new Microsoft.OpenApi.Models.OpenApiInfo.Contact() { Name = "Jenwit Penjamrat", Email = "jenwit_penjamrat@hotmail.com" }
                //});
            });

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddHttpContextAccessor();
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddServices();
            services.AddControllers().AddOData(o => o.Select().Filter().Count().SetMaxTop(null).Expand());
            //services.AddOData(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IDYL.API v1"));
            }
            app.UseCors("CorsPolicy");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                //routeBuilder.EnableDependencyInjection();
                //routeBuilder.Select().Filter().Expand().OrderBy().MaxTop(null).Count();
                //routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<NotifyHub>("/notify", options =>
                //{
                //    options.Transports =
                //        HttpTransportType.WebSockets |
                //        HttpTransportType.LongPolling;
                //});
            });
        }
    }
}
