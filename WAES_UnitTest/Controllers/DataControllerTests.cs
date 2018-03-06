using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestCategory("Unit")]
        [Description("Validate if the encoded Json can be added in the left position")]
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
        [TestCategory("Unit")]
        [Description("Validate the response of the left post method when sending a null parameter")]
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
        [TestCategory("Unit")]
        [Description("Validate if the encoded Json can be added in the right position")]
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
        [TestCategory("Unit")]
        [Description("Validate the response of the right post method when sending a null parameter")]
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
        [TestCategory("Unit")]
        [Description("Validate if the Get method returns the diff properly when left json is equals to right json")]
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
        [TestCategory("Unit")]
        [Description("Validate if the Get method returns the diff properly when the left and right json have the same size but different content")]
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
        [TestCategory("Unit")]
        [Description("Validate if the Get method returns the diff properly when the left and right json are different")]
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
        [TestCategory("Unit")]
        [Description("Validate the behaviour of the Get method when it tries to diff only left json")]
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
        [TestCategory("Unit")]
        [Description("Validate the behaviour of the Get method when it tries to diff only right json")]
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
        [TestCategory("Unit")]
        [Description("Validate the behaviour of the Get method sending a nonexistent ID")]
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