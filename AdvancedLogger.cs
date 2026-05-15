using System.Text;

namespace StudentManagement;

public class AdvancedLogger
{
    private readonly StringBuilder _logBuilder = new();
    private readonly List<string> _logEntries = new();

    public void Log(string level, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string entry = $"[{timestamp}] [{level.ToUpper()}] {message}";
        _logBuilder.AppendLine(entry);
        _logEntries.Add(entry);
    }

    public void SaveToFile(string path)
    {
        try
        {
            File.WriteAllText(path, _logBuilder.ToString());
            Log("INFO", $"Logs saved to {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Error saving logs: {ex.Message}");
        }
    }

    public string GetLogsByLevel(string level)
    {
        StringBuilder sb = new StringBuilder();
        string levelTag = $"[{level.ToUpper()}]";
        
        // This is a bit inefficient to search through a single string, 
        // so we use the list of entries or split the string.
        // Let's use the list for efficiency.
        foreach (var entry in _logEntries)
        {
            if (entry.Contains(levelTag))
            {
                sb.AppendLine(entry);
            }
        }
        return sb.ToString();
    }

    public void Clear()
    {
        _logBuilder.Clear();
        _logEntries.Clear();
        Log("INFO", "Logs cleared");
    }

    public string GetLast(int count)
    {
        if (count <= 0) return string.Empty;
        int skip = Math.Max(0, _logEntries.Count - count);
        return string.Join(Environment.NewLine, _logEntries.Skip(skip));
    }

    public string GetAllLogs() => _logBuilder.ToString();
}
