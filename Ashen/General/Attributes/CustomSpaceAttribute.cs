using System;

namespace EditorUtilities
{
    public class CustomSpaceAttribute : Attribute
    {
        public bool spaceBefore;
        public bool spaceAfter;

        public CustomSpaceAttribute(bool spaceBefore = false, bool spaceAfter = false)
        {
            this.spaceBefore = spaceBefore;
            this.spaceAfter = spaceAfter;
        }
    }
}