using OntapCK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Drawing.Printing;

namespace OntapCK.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetList(string searchVal,int pageSize , int currentPage)
        {
            var list = data.UserInfos.Where(u => u.name.Contains(searchVal));
            int totalPage = (int)Math.Ceiling((double)list.Count() / pageSize);
            var userlist = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.totalPage = totalPage;
            return Content(JsonConvert.SerializeObject(new {userlist,totalPage}));
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public string GetObject(string id)
        {
            UserInfo user = new UserInfo();
            user = data.UserInfos.Where(u => u.id.Equals(id)).FirstOrDefault();
            return JsonConvert.SerializeObject(user);
        }
        public string CreateUser()
        {
            Result_ett<string> rs = new Result_ett<string>();
            try
            {
                string name = Request["name"];
                string country = Request["country"];
                string salary = Request["salary"];
                string email = Request["email"];
                UserInfo user = new UserInfo();
                user.name = name;
                user.country = country;
                user.salary = salary;
                user.email = email;
                data.UserInfos.InsertOnSubmit(user);
                data.SubmitChanges();
                rs.ErrCode = EnumErrCode.Success;
                rs.Message = "Thêm thành công";
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDetail = ex.Message;
                rs.Message = "Thêm không thành công";
                return JsonConvert.SerializeObject(rs);
            }
            return JsonConvert.SerializeObject(rs);
        }

        public string UpdateUser()
        {
            Result_ett<string> rs = new Result_ett<string>();
            try
            {   
                string id = Request["id"];
                string name = Request["name"];
                string country = Request["country"];
                string salary = Request["salary"];
                string email = Request["email"];
                UserInfo user = data.UserInfos.Where(u => u.id.Equals(id)).FirstOrDefault();
                user.name = name;
                user.country = country;
                user.salary = salary;
                user.email = email;
                data.SubmitChanges();
                rs.ErrCode = EnumErrCode.Success;
                rs.Message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDetail = ex.Message;
                rs.Message = "Cập nhật không thành công";
                return JsonConvert.SerializeObject(rs);
            }
            return JsonConvert.SerializeObject(rs);
        }
        public string Delete(string id)
        {
            Result_ett<string> rs = new Result_ett<string>();
            try
            {
                UserInfo userdel = data.UserInfos.Where(u => u.id.Equals(id)).FirstOrDefault();
                data.UserInfos.DeleteOnSubmit(userdel);
                data.SubmitChanges();
                rs.ErrCode = EnumErrCode.Success;
                rs.Message = "Xóa thành công";
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Fail;
                rs.ErrDetail = ex.Message;
                rs.Message = "Xóa không thành công";
                return JsonConvert.SerializeObject(rs);
            }
            return JsonConvert.SerializeObject(rs);
        }
    }
}