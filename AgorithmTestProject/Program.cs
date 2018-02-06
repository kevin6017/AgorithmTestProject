using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgorithmTestProject
{
    

    class Program
    {

        

        static class globalVars
        {
            public static HashSet<Course> prSet;
            public static List<Course> courseList;
            public static List<Course> remainingCourseList;
            public static List<Semester> semesterList = new List<Semester>();
        }

        static void Main(string[] args)
        {
            var ds = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
            globalVars.remainingCourseList = ds.Deserialize<List<Course>>(File.OpenText("..\\..\\CSclasses.eyaml"));
            globalVars.prSet = new HashSet<Course>();
            initializePriorities();
            buildPrereqList();
            prioritizeCourses();
            assignClassDependencyNum();
            buildSemesterList();
            testSemesterArray(globalVars.semesterList);
        }

        static void testArrayForGlobalVars(List<Course> list)
        {
            Course[] test = new Course[list.Count];
            test = list.ToArray<Course>();
        }

        static void testSemesterArray(List<Semester> list)
        {
            Semester[] test = list.ToArray();
        }

        static void initializePriorities()
        {
            foreach (Course course in globalVars.remainingCourseList)
            {
                int standardPriority = 0;
                int semesterOfferingPriority = (course.fall && course.spring ? 0 : 1);
                int courseDependenciesPriority = 1;
                course.priority = new int[3] { standardPriority, semesterOfferingPriority, courseDependenciesPriority };
            }
        }

        static void buildPrereqList()
        {
            foreach (Course course in globalVars.remainingCourseList)
            {
                if (course.prerequisites != null)
                {
                    foreach (Course prereq in course.prerequisites)
                    {
                        globalVars.prSet.Add(prereq);
                    }
                }
            }
        }

        static void prioritizeCourses()
        {
            foreach (Course course in globalVars.remainingCourseList)
            {
                if (!globalVars.prSet.Contains(course))
                {
                    assignPriorityToPrereqs(course);
                }
            }
        }

        static void assignPriorityToPrereqs(Course course)
        {
            if (course.prerequisites != null)
            {
                foreach(Course prereq in course.prerequisites)
                {
                    if (prereq.priority[0] <= course.priority[0])
                    {
                        prereq.priority[0]++;
                    }
                    
                    assignPriorityToPrereqs(prereq);
                }
            }
        }

        static void assignClassDependencyNum()
        {
            globalVars.remainingCourseList = globalVars.remainingCourseList.OrderBy(x => x.priority[0]).ToList();
            foreach (Course course in globalVars.remainingCourseList)
            {
                if (course.prerequisites != null)
                {
                    foreach (Course prereq in course.prerequisites)
                    {
                        prereq.priority[2] += course.priority[2];
                    }
                }
            }
        }

        static void buildSemesterList()
        {
            //Assign according to user input
            int semestersToGo = 1;
            int totalCreditsToGo = findTotalCredits();
            int targetHours = totalCreditsToGo / semestersToGo;
            List<Course> classList = new List<Course>();
            sortCoursesForScheduling();

            while (globalVars.remainingCourseList.Count > 0)
            {
                Semester currentSemester = new Semester();
                currentSemester.totalCreditHours = 0;
                int currentClassIndex = 0;
                classList = new List<Course>();

                while (currentSemester.totalCreditHours < targetHours && currentClassIndex < globalVars.remainingCourseList.Count)
                {
                    if (clearsChecks(currentSemester, globalVars.remainingCourseList[currentClassIndex]))
                    {
                        classList.Add(globalVars.remainingCourseList[currentClassIndex]);
                        currentSemester.totalCreditHours += globalVars.remainingCourseList[currentClassIndex].creditHours;
                        globalVars.remainingCourseList.RemoveAt(currentClassIndex);
                    }
                    else
                    {
                        currentClassIndex += 1;
                        if (currentClassIndex > globalVars.remainingCourseList.Count)
                        {
                            break;
                        }
                    }
                }
                //must use this to build the array of strings required by semester.cs
                currentSemester.classes = classList.ToArray();
                globalVars.semesterList.Add(currentSemester);
            }
        }

        static void sortCoursesForScheduling()
        {
            globalVars.remainingCourseList.Sort(new SortClasses());
        }

        static int findTotalCredits()
        {
            int creditCounter = 0;
            foreach (Course course in globalVars.remainingCourseList)
            {
                creditCounter += course.creditHours;
            }
            return creditCounter;
        }

        static bool clearsChecks(Semester currentSemester, Course currentCourse)
        {
            if(currentCourse.creditHours + currentSemester.totalCreditHours > 18)
            {
                return false;
            }
            return true;
        }

        static void printClassInfo(Course currentCourse)
        {
            //+ "  Course Title: " + currentCourse.courseTitle 
            string output = "Course Number: " + currentCourse.courseNumber + "  Priority: " + currentCourse.priority[0] + " Single Semester Offering: " + currentCourse.priority[1] + "# of Courses w/ Dependency: " + currentCourse.priority[2] + " Credit Hours: " + currentCourse.creditHours;
            Console.WriteLine(output);
        }

        public class SortClasses : Comparer<Course>
        {
            public override int Compare(Course x, Course y)
            {
                if (x.priority[0].CompareTo(y.priority[0]) != 0)
                {
                    return (x.priority[0].CompareTo(y.priority[0]))*-1;
                }
                else if (x.priority[1].CompareTo(y.priority[1]) != 0)
                {
                    return (x.priority[1].CompareTo(y.priority[1]))*-1;
                }
                else if (x.priority[2].CompareTo(y.priority[2]) != 0)
                {
                    return (x.priority[2].CompareTo(y.priority[2]))*-1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /*static void Main(string[] args)
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
                initializePriorityField(course);               
            }

            buildPrereqList();

            //prioritizeClasses();

            prioritizeCoursesStartingWithZeroPriority();

            assignClassDependencyNum(globalVars.remainingCourseList);

            //printClassInfo(globalVars.remainingCourseList[globalVars.remainingCourseList.Count() - 1]);

            foreach(Course course in globalVars.remainingCourseList.OrderBy(x => x.priority[0]))
            {
                printClassInfo(course);
            }

            buildSemesterList();

            foreach(Semester sem in globalVars.semesterList)
            {
                Console.WriteLine(sem);
            }

            Console.ReadLine();
        }

        static void initializePriorityField(Course course)
        {
            int standardPriority = 0;
            int semesterOfferingPriority = (course.fall && course.spring ?  0 :  1);
            int courseDependenciesPriority = 1;
            course.priority = new int[3] {standardPriority, semesterOfferingPriority, courseDependenciesPriority };
        }

        static void buildPrereqList()
        {
            foreach (Course course in globalVars.remainingCourseList)
            {
                foreach (String prereq in course.prerequisites)
                {
                    globalVars.prSet.Add(globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }
        }*/

        /*
        static void prioritizeClasses()
        {
            foreach(Course course in globalVars.remainingCourseList)
            {
                assignPriorityToPrereqs(course);
            }
        }
        */
        /*
        static void prioritizeCoursesStartingWithZeroPriority()
        {
            foreach(Course course in globalVars.remainingCourseList)
            {
                if (!globalVars.prSet.Contains(course))
                {
                    assignPriorityToPrereqs(course);
                }
            }
        }

        static void assignPriorityToPrereqs(Course course)
        {
            if (course.prerequisites.Length > 0)
            {
                foreach (String prereq in course.prerequisites)
                {//we should really find a way to shallow copy this instead of saerch the list 3 times
                    if (globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq).priority[0] <= course.priority[0])
                    {
                        globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq).priority[0]++;
                    }
                    assignPriorityToPrereqs(globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq));
                }
            }  
        }

        static void assignClassDependencyNum(List<Course> classList)
        {        
            classList = classList.OrderBy(x => x.priority[0]).ToList();
            foreach (Course course in classList)
            {
                foreach (String prereq in course.prerequisites)
                { 
                    globalVars.remainingCourseList.Find(targetCourse => targetCourse.courseNumber == prereq).priority[2] += course.priority[2];
                }
            }
        }

        static void printClassInfo(Course currentCourse)
        {
            //+ "  Course Title: " + currentCourse.courseTitle 
            string output = "Course Number: " + currentCourse.courseNumber + "  Priority: " + currentCourse.priority[0] + " Single Semester Offering: " + currentCourse.priority[1] + "# of Courses w/ Dependency: " + currentCourse.priority[2] + " Credit Hours: " + currentCourse.creditHours;
            Console.WriteLine(output);
        }

        //Semester list courses must be in the form of strings
        static void buildSemesterList()
        {
            //Assign according to user input
            int semestersToGo = 5;
            int totalCreditsToGo = findTotalCredits();
            int targetHours = totalCreditsToGo / semestersToGo;
            List<string> classList = new List<string>();

            while(globalVars.remainingCourseList.Count > 0)
            {
                Semester currentSemester = new Semester();
                currentSemester.totalCreditHours = 0;
                int currentClassIndex = 0;
                classList = new List<string>();

                while (currentSemester.totalCreditHours < targetHours && currentClassIndex < globalVars.remainingCourseList.Count){
                    if (clearsChecks(globalVars.remainingCourseList[currentClassIndex]))
                    {
                        classList.Add(globalVars.remainingCourseList[currentClassIndex].courseNumber);
                        currentSemester.totalCreditHours += globalVars.remainingCourseList[currentClassIndex].creditHours;
                        globalVars.remainingCourseList.RemoveAt(currentClassIndex);
                    }
                    else
                    {
                        currentClassIndex += 1;
                        if(currentClassIndex > globalVars.remainingCourseList.Count)
                        {
                            break;
                        }
                    }
                }
                //must use this to build the array of strings required by semester.cs
                currentSemester.classes = classList.ToArray();
                globalVars.semesterList.Add(currentSemester);
            }
        }

        static bool clearsChecks(Course currentCourse)
        {
            //fill with checks
            return true;
        }

        static int findTotalCredits()
        {
            int creditCounter = 0;
            foreach(Course course in globalVars.remainingCourseList)
            {
                creditCounter += course.creditHours;
            }
            return creditCounter;
        }*/
    }

    
}
