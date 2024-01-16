using Legba.Engine.Models;
using Legba.Engine.Services;
using Legba.Engine.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Legba.WPF.Windows;

public class PromptPrefixSelectionView<T> : GenericPromptPrefixSelectionView where T : PromptPrefix
{
    public PromptPrefixSelectionView(IServiceProvider serviceProvider)
        : base(serviceProvider.GetRequiredService<PromptRepository>())
    {
        DataContext = 
            serviceProvider.GetRequiredService<PromptPrefixSelectionViewModel<T>>();
    }
}