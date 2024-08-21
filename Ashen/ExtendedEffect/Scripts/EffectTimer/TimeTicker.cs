using System;
using System.Collections;
using Ashen.ToolSystem;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A time ticker will last for a duration equal to
     * 'duratin' and will tick every 'frequency' seconds
     **/
     [Serializable]
    public class TimeTicker : I_Ticker, I_Timed
    {
        [NonSerialized]
        private I_Tickable tickable;
        private int? originalDuration;
        private int? duration;
        private int? frequency;
        private int nextTick;
        [NonSerialized]
        private bool enabled;
        [NonSerialized]
        private bool tracked;
        private bool timed;
        private bool turn;

        private TimeTicker() { }

        public TimeTicker(int? duration, int? frequency, bool turn = false)
        {
            if (frequency != null && frequency < 0)
            {
                Logger.ErrorLog("Cannot have a frequency less than 0");
            }
            originalDuration = duration;
            this.duration = duration;
            if (duration != null)
            {
                timed = true;
            }
            this.frequency = frequency;
            if (frequency != null)
            {
                nextTick = frequency.Value;
            }
            this.turn = turn;
        }

        public TimeTicker(int? duration, int? frequency, int? currentDuration, int nextTick, bool turn = false) : this(duration, frequency, turn)
        {
            this.nextTick = nextTick;
            this.duration = currentDuration;
        }

        public void Disable()
        {
            if (enabled)
            {
                enabled = false;
            }
        }

        public void Enable(I_Tickable tickable)
        {
            if (!enabled)
            {
                this.tickable = tickable;
                enabled = true;
                if (!tracked && !turn)
                {
                    TimeRegistry.AddListener(this);
                }
            }
        }

        public void Remove()
        {
            Disable();
        }

        public void Reset()
        {
            duration = originalDuration;
            if (frequency != null)
            {
                nextTick = frequency.Value;
            }
        }

        public void Reset(int? duration, int frequency)
        {
            originalDuration = duration;
            this.duration = duration;
            if (duration != null)
            {
                timed = true;
            }
            this.frequency = frequency;
            nextTick = frequency;
        }

        public void UpdateTime()
        {
            if (timed)
            {
                duration -= 1;
            }
            if (frequency != null)
            {
                nextTick -= 1;
            }
            tickable.UpdateTime();
            if (frequency != null && nextTick <= 0)
            {
                nextTick = frequency.Value;
                tickable.Tick();
            }
            if (timed && duration <= 0f)
            {
                tickable.End();
            }
        }

        public bool IsEnabled()
        {
            return enabled;
        }

        public void StopTracking()
        {
            tracked = false;
        }

        public void StartTracking()
        {
            tracked = true;
        }

        public bool IsTracked()
        {
            return tracked;
        }

        public float? TimeLeft()
        {
            return duration;
        }

        public float? TotalDuration()
        {
            return originalDuration;
        }

        public I_Ticker Duplicate()
        {
            return new TimeTicker(originalDuration, frequency, duration, nextTick, turn);
        }
    }
}
