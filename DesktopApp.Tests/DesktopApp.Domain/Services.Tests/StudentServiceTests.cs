
using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;
using DesktopApp.Domain.Services;
using Moq;
using System.Linq.Expressions;

namespace DesktopApp.Tests.DesktopApp.Domain.Services.Tests;

public class StudentServiceTests
{
    readonly CancellationToken cancellationToken;
    readonly Mock<IBaseRepository<Course>> mockRepositoryCourse;
    readonly Mock<IBaseRepository<Student>> mockRepositoryStudent;
    readonly Mock<IBaseRepository<Group>> mockRepositoryGroup;

    public StudentServiceTests()
    {
        cancellationToken = new CancellationToken();
        mockRepositoryCourse = new Mock<IBaseRepository<Course>>();
        mockRepositoryGroup = new Mock<IBaseRepository<Group>>();
        mockRepositoryStudent = new Mock<IBaseRepository<Student>>();
    }

    [Fact]
    public async Task GetStudentById_ValidId_ReturnStudent()
    {
        //arrange

        int Id = 1;

        var exprctedStudent = new Student { Id = 1, Name = "test", LastName = "test", GroupId = 2 };

        mockRepositoryStudent.Setup(repository => repository.GetByIdAsync(Id, cancellationToken)).ReturnsAsync(exprctedStudent);

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);
        //act
        var result = await studentService.GetStudentByIdAsync(Id, cancellationToken);

        //Assert

        Assert.Equal(exprctedStudent, result);
    }

    [Fact]
    public async Task GetStudents_ReturnsListOfStudents()
    {
        Group group = new Group { Name = "testGroup" };
        //Arrange
        var expectedStudents = new List<Student>
        {
            new Student { Id = 1, LastName = "test", Group = group, Name = "test"},
            new Student { Id = 2, LastName = "test", Group = group, Name = "test"}
        };

        mockRepositoryStudent.Setup(repository => repository.GetAllAsync(cancellationToken)).ReturnsAsync(expectedStudents);

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        //Act

        var result = await studentService.GetStudentsAsync(cancellationToken);

        //Assert

        foreach (var student in result)
        {
            Assert.Equal("testGroup", student.Group.Name);
        }
    }

    [Fact]
    public async Task CreateStudent_ValidStudent_CallsCreateAsync()
    {
        // Arrange
        Group group = new Group { Id = 1, Name = "groupName" };
        var expectedStudent = new Student
        {
            Id = 1,
            Name = "name",
            LastName = "lastName",
            Group = group,
        };

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        // Act
        await studentService.CreateStudentAsync(expectedStudent, cancellationToken);

        // Assert
        mockRepositoryStudent.Verify(repository => repository.CreateAsync(expectedStudent, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateStudent_NullStudent_ThrowsCannotCreateEntityException()
    {
        // Arrange

        Student? student = null;

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotCreateEntityException>(() =>
        {
            return studentService.CreateStudentAsync(student, cancellationToken);
        });
    }

    [Fact]
    public async Task UpdateStudent_ValidStudent_CallsUpdateAsync()
    {
        // Arrange
        Group group = new Group { Id = 1, Name = "groupName" };
        var studentToUpdate = new Student
        {
            Id = 1,
            Name = "name",
            LastName = "lastName",
            Group = group
        };

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        // Act
        await studentService.UpdateStudentAsync(studentToUpdate, cancellationToken);

        // Assert
        mockRepositoryStudent.Verify(repository => repository.UpdateAsync(studentToUpdate, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateStudent_NullStudent_ThrowsCannotUpdateEntityException()
    {
        // Arrange

        Student? studentToUpdate = null;

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        // Act and Assert
        await Assert.ThrowsAsync<CannotUpdateEntityException>(() =>
        {
            return studentService.UpdateStudentAsync(studentToUpdate, cancellationToken);
        });
    }

    [Fact]
    public async Task DeleteGroup_WithNoStudents_ReturnsTrue()
    {
        // Arrange
        var studentToDelete = new Student
        {
            Id = 1,
            Name = "test",
        };

        mockRepositoryStudent.Setup(repository => repository.GetByIdAsync(studentToDelete.Id, cancellationToken)).ReturnsAsync(studentToDelete);

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        // Act
        await studentService.DeleteStudentAsync(studentToDelete.Id, cancellationToken);

        // Assert
        mockRepositoryStudent.Verify(repository => repository.DeleteAsync(studentToDelete, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetStudentByGroupIdAsync_ValidCourseId_ReturnsMatchingStudents()
    {
        int groupId = 1;
        Group group = new Group { Id = 1, Name = "groupName" };
        // Arrange
        var studentToFind = new Student
        {
            Id = 1,
            Name = "name",
            LastName = "lastName",
            Group = group
        };

        mockRepositoryStudent.Setup(repository => repository.GetByIdAsync(studentToFind.Id, cancellationToken))
        .ReturnsAsync(studentToFind);

        mockRepositoryStudent.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Student, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Student> { studentToFind });

        var studentService = new StudentService(mockRepositoryStudent.Object, mockRepositoryGroup.Object);

        // Act
        var result = await studentService.GetStudentByGroupIdAsync(groupId, cancellationToken);

        // Assert
        Assert.Equal(studentToFind, result.First());
    }
}

