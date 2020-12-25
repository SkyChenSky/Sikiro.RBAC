using System.IO;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Files;

namespace Sikiro.Web.Admin.Controllers
{
    public class UploadController : Controller
    {

        public IActionResult Img()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
            {
                return Json(ServiceResult.IsSuccess("上传成功", imgFile.FileName));
            }
            return Json(ServiceResult.IsFailed("上传失败"));
        }
    }
}