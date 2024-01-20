﻿using Legba.Engine.Models;
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
            OnPropertyChanged(nameof(AddButtonText));
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

    public string AddButtonText => $"Add New {typeof(T).Name}";

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

    public ICommand UseCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand AddCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
    public ICommand SaveCommand { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public PromptPrefixSelectionViewModel(PromptRepository promptRepository)
    {
        _promptRepository = promptRepository;

        Title = $"Manage {typeof(T).Name} Prefixes";

        PopulatePromptPrefixes();

        UseCommand = new TypedRelayCommand<T>(Use);
        EditCommand = new TypedRelayCommand<T>(Edit);
        DeleteCommand = new TypedRelayCommand<T>(Delete);
        AddCommand = new RelayCommand(() => PromptPrefixToEdit = new T());
        CancelCommand = new RelayCommand(Cancel);
        SaveCommand = new RelayCommand(Save);
    }

    private void Use(T promptPrefix)
    {
        SelectedPromptPrefix = promptPrefix;
    }

    private void Edit(T promptPrefix)
    {
        PromptPrefixToEdit = promptPrefix;
    }

    private void Delete(T promptPrefix)
    {
        _promptRepository.Delete<T>(promptPrefix.Id);

        PopulatePromptPrefixes();
    }

    private void Cancel()
    {
        PromptPrefixToEdit = null;
    }

    private void Save()
    {
        if (PromptPrefixToEdit == null)
        {
            return;
        }

        _promptRepository.AddOrUpdate(PromptPrefixToEdit);

        PromptPrefixToEdit = null;

        PopulatePromptPrefixes();
    }

    private void PopulatePromptPrefixes()
    {
        PromptPrefixes.Clear();

        foreach (var promptPrefix in _promptRepository.GetAll<T>())
        {
            PromptPrefixes.Add(promptPrefix);
        }
    }
}