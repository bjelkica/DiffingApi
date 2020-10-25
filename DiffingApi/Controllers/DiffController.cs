using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiffingApi.Models;
using DiffingApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiffingApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        // GET: v1/<DiffController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
