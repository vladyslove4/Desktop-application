using DesktopApp.Domain.Entity;
using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;
using DesktopApp.Domain.Services;
using Moq;

namespace DesktopApp.Tests.DesktopApp.Domain.Services.Tests
{
    public class TeacherServiceTests
    {
        readonly CancellationToken cancellationToken;

        readonly Mock<IBaseRepository<Teacher>> mockRepositoryTeacher;


        public TeacherServiceTests()
        {
            cancellationToken = new CancellationToken();

            mockRepositoryTeacher = new Mock<IBaseRepository<Teacher>>();

        }

        [Fact]
        public async Task GetTeacherById_ValidId_ReturnTeacher()
        {
            //arrange

            int Id = 1;

            var expectedTeacher = new Teacher { Id = 1, Name = "test", LastName = "test"};

            mockRepositoryTeacher.Setup(repository => repository.GetByIdAsync(Id, cancellationToken)).ReturnsAsync(expectedTeacher);

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);
            //act
            var result = await teachertService.GetTeacherByIdAsync(Id, cancellationToken);

            //Assert

            Assert.Equal(expectedTeacher, result);
        }

        [Fact]
        public async Task GetTeachers_ReturnsListOfTeachers()
        {
            Group group = new Group { Name = "testGroup" };
            //Arrange
            var expectedTeachers = new List<Teacher>
        {
            new Teacher { Id = 1, LastName = "test", Name = "test"},
            new Teacher { Id = 2, LastName = "test",  Name = "test"}
        };

            mockRepositoryTeacher.Setup(repository => repository.GetAllAsync(cancellationToken)).ReturnsAsync(expectedTeachers);

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);

            //Act

            var result = await teachertService.GetTeachersAsync(cancellationToken);

            //Assert

            foreach (var teacher in result)
            {
                Assert.Equal("test", teacher.Name);
            }
        }

        [Fact]
        public async Task CreateTeacher_ValidTeacher_CallsCreateAsync()
        {
            // Arrange
            Group group = new Group { Id = 1, Name = "groupName" };

            var expectedTeacher = new Teacher { Id = 1, Name = "test", LastName = "test" };

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);

            // Act
            await teachertService.CreateTeacherAsync(expectedTeacher, cancellationToken);

            // Assert
            mockRepositoryTeacher.Verify(repository => repository.CreateAsync(expectedTeacher, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task CreateTeacher_NullStudent_ThrowsCannotCreateEntityException()
        {
            // Arrange

            Teacher? teacher = null;

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);

            // Act and Assert
            await Assert.ThrowsAsync<CannotCreateEntityException>(() =>
            {
                return teachertService.CreateTeacherAsync(teacher, cancellationToken);
            });
        }

        [Fact]
        public async Task UpdateTeacher_ValidTeacher_CallsUpdateAsync()
        {
            // Arrange

            var updTeacher = new Teacher { Id = 1, Name = "test", LastName = "test" };

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);

            // Act
            await teachertService.UpdateTeacherAsync(updTeacher, cancellationToken);

            // Assert
            mockRepositoryTeacher.Verify(repository => repository.UpdateAsync(updTeacher, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task UpdateTeacher_NullTeacher_ThrowsCannotUpdateEntityException()
        {
            // Arrange

            Teacher? teacher = null; 

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);

            // Act and Assert
            await Assert.ThrowsAsync<CannotUpdateEntityException>(() =>
            {
                return teachertService.UpdateTeacherAsync(teacher, cancellationToken);
            });
        }

        [Fact]
        public async Task DeleteTeacher_ValidTeacher_ReturnsTrue()
        {
            // Arrange
            var teacherToDelete = new Teacher
            {
                Id = 1,
                Name = "test",
                LastName = "test"
            };

            mockRepositoryTeacher.Setup(repository => repository.GetByIdAsync(teacherToDelete.Id, cancellationToken))
                .ReturnsAsync(teacherToDelete);

            //mockRepositoryGroup.Setup(repository => repository.FindAsync(It.IsAny<Expression<Func<Group, bool>>>(), cancellationToken))
            //    .ReturnsAsync(new List<Group>());

            var teachertService = new TeacherService(mockRepositoryTeacher.Object);

            // Act
            await teachertService.DeleteTeacherAsync(teacherToDelete.Id, cancellationToken);

            // Assert
            mockRepositoryTeacher.Verify(repository => repository.DeleteAsync(teacherToDelete, cancellationToken), Times.Once);
        }



    }
}
