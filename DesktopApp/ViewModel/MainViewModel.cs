using DesktopApp.Command;
using DesktopApp.Service;
using System;

namespace DesktopApp.ViewModel;

class MainViewModel : ViewModelBase
{

    private bool isHomeChecked;
    public bool IsHomeChecked
    {
        get { return isHomeChecked; }
        set
        {
            isHomeChecked = value;
            OnPropertyChanged();
        }
    }

    private bool isCoursesChecked;
    public bool IsCoursesChecked
    {
        get { return isCoursesChecked; }
        set
        {
            isCoursesChecked = value;
            OnPropertyChanged();
        }
    }


    private bool isGroupsChecked;
    public bool IsGroupsChecked
    {
        get { return isGroupsChecked; }
        set
        {
            isGroupsChecked = value;
            OnPropertyChanged();
        }
    }


    private bool isStudentsChecked;
    public bool IsStudentsChecked
    {
        get { return isStudentsChecked; }
        set
        {
            isStudentsChecked = value;
            OnPropertyChanged();
        }
    }


    private bool isTeachersChecked;
    public bool IsTeachersChecked
    {
        get { return isTeachersChecked; }
        set
        {
            isTeachersChecked = value;
            OnPropertyChanged();
        }
    }


    private NavigationService _navigation;

    public NavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public RellayCommand NavigateToHomeCommand { get; set; }
    public RellayCommand NavigateToCoursesCommand { get; set; }

    public RellayCommand NavigateToGroupCommand { get; set; }

    public RellayCommand NavigateToStudentCommand { get; set; }

    public RellayCommand NavigateToTeachersCommand { get; set; }

    public MainViewModel(NavigationService navigation)
    {

        Navigation = navigation;

        Navigation.NavigateTo<HomeViewModel>();
        NavigateToHomeCommand = new RellayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); });
        NavigateToCoursesCommand = new RellayCommand(o => { Navigation.NavigateTo<CoursesViewModel>(); });
        NavigateToGroupCommand = new RellayCommand(o => { Navigation.NavigateTo<GroupsViewModel>(); });
        NavigateToStudentCommand = new RellayCommand(o => { Navigation.NavigateTo<StudentsViewModel>(); });
        NavigateToTeachersCommand = new RellayCommand(o => { Navigation.NavigateTo<TeachersViewModel>(); });

        UpdateIsCheckedProperties(Navigation.CurrentView.GetType());

        Navigation.ViewChanged += (sender, newViewType) =>
        {
            UpdateIsCheckedProperties(newViewType);
        };
    }

    public void UpdateIsCheckedProperties(Type viewType)
    {
        IsHomeChecked = (viewType == typeof(HomeViewModel));
        IsCoursesChecked = (viewType == typeof(CoursesViewModel));
        IsGroupsChecked = (viewType == typeof(GroupsViewModel));
        IsStudentsChecked = (viewType == typeof(StudentsViewModel));
        IsTeachersChecked = (viewType == typeof(TeachersViewModel));
    }
}