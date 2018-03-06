﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using WAES_UnitTest.Helper;
using System.Net.Http;
using System.Web.Http.Results;
using WAES_Test.Models;
using System.Web.Http;
using System.Net;
using static WAES_Test.Helper.APIsHelper;
using System.Linq;
using static WAES_UnitTest.Helper.Enums;

namespace WAES_Test.Controllers.Tests
{
    [TestClass()]
    public class DataControllerTests
    {
        #region Constants
        private const string item1 = "ew0KICAgICJuYW1lIjoiSm9obiIsDQogICAgImFnZSI6MzAsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";
        private const string item2 = "ew0KICAgICJuYW1lIjoiSmVhbiIsDQogICAgImFnZSI6MjYsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";
        private const string item3 = "ew0KCSJhZ2UiOjI2LA0KCSJuYW1lIjoiSmVhbiIsDQoJIm1lc3NhZ2UiOiAiV0FFUyINCn0=";
        private const int id = 1;
        private TestDataAppContext Testcontext = new TestDataAppContext();
        #endregion

        private DataController Controller;
        private TestValidations Validations;

        #region TestConfig
        [TestInitialize]
        public void TestInit()
        {
            Controller = new DataController(Testcontext);
            Validations = new TestValidations();
        }
        #endregion

        #region UnitTests
        [TestMethod]
        public void InsertLeftProperly()
        {
            //Arrange
            var content = new HttpRequestMessage
            {
                Content = new StringContent(item1, UnicodeEncoding.UTF8, "application/json")
            };

            //Act
            IHttpActionResult actionResult = Controller.InsertLeft(id, content);
            var contentResult = actionResult as OkNegotiatedContentResult<Data>;

            //Assert
            Validations.ValidateContentResult(contentResult, id, item1, Side.Left);
        }

        [TestMethod]
        public void InsertNullLeft()
        {
            //Arrange
            var content = new HttpRequestMessage { };

            //Act
            IHttpActionResult actionResult = Controller.InsertLeft(id, content);
            var contentResult = actionResult as BadRequestErrorMessageResult;

            //Assert
            Validations.ValidateNullContentResult(contentResult);
        }

        [TestMethod]
        public void InsertRightProperly()
        {
            //Arrange
            var content = new HttpRequestMessage
            {
                Content = new StringContent(item1, UnicodeEncoding.UTF8, "application/json")
            };

            //Act
            IHttpActionResult actionResult = Controller.InsertRight(id, content);
            var contentResult = actionResult as OkNegotiatedContentResult<Data>;

            //Assert
            Validations.ValidateContentResult(contentResult, id, item1, Side.Right);
        }

        [TestMethod]
        public void InsertNullRight()
        {
            //Arrange
            var content = new HttpRequestMessage { };

            //Act
            IHttpActionResult actionResult = Controller.InsertRight(id, content);
            var contentResult = actionResult as BadRequestErrorMessageResult;

            //Assert
            Validations.ValidateNullContentResult(contentResult);
        }

        [TestMethod]
        public void ValidateEqualSides()
        {
            //Arrange
            var db = Testcontext;
            var data = db.Data.Add(new Data() { Id = id, LeftSide = item1, RightSide = item1 });
            db.SaveChanges();

            //Act
            IHttpActionResult actionResult = Controller.DiffContent(data.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<ResultDiff>;

            //Assert
            Validations.ValidateDiffResult(contentResult, ResultDiffOptions.EqualSides);
        }

        [TestMethod]
        public void ValidateEqualSizeDifferentContent()
        {
            //Arrange
            var db = Testcontext;
            var data = db.Data.Add(new Data() { Id = id, LeftSide = item1, RightSide = item2 });
            db.SaveChanges();

            //Act
            IHttpActionResult actionResult = Controller.DiffContent(data.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<ResultDiff>;

            //Assert
            Validations.ValidateDiffResult(contentResult, ResultDiffOptions.EqualSizeDifferentContent);
        }

        [TestMethod]
        public void ValidateDifferentSizeAndContent()
        {
            //Arrange
            var db = Testcontext;
            var data = db.Data.Add(new Data() { Id = id, LeftSide = item1, RightSide = item3 });
            db.SaveChanges();

            //Act
            IHttpActionResult actionResult = Controller.DiffContent(data.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<ResultDiff>;

            //Assert
            Validations.ValidateDiffResult(contentResult, ResultDiffOptions.DifferentSizeAndContent);
        }

        [TestMethod]
        public void DiffWithOnlyLeftSide()
        {
            //Arrange
            var db = Testcontext;
            var data = db.Data.Add(new Data() { Id = id, LeftSide = item1});
            db.SaveChanges();

            //Act
            IHttpActionResult actionResult = Controller.DiffContent(data.Id);
            var contentResult = actionResult as BadRequestErrorMessageResult;

            //Assert
            Validations.ValidateDiffWithSideMissing(contentResult);
        }

        [TestMethod]
        public void DiffWithOnlyRightSide()
        {
            //Arrange
            var db = Testcontext;
            var data = db.Data.Add(new Data() { Id = id, RightSide = item1 });
            db.SaveChanges();

            //Act
            IHttpActionResult actionResult = Controller.DiffContent(data.Id);
            var contentResult = actionResult as BadRequestErrorMessageResult;

            //Assert
            Validations.ValidateDiffWithSideMissing(contentResult);
        }

        [TestMethod]
        public void DiffNonexistentId()
        {
            //Act
            IHttpActionResult actionResult = Controller.DiffContent(id);
            var contentResult = actionResult as NotFoundResult;

            //Assert
            Validations.ValidateNonexistentId(contentResult);
        }
        #endregion

    }
}