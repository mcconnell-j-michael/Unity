using Sirenix.OdinInspector;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Ashen.GameManagerWindow
{
    public abstract class A_GameManagerUnityObjectOption : A_GameManagerOption
    {
        [NonSerialized]
        public string sourcePath;

        [OnValueChanged(nameof(OnSourcePathChanged))]
        [ClassTypeContraint(typeof(UnityEngine.Object))]
        [HideReferenceObjectPicker]
        public TypeReference sourceType;

        private static Dictionary<Type, string> typeToFile = new Dictionary<Type, string>();
        
        public virtual void OnSourcePathChanged()
        {
            if (this.sourceType == null || this.sourceType.Type == null)
            {
                sourcePath = "Assets/";
                return;
            }
            this.sourcePath = FindFilePath();
        }

        public string FindFilePath()
        {
            if (typeToFile.TryGetValue(sourceType.Type, out string path))
            {
                int startIndex = path.LastIndexOf('/') + 1;
                int endIndex = path.LastIndexOf('.');
                int length = endIndex - startIndex;
                string sourceTypeString = path.Substring(startIndex, length);
                string fileText = File.ReadAllText(path);
                string nameSpaceRegex = "(?<=namespace )[^\\s]*";
                MatchCollection mc = Regex.Matches(fileText, nameSpaceRegex);
                if (mc.Count > 0)
                {
                    string foundNamespace = mc[0].ToString();
                    sourceTypeString = foundNamespace + "." + sourceTypeString;
                }
                Type type;
                type = Type.GetType(sourceTypeString);
                if (type == null)
                {
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = a.GetType(sourceTypeString);
                        if (type != null)
                        {
                            if (sourceType.Type == type)
                            {
                                return path;
                            }
                        }
                            
                    }
                }
            }
            string pathToReturn = null;
            string[] files = Directory.GetFiles("Assets", "*.cs*", SearchOption.AllDirectories);
            foreach (string filePathInitial in files)
            {
                string filePath = filePathInitial.Replace('\\', '/');
                int startIndex = filePath.LastIndexOf('/') + 1;
                int endIndex = filePath.LastIndexOf('.');
                int length = endIndex - startIndex;
                string sourceTypeString = filePath.Substring(startIndex, length);
                string fileText = File.ReadAllText(filePath);
                string nameSpaceRegex = "(?<=namespace )[^\\s]*";
                MatchCollection mc = Regex.Matches(fileText, nameSpaceRegex);
                if (mc.Count > 0)
                {
                    string foundNamespace = mc[0].ToString();
                    sourceTypeString = foundNamespace + "." + sourceTypeString;
                }
                Type type;
                type = Type.GetType(sourceTypeString);
                if (type == null)
                {
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = a.GetType(sourceTypeString);
                        if (type != null)
                        {
                            if (typeToFile.ContainsKey(type))
                            {
                                typeToFile[type] = filePath;
                            }
                            else
                            {
                                typeToFile.Add(type, filePath);
                            }
                            if (type == sourceType.Type)
                            {
                                pathToReturn = filePath;
                            }
                        }
                    }
                }
                else
                {
                    if (type == sourceType.Type)
                    {
                        pathToReturn = filePath;
                    }
                }
            }
            return pathToReturn;
        }

        public void Copy(A_GameManagerUnityObjectOption option)
        {
            option.sourcePath = sourcePath;
            option.sourceType = sourceType;
        }

        public Type FindType()
        {
            if (sourcePath == null)
            {
                return null;
            }
            int startIndex = sourcePath.LastIndexOf('/') + 1;
            int endIndex = sourcePath.LastIndexOf('.');
            int length = endIndex - startIndex;
            string sourceType = sourcePath.Substring(startIndex, length);
            string fileText = File.ReadAllText(sourcePath);
            string nameSpaceRegex = "(?<=namespace )[^\\s]*";
            MatchCollection mc = Regex.Matches(fileText, nameSpaceRegex);
            if (mc.Count > 0)
            {
                string foundNamespace = mc[0].ToString();
                sourceType = foundNamespace + "." + sourceType;
            }
            Type type;
            type = Type.GetType(sourceType);
            if (type == null)
            {
                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = a.GetType(sourceType);
                    if (type != null)
                        return type;
                }
            }
            return type;
        }
    }
}