using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiffingApi.Models
{
    public class DiffResponse
    {
        public string DiffResultType { get; set; }
        public List<Diff> Diffs { get; set; }
    }
}
