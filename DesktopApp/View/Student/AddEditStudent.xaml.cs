using DesktopApp.Model.EntityDto;
using System.Windows;


namespace DesktopApp.View.Student
{
    /// <summary>
    /// Interaction logic for AddEditStudent.xaml
    /// </summary>
    public partial class AddEditStudent : Window
    {
        public StudentDto _studentDto { get; private set; }

        public AddEditStudent(StudentDto studentDto)
        {
            InitializeComponent();

            _studentDto = studentDto;
            
            comboBoxItems.SelectedItem = _studentDto.SelectedGroup;
            comboBoxItems.ItemsSource = _studentDto.Groups;

            DataContext = _studentDto;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}