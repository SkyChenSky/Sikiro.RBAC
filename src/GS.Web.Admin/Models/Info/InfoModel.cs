using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Info
{
    public class InfoModel
    {

        [TableCols(Tile = "消息标题")]
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        [TableCols(Tile = "消息内容")]
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Desc { get; set; }

        [TableCols(Tile = "用户名称")]
        /// <summary>
        /// 000000000000000000000000 代表所有人，否则发送给某一个用
        /// </summary>
        public string UserName { get; set; }

        [TableCols(Tile = "创建时间")]
        /// <summary>
        /// 00000000000000 代表所有人，否则发送给某一个用
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}
