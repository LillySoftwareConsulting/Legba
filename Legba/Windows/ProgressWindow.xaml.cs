using System.Windows;

namespace Legba.Windows
{
    public partial class ProgressWindow : Window
    {
        public ProgressWindow(string message = "Working...")
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }
    }
}
