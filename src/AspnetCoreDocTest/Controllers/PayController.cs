using AspnetCoreApiDoc.Proto.Doc;
using AspnetCoreDocTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreDocTest.Controllers
{
    [ApiDoc("API文档目录二"), Route("/core/v1/[controller]/[action]/")]
    public class PayController
    {
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>输出参数</returns>
        [HttpPost]
        public ProductInput GetProduct2([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "一体机"};
        }
    }
}