using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Interfaces;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using Moq;

namespace DesktopApp.Tests.DesktopApp.Service;

public class DataServiceTests
{
    readonly CancellationToken cancellationToken;
    private DataService dataService;

    private Mock<ICourseService> courseServiceMock;
    private Mock<IGroupService> groupServiceMock;
    private Mock<IStudentService> studentServiceMock;
    private Mock<ITeacherService> teacherServiceMock;

    public DataServiceTests()
    {
        cancellationToken = new CancellationToken();
        courseServiceMock = new Mock<ICourseService>();
        groupServiceMock = new Mock<IGroupService>();
        studentServiceMock = new Mock<IStudentService>();
        teacherServiceMock = new Mock<ITeacherService>();
    }

    [Fact]
    public async Task LoadCoursesAsync_Should_LoadCoursesIntoCoursesCollection()
    {
        // Arrange

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                         studentServiceMock.Object, teacherServiceMock.Object);

        var courses = new List<Course> { new Course { Id = 1, Name = "Test" } };

        courseServiceMock.Setup(c => c.GetCoursesAsync(cancellationToken))
            .ReturnsAsync(courses);

        // Act
        await dataService.LoadCoursesAsync(cancellationToken);

        // Assert
        Assert.Equal(courses.Count, dataService.Courses.Count);

    }

    [Fact]
    public async Task LoadGroupsAsync_Should_LoadGroupsIntoGroupsCollection()
    {
        // Arrange

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                         studentServiceMock.Object, teacherServiceMock.Object);

        var groups = new List<Group> { new Group { Id = 1, Name = "Test", Course = new Course { }, CourseId = 1, TeacherId = 1 } };

        groupServiceMock.Setup(c => c.GetGroupsAsync(cancellationToken))
            .ReturnsAsync(groups);

        // Act
        await dataService.LoadGroupAsync(cancellationToken);

        // Assert
        Assert.Equal(groups.Count, dataService.Groups.Count);

    }

    [Fact]
    public async Task LoadStudentsAsync_Should_LoadStudentIntoStudentsCollection()
    {
        // Arrange

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                         studentServiceMock.Object, teacherServiceMock.Object);

        var students = new List<Student> { new Student { Id = 1, Name = "Test", Group = new Group { }, GroupId = 1 } };

        studentServiceMock.Setup(c => c.GetStudentsAsync(cancellationToken))
            .ReturnsAsync(students);

        // Act
        await dataService.LoadStudentAsync(cancellationToken);

        // Assert
        Assert.Equal(students.Count, dataService.Students.Count);

    }

    [Fact]
    public async Task LoadTeachersAsync_Should_LoadTeachersIntoTeachersCollection()
    {
        // Arrange

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                         studentServiceMock.Object, teacherServiceMock.Object);

        var teachers = new List<Teacher> { new Teacher { Id = 1, Name = "Test" } };

        teacherServiceMock.Setup(c => c.GetTeachersAsync(cancellationToken))
            .ReturnsAsync(teachers);

        // Act
        await dataService.LoadTeacherAsync(cancellationToken);

        // Assert
        Assert.Equal(teachers.Count, dataService.Teachers.Count);

    }

    [Fact]
    public async Task UpdateCourseAsync_Should_UpdateCourseInCoursesCollection()
    {
        // Arrange

        var courseDto = new CourseDto { CourseId = 1, Name = "Test" };


        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        await dataService.UpdateCourseAsync(courseDto, cancellationToken);

        // Assert

        courseServiceMock.Verify(c => c.UpdateCourseAsync(It.IsAny<Course>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateGroupAsync_Should_UpdateGroupInGroupsCollection()
    {
        // Arrange

        var groupDto = new GroupDto { GroupId = 1, Name = "Test" };


        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        await dataService.UpdateGroupAsync(groupDto, cancellationToken);

        // Assert

        groupServiceMock.Verify(c => c.UpdateGroupAsync(It.IsAny<Group>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateStudentAsync_Should_UpdateStudentInStudentsCollection()
    {
        // Arrange

        var studentDto = new StudentDto { StudentId = 1, FirstName = "Test" };


        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        await dataService.UpdateStudentAsync(studentDto, cancellationToken);

        // Assert

        studentServiceMock.Verify(c => c.UpdateStudentAsync(It.IsAny<Student>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateTeacherAsync_Should_UpdateTeacherInTeachersCollection()
    {
        // Arrange

        var teacherDto = new TeacherDto { TeacherId = 1, FirstName = "Test" };


        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        await dataService.UpdateTeacherAsync(teacherDto, cancellationToken);

        // Assert

        teacherServiceMock.Verify(c => c.UpdateTeacherAsync(It.IsAny<Teacher>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteCourseAsync_WhenDeleteCourseSucceeds_ShouldReturnTrue()
    {
        // Arrange
        
        var course = new CourseDto { CourseId = 1 }; 
        
        courseServiceMock.Setup(service => service.DeleteCourseAsync(course.CourseId, cancellationToken))
            .ReturnsAsync(true); 

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        var result = await dataService.DeleteCourseAsync(course, cancellationToken);

        // Assert
        Assert.True(result);
        courseServiceMock.Verify(service => service.DeleteCourseAsync(course.CourseId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteCourseAsync_WhenDeleteCourseFails_ShouldReturnFalse()
    {
        // Arrange

        var course = new CourseDto { CourseId = 1 };

        courseServiceMock.Setup(service => service.DeleteCourseAsync(course.CourseId, cancellationToken))
            .ReturnsAsync(false);

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        var result = await dataService.DeleteCourseAsync(course, cancellationToken);

        // Assert
        Assert.False(result);
        courseServiceMock.Verify(service => service.DeleteCourseAsync(course.CourseId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteGroupAsync_WhenDeleteGroupSucceeds_ShouldReturnTrue()
    {
        // Arrange

        var group = new GroupDto { GroupId = 1 };

        groupServiceMock.Setup(service => service.DeleteGroupAsync(group.GroupId, cancellationToken))
            .ReturnsAsync(true);

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        var result = await dataService.DeleteGroupAsync(group, cancellationToken);

        // Assert
        Assert.True(result);
        groupServiceMock.Verify(service => service.DeleteGroupAsync(group.GroupId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteGroupAsync_WhenDeleteGroupFails_ShouldReturnFalse()
    {
        /// Arrange

        var group = new GroupDto { GroupId = 1 };

        groupServiceMock.Setup(service => service.DeleteGroupAsync(group.GroupId, cancellationToken))
            .ReturnsAsync(false);

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        var result = await dataService.DeleteGroupAsync(group, cancellationToken);

        // Assert
        Assert.False(result);
        groupServiceMock.Verify(service => service.DeleteGroupAsync(group.GroupId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteStudentAsync_WhenDeleteStudentSucceeds_ShouldReturnTrue()
    {
        // Arrange

        var student = new StudentDto { StudentId = 1 };

        studentServiceMock.Setup(service => service.DeleteStudentAsync(student.StudentId, cancellationToken))
            .ReturnsAsync(true);

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        var result = await dataService.DeleteStudentAsync(student, cancellationToken);

        // Assert
        Assert.True(result);
        studentServiceMock.Verify(service => service.DeleteStudentAsync(student.StudentId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteTeacherAsync_WhenDeleteTeacherSucceeds_ShouldReturnTrue()
    {
        // Arrange

        var teacher = new TeacherDto { TeacherId = 1 };

        teacherServiceMock.Setup(service => service.DeleteTeacherAsync(teacher.TeacherId, cancellationToken))
            .ReturnsAsync(true);

        dataService = new DataService(courseServiceMock.Object, groupServiceMock.Object,
                                        studentServiceMock.Object, teacherServiceMock.Object);

        // Act
        var result = await dataService.DeleteTeacherAsync(teacher, cancellationToken);

        // Assert
        Assert.True(result);
        teacherServiceMock.Verify(service => service.DeleteTeacherAsync(teacher.TeacherId, cancellationToken), Times.Once);
    }

}



