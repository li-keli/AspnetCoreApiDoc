using ProtoBuf;

namespace AspnetCoreDocTest.Models
{
    [ProtoContract]
    public class ProductOutput
    {
        /// <summary>
        /// 产品单价
        /// </summary>
        [ProtoMember(1)]
        public double Price { set; get; }
    }
}