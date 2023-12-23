using DesktopApp.DAL.Repositories;
using DesktopApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DesktopApp.DAL;

public class DataSeeder
{
    private readonly ApplicationDbContext _applicationDbContext;

    public DataSeeder(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task SeedAsync()
    {
        if (!await _applicationDbContext.Courses.AnyAsync())
        {
            await SeedCoursesAsync();
        }
        if (!await _applicationDbContext.Groups.AnyAsync())
        {
            await SeedGroupsAsync();
        }
        if (!await _applicationDbContext.Students.AnyAsync())
        {
            await SeedStudentsAsync();
        }
        if (!await _applicationDbContext.Teachers.AnyAsync())
        {
            await SeedTeachersAsync();
        }
    }

    private async Task SeedCoursesAsync()
    {
        var courses = new List<Course>
    {
        new Course { Name = "Unreal Engine", Description = "Introduction to Unreal" },
        new Course { Name = "Android Development", Description = "Introduction to Android Development" },
        new Course { Name = "Automation QA", Description = "Introduction to QA" },
        new Course { Name = "Mobile Development", Description = "Introduction to Android Development" },
        new Course { Name = ".Net", Description = "Introduction to .Net" },
        new Course { Name = "Ios Development", Description = "Introduction to Ios" },
        new Course { Name = "Manual QA", Description = "Introduction to QA" },
        new Course { Name = "Go Lang", Description = "Introduction to Go" }
    };

        await _applicationDbContext.Courses.AddRangeAsync(courses);
        await _applicationDbContext.SaveChangesAsync();
    }

    private async Task SeedGroupsAsync()
    {
        var groups = new List<Group>
    {
        new Group { Name = "SR-01", CourseId = 1, TeacherId = 1 },
        new Group { Name = "SR-02", CourseId = 2, TeacherId = 2 },
        new Group { Name = "SR-03", CourseId = 3, TeacherId = 3 },
        new Group { Name = "SR-04", CourseId = 4, TeacherId = 4 },
        new Group { Name = "SR-05", CourseId = 5, TeacherId = 5 },
        new Group { Name = "SR-06", CourseId = 6, TeacherId = 6 },
        new Group { Name = "SR-07", CourseId = 7, TeacherId = 7 },
        new Group { Name = "SR-08", CourseId = 8, TeacherId = 8 }
    };

        await _applicationDbContext.Groups.AddRangeAsync(groups);
        await _applicationDbContext.SaveChangesAsync();
    }

    private async Task SeedStudentsAsync()
    {
        var students = new List<Student>
    {
        new Student { Name = "John", LastName = "Dopper", GroupId = 1 },
        new Student { Name = "Jane", LastName = "Ticks", GroupId = 2 },
        new Student { Name = "Mark", LastName = "Pitterson", GroupId = 3 },
        new Student { Name = "Michael", LastName = "Smith", GroupId = 4 },
        new Student { Name = "Mo", LastName = "Popper", GroupId = 1 },
        new Student { Name = "Bob", LastName = "Johnson", GroupId = 2 },
        new Student { Name = "John", LastName = "Wick", GroupId = 1 },
        new Student { Name = "Jane", LastName = "Ticks", GroupId = 3 },
        new Student { Name = "Mark", LastName = "Picker", GroupId = 2 },
    };

        await _applicationDbContext.Students.AddRangeAsync(students);
        await _applicationDbContext.SaveChangesAsync();
    }

    private async Task SeedTeachersAsync()
    {
        var teachers = new List<Teacher>
    {
        new Teacher { Name = "Johny", LastName = "Dopperson" },
        new Teacher { Name = "Bob", LastName = "Tickson" },
        new Teacher { Name = "Marko", LastName = "Pitterson" },
        new Teacher { Name = "Mich", LastName = "Smith" },
        new Teacher { Name = "Jack", LastName = "Popperson" },
        new Teacher { Name = "Boby", LastName = "Johnson" },
        new Teacher { Name = "Joanna", LastName = "Wickson" },
        new Teacher { Name = "Jax", LastName = "Tickson" },
        new Teacher { Name = "Mark", LastName = "Pickerson" },
    };

        await _applicationDbContext.Teachers.AddRangeAsync(teachers);
        await _applicationDbContext.SaveChangesAsync();
    }
}