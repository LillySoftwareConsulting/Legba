using Legba.Engine.Models;
using Legba.Engine.Services;
using System.Windows.Input;

namespace Legba.Engine.ViewModels;

public class PromptPrefixEditorViewModel<T> : ObservableObject where T : PromptPrefix, new()
{
    #region Properties, Fields, Commands, and Events

    private readonly PromptRepository _promptRepository;

    private T? _promptPrefixToEdit;

    public string Title
    {
        get
        {
            if(PromptPrefixToEdit == null)
            {
                return $"Add/Update {typeof(T).Name} Prefix";
            }
            else if(PromptPrefixToEdit.Id == Guid.Empty)
            {
                return $"Add {typeof(T).Name} Prefix";
            }
            else
            {
                return $"Update {typeof(T).Name} Prefix";
            }
        }
    }

    public T? PromptPrefixToEdit
    {
        get => _promptPrefixToEdit;
        set
        {
            _promptPrefixToEdit = value;
            OnPropertyChanged(nameof(PromptPrefixToEdit));
            OnPropertyChanged(nameof(Title));
        }
    }

    public ICommand SaveCommand { get; private set; }

    #endregion

    public PromptPrefixEditorViewModel(PromptRepository promptRepository)
    {
        _promptRepository = promptRepository;

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
}