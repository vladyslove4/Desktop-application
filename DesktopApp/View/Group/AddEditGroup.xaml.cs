
using DesktopApp.Model.EntityDto;
using System.Windows;

namespace DesktopApp.View.Group
{
    /// <summary>
    /// Interaction logic for AddGroup.xaml
    /// </summary>
    public partial class AddEditGroup : Window
    {
        public GroupDto _groupDto { get; private set; }
        public AddEditGroup(GroupDto groupDto)
        {
            InitializeComponent();

            _groupDto = groupDto;

            comboBoxItems.SelectedItem = _groupDto.SelectedCourse;
            comboBoxItems.ItemsSource = _groupDto.Courses;

            comboBoxItemsTeacher.SelectedItem = _groupDto.SelectedTeacher;
            comboBoxItemsTeacher.ItemsSource = _groupDto.Teachers;

            DataContext = _groupDto;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}