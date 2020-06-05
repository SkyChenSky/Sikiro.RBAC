using MongoDB.Bson;

namespace Sikiro.Entity.Admin
{
    /// <summary>
    /// 多租户接口
    /// </summary>
    public interface ITenant
    {
        ObjectId? CompanyId { get; set; }
    }
}
