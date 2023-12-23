using System.Collections.Generic;

namespace DesktopApp.Model;

public class TreeViewItem
{
    public string Name { get; set; } = string.Empty;
    public int Category { get; set; }
    public int Id { get; set; }
  
    public List<TreeViewItem> Children { get; set; } = new List<TreeViewItem>();
}
