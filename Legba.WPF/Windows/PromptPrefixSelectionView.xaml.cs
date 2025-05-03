using Legba.Engine.ViewModels;
using System.Windows;

namespace Legba.WPF.Windows;

/// <summary>
/// Interaction logic for PromptPrefixSelectionView.xaml
/// </summary>
public partial class PromptPrefixSelectionView : Window
{
    public PromptPrefixSelectionContainerViewModel ViewModel { get; set; }

    public PromptPrefixSelectionView(PromptPrefixSelectionContainerViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;

        DataContext = ViewModel;
    }

    private void Continue_Click(object sender, RoutedEventArgs e)
    {
        // Signify that the "Contiue" button was pressed, and the process should continue.
        DialogResult = true;
        Close();
    }

    private void ManagePersonas_Click(object sender, RoutedEventArgs e)
    {

    }

    private void AddUpdatePersonaText_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ClearPersonaText_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ManagePurposes_Click(object sender, RoutedEventArgs e) { }
    private void AddUpdatePurposeText_Click(object sender, RoutedEventArgs e) { }
    private void ClearPurposeText_Click(object sender, RoutedEventArgs e) { }

    private void ManagePersuasions_Click(object sender, RoutedEventArgs e) { }
    private void AddUpdatePersuasionText_Click(object sender, RoutedEventArgs e) { }
    private void ClearPersuasionText_Click(object sender, RoutedEventArgs e) { }

    private void ManageProcesses_Click(object sender, RoutedEventArgs e) { }
    private void AddUpdateProcessText_Click(object sender, RoutedEventArgs e) { }
    private void ClearProcessText_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ProcessViewModel.PopulatePromptPrefixes();
    }

}
