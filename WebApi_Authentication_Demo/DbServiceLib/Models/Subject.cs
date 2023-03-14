using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DbServiceLib.Models
{
    public class Subject
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}
