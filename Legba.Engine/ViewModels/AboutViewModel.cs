using CSharpExtender.ExtensionMethods;
using System.Reflection;
using System.Windows.Input;

namespace Legba.Engine.ViewModels;

public class AboutViewModel
{
    private int initialCopyrightYear = 2023;

    private static readonly Version s_version =
        Assembly.GetExecutingAssembly().GetName().Version;

    public string VersionText =>
        $"{s_version.Major}.{s_version.Minor}.{s_version.Revision}";
    public string Copyright =>
        $"© {(DateTime.Now.Year == initialCopyrightYear ? $"{initialCopyrightYear}" : $"{initialCopyrightYear} - {DateTime.Now.Year}")}, Lilly Software Consulting";
    public string License =>
        "Licensed under the MIT License";
    public string ContactInformation =>
        "https://lillysoftwareconsulting.com/contact-me/";
    public string ProjectWebsite =>
        "https://lillysoftwareconsulting.com/legba/";
    public string SourceCode =>
        "https://github.com/LillySoftwareConsulting/Legba";
    public string Disclaimer =>
        "This software is provided as-is, without warranty of any kind, " +
        "express or implied.  Use at your own risk.";
    public string Credits =>
        "This software uses the following third-party components:\n" +
        "• LiteDB\n" +
        "• ScottLilly.CSharpExtender";

    public ICommand OpenUrlCommand { get; }

    public AboutViewModel()
    {
        OpenUrlCommand = new TypedRelayCommand<string>(OpenUrl);
    }

    private void OpenUrl(string url)
    {
        if (url.HasText())
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
        }
    }
}