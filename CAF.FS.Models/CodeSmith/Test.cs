using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.FSModels
{
    public class Test
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

    }

    public class Test1 : Test
    {
        public int Age { get; set; }

        
    }
    public class Test2 : Test
    {
        public string Class { get; set; }
        
    }
}
