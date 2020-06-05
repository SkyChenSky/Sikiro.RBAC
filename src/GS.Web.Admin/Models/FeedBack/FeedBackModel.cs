using System;
using MongoDB.Bson;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.FeedBack
{
    public class FeedBackModel
    {


        [TableCols(Tile = "用户")]
        public string UserName { get; set; }

        public string Id { get; set; }

        public ObjectId UserId { get; set; }
        /// <summary>
        /// 企业商户 id
        /// </summary>
        public ObjectId CompanyId { get; set; }


        [TableCols(Tile = "联系方式")]
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }


        /// <summary>
        /// 反馈内容 
        /// </summary>
        [TableCols(Tile = "反馈内容")]
        public string Desc { get; set; }


        [TableCols(Tile = "反馈日期")]
        /// <summary>
        /// 反馈日期
        /// </summary>

        public DateTime? FeedBackTime { get; set; }


        [TableCols(Tile = "回复")]
        /// <summary>
        /// 回复
        /// </summary>
        public string Reply { get; set; }

        [TableCols(Tile = "回复日期")]
        /// <summary>
        /// 回复日期
        /// </summary>
        public DateTime? ReplyTime { get; set; }

    }
}
