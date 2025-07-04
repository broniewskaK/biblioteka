//testy mockowanie
using NUnit.Framework;
using Moq; // zainstalowany pakiet Moq

[TestFixture]
public class BorrowingServiceTests
{
    private Mock<Catalog> _mockCatalog;
    private BorrowingService _borrowingService;
    private UserSession _testSession;
    private Reader _testReader;

    [SetUp]
    public void Setup()
    {
        _mockCatalog = new Mock<Catalog>();
        _borrowingService = new BorrowingService(_mockCatalog.Object);
        _testReader = new Reader(1, "Test Reader", "test@example.com");
        _testSession = new UserSession(_testReader);
    }

    [Test]
    public void ProcessBorrowRequest_SuccessfullyBorrowsBook()
    {
        // Arrange
        var bookId = 1;
        var book = new Book(bookId, "Mocked Book", "Mocked Author");
        book.Return(); // Ensure it's available initially

        _mockCatalog.Setup(c => c.GetBookById(bookId)).Returns(book);

        // Act
        _borrowingService.ProcessBorrowRequest(_testSession, bookId);

        // Assert
        // Verify that GetBookById was called once
        _mockCatalog.Verify(c => c.GetBookById(bookId), Times.Once);
        // Verify that the book was marked as unavailable (implicitly by LoanTransaction.Borrow())
        Assert.IsFalse(book.Available);
        // Additional verifications for console output or logs would go here
    }

    [Test]
    public void ProcessBorrowRequest_DoesNotBorrowUnavailableBook()
    {
        // Arrange
        var bookId = 1;
        var book = new Book(bookId, "Mocked Book", "Mocked Author");
        book.Borrow(); // Make it unavailable

        _mockCatalog.Setup(c => c.GetBookById(bookId)).Returns(book);

        // Act
        _borrowingService.ProcessBorrowRequest(_testSession, bookId);

        // Assert
        _mockCatalog.Verify(c => c.GetBookById(bookId), Times.Once);
        Assert.IsFalse(book.Available); // Should remain unavailable
        // Verify that no further actions (like saving transaction) were attempted
        // This is harder to verify without mocking the asynchronous tasks,
        // but we can check if the book state changed unexpectedly.
    }

    [Test]
    public void ProcessBorrowRequest_DoesNotBorrowNonExistentBook()
    {
        // Arrange
        var bookId = 99;
        _mockCatalog.Setup(c => c.GetBookById(bookId)).Returns((Book)null);

        // Act
        _borrowingService.ProcessBorrowRequest(_testSession, bookId);

        // Assert
        _mockCatalog.Verify(c => c.GetBookById(bookId), Times.Once);
        // No book state change should occur
    }

    [Test]
    public void ProcessBorrowRequest_DoesNotProcessWithInvalidSession()
    {
        // Arrange
        var invalidSession = new UserSession(_testReader);
        // Simulate invalid session (though IsValid is always true in the current simple UserSession)
        // For a real scenario, UserSession.IsValid would be settable or return false based on some criteria.
        // For this mock, we don't need to mock Catalog.GetBookById if session is invalid early.

        // Act
        _borrowingService.ProcessBorrowRequest(new UserSession(null) { /* Make invalid if possible */ }, 1); // Pass a session that is clearly invalid or set it up to be invalid

        // Assert
        _mockCatalog.Verify(c => c.GetBookById(It.IsAny<int>()), Times.Never); // Should not even try to get the book
    }

    [Test]
    public void ReturnBook_SuccessfullyReturnsBook()
    {
        // Arrange
        var bookId = 1;
        var book = new Book(bookId, "Mocked Borrowed Book", "Mocked Author");
        book.Borrow(); // Mark as borrowed
        _mockCatalog.Setup(c => c.GetBookById(bookId)).Returns(book);

        // Act
        _borrowingService.ReturnBook(_testReader, bookId);

        // Assert
        _mockCatalog.Verify(c => c.GetBookById(bookId), Times.Once);
        Assert.IsTrue(book.Available); // Book should be marked as available
    }

    [Test]
    public void ReturnBook_DoesNotReturnNonBorrowedBook()
    {
        // Arrange
        var bookId = 1;
        var book = new Book(bookId, "Mocked Available Book", "Mocked Author");
        // Book is available initially

        _mockCatalog.Setup(c => c.GetBookById(bookId)).Returns(book);

        // Act
        _borrowingService.ReturnBook(_testReader, bookId);

        // Assert
        _mockCatalog.Verify(c => c.GetBookById(bookId), Times.Once);
        Assert.IsTrue(book.Available); // Should remain available
    }

    [Test]
    public void ReturnBook_DoesNotReturnNonExistentBook()
    {
        // Arrange
        var bookId = 99;
        _mockCatalog.Setup(c => c.GetBookById(bookId)).Returns((Book)null);

        // Act
        _borrowingService.ReturnBook(_testReader, bookId);

        // Assert
        _mockCatalog.Verify(c => c.GetBookById(bookId), Times.Once);
        // No book state change should occur
    }
}
