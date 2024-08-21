using System;
using System.Collections;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public abstract class A_InputState : I_GameState
    {
        private I_GameState waitForState;
        protected GameStateRequest request;
        protected GameStateResponse response;

        private float[] delayGroups = new float[Enum.GetNames(typeof(DelayGroup)).Length];

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            response.nextState = null;
            this.request = request;
            this.response = response;

            for (int x = 0; x < delayGroups.Length; x++)
            {
                delayGroups[x] = 0.05f;
            }

            yield return PreProcessState();

            RegisterInputManager();

            float lastTime = Time.unscaledTime;

            while (response.nextState == null)
            {
                yield return null;
                if (waitForState != null)
                {
                    UnRegisterInputManager();
                    yield return waitForState.RunState(request, response);
                    RegisterInputManager();
                    waitForState = null;
                    lastTime = Time.unscaledTime;
                }
                for (int x = 0; x < delayGroups.Length; x++)
                {
                    if (delayGroups[x] > 0f)
                    {
                        delayGroups[x] -= (Time.unscaledTime - lastTime);
                        if (delayGroups[x] < 0)
                        {
                            OnSelectDelayFinished((DelayGroup)x);
                        }
                    }
                }
                lastTime = Time.unscaledTime;
                InternalRunstate();
            }

            if (waitForState != null)
            {
                UnRegisterInputManager();
                yield return waitForState.RunState(request, response);
                RegisterInputManager();
                waitForState = null;
            }

            UnRegisterInputManager();

            PostProcessState();
        }

        protected void SetSelectDelay(float delay, DelayGroup delayGroup = DelayGroup.GROUP_ONE)
        {
            delayGroups[(int)delayGroup] = delay;
        }

        protected bool IsDelayed(DelayGroup delayGroup = DelayGroup.GROUP_ONE)
        {
            return delayGroups[(int)delayGroup] > 0f;
        }

        protected virtual void OnSelectDelayFinished(DelayGroup delayGroup) { }

        protected void WaitForState(I_GameState state)
        {
            waitForState = state;
        }

        protected virtual IEnumerator PreProcessState()
        {
            yield break;
        }
        protected virtual void InternalRunstate() { }
        protected virtual void PostProcessState() { }

        protected abstract void RegisterInputManager();
        protected abstract void UnRegisterInputManager();
    }

    public enum DelayGroup
    {
        GROUP_ONE, GROUP_TWO, GROUP_THREE
    }
}