using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Services;

public class GroupService: IGroupService
{
    private readonly IBaseRepository<Group> _groupRepository;
    private readonly IBaseRepository<Student> _studentRepositoty;
    private readonly IBaseRepository<Course> _courseRepository;
    private readonly IBaseRepository<Teacher> _teacherRepository;

    public GroupService(IBaseRepository<Group> groupRepository, IBaseRepository<Student> studentRepository,
                        IBaseRepository<Course> courseRepository, IBaseRepository<Teacher> teacherRepository)
    {
        _teacherRepository = teacherRepository;
        _groupRepository = groupRepository;
        _studentRepositoty = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Group> GetGroupByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _groupRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Group>> GetGroupsAsync(CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetAllAsync(cancellationToken);

        foreach (var group in groups)
        {

            var teacher = await _teacherRepository.GetByIdAsync(group.TeacherId, cancellationToken);

            if (teacher != null)
            {
                group.TeacherId = teacher.Id;
                group.TeacherName = $"{teacher.Name} {teacher.LastName}";
            }

        }

        return groups;
    }

    public async Task<Group> CreateGroupAsync(Group group, CancellationToken cancellationToken)
    {
        if (group is null)
        {
            throw new CannotCreateEntityException($"Could not create group");
        }

        var retrievedGroup = await _groupRepository.CreateAsync(group, cancellationToken);

        return retrievedGroup;
    }

    public async Task<bool> DeleteGroupAsync(int id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        var checkStudents = await _studentRepositoty.FindAsync(s => s.GroupId == id, cancellationToken);

        if (checkStudents == null)
        {
            throw new CannotFindEntityException($"Cannot find students with group id {id}.");
        }
        else if (checkStudents.Any())
        {
            return false;
        }

        await _groupRepository.DeleteAsync(group, cancellationToken);

        return true;
    }

    public async Task UpdateGroupAsync(Group group, CancellationToken cancellationToken)
    {
        if (group is null)
        {
            throw new CannotUpdateEntityException($"Could not update group because it not found");
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);
    }

    public async Task<IReadOnlyList<Group>> GetGroupByCourseIdAsync(int courseId, CancellationToken cancellationToken)
    {
        var specififcGroup = await _groupRepository.FindAsync(g => g.CourseId == courseId, cancellationToken);

        return specififcGroup;
    }
}