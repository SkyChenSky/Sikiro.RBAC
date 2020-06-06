using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using Sikiro.Common.Utils;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.System
{
    /// <summary>
    /// 后台管理员
    /// </summary>
    public class AdministratorService : IDepend
    {
        private readonly MongoRepository _mongoRepository;

        public AdministratorService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        /// <summary>
        /// 手机登录验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ServiceResult LogonCheck(string userName, string password)
        {
            var user = _mongoRepository.Get<Administrator>(a => a.UserName == userName && a.Status != EAdministratorStatus.Deleted);

            if (user == null)
                return ServiceResult.IsFailed(AccountConstString.NoUser);
            if (user.Status == EAdministratorStatus.Stop)
                return ServiceResult.IsFailed("该用户已禁止登陆");
            if (user.Status == EAdministratorStatus.NotActive)
                return ServiceResult.IsFailed("该用户未激活");

            var passwordForMd5 = password.EncodePassword(user.Id.ToString());
            if (user.Password != passwordForMd5)
                return ServiceResult.IsFailed(AccountConstString.PasswordError);

            return ServiceResult.IsSuccess(AccountConstString.ValidSuccess, new AdministratorData
            {
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                RealName = user.RealName,
                IsSuper = user.IsSuper
            });
        }

        public PageList<Administrator> GetPageList(int pageIndex, int pageSize, Expression<Func<Administrator, bool>> expression)
        {
            return _mongoRepository.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public Administrator GetById(string id)
        {
            return _mongoRepository.Get<Administrator>(a => a.Id == new ObjectId(id));
        }

        public ServiceResult UpdateStatus(string userId, EAdministratorStatus status)
        {
            var result = _mongoRepository.Update<Administrator>(a => a.Id == new ObjectId(userId), a => new Administrator
            {
                Status = status,
                UpdateDateTime = DateTime.Now
            });
            return result > 0 ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed);
        }

        public ServiceResult UpdateById(string id, Expression<Func<Administrator, Administrator>> update)
        {
            return _mongoRepository.Update(a => a.Id == new ObjectId(id), update) > 0 ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed);
        }

        public ServiceResult IsExists(string userName, string id)
        {
            return _mongoRepository.Exists<Administrator>(a => a.Id != new ObjectId(id) && a.UserName == userName) ? ServiceResult.IsFailed("已存在该用户名") : ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Add(Administrator administrator)
        {
            var userId = ObjectId.GenerateNewId();
            administrator.Id = userId;
            administrator.Password = administrator.Password.EncodePassword(userId.ToString());
            administrator.CreateDateTime = DateTime.Now;
            administrator.UpdateDateTime = DateTime.Now;

            var result = _mongoRepository.AddIfNotExist(a => a.UserName == administrator.UserName, administrator);
            return result
                ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess)
                : ServiceResult.IsFailed("已存在该用户名");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public ServiceResult ChangePassword(ObjectId userId, string oldPassword, string newPassword)
        {
            var user = _mongoRepository.Get<Administrator>(a => a.Id == userId);
            if (user == null)
                return ServiceResult.IsFailed(AccountConstString.NoUser);

            var oldEncodePassword = EncodePassword(userId.ToString(), oldPassword);
            if (user.Password != oldEncodePassword)
                return ServiceResult.IsFailed("原密码错误");

            var newEncodePassword = EncodePassword(userId.ToString(), newPassword);
            _mongoRepository.Update<Administrator>(a => a.Id == userId, a => new Administrator { Password = newEncodePassword });

            return ServiceResult.IsSuccess("修改密码完成");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public ServiceResult ResetPassword(string userId, string newPassword)
        {
            var newEncodePassword = EncodePassword(userId, newPassword);
            _mongoRepository.Update<Administrator>(a => a.Id == userId.ToObjectId(), a => new Administrator { Password = newEncodePassword });

            return ServiceResult.IsSuccess("重置密码完成");
        }

        public string EncodePassword(string userId, string password)
        {
            return (password.EncodeMd5String() + userId).EncodeMd5String();
        }

        public List<Administrator> GetListByIds(IEnumerable<ObjectId> ids)
        {
            if (!ids.Any())
                return new List<Administrator>();

            return _mongoRepository.ToList<Administrator>(a => ids.Contains(a.Id));
        }

        public ServiceResult BatchSetStatus(ObjectId[] userIds, EAdministratorStatus status)
        {
            _mongoRepository.Update<Administrator>(a => userIds.Contains(a.Id), a => new Administrator { Status = status });

            return ServiceResult.IsSuccess("设置完成");
        }

        public Administrator GetByPositionId(ObjectId positionId)
        {
            return _mongoRepository.Get<Administrator>(a => a.PositionIds.Contains(positionId));
        }

        public Administrator GetByRoleId(ObjectId roleId)
        {
            return _mongoRepository.Get<Administrator>(a => a.RoleIds.Contains(roleId));
        }

        /// <summary>
        /// 获取用户已有的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string[] GetUserCanPassUrl(string userId)
        {
            var administrator = GetById(userId);

            if (administrator == null)
                return new string[] { };

            return GetAdministratorUrl(administrator);
        }

        /// <summary>
        /// 设置访问权限缓存，并返回功能权限code
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<int> GetActionCode(string userId)
        {
            userId.ThrowIfNull();

            var admin = _mongoRepository.Get<Administrator>(a => a.Id == userId.ToObjectId());
            if (admin == null)
                return new List<int>();

            var list = admin.IsSuper ? SetSuper(admin) : SetAdmin(admin);
            return list.Select(a => a.Code);
        }

        private List<MenuAction> SetAdmin(Administrator admin)
        {
            var roles = _mongoRepository.ToList<Role>(a => admin.RoleIds.Contains(a.Id));
            var menuActionInRoles = roles.SelectMany(a => a.MenuActionIds).Distinct();

            var menuActionInUserList = _mongoRepository.ToList<MenuAction>(a => menuActionInRoles.Contains(a.Id));

            return menuActionInUserList;
        }

        private List<MenuAction> SetSuper(Administrator admin)
        {
            var menuActionInUserList = _mongoRepository.ToList<MenuAction>(a => true);

            return menuActionInUserList.ToList();
        }

        private string[] GetAdministratorUrl(Administrator admin)
        {
            var roles = _mongoRepository.ToList<Role>(a => admin.RoleIds.Contains(a.Id));
            var menuInRoles = roles.SelectMany(a => a.MenuId).Distinct();
            var menuActionInRoles = roles.SelectMany(a => a.MenuActionIds).Distinct();

            var menuInUser = _mongoRepository.ToList<Menu>(a => menuInRoles.Contains(a.Id)).Select(a => (a.Url??"").ToLower().Trim('/'));
            var menuActionInUserList = _mongoRepository.ToList<MenuAction>(a => menuActionInRoles.Contains(a.Id));

            var menuActionInUser = menuActionInUserList.SelectMany(a => a.Url);
            var allUrl = menuInUser.Concat(menuActionInUser);

            return allUrl.ToArray();
        }

        /// <summary>
        ///获得管理员对应的CompanyId
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public ObjectId GetCompanyId(ObjectId adminId)
        {
            //todo delete
            var model = _mongoRepository.Get<Administrator>(a => a.Id == adminId);
            return new ObjectId();
        }
    }
}
