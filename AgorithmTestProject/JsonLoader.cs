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
        Assembly assembly;
        StreamReader streamReader;

        public List<Course> loadCourseList(string filepath)
        {
            assembly = Assembly.GetExecutingAssembly();
            streamReader = new StreamReader(assembly.GetManifestResourceStream(filepath));
            string json = streamReader.ReadToEnd();
            var jsonObject = JsonConvert.DeserializeObject<List<Course>>(json);
            return jsonObject;
        }

        public List<Semester> loadScheduleList(string filepath)
        {
            assembly = Assembly.GetExecutingAssembly();
            streamReader = new StreamReader(assembly.GetManifestResourceStream(filepath));
            string json = streamReader.ReadToEnd();
            var jsonObject = JsonConvert.DeserializeObject<List<Semester>>(json);
            return jsonObject;
        }
    }
}
