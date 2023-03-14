using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib.ModelDtos
{
    public class SubjectDto
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Subject ToSubject()
        {
            return new Subject { PkId = PkId, Name = Name, Description = Description };
        }
    }
}
