# About Legba
This is a desktop app to help you make, and manage, calls to OpenAI's API. It currently only runs on Windows.

"Legba" is from [Papa Legba](https://en.wikipedia.org/wiki/Papa_Legba), a voodoo *loa* who stood at the crossroads between humans and God.

# Setup steps
- Create an [OpenAI API account](https://openai.com/blog/openai-api)
- Get an [API key](https://platform.openai.com/api-keys)
- Make your API key available to Legba
  - In the Legba.WPF project, modify App.xaml.cs to use your preferred key reader
    - See example functions, for how to set up the key reader
    - Availble types of readers:
      - EnvironmentVariableKeyReader
      - JsonFileKeyReader
      - UserSecretsKeyReader
      - XmlFileKeyReader
- Test the app

# Support
Please report any problems at one of these places:
- [Issues](https://github.com/LillySoftwareConsulting/Legba/issues)
- [Discussions](https://github.com/LillySoftwareConsulting/Legba/discussions)

# Future Plans
- Add new key readers for cloud key stores.
- Convert the UI to something that runs cross-platform - probably [Avalonia](https://avaloniaui.net/).
- Convert the KeyReader class library project into a NuGet package.
