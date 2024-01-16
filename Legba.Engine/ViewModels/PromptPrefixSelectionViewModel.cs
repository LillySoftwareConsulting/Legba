using Legba.Engine.Models;
using Legba.Engine.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Legba.Engine.ViewModels;

public class PromptPrefixSelectionViewModel<T> : ViewModelBase where T : PromptPrefix
{
    #region Properties, Fields, Commands, and Events

    private string _title;
    private T? _selectedPromptPrefix;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public T? SelectedPromptPrefix
    {
        get => _selectedPromptPrefix;
        set
        {
            _selectedPromptPrefix = value;
            OnPropertyChanged(nameof(SelectedPromptPrefix));
        }
    }

    public ObservableCollection<T> PromptPrefixes { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public PromptPrefixSelectionViewModel(PromptRepository promptRepository)
    {
        Title = $"Manage {typeof(T).Name} Prefixes";
        PromptPrefixes = new ObservableCollection<T>(promptRepository.Get<T>());
    }
}