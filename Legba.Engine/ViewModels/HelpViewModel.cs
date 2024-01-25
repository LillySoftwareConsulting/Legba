using Legba.Engine.Models;
using System.Collections.ObjectModel;

namespace Legba.Engine.ViewModels;

public class HelpViewModel : ObservableObject
{
    private readonly string _doubleNewLine = 
        Environment.NewLine + Environment.NewLine;

    private HelpTopic? _selectedTopic;

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

        AddHelpTopic("New Chat Session",
            "When you want to chat with ChatGPT, select File -> New Session -> OpenAi and then the version you want to use." +
            _doubleNewLine +
            "This should display an empty message history on the left, the four prompt prefix sections (Persona, Purpose, Persuasion, and Process) on the right, and the prompt input box on the bottom right.");

        AddHelpTopic("Prompt Prefixes",
            "The prompt prefixes are used to help ChatGPT understand the context of your conversation." +
            _doubleNewLine +
            "They are not required. You can enter you prompt in the lower-right textbox and click the 'Ask' button to prompt ChatGPT without any prompt prefixes." +
            _doubleNewLine +
            "However, you can save prefixes in the database for future use - to save on typing and improve consistentcy in results." +
            _doubleNewLine +
            "Currently, the prefixes are only passed in with the first request to ChatGPT.");

        AddHelpTopic("Managing Prompt Prefixes",
            "Above each prompt prefix section are three buttons." +
            _doubleNewLine +
            "The magnifying glass shows you a list of available prefixes. From there, you can add a new prefix, edit or delete an existing prefix, or select the prefix to use." +
            _doubleNewLine +
            "The green '+' button will let you save what is currently in the prompt's textbox to your library. It will pop up a window for you to enter the prompt prefix's name before saving." +
            _doubleNewLine +
            "The red 'X' button lets you clear out the prompt prefix text box. It does not affect anything in the database.");
    }

    private void AddHelpTopic(string title, string content)
    {
        Topics.Add(new HelpTopic(title, content));
    }
}