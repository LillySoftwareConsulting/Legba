namespace Legba.Engine.Models;

public class PromptPrefixesExport
{
    public List<Persona> Personas { get; set; } = new();
    public List<Purpose> Purposes { get; set; } = new();
    public List<Persuasion> Persuasions { get; set; } = new();
    public List<Process> Processes { get; set; } = new();
}