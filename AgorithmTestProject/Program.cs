using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgorithmTestProject
{

    class Program
    {
        //HashSet<Course> prSet = new HashSet<Course>();
        //List<Course> classList;
        //assign this to remaining class after filter method is written
        //List<Course> remainingClassList;

        static class globalVars
        {
            public static HashSet<Course> prSet;
            public static List<Course> courseList;
            public static List<Course> remainingCourseList;
        }

        static void Main(string[] args)
        {

            List<Course> courseList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json"); //might be passed in at a future time
            List<Course> courseList2 = new JsonLoader().loadCourseList("..\\..\\HonorsCoreClasses.json");
            courseList.AddRange(courseList2);
            //will need to have HonorsCoreClasses/UCore added to it
            globalVars.prSet = new HashSet<Course>();
            //remove this when remaining classes are actually filtered
            globalVars.remainingCourseList = courseList;
            //-----------------------------------

            //set initial to nothing, might want to make this a method
            foreach (Course course in courseList)
            {
                course.priority = 0;
            }

            buildPrereqList();

            prioritizeClasses();

            printClassInfo(globalVars.remainingClassList[globalVars.remainingClassList.Count() - 1]);

            foreach(Course course in globalVars.remainingClassList.OrderBy(x => x.priority))
            {
                printClassInfo(course);
            }

            Console.ReadLine();
        }

        static void buildPrereqList()
        {
            HashSet<Course> constructionSet = new HashSet<Course>();
            foreach (Course course in globalVars.remainingCourseList)
            {
                foreach (String prereq in course.prerequisites)
                {
                    constructionSet.Add(globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }
        }
        /*
        static void findZeroPriorityCourses()
        {
            foreach (Course currentCourse in globalVars.remainingClassList)
            {
                if (!globalVars.prSet.Contains(currentCourse))
                {
                    currentCourse.priority = 0;
                }
            }
        }*/

        static void prioritizeClasses()
        {
            foreach(Course course in globalVars.remainingCourseList)
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
                    globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq).priority++;
                    assignPriorityToPrereqs(globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }  
        }

        static void printClassInfo(Course currentCourse)
        {
            string output = "Course Number: " + currentCourse.courseNumber + "  Course Title: " + currentCourse.courseTitle + "  Priority: " + currentCourse.priority + "  Credit Hours: " + currentCourse.creditHours;
            Console.WriteLine(output);
        }
        //---------------------------------------------------------------------------------
        /*
        void Main(string[] args)
        {
            List<Course> classList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json");
            foreach(Course course in classList)
            {
                course.Priority = "";
            }
            makeSemesters(classList);
        }
        //Need to filter remaining classes still
        void buildPrereqList(List<Course> remainingClassList)
        {
            
            foreach(Course course in remainingClassList)
            {
                foreach(String prereq in course.prerequisites)
                {
                    prSet.Add(remainingClassList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }
        }


        void makeSemesters(List<Course> classList)
        {
            //remove this when remaining classes are actually filtered
            List<Course> remainingClassList = classList;
            //-----------------------------------

            buildPrereqList(remainingClassList);
            prioritizeClasses(remainingClassList);
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
    */
    }
}
