using Legba.Engine.Models;
using Legba.Engine.Services;
using System.Windows.Input;

namespace Legba.Engine.ViewModels;

public class PromptPrefixSelectionContainerViewModel : ObservableObject
{
    #region Fields and Properties

    private readonly PromptRepository _promptRepository;

    public PromptPrefixSelectionViewModel<Persona> PersonaViewModel { get; }
    public PromptPrefixSelectionViewModel<Purpose> PurposeViewModel { get; }
    public PromptPrefixSelectionViewModel<Persuasion> PersuasionViewModel { get; }
    public PromptPrefixSelectionViewModel<Process> ProcessViewModel { get; }

    public ICommand LoadFromSolutionCommand { get; }
    public ICommand LoadFromProjectCommand { get; }
    public ICommand LoadFromFoldersCommand { get; }
    public ICommand LoadFromFilesCommand { get; }

    #endregion

    #region Constructor

    public PromptPrefixSelectionContainerViewModel(PromptRepository promptRepository)
    {
        _promptRepository = promptRepository;

        PersonaViewModel = new PromptPrefixSelectionViewModel<Persona>(_promptRepository);
        PurposeViewModel = new PromptPrefixSelectionViewModel<Purpose>(_promptRepository);
        PersuasionViewModel = new PromptPrefixSelectionViewModel<Persuasion>(_promptRepository);
        ProcessViewModel = new PromptPrefixSelectionViewModel<Process>(_promptRepository);

        LoadFromSolutionCommand = new RelayCommand(LoadFromSolution);
        LoadFromProjectCommand = new RelayCommand(LoadFromProject);
        LoadFromFoldersCommand = new RelayCommand(LoadFromFolders);
        LoadFromFilesCommand = new RelayCommand(LoadFromFiles);
    }

    #endregion

    #region Command Handlers

    private void LoadFromSolution()
    {
        // TODO: Implement solution-level file loading logic
    }

    private void LoadFromProject()
    {
        // TODO: Implement project-level file loading logic
    }

    private void LoadFromFolders()
    {
        // TODO: Implement folder-level file loading logic
    }

    private void LoadFromFiles()
    {
        // TODO: Implement file-level loading logic
    }

    #endregion
}
