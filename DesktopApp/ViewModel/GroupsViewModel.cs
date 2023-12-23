using DesktopApp.Command;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using DesktopApp.View.Group;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace DesktopApp.ViewModel;

class GroupsViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    private string selectedFilePath;
    private CsvService _csvService;
    private PdfService _pdfService;
    private CancellationToken cancellationToken = new CancellationToken();

    public ObservableCollection<GroupDto> Groups => _dataService.Groups;
    public ObservableCollection<StudentDto> Students => _dataService.Students;
    public RellayCommand? addGroupCommand;
    public RellayCommand? editGroupCommand;
    public RellayCommand? deleteGroupCommand;
    public RellayCommand? exportToCSV;
    public RellayCommand? exportToPDF;
    public RellayCommand? importCSV;

    public GroupsViewModel(DataService dataService)
    {
        _dataService = dataService;

        _csvService = new CsvService();
        _pdfService = new PdfService();

        LoadAsync();

        MessagingService.Instance.GroupSelected += OnCategorySelected;
    }

    private void OnCategorySelected(int Id)
    {
        if (Id == -1)
        {

            foreach (var group in Groups)
            {
                group.Highlighted = false;
            }
            return;
        }


        foreach (var group in Groups)
        {
            if (group.GroupId == Id)
            {
                group.Highlighted = true;
            }

        }


    }

    public async void LoadAsync()
    {
        if (Groups.Count == 0)
        {
            await _dataService.LoadGroupAsync(cancellationToken);
        }
    }

    public RellayCommand AddGroupCommand
    {
        get
        {
            return addGroupCommand ??
              (addGroupCommand = new RellayCommand(async (o) =>
              {
                  var group = await _dataService.GetDropdownForGroup(cancellationToken);
                  AddEditGroup viewGroup = new AddEditGroup(group);
                  if (viewGroup.ShowDialog() == true)
                  {
                      GroupDto groupDto = new GroupDto
                      {
                          Name = viewGroup._groupDto.Name,
                          CourseNumber = viewGroup._groupDto.SelectedCourse.Id,
                          CourseName = viewGroup._groupDto.SelectedCourse.Name,
                          TeacherName = viewGroup._groupDto.SelectedTeacher.Name,
                          TeacherNumber = viewGroup._groupDto.SelectedTeacher.Id

                      };

                      await _dataService.CreateGroupAsync(groupDto, cancellationToken);

                  }
              }));
        }
    }

    public RellayCommand EditGroupCommand
    {
        get
        {
            return editGroupCommand ??
              (editGroupCommand = new RellayCommand(async (selectedItem) =>
              {
                  GroupDto? groupDto = selectedItem as GroupDto;

                  if (groupDto == null)
                  {
                      MessageBox.Show("Group not selected or doesn't exist.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  //add validation

                  var dropdownItems = await _dataService.GetDropdownForGroup(cancellationToken);

                  var selectedCourse = dropdownItems.Courses.Find(x => x.Id == groupDto.CourseNumber) ?? null;
                  var selectedTeacher = dropdownItems.Teachers.Find(x => x.Id == groupDto.CourseNumber) ?? null;

                  GroupDto updGroup = new GroupDto
                  {
                      Name = groupDto.Name,
                      OldCourse = groupDto.CourseNumber,
                      CourseName = groupDto.CourseName,
                      Courses = dropdownItems.Courses,
                      SelectedCourse = selectedCourse,
                      Teachers = dropdownItems.Teachers,
                      SelectedTeacher = selectedTeacher
                  };

                  AddEditGroup viewGroup = new AddEditGroup(updGroup);

                  if (viewGroup.ShowDialog() == true)
                  {
                      updGroup.GroupId = groupDto.GroupId;
                      updGroup.Name = viewGroup._groupDto.Name;
                      updGroup.CourseNumber = viewGroup._groupDto.SelectedCourse.Id;
                      updGroup.CourseName = viewGroup._groupDto.SelectedCourse.Name;
                      updGroup.TeacherNumber = viewGroup._groupDto.SelectedTeacher.Id;


                      updGroup.TeacherName = viewGroup._groupDto.SelectedTeacher.Name;


                      ///Check how group will updated here

                      await _dataService.UpdateGroupAsync(updGroup, cancellationToken);

                  }
              }));
        }
    }

    public RellayCommand DeleteGroupCommand
    {
        get
        {
            return deleteGroupCommand ??
              (deleteGroupCommand = new RellayCommand(async (selectedItem) =>
              {

                  GroupDto? groupDto = selectedItem as GroupDto;

                  if (groupDto == null)
                  {
                      MessageBox.Show("Group not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  if (!await _dataService.DeleteGroupAsync(groupDto, cancellationToken))
                  {
                      MessageBox.Show("The group cannot be deleted because it contains students.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;

                  };

              }));
        }
    }

    public RellayCommand ExportToCSV
    {
        get
        {
            return exportToCSV ??
              (exportToCSV = new RellayCommand(async (selectedItem) =>
              {
                  GroupDto? groupDto = selectedItem as GroupDto;

                  if (groupDto == null)
                  {
                      MessageBox.Show("Group not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  var studentsById = _dataService.GetStudentsByGroupIdAsync(groupDto.GroupId, cancellationToken);

                  var csvServise = await _csvService.ExportCSV(studentsById);

                  if (csvServise)
                  {
                      MessageBox.Show($"The group {groupDto.Name} was exported to CSV in the Root folder with name ListOfStudentCSV;.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                  }

              }));
        }
    }

    public RellayCommand ExportToPDF
    {
        get
        {
            return exportToPDF ??
              (exportToPDF = new RellayCommand(async(selectedItem) =>
              {
                  GroupDto? groupDto = selectedItem as GroupDto;

                  if (groupDto == null)
                  {
                      MessageBox.Show("Group not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  var studentsById = _dataService.GetStudentsByGroupIdAsync(groupDto.GroupId, cancellationToken);
                  var pdfService = await _pdfService.ExportPDF(studentsById, groupDto.Name, groupDto.CourseName,cancellationToken);

                  if (pdfService)
                  {
                      MessageBox.Show($"The group {groupDto.Name} was exported to PDF in the Root folder with name StudentListPDF;.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                  }

              }));
        }
    }

    public RellayCommand ImportCSV
    {
        get
        {
            return importCSV ??
              (importCSV = new RellayCommand(async (selectedItem) =>
              {
                  GroupDto? groupDto = selectedItem as GroupDto;

                  if (groupDto == null)
                  {
                      MessageBox.Show("Group not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  OpenFileDialog openFileDialog = new OpenFileDialog();

                  if (openFileDialog.ShowDialog() == true)
                  {

                      selectedFilePath = openFileDialog.FileName;
                      var csvStudents = _csvService.ImportCSV(selectedFilePath);
                      if (csvStudents != null)
                      {
                          var studentsById = _dataService.GetStudentsByGroupIdAsync(groupDto.GroupId, cancellationToken);
                          foreach (var student in studentsById)
                          {
                              await _dataService.DeleteStudentAsync(student, cancellationToken);
                          }

                          foreach (var student in csvStudents)
                          {
                              StudentDto studentDto = new StudentDto
                              {
                                  FirstName = student.FirstName,
                                  LastName = student.LastName,
                                  GroupNumber = groupDto.GroupId
                              };

                              await _dataService.CreateStudentAsync(studentDto, cancellationToken);
                          }

                          MessageBox.Show($"Students was imported to group {groupDto.GroupId} with name {groupDto.Name}.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      }
                      else
                      {

                          MessageBox.Show($"Choose correct CSV file.", "Information",
                                        MessageBoxButton.OK, MessageBoxImage.Information);

                      }

                  }
                  else
                  {
                      MessageBox.Show($"Choose CSV file.", "Information",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                  }

              }));
        }
    }
}