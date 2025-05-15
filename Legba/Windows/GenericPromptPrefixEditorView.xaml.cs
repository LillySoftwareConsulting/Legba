using Legba.Engine.Models;
using System.Windows;

namespace Legba.Windows;

public partial class GenericPromptPrefixEditorView : Window
{
    public GenericPromptPrefixEditorView()
    {
        InitializeComponent();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as ICanSaveToDatabase;

        vm?.Save();

        Close();
    }
}