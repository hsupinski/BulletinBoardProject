using BulletinBoardProject.Controllers;
using BulletinBoardProject.Models.Domain;
using BulletinBoardProject.Models.ViewModels;
using BulletinBoardProject.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace BulletinBoardProjTests
{
    public class UnitTests
    {
        private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private readonly AnnouncementController _announcementController;

        public UnitTests()
        {
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            _announcementController = new AnnouncementController(_announcementRepositoryMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _announcementController.TempData = tempData;
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange

            // Act
            var result = _announcementController.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithAnnouncement_WhenIdIsValid()
        {
            // Arrange
            var announcementId = 1;
            var announcement = new Announcement
            {
                Id = announcementId,
                Title = "Test Announcement",
                Description = "Test Description",
                CreatedAt = DateTime.UtcNow
            };

            _announcementRepositoryMock.Setup(x => x.GetByIdAsync(announcementId)).ReturnsAsync(announcement);

            // Act
            var result = await _announcementController.Details(announcementId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Announcement>(viewResult.Model);
            Assert.Equal(announcementId, model.Id);
        }

        [Fact]
        public async Task Add_ReturnsRedirectToActionResultWithSuccessMessage_WhenAnnouncementIsValid()
        {
            // Arrange 
            var request = new AddAnnouncementRequest
            {
                Title = "Valid Title",
                Description = "Valid Description"
            };

            // Act
            var result = await _announcementController.Add(request);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Create", redirectToActionResult.ActionName);
            Assert.True(_announcementController.TempData.ContainsKey("SuccessMessage"));
        }

        [Fact]
        public async Task Add_ReturnsRedirectToActionResultWithErrorMessage_WhenAnnouncementIsNotValid()
        {
            // Arrange
            var request = new AddAnnouncementRequest
            {
                Title = "",
                Description = ""
            };

            // Act
            var result = await _announcementController.Add(request);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Create", redirectToActionResult.ActionName);
            Assert.True(_announcementController.TempData.ContainsKey("ErrorMessage"));
        }
    }

    public class HomeControllerTests
    {
        private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            _loggerMock = new Mock<ILogger<HomeController>>();
            _homeController = new HomeController(_loggerMock.Object, _announcementRepositoryMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithCorrectViewModel_WhenAnnouncementsExist()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement
                {
                    Id = 1,
                    Title = "Test Announcement 1",
                    Description = "Test Description 1",
                    CreatedAt = DateTime.UtcNow
                },
                new Announcement
                {
                    Id = 2,
                    Title = "Test Announcement 2",
                    Description = "Test Description 2",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _announcementRepositoryMock.Setup(x => x.GetFromTenDaysAgoAsync()).ReturnsAsync(announcements);

            // Act
            var result = await _homeController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AnnouncementListViewModel>(viewResult.Model);
            Assert.Equal(2, model.Announcements.Count);
            Assert.Equal(1, model.CurrentPage);
            Assert.Equal(1, model.TotalPages);
        }

        [Fact]
        public async Task Index_ReturnsCorrectPagination()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Id = 1, Title = "Test1", CreatedAt = DateTime.UtcNow.AddDays(-1), Description = "Description1" },
                new Announcement { Id = 2, Title = "Test2", CreatedAt = DateTime.UtcNow.AddDays(-2), Description = "Description2" },
                new Announcement { Id = 3, Title = "Test3", CreatedAt = DateTime.UtcNow.AddDays(-3), Description = "Description3" },
                new Announcement { Id = 4, Title = "Test4", CreatedAt = DateTime.UtcNow.AddDays(-4), Description = "Description4" },
                new Announcement { Id = 5, Title = "Test5", CreatedAt = DateTime.UtcNow.AddDays(-5), Description = "Description5" },
                new Announcement { Id = 6, Title = "Test6", CreatedAt = DateTime.UtcNow.AddDays(-6), Description = "Description6" }
            };

            _announcementRepositoryMock.Setup(x => x.GetFromTenDaysAgoAsync()).ReturnsAsync(announcements);

            // Act
            var result = await _homeController.Index(page: 2);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AnnouncementListViewModel>(viewResult.Model);

            // Assumes that 5 announcements are displayed on each page

            Assert.Equal(2, model.CurrentPage);
            Assert.Equal(2, model.TotalPages);
            Assert.Equal(1, model.Announcements.Count); // 1 announcement on the second page
            
        }
    }
}