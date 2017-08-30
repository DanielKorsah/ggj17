using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestWavelength
{
    [TestClass]
    public class WorldTests
    {
        [TestMethod]
        public void AddGridTest()
        {
            Grid grid = new Grid();
            World world = new World();

            world.AddGrid(grid, 0, 0);
        }
    }
}
