using DbServiceLib.ModelDtos;
using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib
{
    public static class ExtendFunctions
    {
        public static SubjectDto ToDto(this Subject subject)
        {
            return new SubjectDto { PkId = subject.PkId, Name = subject.Name, Description = subject.Description };
        }
        public static StudentDto ToDto(this Student student)
        {
            return new StudentDto { PkId = student.PkId, Name = student.Name, Age = student.Age, Gender = student.Gender };
        }
    }
}
