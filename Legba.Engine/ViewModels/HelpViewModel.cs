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

        AddHelpTopic("API Setup",
            "Create an API account for any of the following LLMs: OpenAI, Grok, Perplexity, or Groq." + 
            _doubleNewLine +
            "Get your API key and (optionally) Organization ID." + 
            _doubleNewLine +
            "Fund the API account. It may take a few hours for the API to recognize the account was funded.");

        AddHelpTopic("Legba Setup",
            "In the directory where Legba is installed, find the appsettings.json file and open it in a text editor." + 
            _doubleNewLine +
            "Add your API key and (optionally) your Organization ID where appsettings has 'YOUR_API_KEY' and 'YOUR_ORG_ID'." +
            _doubleNewLine +
            "If you are not using an Organization ID, remove the text 'YOUR_ORG_ID'.");

        AddHelpTopic("New LLM Session",
            "When you want to chat with the LLM, select File -> New Session -> LLM Company and the name of the model you want to use." +
            _doubleNewLine +
            "This will display the session screen. Your conversation with the LLM will be displayed in the top section of the screen." +
            _doubleNewLine +
            "You can enter your request in 'User-Entered Prompt', and optionally select a Personality or source code files to include.");

        AddHelpTopic("Personality",
            "The personality is used to help the LLM understand the context of your conversation and how to respond. It is not required." +
            _doubleNewLine +
            "The magnifying glass shows you a list of available personalities. From there, you can add a new one, edit or delete an existing one, or select one to use." +
            _doubleNewLine +
            "The green '+' button lets you save what is currently in the prompt's textbox to the database. It will pop up a window for you to enter the personality's name before saving." +
            _doubleNewLine +
            "The red 'X' button lets you clear out the prompt prefix text box. It does not affect anything in the database." +
            _doubleNewLine +
            "The personality is only sent with the first request. So, the Personality tab is disabled after the first request.");

        AddHelpTopic("Source Code",
            "You can include C# and VB.NET (including XAML) source code files with your first request." +
            _doubleNewLine +
            "You can use the buttons on the left to add source code files from a solution file, project file, folder(s), or just selecting the file(s) to include." +
            _doubleNewLine +
            "You can selectively delete files by clicking the red 'X' button next to the file in the list." +
            _doubleNewLine +
            "The source code files are only sent with the first request. So, the 'Source Code from Files' tab is disabled after the first request."
            );

        AddHelpTopic("Sending Your Request",
            "Enter, or select, any (optional) personality and (optional) source code files you want to include in your request." +
            _doubleNewLine +
            "Enter your prompt/question in the 'User-Entered Prompt' tab, then click the 'Submit Request to LLM' button." +
            _doubleNewLine +
            "Your message will be displayed in the upper response area, along with a 'Thinking...' message while waiting for the LLM's response.");

        AddHelpTopic("Copying Responses",
            "You can right-click on a response and copy its text to your clipboard, to paste into Notepad or an editor.");

        AddHelpTopic("Statistics",
            "Clicking on the 'Statistics' tab at the top of the app will show you the number of tokens you've used during this session." +
            _doubleNewLine +
            "It shows a list of tokens used with each request and the session's grand total at the bottom.");
    }

    private void AddHelpTopic(string title, string content)
    {
        Topics.Add(new HelpTopic(title, content));
    }
}