using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;
using DesktopApp.Domain.Services;
using Moq;
using System.Linq.Expressions;

namespace DesktopApp.Tests.DesktopApp.Domain.Services.Tests;

public class GroupServiceTests
{

    readonly CancellationToken cancellationToken;
    readonly Mock<IBaseRepository<Course>> mockRepositoryCourse;
    readonly Mock<IBaseRepository<Student>> mockRepositoryStudent;
    readonly Mock<IBaseRepository<Group>> mockRepositoryGroup;
    readonly Mock<IBaseRepository<Teacher>> mockRepositoryTeacher;

    public GroupServiceTests()
    {
        cancellationToken = new CancellationToken();
        mockRepositoryCourse = new Mock<IBaseRepository<Course>>();
        mockRepositoryGroup = new Mock<IBaseRepository<Group>>();
        mockRepositoryStudent = new Mock<IBaseRepository<Student>>();
        mockRepositoryTeacher = new Mock<IBaseRepository<Teacher>>();
    }

    [Fact]
    public async Task GetGroupById_ValidId_ReturnGroup()
    {
        //arrange

        int groupId = 1;
        Course course = new Course();

        var exprctedGroup = new Group { Id = 1, Course = course, CourseId = 1, Name = "test" };

        mockRepositoryGroup.Setup(repository => repository.GetByIdAsync(groupId, cancellationToken)).ReturnsAsync(exprctedGroup);

        var groupService = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                            mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        //act
        var result = await groupService.GetGroupByIdAsync(groupId, cancellationToken);

        //Assert

        Assert.Equal(exprctedGroup, result);
    }

    [Fact]

    public async Task GetGroups_ReturnsListOfGroups()
    {
        Course course = new Course { Name = "CourseName" };
        //Arrange
        var expectedGroups = new List<Group>
            {
                new Group { Id = 1, Course = course, CourseId = 1, Name = "test"},
                new Group { Id = 2, Course = course, CourseId = 2, Name = "test"}
            };

        mockRepositoryGroup.Setup(repository => repository.GetAllAsync(cancellationToken)).ReturnsAsync(expectedGroups);

        var groupService = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                            mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        //Act

        var result = await groupService.GetGroupsAsync(cancellationToken);

        //Assert

        foreach (var group in result)
        {
            Assert.Equal("CourseName", group.Course.Name);
        }
    }

    [Fact]
    public async Task CreateGroup_ValidGroup_CallsCreateAsync()
    {
        // Arrange
        Course course = new Course { Id = 1, Name = "CourseName" };
        var expectedGroup = new Group
        {
            Id = 1,
            Name = "test",
            CourseId = 1,
            Course = course
        };

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                       mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act
        await service.CreateGroupAsync(expectedGroup, cancellationToken);

        // Assert
        mockRepositoryGroup.Verify(repository => repository.CreateAsync(expectedGroup, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateGroup_NullGroup_ThrowsCannotCreateEntityException()
    {
        // Arrange

        Group? group = null;

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                       mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotCreateEntityException>(() =>
        {
            return service.CreateGroupAsync(group, cancellationToken);
        });
    }

    [Fact]
    public async Task UpdateGroup_ValidGroup_CallsUpdateAsync()
    {
        // Arrange
        Course course = new Course { Id = 1, Name = "CourseName" };

        var groupToUpdate = new Group
        {
            Id = 1,
            Name = "test",
            CourseId = 1,
            Course = course
        };

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                        mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act
        await service.UpdateGroupAsync(groupToUpdate, cancellationToken);

        // Assert
        mockRepositoryGroup.Verify(repository => repository.UpdateAsync(groupToUpdate, cancellationToken), Times.Once);

    }

    [Fact]
    public async Task UpdateGroup_NullGroup_ThrowsCannotUpdateEntityException()
    {
        // Arrange

        Group? groupToUpdate = null;

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                        mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotUpdateEntityException>(() =>
        {
            return service.UpdateGroupAsync(groupToUpdate, cancellationToken);
        });
    }

    [Fact]
    public async Task DeleteGroup_WithNoStudents_ReturnsTrue()
    {
        // Arrange
        Course course = new Course { Id = 1, Name = "CourseName" };
        var groupToDelete = new Group
        {
            Id = 1,
            Name = "test",
            CourseId = 1,
            Course = course,
        };

        mockRepositoryGroup.Setup(repository => repository.GetByIdAsync(groupToDelete.Id, cancellationToken))
            .ReturnsAsync(groupToDelete);

        mockRepositoryStudent.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Student, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Student>());

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                        mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act
        var result = await service.DeleteGroupAsync(groupToDelete.Id, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteGroup_WithNullStudents_ThrowsCannotFindEntityException()
    {
        // Arrange
        Course course = new Course { Id = 1, Name = "CourseName" };
        var groupToDelete = new Group
        {
            Id = 1,
            Name = "test",
            CourseId = 1,
            Course = course
        };

        mockRepositoryGroup.Setup(repository => repository.GetByIdAsync(groupToDelete.Id, cancellationToken))
            .ReturnsAsync(groupToDelete);

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                        mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotFindEntityException>(() =>
        {
            return service.DeleteGroupAsync(groupToDelete.Id, cancellationToken);
        });
    }

    [Fact]
    public async Task DeleteGroup_WithStudents_ReturnsFalse()
    {
        // Arrange
        Course course = new Course { Id = 1, Name = "CourseName" };
        var groupToDelete = new Group
        {
            Id = 1,
            Name = "test",
            CourseId = 1,
            Course = course,
        };

        mockRepositoryGroup.Setup(repository => repository.GetByIdAsync(groupToDelete.Id, cancellationToken))
            .ReturnsAsync(groupToDelete);

        mockRepositoryStudent.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Student, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Student> { new Student { Id = 1, GroupId = groupToDelete.Id } });

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                        mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act
        var result = await service.DeleteGroupAsync(groupToDelete.Id, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetGroupByCourseIdAsync_ValidCourseId_ReturnsMatchingGroups()
    {
        int courseId = 1;
        // Arrange

        Course course = new Course { Id = 1, Name = "CourseName" };

        var groupToFind = new Group
        {
            Id = 1,
            Name = "test",
            CourseId = 1,
            Course = course
        };

        mockRepositoryGroup.Setup(repository => repository.GetByIdAsync(groupToFind.Id, cancellationToken))
        .ReturnsAsync(groupToFind);

        mockRepositoryGroup.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Group, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Group> { groupToFind });

        var service = new GroupService(mockRepositoryGroup.Object, mockRepositoryStudent.Object,
                                       mockRepositoryCourse.Object, mockRepositoryTeacher.Object);

        // Act
        var result = await service.GetGroupByCourseIdAsync(courseId, cancellationToken);

        // Assert
        Assert.Equal(groupToFind, result.First());
    }
}