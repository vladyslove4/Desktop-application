﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DesktopApp.Domain.Interfaces;

namespace DesktopApp.Domain.Entity;

public class Course : IEntity
{
    [Key]
    [Column("Course_Id")]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}