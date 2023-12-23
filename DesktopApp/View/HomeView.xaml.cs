using DesktopApp.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesktopApp.View;

/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (treeView.SelectedItem != null)
        {
            var item = treeView.SelectedItem;
            HomeViewModel viewModel = (HomeViewModel)DataContext;
            viewModel.MakeNavigationCommand.Execute(item);
        }
    }
}