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
        Console.WriteLine($"User {Name} logged in.");
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
        Console.WriteLine(new string('-', indent) + $"Book: {_title} (Author: {_author})");
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
        Console.WriteLine(new string('-', indent) + $"Category: {_name}");
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
        Console.WriteLine($"[BookSubject] Book '{Title}' changed availability. Notifying observers.");
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
        Console.WriteLine($"[ReaderObserver] {Name}: Book '{subject.Title}' is now available!");
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
        Console.WriteLine("[Strategy] Sorting books by title.");
        return books.OrderBy(b => b.Title).ToList();
    }
}

// Strategy: Concrete strategy - Sort by Author
public class SortByAuthor : ISortStrategy
{
    public List<Book> Sort(List<Book> books)
    {
        Console.WriteLine("[Strategy] Sorting books by author.");
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
            Console.WriteLine("[Catalog] No sorting strategy set. Returning unsorted list.");
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
            Console.WriteLine($"Book updated: {updatedBook.GetDetails()}");
        }
        else
        {
            Console.WriteLine($"Error: Book with ID {updatedBook.Id} not found for editing.");
        }
    }

    public void RemoveBook(int id)
    {
        var bookToRemove = _books.FirstOrDefault(b => b.Id == id);
        if (bookToRemove != null)
        {
            _books.Remove(bookToRemove);
            Console.WriteLine($"Book with ID: {id} removed.");
        }
        else
        {
            Console.WriteLine($"Error: Book with ID {id} not found for removal.");
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
        Console.WriteLine($"Book '{Book.Title}' reserved.");
    }

    public void CancelReservation()
    {
        if (State != TransactionState.Reserved)
            throw new InvalidOperationException("Cancellation is only possible from the Reserved state.");

        ClearReservationTimer();
        State = TransactionState.Available;
        Console.WriteLine($"Reservation for book '{Book.Title}' canceled.");
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
        Console.WriteLine($"Book '{Book.Title}' borrowed. Due date: {DueDate?.ToShortDateString()}");
    }

    public void DueDateElapsed()
    {
        if (State != TransactionState.Borrowed)
            return; // Don't throw exception if already overdue or other state

        State = TransactionState.Overdue;
        ApplyOverdueFine();
        SendOverdueNotifications();
        Console.WriteLine($"Book '{Book.Title}' is overdue.");
    }

    public void StartReturn()
    {
        if (State != TransactionState.Borrowed && State != TransactionState.Overdue)
            throw new InvalidOperationException("Return is only possible for borrowed or overdue books.");

        State = TransactionState.ReturnInProgress;
        CheckBookCondition();
        ProcessReturn();
        Console.WriteLine($"Return of book '{Book.Title}' started.");
    }

    public void CompleteReturn()
    {
        if (State != TransactionState.ReturnInProgress)
            throw new InvalidOperationException("Completing return is only possible while return is in progress.");

        Book.Return(); // Book becomes available
        State = TransactionState.Completed;
        CompleteReturnTransaction();
        UpdateInventory();
        Console.WriteLine($"Return of book '{Book.Title}' completed. Book is available again.");
    }

    public void ReportLost()
    {
        if (State != TransactionState.Borrowed && State != TransactionState.Overdue)
            throw new InvalidOperationException("Reporting lost is only possible for borrowed or overdue books.");

        State = TransactionState.Lost;
        MarkAsLost();
        ApplyLossFee();
        Console.WriteLine($"Book '{Book.Title}' reported as lost.");
    }

    // Private methods simulating ENTRY/DO/EXIT actions
    private void UpdateAvailabilityStatus() => Console.WriteLine("  [Action] availability status updated");
    private void MonitorReservations() => Console.WriteLine("  [Action] monitoring reservations");
    private void DisableAvailability() => Console.WriteLine("  [Action] availability disabled");
    private void AssignReservation() => Console.WriteLine("  [Action] reservation assigned");
    private void WaitForPickup() => Console.WriteLine("  [Action] waiting for book pickup");
    private void ClearReservationTimer() => Console.WriteLine("  [Action] reservation timer cleared");
    private void RegisterBorrowDate() => Console.WriteLine("  [Action] borrow date registered");
    private void MonitorBorrowPeriod() => Console.WriteLine("  [Action] monitoring borrow period");
    private void RegisterReturnIntention() => Console.WriteLine("  [Action] return intention registered");
    private void ApplyOverdueFine() => Console.WriteLine("  [Action] overdue fine applied");
    private void SendOverdueNotifications() => Console.WriteLine("  [Action] overdue notification sent");
    private void CheckBookCondition() => Console.WriteLine("  [Action] book condition checked");
    private void ProcessReturn() => Console.WriteLine("  [Action] return processed");
    private void CompleteReturnTransaction() => Console.WriteLine("  [Action] return transaction completed");
    private void UpdateInventory() => Console.WriteLine("  [Action] inventory updated");
    private void MarkAsLost() => Console.WriteLine("  [Action] book marked as lost");
    private void ApplyLossFee() => Console.WriteLine("  [Action] loss fee applied");
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
            Console.WriteLine("Error: Invalid user session.");
            return;
        }

        Book book;
        LoanTransaction transaction = null; // Variable to store the transaction

        lock (_sync) // Lock access to the catalog
        {
            book = _catalog.GetBookById(bookId);
            if (book == null)
            {
                Console.WriteLine("Error: Book not found.");
                return;
            }
            if (!book.Available)
            {
                Console.WriteLine("Error: Book is not available.");
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

        Console.WriteLine($"Confirmation: Book '{book.Title}' borrowed by {session.User.Name}.");
    }

    // SaveTransactionToDatabase: simulates saving a transaction to the database
    private void SaveTransactionToDatabase(User user, Book book)
    {
        Thread.Sleep(300); // Simulate delay
        Console.WriteLine($"Saved loan transaction for book '{book.Title}' for user {user.Name}.");
    }

    // UpdateSearchIndex: simulates refreshing the search index
    private void UpdateSearchIndex(Book book)
    {
        Thread.Sleep(200); // Simulate delay
        Console.WriteLine($"Updated search index for '{book.Title}'.");
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
            Console.WriteLine($"{reader.Name} returned: {book.GetDetails()}");
        }
        else
        {
            Console.WriteLine("Loan not found for the given ID or book is not borrowed.");
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
    static void Main(string[] args)
    {
        Console.WriteLine("--- Demonstracja wzorców projektowych i logiki systemu ---");

        // 1. Singleton (ConfigurationManager)
        Console.WriteLine("\n=== Singleton (ConfigurationManager) ===");
        var config1 = ConfigurationManager.Instance;
        var config2 = ConfigurationManager.Instance;
        Console.WriteLine($"Czy instancje są takie same? {ReferenceEquals(config1, config2)}");
        config1.DisplayConfig();

        // 2. Factory Method
        Console.WriteLine("\n=== Factory Method ===");
        IUserFactory userFactory = new ConcreteUserFactory();
        User reader = userFactory.CreateUser("Reader", 1, "Alice Smith", "alice@example.com");
        User librarian = userFactory.CreateUser("Librarian", 2, "Bob Johnson", "bob@example.com");
        User admin = userFactory.CreateUser("Admin", 3, "Charlie Brown", "charlie@example.com");

        reader.Login("password123");
        librarian.Login("securepass");
        admin.Login("adminpass");

        // 3. Builder
        Console.WriteLine("\n=== Builder ===");
        EmailBuilder emailBuilder = new EmailBuilder();
        Email welcomeEmail = emailBuilder
            .SetSubject("Witaj w Bibliotece!")
            .SetBody("Dziękujemy za rejestrację w naszym systemie.")
            .Build();
        welcomeEmail.Send();

        Email notificationEmail = emailBuilder
            .SetSubject("Przypomnienie o zwrocie")
            .SetBody("Twoja książka 'W pustyni i w puszczy' jest przeterminowana.")
            .AddAttachment("regulamin.pdf")
            .Build();
        notificationEmail.Send();

        // 4. Adapter
        Console.WriteLine("\n=== Adapter ===");
        LegacyCatalogService legacyService = new LegacyCatalogService();
        ICatalog adaptedCatalog = new LegacyCatalogAdapter(legacyService);
        var booksFromLegacy = adaptedCatalog.Search("Pan");
        Console.WriteLine("Książki z zaadaptowanego katalogu:");
        foreach (var b in booksFromLegacy)
        {
            Console.WriteLine($"- {b.GetDetails()}");
        }

        // 5. Composite
        Console.WriteLine("\n=== Composite ===");
        Category fiction = new Category("Literatura Piękna");
        fiction.Add(new BookLeaf("1984", "George Orwell"));
        fiction.Add(new BookLeaf("Zbrodnia i Kara", "Fiodor Dostojewski"));

        Category fantasy = new Category("Fantasy");
        fantasy.Add(new BookLeaf("Hobbit", "J.R.R. Tolkien"));
        fantasy.Add(new BookLeaf("Wiedźmin: Ostatnie życzenie", "Andrzej Sapkowski"));

        Category mainCategory = new Category("Główna Kategoria Książek");
        mainCategory.Add(fiction);
        mainCategory.Add(fantasy);
        mainCategory.Add(new BookLeaf("Programowanie w C#", "Jan Kowalski")); // Book directly in main category

        mainCategory.Display();

        // 6. Observer
        Console.WriteLine("\n=== Observer ===");
        BookSubject lalkaSubject = new BookSubject("Lalka");
        ReaderObserver aliceReader = new ReaderObserver("Alice");
        ReaderObserver bobReader = new ReaderObserver("Bob");

        lalkaSubject.Attach(aliceReader);
        lalkaSubject.Attach(bobReader);

        lalkaSubject.IsAvailable = false; // Book borrowed, no notification
        lalkaSubject.IsAvailable = true;  // Book returned, notify
        lalkaSubject.IsAvailable = true;  // No change, no re-notification

        lalkaSubject.Detach(bobReader);
        lalkaSubject.IsAvailable = false;
        lalkaSubject.IsAvailable = true; // Only Alice will receive notification

        // 7. Strategy
        Console.WriteLine("\n=== Strategy ===");
        Catalog libraryCatalog = new Catalog();
        libraryCatalog.AddBook(new Book(101, "Władca Pierścieni", "J.R.R. Tolkien", "978-0618053267"));
        libraryCatalog.AddBook(new Book(102, "Solaris", "Stanisław Lem", "978-8308064119"));
        libraryCatalog.AddBook(new Book(103, "Dziady", "Adam Mickiewicz", "978-8308049215"));

        Console.WriteLine("\nKsiążki bez sortowania:");
        foreach (var book in libraryCatalog.GetSortedBooks())
        {
            Console.WriteLine($"- {book.Title} by {book.Author}");
        }

        libraryCatalog.SetSortStrategy(new SortByTitle());
        Console.WriteLine("\nKsiążki posortowane po tytule:");
        foreach (var book in libraryCatalog.GetSortedBooks())
        {
            Console.WriteLine($"- {book.Title} by {book.Author}");
        }

        libraryCatalog.SetSortStrategy(new SortByAuthor());
        Console.WriteLine("\nKsiążki posortowane po autorze:");
        foreach (var book in libraryCatalog.GetSortedBooks())
        {
            Console.WriteLine($"- {book.Title} by {book.Author}");
        }

        // 8. State (for LoanTransaction) and BorrowingService
        Console.WriteLine("\n=== State (LoanTransaction) i BorrowingService ===");
        Book testBook = new Book(201, "W pustyni i w puszczy", "Henryk Sienkiewicz");
        libraryCatalog.AddBook(testBook); // Add book to catalog

        BorrowingService borrowingService = new BorrowingService(libraryCatalog);
        Reader currentReader = (Reader)reader; // Use previously created reader
        UserSession readerSession = new UserSession(currentReader);

        Console.WriteLine($"\nPoczątkowy stan książki '{testBook.Title}': {(new LoanTransaction(testBook)).State}");

        // Simulate borrowing
        Console.WriteLine("\n--- Próba wypożyczenia (proces z BorrowingService) ---");
        borrowingService.ProcessBorrowRequest(readerSession, testBook.Id);

        // Simulate return
        Console.WriteLine("\n--- Próba zwrotu ---");
        borrowingService.ReturnBook(currentReader, testBook.Id);

        // Simulate reservation and borrowing from reservation
        Console.WriteLine("\n--- Rezerwacja, a następnie wypożyczenie z rezerwacji ---");
        Book reservedBook = new Book(202, "Dune", "Frank Herbert");
        LoanTransaction duneTransaction = new LoanTransaction(reservedBook);
        Console.WriteLine($"Początkowy stan 'Dune': {duneTransaction.State}");
        duneTransaction.Reserve();
        Console.WriteLine($"Stan 'Dune' po rezerwacji: {duneTransaction.State}");
        duneTransaction.Borrow();
        Console.WriteLine($"Stan 'Dune' po wypożyczeniu z rezerwacji: {duneTransaction.State}");

        // Simulate overdue
        Console.WriteLine("\n--- Symulacja przeterminowania ---");
        LoanTransaction overdueBookTransaction = new LoanTransaction(new Book(203, "Krzyżacy", "Henryk Sienkiewicz"));
        overdueBookTransaction.Borrow(); // Must be borrowed first
        Console.WriteLine($"Stan 'Krzyżaków' po wypożyczeniu: {overdueBookTransaction.State}");
        overdueBookTransaction.DueDateElapsed();
        Console.WriteLine($"Stan 'Krzyżaków' po przeterminowaniu: {overdueBookTransaction.State}");

        // Simulate lost
        Console.WriteLine("\n--- Symulacja utracenia ---");
        LoanTransaction lostBookTransaction = new LoanTransaction(new Book(204, "Mistrz i Małgorzata", "Michaił Bułhakow"));
        lostBookTransaction.Borrow();
        Console.WriteLine($"Stan 'Mistrza i Małgorzaty' po wypożyczeniu: {lostBookTransaction.State}");
        lostBookTransaction.ReportLost();
        Console.WriteLine($"Stan 'Mistrza i Małgorzaty' po zgłoszeniu utracenia: {lostBookTransaction.State}");

        Console.WriteLine("\nKoniec symulacji. Naciśnij dowolny klawisz...");
        Console.ReadKey();
    }
}
