using DesktopApp.Model.EntityDto;
using System.Windows;

namespace DesktopApp.View.Course
{
    /// <summary>
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourse : Window
    {
        public CourseDto _courseDto { get; private set; }
        public AddCourse(CourseDto courseDto)
        {
            InitializeComponent();

            _courseDto = courseDto;
            DataContext = _courseDto;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}