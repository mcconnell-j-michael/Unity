using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Ashen.EquationSystem;
using Ashen.VariableSystem;

namespace Ashen.DeliverySystem
{
    [SerializeField]
    public class Description
    {
        private static readonly Regex argumentRegex = new Regex("<([^>]*)>");

        [SerializeField, MultiLineProperty(10)]
        public string description;
        [SerializeField]
        public List<Reference<I_Equation>> equationArguments;

        [SerializeField]
        public DescriptionArgumentTypes types;

        private List<DescriptionArgument> parsedDescription;
        private StringBuilder stringBuilder;
        private bool initialized = false;

        private void OnEnable()
        {
            stringBuilder = new StringBuilder();
            parsedDescription = new List<DescriptionArgument>();
            if (description == null)
            {
                description = "";
            }
            if (equationArguments == null)
            {
                equationArguments = new List<Reference<I_Equation>>();
            }
        }

        [Button]
        public void ParseDescription()
        {
            if (!initialized)
            {
                OnEnable();
                initialized = true;
            }
            parsedDescription.Clear();
            int equationIndex = 0;
            int lastIndex = 0;
            foreach (Match match in argumentRegex.Matches(description))
            {
                string argument = match.Groups[1].ToString();
                if (match.Index - lastIndex != 0)
                {
                    string last = description.Substring(lastIndex, match.Index - lastIndex);
                    DescriptionArgument stringDa = new DescriptionArgument
                    {
                        argument = last,
                        descriptionArgumentType = DescriptionArgumentTypes.Instance.STRING
                    };
                    parsedDescription.Add(stringDa);
                }
                lastIndex = match.Length + match.Index;
                DescriptionArgument da = new DescriptionArgument();
                if (equationArguments == null || equationIndex >= equationArguments.Count)
                {
                    da.argument = "!NO_EQUATION";
                }
                else
                {
                    da.argument = argument;
                    da.equation = equationArguments[equationIndex].Value;
                    equationIndex++;

                    if (argument.Equals(types.EQUATION_RANGE.argument))
                    {
                        da.descriptionArgumentType = types.EQUATION_RANGE;
                    }
                    else if (argument.Equals(types.EQUATION_AVERAGE.argument))
                    {
                        da.descriptionArgumentType = types.EQUATION_AVERAGE;
                    }
                    else if (argument.Equals(types.EQUATION_DEFINITION.argument))
                    {
                        da.descriptionArgumentType = types.EQUATION_DEFINITION;
                    }
                }
                parsedDescription.Add(da);
            }
            if (lastIndex < description.Length)
            {
                string remaining = description.Substring(lastIndex);
                DescriptionArgument stringDa = new DescriptionArgument
                {
                    argument = remaining,
                    descriptionArgumentType = DescriptionArgumentTypes.Instance.STRING
                };
                parsedDescription.Add(stringDa);
            }
        }

        [Button]
        public void Print(I_DeliveryTool deliveryTool)
        {
            Logger.DebugLog(BuildString(deliveryTool));
        }

        public string BuildString(I_DeliveryTool deliveryTool)
        {
            if (!initialized)
            {
                ParseDescription();
            }
            stringBuilder.Remove(0, stringBuilder.Length);
            for (int x = 0; x < parsedDescription.Count; x++)
            {
                stringBuilder.Append(parsedDescription[x].GetString(deliveryTool, null));
            }
            return stringBuilder.ToString();
        }
    }
}