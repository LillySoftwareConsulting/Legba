![Build Status](https://github.com/LillySoftwareConsulting/Legba/actions/workflows/ci.yml/badge.svg)

# About Legba

<img align="left" width="125" height="125" style="color:white" src="https://github.com/LillySoftwareConsulting/Legba/blob/master/Legba/Images/LegbaLogo.png">

Legba is a desktop app to help you test, optimize, and share prompts when calling OpenAI's API. It currently only runs on Windows, but I hope to eventually make it cross-platform.

It started out as a simple desktop request/response app, but I've expanded it to help test building prompts with different 'prefix' text describing the persona, purpose, persuasion, and process to include when submitting the prompt.

These prompt prefixes can be saved to be re-used (and/or edited) with future prompts.

The name "Legba" is from [Papa Legba](https://en.wikipedia.org/wiki/Papa_Legba), a voodoo *loa* who stood at the crossroads between humans and God, to help them communicate.

# Setup steps
- Create an [OpenAI API account](https://openai.com/blog/openai-api)
- Get an [API key](https://platform.openai.com/api-keys)
- Fund the API account. It may take a few hours for the API to recognize the account was funded.
- Make your API key available to Legba
  - In the Legba.WPF project, modify appsettings.json to use your api key and (optional) organization Id.
- Test the app

# Support
Please report any problems at one of these places:
- [Issues](https://github.com/LillySoftwareConsulting/Legba/issues)
- [Discussions](https://github.com/LillySoftwareConsulting/Legba/discussions)

# Future Plans
- Convert the UI to something that runs cross-platform - probably [Avalonia](https://avaloniaui.net/).
- Able to share prompts with friends, co-workers, or the public.
  - Possibly host a web service to share prompt prefixes.
- Connect to other LLMs besides OpenAI (on the web and/or locally).
