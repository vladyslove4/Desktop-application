using DesktopApp.Command;
using DesktopApp.Model;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace DesktopApp.ViewModel;

class HomeViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    private NavigationService _navigation;
    private CancellationToken cancellationToken = new CancellationToken();

    public List<TreeViewItem> TreeViewItems { get; set; }
    public RellayCommand? makeNavigationCommand;

    private ObservableCollection<GroupDto> _groups;
    public ObservableCollection<GroupDto> Groups
    {
        get { return _groups; }
        set
        {
            _groups = value;


        }

    }

    private ObservableCollection<StudentDto> _students;
    public ObservableCollection<StudentDto> Students
    {
        get { return _students; }
        set
        {
            _students = value;
        }
    }

    private ObservableCollection<CourseDto> _courses;

    public ObservableCollection<CourseDto> Courses
    {
        get { return _courses; }
        set
        {
            _courses = value;


        }
    }

    public HomeViewModel(DataService dataService, NavigationService navigation)
    {
        _dataService = dataService;
        Courses = dataService.Courses;
        Groups = dataService.Groups;
        Students = dataService.Students;
        _navigation = navigation;

        TreeViewItems = new List<TreeViewItem>();

        LoadDataAsync();

    }

    public async void LoadDataAsync()
    {
        await _dataService.LoadCoursesAsync(cancellationToken);
        await _dataService.LoadGroupAsync(cancellationToken);
        await _dataService.LoadStudentAsync(cancellationToken);

        PopulateTreeView(cancellationToken);

        _groups.CollectionChanged += GroupsCollectionChanged;
        _courses.CollectionChanged += CoursesCollectionChanged;
        _students.CollectionChanged += StudentsCollectionChanged;

        TreeView = TreeViewItems;
    }

    private void CoursesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {

        PopulateTreeView(cancellationToken);

    }

    private void GroupsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {

        PopulateTreeView(cancellationToken);

    }

    private void StudentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PopulateTreeView(cancellationToken);
    }

    private List<TreeViewItem> treeView;

    public List<TreeViewItem> TreeView
    {
        get { return treeView; }
        set
        {
            treeView = value;
            OnPropertyChanged(nameof(TreeView));
        }
    }

    public List<TreeViewItem> PopulateTreeView(CancellationToken cancellationToken)
    {
        var sortedCourses = Courses.OrderBy(c => c.CourseId);
        var sortedGroups = Groups.OrderBy(g => g.GroupId);
        var sortedStudents = Students.OrderBy(s => s.StudentId);

        TreeViewItems.Clear();

        foreach (var course in sortedCourses)
        {
            var courseNode = new TreeViewItem { Name = course.Name, Id = course.CourseId, Category = 1, Children = new List<TreeViewItem>() };
            TreeViewItems.Add(courseNode);

            foreach (var group in sortedGroups.Where(g => g.CourseNumber == course.CourseId))
            {
                var groupNode = new TreeViewItem { Name = group.Name, Id = group.GroupId, Category = 2, Children = new List<TreeViewItem>() };
                courseNode.Children.Add(groupNode);

                foreach (var student in sortedStudents.Where(s => s.GroupNumber == group.GroupId))
                {
                    var studentNode = new TreeViewItem { Name = student.FirstName + " " + student.LastName, Id = student.StudentId, Category = 3 };
                    groupNode.Children.Add(studentNode);
                }
            }
        }

        TreeView = TreeViewItems;
        return TreeViewItems;
    }

    public RellayCommand MakeNavigationCommand
    {
        get
        {
            return makeNavigationCommand ??
              (makeNavigationCommand = new RellayCommand(async (selectedItem) =>
              {
                  if (selectedItem != null)
                  {
                      var item = selectedItem as TreeViewItem;

                      switch (item.Category)
                      {
                          case 1:


                              _navigation.NavigateTo<CoursesViewModel>();

                              MessagingService.Instance.ChangeSelectedCourse(item.Id);

                              break;

                          case 2:

                              _navigation.NavigateTo<GroupsViewModel>();

                              MessagingService.Instance.ChangeSelectedGroup(item.Id);

                              break;

                          case 3:

                              _navigation.NavigateTo<StudentsViewModel>();

                              MessagingService.Instance.ChangeSelectedStudent(item.Id);

                              break;

                          default:

                              break;
                      }
                  }
              }));
        }
    }
}