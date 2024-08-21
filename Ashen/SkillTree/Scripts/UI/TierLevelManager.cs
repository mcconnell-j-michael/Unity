using TMPro;
using UnityEngine;

namespace Ashen.SkillTreeSystem
{
    public class TierLevelManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI tierChange;

        public void SetTierChange(int tier)
        {
            tierChange.text = tier + "";
        }
    }
}