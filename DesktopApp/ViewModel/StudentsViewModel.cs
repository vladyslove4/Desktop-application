using DesktopApp.Command;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using DesktopApp.View.Student;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace DesktopApp.ViewModel;

class StudentsViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    CancellationToken cancellationToken = new CancellationToken();

    RellayCommand? addStudentCommand;
    RellayCommand? editStudentCommand;
    RellayCommand? deleteStudentCommand;
    public ObservableCollection<StudentDto> Students => _dataService.Students;

    public StudentsViewModel(DataService dataService)
    {
        _dataService = dataService;

        LoadStudentsAsync();

        MessagingService.Instance.StudentSelected += OnCategorySelected;
    }


    private void OnCategorySelected(int Id)
    {
        if (Id == -1)
        {

            foreach (var student in Students)
            {
                student.Highlighted = false;
            }
            return;
        }


        foreach (var student in Students)
        {
            if (student.StudentId == Id)
            {
                student.Highlighted = true;
            }

        }
    }

    public async void LoadStudentsAsync()
    {
        if (Students.Count == 0)
        {

            await _dataService.LoadStudentAsync(cancellationToken);
        }
    }

    public RellayCommand AddStudentCommand
    {
        get
        {
            return addStudentCommand ??
              (addStudentCommand = new RellayCommand(async (o) =>
              {
                  var student = await _dataService.GetGroupsforStudent(cancellationToken);

                  AddEditStudent viewStudent = new AddEditStudent(student);
                  if (viewStudent.ShowDialog() == true)
                  {
                      StudentDto studentDto = new StudentDto
                      {
                          FirstName = viewStudent._studentDto.FirstName,
                          LastName = viewStudent._studentDto.LastName,
                          GroupNumber = viewStudent._studentDto.SelectedGroup.Id,
                          GroupName = viewStudent._studentDto.SelectedGroup.Name
                      };

                      await _dataService.CreateStudentAsync(studentDto, cancellationToken);
                  }
              }));
        }
    }

    public RellayCommand EditStudentCommand
    {
        get
        {
            return editStudentCommand ??
              (editStudentCommand = new RellayCommand(async (selectedItem) =>
              {
                  StudentDto? studentDto = selectedItem as StudentDto;

                  if (studentDto == null)
                  {
                      MessageBox.Show("Student not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  //add validation

                  var student = await _dataService.GetGroupsforStudent(cancellationToken);

                  var selected = student.Groups.Find(x => x.Id == studentDto.GroupNumber) ?? null;

                  StudentDto updStudent = new StudentDto
                  {
                      FirstName = studentDto.FirstName,
                      LastName = studentDto.LastName,
                      GroupNumber = studentDto.GroupNumber,
                      Groups = student.Groups,
                      SelectedGroup = selected
                  };

                  AddEditStudent viewStudent = new AddEditStudent(updStudent); ;

                  if (viewStudent.ShowDialog() == true)
                  {
                      updStudent.StudentId = studentDto.StudentId;
                      updStudent.FirstName = viewStudent._studentDto.FirstName;
                      updStudent.GroupNumber = viewStudent._studentDto.SelectedGroup.Id;
                      updStudent.GroupName = viewStudent._studentDto.SelectedGroup.Name;

                      await _dataService.UpdateStudentAsync(updStudent, cancellationToken);
                  }
              }));
        }
    }

    public RellayCommand DeleteStudentCommand
    {
        get
        {
            return deleteStudentCommand ??
              (deleteStudentCommand = new RellayCommand(async (selectedItem) =>
              {

                  StudentDto? studentDto = selectedItem as StudentDto;

                  if (studentDto == null)
                  {
                      MessageBox.Show("Student not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }


                  await _dataService.DeleteStudentAsync(studentDto, cancellationToken);

              }));
        }
    }
}