using System.ComponentModel;
using System.Windows.Input;

namespace Legba.Engine.ViewModels;

public class PromptPrefixSelectionViewModel : ViewModelBase
{
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand OkCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }

    public PromptPrefixSelectionViewModel()
    {
        OkCommand = new RelayCommand(() => CloseWindow(true));
        CancelCommand = new RelayCommand(() => CloseWindow(false));
    }

    private void CloseWindow(bool? dialogResult)
    {
        ;
        // Assuming the ViewModel has a reference to its View
        // You can achieve this by passing the View to the ViewModel via constructor
        //(this.View as PopupView).DialogResult = dialogResult;
    }
}