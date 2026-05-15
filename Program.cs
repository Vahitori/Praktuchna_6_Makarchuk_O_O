using System.Text;
using StudentManagement;
using Praktuchna_Makarchuk_O_O;
using Variant2;

// ─── Initial data ─────────────────────────────────────────────────────────────
var group = new StudentGroup("КН-21", "Комп'ютерні науки", 2);
var matrix = new PortMatrix();
var logger = new AdvancedLogger();
var processor = new TextProcessor();

try
{
    // Звичайний студент
    var s1 = new Student("Іваненко Іван Іванович", new DateTime(2003, 5, 12), "12345678", "ivanen@gmail.com")
    {
        CourseProgress = 85
    };
    s1.AddGrade(new GradePoint(9.5));
    s1.AddGrade(new GradePoint(8.0));
    group.AddStudent(s1);

    // Відмінник
    var es = new ExcellentStudent("Коваль Марія Петрівна", new DateTime(2002, 3, 15), "22345678", "koval@edu.ua");
    es.AddGrade(new GradePoint(9.8));
    es.AddGrade(new GradePoint(9.5));
    group.AddStudent(es);

    // Іноземний студент
    var fs = new ForeignStudent("Smith John", new DateTime(2001, 7, 22), "33345678", "smith@uni.uk",
                                "Велика Британія", "University of London")
    {
        HasLanguageCertificate = true,
        CourseProgress = 65
    };
    fs.AddGrade(new GradePoint(7.2));
    group.AddStudent(fs);

    // Студент, що працює
    var ws = new WorkingStudent("Мельник Олексій Вікторович", new DateTime(2000, 11, 5),
                                "44345678", "melnyk@work.ua",
                                "IT-Corp", "Junior Developer", 20)
    {
        CourseProgress = 72
    };
    ws.AddGrade(new GradePoint(7.8));
    ws.AddGrade(new GradePoint(8.0));
    group.AddStudent(ws);

    // Аспірант
    var gs = new GraduateStudent("Сидоренко Юлія Олегівна", new DateTime(1998, 4, 10),
                                 "55345678", "sydorenko@univ.ua",
                                 "Штучний інтелект у медицині", "проф. Кириленко О.П.")
    {
        YearOfStudy = 2
    };
    gs.AddGrade(new GradePoint(9.0));
    group.AddStudent(gs);

    // Звичайний студент з низьким балом
    var s2 = new Student("Петренко Олена Сергіївна", new DateTime(2002, 8, 20), "66345678", "petrenko@ukr.net")
    {
        CourseProgress = 40
    };
    s2.AddGrade(new GradePoint(4.5));
    s2.AddGrade(new GradePoint(5.0));
    group.AddStudent(s2);
}
catch (Exception ex) { logger.Log("ERROR", ex.Message); }

bool running = true;
while (running)
{
    PrintMenuHeader();
    string? choice = Console.ReadLine()?.Trim();
    Console.Clear();

    try
    {
        switch (choice)
        {
            case "1":  AddStudentMenu(group, logger); break;
            case "2":  RemoveStudentMenu(group, logger); break;
            case "3":  ListAllStudents(group); break;
            case "4":  SearchStudentMenu(group); break;
            case "5":  EditStudentMenu(group, logger); break;
            case "6":  ShowExcellentAndFailing(group); break;
            case "7":  group.PrintStatistics(); break;
            case "8":  group.SaveToFile(); logger.Log("INFO", "Data saved"); break;
            case "9":  group.LoadFromFile(); logger.Log("INFO", "Data loaded"); break;
            case "10": SearchByFragmentMenu(group); break;
            case "11": Console.WriteLine(processor.BuildGroupReport(group)); break;
            case "12": NormalizeNotes(group, processor, logger); break;
            case "13": CheckPalindromes(group, processor); break;
            case "14": ExportCsv(group, logger); break;
            case "15": ImportFromText(group, logger); break;
            case "16": ShowLogs(logger); break;
            case "17": Console.WriteLine(processor.ComparePerformance(10000)); break;
            case "18": TextProcessingMenu(processor); break;
            case "19": CompareStudentsMenu(group); break;
            case "20": MergeGroupsMenu(group, logger); break;
            case "21": VectorDemo(); break;
            case "22": GradePointDemo(); break;
            case "23": BestStudentMenu(group); break;
            case "24": FractionDemo(); break;
            // ── PR5 NEW ITEMS ──
            case "25": ShowStudentTypeInfo(group); break;
            case "26": ShowByTypeMenu(group); break;
            case "27": ShowScholarshipInfo(group); break;
            case "28": EnrollStudentDemo(group); break;
            case "29": VehicleHierarchyDemo(); break;
            case "30": VehiclePolymorphismDemo(); break;
            case "0":  running = false; break;
            default:   Console.WriteLine("  [!] Невірний вибір."); break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  [!] Помилка: {ex.Message}");
        logger.Log("ERROR", ex.Message);
    }

    if (running)
    {
        Console.WriteLine("\n  Натисніть будь-яку клавішу...");
        Console.ReadKey(true);
        Console.Clear();
    }
}

// ─── Menu ─────────────────────────────────────────────────────────────────────
static void PrintMenuHeader()
{
    Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║    СИСТЕМА УПРАВЛІННЯ НАВЧАЛЬНОЮ ГРУПОЮ (ПР №5 - Варіант 2)  ║");
    Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
    Console.WriteLine("║  1.  Додати студента         16. Логи системи               ║");
    Console.WriteLine("║  2.  Видалити студента        17. Продуктивність (SB)        ║");
    Console.WriteLine("║  3.  Список студентів         18. Обробка тексту             ║");
    Console.WriteLine("║  4.  Пошук студента           19. Порівняти студентів        ║");
    Console.WriteLine("║  5.  Редагувати студента      20. Об'єднати групи (+)        ║");
    Console.WriteLine("║  6.  Відмінники / Слабкі      21. Демо Vector                ║");
    Console.WriteLine("║  7.  Статистика групи         22. Демо GradePoint            ║");
    Console.WriteLine("║  8.  Зберегти дані            23. Найкращий студент          ║");
    Console.WriteLine("║  9.  Завантажити дані         24. Демо Fraction              ║");
    Console.WriteLine("║  10. Пошук за фрагментом  ── PR5 (Успадкування) ──          ║");
    Console.WriteLine("║  11. Звіт (SB)                25. Типи студентів (GetInfo)  ║");
    Console.WriteLine("║  12. Нормалізація             26. Фільтр за типом            ║");
    Console.WriteLine("║  13. Паліндроми в нотатках    27. Стипендії + фонд          ║");
    Console.WriteLine("║  14. Експорт у CSV            28. Зарахування (Enroll)      ║");
    Console.WriteLine("║  15. Імпорт з тексту          29. Варіант 2: Vehicle демо   ║");
    Console.WriteLine("║                               30. Варіант 2: Поліморфізм   ║");
    Console.WriteLine("║                                0. Вийти                     ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
    Console.Write("  Ваш вибір: ");
}

// ─── Existing handlers (PR1–PR4) ─────────────────────────────────────────────

static void AddStudentMenu(StudentGroup group, AdvancedLogger logger)
{
    Console.Write("  ПІБ: "); string name = Console.ReadLine() ?? "";
    Console.Write("  Дата нар (рррр-мм-дд): "); DateTime dob = DateTime.Parse(Console.ReadLine() ?? "2000-01-01");
    Console.Write("  Залікова: "); string rb = Console.ReadLine() ?? "";
    Console.Write("  Email: "); string email = Console.ReadLine() ?? "";
    Console.Write("  Прогрес (0-100): "); int progress = int.Parse(Console.ReadLine() ?? "0");
    var s = new Student(name, dob, rb, email) { CourseProgress = progress };
    if (group.AddStudent(s)) logger.Log("INFO", $"Added student {name}");
}

static void RemoveStudentMenu(StudentGroup group, AdvancedLogger logger)
{
    Console.Write("  Номер залікової: ");
    string rb = Console.ReadLine() ?? "";
    if (group.RemoveStudent(rb)) logger.Log("INFO", $"Removed student {rb}");
}

static void ListAllStudents(StudentGroup group)
{
    var students = group.GetAllStudents();
    if (students.Count == 0) Console.WriteLine("Група порожня.");
    else foreach (var s in students) Console.WriteLine(s.GetFormattedInfo(false));
}

static void SearchStudentMenu(StudentGroup group)
{
    Console.Write("  Номер залікової: ");
    string query = Console.ReadLine() ?? "";
    var found = group[query];
    if (found == null) Console.WriteLine("Нічого не знайдено.");
    else Console.WriteLine(found.GetFormattedInfo(true));
}

static void EditStudentMenu(StudentGroup group, AdvancedLogger logger)
{
    Console.Write("  Залікова: ");
    string rb = Console.ReadLine() ?? "";
    var s = group[rb];
    if (s != null)
    {
        Console.Write("  Додати оцінку (0-10): ");
        if (double.TryParse(Console.ReadLine(), out double g))
        {
            s.AddGrade(new GradePoint(g));
            logger.Log("INFO", $"Added grade {g} to {s.FullName}");
        }
        Console.Write("  Новий прогрес (0-100): ");
        if (int.TryParse(Console.ReadLine(), out int p)) s.CourseProgress = p;
    }
}

static void ShowExcellentAndFailing(StudentGroup group)
{
    Console.WriteLine("  ── Відмінники ──");
    foreach (var s in group.GetExcellentStudents()) Console.WriteLine($"  {s.GetInfo()}");
    Console.WriteLine("\n  ── Слабкі (бал < 60) ──");
    foreach (var s in group.GetAllStudents().Where(s => s.IsFailing())) Console.WriteLine($"  {s.GetInfo()}");
}

static void SearchByFragmentMenu(StudentGroup group)
{
    Console.Write("  Введіть фрагмент ПІБ: ");
    string frag = Console.ReadLine() ?? "";
    Console.WriteLine(group.SearchByNameFragment(frag));
}

static void NormalizeNotes(StudentGroup group, TextProcessor proc, AdvancedLogger logger)
{
    foreach (var s in group.GetAllStudents()) s.Notes = proc.Normalize(s.Notes);
    Console.WriteLine("  [✓] Нотатки нормалізовано.");
    logger.Log("INFO", "Notes normalized");
}

static void CheckPalindromes(StudentGroup group, TextProcessor proc)
{
    Console.WriteLine("Перевірка паліндромів у нотатках:");
    foreach (var s in group.GetAllStudents())
        if (!string.IsNullOrWhiteSpace(s.Notes) && proc.IsPalindrome(s.Notes))
            Console.WriteLine($"- {s.FullName}: {s.Notes}");
}

static void ExportCsv(StudentGroup group, AdvancedLogger logger)
{
    File.WriteAllText("group_export.csv", group.ExportToCsv(), Encoding.UTF8);
    Console.WriteLine("  [✓] Експортовано у group_export.csv");
    logger.Log("INFO", "Exported to CSV");
}

static void ImportFromText(StudentGroup group, AdvancedLogger logger)
{
    Console.WriteLine("Формат: ПІБ | Залікова | Email | Дата_ДД.ММ.РРРР");
    Console.WriteLine("Порожній рядок — завершити.");
    var sb = new StringBuilder();
    string? line;
    while (!string.IsNullOrEmpty(line = Console.ReadLine())) sb.AppendLine(line);
    group.ImportStudentsFromText(sb.ToString());
    logger.Log("INFO", "Imported from text");
}

static void ShowLogs(AdvancedLogger logger)
{
    Console.WriteLine("── ЛОГИ СИСТЕМИ ──");
    Console.WriteLine(logger.GetAllLogs());
}

static void TextProcessingMenu(TextProcessor proc)
{
    Console.Write("Введіть текст: ");
    string text = Console.ReadLine() ?? "";
    Console.WriteLine($"1. Реверс: {proc.Reverse(text)}");
    Console.WriteLine($"2. Кількість слів: {proc.CountWords(text)}");
}

static void CompareStudentsMenu(StudentGroup group)
{
    Console.Write("  Залікова №1: "); string rb1 = Console.ReadLine() ?? "";
    Console.Write("  Залікова №2: "); string rb2 = Console.ReadLine() ?? "";
    var s1 = group[rb1]; var s2 = group[rb2];
    if (s1 != null && s2 != null)
    {
        Console.WriteLine($"  {s1.FullName} vs {s2.FullName}");
        Console.WriteLine($"  s1 > s2: {s1 > s2}  |  s1 < s2: {s1 < s2}  |  s1 == s2: {s1 == s2}");
        var team = s1 + s2;
        Console.WriteLine($"  Команда (+): {team}");
    }
    else Console.WriteLine("  [!] Студентів не знайдено.");
}

static void MergeGroupsMenu(StudentGroup group, AdvancedLogger logger)
{
    var other = new StudentGroup("TEST-99", "Тестування", 1);
    other.AddStudent(new Student("Тестовий Тест Тестович", DateTime.Today, "99999999", "test@test.com"));
    Console.WriteLine($"  Поточна: {group.GroupName} ({group.GroupSize} осіб)");
    Console.WriteLine($"  Інша:    {other.GroupName} ({other.GroupSize} осіб)");
    var merged = group + other;
    Console.WriteLine($"  Результат (+): {merged.GroupName} ({merged.GroupSize} осіб)");
    logger.Log("INFO", "Groups merged");
}

static void VectorDemo()
{
    var v1 = new Vector(1, 2, 3); var v2 = new Vector(4, 5, 6);
    Console.WriteLine($"  v1: {v1}  |  v2: {v2}");
    Console.WriteLine($"  v1 + v2: {v1 + v2}");
    Console.WriteLine($"  v1 * 2: {v1 * 2}");
    Console.WriteLine($"  v1 > v2: {v1 > v2}");
    Console.WriteLine($"  |v1|: {(double)v1:F2}");
    v1++; Console.WriteLine($"  v1++: {v1}");
}

static void GradePointDemo()
{
    var g1 = new GradePoint(7.5); var g2 = new GradePoint(9.2);
    Console.WriteLine($"  g1: {g1}  |  g2: {g2}");
    Console.WriteLine($"  g1 + g2: {g1 + g2}");
    Console.WriteLine($"  g2 висока? {(g2 ? "Так" : "Ні")}");
    Console.WriteLine($"  g1 as double: {(double)g1}");
}

static void BestStudentMenu(StudentGroup group)
{
    var best = group.GetBestStudent();
    if (best != null) Console.WriteLine($"  Найкращий (operator >): {best.GetInfo()}");
    else Console.WriteLine("  Група порожня.");
}

static void FractionDemo()
{
    var f1 = new Fraction(1, 2); var f2 = new Fraction(1, 3);
    Console.WriteLine($"  f1: {f1}  |  f2: {f2}");
    Console.WriteLine($"  f1 + f2: {f1 + f2}");
    Console.WriteLine($"  f1 * f2: {f1 * f2}");
    Console.WriteLine($"  f1 / f2: {f1 / f2}");
    Console.WriteLine($"  f1 as double: {(double)f1:F3}");
    Console.WriteLine($"  4/8 auto-reduced: {new Fraction(4, 8)}");
}

// ─── PR5 NEW handlers ─────────────────────────────────────────────────────────

/// <summary>25. Показ GetInfo() через поліморфізм — кожен тип повертає свій опис.</summary>
static void ShowStudentTypeInfo(StudentGroup group)
{
    Console.WriteLine("  ── GetInfo() через поліморфізм (PR5) ──\n");
    foreach (var s in group.GetAllStudents())
    {
        Console.WriteLine($"  {s.GetInfo()}");
        Console.WriteLine();
    }
}

/// <summary>26. Фільтрація за конкретним типом через GetMembersByType&lt;T&gt;().</summary>
static void ShowByTypeMenu(StudentGroup group)
{
    Console.WriteLine("  Оберіть тип:");
    Console.WriteLine("  1 - ExcellentStudent   2 - ForeignStudent");
    Console.WriteLine("  3 - WorkingStudent     4 - GraduateStudent   5 - Student (базовий)");
    Console.Write("  Вибір: ");
    string t = Console.ReadLine() ?? "5";

    IEnumerable<Student> result = t switch
    {
        "1" => group.GetMembersByType<ExcellentStudent>(),
        "2" => group.GetMembersByType<ForeignStudent>(),
        "3" => group.GetMembersByType<WorkingStudent>(),
        "4" => group.GetMembersByType<GraduateStudent>(),
        _   => group.GetMembersByType<Student>()
    };

    Console.WriteLine($"\n  ── Результати ──");
    foreach (var s in result) Console.WriteLine($"  {s.GetInfo()}");
}

/// <summary>27. Стипендії по кожному студенту + загальний фонд.</summary>
static void ShowScholarshipInfo(StudentGroup group)
{
    Console.WriteLine("  ── Інформація про стипендії ──\n");
    foreach (var s in group.GetAllStudents())
    {
        decimal sch = s.CalculateScholarship();
        Console.WriteLine($"  {s.FullName,-30} [{s.GetType().Name,-18}] → {sch:C0}");
    }
    Console.WriteLine($"\n  Загальний стипендіальний фонд: {group.GetTotalScholarship():C0}");
}

/// <summary>28. Демонстрація виклику Enroll() через поліморфізм.</summary>
static void EnrollStudentDemo(StudentGroup group)
{
    Console.WriteLine("  ── Зарахування через поліморфізм IUniversityMember.Enroll() ──\n");
    foreach (var s in group.GetAllStudents())
    {
        Console.WriteLine($"  → Зарахування: {s.FullName} ({s.GetType().Name})");
        s.Enroll();
        Console.WriteLine();
    }
}

/// <summary>29. Демо Vehicle hierarchy (Варіант 2).</summary>
static void VehicleHierarchyDemo()
{
    Console.WriteLine("  ── Варіант 2: Ієрархія транспортних засобів ──\n");

    Vehicle car = new Car("Toyota", "Camry", 2020, 45000, 9.0, true, "Чорний");
    Vehicle bus = new Bus("Mercedes", "Sprinter", 2019, 25, "14A", false, 120000, "Білий");
    Vehicle truck = new Truck("Volvo", "FH16", 2018, 20.0, true, 250000, "Сірий")
    {
        CurrentLoadTons = 15.0
    };

    Console.WriteLine("  Список транспортних засобів:");
    Console.WriteLine($"  {car.GetInfo()}");
    Console.WriteLine($"  {bus.GetInfo()}");
    Console.WriteLine($"  {truck.GetInfo()}");

    Console.WriteLine("\n  ── Вартість палива на 500 км ──");
    Console.WriteLine($"  Легковик:   {car.CalculateFuelCost(500):N2} грн");
    Console.WriteLine($"  Автобус:    {bus.CalculateFuelCost(500):N2} грн");
    Console.WriteLine($"  Вантажівка: {truck.CalculateFuelCost(500):N2} грн");

    Console.WriteLine("\n  ── Оператори ──");
    Console.WriteLine($"  car > truck (пробіг): {car > truck}");
    Console.WriteLine($"  car == truck:         {car == truck}");

    Console.WriteLine("\n  ── Explicit: вік авто ──");
    Console.WriteLine($"  Вік легковика:    {(int)car} рр.");
    Console.WriteLine($"  Вік автобуса:     {(int)bus} рр.");
    Console.WriteLine($"  Вік вантажівки:   {(int)truck} рр.");

    if (truck is Truck t)
    {
        string canLoad = t.CanLoad(3) ? "Так" : "Ні";
        Console.WriteLine($"\n  Вантажівка може завантажити ще 3 т? → {canLoad}");
    }
}

/// <summary>30. Поліморфізм через List&lt;Vehicle&gt;.</summary>
static void VehiclePolymorphismDemo()
{
    Console.WriteLine("  ── Варіант 2: Поліморфізм Vehicle через List&lt;Vehicle&gt; ──\n");

    var fleet = new List<Vehicle>
    {
        new Car("Honda", "Civic", 2021, 30000, 7.5, false, "Синій"),
        new Car("BMW", "X5", 2023, 5000, 11.0, true, "Чорний"),
        new Bus("MAN", "Lion's City", 2018, 45, "5B", false, 200000),
        new Bus("Setra", "S 531 DT", 2022, 80, "Інтерсіті", true, 50000),
        new Truck("DAF", "XF 530", 2020, 25.0, false, 180000) { CurrentLoadTons = 10 },
        new Truck("Scania", "R 650", 2019, 30.0, true, 300000) { CurrentLoadTons = 28 }
    };

    // Поліморфний виклик GetInfo() і CalculateFuelCost()
    Console.WriteLine("  Парк транспорту:");
    for (int i = 0; i < fleet.Count; i++)
        Console.WriteLine($"  [{i+1}] {fleet[i].GetInfo()}");

    Console.WriteLine("\n  ── Витрати палива на 1000 км ──");
    foreach (var v in fleet)
        Console.WriteLine($"  {v.Make,-10} {v.Model,-15}: {v.CalculateFuelCost(1000),10:N2} грн");

    Console.WriteLine("\n  ── Сортування за пробігом (оператор >) ──");
    var sorted = fleet.OrderByDescending(v => v.Mileage).ToList();
    foreach (var v in sorted)
        Console.WriteLine($"  {v.Make} {v.Model} — {v.Mileage:N0} км");

    Console.WriteLine($"\n  ── Загальна вартість обслуговування всього парку ──");
    decimal total = fleet.Sum(v => v.CalculateMaintenanceCost());
    Console.WriteLine($"  {total:C0}");

    Console.WriteLine("\n  ── Фільтрація: тільки вантажівки з рефрижератором ──");
    var fridgeTrucks = fleet.OfType<Truck>().Where(t => t.HasRefrigerator);
    foreach (var t in fridgeTrucks) Console.WriteLine($"  {t.GetInfo()}");
}
