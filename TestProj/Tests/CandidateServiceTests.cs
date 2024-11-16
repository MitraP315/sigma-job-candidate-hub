using JobHubAPI.Crud;
using JobHubAPI.Model;
using JobHubAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace JobHubAPI.Tests
{
    [TestClass]
    public class CandidateServicesTests
    {
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<CrudService<Candidate>> _mockCrudService;
        private readonly CandidateServices _candidateService;
        private readonly Candidate _testCandidate;

        public CandidateServicesTests()
        {
            _mockCache = new Mock<IMemoryCache>();
            _mockCrudService = new Mock<CrudService<Candidate>>();

            // Initialize the test candidate
            _testCandidate = new Candidate
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "Mitra Prasad",
                LastName = "Poudel",
                PhoneNumber = +9779867754315,
                PreferredContactTime = "11:30 NPT",
                LinkedInProfile = "https://www.linkedin.com/in/poudelmitra/",
                GitHubProfile = "https://github.com/MitraP315",
                Comment = "test Data"
            };

            // Mock Cache Entry
            var mockCacheEntry = new Mock<ICacheEntry>();
            _mockCache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(mockCacheEntry.Object);

            // Inject mocks into service
            _candidateService = new CandidateServices(_mockCache.Object)
            {
                CandidateCrudService = _mockCrudService.Object
            };
        }

        [TestMethod]
        public async Task CreateOrUpdateCandidateInfo_CreatesNewCandidate_WhenNotInCacheOrDatabase()
        {
            // Arrange
            Candidate candidateFromCache = null; // Local variable for TryGetValue out parameter
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<string>(), out candidateFromCache)).Returns(false);
            _mockCrudService.Setup(s => s.QueryAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((Candidate)null);
            _mockCrudService.Setup(s => s.InsertAsync(It.IsAny<Candidate>())).ReturnsAsync(1);

            // Act
            var response = await _candidateService.CreateOrUpdateCandidateInfo(_testCandidate);

            // Assert
            Assert.AreEqual(200, response.Code);
            Assert.AreEqual("Candidate Info Saved Successfully.", response.Message);
            Assert.AreEqual(_testCandidate.Email, response.Data);

            // Verify cache set and insert operations
            _mockCache.Verify(c => c.Set(It.IsAny<string>(), _testCandidate, It.IsAny<TimeSpan>()), Times.Once);
            _mockCrudService.Verify(s => s.InsertAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateOrUpdateCandidateInfo_UpdatesExistingCandidate_WhenFoundInCache()
        {
            // Arrange
            Candidate candidateFromCache = _testCandidate; // Local variable for TryGetValue out parameter
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<string>(), out candidateFromCache)).Returns(true);
            _mockCrudService.Setup(s => s.UpdateAsync(It.IsAny<Candidate>())).ReturnsAsync(true);

            // Act
            var response = await _candidateService.CreateOrUpdateCandidateInfo(_testCandidate);

            // Assert
            Assert.AreEqual(200, response.Code);
            Assert.AreEqual("Candidate Info Updated Successfully.", response.Message);
            Assert.AreEqual(true, response.Data);

            // Verify cache set and update operations
            _mockCache.Verify(c => c.Set(It.IsAny<string>(), _testCandidate, It.IsAny<TimeSpan>()), Times.Once);
            _mockCrudService.Verify(s => s.UpdateAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateOrUpdateCandidateInfo_UpdatesExistingCandidate_WhenFoundInDatabase()
        {
            // Arrange
            Candidate candidateFromCache = null; // Local variable for TryGetValue out parameter
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<string>(), out candidateFromCache)).Returns(false);
            _mockCrudService.Setup(s => s.QueryAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(_testCandidate);
            _mockCrudService.Setup(s => s.UpdateAsync(It.IsAny<Candidate>())).ReturnsAsync(true);

            // Act
            var response = await _candidateService.CreateOrUpdateCandidateInfo(_testCandidate);

            // Assert
            Assert.AreEqual(200, response.Code);
            Assert.AreEqual("Candidate Info Updated Successfully.", response.Message);
            Assert.AreEqual(true, response.Data);

            // Verify cache set and update operations
            _mockCache.Verify(c => c.Set(It.IsAny<string>(), _testCandidate, It.IsAny<TimeSpan>()), Times.Once);
            _mockCrudService.Verify(s => s.UpdateAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateOrUpdateCandidateInfo_ReturnsErrorResponse_OnException()
        {
            // Arrange
            Candidate candidateFromCache = null; // Local variable for TryGetValue out parameter
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<string>(), out candidateFromCache)).Throws(new Exception("Cache error"));

            // Act
            var response = await _candidateService.CreateOrUpdateCandidateInfo(_testCandidate);

            // Assert
            Assert.AreEqual(500, response.Code);
            Assert.AreEqual("Cache error", response.Message);
            Assert.IsNull(response.Data); // Typically, on an error, the data is null or empty

            // Note: No cache set or CRUD service methods should be called in case of an exception.
            _mockCache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
            _mockCrudService.VerifyNoOtherCalls();
        }
    }
}
