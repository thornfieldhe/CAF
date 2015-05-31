using System;
using System.ComponentModel.DataAnnotations;

namespace Util.Tests.Samples
{
    using CAF;

    /// <summary>
    /// 测试2
    /// </summary>
    [Serializable]
    public class Test2
    {
        public Test2()
        {
        }
        public Test2(string name)
        {
            Name = name;
        }

        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(R), ErrorMessageResourceName = "IsEmpty")]
        public int Int { get; set; }
        public int? NullableInt { get; set; }
        public decimal? NullableDecimal { get; set; }
        public decimal Decimal { get; set; }
        public TestA A { get; set; }
        [Serializable]
        public class TestA
        {
            public int Integer { get; set; }
            public string Address { get; set; }
            public TestB B { get; set; }
            public class TestB
            {
                public string Name { get; set; }
            }
        }
    }
}
