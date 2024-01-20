using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Legba.Engine.ViewModels;
using Legba.WPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Legba.WPF;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;

    private ChatViewModel? VM => DataContext as ChatViewModel;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
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

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion

    #region Button click handlers

    private void ManagePersonas_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Persona>();
    }

    private void SavePersonaText_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Popup window to enter the name of the persona
        // and save it to the database.
    }

    private void ClearPersonaText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Persona = new Persona();
    }

    private void ManagePurposes_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Purpose>();
    }

    private void SavePurposeText_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Popup window to enter the name of the purpose
        // and save it to the database.
    }

    private void ClearPurposeText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Purpose = new Purpose();
    }

    private void ManagePersuasions_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Persuasion>();
    }

    private void SavePersuasionText_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Popup window to enter the name of the persuasion
        // and save it to the database.
    }

    private void ClearPersuasionText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Persuasion = new Persuasion();
    }

    private void ManageProcesses_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Process>();
    }

    private void SaveProcessText_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Popup window to enter the name of the process
        // and save it to the database.
    }

    private void ClearProcessText_Click(object sender, RoutedEventArgs e)
    {
        VM.ChatSession.Process = new Process();
    }

    #endregion

    #region Private support methods

    private void DisplayPromptPrefixPopup<T>() where T : PromptPrefix, new()
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