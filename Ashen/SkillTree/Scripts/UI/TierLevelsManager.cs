using UnityEngine;

namespace Ashen.SkillTreeSystem
{
    public class TierLevelsManager : MonoBehaviour
    {
        [SerializeField]
        private TierLevelManager upTier;
        [SerializeField]
        private TierLevelManager downTier;

        public void SetTier(int tier)
        {
            if (tier == 0)
            {
                upTier.gameObject.SetActive(false);
                downTier.gameObject.SetActive(false);
            }
            else if (tier > 0)
            {
                downTier.gameObject.SetActive(false);
                upTier.gameObject.SetActive(true);
                upTier.SetTierChange(tier);
            }
            else
            {
                upTier.gameObject.SetActive(false);
                downTier.gameObject.SetActive(true);
                downTier.SetTierChange(-tier);
            }
        }
    }
}