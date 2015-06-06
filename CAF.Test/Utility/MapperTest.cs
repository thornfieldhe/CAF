using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Utility
{
    using CAF.Data;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void Test_Mapper()
        {

            var one = new One()
            {
                X1 = "test value",
                X2 = 77,
                X3 = new Two()
                {
                    X11 = new Three()
                    {
                        X21 = 99
                    },
                    X12 = new List<Three>()
                            {
                                new Three(){ X21 = 105 }
                            }
                },
                X4 = new List<Three>()
                        {
                            new Three(){X21=203}
                        },
                X5 = new Three[]
                        {
                            new Three(){X21=204}
                        },
                X6 = new Collection<Three>()
                        {
                            new Three(){X21=205}
                        },
                X7 = new List<Three>()
                        {
                            new Three(){X21=206}
                        },
                X8 = new Three[]
                        {
                            new Three(){X21=207}
                        },
                X9 = new Collection<Three>()
                        {
                            new Three(){X21=208}
                        },
                X10 = new Two()
                {
                    X11 = new Three() { X21 = 301 },
                    X12 = new Collection<Three>() { new Three() { X21 = 401 } }
                }
            };

            var mapper = new Mapper<One, VMOne>().UseMapper<Two, VMTwo>().UseMapper<Three, VMThree>();
            var vmOne = mapper.Map(one);
            Assert.AreEqual(vmOne.X7.Length, 1);
            Assert.IsNull(vmOne.X10.X11);
            Assert.AreEqual(vmOne.X3.X11.X21, 99);
        }

        public void Test_Mapper2()
        {
            
        }

        private class One
        {
            public string X1 { get; set; }
            public int X2 { get; set; }
            public Two X3 { get; set; }
            public IList<Three> X4 { get; set; }
            public Three[] X5 { get; set; }
            public ICollection<Three> X6 { get; set; }
            public IList<Three> X7 { get; set; }
            public Three[] X8 { get; set; }
            public ICollection<Three> X9 { get; set; }
            public Two X10 { get; set; }
        }

        private class VMOne
        {
            public string X1 { get; set; }
            public int X2 { get; set; }
            public Two X3 { get; set; }
            public Three[] X4 { get; set; }
            public ICollection<Three> X5 { get; set; }
            public IList<Three> X6 { get; set; }
            public VMThree[] X7 { get; set; }
            public ICollection<VMThree> X8 { get; set; }
            public IList<VMThree> X9 { get; set; }
            public VMTwo X10 { get; set; }
        }
        private class Two
        {
            public Three X11 { get; set; }
            public IList<Three> X12 { get; set; }
        }

        private class VMTwo
        {
            public VMThree X11 { get; set; }
            public IList<VMThree> X12 { get; set; }
        }
        private class Three
        {
            public int X21 { get; set; }
        }

        private class VMThree
        {
            public int X21 { get; set; }
        }
    }
}
