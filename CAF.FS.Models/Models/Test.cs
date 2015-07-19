
namespace CAF.Models
{
    public partial class Test
    {
        public string Name { get; set; }

    }

    public partial class Test1 : Test
    {
        public int Age { get; set; }


    }
    public partial class Test2 : Test
    {
        public string Class { get; set; }

    }
}
