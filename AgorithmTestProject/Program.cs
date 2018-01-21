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
            dynamic classList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json");
            foreach(Course course in classList)
            {
                course.Priority = 0;
            }
            Console.WriteLine(classList[0].Priority);
            makeSemesters(classList);
        }

        static void makeSemesters(List<Course> classList)
        {
            buildPrereqList(classList);
        }

        //Need to filter remaining classes still
        static void buildPrereqList(List<Course> remainingClassList)
        {
            HashSet<String> prSet = new HashSet<String>();
            foreach(Course course in remainingClassList)
            {
                foreach(String prereq in course.prerequisites)
                {
                    prSet.Add(prereq);
                }
            }
        }
    }
}
