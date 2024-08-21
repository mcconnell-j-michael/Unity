using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.ToolSystem
{
    public class PlayerInfoTool : A_EnumeratedTool<PlayerInfoTool>
    {
        [ShowInInspector]
        private List<I_Focusable> focusables;
        private int focusIndex;
        private I_Focusable focus;
        private int frameCount;

        public Text text;
        public int updateRate = 100;

        private void Start()
        {
            focus = null;
            frameCount = 0;
            focusables = new List<I_Focusable>();
            focusIndex = -1;
        }

        private void Update()
        {
            if (focus == null)
            {
                return;
            }
            if (frameCount == updateRate)
            {
                text.text = focus.BuildString(toolManager);
                frameCount = 0;
            }
            else
            {
                frameCount++;
            }
            if (focusables.Count > 1)
            {
                if (Input.GetKeyUp(KeyCode.N))
                {
                    SetFocus(focusables[((focusIndex + 1) % focusables.Count)]);
                }
            }
            if (!focus.HandleFocus(toolManager))
            {
                RemoveFocus(focus);
            }
        }

        public void SetFocus(I_Focusable focus)
        {
            if (this.focus == focus)
            {
                return;
            }
            this.focus = focus;
            text.text = focus.BuildString(toolManager);
            text.enabled = true;
            frameCount = 0;
            if (!focusables.Contains(focus))
            {
                focusables.Add(focus);
                focusIndex = focusables.Count - 1;
            }
            else
            {
                focusIndex = focusables.IndexOf(focus);
            }
        }

        public void RemoveFocus(I_Focusable focus)
        {
            if (this.focus == focus)
            {
                this.focus = null;
                frameCount = 0;
                text.text = "";
                text.enabled = false;
                focusIndex = -1;
            }
            focusables.Remove(focus);
            if (focus == null)
            {
                if (focusables.Count != 0)
                {
                    SetFocus(focusables[0]);
                }
            }
            else
            {
                focusIndex = focusables.IndexOf(this.focus);
            }
        }

        public I_Focusable GetFocusable()
        {
            return focus;
        }
    }
}