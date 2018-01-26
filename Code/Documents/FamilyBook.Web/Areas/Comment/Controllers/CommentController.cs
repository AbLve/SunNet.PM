using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FamilyBook.Business.Comment;
using FamilyBook.Entity;
using FamilyBook.Web.Controllers;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;

namespace PM.Document.Web.Areas.Comment.Controllers
{
    public class CommentController : BaseController
    {

        private CommentBusiness business = new CommentBusiness();
        private UsersEntity _usersEntity = null;

        private UsersEntity UserSEntity
        {
            get
            {
                if (_usersEntity == null)
                    _usersEntity = (new UserApplication()).GetUser(UserID);
                return _usersEntity;
            }
        }

        public ActionResult Index(int documentId, string fileName, string author, string time)
        {
            ViewBag.documentId = documentId;
            ViewBag.fileName = fileName;
            ViewBag.author = author;
            ViewBag.time = time;
            ViewBag.userName = UserSEntity.FirstName + " " + UserSEntity.LastName;
            return View();
        }

        public JsonResult GetList(int documentID)
        {
            List<CommentEntity> commentList =new List<CommentEntity>();
            //若UserType是‘CLIENT’,刷选掉CN的评论
            if (UserSEntity.UserType == "CLIENT")
                commentList = business.GetList(f => f.DocumentID == documentID&&f.Office!="CN");
            else
                commentList=business.GetList(f => f.DocumentID == documentID);

            //若UserType是‘CLIENT’,刷选掉CN的回复
            if (UserSEntity.UserType == "CLIENT")
            {
                foreach (var item in commentList)
                {
                    item.ReplyList = item.ReplyList.Where(r => r.Office != "CN").ToList();
                }
            }
            return Json(commentList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加评论或回复
        /// </summary>
        /// <param name="replyID"></param>
        /// <param name="content">评论或回复内容</param>
        /// <returns>包含ID，Name和Time的Json对象</returns>
        public JsonResult InsertComment(int documentID,int replyID, string content)
        {
            CommentEntity entity = new CommentEntity();
            entity.Name = UserSEntity.FirstName + " " + UserSEntity.LastName;
            entity.Content = content;
            entity.ReplyID = replyID;
            entity.Office = UserSEntity.Office;
            entity.UserType = UserSEntity.UserType;
            entity.UserID = UserSEntity.UserID;
            entity.DocumentID = documentID;
            int id = business.Insert(entity);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", id);
            dic.Add("Name", entity.Name);
            dic.Add("CreatedTime", System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            dic.Add("Office",entity.Office);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="ID">评论ID</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteComment(int ID)
        {
            return business.Delete(ID);
        }

        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(string filename)
        {
            ViewBag.filename = filename;
            ViewBag.filecount = Request.Files.Count;
            return View();
        }

    }
}
