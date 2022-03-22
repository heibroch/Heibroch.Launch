using System.Collections.Generic;
using System.Linq;

namespace Heibroch.Launch
{
    public class SelectionCycler
    {
        public SelectionCycler() => Delta = Constants.MaxResultCount;

        private int delta;
        public int Delta
        {
            get => delta;
            set
            {
                StartIndex = 0;
                StopIndex = delta = value;
            }
        }

        public int StartIndex { get; set; }

        public int StopIndex { get; set; }

        public int CurrentIndex { get; set; }

        public int CycleIndex { get; set; }

        public void Increment(int increment, int collectionCount)
        {
            //If it has reached the min limit, then do nothing
            if (CurrentIndex + increment < 0)
                return;

            //If it has reached the max limit, then do nothing
            if (CurrentIndex + increment >= (collectionCount > Delta ? collectionCount : Delta))
                return;

            CurrentIndex += increment;
            CycleIndex += increment;

            if (CycleIndex <= 0)
            {
                CycleIndex = 0;

                if (collectionCount > Delta && (StartIndex + increment) >= 0)
                {
                    StartIndex += increment;
                    StopIndex += increment;
                }
            }

            if (CycleIndex >= Delta || CycleIndex >= collectionCount)
            {
                CycleIndex = (collectionCount < Delta ? collectionCount : Delta) - 1;

                if (collectionCount > Delta)
                {
                    StartIndex += increment;
                    StopIndex += increment;
                }
            }
        }

        public void Reset()
        {
            StartIndex = 0;
            StopIndex = delta;
            CurrentIndex = 0;
            CycleIndex = 0;
        }

        public IEnumerable<T> SubSelect<T>(IEnumerable<T> collection) => collection.Skip(StartIndex).Take(delta).ToList();
    }
}
