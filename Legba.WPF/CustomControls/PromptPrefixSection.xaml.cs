using System.Windows;
using System.Windows.Controls;

namespace Legba.WPF.CustomControls;

public partial class PromptPrefixSection : UserControl
{
    public PromptPrefixSection()
    {
        InitializeComponent();

        DataContext = this;
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(object), typeof(PromptPrefixSection), new PropertyMetadata(null));

    public object ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty SectionTitleProperty =
        DependencyProperty.Register(nameof(SectionTitle), typeof(string), typeof(PromptPrefixSection), new PropertyMetadata(string.Empty));

    public string SectionTitle
    {
        get => (string)GetValue(SectionTitleProperty);
        set => SetValue(SectionTitleProperty, value);
    }

    public event RoutedEventHandler ManageClicked;
    public event RoutedEventHandler AddUpdateClicked;
    public event RoutedEventHandler ClearClicked;

    private void Manage_Click(object sender, RoutedEventArgs e) => ManageClicked?.Invoke(this, e);
    private void AddUpdate_Click(object sender, RoutedEventArgs e) => AddUpdateClicked?.Invoke(this, e);
    private void Clear_Click(object sender, RoutedEventArgs e) => ClearClicked?.Invoke(this, e);
}