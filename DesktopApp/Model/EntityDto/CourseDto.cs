namespace DesktopApp.Model.EntityDto;

public class CourseDto
{
    public int CourseId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Highlighted { get; set; }
}