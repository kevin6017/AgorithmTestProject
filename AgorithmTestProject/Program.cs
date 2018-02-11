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
            globalVars.remainingCourseList = ds.Deserialize<List<Course>>(File.OpenText("..\\..\\HonorsCoreClasses.eyaml"));
            globalVars.remainingCourseList.AddRange(ds.Deserialize<List<Course>>(File.OpenText("..\\..\\HonorsCoreClasses.eyaml")));
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
                foreach (Course prereq in course.prerequisites)
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
            bool isFallTracker = true;

            int totalCreditsToGo = findTotalCredits();
            int targetHours = totalCreditsToGo / semestersToGo;
            List<Course> classList = new List<Course>();
            sortCoursesForScheduling();

            while (globalVars.remainingCourseList.Count > 0)
            {
                Semester currentSemester = new Semester();
                currentSemester.isFall = isFallTracker;
                currentSemester.totalCreditHours = 0;
                int currentClassIndex = 0;
                currentSemester.classes = new List<Course>();

                while (currentSemester.totalCreditHours < targetHours && currentClassIndex < globalVars.remainingCourseList.Count)
                {
                    if (clearsChecks(currentSemester, globalVars.remainingCourseList[currentClassIndex]))
                    {
                        currentSemester.classes.Add(globalVars.remainingCourseList[currentClassIndex]);
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
                globalVars.semesterList.Add(currentSemester);
                isFallTracker = !isFallTracker;
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
            if (currentCourse.creditHours + currentSemester.totalCreditHours > 18)
            {
                return false;
            }
            if (!(currentCourse.prerequisites == null || currentSemester.classes.Count == 0))
            {
                foreach (Course prereq in currentCourse.prerequisites)
                {
                    if (currentSemester.classes.Contains(prereq) || globalVars.remainingCourseList.Contains(prereq))
                    {
                        return false;
                    }
                }
            }
            if (currentSemester.isFall)
            {
                return currentCourse.fall;
            }
            else //spring semester
            {
                return currentCourse.spring;
            }
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
                    return (x.priority[0].CompareTo(y.priority[0])) * -1;
                }
                else if (x.priority[1].CompareTo(y.priority[1]) != 0)
                {
                    return (x.priority[1].CompareTo(y.priority[1])) * -1;
                }
                else if (x.priority[2].CompareTo(y.priority[2]) != 0)
                {
                    return (x.priority[2].CompareTo(y.priority[2])) * -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
