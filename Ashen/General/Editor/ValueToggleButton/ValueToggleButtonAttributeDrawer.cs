using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ValueToggleButtonAttributeDrawer<T> : OdinAttributeDrawer<ValueToggleButtonAttribute, T>
{
    private ValueResolver<object> rawGetter;
    private string error;
    private Func<IEnumerable<ValueDropdownItem<T>>> getValues;
    private int initial;

    private Context context;

    private class Context
    {

        public GUIContent[] Names;
        public T[] Values;
        public float[] NameSizes;
        public List<int> ColumnCounts;
        public float PreviousControlRectWidth;
    }

    protected override void Initialize()
    {
        initial = 0;
        this.rawGetter = ValueResolver.Get<object>(this.Property, this.Attribute.MemberName);
        this.error = this.rawGetter.ErrorMessage;
        this.getValues = () =>
        {
            object value = this.rawGetter.GetValue();

            if (value is IEnumerable enumerable)
            {
                return enumerable
                    .Cast<object>()
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        if (x is ValueDropdownItem<T> item)
                        {
                            return item;
                        }

                        if (x is IValueDropdownItem ix)
                        {
                            return new ValueDropdownItem<T>(ix.GetText(), (T)ix.GetValue());
                        }

                        if (x is UnityEngine.Object uo)
                        {
                            return new ValueDropdownItem<T>(uo.name, (T)x);
                        }

                        return new ValueDropdownItem<T>(x.ToString(), (T)x);
                    });
            }
            return null;

        };
    }

    private Context BuildContext()
    {
        IEnumerable<ValueDropdownItem<T>> valueDropdownItems = this.getValues();
        List<string> enumNames = new List<string>();
        context = new Context();
        context.Values = new T[valueDropdownItems.Count()];
        context.Names = new GUIContent[valueDropdownItems.Count()];
        int i = 0;
        foreach (ValueDropdownItem<T> item in valueDropdownItems)
        {
            enumNames.Add(item.Text);
            context.Values[i] = item.Value;
            context.Names[i] = new GUIContent(item.Text);
            i++;
        }

        // Calculate the default sizes for each button
        context.NameSizes = new float[context.Names.Length];
        for (int x = 0; x < context.NameSizes.Length; x++)
        {
            GUIContent content = context.Names[x];
            context.NameSizes[x] = SirenixGUIStyles.MiniButtonMid.CalcSize(content).x + 3;
        }

        // Assume there is one row with the number of options in columns
        context.ColumnCounts = new List<int>() { context.NameSizes.Length };
        return context;
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (initial < 2)
        {
            initial++;
            base.CallNextDrawer(label);
            return;
        }

        IPropertyValueEntry<T> entry = this.ValueEntry;

        Type t = entry.WeakValues[0].GetType();

        if (Event.current.type == EventType.Layout)
        {
            if (ShouldResetContext(context))
            {
                context = BuildContext();
                GUIHelper.RequestRepaint();
            }
        }

        if (context == null)
        {
            context = BuildContext();
            GUIHelper.RequestRepaint();
        }

        T value = entry.SmartValue;

        Rect controlRect = new Rect();

        for (int j = 0, i = 0; j < context.ColumnCounts.Count; j++)
        {
            SirenixEditorGUI.GetFeatureRichControlRect(j == 0 ? label : GUIContent.none, out int id, out bool hasFocus, out Rect rect);

            if (j == 0)
            {
                controlRect = rect;
            }
            else
            {
                rect.xMin = controlRect.xMin;
            }

            float xMax = rect.xMax;
            rect.width /= context.ColumnCounts[j];
            rect.width = (int)rect.width;
            int from = i;
            int to = i + context.ColumnCounts[j];
            for (; i < to; i++)
            {
                bool selected;

                selected = context.Values[i].Equals(value);

                GUIStyle style;
                Rect btnRect = rect;
                if (i == from && i == to - 1)
                {
                    style = SirenixGUIStyles.MiniButton;
                    btnRect.x -= 1;
                    btnRect.xMax = xMax + 1;
                }
                else if (i == from)
                {
                    style = SirenixGUIStyles.MiniButtonLeft;
                }
                else if (i == to - 1)
                {
                    style = SirenixGUIStyles.MiniButtonRight;
                    btnRect.xMax = xMax;
                }
                else
                {
                    style = SirenixGUIStyles.MiniButtonMid;
                }
                if (SirenixEditorGUI.SDFIconButton(btnRect, context.Names[i], default, IconAlignment.LeftOfText, style, selected))
                {
                    GUIHelper.RemoveFocusControl();

                    entry.WeakSmartValue = context.Values[i];

                    GUIHelper.RequestRepaint();
                }

                rect.x += rect.width;
            }
        }

        if (Event.current.type == EventType.Repaint && context.PreviousControlRectWidth != controlRect.width)
        {
            context.PreviousControlRectWidth = controlRect.width;

            float maxBtnWidth = 0;
            int row = 0;
            context.ColumnCounts.Clear();
            context.ColumnCounts.Add(0);
            for (int i = 0; i < context.NameSizes.Length; i++)
            {
                float btnWidth = context.NameSizes[i];
                context.ColumnCounts[row]++;
                int columnCount = context.ColumnCounts[row];
                float columnWidth = controlRect.width / columnCount;

                maxBtnWidth = Mathf.Max(btnWidth, maxBtnWidth);

                if (maxBtnWidth > columnWidth && columnCount > 1)
                {
                    context.ColumnCounts[row]--;
                    context.ColumnCounts.Add(1);
                    row++;
                    maxBtnWidth = btnWidth;
                }
            }
        }
    }

    private bool ShouldResetContext(Context value)
    {
        if (value == null)
        {
            return true;
        }
        IEnumerable<ValueDropdownItem<T>> valueDropdownItems = this.getValues();
        int count = 0;
        foreach (ValueDropdownItem<T> item in valueDropdownItems)
        {
            if (value.Values.Length <= count || !value.Values[count].Equals(item.Value) || !value.Names[count].text.Equals(item.Text))
            {
                return true;
            }
            count++;
        }
        if (count != value.Names.Length)
        {
            return true;
        }
        return false;
    }
}