using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib.ModelDtos
{
    public class StudentDto
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }

        public Student ToStudent()
        {
            return new Student {PkId= PkId, Name = Name, Gender=Gender, Age=Age};
        }
    }
}
