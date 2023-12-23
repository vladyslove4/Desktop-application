using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Entity;

public class Group : IEntity
{
    [Key]
    [Column("Group_Id")]
    public int Id { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }

    public string Name { get; set; } = string.Empty;

    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public Course? Course { get; set; }
}
