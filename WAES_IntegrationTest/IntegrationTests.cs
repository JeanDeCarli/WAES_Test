using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WAES_Test.Models;

namespace WAES_IntegrationTest
{
    [TestFixture]
    public class IntegrationTests
    {
        #region Constants
        private const string item1 = "ew0KICAgICJuYW1lIjoiSm9obiIsDQogICAgImFnZSI6MzAsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";
        private const string item2 = "ew0KICAgICJuYW1lIjoiSmVhbiIsDQogICAgImFnZSI6MjYsDQogICAgImNhcnMiOiBbDQogICAgICAgIHsgIm5hbWUiOiJGb3JkIiwgIm1vZGVscyI6WyAiRmllc3RhIiwgIkZvY3VzIiwgIk11c3RhbmciIF0gfSwNCiAgICAgICAgeyAibmFtZSI6IkJNVyIsICJtb2RlbHMiOlsgIjMyMCIsICJYMyIsICJYNSIgXSB9LA0KICAgICAgICB7ICJuYW1lIjoiRmlhdCIsICJtb2RlbHMiOlsgIjUwMCIsICJQYW5kYSIgXSB9DQogICAgXQ0KIH0=";
        private const string item3 = "ew0KCSJhZ2UiOjI2LA0KCSJuYW1lIjoiSmVhbiIsDQoJIm1lc3NhZ2UiOiAiV0FFUyINCn0=";
        private const string baseURL = "http://waestestjean.azurewebsites.net/v1/diff";
        private const string left = "left";
        private const string right = "right";
        private const string id = "1";
        #endregion

        static HttpClient client = new HttpClient();

        [Test]
        [Category("Integration")]
        [Description("Validate if the left endpoint is returning the right status code")]
        public async Task ValidateLeftEndpointReturnsOKwhenSuccess()
        {
            //Arrange
            var content = new StringContent(item3, UnicodeEncoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, left), content);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        [Category("Integration")]
        [Description("Validate if the right endpoint is returning the right status code")]
        public async Task ValidateRightEndpointReturnsOKwhenSuccess()
        {
            //Arrange
            var content = new StringContent(item3, UnicodeEncoding.UTF8, "application/json");
            
            //Act
            HttpResponseMessage response = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, right), content);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        [Category("Integration")]
        [Description("Validate if the diff endpoint is returning the right object when sending the same json to left and right side")]
        public async Task ValidateDiffEndpointSameData()
        {
            //Arrange
            var contentLeft = new StringContent(item3, UnicodeEncoding.UTF8, "application/json");
            var contentRight = new StringContent(item3, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage responseLeft = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, left), contentLeft);
            responseLeft.EnsureSuccessStatusCode();
            HttpResponseMessage responseRight = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, right), contentRight);
            responseRight.EnsureSuccessStatusCode();

            // Act
            ResultDiff resultDiff = null;
            HttpResponseMessage response = await client.GetAsync(string.Format("{0}/{1}", baseURL, id));
            if (response.IsSuccessStatusCode)
            {
                resultDiff = await response.Content.ReadAsAsync<ResultDiff>();
            }

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(resultDiff.AreEqual);
        }

        [Test]
        [Category("Integration")]
        [Description("Validate if the diff endpoint is returning the right object when sending the jsons with the same size but different data")]
        public async Task ValidateDiffEndpointSameSizeDifferentData()
        {
            //Arrange
            var contentLeft = new StringContent(item1, UnicodeEncoding.UTF8, "application/json");
            var contentRight = new StringContent(item2, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage responseLeft = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, left), contentLeft);
            responseLeft.EnsureSuccessStatusCode();
            HttpResponseMessage responseRight = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, right), contentRight);
            responseRight.EnsureSuccessStatusCode();

            // Act
            ResultDiff resultDiff = null;
            HttpResponseMessage response = await client.GetAsync(string.Format("{0}/{1}", baseURL, id));
            if (response.IsSuccessStatusCode)
            {
                resultDiff = await response.Content.ReadAsAsync<ResultDiff>();
            }

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(resultDiff.SameSize);
            Assert.IsFalse(resultDiff.AreEqual);
        }

        [Test]
        [Category("Integration")]
        [Description("Validate if the diff endpoint is returning the right object when sending completely different jsons")]
        public async Task ValidateDiffEndpointDifferentSizeAndData()
        {
            //Arrange
            var contentLeft = new StringContent(item1, UnicodeEncoding.UTF8, "application/json");
            var contentRight = new StringContent(item3, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage responseLeft = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, left), contentLeft);
            responseLeft.EnsureSuccessStatusCode();
            HttpResponseMessage responseRight = await client.PostAsync(
                string.Format("{0}/{1}/{2}", baseURL, id, right), contentRight);
            responseRight.EnsureSuccessStatusCode();

            // Act
            ResultDiff resultDiff = null;
            HttpResponseMessage response = await client.GetAsync(string.Format("{0}/{1}", baseURL, id));
            if (response.IsSuccessStatusCode)
            {
                resultDiff = await response.Content.ReadAsAsync<ResultDiff>();
            }

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsFalse(resultDiff.SameSize);
            Assert.IsFalse(resultDiff.AreEqual);
        }
    }
}
