using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Sikiro.Common.Utils;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Permission
{
    /// <summary>
    /// 功能权限
    /// </summary>
    public static class PermissionUtil
    {
        public static readonly Dictionary<string, IEnumerable<int>> PermissionUrls = new Dictionary<string, IEnumerable<int>>();
        private static MongoRepository _mongoRepository;

        /// <summary>
        /// 判断权限值是否被重复使用
        /// </summary>
        public static void ValidPermissions()
        {
            var codes = Enum.GetValues(typeof(PermCode)).Cast<int>();
            var dic = new Dictionary<int, int>();
            foreach (var code in codes)
            {
                if (!dic.ContainsKey(code))
                    dic.Add(code, 1);
                else
                    throw new Exception($"权限值 {code} 被重复使用，请检查 PermCode 的定义");
            }
        }

        /// <summary>
        /// 初始化添加预定义权限值
        /// </summary>
        /// <param name="app"></param>
        public static void InitPermission(IApplicationBuilder app)
        {
            //验证权限值是否重复
            ValidPermissions();

            //反射被标记的Controller和Action
            _mongoRepository = (MongoRepository)app.ApplicationServices.GetService(typeof(MongoRepository));

            var permList = new List<MenuAction>();
            var actions = typeof(PermissionUtil).Assembly.GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t) && !t.IsAbstract)
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly));

            //遍历集合整理信息
            foreach (var action in actions)
            {
                var permissionAttribute =
                    action.GetCustomAttributes(typeof(PermissionAttribute), false).ToList();
                if (!permissionAttribute.Any())
                    continue;

                var codes = permissionAttribute.Select(a => ((PermissionAttribute)a).Code).ToArray();
                var controllerName = action?.ReflectedType?.Name.Replace("Controller", "").ToLower();
                var actionName = action.Name.ToLower();

                foreach (var item in codes)
                {
                    if (permList.Exists(c => c.Code == item))
                    {
                        var menuAction = permList.FirstOrDefault(a => a.Code == item);
                        menuAction?.Url.Add($"{controllerName}/{actionName}".ToLower());
                    }
                    else
                    {
                        var perm = new MenuAction
                        {
                            Id = item.ToString().EncodeMd5String().ToObjectId(),
                            CreateDateTime = DateTime.Now,
                            Url = new List<string> { $"{controllerName}/{actionName}".ToLower() },
                            Code = item,
                            Name = ((PermCode)item).GetDisplayName() ?? ((PermCode)item).ToString()
                        };
                        permList.Add(perm);
                    }
                }
                PermissionUrls.TryAdd($"{controllerName}/{actionName}".ToLower(), codes);
            }

            //业务功能持久化
            _mongoRepository.Delete<MenuAction>(a => true);
            _mongoRepository.BatchAdd(permList);
        }

        /// <summary>
        /// 获取当前路径
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public static string CurrentUrl(HttpContext filterContext)
        {
            var url = filterContext.Request.Path.ToString().ToLower().Trim('/');
            return url;
        }
    }
}
