using CSharpExtender.ExtensionMethods;
using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Legba.Engine.Services;
using Legba.Engine.ViewModels;
using Legba.WPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Legba.WPF;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PromptRepository _promptRepository;

    private ChatViewModel? VM => DataContext as ChatViewModel;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _promptRepository = _serviceProvider.GetRequiredService<PromptRepository>();
    }

    #region Menu item click handlers

    private void MenuItemCopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        if (messages.SelectedIndex != -1)
        {
            var message = (Message)messages.SelectedItem;

            Clipboard.SetText(message.Content);
        }
    }

    private void ExportPromptPrefixesLibrary_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = 
            new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

        if(saveFileDialog.ShowDialog() == true)
        {
            var export = _promptRepository.Export();

            File.WriteAllText(saveFileDialog.FileName, 
                export.AsSerializedJson().PrettyPrintJson());
        }
    }

    private void ExportCurrentChatMessages_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog =
            new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

        if (saveFileDialog.ShowDialog() == true)
        {
            var export = VM.ChatSession.Messages;

            File.WriteAllText(saveFileDialog.FileName,
                export.AsSerializedJson().PrettyPrintJson());
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion

    #region Button click handlers

    private void ManagePersonas_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixSelectionPopup<Persona>();
    }

    private void AddUpdatePersonaText_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixEditorPopup(VM.ChatSession.Persona);
    }

    private void ClearPersonaText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Persona = new Persona();
    }

    private void ManagePurposes_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixSelectionPopup<Purpose>();
    }

    private void AddUpdatePurposeText_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixEditorPopup(VM.ChatSession.Purpose);
    }

    private void ClearPurposeText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Purpose = new Purpose();
    }

    private void ManagePersuasions_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixSelectionPopup<Persuasion>();
    }

    private void AddUpdatePersuasionText_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixEditorPopup(VM.ChatSession.Persuasion);
    }

    private void ClearPersuasionText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Persuasion = new Persuasion();
    }

    private void ManageProcesses_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixSelectionPopup<Process>();
    }

    private void AddUpdateProcessText_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixEditorPopup(VM.ChatSession.Process);
    }

    private void ClearProcessText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Process = new Process();
    }

    #endregion

    #region Private support methods

    private void DisplayPromptPrefixEditorPopup<T>(T promptPrefix) where T : PromptPrefix, new()
    {
        var view =
            _serviceProvider.GetRequiredService<PromptPrefixEditorView<T>>();
        var dataContext =
            view.DataContext as PromptPrefixEditorViewModel<T>;
        dataContext.PromptPrefixToEdit = promptPrefix;
        view.Owner = this;

        view.ShowDialog();
    }

    private void DisplayPromptPrefixSelectionPopup<T>() where T : PromptPrefix, new()
    {
        var view =
            _serviceProvider.GetRequiredService<PromptPrefixSelectionView<T>>();
        view.Owner = this;

        view.ShowDialog();

        // Handle the result of the dialog.
        var promptPrefixSelectionViewModel =
            view.DataContext as PromptPrefixSelectionViewModel<T>;
        
        if(VM is null ||
            promptPrefixSelectionViewModel is null ||
            promptPrefixSelectionViewModel.SelectedPromptPrefix is null)
        {
            return;
        }

        if(promptPrefixSelectionViewModel.SelectedPromptPrefix is Persona persona)
        {
            VM.ChatSession.Persona = persona;
        }
        else if(promptPrefixSelectionViewModel.SelectedPromptPrefix is Purpose purpose)
        {
            VM.ChatSession.Purpose = purpose;
        }
        else if(promptPrefixSelectionViewModel.SelectedPromptPrefix is Persuasion persuasion)
        {
            VM.ChatSession.Persuasion = persuasion;
        }
        else if(promptPrefixSelectionViewModel.SelectedPromptPrefix is Process process)
        {
            VM.ChatSession.Process = process;
        }
    }

    #endregion
}