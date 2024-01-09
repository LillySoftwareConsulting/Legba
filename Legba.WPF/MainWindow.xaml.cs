using OpenAiConnector.Models;
using System.Windows;

namespace Legba.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
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