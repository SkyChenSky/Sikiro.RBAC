using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Base;

namespace Sikiro.Web.Admin.Controllers
{
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        public UploadController(IHostingEnvironment env)
        {
            _hostingEnv = env;
        }

        public IActionResult ImageUpload()
        {
            try
            {
                var imgFile = Request.Form.Files[0];
                if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
                {
                    if (imgFile.Length / 1024 / 1024 > 2)
                    {
                        return Json(new { code = 1, msg = "上传的图片不能超过1M", });
                    }

                    long size = 0;
                    string tempname = "";
                    var filename = ContentDispositionHeaderValue
                                    .Parse(imgFile.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    var extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                    var filename1 = System.Guid.NewGuid().ToString() + extname;
                    tempname = filename1;
                    var path = _hostingEnv.WebRootPath;
                    string dir = DateTime.Now.ToString("yyyyMMdd");
                    if (!System.IO.Directory.Exists(_hostingEnv.WebRootPath + $@"/upload/{dir}"))
                    {
                        System.IO.Directory.CreateDirectory(_hostingEnv.WebRootPath + $@"/upload/{dir}");
                    }
                    filename = _hostingEnv.WebRootPath + $@"/upload/{dir}/{filename1}";
                    size += imgFile.Length;
                    using (FileStream fs = System.IO.File.Create(filename))
                    {

                        imgFile.CopyTo(fs);
                        fs.Flush();
                    }
                    return Json(new { code = 0, msg = "上传成功", data = new { src = $"/upload/{dir}/{filename1}", title = "图片标题" } });
                }
                return Json(new { code = 1, msg = "上传失败", });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.ToString(), });
            };
        }

        public IActionResult Img()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
            {
                var dir = $@"/upload/{DateTime.Now:yyyyMMdd}";

                var result = UploadHelper.Img(_hostingEnv.WebRootPath + dir, imgFile);
                if (!result.Success)
                    return Json(ServiceResult.IsFailed("上传失败"));

                var imgUrl = $@"{dir}" + @"/" + Path.GetFileName(result.Data.ToString());
                return Json(ServiceResult.IsSuccess("上传成功", imgUrl));
            }
            return Json(ServiceResult.IsFailed("上传失败"));
        }
    }
}