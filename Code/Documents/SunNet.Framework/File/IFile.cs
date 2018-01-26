using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using SF.Framework.Result;

namespace SF.Framework.File
{
    public interface IFile
    {
        void Move(string source, string destination);
        void Copy(string source, string destination);
        bool Delete(string filePath);
        bool Delete(string filePath, string thumbPath);
        string Upload(HttpPostedFile fileData, string fileFolder, string filePhysicalPath);
        FileEntity UploadReturnFileEntity(HttpPostedFile fileData, string fileFolder, string filePhysicalPath, string virtualDir="");
        string DESEncryptUploadFile(HttpPostedFile fileData, string fileFolder, string filePhysicalPath);
        FileEntity DESEncryptUploadFileReturnFileEntity(HttpPostedFile fileData, string fileFolder, string filePhysicalPath, string virtualDir = "");
        byte[] DESDecryptFile(string filePhysicalPath);

        string UploadVideo(HttpPostedFile fileData, string fileFolder, string filePhysicalPath);
        void DeleteFile(string path);
        FileEntity UploadImageThumbnailByWidth(HttpPostedFile fileData, string fileFolder,string physicalpath, int width, int height, string prefix = "");
        FileEntity UploadImageThumbnail2(HttpPostedFile fileData, string fileFolder,string filePhysicalPath, int width, int height, string prefix = "");
        FileEntity UploadCutImage(string inputFileName, string outputFileName, string fileFolder, string filePhysicalPath, int toWidth, int toHeight, int cropWidth, int cropHeight, int x, int y);
        
        FileEntity UploadFile(HttpFileCollection files, out JObject result, string[] fileTypes, string fileFolder, string fileID);
        FileEntity UploadFile(HttpFileCollection files, out FunctionResult result, string[] fileTypes, string fileFolder, string fileID);
        FileEntity UploadFile(HttpFileCollection files, out ResultEntity result, string[] fileTypes, string fileFolder, string fileID);
    }
}
