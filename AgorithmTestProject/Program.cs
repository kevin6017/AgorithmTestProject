using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgorithmTestProject
{
    
    class Program
    {
        HashSet<string> prSet = new HashSet<string>();
        dynamic classList;
        //assign this to remaining class after filter method is written
        dynamic remainingClassList;
        void Main(string[] args)
        {
            classList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json");
            foreach(Course course in classList)
            {
                course.Priority = "";
            }
            makeSemesters(classList);
        }

        void makeSemesters(List<Course> classList)
        {
            //remove this when remaining classes are actually filtered
            remainingClassList = classList;
            //-----------------------------------

            buildPrereqList(remainingClassList);
            prioritizeClasses(remainingClassList);
        }

        //Need to filter remaining classes still
        void buildPrereqList(List<Course> remainingClassList)
        {
            
            foreach(Course course in remainingClassList)
            {
                foreach(String prereq in course.prerequisites)
                {
                    prSet.Add(prereq);
                }
            }
        }

        void prioritizeClasses(List<Course> remainingClassList)
        {
            foreach(Course course in remainingClassList)
            {
                givePriorityToPrereqs(course);
            }
        }

        void givePriorityToPrereqs(Course currentCourse)
        {
            if(currentCourse.prerequisites != null)
            {
                //prioritize shit
            }
        }
    }
}
