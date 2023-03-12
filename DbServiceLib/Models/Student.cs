using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib.Models
{
    public class Student
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
