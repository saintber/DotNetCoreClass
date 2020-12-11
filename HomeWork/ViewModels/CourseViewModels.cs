using System.Collections.Generic;

namespace HomeWork.ViewModels
{
    public class CreateCourseViewModel
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
    }

    public class UpdateCourseViewModel
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
    }

    public class AddCourseInstructorsViewModel
    {
        public IList<int> InstructorID { get; set; }
    }

    public class RemoveCourseInstructorsViewModel
    {
        public IList<int> InstructorID { get; set; }
    }

    public class UpdateGradeViewModel
    {
        public int StudentId { get; set; }
        public int Grade { get; set; }
    }
}