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

        public string[] classes { get; set; }

        public int totalCreditHours { get; internal set; }
    }
}
