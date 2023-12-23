using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Entity;

public class Student : IEntity
{
    [Key]
    [Column("Student_Id")]
    public int Id { get; set; }

    [ForeignKey("Group")]
    public int GroupId { get; set; }

    
    public string Name { get; set; } = string.Empty;

    
    public string LastName { get; set; } = string.Empty;

    
    public Group? Group { get; set; }

}
