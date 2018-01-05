using AspnetCoreApiDoc.Proto.Doc;
using AspnetCoreDocTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspnetCoreDocTest.Controllers
{
    [ApiDoc("API文档目录一"), Route("/core/v1/[controller]/[action]/")]
    public class ApiController
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiController>();
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>输出参数</returns>
        [HttpPost]
        public ProductInput GetProduct([FromBody] ProductInput input)
        {
//            _logger.LogError("这是个异常");
            return new ProductInput {ProductName = "一体机"};
        }

        /// <summary>
        /// 获取产品方法2
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>输出参数</returns>
        [HttpPost]
        public ProductInput GetProduct2([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "一体机"};
        }

        /// <summary>
        /// 获取产品方法3
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>输出参数</returns>
        [HttpPost]
        public ProductInput GetProduct3([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "一体机"};
        }

        /// <summary>
        /// 获取产品 不生成文档
        /// </summary>
        [HttpPost]
        [ApiDoc(isCreateDoc: false)]
        public ProductInput GetProductNoDoc([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "二体机"};
        }
    }
}