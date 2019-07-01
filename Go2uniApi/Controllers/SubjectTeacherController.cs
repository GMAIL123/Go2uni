using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.Controllers
{
    public class SubjectTeacherController : Controller
    {
        // GET: SubjectTeacher
        public ActionResult Index()
        {
            return View();
        }

        #region EditProfile SUBJECT TEACHER
        [HttpPost]
        public JsonResult EditProfile(long TID)
        {
            ResultInfo<EditProfileTeacher> ResultInfo = new ResultInfo<EditProfileTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            SubjectTeacher PageObj = new SubjectTeacher();

            ResultInfo.Info = PageObj.EditProfile(TID);



            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Details ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTeacherProfile(EditProfileTeacher Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Data != null)
            {
                SubjectTeacher PageObj = new SubjectTeacher();
                // Data.JoiningDate = Convert.ToDateTime(Data.JoiningDate);
                ResultInfo.Info = PageObj.UpdateTeacherProfile(Data);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success!Update Successfull";
                    ResultInfo.Status = true;
                }
                else
                {
                    ResultInfo.Description = "Failed!Process Failed";
                    ResultInfo.Status = false;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}