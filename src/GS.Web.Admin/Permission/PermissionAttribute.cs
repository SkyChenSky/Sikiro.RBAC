using System;

namespace Sikiro.Web.Admin.Permission
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : System.Attribute
    {
        public int Code { get; set; }

        public PermissionAttribute(PermCode perm)
        {
            Code = (int)perm;
        }
    }
}
