using DesktopApp.Domain.Entity;

namespace DesktopApp.Domain.Interfaces
{

    public interface ICourseService
    {
        Task<Course> GetCourseByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Course>> GetCoursesAsync(CancellationToken cancellationToken);
        Task<Course> CreateCourseAsync(Course course, CancellationToken cancellationToken);
        Task<bool> DeleteCourseAsync(int id, CancellationToken cancellationToken);
        Task UpdateCourseAsync(Course course, CancellationToken cancellationToken);
    }

}
