using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Services;

public class StudentService: IStudentService
{
    private readonly IBaseRepository<Student> _studentRepository;
    private readonly IBaseRepository<Group> _groupRepository;

    public StudentService(IBaseRepository<Student> studentRepository, IBaseRepository<Group> groupRepository)
    {
        _studentRepository = studentRepository;
        _groupRepository = groupRepository;
    }

    public async Task<Student> GetStudentByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetStudentsAsync(CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllAsync(cancellationToken);

        return students;
    }

    public async Task<Student> CreateStudentAsync(Student student, CancellationToken cancellationToken)
    {
        if (student is null)
        {
            throw new CannotCreateEntityException($"Could not create student");
        }

         var retrievedStudent =await _studentRepository.CreateAsync(student, cancellationToken);

        return retrievedStudent;
    }

    public  async Task<bool> DeleteStudentAsync(int id, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);

        await _studentRepository.DeleteAsync(student, cancellationToken);

        return true;
    }

    public async Task UpdateStudentAsync(Student student, CancellationToken cancellationToken)
    {
        if (student is null)
        {
            throw new CannotUpdateEntityException($"Could not update student because it not found");
        }

        await _studentRepository.UpdateAsync(student, cancellationToken);
    }

    public async Task<IReadOnlyList<Student>> GetStudentByGroupIdAsync(int groupId, CancellationToken cancellationToken)
    {
        var specificStudents = await _studentRepository.FindAsync(s => s.GroupId == groupId, cancellationToken);
        

        return specificStudents;
    }
}