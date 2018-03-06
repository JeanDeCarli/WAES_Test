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

namespace WAES_Test.Controllers.Tests
{
    [TestClass()]
    public class DataControllerTests
    {
        #region Constants
        private const string item = "ew0KICAgICJuYW1lIjoiSm9obiIsDQogICAgImFnZSI6MzAsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";
        private const int id = 1;
        #endregion

        private DataController Controller;
        private TestValidations Validations;

        #region TestConfig
        [TestInitialize]
        public void TestInit()
        {
            Controller = new DataController(new TestDataAppContext());
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
                Content = new StringContent(item, UnicodeEncoding.UTF8, "application/json")
            };

            //Act
            IHttpActionResult actionResult = Controller.InsertLeft(id, content);
            var contentResult = actionResult as OkNegotiatedContentResult<Data>;

            //Assert
            Validations.ValidateContentResult(contentResult, id, item, Side.Left);
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
                Content = new StringContent(item, UnicodeEncoding.UTF8, "application/json")
            };

            //Act
            IHttpActionResult actionResult = Controller.InsertRight(id, content);
            var contentResult = actionResult as OkNegotiatedContentResult<Data>;

            //Assert
            Validations.ValidateContentResult(contentResult, id, item, Side.Right);
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
        #endregion

    }
}