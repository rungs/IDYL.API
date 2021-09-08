using Domain.Interfaces;
using Domain.Interfaces.Services.Syst;
using IdylAPI.Services.Interfaces.Company;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Services.Interfaces.Service.Files;
using IdylAPI.Services.Interfaces.Service.Specification;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Services.Master;
using IdylAPI.Services.Repository;
using IdylAPI.Services.Repository.Company;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Connections;
using Persistence.Contexts;
using SocialMedia.Core.Services;

namespace IdylAPI
{
    public static class ConfigureExtensions
    {
        public static void CorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder =>
                builder.SetIsOriginAllowed((host) => true).
                AllowAnyMethod().
                AllowAnyHeader().
                AllowAnyOrigin());
            });
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ISystUserService, SystUserService>();
            services.AddTransient<IUserGroupMenuService, UserGroupMenuService>();
            services.AddTransient<IUserGroupService, UserGroupService>();
            services.AddTransient<ISpecService, SpecService>();
            services.AddTransient<IEQTypeSpecService, EQTypeSpecService>();
            services.AddTransient<IEQSpecService, EQSpecService>();
            services.AddTransient<IAttachFileService, AttachFileService>();
            services.AddTransient<IPMResourceService, PMResourceService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ISystUserCustomerService, SystUserCustomerService>();
            services.AddTransient<ISectionService, SectionService>();
            services.AddTransient<ICraftTypeService, CraftTypeService>();
            services.AddTransient<ILoginHistoryService, LoginHistoryService>();
            services.AddTransient<IPMService, PMService>();
            services.AddTransient<IProblemTypeService, ProblemTypeService>();
            services.AddTransient<IFreqUnitService, FreqUnitService>();
           
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepositoryV2<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<AppDBContext>());
            //services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
            services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();
          
            return services;
        }
    }
}
