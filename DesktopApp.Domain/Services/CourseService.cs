using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Services;

public class CourseService: ICourseService
{
    private readonly IBaseRepository<Course> _courseRepository;
    private readonly IBaseRepository<Group> _groupRepository;

    private List<Course> _courses = new List<Course>();

    public CourseService(IBaseRepository<Course> courseRepository, IBaseRepository<Group> groupRepository)
    {

        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
    }

    public async Task<Course> GetCourseByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _courseRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync(CancellationToken cancellationToken)
    {

        return await _courseRepository.GetAllAsync(cancellationToken); ;
    }

    public async Task<Course> CreateCourseAsync(Course course, CancellationToken cancellationToken)
    {
        if (course is null)
        {
            throw new CannotCreateEntityException($"Could not create course");
        }

        var retrievedCourse = await _courseRepository.CreateAsync(course, cancellationToken);

        return retrievedCourse;
    }

    public async Task<bool> DeleteCourseAsync(int id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);

        var checkGroups = await _groupRepository.FindAsync(s => s.CourseId == id, cancellationToken);

        if (checkGroups == null)
        {
            throw new CannotFindEntityException($"Cannot find group with course id {id}.");
        }
        else if (checkGroups.Any())
        {
            return false;
        }

        await _courseRepository.DeleteAsync(course, cancellationToken);

        return true;
    }

    public async Task UpdateCourseAsync(Course course, CancellationToken cancellationToken)
    {
        if (course is null)
        {
            throw new CannotUpdateEntityException($"Could not update course because it not found");
        }

        await _courseRepository.UpdateAsync(course, cancellationToken);
    }
}