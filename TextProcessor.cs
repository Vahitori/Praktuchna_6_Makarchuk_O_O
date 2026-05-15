using System.Text;

namespace StudentManagement;

public class TextProcessor
{
    public string Reverse(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        return text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public int CountCharacters(string text, bool ignoreWhitespace = true)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        if (ignoreWhitespace)
        {
            return text.Count(c => !char.IsWhiteSpace(c));
        }
        return text.Length;
    }

    public string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text?.Trim() ?? string.Empty;
        return string.Join(" ", text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)).Trim();
    }

    public bool IsPalindrome(string text, bool ignoreCase = true, bool ignoreSpaces = true)
    {
        if (string.IsNullOrEmpty(text)) return true;
        
        string processed = text;
        if (ignoreSpaces)
        {
            processed = new string(processed.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
        if (ignoreCase)
        {
            processed = processed.ToLower();
        }

        string reversed = Reverse(processed);
        return processed == reversed;
    }

    public string ReplaceMultiple(string text, Dictionary<string, string> replacements)
    {
        if (string.IsNullOrEmpty(text) || replacements == null || replacements.Count == 0) return text;
        
        StringBuilder sb = new StringBuilder(text);
        foreach (var replacement in replacements)
        {
            sb.Replace(replacement.Key, replacement.Value);
        }
        return sb.ToString();
    }

    public string[] SplitIntoSentences(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return Array.Empty<string>();
        return text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(s => s.Trim())
                   .Where(s => !string.IsNullOrEmpty(s))
                   .ToArray();
    }

    public string BuildGroupReport(StudentGroup group)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"========================================");
        sb.AppendLine($"ЗВІТ ГРУПИ: {group.GroupName}");
        sb.AppendLine($"Спеціальність: {group.Specialty}");
        sb.AppendLine($"Курс: {group.Course}");
        sb.AppendLine($"Дата формування: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        sb.AppendLine($"========================================");
        sb.AppendLine();
        
        var students = group.GetAllStudents();
        if (students.Count == 0)
        {
            sb.AppendLine("У групі немає студентів.");
        }
        else
        {
            sb.AppendLine($"{"№",-3} {"ПІБ",-30} {"Бал",-5} {"Статус",-15}");
            sb.AppendLine(new string('-', 60));
            int i = 1;
            foreach (var s in students)
            {
                sb.AppendLine($"{i++, -3} {s.FullName, -30} {s.AverageGrade, -5} {s.Status}");
            }
        }
        
        sb.AppendLine();
        sb.AppendLine($"Всього студентів: {group.GroupSize}");
        sb.AppendLine($"Середній бал групи: {group.AverageGroupGrade}");
        sb.AppendLine($"========================================");
        
        return sb.ToString();
    }

    public string ComparePerformance(int iterations)
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine($"Порівняння продуктивності (ітерацій: {iterations}):");
        
        // String concatenation
        var sw = System.Diagnostics.Stopwatch.StartNew();
        string s = "";
        for (int i = 0; i < iterations; i++)
        {
            s += "test";
        }
        sw.Stop();
        long stringTime = sw.ElapsedMilliseconds;
        report.AppendLine($"- String (+): {stringTime} ms");

        // StringBuilder
        sw.Restart();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < iterations; i++)
        {
            sb.Append("test");
        }
        sw.Stop();
        long sbTime = sw.ElapsedMilliseconds;
        report.AppendLine($"- StringBuilder: {sbTime} ms");
        
        if (sbTime > 0)
            report.AppendLine($"StringBuilder швидше в {Math.Round((double)stringTime / sbTime, 1)} разів.");
        else if (stringTime > 0)
            report.AppendLine("StringBuilder значно швидше (час < 1ms).");

        return report.ToString();
    }

    // Variant 2: Sentiment analysis
    public string AnalyzeGroupMood(StudentGroup group)
    {
        var positiveKeywords = new[] { "відмінно", "чудово", "добре", "успіх", "задоволений", "цікаво" };
        var negativeKeywords = new[] { "погано", "проблема", "важко", "втомився", "не встигаю", "борг" };
        var neutralKeywords = new[] { "відпустка", "план", "сесія", "пари" };

        int positiveCount = 0;
        int negativeCount = 0;
        int neutralCount = 0;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Аналіз настрою групи:");
        
        foreach (var student in group.GetAllStudents())
        {
            if (string.IsNullOrWhiteSpace(student.Notes)) continue;
            
            string notesLower = student.Notes.ToLower();
            
            int p = positiveKeywords.Count(k => notesLower.Contains(k));
            int n = negativeKeywords.Count(k => notesLower.Contains(k));
            int neu = neutralKeywords.Count(k => notesLower.Contains(k));
            
            positiveCount += p;
            negativeCount += n;
            neutralCount += neu;
            
            if (p > 0 || n > 0 || neu > 0)
            {
                sb.AppendLine($"- {student.FullName}: +{p}, -{n}, ?{neu}");
            }
        }
        
        sb.AppendLine();
        sb.AppendLine($"Загальна статистика:");
        sb.AppendLine($"Позитив: {positiveCount}");
        sb.AppendLine($"Негатив: {negativeCount}");
        sb.AppendLine($"Нейтрально: {neutralCount}");
        
        string mood = "Нейтральний";
        if (positiveCount > negativeCount * 1.5) mood = "Позитивний";
        else if (negativeCount > positiveCount) mood = "Напружений";
        
        sb.AppendLine($"Загальний настрій: {mood}");
        
        return sb.ToString();
    }
}
