using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WAES_Test.Models;
using static WAES_Test.Helper.APIsHelper;

namespace WAES_UnitTest.Helper
{
    public class TestValidations
    {
        public void ValidateContentResult(OkNegotiatedContentResult<Data> contentResult, int id, string item, Side side)
        {
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.Id, id);
            switch (side)
            {
                case Side.Left:
                    Assert.AreEqual(contentResult.Content.LeftSide, item);
                    break;
                case Side.Right:
                    Assert.AreEqual(contentResult.Content.RightSide, item);
                    break;
            }
            
        }

        public void ValidateNullContentResult(BadRequestErrorMessageResult contentResult)
        {
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Message, "Please provide a JSON base64 encoded binary data");
        }
    }
}
