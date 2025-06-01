using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;

namespace Legba.Engine.Services;

public class FileCollector
{
    #region Constants and Fields

    private static readonly IReadOnlyList<string> s_fileExtensionsToInclude =
    [
        ".cs", ".vb", ".xaml", ".xaml.cs", ".xaml.vb", ".vsixmanifest", ".vsct", ".resx", 
        ".targets", ".props", ".ruleset", ".settings", ".cshtml", ".vbhtml", ".aspx", ".ascx"
    ];
    private static readonly IReadOnlyList<Regex> s_excludedFilePatterns =
    [
        new Regex(
            @"(AssemblyInfo|AssemblyAttributes|\.g|\.g\.i|\.Designer|\.generated|\.razor\.cs|\.g\.cshtml|\.tt|\.t4|\.Reference)\.(cs|vb|xaml|cshtml)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled)
    ];

    private static readonly IReadOnlyList<string> s_excludedDirectories =
        [ "bin", "obj", "node_modules", ".vs", "packages" ];

    #endregion

    #region Public Methods

    /// <summary>
    /// Retrieves all code files (*.cs|*.vb) from a solution, 
    /// excluding files that match specific patterns (generally the automatically created files).
    /// </summary>
    /// <param name="solutionFileName">Name of Visual Studio solution</param>
    /// <returns>ReadOnlyList of code files in a Visual Studio solution</returns>
    /// <exception cref="InvalidOperationException">Exception if .sln file cannot be loaded</exception>
    public static async Task<IReadOnlyList<string>> GetFilesFromSolutionAsync(string solutionFileName)
    {
        ValidateFilePath(solutionFileName, nameof(solutionFileName));

        using var workspace = MSBuildWorkspace.Create();

        var solution = await workspace.OpenSolutionAsync(solutionFileName)
            ?? throw new InvalidOperationException("Failed to open solution.");

        var filePaths = new ConcurrentBag<string>();

        foreach (var project in solution.Projects)
        {
            foreach (var document in project.Documents)
            {
                if (!IsExcludedFile(document.Name))
                {
                    filePaths.Add(document.FilePath ?? document.Name);
                }
            }
        }

        return filePaths.ToList().AsReadOnly();
    }

    /// <summary>
    /// Retrieves all code files (*.cs|*.vb) from a project,
    /// excluding files that match specific patterns (generally the automatically created files).
    /// </summary>
    /// <param name="projectFileName">Name of the Visual Studio project</param>
    /// <returns>ReadOnlyList of code files in a Visual Studio project</returns>
    /// <exception cref="InvalidOperationException">Exception if .csproj|.vbproj file cannot be loaded</exception>
    public static async Task<IReadOnlyList<string>> GetFilesFromProjectAsync(string projectFileName)
    {
        ValidateFilePath(projectFileName, nameof(projectFileName));

        using var workspace = MSBuildWorkspace.Create();

        var project = await workspace.OpenProjectAsync(projectFileName)
            ?? throw new InvalidOperationException("Failed to open project.");

        var filePaths = new List<string>();

        foreach (var document in project.Documents)
        {
            if (!IsExcludedFile(document.Name))
            {
                filePaths.Add(document.FilePath ?? document.Name);
            }
        }

        return filePaths.AsReadOnly();
    }

    /// <summary>
    /// Retrieves all code files (*.cs|*.vb) from specified folders,
    /// excluding files that match specific patterns (generally the automatically created files).
    /// </summary>
    /// <param name="folderPaths">List of folder paths</param>
    /// <returns>ReadOnlyList of code files</returns>
    public static async Task<IReadOnlyList<string>> GetFilesFromFoldersAsync(string[] folderPaths)
    {
        ValidateFolderPaths(folderPaths);

        var filePaths = new ConcurrentBag<string>();

        await Parallel.ForEachAsync(folderPaths, async (folderPath, ct) =>
        {
            await foreach (var file in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .ToAsyncEnumerable()
                .Where(file =>
                    s_fileExtensionsToInclude.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) &&
                    !s_excludedDirectories.Any(dir => file.Contains(Path.DirectorySeparatorChar + dir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) &&
                    !IsExcludedFile(file)))
            {
                filePaths.Add(file);
            }
        });

        return filePaths.ToList().AsReadOnly();
    }

    public static async Task<IReadOnlyList<string>> GetFilesFromFilesAsync(string[] filePaths)
    {
        ValidateFilePaths(filePaths);

        return await Task.FromResult<IReadOnlyList<string>>(filePaths.AsReadOnly());
    }

    #endregion

    #region Private Helper Methods

    private static void ValidateFilePath(string filePath, string paramName)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException($"{paramName} cannot be null or empty.", paramName);
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"{paramName} not found.", filePath);
        }
    }

    private static void ValidateFolderPaths(string[] folderPaths)
    {
        if (folderPaths.Length == 0)
        {
            throw new ArgumentException("Folder paths cannot be null or empty.", nameof(folderPaths));
        }

        var invalidPaths = folderPaths.Where(fp => !Directory.Exists(fp)).ToArray();

        if (invalidPaths.Length != 0)
        {
            throw new DirectoryNotFoundException("One or more folder paths do not exist: " + string.Join(", ", invalidPaths));
        }
    }

    private static void ValidateFilePaths(string[] filePaths)
    {
        if (filePaths.Length == 0)
        {
            throw new ArgumentException("File paths cannot be null or empty.", nameof(filePaths));
        }

        var invalidPaths = filePaths.Where(fp => !File.Exists(fp)).ToArray();

        if (invalidPaths.Length != 0)
        {
            throw new FileNotFoundException("One or more files do not exist: " + string.Join(", ", invalidPaths));
        }
    }

    private static bool IsExcludedFile(string fileName)
    {
        return s_excludedFilePatterns.Any(regex => regex.IsMatch(fileName));
    }

    #endregion
}
