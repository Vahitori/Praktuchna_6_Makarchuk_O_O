# REPORT — Практична робота №5
## Тема: Успадкування та поліморфізм у C#
**Студент:** Макарчук О.О.  
**Варіант:** 2 — Ієрархія транспортних засобів (Vehicle → Car, Bus, Truck)

---

## 1. Мета роботи
- Реалізувати успадкування через абстрактний клас `Person` та базовий клас `Student`.
- Опанувати `virtual` / `override` / `abstract` / `sealed`.
- Застосувати поліморфізм через базовий тип і інтерфейс `IUniversityMember`.
- Реалізувати індивідуальний варіант 2: ієрархія транспортних засобів.
- Інтегрувати нові класи у систему `StudentGroupManagementSystem` (ПР1–ПР4).

---

## 2. Ієрархія класів

### Базовий клас Person (абстрактний)
```
Person (abstract)
├── FullName, DateOfBirth, PersonalEmail, Notes
└── virtual string GetInfo()
```
Містить спільні властивості для всіх осіб. Метод `GetInfo()` є `virtual` — дочірні класи можуть його перевизначити.

### Клас Student : Person, IUniversityMember
```
Student : Person, IUniversityMember
├── RecordBookNumber, AverageGrade, Status, CourseProgress, Grades
├── override GetInfo()
├── virtual CalculateScholarship()  ← IUniversityMember
└── virtual Enroll()               ← IUniversityMember
```
`Student` успадковує `Person` та реалізує інтерфейс `IUniversityMember`.  
Методи `CalculateScholarship()` та `Enroll()` оголошені `virtual`, щоб підкласи могли їх перевизначити.

### Підкласи Student
| Клас | Особливості | override методів |
|---|---|---|
| `ExcellentStudent` | `AchievementTitle`, `HasScholarshipBonus` | `CalculateScholarship` (+50%), `Enroll`, `GetInfo` |
| `ForeignStudent` | `Country`, `HomeUniversity`, `HasLanguageCertificate` | `CalculateScholarship` (0), `Enroll`, `GetInfo` |
| `WorkingStudent` | `Employer`, `JobTitle`, `WorkHoursPerWeek` | `CalculateScholarship` (50%), `Enroll`, `GetInfo` |
| `GraduateStudent` (**sealed**) | `ResearchTopic`, `Supervisor`, `YearOfStudy` | `CalculateScholarship` (фіксована), `Enroll`, `GetInfo` |

`GraduateStudent` оголошено **sealed** — жоден клас не може успадкувати аспіранта далі, оскільки це фінальна ланка.

---

## 3. Інтерфейс IUniversityMember
```csharp
public interface IUniversityMember
{
    decimal CalculateScholarship();
    void Enroll();
}
```
Дозволяє працювати з будь-яким студентом через контракт без знання конкретного типу.  
`StudentGroup.GetTotalScholarship()` використовує `_members.OfType<IUniversityMember>()` — класичний поліморфізм через інтерфейс.

---

## 4. Оновлений StudentGroup
- Внутрішній список змінено з `List<Student>` → `List<Person>`.
- Доданий **generic метод** `GetMembersByType<T>() where T : Person` — фільтрує членів групи за конкретним типом.
- `GetTotalScholarship()` — підраховує загальний стипендіальний фонд через `IUniversityMember`.
- `AddMember(IUniversityMember)` — додає будь-якого члена університету.
- `PrintStatistics()` тепер показує розбивку по типах студентів.

---

## 5. Варіант 2 — Ієрархія транспортних засобів

### Клас Vehicle (abstract)
```
Vehicle (abstract)
├── Make, Model, Year, Mileage, Color
├── virtual CalculateMaintenanceCost()
├── virtual GetInfo()
├── abstract CalculateFuelCost(double distanceKm)  ← обов'язково перевизначити
├── Оператори: >, <, >=, <=, ==, !=
└── Конверсії: explicit (int) — вік; implicit (string) — GetInfo()
```

`CalculateFuelCost` оголошено **abstract** — базовий клас не знає, скільки споживає кожен тип.

### Car : Vehicle
- Додаткові поля: `PassengerCount`, `HasAirCon`, `FuelConsumption`.
- `CalculateFuelCost`: враховує кондиціонер (+10%).
- `CalculateMaintenanceCost`: +20% якщо пробіг > 100 000 км.

### Bus : Vehicle
- Поля: `SeatCount`, `RouteNumber`, `IsIntercity`.
- `CalculateFuelCost`: міжміський +20%.
- `CalculateMaintenanceCost`: подвійна базова вартість.
- Додатковий метод `CalculateRouteRevenue`.

### Truck : Vehicle
- Поля: `MaxPayloadTons`, `CurrentLoadTons`, `HasRefrigerator`.
- `CalculateFuelCost`: витрата залежить від завантаженості + рефрижератор.
- `CalculateMaintenanceCost`: базова × 3 + 5000 за рефрижератор.

---

## 6. Поліморфізм — ключові приклади

### virtual / override
```csharp
// Базовий клас
public virtual string GetInfo() { ... }

// ExcellentStudent
public override string GetInfo() =>
    $"[★ ExcellentStudent] {FullName} | Нагорода: «{AchievementTitle}»...";
```

### abstract
```csharp
// Vehicle — abstract: підклас зобов'язаний реалізувати
public abstract double CalculateFuelCost(double distanceKm);

// Car — конкретна реалізація
public override double CalculateFuelCost(double distanceKm) { ... }
```

### Поліморфізм через базовий тип
```csharp
List<Vehicle> fleet = new() { new Car(...), new Bus(...), new Truck(...) };
// Кожен виклик — правильна реалізація потрібного підкласу
foreach (var v in fleet)
    Console.WriteLine(v.CalculateFuelCost(500)); // virtual dispatch
```

### Liskov Substitution Principle
Клас `GraduateStudent` підставляється замість `Student` скрізь, де очікується `Student`, без порушення поведінки. Те ж саме для `Car`/`Bus`/`Truck` замість `Vehicle`.

### sealed
```csharp
public sealed class GraduateStudent : Student { ... }
// Не можна: class PostDoc : GraduateStudent { } // помилка компіляції
```

---

## 7. Нові пункти меню (ПР5)

| Пункт | Опис |
|---|---|
| 25 | Показ `GetInfo()` через поліморфізм — різні типи студентів |
| 26 | Фільтрація членів групи за типом через `GetMembersByType<T>()` |
| 27 | Стипендії по кожному студенту + загальний фонд |
| 28 | Демо `Enroll()` — зарахування через `IUniversityMember` |
| 29 | Демо Vehicle ієрархії — Car, Bus, Truck |
| 30 | Поліморфізм Vehicle через `List<Vehicle>` |

---

## 8. Складнощі та рішення

| Проблема | Рішення |
|---|---|
| `Student` вже мав всі поля `Person` | Вилучив дублювання — `Person` тепер є єдиним джерелом полів |
| `Clone()` після зміни ієрархії | `MemberwiseClone()` залишається коректним, бо поля — value types або явно копіюються |
| `StudentGroup` зберігав `List<Student>` | Змінено на `List<Person>`, всі запити — через `OfType<Student>()` |
| Використання `base()` у конструкторах | Кожен підклас явно викликає `base(fullName, dob, email)` |
| `sealed` клас `GraduateStudent` | Обраний, бо аспірантура — кінцевий академічний ступінь |

---

## 9. Git-гілки

| Гілка | Призначення |
|---|---|
| `main` | Базовий стан (ПР4) |
| `develop` | Інтеграційна гілка |
| `feature/inheritance-base` | Абстрактний `Person` + `IUniversityMember` |
| `feature/student-hierarchy` | 4 підкласи `Student` |
| `feature/group-refactoring` | `StudentGroup` з generics |
| `feature/variant-2` | Vehicle → Car, Bus, Truck |
| `feature/menu-inheritance` | Меню пунктів 25–30 + `Program.cs` |
| `refactor/hierarchy-cleanup` | Очищення та фінальний REPORT |

---

## 11. Контрольні запитання

**1. Що таке успадкування і як воно реалізоване в даній роботі?**  
Успадкування — це механізм, що дозволяє одному класу (похідному) переймати властивості та методи іншого класу (базового). В роботі реалізована ієрархія: `Person` (базовий) -> `Student` -> `ExcellentStudent`/`ForeignStudent` тощо. Також реалізована ієрархія транспортних засобів: `Vehicle` -> `Car`/`Bus`/`Truck`.

**2. Яка роль ключового слова base у конструкторах та методах?**  
`base` дозволяє звертатися до членів базового класу. У конструкторах `base(...)` використовується для передачі параметрів у конструктор базового класу. У методах `base.MethodName()` викликає реалізацію методу з батьківського класу (наприклад, у `CalculateScholarship`).

**3. Яка різниця між virtual / override та new (приховування методів)?**  
`virtual` позначає метод, який *можна* перевизначити. `override` забезпечує поліморфний виклик (виконується версія найбільш похідного класу навіть через посилання на базовий). `new` просто приховує метод базового класу, розриваючи ланцюжок поліморфізму (через посилання на базовий клас виконається стара версія).

**4. В яких випадках краще використовувати абстрактний клас, а не інтерфейс?**  
Абстрактний клас використовується, коли між об'єктами є зв'язок "є" (is-a) і потрібно надати спільну базову реалізацію (поля, методи). Інтерфейс кращий, коли потрібно описати спільну поведінку ("здатний") для зовсім різних об'єктів або забезпечити множинну спадковість поведінки.

**5. Навіщо використовувати sealed для класів та методів?**  
`sealed` забороняє подальше успадкування класу (як у `GraduateStudent`) або перевизначення методу. Це підвищує безпеку (ніхто не змінить логіку фінального класу) та може дати невеликий приріст продуктивності (devirtualization).

**6. Як було реалізовано поліморфізм у системі управління студентами?**  
Поліморфізм реалізовано через перевизначення `GetInfo()` у підкласах `Student` та через інтерфейс `IUniversityMember`. Об'єкти різних типів зберігаються у `List<Person>`, але при виклику методів виконується специфічна логіка кожного типу.

**7. Яка перевага використання узагальнених методів (generics) у групі?**  
Generics (метод `GetMembersByType<T>`) дозволяють писати гнучкий та безпечний (type-safe) код. Ми можемо одним методом дістати зі списку `Person` тільки аспірантів або тільки відмінників без явного приведення типів та перевірок у кожному окремому випадку.

**8. Що таке принцип підстановки Лісков (Liskov Substitution Principle)?**  
Це принцип, згідно з яким об'єкти базового класу повинні мати можливість бути замінені об'єктами підкласів без порушення коректності роботи програми. У роботі це дотримано: будь-який підклас `Vehicle` або `Student` коректно працює через посилання базового типу.

**9. Як ви організували роботу з Git у цій практичній роботі?**  
Робота організована за моделлю Git Flow: основна гілка `main`, інтеграційна `develop` та окремі `feature/` гілки для кожної логічної частини (базова ієрархія, типи студентів, варіант 2, меню). Злиття проводилося з прапором `--no-ff` для збереження структури історії.

---

## 12. Git-log (фактичний)

```text
*   d106c1c (HEAD -> main, tag: v5.0, origin/main) merge: develop into main — PR5 complete (Variant 2: Vehicle hierarchy)
|\  
| *   c62950a (develop) merge: refactor/hierarchy-cleanup into develop
| |\  
| | * 1b27c8e (refactor/hierarchy-cleanup) refactor: final cleanup and PR5 REPORT update
| |/  
| *   4f1965e (feature/menu-inheritance) merge: feature/menu-inheritance into develop
| |\  
| | * 7e41ac7 feat: update Program.cs with PR5 menu items 25-30 + Vehicle polymorphism demo
| |/  
| *   177c514 (feature/variant-2) merge: feature/variant-2 into develop
| |\  
| | * e69906c feat(variant-2): add Vehicle hierarchy - abstract Vehicle, Car, Bus, Truck with polymorphism
| |/  
| *   2e267cd (feature/group-refactoring) merge: feature/group-refactoring into develop
| |\  
| | * 715c8a8 refactor: StudentGroup uses List<Person>, adds GetMembersByType<T> and GetTotalScholarship
| |/  
| *   c03a1b4 (feature/student-hierarchy) merge: feature/student-hierarchy into develop
| |\  
| | * 7e20b47 feat: add student hierarchy ExcellentStudent, ForeignStudent, WorkingStudent, GraduateStudent (sealed)
| |/  
| *   206c7cd (feature/inheritance-base) merge: feature/inheritance-base into develop
| |\  
| | * b15038b feat: add abstract Person base class and IUniversityMember interface; update Student to inherit Person
| |/  
| *   3b4ee9f Merge feature/abstract-universitymember into develop
| |\  
| | * 9266f69 Add UniversityMember abstract class
| |/  
| * 2147a5f Rename csproj to PR5
|/  
* f9b816a Initial commit from Practical 4
```
