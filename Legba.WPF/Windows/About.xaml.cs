using Legba.Engine.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Legba.WPF.Windows;

public partial class About : Window
{
    public About(AboutViewModel aboutViewModel)
    {
        InitializeComponent();

        DataContext = aboutViewModel;
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}