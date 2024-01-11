using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using System.Windows;

namespace Legba.WPF;

public partial class MainWindow : Window
{
    public MainWindow(Settings settings)
    {
        InitializeComponent();
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
}