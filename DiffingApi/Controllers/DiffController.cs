using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DiffingApi.BusinessLogic;
using DiffingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiffingApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        public DiffLogic diffLogic;

        public DiffController()
        {
            diffLogic = new DiffLogic();
        }

        // GET: v1/<DiffController>
        [HttpGet]
        public string Get()
        {

            return "Diffing Api";
        }

        // GET v1/<DiffController>/<id>
        // An endpoint where we get a result of data diffing for chosen id if it exists
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var diff = diffLogic.GetDiffDataById(id);

                if (diffLogic.IsDiffMissingData(diff))
                {
                    return NotFound();
                }

                DiffResponse d = diffLogic.Diffing(diff);
                string json = JsonConvert.SerializeObject(d, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return Ok(json);                
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT v1/<DiffController>/<id>/<side>
        // An endpoint that accepts JSON data and depending on the parameter side adds data to a database
        [HttpPut("{id}/{side}")]
        public ActionResult PutData(int id, string side, [FromBody] InputData data)
        {
            if (!ModelState.IsValid || data.Data == null || (side != "left" && side != "right"))
            {
                return BadRequest();
            }

            try
            {
                diffLogic.AddData(id, side, data);
                return Created(HttpStatusCode.Created.ToString(), null);
            }
            catch (Exception ex)
            {
                if (ex is System.FormatException)
                {
                    return BadRequest(ex.Message);
                }
                return StatusCode(500);
            }
        }
    }
}
