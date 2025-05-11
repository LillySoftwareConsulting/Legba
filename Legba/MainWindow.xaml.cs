using Legba.Engine.Models;
using Legba.Engine.ViewModels;
using Legba.Windows;
using Legba.WPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Legba;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private HelpView? _helpView;

    private readonly IServiceProvider _serviceProvider;

    private ChatSessionViewModel? _chatSessionViewModel;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _chatSessionViewModel = new ChatSessionViewModel(serviceProvider);

        DataContext = _chatSessionViewModel;
    }

    #region Eventhandlers

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Help_Click(object sender, RoutedEventArgs e)
    {
        if (_helpView == null)
        {
            _helpView = new HelpView();
            _helpView.Owner = this;

            // Set _helpView to null when it's closed
            _helpView.Closed += (s, args) => _helpView = null;
        }

        // Bring the window to the top and set focus
        _helpView.Activate();

        if (_helpView.WindowState == WindowState.Minimized)
        {
            // If the window was minimized, restore it
            _helpView.WindowState = WindowState.Normal;
        }

        // Show the window if it's not already visible
        _helpView.Show();
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        var aboutView = new AboutView();
        aboutView.Owner = this;
        aboutView.ShowDialog();
    }

    #endregion

    private void SelectPersonality_Click(object sender, RoutedEventArgs e)
    {
        var view =
            _serviceProvider.GetRequiredService<PromptPrefixSelectionView<Personality>>();
        view.Owner = this;

        view.ShowDialog();

        // Handle the result of the dialog.
        var promptPrefixSelectionViewModel =
            view.DataContext as PromptPrefixSelectionViewModel<Personality>;

        if (promptPrefixSelectionViewModel is null ||
            promptPrefixSelectionViewModel.SelectedPromptPrefix is null)
        {
            return;
        }

        if (_chatSessionViewModel?.ChatSession != null)
        {
            _chatSessionViewModel.ChatSession.Personality =
                promptPrefixSelectionViewModel.SelectedPromptPrefix.Text;
        }
    }

    private void AddUpdatePersonality_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ClearPersonality_Click(object sender, RoutedEventArgs e)
    {
        if (_chatSessionViewModel?.ChatSession != null)
        {
            _chatSessionViewModel.ChatSession.Personality = string.Empty;
        }
    }
}