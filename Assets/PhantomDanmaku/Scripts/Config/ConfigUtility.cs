using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PhantomDanmaku.Config
{
    public class ConfigUtility
    {
        public static List<Type> FindAllTypes<T>()
        {
            List<Type> scriptableObjectTypes = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if ((type.IsSubclassOf(typeof(T)) || type == typeof(T)) && !type.IsAbstract)
                    {
                        scriptableObjectTypes.Add(type);
                    }
                }
            }

            return scriptableObjectTypes;
        }
        
        public static string GetRelativePath(string absolutePath)
        {
            string projectPath = Application.dataPath + "Assets";
            projectPath = projectPath.Substring(0, projectPath.Length - "Assets".Length);
            var regularPath = Utility.Path.GetRegularPath(absolutePath);
            return regularPath.Replace(projectPath, "Assets");
        }
    }
}