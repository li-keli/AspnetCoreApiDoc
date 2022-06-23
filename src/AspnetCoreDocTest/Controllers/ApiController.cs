using AspnetCoreApiDoc.Proto.Doc;
using AspnetCoreDocTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspnetCoreDocTest.Controllers
{
    [ApiDoc("公共API文档"), Route("/core/v1/[controller]/[action]/")]
    public class ApiController
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiController>();
        }

        /// <summary>
        /// 获取产品(Get方法)
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>输出参数</returns>
        [HttpGet]
        public ProductInput GetProduct([FromBody] ProductInput input)
        {
            // _logger.LogError("这是个异常");
            return new ProductInput {ProductName = "一体机"};
        }

        /// <summary>
        /// 获取产品(Post方法)
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>输出参数</returns>
        [HttpPost]
        public ProductInput GetProduct2([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "一体机"};
        }

        /// <summary>
        /// 获取产品吴文打死你个
        /// </summary>
        [HttpPost]
        [ApiDoc(isCreateDoc: false)]
        public ProductInput GetProductNoDoc([FromBody] ProductInput input)
        {
            return new ProductInput {ProductName = "二体机"};
        }
    }
}