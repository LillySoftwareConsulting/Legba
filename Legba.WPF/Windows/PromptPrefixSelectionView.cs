using Legba.Engine.Models;
using Legba.Engine.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Legba.WPF.Windows;

// This class is needed to use generics to get the appropriate view model.
// You can't use generics when instantiating the GenericPromptPrefixSelectionView view.
public class PromptPrefixSelectionView<T> : GenericPromptPrefixSelectionView where T : PromptPrefix, new()
{
    public PromptPrefixSelectionView(IServiceProvider serviceProvider)
        : base()
    {
        DataContext = 
            serviceProvider.GetRequiredService<PromptPrefixSelectionViewModel<T>>();
    }
}