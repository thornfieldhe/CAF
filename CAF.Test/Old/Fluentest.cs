﻿using System;
using System.Collections.Generic;

namespace CAF.Test
{

    using CAF;
    using CAF.Utility;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Fluentester
    {


   

 
        //        [TestMethod]
        //        public void Test_Guard_Against()
        //        {
        //            try
        //            {
        //                Guard.Against<NotImplementedException>(false);
        //                Assert.AreEqual(true, true);
        //            }
        //            catch (NotImplementedException)
        //            {
        //                Assert.AreEqual(false, true);
        //            }
        //        }

        [TestMethod]
        public void Test_Foreach_WithIndex()
        {
            List<string> data = new List<string>()
            {
                "A", "B", "C", "D"
            };

            var counter = 0;
            data.ForEach((item, index) =>
            {
                counter += 1;
                var y = index;
            });
        }

     

 
        [TestMethod]
        public void Test_Or_Specification()
        {

        }

        [TestMethod]
        public void Test_Not_Specification()
        {

        }

        [TestMethod]
        public void Test_Expression_Specification()
        {

        }
  


        //        [TestMethod]
        //        public void Test_Mapper()
        //        {
        //
        //            var one = new One()
        //            {
        //                X1 = "test value",
        //                X2 = 77,
        //                X3 = new Two()
        //                {
        //                    X11 = new Three()
        //                    {
        //                        X21 = 99
        //                    },
        //                    X12 = new List<Three>()
        //                    {
        //                        new Three(){ X21 = 105 }
        //                    }
        //                },
        //                X4 = new List<Three>()
        //                {
        //                    new Three(){X21=203}
        //                },
        //                X5 = new Three[]
        //                {
        //                    new Three(){X21=204}
        //                },
        //                X6 = new Collection<Three>()
        //                {
        //                    new Three(){X21=205}
        //                },
        //                X7 = new List<Three>()
        //                {
        //                    new Three(){X21=206}
        //                },
        //                X8 = new Three[]
        //                {
        //                    new Three(){X21=207}
        //                },
        //                X9 = new Collection<Three>()
        //                {
        //                    new Three(){X21=208}
        //                },
        //                X10 = new Two()
        //                {
        //                    X11 = new Three() { X21 = 301 },
        //                    X12 = new Collection<Three>() { new Three() { X21 = 401 } }
        //                }
        //            };
        //
        //            var mapper = new Mapper<One, VMOne>().UseMapper<Two, VMTwo>().UseMapper<Three, VMThree>();
        //            var vmOne = mapper.Map(one);
        //        }



        private class A
        {
            public List<B> Bs { get; set; }
            public int Id { get; set; }
        }

        private class B
        {
            public int Id { get; set; }
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
