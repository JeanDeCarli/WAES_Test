using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using WAES_UnitTest.Helper;
using System.Net.Http;
using System.Web.Http.Results;
using WAES_Test.Models;
using System.Web.Http;
using System.Net;

namespace WAES_Test.Controllers.Tests
{
    [TestClass()]
    public class DataControllerTests
    {
        #region Consts
        private const string item = "ew0KICAgICJuYW1lIjoiSm9obiIsDQogICAgImFnZSI6MzAsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";
        private const int id = 1;
        #endregion

        private DataController controller;

        [TestInitialize]
        public void TestInit()
        {
            controller = new DataController(new TestDataAppContext());
        }

        [TestMethod]
        public void InsertLeftProperly()
        {
            //Arrange
            var content = new HttpRequestMessage
            {
                Content = new StringContent(item, UnicodeEncoding.UTF8, "application/json")
            };

            //Act
            IHttpActionResult actionResult = controller.InsertLeft(id, content);
            var contentResult = actionResult as OkNegotiatedContentResult<Data>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.Id, id);
            Assert.AreEqual(contentResult.Content.LeftSide, item);
        }
    }
}