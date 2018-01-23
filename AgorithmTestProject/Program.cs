using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgorithmTestProject
{

    class Program
    {

        static class globalVars
        {
            public static HashSet<Course> prSet;
            public static List<Course> classList;
            public static List<Course> remainingClassList;
        }

        static void Main(string[] args)
        {

            List<Course> courseList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json"); //might be passed in at a future time
            List<Course> courseList2 = new JsonLoader().loadCourseList("..\\..\\HonorsCoreClasses.json");
            courseList.AddRange(courseList2);
            //will need to have HonorsCoreClasses/UCore added to it
            globalVars.prSet = new HashSet<Course>();
            //remove this when remaining classes are actually filtered
            globalVars.remainingClassList = courseList;
            //-----------------------------------

            //set initial to nothing, might want to make this a method
            foreach (Course course in courseList)
            {
                course.priority = 0;
            }

            buildPrereqList();

            prioritizeClasses();


            foreach(Course course in globalVars.remainingClassList.OrderBy(x => x.priority))
            {
                printClassInfo(course);
            }

            Console.ReadLine();
        }

        static void buildPrereqList()
        {
            HashSet<Course> constructionSet = new HashSet<Course>();
            foreach (Course course in globalVars.remainingClassList)
            {
                foreach (String prereq in course.prerequisites)
                {
                    constructionSet.Add(globalVars.remainingClassList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }
        }

        static void prioritizeClasses()
        {
            foreach(Course course in globalVars.remainingClassList)
            {
                assignPriorityToPrereqs(course);
            }
        }

        static void assignPriorityToPrereqs(Course course)
        {
            if (course.prerequisites.Length > 0)
            {
                foreach (String prereq in course.prerequisites)
                {
                    globalVars.remainingClassList.Find(targetCourse => targetCourse.courseNumber == prereq).priority++;
                    assignPriorityToPrereqs(globalVars.remainingClassList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }  
        }

        static void printClassInfo(Course currentCourse)
        {
            string output = "Course Number: " + currentCourse.courseNumber + "  Course Title: " + currentCourse.courseTitle + "  Priority: " + currentCourse.priority + "  Credit Hours: " + currentCourse.creditHours;
            Console.WriteLine(output);
        }
    }
}
