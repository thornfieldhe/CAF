using System.Collections.Generic;
namespace CAF.Web.WebForm.Common
{
    using FineUI;


    public class ListItemCompare : IEqualityComparer<ListItem>
    {
        public bool Equals(ListItem x, ListItem y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(ListItem obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}