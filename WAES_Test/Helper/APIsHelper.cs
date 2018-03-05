using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WAES_Test.Models;

namespace WAES_Test.Helper
{
    public static class APIsHelper
    {
        #region Enums

        public enum Side
        {
            Left,
            Right
        }

        #endregion

        #region Methods

        public static void InsertData(Data data, Side side)
        {
            var db = new WAESAssignmentDBEntities();
            var existentData = db.Data.Find(data.Id);

            if (existentData != null)
            {
                switch (side)
                {
                    case Side.Left:
                        existentData.LeftSide = data.LeftSide;
                        break;
                    case Side.Right:
                        existentData.RightSide = data.RightSide;
                        break;
                }
            }
            else
            {
                db.Data.Add(data);
            }
            db.SaveChanges();
        }

        public static ResultDiff GetDiff(Data data)
        {
            byte[] leftJsonByte = Convert.FromBase64String(data.LeftSide);
            byte[] rightJsonByte = Convert.FromBase64String(data.RightSide);
            var leftJson = Encoding.UTF8.GetString(leftJsonByte);
            var rightJson = Encoding.UTF8.GetString(rightJsonByte);

            var result = new ResultDiff();

            if (leftJson.SequenceEqual(rightJson))
            {
                result.AreEqual = true;
                result.SameSize = true;
            }
            else
            {
                if (leftJsonByte.Length.Equals(rightJsonByte.Length))
                {
                    result.SameSize = true;
                }
                var diffPatch = new JsonDiffPatch();
                var diffObj = JObject.Parse(diffPatch.Diff(leftJson, rightJson));
                result.Diffs = diffObj;
            }
            result.LetfSize = leftJson.Length;
            result.RightSize = rightJson.Length;

            return result;
        }

        #endregion
    }
}