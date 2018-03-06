using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WAES_Test.Models;
using static WAES_Test.Helper.APIsHelper;
using static WAES_UnitTest.Helper.Enums;

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

        public void ValidateDiffResult(OkNegotiatedContentResult<ResultDiff> resultDiff, ResultDiffOptions resultOptions)
        {
            Assert.IsNotNull(resultDiff);

            switch (resultOptions)
            {
                case ResultDiffOptions.EqualSides:
                    Assert.IsTrue(resultDiff.Content.AreEqual);
                    Assert.IsTrue(resultDiff.Content.SameSize);
                    Assert.AreEqual(resultDiff.Content.LetfSize, resultDiff.Content.RightSize);
                    Assert.IsNull(resultDiff.Content.Diffs);
                    break;
                case ResultDiffOptions.EqualSizeDifferentContent:
                    Assert.IsFalse(resultDiff.Content.AreEqual);
                    Assert.IsTrue(resultDiff.Content.SameSize);
                    Assert.AreEqual(resultDiff.Content.LetfSize, resultDiff.Content.RightSize);
                    Assert.IsNotNull(resultDiff.Content.Diffs);
                    break;
                case ResultDiffOptions.DifferentSizeAndContent:
                    Assert.IsFalse(resultDiff.Content.AreEqual);
                    Assert.IsFalse(resultDiff.Content.SameSize);
                    Assert.AreNotEqual(resultDiff.Content.LetfSize, resultDiff.Content.RightSize);
                    Assert.IsNotNull(resultDiff.Content.Diffs);
                    break;
            }
        }

        public void ValidateDiffWithSideMissing(BadRequestErrorMessageResult diffResult)
        {
            Assert.IsNotNull(diffResult);
            Assert.IsTrue(diffResult.Message.Contains("There is a side data missing, please provide the left and right data in the record ID"));
        }

        public void ValidateNonexistentId(NotFoundResult diffResult)
        {
            Assert.IsNotNull(diffResult);
            Assert.IsInstanceOfType(diffResult, typeof(NotFoundResult));
        }
    }
}
