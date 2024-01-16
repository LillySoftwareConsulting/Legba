using Legba.Engine.Models;
using Legba.Engine.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Legba.Engine.ViewModels;

public class PromptPrefixSelectionViewModel<T> : ViewModelBase where T : PromptPrefix
{
    #region Properties, Fields, Commands, and Events

    private string _title;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public ObservableCollection<T> PromptPrefixes { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public PromptPrefixSelectionViewModel(PromptRepository promptRepository)
    {
        Title = $"Manage {typeof(T).Name}s";
        PromptPrefixes = new ObservableCollection<T>(promptRepository.Get<T>());
    }
}