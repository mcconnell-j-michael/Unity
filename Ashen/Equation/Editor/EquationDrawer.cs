using Ashen.EquationSystem;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

public class EquationDrawer : OdinValueDrawer<Equation>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (ValueEntry.SmartValue != null && !ValueEntry.SmartValue.ValidEquation)
        {
            if (ValueEntry.SmartValue.ParseReason != null)
            {
                SirenixEditorGUI.ErrorMessageBox(ValueEntry.SmartValue.ParseReason);
            }
            else
            {
                if (ValueEntry.SmartValue.Reason == null)
                {
                    ValueEntry.SmartValue.VerifyEquation();
                }
                SirenixEditorGUI.ErrorMessageBox(ValueEntry.SmartValue.Reason);
            }
        }
        CallNextDrawer(label);
    }
}

//[DrawerPriority(0, 0, 1)]
//public class EquationInterfaceDrawer<T> : OdinValueDrawer<T> where T : I_Equation
//{
//    protected override void DrawPropertyLayout(GUIContent label)
//    {
//        if (ValueEntry.SmartValue == null)
//        {
//            ValueEntry.SmartValue = (T)(new Equation() as I_Equation);
//        }
//        CallNextDrawer(label);
//    }
//}

public class EquationReferenceDrawer : OdinValueDrawer<Equation>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (ValueEntry.SmartValue == null)
        {
            ValueEntry.SmartValue = new Equation();
        }
        CallNextDrawer(label);
    }
}