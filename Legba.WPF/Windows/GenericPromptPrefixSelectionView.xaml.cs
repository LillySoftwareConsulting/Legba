using Legba.Engine.Models;
using Legba.Engine.Services;
using Legba.Engine.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Legba.WPF.Windows;

public partial class GenericPromptPrefixSelectionView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PromptRepository _promptRepository;

    public GenericPromptPrefixSelectionView(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _promptRepository = _serviceProvider.GetRequiredService<PromptRepository>();
    }

    // TODO: Find a better way to do this, without reflection
    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var dataType = DataContext.GetType().GetGenericArguments()[0];

        var newPromptPrefix = Activator.CreateInstance(dataType);

        // Use reflection to call DisplayPromptPrefixEditorPopup<T>() with the correct type
        MethodInfo method = GetType().GetMethod(nameof(DisplayPromptPrefixEditorPopup));
        MethodInfo genericMethod = method.MakeGenericMethod(dataType);

        genericMethod.Invoke(this, new[] { newPromptPrefix });
    }

    // TODO: Find a better way to do this, without reflection
    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null)
        {
            return;
        }

        var promptPrefix = button.DataContext;
        if (promptPrefix == null)
        { 
            return; 
        }

        var dataType = promptPrefix.GetType();

        // Use reflection to call DisplayPromptPrefixEditorPopup<T>() with the correct type
        MethodInfo method = GetType().GetMethod(nameof(DisplayPromptPrefixEditorPopup));
        MethodInfo genericMethod = method.MakeGenericMethod(dataType);

        genericMethod.Invoke(this, new[] { promptPrefix });
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    // This is public so it can be called via reflection in the Edit_Click method
    public void DisplayPromptPrefixEditorPopup<T>(T promptPrefix) where T : PromptPrefix, new()
    {
        var view =
            _serviceProvider.GetRequiredService<PromptPrefixEditorView<T>>();
        var dataContext =
            view.DataContext as PromptPrefixEditorViewModel<T>;
        dataContext.PromptPrefixToEdit = promptPrefix;
        view.Owner = this;

        view.ShowDialog();

        // TODO: Find a better way to do this (clone object before editing? memento?)
        // This is here in case the user cancels the edit of the prompt prefix
        var myDataContext = DataContext as PromptPrefixSelectionViewModel<T>;
        myDataContext?.PopulatePromptPrefixes();
    }
}