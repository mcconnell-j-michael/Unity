using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine;

public class FacultyToolConfiguration : A_Configuration<FacultyTool, FacultyToolConfiguration>
{
    [SerializeField]
    private List<Faculty> defaultDisabledFaculties;

    public List<Faculty> DefaultDisabledFaculties
    {
        get
        {
            List<Faculty> disabledFaculties = new List<Faculty>();
            if (disabledFaculties != null)
            {
                disabledFaculties.AddRange(defaultDisabledFaculties);
            }
            else if (GetDefault() != this)
            {
                disabledFaculties.AddRange(GetDefault().defaultDisabledFaculties);
            }
            return disabledFaculties;
        }
    }
}

