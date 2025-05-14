using System.IO;
using System.Text;

namespace Legba.Engine.Services;

public class FileConsolidator
{
    private static readonly string _separator = new('=', 30);

    public static async Task<string> GetFilesAsStringAsync(IReadOnlyList<string> files)
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
            sb.AppendLine(Path.GetFileName(file));
            sb.AppendLine(_separator);
            sb.Append(await File.ReadAllTextAsync(file));
            sb.AppendLine("");
        }

        return sb.ToString();
    }
}
