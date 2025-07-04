using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ---------- Creational Patterns: Factory Method, Builder, Singleton (for NotificationService) ----------

// Factory Method: Abstract base class for users
public abstract class User
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }

    protected User(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    // Login: simulates user authentication (simplified)
    public virtual bool Login(string password)
    {
        Console.WriteLine($"Użytkownik {Name} zalogowany.");
        return true; // Simplified logic: always true
    }
}

// Concrete User type: Reader
public class Reader : User
{
    public Reader(int id, string name, string email) : base(id, name, email) { }
}

// Concrete User type: Librarian
public class Librarian : User
{
    public Librarian(int id, string name, string email) : base(id, name, email) { }
}

// Concrete User type: Admin
public class Admin : User
{
    public Admin(int id, string name, string email) : base(id, name, email) { }
}

// Factory Method: User factory interface
public interface IUserFactory
{
    User CreateUser(string role, int id, string name, string email);
}

// Factory Method: Concrete implementation of the user factory
public class ConcreteUserFactory : IUserFactory
{
    public User CreateUser(string role, int id, string name, string email)
    {
        switch (role)
        {
            case "Reader": return new Reader(id, name, email);
            case "Librarian": return new Librarian(id, name, email);
            case "Admin": return new Admin(id, name, email);
            default: throw new ArgumentException("Unknown user role.");
        }
    }
}

// Builder: Class representing an Email
public class Email
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> Attachments { get; } = new List<string>();

    public void Send()
    {
        Console.WriteLine($"Email sent. Subject: '{Subject}', Body: '{Body}', Attachments: {string.Join(", ", Attachments)}");
    }
}

// Builder: Class for building an Email
public class EmailBuilder
{
    private readonly Email _email = new Email();

    public EmailBuilder SetSubject(string subject)
    {
        _email.Subject = subject;
        return this;
    }

    public EmailBuilder SetBody(string body)
    {
        _email.Body = body;
        return this;
    }

    public EmailBuilder AddAttachment(string filePath)
    {
        _email.Attachments.Add(filePath);
        return this;
    }

    public Email Build()
    {
        // Add validation before returning (e.g., if subject and body are set)
        if (string.IsNullOrEmpty(_email.Subject) || string.IsNullOrEmpty(_email.Body))
        {
            throw new InvalidOperationException("Email must have a subject and body.");
        }
        return _email;
    }
}

// Singleton: Class for configuration management (example)
public sealed class ConfigurationManager
{
    private static ConfigurationManager _instance = null;
    private static readonly object _lock = new object();
    public string SystemName { get; private set; }

    private ConfigurationManager()
    {
        // Simulate configuration loading
        SystemName = "Intelligent Library System v1.0";
        Console.WriteLine($"[Singleton] Configuration loaded: {SystemName}");
    }

    public static ConfigurationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationManager();
                    }
                }
            }
            return _instance;
        }
    }

    public void DisplayConfig()
    {
        Console.WriteLine($"Current system configuration: {SystemName}");
    }
}


// Singleton: NotificationService (example implementation, based on Singleton concept)
// Note that in the diagram it is described as static, which is consistent with the Singleton idea.
public sealed class NotificationService // Use 'sealed' to prevent inheritance
{
    private static NotificationService _instance = null;
    private static readonly object _lock = new object();

    private NotificationService()
    {
        Console.WriteLine("Notification service initialized.");
    }

    public static NotificationService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new NotificationService();
                    }
                }
            }
            return _instance;
        }
    }

    public void SendEmail(string to, string subject, string body)
    {
        Console.WriteLine($"Email sent to {to}: Subject '{subject}', Body: '{body}'");
    }

    public void SendSms(string number, string message)
    {
        Console.WriteLine($"SMS sent to {number}: '{message}'");
    }
}


// ---------- Structural Patterns: Adapter, Composite ----------

// Adapter: Old API
public class OldBook
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int OldId { get; set; }
}

public class LegacyCatalogService
{
    public OldBook[] FindBooks(string q)
    {
        Console.WriteLine($"[LegacyCatalogService] Searching for books using old API: '{q}'");
        // Simulate returning old objects
        return new OldBook[] { new OldBook { OldId = 1, Title = "Pan Tadeusz", Author = "Adam Mickiewicz" },
                               new OldBook { OldId = 2, Title = "Lalka", Author = "Bolesław Prus" } };
    }
}

// Basic book model used in the new system
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool Available { get; private set; } = true; // Changed to private set
    public string ISBN { get; set; } // Added for sorting example

    public Book(int id, string title, string author, string isbn = null)
    {
        Id = id;
        Title = title;
        Author = author;
        ISBN = isbn;
    }

    public string GetDetails()
    {
        return $"[{Id}] {Title} - {Author} ({(Available ? "available" : "borrowed")})";
    }

    public void Borrow() => Available = false;
    public void Return() => Available = true;
}

// Adapter: New catalog interface
public interface ICatalog
{
    List<Book> Search(string query);
}

// Adapter: Adapter for LegacyCatalogService
public class LegacyCatalogAdapter : ICatalog
{
    private readonly LegacyCatalogService _legacy;

    public LegacyCatalogAdapter(LegacyCatalogService legacy)
    {
        _legacy = legacy;
    }

    public List<Book> Search(string query)
    {
        var oldBooks = _legacy.FindBooks(query);
        // Mapping OldBook -> Book
        return oldBooks.Select(ob => new Book(ob.OldId, ob.Title, ob.Author)).ToList();
    }
}

// Composite: Interface for category/book components
public interface ICategory
{
    void Display(int indent = 0);
}

// Composite: Leaf (Book)
public class BookLeaf : ICategory
{
    private readonly string _title;
    private readonly string _author;

    public BookLeaf(string title, string author)
    {
        _title = title;
        _author = author;
    }

    public void Display(int indent = 0)
    {
        Console.WriteLine(new string('-', indent) + $"Książka: {_title} (Autor: {_author})");
    }
}

// Composite: Composite (Category)
public class Category : ICategory
{
    private readonly string _name;
    private readonly List<ICategory> _children = new List<ICategory>();

    public Category(string name)
    {
        _name = name;
    }

    public void Add(ICategory component)
    {
        _children.Add(component);
    }

    public void Remove(ICategory component)
    {
        _children.Remove(component);
    }

    public void Display(int indent = 0)
    {
        Console.WriteLine(new string('-', indent) + $"Kategoria: {_name}");
        foreach (var component in _children)
        {
            component.Display(indent + 2);
        }
    }
}

// ---------- Behavioral Patterns: Observer, Strategy, State (for LoanTransaction) ----------

// Observer: Observer interface
public interface IObserver
{
    void Update(BookSubject subject);
}

// Observer: Subject (Observable)
public class BookSubject
{
    private readonly List<IObserver> _observers = new List<IObserver>();
    private bool _isAvailable;
    public string Title { get; }

    public BookSubject(string title) => Title = title;

    public void Attach(IObserver obs) => _observers.Add(obs);
    public void Detach(IObserver obs) => _observers.Remove(obs);

    public bool IsAvailable
    {
        get => _isAvailable;
        set
        {
            if (_isAvailable != value) // Notify only on state change
            {
                _isAvailable = value;
                if (_isAvailable) Notify();
            }
        }
    }

    public void Notify()
    {
        Console.WriteLine($"[BookSubject] Książka '{Title}' zmieniła dostępność. Powiadamiam obserwatorów.");
        foreach (var obs in _observers.ToList()) // Use ToList() to avoid collection modification during iteration
        {
            obs.Update(this);
        }
    }
}

// Observer: Concrete Observer (Reader)
public class ReaderObserver : IObserver
{
    public string Name { get; }

    public ReaderObserver(string name) => Name = name;

    public void Update(BookSubject subject)
    {
        Console.WriteLine($"[ReaderObserver] {Name}: Książka '{subject.Title}' jest dostępna!");
    }
}

// Strategy: Sorting strategy interface
public interface ISortStrategy
{
    List<Book> Sort(List<Book> books);
}

// Strategy: Concrete strategy - Sort by Title
public class SortByTitle : ISortStrategy
{
    public List<Book> Sort(List<Book> books)
    {
        Console.WriteLine("[Strategy] Sortowanie książek po tytule.");
        return books.OrderBy(b => b.Title).ToList();
    }
}

// Strategy: Concrete strategy - Sort by Author
public class SortByAuthor : ISortStrategy
{
    public List<Book> Sort(List<Book> books)
    {
        Console.WriteLine("[Strategy] Sortowanie książek po autorze.");
        return books.OrderBy(b => b.Author).ToList();
    }
}

// Context class for the strategy (e.g., Catalog)
public class Catalog
{
    private readonly List<Book> _books = new List<Book>();
    private ISortStrategy _sortStrategy;

    public Catalog() { }

    public void AddBook(Book book) => _books.Add(book);

    public List<Book> SearchByTitle(string query)
    {
        return _books.Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void SetSortStrategy(ISortStrategy strategy)
    {
        _sortStrategy = strategy;
    }

    public List<Book> GetSortedBooks()
    {
        if (_sortStrategy == null)
        {
            Console.WriteLine("[Catalog] Brak strategii sortowania. Zwracam niesortowaną listę.");
            return _books.ToList();
        }
        return _sortStrategy.Sort(_books.ToList()); // Create a copy of the list for sorting
    }

    // Other Catalog methods (editBook, removeBook)
    public void EditBook(Book updatedBook)
    {
        var existingBook = _books.FirstOrDefault(b => b.Id == updatedBook.Id);
        if (existingBook != null)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.ISBN = updatedBook.ISBN;
            // Availability is changed by Borrow/Return in Book, not directly here
            Console.WriteLine($"Zaktualizowano książkę: {updatedBook.GetDetails()}");
        }
        else
        {
            Console.WriteLine($"Błąd: Nie znaleziono książki o ID {updatedBook.Id} do edycji.");
        }
    }

    public void RemoveBook(int id)
    {
        var bookToRemove = _books.FirstOrDefault(b => b.Id == id);
        if (bookToRemove != null)
        {
            _books.Remove(bookToRemove);
            Console.WriteLine($"Usunięto książkę o ID: {id}");
        }
        else
        {
            Console.WriteLine($"Błąd: Nie znaleziono książki o ID {id} do usunięcia.");
        }
    }

    public Book GetBookById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }
}

// State: Enumeration of transaction states
public enum TransactionState
{
    Available,
    Reserved,
    Borrowed,
    Overdue,
    ReturnInProgress,
    Completed,
    Lost
}

// State: Class managing transitions between states based on events
public class LoanTransaction
{
    public TransactionState State { get; private set; }
    public DateTime? BorrowDate { get; private set; }
    public DateTime? DueDate { get; private set; }
    public Book Book { get; private set; } // Added reference to the book

    // Constructor: initializes initial state and runs entry/do actions
    public LoanTransaction(Book book)
    {
        Book = book;
        State = TransactionState.Available;
        UpdateAvailabilityStatus();
        MonitorReservations();
    }

    // Events changing state
    public void Reserve()
    {
        if (State != TransactionState.Available)
            throw new InvalidOperationException("Reservation is only possible from the Available state.");

        DisableAvailability();
        AssignReservation();
        State = TransactionState.Reserved;
        WaitForPickup();
        Console.WriteLine($"Książka '{Book.Title}' zarezerwowana.");
    }

    public void CancelReservation()
    {
        if (State != TransactionState.Reserved)
            throw new InvalidOperationException("Cancellation is only possible from the Reserved state.");

        ClearReservationTimer();
        State = TransactionState.Available;
        Console.WriteLine($"Rezerwacja książki '{Book.Title}' anulowana.");
    }

    public void Borrow()
    {
        if (State != TransactionState.Reserved && State != TransactionState.Available) // Allow borrowing directly from available
            throw new InvalidOperationException("Borrowing is only possible from reserved or available state.");

        State = TransactionState.Borrowed;
        BorrowDate = DateTime.Now;
        DueDate = DateTime.Now.AddDays(14); // Example: 14 days
        Book.Borrow(); // Update book's availability status
        RegisterBorrowDate();
        MonitorBorrowPeriod();
        RegisterReturnIntention();
        Console.WriteLine($"Książka '{Book.Title}' wypożyczona. Termin zwrotu: {DueDate?.ToShortDateString()}");
    }

    public void DueDateElapsed()
    {
        if (State != TransactionState.Borrowed)
            return; // Don't throw exception if already overdue or other state

        State = TransactionState.Overdue;
        ApplyOverdueFine();
        SendOverdueNotifications();
        Console.WriteLine($"Książka '{Book.Title}' przeterminowana.");
    }

    public void StartReturn()
    {
        if (State != TransactionState.Borrowed && State != TransactionState.Overdue)
            throw new InvalidOperationException("Return is only possible for borrowed or overdue books.");

        State = TransactionState.ReturnInProgress;
        CheckBookCondition();
        ProcessReturn();
        Console.WriteLine($"Rozpoczęto zwrot książki '{Book.Title}'.");
    }

    public void CompleteReturn()
    {
        if (State != TransactionState.ReturnInProgress)
            throw new InvalidOperationException("Completing return is only possible while return is in progress.");

        Book.Return(); // Book becomes available
        State = TransactionState.Completed;
        CompleteReturnTransaction();
        UpdateInventory();
        Console.WriteLine($"Zwrot książki '{Book.Title}' zakończony. Książka ponownie dostępna.");
    }

    public void ReportLost()
    {
        if (State != TransactionState.Borrowed && State != TransactionState.Overdue)
            throw new InvalidOperationException("Reporting lost is only possible for borrowed or overdue books.");

        State = TransactionState.Lost;
        MarkAsLost();
        ApplyLossFee();
        Console.WriteLine($"Książka '{Book.Title}' zgłoszona jako utracona.");
    }

    // Private methods simulating ENTRY/DO/EXIT actions
    private void UpdateAvailabilityStatus() => Console.WriteLine("  [Akcja] aktualizowano status dostępności");
    private void MonitorReservations() => Console.WriteLine("  [Akcja] monitorowanie rezerwacji");
    private void DisableAvailability() => Console.WriteLine("  [Akcja] wyłączono dostępność");
    private void AssignReservation() => Console.WriteLine("  [Akcja] przypisano rezerwację");
    private void WaitForPickup() => Console.WriteLine("  [Akcja] oczekiwanie na odbiór książki");
    private void ClearReservationTimer() => Console.WriteLine("  [Akcja] wyczyszczono timer rezerwacji");
    private void RegisterBorrowDate() => Console.WriteLine("  [Akcja] zarejestrowano datę wypożyczenia");
    private void MonitorBorrowPeriod() => Console.WriteLine("  [Akcja] monitorowanie okresu wypożyczenia");
    private void RegisterReturnIntention() => Console.WriteLine("  [Akcja] zarejestrowano intencje zwrotu");
    private void ApplyOverdueFine() => Console.WriteLine("  [Akcja] nałożono karę za przeterminowanie");
    private void SendOverdueNotifications() => Console.WriteLine("  [Akcja] wysłano powiadomienie o przeterminowaniu");
    private void CheckBookCondition() => Console.WriteLine("  [Akcja] sprawdzono stan książki");
    private void ProcessReturn() => Console.WriteLine("  [Akcja] przetworzono zwrot");
    private void CompleteReturnTransaction() => Console.WriteLine("  [Akcja] zakończono transakcję zwrotu");
    private void UpdateInventory() => Console.WriteLine("  [Akcja] zaktualizowano inwentarz");
    private void MarkAsLost() => Console.WriteLine("  [Akcja] oznaczono książkę jako utraconą");
    private void ApplyLossFee() => Console.WriteLine("  [Akcja] nałożono opłatę za utracenie");
}

// BorrowingService class (moved and modified to use states and asynchronous operations)
public class BorrowingService
{
    private readonly Catalog _catalog;
    private readonly object _sync = new object(); // Object for synchronization

    public BorrowingService(Catalog catalog)
    {
        _catalog = catalog;
    }

    // ProcessBorrowRequest: verifies session, retrieves book, locks access, borrows and initiates save/index
    public void ProcessBorrowRequest(UserSession session, int bookId)
    {
        if (!session.IsValid)
        {
            Console.WriteLine("Błąd: Niepoprawna sesja użytkownika.");
            return;
        }

        Book book;
        LoanTransaction transaction = null; // Variable to store the transaction

        lock (_sync) // Lock access to the catalog
        {
            book = _catalog.GetBookById(bookId);
            if (book == null)
            {
                Console.WriteLine("Błąd: Nie znaleziono książki.");
                return;
            }
            if (!book.Available)
            {
                Console.WriteLine("Błąd: Książka niedostępna.");
                return;
            }

            // Create a new transaction or retrieve an existing one if it's a reservation
            // For simplicity, we assume a new loan transaction is created.
            transaction = new LoanTransaction(book);
            transaction.Borrow(); // Change state to Borrowed
        }

        // Run tasks asynchronously
        var saveTask = Task.Run(() => SaveTransactionToDatabase(session.User, book));
        var updateIndexTask = Task.Run(() => UpdateSearchIndex(book));

        Task.WaitAll(saveTask, updateIndexTask); // Wait for both tasks to complete

        Console.WriteLine($"Potwierdzenie: Książka '{book.Title}' wypożyczona przez {session.User.Name}.");
    }

    // SaveTransactionToDatabase: simulates saving a transaction to the database
    private void SaveTransactionToDatabase(User user, Book book)
    {
        Thread.Sleep(300); // Simulate delay
        Console.WriteLine($"Zapisano transakcję wypożyczenia książki '{book.Title}' dla użytkownika {user.Name}.");
    }

    // UpdateSearchIndex: simulates refreshing the search index
    private void UpdateSearchIndex(Book book)
    {
        Thread.Sleep(200); // Simulate delay
        Console.WriteLine($"Zaktualizowano indeks wyszukiwania dla '{book.Title}'.");
    }

    public void ReturnBook(Reader reader, int bookId)
    {
        var book = _catalog.GetBookById(bookId);
        if (book != null && !book.Available) // Check if book exists and is borrowed
        {
            // In a real system, we would look for a specific transaction instance for this book and reader
            // Here we simulate finding the appropriate transaction and changing its state.
            var tempTransaction = new LoanTransaction(book); // Create a temporary transaction to use state logic
            // Simulate transition to ReturnInProgress, then to Completed
            tempTransaction.StartReturn();
            tempTransaction.CompleteReturn();
            Console.WriteLine($"{reader.Name} zwrócił: {book.GetDetails()}");
        }
        else
        {
            Console.WriteLine("Nie znaleziono wypożyczenia o podanym ID lub książka nie jest wypożyczona.");
        }
    }
}

// UserSession class (moved from previous documents)
public class UserSession
{
    public User User { get; }
    public bool IsValid => true; // Simplification: always true

    public UserSession(User user)
    {
        User = user;
    }
}


class Program
{
    private static Catalog _libraryCatalog = new Catalog();
    private static IUserFactory _userFactory = new ConcreteUserFactory();
    private static BorrowingService _borrowingService = new BorrowingService(_libraryCatalog);
    private static User _loggedInUser = null; // Zalogowany użytkownik

    static void Main(string[] args)
    {
        InitializeLibraryData();

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("--- Inteligentny System Zarządzania Biblioteką ---");
            Console.WriteLine("1. Logowanie");
            Console.WriteLine("2. Zarejestruj się (jako czytelnik)");
            Console.WriteLine("3. Przeglądaj katalog");
            Console.WriteLine("4. Wyszukaj książkę");
            Console.WriteLine("5. Wypożycz książkę");
            Console.WriteLine("6. Zwróć książkę");
            Console.WriteLine("7. Zarządzaj książkami (tylko Bibliotekarz)");
            Console.WriteLine("8. Wyświetl hierarchię kategorii");
            Console.WriteLine("9. Zarządzaj powiadomieniami (demo)");
            Console.WriteLine("0. Wyjście");
            Console.Write("Wybierz opcję: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    LoginUser();
                    break;
                case "2":
                    RegisterReader();
                    break;
                case "3":
                    BrowseCatalog();
                    break;
                case "4":
                    SearchBook();
                    break;
                case "5":
                    BorrowBook();
                    break;
                case "6":
                    ReturnBook();
                    break;
                case "7":
                    ManageBooks();
                    break;
                case "8":
                    DisplayCategoryHierarchy();
                    break;
                case "9":
                    ManageNotifications();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    break;
            }
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }
        Console.WriteLine("Dziękujemy za skorzystanie z systemu bibliotecznego!");
    }

    static void InitializeLibraryData()
    {
        _libraryCatalog.AddBook(new Book(101, "Władca Pierścieni", "J.R.R. Tolkien", "978-0618053267"));
        _libraryCatalog.AddBook(new Book(102, "Solaris", "Stanisław Lem", "978-8308064119"));
        _libraryCatalog.AddBook(new Book(103, "Dziady", "Adam Mickiewicz", "978-8308049215"));
        _libraryCatalog.AddBook(new Book(104, "Lalka", "Bolesław Prus", "978-8380084568"));
        _libraryCatalog.AddBook(new Book(105, "Pan Tadeusz", "Adam Mickiewicz", "978-8308064119"));
    }

    static void LoginUser()
    {
        Console.WriteLine("\n--- Logowanie ---");
        Console.Write("Podaj nazwę użytkownika: ");
        string name = Console.ReadLine();
        Console.Write("Podaj hasło (dowolne, symulacja): ");
        string password = Console.ReadLine();

        // W rzeczywistości tutaj byłaby weryfikacja danych logowania z bazy
        // Dla uproszczenia, sprawdzamy tylko nazwę i tworzymy tymczasowego użytkownika
        if (name.ToLower() == "admin")
        {
            _loggedInUser = _userFactory.CreateUser("Admin", 100, name, $"{name}@lib.com");
        }
        else if (name.ToLower() == "librarian")
        {
            _loggedInUser = _userFactory.CreateUser("Librarian", 200, name, $"{name}@lib.com");
        }
        else
        {
            _loggedInUser = _userFactory.CreateUser("Reader", 300, name, $"{name}@lib.com");
        }

        if (_loggedInUser.Login(password))
        {
            Console.WriteLine($"Zalogowano jako: {_loggedInUser.Name} ({_loggedInUser.GetType().Name})");
        }
        else
        {
            Console.WriteLine("Błąd logowania.");
            _loggedInUser = null;
        }
    }

    static void RegisterReader()
    {
        Console.WriteLine("\n--- Rejestracja Czytelnika ---");
        Console.Write("Podaj nazwę użytkownika: ");
        string name = Console.ReadLine();
        Console.Write("Podaj email: ");
        string email = Console.ReadLine();
        // W realnym systemie byłyby też pola na hasło i jego potwierdzenie

        // Generowanie prostego ID (w realnym systemie z bazy danych)
        Random rnd = new Random();
        int newId = rnd.Next(1000, 9999);

        try
        {
            User newReader = _userFactory.CreateUser("Reader", newId, name, email);
            Console.WriteLine($"Czytelnik '{newReader.Name}' zarejestrowany pomyślnie z ID: {newReader.Id}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Błąd rejestracji: {ex.Message}");
        }
    }

    static void BrowseCatalog()
    {
        Console.WriteLine("\n--- Przeglądanie Katalogu ---");
        Console.WriteLine("Wybierz metodę sortowania:");
        Console.WriteLine("1. Po tytule");
        Console.WriteLine("2. Po autorze");
        Console.WriteLine("3. Bez sortowania");
        Console.Write("Wybierz opcję: ");
        string sortChoice = Console.ReadLine();

        List<Book> booksToDisplay = null;
        switch (sortChoice)
        {
            case "1":
                _libraryCatalog.SetSortStrategy(new SortByTitle());
                booksToDisplay = _libraryCatalog.GetSortedBooks();
                break;
            case "2":
                _libraryCatalog.SetSortStrategy(new SortByAuthor());
                booksToDisplay = _libraryCatalog.GetSortedBooks();
                break;
            case "3":
            default:
                // Aby pobrać wszystkie książki bez aktywnej strategii, musimy dodać metodę do Catalogu
                // Na potrzeby demo, SearchByTitle("") zwróci wszystkie
                booksToDisplay = _libraryCatalog.SearchByTitle(""); 
                Console.WriteLine("[Catalog] Wyświetlam książki bez sortowania.");
                break;
        }

        if (booksToDisplay.Any())
        {
            foreach (var book in booksToDisplay)
            {
                Console.WriteLine($"- {book.GetDetails()}");
            }
        }
        else
        {
            Console.WriteLine("Katalog jest pusty.");
        }
    }

    static void SearchBook()
    {
        Console.WriteLine("\n--- Wyszukiwanie Książki ---");
        Console.Write("Podaj fragment tytułu do wyszukania: ");
        string query = Console.ReadLine();

        var results = _libraryCatalog.SearchByTitle(query);

        if (results.Any())
        {
            Console.WriteLine("Znalezione książki:");
            foreach (var book in results)
            {
                Console.WriteLine($"- {book.GetDetails()}");
            }
        }
        else
        {
            Console.WriteLine("Brak książek pasujących do zapytania.");
        }
    }

    static void BorrowBook()
    {
        if (_loggedInUser == null)
        {
            Console.WriteLine("Musisz być zalogowany, aby wypożyczyć książkę.");
            return;
        }

        if (!(_loggedInUser is Reader))
        {
            Console.WriteLine("Tylko czytelnicy mogą wypożyczać książki.");
            return;
        }

        Console.WriteLine("\n--- Wypożyczanie Książki ---");
        Console.Write("Podaj ID książki do wypożyczenia: ");
        if (int.TryParse(Console.ReadLine(), out int bookId))
        {
            UserSession currentSession = new UserSession(_loggedInUser);
            _borrowingService.ProcessBorrowRequest(currentSession, bookId);
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID książki.");
        }
    }

    static void ReturnBook()
    {
        if (_loggedInUser == null)
        {
            Console.WriteLine("Musisz być zalogowany, aby zwrócić książkę.");
            return;
        }
        if (!(_loggedInUser is Reader))
        {
            Console.WriteLine("Tylko czytelnicy mogą zwracać książki.");
            return;
        }

        Console.WriteLine("\n--- Zwracanie Książki ---");
        Console.Write("Podaj ID książki do zwrotu: ");
        if (int.TryParse(Console.ReadLine(), out int bookId))
        {
            _borrowingService.ReturnBook((Reader)_loggedInUser, bookId);
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID książki.");
        }
    }

    static void ManageBooks()
    {
        if (_loggedInUser == null || !(_loggedInUser is Librarian))
        {
            Console.WriteLine("Tylko Bibliotekarz może zarządzać książkami.");
            return;
        }

        Console.WriteLine("\n--- Zarządzanie Książkami (Bibliotekarz) ---");
        Console.WriteLine("1. Dodaj książkę");
        Console.WriteLine("2. Edytuj książkę");
        Console.WriteLine("3. Usuń książkę");
        Console.Write("Wybierz opcję: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddBook();
                break;
            case "2":
                EditBook();
                break;
            case "3":
                RemoveBook();
                break;
            default:
                Console.WriteLine("Nieprawidłowa opcja.");
                break;
        }
    }

    static void AddBook()
    {
        Console.Write("Podaj ID nowej książki: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Nieprawidłowe ID."); return; }
        Console.Write("Podaj tytuł: ");
        string title = Console.ReadLine();
        Console.Write("Podaj autora: ");
        string author = Console.ReadLine();
        Console.Write("Podaj ISBN (opcjonalnie): ");
        string isbn = Console.ReadLine();

        _libraryCatalog.AddBook(new Book(id, title, author, isbn));
        Console.WriteLine($"Książka '{title}' dodana pomyślnie.");
    }

    static void EditBook()
    {
        Console.Write("Podaj ID książki do edycji: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Nieprawidłowe ID."); return; }

        var existingBook = _libraryCatalog.GetBookById(id);
        if (existingBook == null)
        {
            Console.WriteLine("Książka o podanym ID nie istnieje.");
            return;
        }

        Console.WriteLine($"Edytujesz książkę: {existingBook.GetDetails()}");
        Console.Write("Podaj nowy tytuł (pozostaw puste, aby nie zmieniać): ");
        string newTitle = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newTitle)) existingBook.Title = newTitle;

        Console.Write("Podaj nowego autora (pozostaw puste, aby nie zmieniać): ");
        string newAuthor = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newAuthor)) existingBook.Author = newAuthor;

        Console.Write("Podaj nowy ISBN (pozostaw puste, aby nie zmieniać): ");
        string newIsbn = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newIsbn)) existingBook.ISBN = newIsbn;

        _libraryCatalog.EditBook(existingBook); // Upewnij się, że EditBook poprawnie działa na istniejącym obiekcie
        Console.WriteLine("Książka zaktualizowana.");
    }

    static void RemoveBook()
    {
        Console.Write("Podaj ID książki do usunięcia: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _libraryCatalog.RemoveBook(id);
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID.");
        }
    }

    static void DisplayCategoryHierarchy()
    {
        Console.WriteLine("\n--- Hierarchia Kategorii (Demo Composite) ---");
        Category fiction = new Category("Literatura Piękna");
        fiction.Add(new BookLeaf("1984", "George Orwell"));
        fiction.Add(new BookLeaf("Zbrodnia i Kara", "Fiodor Dostojewski"));

        Category fantasy = new Category("Fantasy");
        fantasy.Add(new BookLeaf("Hobbit", "J.R.R. Tolkien"));
        fantasy.Add(new BookLeaf("Wiedźmin: Ostatnie życzenie", "Andrzej Sapkowski"));

        Category mainCategory = new Category("Główna Kategoria Książek");
        mainCategory.Add(fiction);
        mainCategory.Add(fantasy);
        mainCategory.Add(new BookLeaf("Programowanie w C#", "Jan Kowalski"));

        mainCategory.Display();
    }

    static void ManageNotifications()
    {
        Console.WriteLine("\n--- Zarządzanie Powiadomieniami (Demo Singleton/Observer) ---");
        Console.WriteLine("1. Wyślij testowy e-mail (Singleton Builder)");
        Console.WriteLine("2. Zarejestruj się na powiadomienia o dostępności 'Lalki' (Observer)");
        Console.WriteLine("3. Zmień dostępność 'Lalki' (Observer trigger)");
        Console.Write("Wybierz opcję: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                // Demo Builder i Singleton NotificationService
                EmailBuilder emailBuilder = new EmailBuilder();
                Email testEmail = emailBuilder
                    .SetSubject("Testowy email z biblioteki")
                    .SetBody("To jest testowa wiadomość wysłana za pomocą wzorca Builder i Singleton.")
                    .Build();
                // Wysyłanie przez Singleton
                NotificationService.Instance.SendEmail("user@example.com", testEmail.Subject, testEmail.Body);
                break;
            case "2":
                // Demo Observer - Attach
                if (_loggedInUser is Reader loggedReader)
                {
                    BookSubject lalkaSubject = new BookSubject("Lalka"); // Zakładamy istnienie Lalki
                    ReaderObserver observer = new ReaderObserver(loggedReader.Name);
                    lalkaSubject.Attach(observer);
                    Console.WriteLine($"{loggedReader.Name} subskrybuje powiadomienia o dostępności 'Lalki'.");
                    // UWAGA: W realnej aplikacji, BookSubject 'Lalka' byłby zarządzany centralnie
                    // i subskrypcje byłyby trwałe. To jest tylko demo mechanizmu.
                }
                else
                {
                    Console.WriteLine("Tylko zalogowany czytelnik może subskrybować powiadomienia.");
                }
                break;
            case "3":
                // Demo Observer - Trigger
                Console.WriteLine("Symuluję zmianę dostępności 'Lalki'...");
                BookSubject lalkaTrigger = new BookSubject("Lalka");
                lalkaTrigger.IsAvailable = false; // Najpierw niedostępna
                lalkaTrigger.IsAvailable = true;  // Potem dostępna - wywoła powiadomienie
                break;
            default:
                Console.WriteLine("Nieprawidłowa opcja.");
                break;
        }
    }
}
