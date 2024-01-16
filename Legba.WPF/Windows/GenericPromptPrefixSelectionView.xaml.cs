using Legba.Engine.Services;
using System.Windows;

namespace Legba.WPF.Windows;

public partial class GenericPromptPrefixSelectionView : Window
{
    private readonly PromptRepository _promptRepository;

    public GenericPromptPrefixSelectionView(PromptRepository promptRepository)
    {
        InitializeComponent();
        _promptRepository = promptRepository;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}