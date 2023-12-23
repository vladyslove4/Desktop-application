using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Services;

public class TeacherService: ITeacherService
{
    private readonly IBaseRepository<Teacher> _teacherRepository;
    

    public TeacherService(IBaseRepository<Teacher> teacherRepository)
    {

        _teacherRepository = teacherRepository;
        
    }

    public async Task<Teacher> GetTeacherByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _teacherRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Teacher>> GetTeachersAsync(CancellationToken cancellationToken)
    {
        var teachers = await _teacherRepository.GetAllAsync(cancellationToken);

        

        return teachers;
    }

    public async Task<Teacher> CreateTeacherAsync(Teacher teacher, CancellationToken cancellationToken)
    {
        if (teacher is null)
        {
            throw new CannotCreateEntityException($"Could not create course");
        }

       var retrievedTeacher = await _teacherRepository.CreateAsync(teacher, cancellationToken);

        return retrievedTeacher;
    }

    public async Task<bool> DeleteTeacherAsync(int id, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id, cancellationToken);

        await _teacherRepository.DeleteAsync(teacher, cancellationToken);

        return true;
    }

    public async Task UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken)
    {
        if (teacher is null)
        {
            throw new CannotUpdateEntityException($"Could not update teacher because it not found");
        }

        await _teacherRepository.UpdateAsync(teacher, cancellationToken);
    }
}