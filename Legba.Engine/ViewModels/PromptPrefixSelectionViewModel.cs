using Legba.Engine.Models;
using Legba.Engine.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Legba.Engine.ViewModels;

public class PromptPrefixSelectionViewModel<T> : ObservableObject where T : PromptPrefix, new()
{
    #region Properties, Fields, Commands, and Events

    private string _title;
    private T? _selectedPromptPrefix;
    private T? _promptPrefixToEdit;
    private readonly PromptRepository _promptRepository;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(AddNewButtonText));
        }
    }

    public T? PromptPrefixToEdit
    {
        get => _promptPrefixToEdit;
        set
        {
            _promptPrefixToEdit = value;
            OnPropertyChanged(nameof(PromptPrefixToEdit));
        }
    }

    public string AddNewButtonText => $"Add New {typeof(T).Name}";

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

    public ICommand AddNewCommand { get; private set; }
    public ICommand SaveCommand { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public PromptPrefixSelectionViewModel(PromptRepository promptRepository)
    {
        _promptRepository = promptRepository;

        Title = $"Manage {typeof(T).Name} Prefixes";

        UpdatePromptPrefixes();

        AddNewCommand = new RelayCommand(() => PromptPrefixToEdit = new T());
        SaveCommand = new RelayCommand(Save);
    }

    private void Save()
    {
        if (PromptPrefixToEdit == null)
        {
            return;
        }

        _promptRepository.AddOrUpdate(PromptPrefixToEdit);

        PromptPrefixToEdit = null;
    }

    private void UpdatePromptPrefixes()
    {
        PromptPrefixes.Clear();

        foreach (var promptPrefix in _promptRepository.GetAll<T>())
        {
            PromptPrefixes.Add(promptPrefix);
        }
    }
}