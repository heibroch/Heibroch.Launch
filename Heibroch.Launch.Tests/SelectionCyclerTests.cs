//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Heibroch.Launch.Tests
//{
//    [TestClass]
//    public class SelectionCyclerTests
//    {
//        [TestMethod]
//        public void TestCycling()
//        {
//            var target = new SelectionCycler();

//            Assert.AreEqual(4, target.Delta);

//            target.Increment(1, 10);

//            Assert.AreEqual(0, target.StartIndex);
//            Assert.AreEqual(target.Delta, target.StopIndex);
//            Assert.AreEqual(1, target.CycleIndex);
//            Assert.AreEqual(1, target.CurrentIndex);

//            target.Increment(1);

//            Assert.AreEqual(0, target.StartIndex);
//            Assert.AreEqual(target.Delta, target.StopIndex);
//            Assert.AreEqual(2, target.CycleIndex);
//            Assert.AreEqual(2, target.CurrentIndex);

//            target.Increment(1);

//            Assert.AreEqual(0, target.StartIndex);
//            Assert.AreEqual(target.Delta, target.StopIndex);
//            Assert.AreEqual(3, target.CycleIndex);
//            Assert.AreEqual(3, target.CurrentIndex);

//            target.Increment(1);

//            Assert.AreEqual(0, target.StartIndex);
//            Assert.AreEqual(target.Delta, target.StopIndex);
//            Assert.AreEqual(4, target.CycleIndex);
//            Assert.AreEqual(4, target.CurrentIndex);

//            target.Increment(1);

//            Assert.AreEqual(1, target.StartIndex);
//            Assert.AreEqual(5, target.StopIndex);
//            Assert.AreEqual(4, target.CycleIndex);
//            Assert.AreEqual(5, target.CurrentIndex);

//            target.Increment(1);

//            Assert.AreEqual(2, target.StartIndex);
//            Assert.AreEqual(6, target.StopIndex);
//            Assert.AreEqual(4, target.CycleIndex);
//            Assert.AreEqual(6, target.CurrentIndex);

//            target.Increment(1);

//            Assert.AreEqual(3, target.StartIndex);
//            Assert.AreEqual(7, target.StopIndex);
//            Assert.AreEqual(4, target.CycleIndex);
//            Assert.AreEqual(7, target.CurrentIndex);

//            target.Increment(-1);

//            Assert.AreEqual(3, target.StartIndex);
//            Assert.AreEqual(7, target.StopIndex);
//            Assert.AreEqual(3, target.CycleIndex);
//            Assert.AreEqual(6, target.CurrentIndex);

//            target.Increment(-1);

//            Assert.AreEqual(3, target.StartIndex);
//            Assert.AreEqual(7, target.StopIndex);
//            Assert.AreEqual(2, target.CycleIndex);
//            Assert.AreEqual(5, target.CurrentIndex);

//            target.Increment(-1);

//            Assert.AreEqual(3, target.StartIndex);
//            Assert.AreEqual(7, target.StopIndex);
//            Assert.AreEqual(1, target.CycleIndex);
//            Assert.AreEqual(4, target.CurrentIndex);

//            target.Increment(-1);

//            Assert.AreEqual(3, target.StartIndex);
//            Assert.AreEqual(7, target.StopIndex);
//            Assert.AreEqual(0, target.CycleIndex);
//            Assert.AreEqual(3, target.CurrentIndex);

//            target.Increment(-1);

//            Assert.AreEqual(2, target.StartIndex);
//            Assert.AreEqual(6, target.StopIndex);
//            Assert.AreEqual(0, target.CycleIndex);
//            Assert.AreEqual(2, target.CurrentIndex);
//        }
//    }
//}
