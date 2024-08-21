using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class FacultyTool : A_ShiftableCacheTool<FacultyTool, FacultyToolConfiguration, Faculty, bool, bool, bool>
    {
        public bool Can(Faculty faculty)
        {
            return Get(faculty);
        }

        protected override int GetEnumListSize()
        {
            return Faculties.Count;
        }

        protected override IEnumerator<Faculty> GetEnumeratorInternal()
        {
            return Faculties.Instance.GetEnumerator();
        }

        public override A_Shiftable<bool, bool, bool> GenerateShiftableValues()
        {
            SingleShiftableAttribute<bool> shiftable = new(GetEnumListSize());
            FacultyToolConfiguration config = Config;
            List<Faculty> disabled = config.DefaultDisabledFaculties;

            foreach (Faculty faculty in Faculties.Instance)
            {
                shiftable.SetDefault((int)faculty, !disabled.Contains(faculty));
            }

            return shiftable;
        }
    }
}