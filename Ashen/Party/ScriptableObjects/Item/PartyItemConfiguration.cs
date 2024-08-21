using Ashen.ItemSystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.PartySystem
{
    public class PartyItemConfiguration : A_Configuration<PartyItemsManager, PartyItemConfiguration>
    {
        [OdinSerialize]
        private List<ItemSO> defaultItems;

        public List<ItemSO> DefaultItems
        {
            get
            {
                List<ItemSO> items = new();
                if (defaultItems == null)
                {
                    items.AddRange(GetDefault().defaultItems);
                    return items;
                }
                items.AddRange(defaultItems);
                return items;
            }
        }
    }
}