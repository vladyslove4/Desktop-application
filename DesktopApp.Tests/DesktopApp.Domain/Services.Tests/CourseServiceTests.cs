using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;
using DesktopApp.Domain.Services;
using Moq;
using System.Linq.Expressions;

namespace DesktopApp.Tests.DesktopApp.Domain.Services.Tests;

public class CourseServiceTests
{

    readonly CancellationToken cancellationToken;
    readonly Mock<IBaseRepository<Course>> mockRepositoryCourse;
    readonly Mock<IBaseRepository<Group>> mockRepositoryGroup;

    public CourseServiceTests()
    {
        cancellationToken = new CancellationToken();
        mockRepositoryCourse = new Mock<IBaseRepository<Course>>();
        mockRepositoryGroup = new Mock<IBaseRepository<Group>>();
    }

    [Fact]
    public async Task GetCourseById_ValidId_ReturnsCourse()
    {
        // Arrange
        int courseId = 1;

        var expectedCourse = new Course { Id = 1, Name = "test", Description = "testDescription" };

        mockRepositoryCourse.Setup(repository => repository.GetByIdAsync(courseId, cancellationToken)).ReturnsAsync(expectedCourse);

        var courseService = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act
        var result = await courseService.GetCourseByIdAsync(courseId, cancellationToken);

        // Assert
        Assert.Equal(expectedCourse, result);
    }

    [Fact]
    public async Task GetCourses_ReturnsListOfCourses()
    {
        // Arrange
        var expectedCourses = new List<Course>
    {
        new Course { Id = 1, Name = "test",  Description = "testDescription"},
        new Course { Id = 2, Name = "test1", Description = "testDescription1"}
    };

        mockRepositoryCourse.Setup(repository => repository.GetAllAsync(cancellationToken)).ReturnsAsync(expectedCourses);

        var service = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act
        var result = await service.GetCoursesAsync(cancellationToken);

        // Assert  
        Assert.Equal(expectedCourses, result);
    }

    [Fact]
    public async Task CreateCourse_ValidCourse_CallsCreateAsync()
    {
        // Arrange
        var expectedCourse = new Course
        {
            Id = 1,
            Name = "test",
            Description = "testDescription"
        };

        var service = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act
        await service.CreateCourseAsync(expectedCourse, cancellationToken);

        // Assert
        mockRepositoryCourse.Verify(repository => repository.CreateAsync(expectedCourse, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateCourse_NullCourse_ThrowsCannotCreateEntityException()
    {
        // Arrange

        Course? course = null;

        var service = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotCreateEntityException>(() =>
        {
            return service.CreateCourseAsync(course, cancellationToken);
        });
    }

    [Fact]
    public async Task UpdateCourse_ValidCourse_CallsUpdateAsync()
    {
        // Arrange
        var courseToUpdate = new Course
        {
            Id = 1,
            Name = "test",
            Description = "testDescription"
        };

        var service = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act
        await service.UpdateCourseAsync(courseToUpdate, cancellationToken);

        // Assert
        mockRepositoryCourse.Verify(repository => repository.UpdateAsync(courseToUpdate, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateCourse_NullCourse_ThrowsCannotUpdateEntityException()
    {
        // Arrange

        Course? courseToUpdate = null;

        var service = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotUpdateEntityException>(() =>
        {
            return service.UpdateCourseAsync(courseToUpdate, cancellationToken);
        });
    }

    [Fact]
    public async Task DeleteCourse_WithNoGroups_ReturnsTrue()
    {
        // Arrange
        var courseToDelete = new Course
        {
            Id = 1,
            Name = "test",
            Description = "testDescription"
        };

        mockRepositoryCourse.Setup(repository => repository.GetByIdAsync(courseToDelete.Id, cancellationToken))
            .ReturnsAsync(courseToDelete);

        mockRepositoryGroup.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Group, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Group>());

        var courseService = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act
        var result = await courseService.DeleteCourseAsync(courseToDelete.Id, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCourse_WithNullGroups_ThrowsCannotFindEntityException()
    {
        // Arrange

        var courseToDelete = new Course
        {
            Id = 1,
            Name = "test",
            Description = "testDescription"
        };

        mockRepositoryCourse.Setup(repository => repository.GetByIdAsync(courseToDelete.Id, cancellationToken))
            .ReturnsAsync(courseToDelete);

        var service = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotFindEntityException>(() =>
        {
            return service.DeleteCourseAsync(courseToDelete.Id, cancellationToken);
        });
    }

    [Fact]
    public async Task DeleteCourse_WithGroups_ReturnsFalse()
    {
        // Arrange
        var courseToDelete = new Course
        {
            Id = 1,
            Name = "test",
            Description = "testDescription"
        };

        mockRepositoryCourse.Setup(repository => repository.GetByIdAsync(courseToDelete.Id, cancellationToken))
            .ReturnsAsync(courseToDelete);

        mockRepositoryGroup.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Group, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Group> { new Group { Id = 1, CourseId = courseToDelete.Id } });

        var courseService = new CourseService(mockRepositoryCourse.Object, mockRepositoryGroup.Object);

        // Act
        var result = await courseService.DeleteCourseAsync(courseToDelete.Id, cancellationToken);

        // Assert
        Assert.False(result);
    }
}

