using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork.ViewModels
{
    public class CreateDepartmentViewModel
    {
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? InstructorId { get; set; }
    }

    public class CreateDepartmentResult
    {
        public int DepartmentId { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
