using DesktopApp.Command;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using DesktopApp.View.Teacher;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace DesktopApp.ViewModel;

public class TeachersViewModel: ViewModelBase
{
    private readonly DataService _dataService;
    CancellationToken cancellationToken = new CancellationToken();
    
    public RellayCommand? addTeacherCommand;
    public RellayCommand? editTeacherCommand;
    public RellayCommand? deleteTeacherCommand;
    public ObservableCollection<TeacherDto> Teachers => _dataService.Teachers;

    public TeachersViewModel(DataService dataService)
    {
        _dataService = dataService;

        LoadTeachersAsync();
    }

    public async void LoadTeachersAsync()
    {
        if (Teachers.Count == 0)
        {
            await _dataService.LoadTeacherAsync(cancellationToken);
        }
    }

    public RellayCommand AddTeacherCommand
    {
        get
        {
            return addTeacherCommand ??
              (addTeacherCommand = new RellayCommand(async (o) =>
              {
                  AddEditTeacher viewTeacher = new AddEditTeacher(new TeacherDto());
                  if (viewTeacher.ShowDialog() == true)
                  {
                      TeacherDto teacherDto = viewTeacher._teacherDto;

                      await _dataService.CreateTeacherAsync(teacherDto, cancellationToken);
                      
                  }
              }));
        }
    }

    public RellayCommand EditTeacherCommand
    {
        get
        {
            return editTeacherCommand ??
              (editTeacherCommand = new RellayCommand(async (selectedItem) =>
              {

                  TeacherDto? teacherDto = selectedItem as TeacherDto;

                  if (teacherDto == null)
                  {
                      MessageBox.Show("Teacher not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }

                  TeacherDto updTeacher = new TeacherDto
                  {
                      FirstName = teacherDto.FirstName,
                      LastName = teacherDto.LastName
                  };
                  AddEditTeacher viewTeacher = new AddEditTeacher(updTeacher);


                  if (viewTeacher.ShowDialog() == true)
                  {
                      updTeacher.TeacherId = teacherDto.TeacherId;
                      updTeacher.FirstName = viewTeacher._teacherDto.FirstName;
                      updTeacher.LastName = viewTeacher._teacherDto.LastName;

                      await _dataService.UpdateTeacherAsync(updTeacher, cancellationToken);

                  }
              }));
        }
    }

    public RellayCommand DeleteTeacherCommand
    {
        get
        {
            return deleteTeacherCommand ??
              (deleteTeacherCommand = new RellayCommand(async (selectedItem) =>
              {

                  TeacherDto? teacherDto = selectedItem as TeacherDto;

                  if (teacherDto == null)
                  {
                      MessageBox.Show("Teacher not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }


                  await _dataService.DeleteTeacherAsync(teacherDto, cancellationToken);

                  Teachers.Remove(teacherDto);

              }));
        }
    }
}