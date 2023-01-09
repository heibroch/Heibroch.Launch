namespace Heibroch.Launch
{
    public class SelectionCycler
    {
        public SelectionCycler(int cylingWindowSize) => CylingWindowSize = cylingWindowSize;

        private int cyclingWindowSize;
        public int CylingWindowSize
        {
            get => cyclingWindowSize;
            set
            {
                FullCollectionStartIndex = 0;
                FullCollectionStopIndex = cyclingWindowSize = value;
            }
        }

        public int FullCollectionStartIndex { get; set; }

        public int FullCollectionStopIndex { get; set; }

        public int FullCollectionCurrentIndex { get; set; }

        public int CyclingWindowCurrentIndex { get; set; }

        public void Increment(int increment, int collectionCount)
        {
            //If it has reached the min limit, then do nothing
            if (FullCollectionCurrentIndex + increment < 0)
                return;

            //If it has reached the max limit, then do nothing
            if (FullCollectionCurrentIndex + increment >= (collectionCount > CylingWindowSize ? collectionCount : CylingWindowSize))
                return;

            FullCollectionCurrentIndex += increment;
            CyclingWindowCurrentIndex += increment;

            if (CyclingWindowCurrentIndex <= 0)
            {
                CyclingWindowCurrentIndex = 0;

                if (collectionCount > CylingWindowSize && (FullCollectionStartIndex + increment) >= 0)
                {
                    FullCollectionStartIndex += increment;
                    FullCollectionStopIndex += increment;
                }
            }

            if (CyclingWindowCurrentIndex >= CylingWindowSize || CyclingWindowCurrentIndex >= collectionCount)
            {
                CyclingWindowCurrentIndex = (collectionCount < CylingWindowSize ? collectionCount : CylingWindowSize) - 1;

                if (collectionCount > CylingWindowSize)
                {
                    FullCollectionStartIndex += increment;
                    FullCollectionStopIndex += increment;
                }
            }
        }

        public void Reset()
        {
            FullCollectionStartIndex = 0;
            FullCollectionStopIndex = cyclingWindowSize;
            FullCollectionCurrentIndex = 0;
            CyclingWindowCurrentIndex = 0;
        }

        public IEnumerable<T> SubSelect<T>(IEnumerable<T> collection) => collection.Skip(FullCollectionStartIndex).Take(cyclingWindowSize).ToList();
    }
}
