using Legba.Engine.ViewModels;
using System.Windows;

namespace Legba.WPF.Windows;

public partial class AboutView : Window
{
    public AboutView(AboutViewModel aboutViewModel)
    {
        InitializeComponent();

        DataContext = aboutViewModel;
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}