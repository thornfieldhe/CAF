using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAF.Tests.Samples
{
    /// <summary>
    /// 地址
    /// </summary>
    public class Address : ValueObjectBase<Address>
    {
        /// <summary>
        /// 初始化地址
        /// </summary>
        public Address()
        {
        }

        /// <summary>
        /// 初始化地址
        /// </summary>
        /// <param name="city">城市</param>
        /// <param name="street">街道</param>
        public Address(string city, string street)
            : this(city, street, null)
        {
        }

        /// <summary>
        /// 初始化地址
        /// </summary>
        /// <param name="city">城市</param>
        /// <param name="street">街道</param>
        /// <param name="customer">客户</param>
        public Address(string city, string street, Customer2 customer)
            : this(city, street, customer, null)
        {
        }

        /// <summary>
        /// 初始化地址
        /// </summary>
        /// <param name="city">城市</param>
        /// <param name="street">街道</param>
        /// <param name="customer">客户</param>
        /// <param name="child">子地址</param>
        public Address(string city, string street, Customer2 customer, Address child)
        {
            City = city;
            Street = street;
            Customer = customer;
            Child = child;
        }

        /// <summary>
        /// 城市
        /// </summary>
        [StringLength(100, ErrorMessage = "城市输入过长，不能超过100位")]
        public string City { get; private set; }
        /// <summary>
        /// 街道
        /// </summary>
        [StringLength(200, ErrorMessage = "街道输入过长，不能超过200位")]
        public string Street { get; private set; }

        /// <summary>
        /// 客户
        /// </summary>
        [NotMapped]
        public Customer2 Customer { get; private set; }

        /// <summary>
        /// 子地址
        /// </summary>
        [NotMapped]
        public Address Child { get; private set; }

    }
}
