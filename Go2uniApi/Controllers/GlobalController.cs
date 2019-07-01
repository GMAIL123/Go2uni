using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.Controllers
{
    public class GlobalController : Controller
    {
        [HttpPost]
        public JsonResult GetAllNewsfeed(long SID)
        {
            ResultInfo<NewsfeedDetails> ResultInfo = new ResultInfo<NewsfeedDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            NewsfeedDetails temp = new NewsfeedDetails();
            Global PageObj = new Global();
            temp.Newsfeed = PageObj.ShowAllNewsfeed();
            temp.GoalDetails = PageObj.goaldetailsForNewsfeed(SID);
            temp.ExamForNewsfeed = PageObj.ExamForNewsfeed();
            temp.feedlist = PageObj.LeftpanelFeedDetails(SID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Divition ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #region Next College Days and event
        [HttpPost]
        public JsonResult GetAllCollegeEvent()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetAllCollegeEvent();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllNextCollegeUpcoming()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetAllNextCollegeUpcoming();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllNextCollegePast()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetAllNextCollegePast();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllNextCollegeCurrent()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetAllCollegeEvent();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllEventUpcoming()
        {
            ResultInfo<List<Event>> ResultInfo = new ResultInfo<List<Event>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetAllEventUpcoming();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllEventCurrent()
        {
            ResultInfo<List<Event>> ResultInfo = new ResultInfo<List<Event>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetAllEventCurrent();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEventPastData()
        {
            ResultInfo<List<Event>> ResultInfo = new ResultInfo<List<Event>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetEventPastData();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCollegeDayDetailsByID(long id)
        {
            NextCollegeDay temp = new NextCollegeDay();
            ResultInfo<NextCollegeDay> ResultInfo = new ResultInfo<NextCollegeDay>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            temp.CollegeDetails = PageObj.GetCollegeEventDetailsByID(id);
            temp.CollegeCommentList = PageObj.GetCollegeComments(id);
            ResultInfo.Info = temp;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEventDayDetailsByID(long id)
        {
            NextCollegeDay temp = new NextCollegeDay();
            ResultInfo<NextCollegeDay> ResultInfo = new ResultInfo<NextCollegeDay>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            temp.EventDetails = PageObj.GetEventDetailsByID(id);
            temp.EventCommentList = PageObj.GetEventComments(id);
            ResultInfo.Info = temp;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertEventComment(EventComment Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    Global PageObj = new Global();
                    ResultInfo.Info = PageObj.InsertEventComment(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insert Topic";
                        ResultInfo.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Description = ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertCollegeDayComment(NextCollegeDayComment Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    Global PageObj = new Global();
                    ResultInfo.Info = PageObj.InsertCollegeDayComment(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insert Topic";
                        ResultInfo.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Description = ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult InsertNote(Notes Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                Global PageObj = new Global();
                ResultInfo.Info = PageObj.InsertNote(Info);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Inserted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNote(long UserID)
        {
            ResultInfo<List<Notes>> ResultInfo = new ResultInfo<List<Notes>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.GetNote(UserID);

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Notes By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveNote(long NoteID, long UserID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (UserID > 0 && NoteID > 0)
            {
                Global PageObj = new Global();
                ResultInfo.Info = PageObj.RemoveNote(UserID, NoteID);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Deleted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DataForCalender(long UserID)
        {
            ResultInfo<List<Data_Calender>> ResultInfo = new ResultInfo<List<Data_Calender>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Global PageObj = new Global();
            ResultInfo.Info = PageObj.DataForCalender(UserID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Result";
                ResultInfo.Status = true;

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

    }
}