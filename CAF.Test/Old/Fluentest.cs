using System;
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

 

        //      



        private class A
        {
            public List<B> Bs { get; set; }
            public int Id { get; set; }
        }

        private class B
        {
            public int Id { get; set; }
        }
        
    
    }
}
