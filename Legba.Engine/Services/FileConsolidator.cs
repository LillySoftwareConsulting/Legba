using System.IO;
using System.Text;

namespace Legba.Engine.Services;

public class FileConsolidator
{
    private static readonly string _separator = new('=', 30);

    public event EventHandler<string> StatusUpdated = null!;

    public async Task<string> GetFilesAsStringAsync(IReadOnlyList<string> files)
    {
        if (files == null || files.Count == 0)
        {
            return string.Empty;
        }

        // Calculate total size for StringBuilder capacity
        long estimatedSize = 0;
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            estimatedSize += fileInfo.Length;
            estimatedSize += Path.GetFileName(file).Length + Environment.NewLine.Length;
            estimatedSize += _separator.Length + Environment.NewLine.Length;
            estimatedSize += Environment.NewLine.Length;
        }

        // Set StringBuilder's capacity, cap at int.MaxValue
        var sb = new StringBuilder(Math.Min((int)estimatedSize, int.MaxValue));
        foreach (var file in files)
        {
            RaiseStatusMessage($"Reading: {file}");

            sb.AppendLine(Path.GetFileName(file));
            sb.AppendLine(_separator);
            sb.Append(await File.ReadAllTextAsync(file));
            sb.AppendLine("");
        }

        return sb.ToString();
    }

    public async Task ConsolidateToSingleFileAsync(IReadOnlyList<string> files, string outputFilePath)
    {
        if (files == null || files.Count == 0)
        {
            throw new ArgumentException("No files to consolidate.", nameof(files));
        }

        if (string.IsNullOrEmpty(outputFilePath))
        {
            throw new ArgumentException("Output file path cannot be null or empty.", nameof(outputFilePath));
        }

        // Create the output directories if they do not exist
        string? directoryPath = Path.GetDirectoryName(outputFilePath);

        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Use streams to read source files and write consolidated file
        using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var outputWriter = new StreamWriter(outputStream);
        foreach (var file in files)
        {
            RaiseStatusMessage($"Consolidating: {file}");

            // Write filename and separator
            await outputWriter.WriteLineAsync(Path.GetFileName(file));
            await outputWriter.WriteLineAsync(_separator);

            // Read the file content in chunks to avoid memory issues with large files
            using (var inputStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var inputReader = new StreamReader(inputStream))
            {
                char[] buffer = new char[8192]; // 8KB buffer
                int charsRead;
                while ((charsRead = await inputReader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await outputWriter.WriteAsync(buffer, 0, charsRead);
                }
            }

            // Write a line feed after each file's content
            await outputWriter.WriteLineAsync();
        }
    }

    private void RaiseStatusMessage(string statusMessage)
    {
        StatusUpdated?.Invoke(this, statusMessage);
    }
}
