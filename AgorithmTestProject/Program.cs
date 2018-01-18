using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgorithmTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var classList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json");
            makeSemesters(classList);
        }

        static void makeSemesters(List<Course> classList)
        {

        }
    }
}
