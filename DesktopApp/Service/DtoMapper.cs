using DesktopApp.Domain.Entity;
using DesktopApp.Model.EntityDto;

namespace DesktopApp.Service;

public static class DtoMapper
{
    public static CourseDto CourseToCourseDto(Course course)
    {
        return new CourseDto
        {
            CourseId = course.Id,
            Name = course.Name,
            Description = course.Description
        };
    }

    public static Course CourseDtoToCourse(CourseDto courseDto)
    {
        return new Course
        {
            Id = courseDto.CourseId,
            Name = courseDto.Name,
            Description = courseDto.Description
        };
    }

    public static GroupDto GroupToGroupDto(Group group)
    {
        return new GroupDto
        {
            GroupId = group.Id,
            Name = group.Name,
            CourseNumber = group.CourseId,
            CourseName = group.Course.Name,
            TeacherNumber = group.TeacherId,
            TeacherName = $"{group.TeacherName}",

        };
    }

    public static Group GroupDtoToGroup(GroupDto group)
    {
        return new Group
        {
            Id = group.GroupId,
            Name = group.Name,
            CourseId = group.CourseNumber,
            TeacherId = group.TeacherNumber
        };
    }

    public static StudentDto StudentToStudentDto(Student student)
    {
        return new StudentDto
        {
            StudentId = student.Id,
            FirstName = student.Name,
            LastName = student.LastName,
            GroupNumber = student.GroupId,
            GroupName = student.Group.Name
        };
    }

    public static Student StudentDtoToStudent(StudentDto student)
    {
        return new Student
        {
            Id = student.StudentId,
            Name = student.FirstName,
            LastName = student.LastName,
            GroupId = student.GroupNumber,
        };
    }

    public static TeacherDto TeacherToTeacherDto(Teacher teacher)
    {
        return new TeacherDto
        {
            TeacherId = teacher.Id,
            FirstName = teacher.Name,
            LastName = teacher.LastName,
        };
    }

    public static Teacher TeacherDtoToTeacher(TeacherDto teacher)
    {
        return new Teacher
        {
            Id = teacher.TeacherId,
            Name = teacher.FirstName,
            LastName = teacher.LastName,
        };
    }
}