using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WAES_Test.Helper;
using WAES_Test.Models;

namespace WAES_Test.Controllers
{
    [RoutePrefix("v1/diff")]
    public class DataController : ApiController
    {
        private WAESAssignmentDBEntities db = new WAESAssignmentDBEntities();

        [Route("all")]
        public IQueryable<Data> GetData()
        {
            return db.Data;
        }

        [Route("{id}")]
        [ResponseType(typeof(Data))]
        public IHttpActionResult DiffContent(int id)
        {
            Data data = db.Data.Find(id);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [Route("{id}/left")]
        [HttpPost]
        public IHttpActionResult InsertLeft(int id, HttpRequestMessage content)
        {
            // Validate if there is a BadRequest
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Creates the object with all the data needed
            var data = new Data() { Id = id, LeftSide = content.Content.ReadAsStringAsync().Result };

            try
            {
                // Call the method that insert the record inside the database
                APIsHelper.InsertData(data, APIsHelper.Side.Left);
            }
            catch (DbUpdateException)
            {
                if (DataExists(data.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.Created);
        }

        [Route("{id}/right")]
        [HttpPost]
        public IHttpActionResult InsertRight(int id, HttpRequestMessage content)
        {
            // Validate if there is a BadRequest
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Creates the object with all the data needed
            var data = new Data() { Id = id, RightSide = content.Content.ReadAsStringAsync().Result };

            try
            {
                // Call the method that insert the record inside the database
                APIsHelper.InsertData(data, APIsHelper.Side.Right);
            }
            catch (DbUpdateException)
            {
                if (DataExists(data.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.Created);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DataExists(int id)
        {
            return db.Data.Count(e => e.Id == id) > 0;
        }
    }
}