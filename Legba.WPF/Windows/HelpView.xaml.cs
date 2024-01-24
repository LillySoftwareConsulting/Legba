using Legba.Engine.ViewModels;
using System.Windows;

namespace Legba.WPF.Windows;

public partial class HelpView : Window
{
    public HelpView(HelpViewModel helpViewModel)
    {
        InitializeComponent();

        DataContext = helpViewModel;
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}