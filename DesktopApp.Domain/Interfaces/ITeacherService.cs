using DesktopApp.Domain.Entity;

namespace DesktopApp.Domain.Interfaces
{
    public interface ITeacherService
    {
        Task<Teacher> GetTeacherByIdAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Teacher>> GetTeachersAsync(CancellationToken cancellationToken);

        Task<Teacher> CreateTeacherAsync(Teacher teacher, CancellationToken cancellationToken);

        Task <bool> DeleteTeacherAsync(int id, CancellationToken cancellationToken);

        Task UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken);

    }
}