using Ashen.EquationSystem;
using Ashen.ToolSystem;

public class Faculty : A_EnumSO<Faculty, Faculties>, I_EquationAttribute<FacultyTool, Faculty, bool>
{
    public float GetAsFloat(bool value)
    {
        return value ? 1 : 0;
    }
}
