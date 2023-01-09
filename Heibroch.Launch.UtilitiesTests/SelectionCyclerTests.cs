using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heibroch.Launch.UtilitiesTests
{
    public class SelectionCyclerTests
    {
        [Fact]
        public void GivenACollectionOf4WithACylingWindowDeltaOf4_OnCyling_ThenValidateIndexCycling()
        {
            var startDelta = 4;
            var target = new SelectionCycler(startDelta); //Constants.MaxResultCount
            Assert.Equal(startDelta, target.CylingWindowSize);

            //Initial position
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(0, target.CyclingWindowCurrentIndex);
            Assert.Equal(0, target.FullCollectionCurrentIndex);

            //After one increment
            target.Increment(1, startDelta);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(1, target.CyclingWindowCurrentIndex);
            Assert.Equal(1, target.FullCollectionCurrentIndex);

            //After second increment
            target.Increment(1, startDelta);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(2, target.CyclingWindowCurrentIndex);
            Assert.Equal(2, target.FullCollectionCurrentIndex);

            //After third increment. This is last position
            target.Increment(1, startDelta);            
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);
            Assert.Equal(3, target.FullCollectionCurrentIndex);

            //It should no longer increment as we have reached max
            target.Increment(1, startDelta);            
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);
            Assert.Equal(3, target.FullCollectionCurrentIndex);

            target.Increment(-1, startDelta);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(2, target.CyclingWindowCurrentIndex);
            Assert.Equal(2, target.FullCollectionCurrentIndex);

            target.Increment(-1, startDelta);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(1, target.CyclingWindowCurrentIndex);
            Assert.Equal(1, target.FullCollectionCurrentIndex);

            target.Increment(-1, startDelta);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(0, target.CyclingWindowCurrentIndex);
            Assert.Equal(0, target.FullCollectionCurrentIndex);

            //It should no move position, because it's the first position
            target.Increment(-1, startDelta);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(target.CylingWindowSize, target.FullCollectionStopIndex);
            Assert.Equal(0, target.CyclingWindowCurrentIndex);
            Assert.Equal(0, target.FullCollectionCurrentIndex);
        }

        [Fact]
        public void GivenACollectionOf4WithACollectionSizeOf10_OnCyling_ThenValidateIndexCycling()
        {
            var target = new SelectionCycler(4); //Constants.MaxResultCount
            Assert.Equal(4, target.CylingWindowSize);

            //Initial position
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(4, target.FullCollectionStopIndex);
            Assert.Equal(0, target.FullCollectionCurrentIndex);
            Assert.Equal(0, target.CyclingWindowCurrentIndex);            

            //After one increment
            target.Increment(1, 10);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(4, target.FullCollectionStopIndex);
            Assert.Equal(1, target.FullCollectionCurrentIndex);
            Assert.Equal(1, target.CyclingWindowCurrentIndex);

            //After second increment
            target.Increment(1, 10);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(4, target.FullCollectionStopIndex);
            Assert.Equal(2, target.FullCollectionCurrentIndex);
            Assert.Equal(2, target.CyclingWindowCurrentIndex);

            target.Increment(1, 10);
            Assert.Equal(0, target.FullCollectionStartIndex);
            Assert.Equal(4, target.FullCollectionStopIndex);
            Assert.Equal(3, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);

            //Edge of cycling window reached, so we now move through the collection
            target.Increment(1, 10);
            Assert.Equal(1, target.FullCollectionStartIndex);
            Assert.Equal(5, target.FullCollectionStopIndex);
            Assert.Equal(5, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);

            target.Increment(1, 10);
            Assert.Equal(2, target.FullCollectionStartIndex);
            Assert.Equal(6, target.FullCollectionStopIndex);
            Assert.Equal(6, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);

            target.Increment(1, 10);
            Assert.Equal(3, target.FullCollectionStartIndex);
            Assert.Equal(7, target.FullCollectionStopIndex);
            Assert.Equal(7, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);

            target.Increment(1, 10);
            Assert.Equal(4, target.FullCollectionStartIndex);
            Assert.Equal(8, target.FullCollectionStopIndex);
            Assert.Equal(8, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);

            target.Increment(1, 10);
            Assert.Equal(5, target.FullCollectionStartIndex);
            Assert.Equal(9, target.FullCollectionStopIndex);
            Assert.Equal(9, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);

            //We have reached the max, and we are therefore not incrementing anything further
            target.Increment(1, 10);
            Assert.Equal(5, target.FullCollectionStartIndex);
            Assert.Equal(9, target.FullCollectionStopIndex);
            Assert.Equal(9, target.FullCollectionCurrentIndex);
            Assert.Equal(3, target.CyclingWindowCurrentIndex);
        }
    }
}
