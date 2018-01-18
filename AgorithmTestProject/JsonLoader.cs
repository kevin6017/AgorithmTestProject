using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace AgorithmTestProject
{
    class JsonLoader
    {


        public List<Course> loadCourseList(string filepath)
        {
            using (StreamReader r = new StreamReader(filepath)) { 
                string json = r.ReadToEnd();
                var jsonObject = JsonConvert.DeserializeObject<List<Course>>(json);
                return jsonObject;
            }
        }
    
    public List<Semester> loadScheduleList(string filepath)
    {
            using (StreamReader r = new StreamReader(filepath))
            {
                string json = r.ReadToEnd();
                var jsonObject = JsonConvert.DeserializeObject<List<Semester>>(json);
                return jsonObject;
            }
    }
}
}
