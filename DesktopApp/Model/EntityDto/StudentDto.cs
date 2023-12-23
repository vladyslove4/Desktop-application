using System.Collections.Generic;

namespace DesktopApp.Model.EntityDto;

public class StudentDto
{
    public int StudentId { get; set; }

    public int GroupNumber { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public List<DropDownItem> Groups { get; set; } = new List<DropDownItem>();

    public DropDownItem SelectedGroup { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public bool Highlighted { get; set; }
}