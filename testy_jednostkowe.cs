//testy jednostkowe
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

// Aby uruchomić te testy, potrzebne jest zainstalowanie pakietów NUnit i Moq przez NuGet:
// Install-Package NUnit
// Install-Package Moq

[TestFixture]
public class BookTests
{
    [Test]
    public void Borrow_SetsAvailableToFalse()
    {
        // Arrange
        var book = new Book(1, "Test Title", "Test Author");

        // Act
        book.Borrow();

        // Assert
        Assert.IsFalse(book.Available);
    }

    [Test]
    public void Return_SetsAvailableToTrue()
    {
        // Arrange
        var book = new Book(1, "Test Title", "Test Author");
        book.Borrow(); // Ensure it's borrowed first

        // Act
        book.Return();

        // Assert
        Assert.IsTrue(book.Available);
    }

    [Test]
    public void GetDetails_ReturnsCorrectStringForAvailableBook()
    {
        // Arrange
        var book = new Book(1, "Title A", "Author B");

        // Act
        var details = book.GetDetails();

        // Assert
        StringAssert.Contains("[1] Title A - Author B (available)", details);
    }

    [Test]
    public void GetDetails_ReturnsCorrectStringForBorrowedBook()
    {
        // Arrange
        var book = new Book(1, "Title C", "Author D");
        book.Borrow();

        // Act
        var details = book.GetDetails();

        // Assert
        StringAssert.Contains("[1] Title C - Author D (borrowed)", details);
    }
}

[TestFixture]
public class CatalogTests
{
    private Catalog _catalog;

    [SetUp]
    public void Setup()
    {
        _catalog = new Catalog();
        _catalog.AddBook(new Book(1, "Book One", "Author A"));
        _catalog.AddBook(new Book(2, "Another Book", "Author B"));
        _catalog.AddBook(new Book(3, "Book Three", "Author A"));
    }

    [Test]
    public void AddBook_IncreasesBookCount()
    {
        // Arrange
        var newBook = new Book(4, "New Book", "New Author");

        // Act
        _catalog.AddBook(newBook);

        // Assert
        Assert.IsNotNull(_catalog.GetBookById(4));
    }

    [Test]
    public void GetBookById_ReturnsCorrectBook()
    {
        // Act
        var book = _catalog.GetBookById(2);

        // Assert
        Assert.IsNotNull(book);
        Assert.AreEqual("Another Book", book.Title);
    }

    [Test]
    public void GetBookById_ReturnsNullForNonExistentBook()
    {
        // Act
        var book = _catalog.GetBookById(99);

        // Assert
        Assert.IsNull(book);
    }

    [Test]
    public void SearchByTitle_ReturnsMatchingBooks()
    {
        // Act
        var results = _catalog.SearchByTitle("Book").ToList();

        // Assert
        Assert.AreEqual(2, results.Count);
        Assert.IsTrue(results.Any(b => b.Title == "Book One"));
        Assert.IsTrue(results.Any(b => b.Title == "Another Book"));
    }

    [Test]
    public void SearchByTitle_ReturnsEmptyListForNoMatch()
    {
        // Act
        var results = _catalog.SearchByTitle("NonExistent").ToList();

        // Assert
        Assert.IsEmpty(results);
    }

    [Test]
    public void EditBook_UpdatesBookDetails()
    {
        // Arrange
        var updatedBook = new Book(1, "Updated Book One", "Updated Author A");

        // Act
        _catalog.EditBook(updatedBook);
        var book = _catalog.GetBookById(1);

        // Assert
        Assert.IsNotNull(book);
        Assert.AreEqual("Updated Book One", book.Title);
        Assert.AreEqual("Updated Author A", book.Author);
    }

    [Test]
    public void RemoveBook_RemovesBookFromCatalog()
    {
        // Act
        _catalog.RemoveBook(1);
        var book = _catalog.GetBookById(1);

        // Assert
        Assert.IsNull(book);
    }
}

[TestFixture]
public class LoanTransactionTests
{
    private Book _testBook;

    [SetUp]
    public void Setup()
    {
        _testBook = new Book(1, "Test Book", "Test Author");
    }

    [Test]
    public void Constructor_InitializesStateToAvailable()
    {
        // Arrange & Act
        var transaction = new LoanTransaction(_testBook);

        // Assert
        Assert.AreEqual(TransactionState.Available, transaction.State);
    }

    [Test]
    public void Reserve_ChangesStateToReservedFromAvailable()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);

        // Act
        transaction.Reserve();

        // Assert
        Assert.AreEqual(TransactionState.Reserved, transaction.State);
    }

    [Test]
    public void Reserve_ThrowsExceptionFromNonAvailableState()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Reserve(); // Change to Reserved

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => transaction.Reserve());
    }

    [Test]
    public void Borrow_ChangesStateToBorrowedFromAvailable()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);

        // Act
        transaction.Borrow();

        // Assert
        Assert.AreEqual(TransactionState.Borrowed, transaction.State);
        Assert.IsFalse(_testBook.Available); // Book should be marked as unavailable
        Assert.IsNotNull(transaction.BorrowDate);
        Assert.IsNotNull(transaction.DueDate);
    }

    [Test]
    public void Borrow_ChangesStateToBorrowedFromReserved()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Reserve();

        // Act
        transaction.Borrow();

        // Assert
        Assert.AreEqual(TransactionState.Borrowed, transaction.State);
        Assert.IsFalse(_testBook.Available);
    }

    [Test]
    public void DueDateElapsed_ChangesStateToOverdueFromBorrowed()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Borrow();

        // Act
        transaction.DueDateElapsed();

        // Assert
        Assert.AreEqual(TransactionState.Overdue, transaction.State);
    }

    [Test]
    public void StartReturn_ChangesStateToReturnInProgressFromBorrowed()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Borrow();

        // Act
        transaction.StartReturn();

        // Assert
        Assert.AreEqual(TransactionState.ReturnInProgress, transaction.State);
    }

    [Test]
    public void StartReturn_ChangesStateToReturnInProgressFromOverdue()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Borrow();
        transaction.DueDateElapsed();

        // Act
        transaction.StartReturn();

        // Assert
        Assert.AreEqual(TransactionState.ReturnInProgress, transaction.State);
    }

    [Test]
    public void CompleteReturn_ChangesStateToCompletedFromReturnInProgress()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Borrow();
        transaction.StartReturn();

        // Act
        transaction.CompleteReturn();

        // Assert
        Assert.AreEqual(TransactionState.Completed, transaction.State);
        Assert.IsTrue(_testBook.Available); // Book should be marked as available again
    }

    [Test]
    public void ReportLost_ChangesStateToLostFromBorrowed()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Borrow();

        // Act
        transaction.ReportLost();

        // Assert
        Assert.AreEqual(TransactionState.Lost, transaction.State);
    }

    [Test]
    public void ReportLost_ChangesStateToLostFromOverdue()
    {
        // Arrange
        var transaction = new LoanTransaction(_testBook);
        transaction.Borrow();
        transaction.DueDateElapsed();

        // Act
        transaction.ReportLost();

        // Assert
        Assert.AreEqual(TransactionState.Lost, transaction.State);
    }
}
