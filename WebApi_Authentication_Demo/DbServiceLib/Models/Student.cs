using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DbServiceLib.Models
{
    public class Student
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        //这里使用virtual是为了启用延迟加载（Lazy Loading）功能，这样在访问Subjects属性时，相关的Subject数据会在需要时才从数据库中加载，
        //而不是在查询Student时立即加载所有相关的Subject数据。这有助于提高性能，尤其是在处理大量数据时。
        public virtual List<Subject> Subjects { get; set; }
    }
}
