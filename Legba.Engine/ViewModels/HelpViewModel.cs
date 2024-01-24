using Legba.Engine.Models;
using System.Collections.ObjectModel;

namespace Legba.Engine.ViewModels;

public class HelpViewModel : ObservableObject
{
    private readonly string _doubleNewLine = 
        Environment.NewLine + Environment.NewLine;

    private HelpTopic _selectedTopic;

    public ObservableCollection<HelpTopic> Topics { get; set; } = 
        new ObservableCollection<HelpTopic>();

    public HelpTopic? SelectedTopic
    {
        get => _selectedTopic;
        set
        {
            _selectedTopic = value;
            OnPropertyChanged(nameof(SelectedTopic));
        }
    }

    public HelpViewModel()
    {
        PopulateHelpTopics();
    }

    private void PopulateHelpTopics()
    {
        Topics.Clear();

        AddHelpTopic("OpenAI API Setup",
            "Create an OpenAI API account at https://openai.com/blog/openai-api." + 
            _doubleNewLine +
            "Get an OpenAI key at https://platform.openai.com/api-keys." + 
            _doubleNewLine +
            "Fund the API account. It may take a few hours for the API to recognize the account was funded.");

        AddHelpTopic("Legba Setup",
            "In the directory where Legba is installed, find the appsettings.json file and open it in a text editor." + 
            _doubleNewLine +
            "Add your OpenAI API key and (optionally) your OpenAI Organization ID.");
    }

    private void AddHelpTopic(string title, string content)
    {
        Topics.Add(new HelpTopic(title, content));
    }
}