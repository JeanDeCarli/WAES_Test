using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using WAES_UnitTest.Helper;
using System.Net.Http;
using System.Web.Http.Results;
using WAES_Test.Models;

namespace WAES_Test.Controllers.Tests
{
    [TestClass()]
    public class DataControllerTests
    {
        [TestMethod()]
        public void InsertLeftTest()
        {
            //Arrange
            var controller = new DataController(new TestDataAppContext());

            var item = "ew0KICAgICJuYW1lIjoiSm9obiIsDQogICAgImFnZSI6MzAsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";

            var content = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:54024/v1/diff/1/left"),
                Content = new StringContent(item, UnicodeEncoding.UTF8, "application/json")
            };

            //Act

            var result =
                controller.InsertLeft(3, content) as CreatedAtRouteNegotiatedContentResult<Data>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "DefaultApi");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }
        
    }
}