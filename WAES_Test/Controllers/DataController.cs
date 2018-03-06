using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WAES_Test.Helper;
using WAES_Test.Models;

namespace WAES_Test.Controllers
{
    [RoutePrefix("v1/diff")]
    public class DataController : ApiController
    {
        private IWAESAssignmentDBEntities db = new WAESAssignmentDBEntities(); // Creates the object to interact with the database using Entity Framework

        public DataController() { }

        public DataController(IWAESAssignmentDBEntities context)
        {
            db = context;
        }

        /// <summary>
        /// Returns the comparison of 2 Jsons based on the id received by parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Data))]
        [HttpGet]
        public IHttpActionResult DiffContent(int id)
        {
            Data data = db.Data.Find(id); // Get the saved left and right encoded Jsons

            // Validate if the data is null
            if (data == null)
                return NotFound();

            return Ok(APIsHelper.GetDiff(data)); // Call the method responsable to get the comparison
        }

        /// <summary>
        /// Inserts the LEFT Json in the record based on the id received by parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Route("{id}/left")]
        [HttpPost]
        public IHttpActionResult InsertLeft(int id, HttpRequestMessage content)
        {
            // Validate if there is a BadRequest
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var data = new Data() { Id = id, LeftSide = content.Content.ReadAsStringAsync().Result }; // Creates the object with all the data needed

            try
            {
                APIsHelper.InsertData(data, APIsHelper.Side.Left, db); // Call the method that insert the record inside the database
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(data);
        }

        /// <summary>
        /// Inserts the RIGHT Json in the record based on the id received by parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Route("{id}/right")]
        [HttpPost]
        public IHttpActionResult InsertRight(int id, HttpRequestMessage content)
        {
            // Validate if there is a BadRequest
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = new Data() { Id = id, RightSide = content.Content.ReadAsStringAsync().Result }; // Creates the object with all the data needed

            try
            {
                APIsHelper.InsertData(data, APIsHelper.Side.Right, db); // Call the method that insert the record inside the database
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(data);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}