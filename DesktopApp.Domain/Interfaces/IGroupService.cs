using DesktopApp.Domain.Entity;

namespace DesktopApp.Domain.Interfaces
{
    public interface IGroupService
    {
        Task<Group> GetGroupByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Group>> GetGroupsAsync(CancellationToken cancellationToken);
        Task<Group> CreateGroupAsync(Group group, CancellationToken cancellationToken);
        Task<bool> DeleteGroupAsync(int id, CancellationToken cancellationToken);
        Task UpdateGroupAsync(Group group, CancellationToken cancellationToken);
        Task<IReadOnlyList<Group>> GetGroupByCourseIdAsync(int courseId, CancellationToken cancellationToken);
    }
}