using Legba.Engine.ViewModels;
using System.Windows;

namespace Legba.WPF.Windows;

public partial class AboutView : Window
{
    public AboutView()
    {
        InitializeComponent();

        DataContext = new AboutViewModel();
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}