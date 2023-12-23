using DesktopApp.Domain.Interfaces;
using DesktopApp.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopApp.Domain;

public static class DomainLayerServiceExtension
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
       
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();

        return services;
    }
}