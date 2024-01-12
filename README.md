# About Legba
Legba is a desktop app to help you make and manage calls to OpenAI's API. It currently only runs on Windows, but I plan to make it cross-platform.

It started out as a simple desktop request/response app, but I'm expanding it to help test building prompts with different 'setup' text describing the persona, purpose, persuasion, and process to include with the prompt.

The name "Legba" is from [Papa Legba](https://en.wikipedia.org/wiki/Papa_Legba), a voodoo *loa* who stood at the crossroads between humans and God and helped them communicate.

# Setup steps
- Create an [OpenAI API account](https://openai.com/blog/openai-api)
- Get an [API key](https://platform.openai.com/api-keys)
- Fund the API account. It may take a while for your account to be funded and activated.
- Make your API key available to Legba
  - In the Legba.WPF project, modify appsettings.json to use your api key and (optional) organization Id.
- Test the app

# Support
Please report any problems at one of these places:
- [Issues](https://github.com/LillySoftwareConsulting/Legba/issues)
- [Discussions](https://github.com/LillySoftwareConsulting/Legba/discussions)

# Future Plans
- Convert the UI to something that runs cross-platform - probably [Avalonia](https://avaloniaui.net/).
- Able to store and retrieve "prompt prefixes" to be passed in when making a request to OpenAI's API.
- Able to share prompts with friends, co-workers, or the public.
- Connect to other LLMs (on the web or locally).
