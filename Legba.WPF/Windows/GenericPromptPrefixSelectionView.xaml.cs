using System.Windows;

namespace Legba.WPF.Windows;

public partial class GenericPromptPrefixSelectionView : Window
{
    public GenericPromptPrefixSelectionView()
    {
        InitializeComponent();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}