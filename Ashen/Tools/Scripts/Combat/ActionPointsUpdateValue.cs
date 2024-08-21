using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ActionPointsUpdateValue
    {
        public List<ActionPointUpdateValue> actionPointUpdates;
        public int maxResourceValue;
        public int currentResourceValue;
        public int preview;
        public bool previewValid;

        public ActionPointUpdateValue Get(int index)
        {
            if (actionPointUpdates == null)
            {
                return null;
            }
            int count = 0;
            for (int x = 0; x < actionPointUpdates.Count; x++)
            {
                count += actionPointUpdates[x].actionPointCost;
                if (index < count)
                {
                    return actionPointUpdates[x];
                }
            }
            return null;
        }
    }
}