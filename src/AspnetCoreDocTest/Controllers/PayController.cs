using AspnetCoreApiDoc.Proto.Doc;
using AspnetCoreDocTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreDocTest.Controllers
{
    [ApiDoc("模拟支付"), Route("/core/v1/[controller]/[action]/")]
    public class PayController
    {
        /// <summary>
        /// 获取支付产品信息
        /// </summary>
        /// <param name="input">请求参数</param>
        /// <returns>响应参数</returns>
        [HttpPost]
        public ProductInput GetProduct2([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "一体机"};
        }
    }
}