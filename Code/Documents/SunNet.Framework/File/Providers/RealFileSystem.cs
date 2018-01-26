using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO;
using SF.Framework.ConfigLibray;
using Newtonsoft.Json.Linq;
using SF.Framework.ResultMessages.ResultMessage;
using SF.Framework.Core;
using SF.Framework.Log;
using SF.Framework.Helpers;
using System.Drawing;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace SF.Framework.File.Providers
{
    public class RealFileSystem : IFile
    {
        public void Move(string source, string destination)
        {
            System.IO.File.Move(source, destination);
        }

        public void Copy(string source, string destination)
        {
            System.IO.File.Copy(source, destination);
        }

        public bool Delete(string filePath)
        {
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Delete(string filePath, string thumbPath)
        {
            try
            {
                System.IO.File.Delete(filePath);
                System.IO.File.Delete(thumbPath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void DeleteFile(string path)
        {
            FileInfo file = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(path));
            if (file.Exists)
            {
                file.Delete();
            }
        }

        //general video jpg.
        public string CatchImg(string fileName, string imgFile)
        {
            string ffmpeg = HttpContext.Current.Server.MapPath("~/" + SFConfig.Ffmpeg);
            string flv_img = imgFile;
            string FlvImgSize = SFConfig.CatchFlvImgSize;
            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ImgstartInfo.Arguments = "  -i  " + fileName + "  -y  -f  image2  -ss 1 -vframes 1  -s  " + FlvImgSize + " " + flv_img;
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo).WaitForExit();
            }
            catch
            {
                return "";
            }
            if (System.IO.File.Exists(flv_img))
            {
                return flv_img;
            }

            return "";
        }

        public string UploadVideo(HttpPostedFile fileData, string fileFolder, string filePhysicalPath)
        {
            FileEntity file = new FileEntity();
            if (fileData != null)
            {
                try
                {
                    string filePath = filePhysicalPath + "/";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    file.DisplayName = Path.GetFileName(fileData.FileName);
                    file.Extension = Path.GetExtension(file.DisplayName);
                    file.Size = fileData.ContentLength;
                    file.ContentType = fileData.ContentType;
                    file.CreateTime = DateTime.Now;
                    file.DbName = GetFileDBName(file.Extension);
                    string imgName = GetFileDBName(".jpg");
                    file.FilePath = "/" + fileFolder + "/" + file.DbName;
                    fileData.SaveAs(filePath + file.DbName);
                    CatchImg(filePath + file.DbName, filePath + imgName);
                    file.ImgPath = "/" + fileFolder + "/" + imgName;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(file);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图保持原来的比例
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileFolder"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <summary>
        /// 生成缩略图保持原来的比例
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileFolder"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public FileEntity UploadImageThumbnail2
            (HttpPostedFile fileData, string fileFolder, string filePhysicalPath, int width, int height, string prefix = "")
        {
            ThumnailMode mode = ThumnailMode.HW;
            string fileType = "jpg";

            FileEntity file = new FileEntity();
            if (fileData != null)
            {
                try
                {
                    string filePath = filePhysicalPath + "/";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    file.DisplayName = Path.GetFileName(fileData.FileName);
                    file.Extension = Path.GetExtension(file.DisplayName);
                    file.Size = fileData.ContentLength;
                    file.ContentType = fileData.ContentType;
                    file.CreateTime = DateTime.Now;
                    file.DbName = GetFileDBName(file.Extension);
                    file.FilePath = "/" + fileFolder + "/" + file.DbName;
                    Image image = Image.FromStream(fileData.InputStream);
                    mode = ThumnailMode.Cut;
                    ThumbnailHelper.Create(image, filePath + file.DbName, width, height, mode, fileType);
                    return file;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public FileEntity UploadImageThumbnailByWidth
            (HttpPostedFile fileData, string fileFolder, string physicalpath, int width, int height, string prefix = "")
        {
            ThumnailMode mode = ThumnailMode.EqualW;
            string fileType = "jpg";

            FileEntity file = new FileEntity();
            if (fileData != null)
            {
                try
                {
                    string filePath = physicalpath + "/";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    file.DisplayName = Path.GetFileName(fileData.FileName);
                    file.Extension = Path.GetExtension(file.DisplayName);
                    file.Size = fileData.ContentLength;
                    file.ContentType = fileData.ContentType;
                    file.CreateTime = DateTime.Now;
                    file.DbName = GetFileDBName(file.Extension);
                    file.FilePath = "/" + fileFolder + "/" + file.DbName;

                    Image image = Image.FromStream(fileData.InputStream);
                    int outWidth = 0;
                    int outHeight = 0;
                    ThumbnailHelper.CreateOutWAndH(image, filePath + file.DbName, width, height, mode, fileType, out outWidth, out outHeight);

                    file.ImageWidth = outWidth;
                    file.ImageHeight = outHeight;

                    return file;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public FileEntity UploadCutImage(string inputFileName, string outputFileName, string fileFolder, string filePhysicalPath, int toWidth, int toHeight, int cropWidth, int cropHeight, int x, int y)
        {
            string fileType = "jpg";

            FileEntity file = new FileEntity();
            try
            {
                string filePath = filePhysicalPath + "/";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                file.DisplayName = Path.GetFileName(outputFileName);
                file.Extension = Path.GetExtension(file.DisplayName);
                file.CreateTime = DateTime.Now;
                file.DbName = GetFileDBName(file.Extension);
                file.FilePath = "/" + fileFolder + "/" + file.DbName;
                file.ImgPath = file.FilePath;

                ThumbnailHelper.Cut(inputFileName, filePath + file.DbName, toWidth, toHeight, cropWidth, cropHeight, x, y, fileType);
                return file;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return null;
            }
        }

        public string Upload(HttpPostedFile fileData, string fileFolder, string filePhysicalPath)
        {
            if (fileData != null)
            {
                try
                {

                    FileEntity file = UploadReturnFileEntity(fileData, fileFolder, filePhysicalPath);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(file);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public FileEntity UploadReturnFileEntity(HttpPostedFile fileData, string fileFolder, string filePhysicalPath, string virtualDir = "")
        {
            FileEntity file = new FileEntity();
            if (fileData != null)
            {
                try
                {
                    string filePath = filePhysicalPath + "/";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    file.DisplayName = Path.GetFileName(fileData.FileName);
                    file.Extension = Path.GetExtension(file.DisplayName);
                    file.Size = fileData.ContentLength;
                    file.ContentType = fileData.ContentType;
                    file.CreateTime = DateTime.Now;
                    file.DbName = GetFileDBName(file.Extension);
                    file.FilePath = "/" + fileFolder + "/" + file.DbName;
                    file.FullPath = virtualDir + file.FilePath;
                    fileData.SaveAs(filePath + file.DbName);
                    return file;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public string DESEncryptUploadFile(HttpPostedFile fileData, string fileFolder, string filePhysicalPath)
        {
            if (fileData != null)
            {
                try
                {

                    FileEntity file = DESEncryptUploadFileReturnFileEntity(fileData, fileFolder, filePhysicalPath);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(file);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public FileEntity DESEncryptUploadFileReturnFileEntity(HttpPostedFile fileData, string fileFolder, string filePhysicalPath, string virtualDir = "")
        {
            FileEntity file = new FileEntity();
            if (fileData != null)
            {
                try
                {
                    string filePath = filePhysicalPath + "/";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    file.DisplayName = Path.GetFileName(fileData.FileName);
                    file.Extension = Path.GetExtension(file.DisplayName);
                    file.Size = fileData.ContentLength;
                    file.ContentType = fileData.ContentType;
                    file.CreateTime = DateTime.Now;
                    file.DbName = GetFileDBName(file.Extension);
                    file.FilePath = "/" + fileFolder + "/" + file.DbName;
                    file.FullPath = virtualDir + file.FilePath;
                    string desKey = "sunneTUs";
                    string desIV = "sunneT.U";
                    byte[] bytes = new byte[fileData.InputStream.Length];
                    fileData.InputStream.Read(bytes, 0, bytes.Length);
                    fileData.InputStream.Close();
                    FileStream fileStream = new FileStream(filePath + file.DbName, FileMode.OpenOrCreate, FileAccess.Write);
                    DES des = new DESCryptoServiceProvider();
                    CryptoStream cryptoStream = new CryptoStream(fileStream, des.CreateEncryptor(Encoding.Default.GetBytes(desKey), Encoding.Default.GetBytes(desIV)), CryptoStreamMode.Write);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.Close();
                    fileStream.Close();
                    return file;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public byte[] DESDecryptFile(string filePhysicalPath)
        {
            if (System.IO.File.Exists(filePhysicalPath))
            {
                try
                {
                    string desKey = "sunneTUs";
                    string desIV = "sunneT.U";
                    FileStream fileStream = System.IO.File.OpenRead(filePhysicalPath);
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, bytes.Length);
                    fileStream.Close();
                    DES des = new DESCryptoServiceProvider();
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(Encoding.Default.GetBytes(desKey), Encoding.Default.GetBytes(desIV)), CryptoStreamMode.Write);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] buffer = new byte[memoryStream.Length];
                    memoryStream.Position = 0;
                    memoryStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                    return buffer;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private string GetFileDBName(string fileExt)
        {
            int rand = new Random().Next(1000, 9999);
            return DateTime.Now.ToString("yyMMddHHmmssffff") + "_" + rand + fileExt;
        }

        /// <summary>
        /// if error ,result = {"type":"ext","success",bool,msg:""};
        /// </summary>
        public FileEntity UploadFile(HttpFileCollection files, out ResultEntity resultEntity, string[] fileTypes, string fileFolder, string fileID)
        {
            FileEntity fileEntity = new FileEntity();
            resultEntity = new ResultEntity(ActionType.Ext);
            if (files[fileID].ContentLength == 0)
            {
                resultEntity.CreateResultEntity(ActionType.Ext, false, new List<IMessageEntity>() 
                {
                     new MessageEntity("msg",MessageManager.ReadValueByKey("FileNotNull"))
                });
                //result.Add("success", false);
                //result.Add("msg", );
                return null;
            }
            HttpPostedFile file = files[fileID];
            Stream filedate = file.InputStream;
            string targetPath = System.Web.HttpContext.Current.Server.MapPath("~/UploadFile/" + fileFolder + "/");

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string fileName = Path.GetFileName(file.FileName);
            if (fileTypes != null && fileTypes.Length > 0)
            {
                if (!fileTypes.Contains(Path.GetExtension(fileName).ToLower()))
                {
                    string fileFormat = "";
                    foreach (var item in fileTypes)
                    {
                        fileFormat += item + ",";
                    }
                    fileFormat = fileFormat.Substring(0, fileFormat.Length - 1);
                    resultEntity.CreateResultEntity(ActionType.Ext, false, new List<IMessageEntity>() 
                     {
                     new MessageEntity("msg",string.Format(MessageManager.ReadValueByKey("FileInvalid"), fileFormat))
                    });
                    //result.Add("success", false);
                    //result.Add("msg", );
                    return null;
                }
            }
            fileEntity.DisplayName = fileName;
            fileEntity.Extension = Path.GetExtension(fileEntity.DisplayName);

            fileEntity.Size = file.ContentLength;
            fileEntity.DbName = fileEntity.DisplayName.Replace("&", "").Replace(fileEntity.Extension, "") + DateTime.Now.ToString("yyMMddHHmmssffff") + fileEntity.Extension;
            fileEntity.FilePath = "/UploadFile/" + fileFolder + "/" + fileEntity.DbName;
            string SaveFileName = targetPath + fileEntity.DbName;
            file.SaveAs(SaveFileName);
            return fileEntity;
        }

        /// <summary>
        /// if error ,result = {"type":"ext","success",bool,msg:""};
        /// </summary>
        public FileEntity UploadFile(HttpFileCollection files, out JObject result, string[] fileTypes, string fileFolder, string fileID)
        {
            FileEntity fileEntity = new FileEntity();
            result = new JObject();
            result.Add("type", "ext");
            if (files[fileID] == null || files[fileID].ContentLength == 0)
            {
                result.Add("success", false);
                result.Add("msg", MessageManager.ReadValueByKey("FileNotNull"));
                return null;
            }
            HttpPostedFile file = files[fileID];
            Stream filedate = file.InputStream;
            string targetPath = System.Web.HttpContext.Current.Server.MapPath("~/UploadFile/" + fileFolder + "/");

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string fileName = Path.GetFileName(file.FileName);
            if (fileTypes != null && fileTypes.Length > 0)
            {
                if (!fileTypes.Contains(Path.GetExtension(fileName).ToLower()))
                {
                    string fileFormat = string.Join(",", fileTypes);
                    result.Add("success", false);
                    result.Add("msg", string.Format(MessageManager.FormatValueByKey("FileInvalid", fileFormat)));
                    return null;
                }
            }
            fileEntity.DisplayName = fileName;
            fileEntity.Extension = Path.GetExtension(fileEntity.DisplayName);

            fileEntity.Size = file.ContentLength;
            fileEntity.DbName = fileEntity.DisplayName.Replace("&", "").Replace("#", "").Replace(fileEntity.Extension, "") + DateTime.Now.ToString("yyMMddHHmmssffff") + fileEntity.Extension;
            fileEntity.FilePath = "/UploadFile/" + fileFolder + "/" + fileEntity.DbName;
            string SaveFileName = targetPath + fileEntity.DbName;
            file.SaveAs(SaveFileName);
            return fileEntity;
        }


        public FileEntity UploadFile(HttpFileCollection files, out Result.FunctionResult result, string[] fileTypes, string fileFolder, string fileID)
        {
            FileEntity fileEntity = new FileEntity();
            result = new Result.FunctionResult();
            if (files[fileID] == null || files[fileID].ContentLength == 0)
            {
                result.BoolResult = false;
                result.AddBrokenRuleMessage("Error", MessageManager.ReadValueByKey("FileNotNull"));
                return null;
            }
            HttpPostedFile file = files[fileID];
            Stream filedate = file.InputStream;
            string targetPath = SFConfig.UploadFile;

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string fileName = Path.GetFileName(file.FileName);
            if (fileTypes != null && fileTypes.Length > 0)
            {
                if (!fileTypes.Contains(Path.GetExtension(fileName).ToLower()))
                {
                    string fileFormat = string.Join(",", fileTypes);
                    result.BoolResult = false;
                    result.AddBrokenRuleMessage("Error", MessageManager.FormatValueByKey("FileInvalid", fileFormat));
                    return null;
                }
            }
            fileEntity.DisplayName = fileName;
            fileEntity.Extension = Path.GetExtension(fileEntity.DisplayName);

            fileEntity.Size = file.ContentLength;
            fileEntity.DbName = fileEntity.DisplayName.Replace("&", "").Replace("#", "").Replace(fileEntity.Extension, "")
                + DateTime.Now.ToString("yyMMddHHmmssffff") + IdentityContext.UserID.ToString().PadLeft(5, '0') + Guid.NewGuid().ToString().Substring(0, 5) + fileEntity.Extension;
            fileEntity.FilePath = fileEntity.DbName;
            string SaveFileName = targetPath + fileEntity.DbName;
            fileEntity.FullPath = SaveFileName;
            file.SaveAs(SaveFileName);
            return fileEntity;
        }

    }
}
