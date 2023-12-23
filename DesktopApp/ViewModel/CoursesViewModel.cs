using DesktopApp.Command;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using DesktopApp.View.Course;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace DesktopApp.ViewModel;

class CoursesViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    CancellationToken cancellationToken = new CancellationToken();

    public ObservableCollection<CourseDto> Courses => _dataService.Courses;
    public RellayCommand? addCourseCommand;
    public RellayCommand? editCourseCommand;
    public RellayCommand? deleteCourseCommand;

    public CoursesViewModel(DataService dataService)
    {
        _dataService = dataService;
        LoadAsync();

        MessagingService.Instance.CourseSelected += OnCategorySelected;
    }

    private void OnCategorySelected(int Id)
    {
        if (Id == -1)
        {

            foreach (var course in Courses)
            {
                course.Highlighted = false;
            }
            return;
        }


        foreach (var course in Courses)
        {
            if (course.CourseId == Id)
            {
                course.Highlighted = true;
            }


        }

    }

    public async void LoadAsync()
    {
        if (Courses.Count == 0)
        {
            await _dataService.LoadCoursesAsync(cancellationToken);
        }
    }

    public RellayCommand AddCourseCommand
    {
        get
        {
            return addCourseCommand ??
              (addCourseCommand = new RellayCommand(async (o) =>
              {
                  AddCourse viewCourse = new AddCourse(new CourseDto());
                  if (viewCourse.ShowDialog() == true)
                  {
                      CourseDto course = viewCourse._courseDto;

                      await _dataService.CreateCourseAsync(course, cancellationToken);
                  }
              }));
        }
    }

    public RellayCommand EditCourseCommand
    {
        get
        {
            return editCourseCommand ??
              (editCourseCommand = new RellayCommand(async (selectedItem) =>
              {

                  CourseDto? course = selectedItem as CourseDto;
                  if (course == null)
                  {
                      MessageBox.Show("Course not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }


                  CourseDto updCourse = new CourseDto
                  {
                      Name = course.Name,
                      Description = course.Description
                  };
                  AddCourse viewCourse = new AddCourse(course);


                  if (viewCourse.ShowDialog() == true)
                  {
                      updCourse.CourseId = course.CourseId;
                      updCourse.Name = viewCourse._courseDto.Name;
                      updCourse.Description = viewCourse._courseDto.Description;

                      await _dataService.UpdateCourseAsync(updCourse, cancellationToken);
                  }

              }));
        }
    }

    public RellayCommand DeleteCourseCommand
    {
        get
        {
            return deleteCourseCommand ??
              (deleteCourseCommand = new RellayCommand(async (selectedItem) =>
              {

                  CourseDto? course = selectedItem as CourseDto;
                  if (course == null)
                  {
                      MessageBox.Show("Course not selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;
                  }
                  else if (!await _dataService.DeleteCourseAsync(course, cancellationToken))
                  {
                      MessageBox.Show("The course cannot be deleted because it contains groups.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                      return;

                  };
              }));
        }
    }
}