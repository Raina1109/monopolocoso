using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Assignment4.Core
{
    public sealed class LogicLoader
    {
        static readonly LogicLoader _instance = new LogicLoader();

        private List<IDetector> _detectors;

        public static LogicLoader Instance
        {
            get { return _instance; }
        }

        private LogicLoader()
        {
            _detectors = new List<IDetector>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> assemblyTypes = new List<Type>();

            assemblyTypes.AddRange(assembly.GetTypes());

            List<Type> detectorList = assemblyTypes.FindAll(delegate(Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                return interfaceTypes.Contains(typeof(IDetector));
            });

            // convert the list of Objects to an instantiated list of IDetectors
            _detectors = detectorList.ConvertAll<IDetector>(delegate(Type t) { return Activator.CreateInstance(t) as IDetector; });
        }

        public List<IDetector> Detectors
        {
            get
            {
                return _detectors;
            }
        }
    }
}
