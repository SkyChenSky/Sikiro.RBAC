using System;
using MongoDB.Bson;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.ChatReply
{
    public class ChatReplyModel
    {

        public string Id { get; set; }
        public ObjectId? CompanyId { get; set; }

            public ObjectId ParentId { get; set; }

            [TableCols(Tile = "预设回复")]
        public string News { get; set; }

        [TableCols(Tile = "类型")]
        public string ChatReplyType { get; set; }

        [TableCols(Tile = "状态")]
        public string  Status { get; set; }



        [TableCols(Tile = "排序")]
        public int Order { get; set; }

        [TableCols(Tile = "创建时间")]

        public DateTime CreateDateTime { get; set; }


        [TableCols(Tile = "简码")]
        public string Code { get; set; }









    }
}
