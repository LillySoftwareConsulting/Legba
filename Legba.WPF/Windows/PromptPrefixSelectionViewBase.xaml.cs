using Legba.Engine.Services;
using System.Windows;

namespace Legba.WPF.Windows;

public partial class PromptPrefixSelectionViewBase : Window
{
    private readonly PromptRepository _promptRepository;

    public PromptPrefixSelectionViewBase(PromptRepository promptRepository)
    {
        InitializeComponent();
        _promptRepository = promptRepository;
    }

    private void Done_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}