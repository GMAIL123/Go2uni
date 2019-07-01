using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.Controllers
{
    public class HodController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertTeacherNote(TeacherNote Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                Hod PageObj = new Hod();
                ResultInfo.Info = PageObj.InsertTeacherNote(Info);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Inserted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTeacherNotes(long TeacherID_FK)
        {
            ResultInfo<List<TeacherNote>> ResultInfo = new ResultInfo<List<TeacherNote>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Hod PageObj = new Hod();
            ResultInfo.Info = PageObj.GetTeacherNotes(TeacherID_FK);

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Notes By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveTeacherNote(long TeacherID_FK, long NoteID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (TeacherID_FK > 0 && NoteID > 0)
            {
                Hod PageObj = new Hod();
                ResultInfo.Info = PageObj.RemoveTeacherNote(TeacherID_FK, NoteID);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Deleted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TeachersPlanner(long ID)
        {
           
            ResultInfo<TeacherPlan> ResultInfo = new ResultInfo<TeacherPlan>()
            {
                Status = false,
                Description = "Failed|Login",
                ErrorCode = 400,
            };
            try
            {
                TeacherPlan temp = new TeacherPlan();
                Hod obj = new Hod();
                temp.TopicCoverDate = obj.GetAllTeacherTopics(ID);
                temp.AssignmentDueDate= obj.GetAssignmentDetails(ID);
                temp.TestDueDate= obj.GetAllTestDueDate(ID);

                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Topic Cover";
                    ResultInfo.Status = true;
                    ResultInfo.ErrorCode = 200;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #region Teacher Time table
        [HttpPost]
        public JsonResult GetTimeTable(long ID)
        {
            ResultInfo<TimeTableDetails> ResultInfo = new ResultInfo<TimeTableDetails>()
            {
                Status = false,
                Description = "Failed|Login",
                ErrorCode = 400,
            };
            try
            {
                TimeTableDetails temp = new TimeTableDetails();
                Hod obj = new Hod();
                temp.WeekList = obj.GetAllWeekdays();
                temp.PeriodList = obj.GetAllperiods();
                temp.TimeTableList = obj.GetAllTimeTableList(ID);
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get time table details";
                    ResultInfo.Status = true;
                    ResultInfo.ErrorCode = 200;
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region EditProfile HOD
        [HttpPost]
        public JsonResult EditProfile(long TID)
        {
            ResultInfo<EditProfileTeacher> ResultInfo = new ResultInfo<EditProfileTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Hod PageObj = new Hod();

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
                Hod PageObj = new Hod();
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