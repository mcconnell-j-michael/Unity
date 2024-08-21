using UnityEngine;
using System.Collections.Generic;

namespace Ashen.GameManagerWindow
{
    public class SubManagerGameManagerOption : A_GameManagerOption
    {
        [HideInInspector]
        public DrawSubManager subManager;

        public GameManagerStateCollection collection;
        public string title;
        public string subtitle;

        public override void AddTarget(List<object> targets)
        {
            targets.Add(subManager);
        }

        public override GameManagerOptionType GetGameManagerOptionType()
        {
            return GameManagerOptionType.SubGameManager;
        }

        public override void Initialize()
        {
            subManager = new DrawSubManager(collection, title, subtitle);
        }

        public override void OnSelected()
        {
            subManager.OnSelected();
        }

        public override void OnDeselected()
        {
            subManager.OnDeselected();
        }

        public void OnClose()
        {

        }

        public override string GetName()
        {
            return this.title;
        }

        public override A_GameManagerOption Clone(GameManagerState state)
        {
            SubManagerGameManagerOption subManagerGameManagerOption = new SubManagerGameManagerOption();
            subManagerGameManagerOption.state = state;
            subManagerGameManagerOption.collection = collection;
            subManagerGameManagerOption.title = title;
            subManagerGameManagerOption.subtitle = subtitle;
            subManagerGameManagerOption.subManager = new DrawSubManager(collection, title, subtitle);
            return subManagerGameManagerOption;
        }
    }
}