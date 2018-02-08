using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgorithmTestProject
{
    class Semester
    {
        public string semester { get; set; }

        public int position { get; set; }

        public List<Course> classes { get; set; }

        public int totalCreditHours { get; internal set; }

        public bool isFall { get; internal set; }
    }
}
