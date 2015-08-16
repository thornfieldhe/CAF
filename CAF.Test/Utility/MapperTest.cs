// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapperTest.cs" company="">
//   
// </copyright>
// <summary>
//   The mapper test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CAF.Tests.Utility
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using CAF.Data;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The mapper test.
    /// </summary>
    [TestClass]
    public class MapperTest
    {
        /// <summary>
        /// The test_ mapper.
        /// </summary>
        [TestMethod]
        public void Test_Mapper()
        {
            var one = new One
                          {
                              X1 = "test value", 
                              X2 = 77, 
                              X3 =
                                  new Two
                                      {
                                          X11 = new Three { X21 = 99 }, 
                                          X12 = new List<Three> { new Three { X21 = 105 } }
                                      }, 
                              X4 = new List<Three> { new Three { X21 = 203 } }, 
                              X5 = new[] { new Three { X21 = 204 } }, 
                              X6 = new Collection<Three> { new Three { X21 = 205 } }, 
                              X7 = new List<Three> { new Three { X21 = 206 } }, 
                              X8 = new[] { new Three { X21 = 207 } }, 
                              X9 = new Collection<Three> { new Three { X21 = 208 } }, 
                              X10 =
                                  new Two
                                      {
                                          X11 = new Three { X21 = 301 }, 
                                          X12 = new Collection<Three> { new Three { X21 = 401 } }
                                      }
                          };

            var mapper = new Mapper<One, VMOne>().UseMapper<Two, VMTwo>().UseMapper<Three, VMThree>();
            var vmOne = mapper.Map(one);
            Assert.AreEqual(vmOne.X7.Length, 1);
            Assert.IsNull(vmOne.X10.X11);
            Assert.AreEqual(vmOne.X3.X11.X21, 99);
        }

        /// <summary>
        /// The one.
        /// </summary>
        private class One
        {
            /// <summary>
            /// Gets or sets the x 1.
            /// </summary>
            public string X1 { get; set; }

            /// <summary>
            /// Gets or sets the x 2.
            /// </summary>
            public int X2 { get; set; }

            /// <summary>
            /// Gets or sets the x 3.
            /// </summary>
            public Two X3 { get; set; }

            /// <summary>
            /// Gets or sets the x 4.
            /// </summary>
            public IList<Three> X4 { get; set; }

            /// <summary>
            /// Gets or sets the x 5.
            /// </summary>
            public Three[] X5 { get; set; }

            /// <summary>
            /// Gets or sets the x 6.
            /// </summary>
            public ICollection<Three> X6 { get; set; }

            /// <summary>
            /// Gets or sets the x 7.
            /// </summary>
            public IList<Three> X7 { get; set; }

            /// <summary>
            /// Gets or sets the x 8.
            /// </summary>
            public Three[] X8 { get; set; }

            /// <summary>
            /// Gets or sets the x 9.
            /// </summary>
            public ICollection<Three> X9 { get; set; }

            /// <summary>
            /// Gets or sets the x 10.
            /// </summary>
            public Two X10 { get; set; }
        }

        /// <summary>
        /// The vm one.
        /// </summary>
        private class VMOne
        {
            /// <summary>
            /// Gets or sets the x 1.
            /// </summary>
            public string X1 { get; set; }

            /// <summary>
            /// Gets or sets the x 2.
            /// </summary>
            public int X2 { get; set; }

            /// <summary>
            /// Gets or sets the x 3.
            /// </summary>
            public Two X3 { get; set; }

            /// <summary>
            /// Gets or sets the x 4.
            /// </summary>
            public Three[] X4 { get; set; }

            /// <summary>
            /// Gets or sets the x 5.
            /// </summary>
            public ICollection<Three> X5 { get; set; }

            /// <summary>
            /// Gets or sets the x 6.
            /// </summary>
            public IList<Three> X6 { get; set; }

            /// <summary>
            /// Gets or sets the x 7.
            /// </summary>
            public VMThree[] X7 { get; set; }

            /// <summary>
            /// Gets or sets the x 8.
            /// </summary>
            public ICollection<VMThree> X8 { get; set; }

            /// <summary>
            /// Gets or sets the x 9.
            /// </summary>
            public IList<VMThree> X9 { get; set; }

            /// <summary>
            /// Gets or sets the x 10.
            /// </summary>
            public VMTwo X10 { get; set; }
        }

        /// <summary>
        /// The two.
        /// </summary>
        private class Two
        {
            /// <summary>
            /// Gets or sets the x 11.
            /// </summary>
            public Three X11 { get; set; }

            /// <summary>
            /// Gets or sets the x 12.
            /// </summary>
            public IList<Three> X12 { get; set; }
        }

        /// <summary>
        /// The vm two.
        /// </summary>
        private class VMTwo
        {
            /// <summary>
            /// Gets or sets the x 11.
            /// </summary>
            public VMThree X11 { get; set; }

            /// <summary>
            /// Gets or sets the x 12.
            /// </summary>
            public IList<VMThree> X12 { get; set; }
        }

        /// <summary>
        /// The three.
        /// </summary>
        private class Three
        {
            /// <summary>
            /// Gets or sets the x 21.
            /// </summary>
            public int X21 { get; set; }
        }

        /// <summary>
        /// The vm three.
        /// </summary>
        private class VMThree
        {
            /// <summary>
            /// Gets or sets the x 21.
            /// </summary>
            public int X21 { get; set; }
        }
    }
}