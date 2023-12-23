using DesktopApp.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApp.Domain.Entity
{
    public class Teacher : IEntity
    {
        [Key]
        [Column("Teacher_Id")]
        public int Id { get; set; }

        [Column("First_Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Last_Name")]
        public string LastName { get; set; } = string.Empty;

    }
}
