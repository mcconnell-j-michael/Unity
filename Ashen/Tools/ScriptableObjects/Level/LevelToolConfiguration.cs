using Sirenix.Serialization;
using UnityEngine;

namespace Ashen.ToolSystem
{
    [CreateAssetMenu(fileName = "LevelToolConfiguration", menuName = "Custom/Tool/LevelToolConfiguration")]
    public class LevelToolConfiguration : A_Configuration<LevelTool, LevelToolConfiguration>
    {
        [OdinSerialize]
        private ResourceValue experienceResourceValue = default;
        [OdinSerialize]
        private BaseAttribute levelBaseAttribute = default;

        public ResourceValue ExperienceResourceValue
        {
            get
            {
                if (experienceResourceValue != null)
                {
                    return experienceResourceValue;
                }
                return GetDefault().experienceResourceValue;
            }
        }

        public BaseAttribute LevelBaseAttribute
        {
            get
            {
                if (levelBaseAttribute != null)
                {
                    return levelBaseAttribute;
                }
                return GetDefault().levelBaseAttribute;
            }
        }
    }
}