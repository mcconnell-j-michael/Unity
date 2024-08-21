using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class ShiftPack
    {
        private static readonly int initialIndexSize = 20;
        private Dictionary<string, int> consumedShifts;

        private List<int> availableIndecies;
        private List<int> occupiedIndecies;
        private NumberShift[] numberShifts;

        private NumberShift finalShift = new NumberShift();

        public void Initialize()
        {
            availableIndecies = new List<int>(initialIndexSize);
            occupiedIndecies = new List<int>(initialIndexSize);
            consumedShifts = new Dictionary<string, int>();
            numberShifts = new NumberShift[initialIndexSize];
            for (int x = 0; x < initialIndexSize; x++)
            {
                availableIndecies.Add(x);
                numberShifts[x] = new NumberShift();
            }
        }

        public void Apply(string source, float value)
        {
            if (!consumedShifts.ContainsKey(source))
            {
                if (availableIndecies.Count == 0)
                {
                    NumberShift[] tempShifts = new NumberShift[numberShifts.Length * 2];
                    availableIndecies.Capacity = tempShifts.Length;
                    for (int x = 0; x < numberShifts.Length; x++)
                    {
                        tempShifts[x] = numberShifts[x];
                    }
                    for (int x = numberShifts.Length; x < tempShifts.Length; x++)
                    {
                        availableIndecies.Add(x);
                        tempShifts[x] = new NumberShift();
                    }
                    numberShifts = tempShifts;
                }
                int index = availableIndecies[availableIndecies.Count - 1];
                consumedShifts.Add(source, index);
                availableIndecies.RemoveAt(availableIndecies.Count - 1);
                occupiedIndecies.Add(index);
                NumberShift shiftToConsume = numberShifts[index];
                shiftToConsume.value = value;
                EnsureValues();
            }
        }

        public void Clear(string source)
        {
            if (consumedShifts.TryGetValue(source, out int index))
            {
                UnorderedListUtility<int>.RemoveAt(occupiedIndecies, occupiedIndecies.IndexOf(index));
                availableIndecies.Add(index);
                consumedShifts.Remove(source);
                EnsureValues();
            }
        }

        public float GetValue()
        {
            return finalShift.Value();
        }

        private void EnsureValues()
        {
            float finalValue = 0f;
            for (int x = 0; x < occupiedIndecies.Count; x++)
            {
                int index = occupiedIndecies[x];
                NumberShift shift = numberShifts[index];
                finalValue += shift.value;
            }
            finalShift.value = finalValue;
        }

        public ShiftPack Copy()
        {
            ShiftPack shiftPack = new ShiftPack();
            shiftPack.Initialize();
            return shiftPack;
        }
    }
}