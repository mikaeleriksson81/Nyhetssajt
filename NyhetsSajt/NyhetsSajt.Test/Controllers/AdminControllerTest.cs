using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NyhetsSajt.Controllers;
using NyhetsSajt.Interfaces;
using NyhetsSajt.Models.Entites;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NyhetsSajt.Test.Controllers
{
    public class AdminControllerTest
    {
        private Mock<IRSSFeedsRepository> rSSRepoMock;
        private AdminController controller;

        public AdminControllerTest()
        {
            rSSRepoMock = new Mock<IRSSFeedsRepository>();
            controller = new AdminController(rSSRepoMock.Object);
        }

        [Fact]
        public void CreateRSSUrlTest_Post_ReturnsCreateRSSUrlView_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRSSUrl = new RSSUrl { FeedName = "mock Expressen" };
            controller.ModelState.AddModelError("Url", "This field is required");

            // Act
            var result = controller.CreateRSSUrl(mockRSSUrl);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(mockRSSUrl, viewResult.ViewData.Model);
            rSSRepoMock.Verify(repo => repo.CreateRSSUrl(mockRSSUrl), Times.Never());

        }

        [Fact]
        public void CreateRSSUrlTest_Post_AddsRSSUrlToRepository_AndRedirectsToIndex()
        {
            // Arrange
            var mockRSSUrl = new RSSUrl { FeedName = "mock NT", Url = "http://www.nt.se/nyheter/norrkoping/rss/" };

            // Act
            var result = controller.CreateRSSUrl(mockRSSUrl);

            // Assert
            rSSRepoMock.Verify(repo => repo.CreateRSSUrl(mockRSSUrl));
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

        }


        [Fact]
        public void DeleteRSSUrlTest_DeleteRSSUrlInRepository_AndRedirectsToIndex()
        {
            // Arrange
            var mockRSSUrlId = 1;
            var mockRSSUrl = new RSSUrl { Id = 1, FeedName = "mock NT", Url = "http://www.nt.se/nyheter/norrkoping/rss/" };
            rSSRepoMock.Setup(repo => repo.GetRSSUrl(mockRSSUrlId)).Returns(mockRSSUrl);

            // Act
            var result = controller.DeleteRSSUrl(mockRSSUrlId);

            // Assert
            rSSRepoMock.Verify(repo => repo.GetRSSUrl(mockRSSUrlId));
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

        }


        [Fact]
        public void EditRSSUrlTest_Post_ReturnsEditRSSUrlView_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRSSUrl = new RSSUrl { FeedName = "mock NT" };
            controller.ModelState.AddModelError("Url", "This field is required");

            // Act
            var result = controller.EditRSSUrl(mockRSSUrl);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(mockRSSUrl, viewResult.ViewData.Model);
            rSSRepoMock.Verify(repo => repo.EditRSSUrl(mockRSSUrl), Times.Never());

        }

        [Fact]
        public void EditRSSUrlTest_Post_EditRSSUrlInRepository_AndRedirectsToIndex()
        {
            // Arrange
            var mockRSSUrl = new RSSUrl { FeedName = "mock NT", Url = "http://www.nt.se/nyheter/norrkoping/rss/" };

            // Act
            var result = controller.EditRSSUrl(mockRSSUrl);

            // Assert
            rSSRepoMock.Verify(repo => repo.EditRSSUrl(mockRSSUrl));
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

        }


    }
}
