using Legba.Engine.Models;
using Legba.Engine.Services;
using Legba.Engine.ViewModels;
using Legba.Windows;
using Legba.WPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Legba;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private HelpView? _helpView;

    private readonly IServiceProvider _serviceProvider;

    private readonly ChatSessionViewModel? _chatSessionViewModel;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _chatSessionViewModel = new ChatSessionViewModel(serviceProvider, ScrollToLastParagraphTop);

        DataContext = _chatSessionViewModel;
    }

    #region Menu eventhandlers

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

    #region Work area eventhandlers

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
                promptPrefixSelectionViewModel.SelectedPromptPrefix;
        }
    }

    private void AddUpdatePersonality_Click(object sender, RoutedEventArgs e)
    {
        var view =
            _serviceProvider.GetRequiredService<PromptPrefixEditorView<Personality>>();
        var dataContext =
            view.DataContext as PromptPrefixEditorViewModel<Personality>;
        dataContext.PromptPrefixToEdit = _chatSessionViewModel.ChatSession.Personality;
        view.Owner = this;

        view.ShowDialog();
    }

    private void ClearPersonality_Click(object sender, RoutedEventArgs e)
    {
        if (_chatSessionViewModel?.ChatSession != null)
        {
            _chatSessionViewModel.ChatSession.Personality = new Personality();
        }
    }

    private void MenuItemCopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement copy to clipboard functionality for messages
        //if (RequestResponseMessages.SelectedIndex == -1)
        //{
        //    return;
        //}

        //var message = (Engine.Models.OpenAi.Message)RequestResponseMessages.SelectedItem;

        //System.Windows.Clipboard.SetText(message.Content);
    }

    private async void AddSolution_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source Solution",
            Filter = "Solution Files (*.sln)|*.sln",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        ProgressWindow progressWindow = new("Finding files in solution...")
        {
            Owner = this
        };

        try
        {
            progressWindow.Show();

            var filesToConsolidate =
                await FileCollector.GetFilesFromSolutionAsync(fileDialog.FileName);

            _chatSessionViewModel?.ChatSession?.AddSourceCodeFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error processing solution: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        finally
        {
            progressWindow.Close();
        }
    }

    private async void AddProject_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source Project",
            Filter = "Project Files (*.csproj, *.vbproj)|*.csproj;*.vbproj",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        ProgressWindow progressWindow = new("Finding files in project...")
        {
            Owner = this
        };

        try
        {
            progressWindow.Show();

            var filesToConsolidate =
                await FileCollector.GetFilesFromProjectAsync(fileDialog.FileName);

            _chatSessionViewModel?.ChatSession?.AddSourceCodeFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error processing project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        finally
        {
            progressWindow.Close();
        }
    }

    private async void AddFolders_Click(object sender, RoutedEventArgs e)
    {
        var folderDialog = new OpenFolderDialog
        {
            Title = "Select Source Folder(s)",
            Multiselect = true
        };

        if (folderDialog.ShowDialog() != true)
        {
            return;
        }

        ProgressWindow progressWindow = new("Finding files in folder(s)...")
        {
            Owner = this
        };

        try
        {
            progressWindow.Show();

            var filesToConsolidate =
                await FileCollector.GetFilesFromFoldersAsync(folderDialog.FolderNames);

            _chatSessionViewModel?.ChatSession?.AddSourceCodeFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error processing folder(s): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        finally
        {
            progressWindow.Close();
        }
    }

    private async void AddFiles_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source Code File(s)",
            Filter = "Source Code Files (*.cs, *.vb, *.xaml)|*.cs;*.vb;*.xaml|All Files (*.*)|*.*",
            Multiselect = true
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        ProgressWindow progressWindow = new("Finding source code files...")
        {
            Owner = this
        };

        try
        {
            progressWindow.Show();

            var filesToConsolidate =
                await FileCollector.GetFilesFromFilesAsync(fileDialog.FileNames);

            _chatSessionViewModel?.ChatSession?.AddSourceCodeFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error processing file(s): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        finally
        {
            progressWindow.Close();
        }
    }

    #endregion

    #region Private methods

    private void ScrollToLastParagraphTop()
    {
        if (_chatSessionViewModel?.ChatSession == null)
        {
            return;
        }

        var messages = _chatSessionViewModel.ChatSession.Messages;

        // Find the last user message (excluding system messages)
        var lastUserMessage = 
            messages.LastOrDefault(m => m.Role == Engine.Enums.Role.User && !m.IsInitialSourceCode);

        if (lastUserMessage == null)
        {
            return;
        }

        // Find the corresponding paragraph in the FlowDocument
        var document = MessageViewer?.Document;
        if (document == null)
        {
            return;
        }

        // Match the paragraph by DisplayText
        var paragraphToScroll = document.Blocks
            .OfType<Paragraph>()
            .FirstOrDefault(p => (p.Inlines.FirstInline as Run)?.Text == lastUserMessage.DisplayText);

        if (paragraphToScroll != null)
        {
            Dispatcher.InvokeAsync(() =>
            {
                paragraphToScroll.BringIntoView();
            }, DispatcherPriority.Loaded);
        }
    }

    #endregion
}