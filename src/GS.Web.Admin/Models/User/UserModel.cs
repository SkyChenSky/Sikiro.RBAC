using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.User
{
    public class UserModel
    {
        public string Id { get; set; }


        [TableCols(Tile = "用户昵称")]
        public string NickName { get; set; }

        [TableCols(Tile = "用户名")]
        public string UserName { get; set; }


        public string UserLogo { get; set; }


        public string Phone { get; set; }


        public string OpenId { get; set; }

        public int UserStatus { get; set; }

        public decimal Progress { get; set; }
    }
}
