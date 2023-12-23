
using DesktopApp.Model.EntityDto;
using System.Windows;

namespace DesktopApp.View.Teacher
{
    /// <summary>
    /// Interaction logic for AddEditTeacher.xaml
    /// </summary>
    public partial class AddEditTeacher : Window
    {
        public TeacherDto _teacherDto { get; private set; }
        public AddEditTeacher(TeacherDto teacherDto)
        {
            InitializeComponent();
            _teacherDto = teacherDto;

            DataContext = _teacherDto;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}