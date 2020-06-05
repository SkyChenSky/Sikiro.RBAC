using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.Common.Utils
{
    public static class UploadHelper
    {
        /// <summary>
        /// 存放路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        public static ServiceResult Img(string path, IFormFile imgFile)
        {
            path.ThrowIfNull();
            imgFile.ThrowIfNull();

            try
            {
                if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
                {
                    var extensionName = Path.GetExtension(imgFile.FileName);
                    var filename = Guid.NewGuid().ToString("N") + extensionName;

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    filename = Path.Combine(path, filename);
                    using (var fs = File.Create(filename))
                    {
                        imgFile.CopyTo(fs);
                        fs.Flush();
                    }

                    return ServiceResult.IsSuccess("上传成功", filename);
                }
            }
            catch (Exception e)
            {
                e.WriteToFile("上传异常");

            }

            return ServiceResult.IsFailed("上传失败");
        }
    }
}
