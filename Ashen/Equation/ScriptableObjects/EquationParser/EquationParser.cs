using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(EquationParser), menuName = "Custom/EquationParser/" + nameof(EquationParser))]
    public class EquationParser : SingletonScriptableObject<EquationParser>
    {
        public List<I_ComponentParser> parsers;

        public List<I_EquationComponent> ParseEquationString(string equationString, out List<string> invalidReason)
        {
            invalidReason = new List<string>();
            if (equationString == null)
            {
                invalidReason.Add("equationString cannot be null");
                return null;
            }
            List<I_EquationComponent> components = new List<I_EquationComponent>();
            string next = "";
            int startIndex = 0;
            EquationType? type = null;
            equationString = equationString.ToLower();
            while (startIndex < equationString.Length)
            {
                for (int x = startIndex; x < equationString.Length; x++)
                {
                    startIndex = x;
                    char c = equationString[x];
                    if (type == null)
                    {
                        type = GetEquationType(c);
                        next = c + "";
                    }
                    else if (type == GetEquationType(c) && type != EquationType.symbol)
                    {
                        next += c;
                    }
                    else
                    {
                        I_EquationComponent component = FindComponent(next, (EquationType)type);
                        if (component == null)
                        {
                            invalidReason.Add("Could not parse: '" + next + "'");
                            component = new UnknownValue(next);
                        }
                        components.Add(component);
                        type = null;
                        next = "";
                        break;
                    }
                    if (x == equationString.Length - 1)
                    {
                        I_EquationComponent component = FindComponent(next, (EquationType)type);
                        if (component == null)
                        {
                            invalidReason.Add("Could not parse: '" + next + "'");
                            component = new UnknownValue(next);
                        }
                        components.Add(component);
                        startIndex = equationString.Length;
                    }
                }
            }
            return components;
        }

        private I_EquationComponent FindComponent(string toParse, EquationType equationType)
        {
            if (equationType == EquationType.number)
            {
                try
                {
                    float value = float.Parse(toParse);
                    BasicValue bv = new BasicValue
                    {
                        value = value
                    };
                    return bv;
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            else if (equationType == EquationType.word || equationType == EquationType.symbol)
            {
                if (parsers != null)
                {
                    foreach (I_ComponentParser parser in parsers)
                    {
                        I_EquationComponent component = parser.GetEquationComponent(toParse);
                        if (component != null)
                        {
                            return component;
                        }
                    }
                }
            }
            return null;
        }

        private EquationType GetEquationType(char c)
        {
            if (char.IsLetter(c) || c == '_')
            {
                return EquationType.word;
            }
            else if (char.IsNumber(c) || c == '.')
            {
                return EquationType.number;
            }
            else
            {
                return EquationType.symbol;
            }
        }

        private enum EquationType
        {
            word, number, symbol
        }
    }
}