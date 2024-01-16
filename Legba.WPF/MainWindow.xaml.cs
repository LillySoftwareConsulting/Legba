using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Legba.WPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Legba.WPF;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
    }

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

    private void Persona_OnClick(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Persona>();
    }

    private void Purpose_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Purpose>();
    }

    private void Persuasion_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Persuasion>();
    }

    private void Process_Click(object sender, RoutedEventArgs e)
    {
        DisplayPromptPrefixPopup<Process>();
    }

    private void DisplayPromptPrefixPopup<T>() where T : PromptPrefix
    {
        var view =
            _serviceProvider.GetRequiredService<PromptPrefixSelectionView<T>>();
        view.Owner = this;

        view.ShowDialog();
    }
}