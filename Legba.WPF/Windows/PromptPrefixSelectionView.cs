using Legba.Engine.Models;
using Legba.Engine.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Legba.WPF.Windows;

// You can't use generics when instantiating the GenericPromptPrefixSelectionView view.
// So, this class is used to let us have generics in the appropriate view model.
public class PromptPrefixSelectionView<T> : 
    GenericPromptPrefixSelectionView where T : PromptPrefix, new()
{
    public PromptPrefixSelectionView(IServiceProvider serviceProvider)
        : base()
    {
        DataContext = 
            serviceProvider.GetRequiredService<PromptPrefixSelectionViewModel<T>>();
    }
}