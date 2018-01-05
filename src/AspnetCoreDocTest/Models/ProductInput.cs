using ProtoBuf;

namespace AspnetCoreDocTest.Models
{
    [ProtoContract]
    public class ProductInput
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        [ProtoMember(2)]
        public int ProductId { set; get; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [ProtoMember(1)]
        public string ProductName { set; get; }
    }
}