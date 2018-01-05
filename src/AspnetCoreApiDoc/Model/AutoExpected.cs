using ProtoBuf;

namespace AspnetCoreApiDoc.Model
{
    [ProtoContract]
    public class AutoExpected
    {
        [ProtoMember(1)]
        public ExpectedOutput BaseOutput { set; get; }
    }

    [ProtoContract]
    public class ExpectedOutput
    {
        /// <summary>
        /// 接口是否成功
        /// </summary>
        [ProtoMember(1)]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 接口错误消息
        /// </summary>
        [ProtoMember(2)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 接口错误代码
        /// </summary>
        [ProtoMember(3)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public string Exception { set; get; }
    }
}
