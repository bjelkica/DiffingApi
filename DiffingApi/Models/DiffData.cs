using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiffingApi.Models
{
    public class DiffData
    {
        public int Id { get; set; }
        public string LeftData { get; set; }
        public string RightData { get; set; }
    }
}
