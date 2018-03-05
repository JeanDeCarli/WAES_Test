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
        /// <summary>
        /// Enum to indicate the Json`s side
        /// </summary>
        public enum Side
        {
            Left,
            Right
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert the encoded JSON into the database
        /// </summary>
        /// <param name="data"></param>
        /// <param name="side"></param>
        public static void InsertData(Data data, Side side)
        {
            var db = new WAESAssignmentDBEntities(); // Create the object to interact with the database using Entity Framework
            var existentData = db.Data.Find(data.Id); // Get the record based on the ID

            // Validate if the record exists, if so update it
            if (existentData != null)
            {
                switch (side)
                {
                    case Side.Left:
                        existentData.LeftSide = data.LeftSide; // Inserts the LEFT encoded Json into the record
                        break;
                    case Side.Right:
                        existentData.RightSide = data.RightSide; // Inserts the Right encoded Json into the record
                        break;
                }
            }
            else // If there is not any record with the same id searched before, create a new one
            {
                db.Data.Add(data); // Insert the new data into de databese
            }
            db.SaveChanges(); // Save the changes
        }

        /// <summary>
        /// Return the comparison of the 2 Jsons based on the data received by parameter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultDiff GetDiff(Data data)
        {
            // Decode the Jsons
            byte[] leftJsonByte = Convert.FromBase64String(data.LeftSide);
            byte[] rightJsonByte = Convert.FromBase64String(data.RightSide);
            var leftJson = Encoding.UTF8.GetString(leftJsonByte);
            var rightJson = Encoding.UTF8.GetString(rightJsonByte);

            var result = new ResultDiff(); // Creates an object to manipulate the result of the comparison

            // Validates if the both Jsons are equal
            if (leftJson.SequenceEqual(rightJson))
            {
                result.AreEqual = true; // add the value into the object indicating that the Jsons are equal
                result.SameSize = true; // add the value into the object indicating that the Jsons have the same size
            }
            else
            {
                // Validate if the Jsons have the same size
                if (leftJsonByte.Length.Equals(rightJsonByte.Length))
                {
                    result.SameSize = true; // add the value into the object indicating that the Jsons have the same size
                }
                var diffPatch = new JsonDiffPatch(); // Creates an object of a nuget package extension responsible for compare the Jsons
                var diffObj = JObject.Parse(diffPatch.Diff(leftJson, rightJson)); // Get the difference between the Jsons
                result.Diffs = diffObj; // Add the differences into the object
            }
            // Add the Size of the Jsons into the object
            result.LetfSize = leftJson.Length;
            result.RightSize = rightJson.Length;

            return result; // Return the result of the comparison
        }

        #endregion
    }
}