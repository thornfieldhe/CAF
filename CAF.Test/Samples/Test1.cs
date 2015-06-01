using System.ComponentModel.DataAnnotations;

namespace CAF.Tests.Samples {
    /// <summary>
    /// 测试1
    /// </summary>
    public class Test1 : ITest {
        [StringLength( 20, ErrorMessage = "长度不能超过20" )]
        [Required( ErrorMessage = "名称不能为空" )]
        [Display( Name = "名称" )]
        public string Name { get; set; }
        public int Age { get; set; }
        public int? NullableInt { get; set; }
        public decimal? NullableDecimal { get; set; }
        public LogType EnumValue { get; set; }
        public LogType? NullableEnumValue { get; set; }
        public TestA A { get; set; }
        public class TestA {
            public int Integer { get; set; }
            public string Address { get; set; }
            public TestB B { get; set; }
            public class TestB {
                public string Name { get; set; }
            }
        }
    }
}
