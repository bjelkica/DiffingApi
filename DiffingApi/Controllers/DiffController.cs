using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DiffingApi.Models;
using DiffingApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiffingApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        public DiffService diffService;

        public DiffController()
        {
            diffService = new DiffService();
        }

        // GET: v1/<DiffController>
        [HttpGet]
        public string Get()
        {
            return "Diffing Api";
        }

        // GET v1/<DiffController>/5
        // An endpoint where we get a result of data diffing for chosen id if it exists
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var diff = diffService.GetDiffDataById(id);

            if (diff != null)
            {
                // If diff with given id is missing a data from a one of the endpoints we label it as not found
                if (diff.LeftData == null || diff.RightData == null)
                {
                    return NotFound(null);
                }

                DiffResponse d = Diffing(diff);
                string json = JsonConvert.SerializeObject(d, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return Ok(json);
            }
            else
            {
                return NotFound(null);
            }
        }

        // PUT v1/<DiffController>/5
        // Left endpoint that accepts JSON data
        [HttpPut("{id}/left")]
        public ActionResult PutLeft(int id, [FromBody] InputData data)
        {
            if (!ModelState.IsValid || data.Data == null)
            {
                return BadRequest(null);
            }

            try
            {
                string decodedData = DecodeBase64String(data.Data);
                data.Data = decodedData;
                diffService.AddLeftData(id, data);
                return Created(HttpStatusCode.Created.ToString(), null);
            }
            catch (Exception ex)
            {
                if (ex is System.FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        // PUT v1/<DiffController>/5
        // Right endpoint that accepts JSON data
        [HttpPut("{id}/right")]
        public ActionResult PutRight(int id, [FromBody] InputData data)
        {
            if (!ModelState.IsValid || data.Data == null)
            {
                return BadRequest(null);
            }

            try
            {
                string decodedData = DecodeBase64String(data.Data);
                data.Data = decodedData;
                diffService.AddRightData(id, data);
                return Created(HttpStatusCode.Created.ToString(), null);
            }
            catch (Exception ex)
            {
                if (ex is System.FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }



        #region Custom functions
        // Function that decodes base64 string
        public string DecodeBase64String (string encodedString)
        {
            string converted = Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in converted)
            {
                stringBuilder.Append(((int)c).ToString("x"));
            }

            string decodedString = stringBuilder.ToString();
            return decodedString;
        }

        // Function for diff-ing data
        public DiffResponse Diffing(DiffData diff)
        {
            DiffResponse response = new DiffResponse();
            List<Diff> diffs = new List<Diff>();
            string leftDiff = diff.LeftData;
            string rightDiff = diff.RightData;
            int leftLen = leftDiff.Length;
            int rightLen = rightDiff.Length;

            // If the strings aren't of the same length we aren't interested in differences
            // so we first compare their lengths
            if (leftLen != rightLen)
            {
                response.DiffResultType = "SizeDoNotMatch";
            }
            else
            {
                // If the strings are of the same length we compare them. 
                // If they are not equal we locate and return differences.
                int prevDiff = -1; // Index of the last diff object in the list diffs
                int lastDiff = -2; // Index of the last string chars that were recognized as different 

                for (int i = 0; i < leftLen; i++)
                {
                    if (leftDiff[i] != rightDiff[i])
                    {
                        // If previous chars of both strings were equal this diff represents a start of a new different substring
                        if (lastDiff != i-1)
                        {
                            prevDiff += 1;
                            Diff d = new Diff
                            {
                                Offset = i,
                                Length = 1
                            };
                            diffs.Add(d);                           
                        }
                        // Otherwise we just increase a length of previous diff
                        else
                        {
                            diffs[prevDiff].Length += 1;
                        }
                        lastDiff = i;
                    }
                }
                // We check if we found any differences
                if (prevDiff == -1)
                {
                    response.DiffResultType = "Equals";
                }
                else
                {
                    response.DiffResultType = "ContentDoNotMatch";
                    response.Diffs = diffs;
                }
            }
            return response;
        }
        #endregion
    }
}
