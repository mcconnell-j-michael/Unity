using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    public class ExtendedEffectPackBuilder : I_EffectBuilder
    {
        [OdinSerialize]
        [HorizontalGroup("StatusEffect")]
        [LabelWidth(50)]
        public StatusEffectScriptableObject Copy;
        [OdinSerialize, BoxGroup(nameof(TickerPack)), HideLabel, InlineProperty]
        public I_TickerPack TickerPack;
        [OdinSerialize]
        private List<I_ExtendedEffectArgumentBuilder> argumentBuilders;
        [OdinSerialize]
        public bool save;

        public ExtendedEffectPackBuilder() { }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ExtendedEffectArgumentFiller filler = new ExtendedEffectArgumentFiller();
            if (argumentBuilders != null)
            {
                foreach (I_ExtendedEffectArgumentBuilder argumentBuilder in argumentBuilders)
                {
                    argumentBuilder.FillArguments(filler, owner, target, deliveryArguments);
                }
            }
            return new StatusEffectPack(Copy, TickerPack?.Build(owner, target, deliveryArguments), filler, save);
        }

        public override string ToString()
        {
            return Copy.ToString();
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Apply " + Copy.name;
            return vis;
        }

        public ExtendedEffectPackBuilder(SerializationInfo info, StreamingContext context)
        {
            Copy = StaticUtilities.LoadSOFromLibrary<ExtendedEffectLibrary, StatusEffectScriptableObject>(
                info,
                nameof(Copy)
            );
            TickerPack = StaticUtilities.LoadInterfaceValue<I_TickerPack>(info, nameof(TickerPack));
            argumentBuilders = StaticUtilities.LoadList(info, nameof(argumentBuilders), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_ExtendedEffectArgumentBuilder>(info, name);
            });
            save = info.GetBoolean(nameof(save));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveSOFromLibrary(info, nameof(Copy), Copy);
            StaticUtilities.SaveInterfaceValue(info, nameof(TickerPack), TickerPack);
            StaticUtilities.SaveList(info, nameof(argumentBuilders), argumentBuilders, (string name, I_ExtendedEffectArgumentBuilder builder) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, builder);
            });
            info.AddValue(nameof(save), save);
        }
    }
}