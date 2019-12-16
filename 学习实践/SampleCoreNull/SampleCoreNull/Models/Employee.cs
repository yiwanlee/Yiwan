using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCoreNull.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "尚有未填写项，请先完善"), MinLength(3)]
        public string Name { get; set; }
    }
}
