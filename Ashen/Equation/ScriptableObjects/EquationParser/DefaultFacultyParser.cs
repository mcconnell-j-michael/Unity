using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    public class DefaultFacultyParser : A_DefaultComponentParser
    {
        protected override I_EquationComponent GetEquationComponentInternal(string toParse, List<string> arguments)
        {
            foreach (Faculty faculty in Faculties.Instance)
            {
                if (faculty.name.ToLower().Equals(toParse.ToLower()))
                {
                    FacultyValue facultyValue = new FacultyValue
                    {
                        enumSO = faculty
                    };
                    if (arguments.Contains("t"))
                    {
                        facultyValue.useTarget = true;
                    }
                    if (arguments.Contains("!"))
                    {
                        facultyValue.inverse = true;
                    }
                    return facultyValue;
                }
            }
            return null;
        }

        private readonly List<string> validArguments = new() { "t", "!" };
        protected override List<string> GetValidArguments()
        {
            return validArguments;
        }

        protected override bool StringValidInternal(string toParse)
        {
            foreach (Faculty faculty in Faculties.Instance)
            {
                if (faculty.name.ToLower().Equals(toParse.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}