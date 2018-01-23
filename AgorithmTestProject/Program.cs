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

        static void Main(string[] args)
        {

            List<Course> courseList = new JsonLoader().loadCourseList("..\\..\\CSclasses.json"); //might be passed in at a future time
            //will need to have HonorsCoreClasses/UCore added to it
            HashSet<Course> prereqSet = new HashSet<Course>();
            //remove this when remaining classes are actually filtered
            List<Course> remainingCourseList = courseList;
            //-----------------------------------

            //set initial to nothing, might want to make this a method
            foreach (Course course in courseList)
            {
                course.priority = -1;
            }
            printClassInfo(courseList[0]);

            prereqSet = buildPrereqList(remainingCourseList);


            Console.ReadLine();
        }

        static HashSet<Course> buildPrereqList(List<Course> remainingCourseList)
        {
            HashSet<Course> constructionSet = new HashSet<Course>();
            foreach (Course course in remainingCourseList)
            {
                foreach (String prereq in course.prerequisites)
                {
                    constructionSet.Add(remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }
            return constructionSet;
        }

        static void findZeroPriorityCourses(List<Course> remainingCourseList, HashSet<Course> prereqSet)
        {
            foreach (Course currentCourse in remainingCourseList)
            {
                if (!prereqSet.Contains(currentCourse))
                {
                    currentCourse.priority = 0;
                }
            }
        }

        static void givePriorityToPrereqs(Course currentCourse)
        {

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
