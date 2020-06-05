using Microsoft.AspNetCore.Mvc;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models;
using Sikiro.Web.Admin.Models.Chat;
using Sikiro.Web.Admin.Permission;
using PageListResultExtension = Sikiro.Tookits.Base.PageListResultExtension;

namespace Sikiro.Web.Admin.Controllers
{

    /// <summary>
    /// 聊天记录
    /// </summary>
    public class ChatController : BaseController
    {
        private readonly ChatService _chatService;
        private readonly UserService _userService;
        public ChatController(ChatService chatService, UserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        public IActionResult Index(string userId)
        {
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                userName = _userService.Get(c => c.Id == userId.ToObjectId()) != null
                    ? _userService.Get(c => c.Id == userId.ToObjectId()).UserName
                    : string.Empty;
            }

            ViewBag.userId = userId ?? "";
            ViewBag.userName = userName;
            return View();
        }

        [Permission(PermCode.Chat_List)]
        public IActionResult List(PageListParams<ChatParams> model)
        {
            var where = ExpressionBuilder.Init<Chat>();
            var param = model.Params;


            if (!param.UserId.IsNullOrEmpty())
                where = where.And(a => a.UserId == param.UserId.ToObjectId());

            if (!param.News.IsNullOrEmpty())
                where = where.And(a => a.News.Contains(param.News));

            var result = PageListResultExtension.UpdateForPageListResult(_chatService.GetPageList(model.Page, model.Limit, @where), a => new ChatModel
            {
                News = a.Type == EChatType.Word ? a.News : "",
                FormUser = _userService.Get(c => c.Id == a.UserId)!=null?_userService.Get(c => c.Id == a.UserId).UserName:"",
                CreateDateTime = a.CreateDateTime,
                Picture = a.Type == EChatType.Picture ? a.News : ""
            });
            return PageList(result);

        }
    }
}