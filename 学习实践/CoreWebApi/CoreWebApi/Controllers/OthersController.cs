using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    /// <summary>
    /// 机票
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OthersController : ControllerBase
    {
        private readonly TodoContext _context;

        public OthersController(TodoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取班级的所有学生
        /// </summary>
        /// <param name="clid">班级id</param>
        /// <returns>学生列表</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Students(int clid)
        {
            return await Task.Run(() =>
             {
                 List<object> sss = new List<object>
                 {
                     new {id=clid,name= "aaa" },
                     new {id=clid,name= "bbb" }
                 };
                 return sss;
             });
        }

        /// <summary>
        /// 获取所有班级
        /// </summary>
        /// <param name="scid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Classes(int scid)
        {
            return await Task.Run(() =>
            {
                List<string> sss = new List<string>
                 {
                     "ccc"+scid,
                     "ddd"+scid
                 };
                return sss;
            });
        }
    }
}