using System.Text;

namespace StudentManagement;

public class PortLogger
{
    private readonly StringBuilder _logs = new StringBuilder();

    public void LogOperation(string operation, int portNumber, string details)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        _logs.AppendLine($"[{timestamp}] Port {portNumber} | {operation}: {details}");
    }

    public void SaveLogToFile(string filePath)
    {
        File.WriteAllText(filePath, _logs.ToString());
    }

    public string GetFullLog() => _logs.ToString();
    
    public void Clear() => _logs.Clear();
}
