using DesktopApp.Domain.Entity;

namespace DesktopApp.Domain.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetStudentByIdAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Student>> GetStudentsAsync(CancellationToken cancellationToken);

        Task<Student> CreateStudentAsync(Student student, CancellationToken cancellationToken);

         Task<bool> DeleteStudentAsync(int id, CancellationToken cancellationToken);

        Task UpdateStudentAsync(Student student, CancellationToken cancellationToken);

        Task<IReadOnlyList<Student>> GetStudentByGroupIdAsync(int groupId, CancellationToken cancellationToken);
    }
}