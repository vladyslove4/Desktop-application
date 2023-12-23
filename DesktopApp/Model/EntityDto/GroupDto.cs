using System.Collections.Generic;

namespace DesktopApp.Model.EntityDto;

public class GroupDto
{
    public int GroupId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public int CourseNumber { get; set; }

    public List<DropDownItem> Courses { get; set; } = new List<DropDownItem>();

    public DropDownItem SelectedCourse { get; set; } = new DropDownItem();

    public string CourseName { get; set; } = string.Empty;
    public int TeacherNumber { get; set; }
    public string TeacherName {  get; set; } = string.Empty;
    public string TeacherLastName { get; set; } = string.Empty;
    public List<DropDownItem> Teachers { get; set; } = new List<DropDownItem>();

    public DropDownItem SelectedTeacher { get; set; } = new DropDownItem();

    public bool Highlighted { get; set; }

    public int OldCourse {  get; set; } 

}