using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go2uniApi.Models;
using Go2uniApi.CodeFile;

namespace Go2uniApi.Controllers
{
    public class SiteAdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAllStudent()
        {
            User_Backend bck_end = new User_Backend();
            List<studentList> list = bck_end.fetchallStudentList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMessagesById(long eid){
            User_Backend bck_end = new User_Backend();
            studentList list=bck_end.getStudentById(eid);
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertSiteAdminComment(Messages_Admin list){
            User_Backend bck_end = new User_Backend();
            bool response=bck_end.InsertSiteAdminMessages(list);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMessagesByIdForUserPart(long eid)
        {
            User_Backend bck_end = new User_Backend();
            studentList list = bck_end.getStudentByIdForUserPart(eid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #region Talk to tutor By Indranil 05/02/2019

        public JsonResult InsertMessages(Messages_Admin Info){
            string res = String.Empty;
            User_Backend bck_end = new User_Backend();
            bool response = bck_end.InsertUserMessages(Info);
            if (response){
                res = "success";
            }else{
                res = "failed";
            }
            return Json(res,JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Site admin livechat update

        public JsonResult refreshSiteAdminChatUpdate(){
            User_Backend bck_end = new User_Backend();
            List<studentList> list = bck_end.fetchLiveUserMesagesByName();
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region REPORT COMMENT from SITE ADMIN,  SM 
        [HttpPost]
        public JsonResult GetReportList()
        {
            ResultInfo<ReportTab> ResultInfo = new ResultInfo<ReportTab>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                ReportTab temp = new ReportTab();
                User_Backend obj = new User_Backend();
                temp.comrep = obj.GetAllReports();

                ResultInfo.Info = temp;

                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| List Of reports fetched";
                    ResultInfo.Status = true;
                }

            }
            catch (Exception)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        #region UPDATE REPORT STATUS
        [HttpPost]
        public JsonResult ReportStatus(ReportTab Info)
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
                    User_Backend Obj = new User_Backend();
                    ResultInfo.Info = Obj.ChangeReportStatus(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Report Registered";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #endregion

        #region site admin Newsfeed by Indranil

        public JsonResult getNewsFeedForSiteAdmin()
        {
            User_Backend bck_end = new User_Backend();
            List<Newsfeed> result = bck_end.getAllNewsfeedForSiteAdmin();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult toggleStatus(long id)
        {
            User_Backend bck_end = new User_Backend();
            string result = bck_end.toggleStatus(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateNewsById(Newsfeed feed)
        {
            User_Backend bck_end = new User_Backend();
            string result = bck_end.UpdateNews(feed);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteNewsById(long id)
        {
            User_Backend bck_end = new User_Backend();
            string result = bck_end.DeleteNews(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertNews(Newsfeed feed)
        {
            User_Backend bck_end = new User_Backend();
            string result = bck_end.insertNews(feed);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region site admin Events By Indranil

        public JsonResult getEventsforSiteAdmin()
        {
            User_Backend bck_end = new User_Backend();
            List<ViewEvent> events = bck_end.getEventsforSiteAdmin();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getViewEvent(long ID)
        {
            User_Backend bck_end = new User_Backend();
            ViewEvent ev = bck_end.getEventsView(ID);
            return Json(ev, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateEventDetails(ViewEvent _evObj)
        {
            User_Backend bck_end = new User_Backend();
            string result = bck_end.updateEvent(_evObj);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteEvents(long ID)
        {
            User_Backend bck_end = new User_Backend();
            bool result = bck_end.deleteEvent(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddEventSiteAdmin(ViewEvent _evObj)
        {
            User_Backend bck_end = new User_Backend();
            string result = bck_end.InsertEvent(_evObj);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ADD QUESTIONS BY SITE ADMIN By Indranil

        [HttpPost]
        public JsonResult ViewExamType()
        {
            ResultInfo<List<ExamTypeDetails>> ResultInfo = new ResultInfo<List<ExamTypeDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ExamTypeDetails> temp = new List<ExamTypeDetails>();
            Social_Student PageObj = new Social_Student();
            temp = PageObj.ViewExamType();
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ViewSubject(long ExamID)
        {
            ResultInfo<List<ExamSubject>> ResultInfo = new ResultInfo<List<ExamSubject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ExamSubject> temp = new List<ExamSubject>();
            Social_Student PageObj = new Social_Student();
            temp = PageObj.ViewSubjectbyExamID(ExamID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTopicTypeBySubjectTypeForMockTest(long ExamID, long SubjectID)
        {
            ResultInfo<List<ExamTopic>> ResultInfo = new ResultInfo<List<ExamTopic>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ExamTopic> nm = new List<ExamTopic>();
            Social_Student PageObj = new Social_Student();
            if (ExamID > 0)
            {

                nm = PageObj.GetTopicTypeBySubjectTypeForMockTest(ExamID, SubjectID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetYearBySubjectTypeForQuestionPanel()
        {
            ResultInfo<List<string>> ResultInfo = new ResultInfo<List<string>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            QuestionPanel nm = new QuestionPanel();
            User_Backend PageObj = new User_Backend();

            nm.Year = PageObj.GetYearBySubjectTypeForQuestionPanel();
            ResultInfo.Info = nm.Year;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult getQuestionListByYear(Questionlist Info)
        {
            User_Backend bck_end = new User_Backend();
            List<AddTestQuestion> listobj = bck_end.GetfilteredQuestionsByYear(Info);
            return Json(listobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTopicBySubjectTypeQuesPanel(long SubId)
        {
            User_Backend bck_end = new User_Backend();
            List<newTopicList> listobj = bck_end.GetfilteredQuestionsByTopic(SubId);
            return Json(listobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetfilteredQuestionByTopic(Questionlist Info)
        {
            User_Backend bck_end = new User_Backend();
            List<AddTestQuestion> Obj = bck_end.GetfilteredQuestionByTopic(Info);
            return Json(Obj, JsonRequestBehavior.AllowGet);
        }  
       
        [HttpPost]
        public JsonResult deleteSiteAdminQuestion(string ID, string Type)
        {
            User_Backend bck_end = new User_Backend();
            bool result = bck_end.deleteQuestions(ID, Type);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult getQuestionView(string ID, string QuestionType)
        {
            User_Backend bck_end = new User_Backend();
            AddTestQuestion Obj = new AddTestQuestion();
            if (QuestionType == "topic")
            {
                Obj = bck_end.getQuestionViewTopicWise(ID, QuestionType);

            }
            if (QuestionType == "year")
            {
                Obj = bck_end.getQuestionViewYearWise(ID, QuestionType);

            }

            return Json(Obj, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult getQuestionUpdateView(string ID, string QuestionType)
        {
            User_Backend bck_end = new User_Backend();
            AddTestQuestion Obj = new AddTestQuestion();
            if (QuestionType == "topic")
            {
                Obj = bck_end.getQuestionViewTopicWise(ID, QuestionType);

            }
            if (QuestionType == "year")
            {
                Obj = bck_end.getQuestionViewYearWise(ID, QuestionType);

            }
            return Json(Obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SiteAdminQuestionUpdate(AddTestQuestion questions)
        {
            User_Backend bck_end = new User_Backend();
            string result = "";
            if (questions.QuestionType == "topic")
            {
                result = bck_end.UpdateQuestionsTopicWise(questions);
            }
            if (questions.QuestionType == "year")
            {
                result = bck_end.UpdateQuestionsYearWise(questions);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ADD QUESTIONS BY SITE ADMIN By Sneha

        [HttpPost]
        public JsonResult ViewExamTypeandQuesType()
        {
            ResultInfo<QuestionPanel> ResultInfo = new ResultInfo<QuestionPanel>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            QuestionPanel obj = new QuestionPanel();

            Social_Student PageObj = new Social_Student();
            User_Backend bck = new User_Backend();


            obj.ExamTypeDetails = PageObj.ViewExamType();
            obj.typeOfQues = bck.GetQuestionType();

            ResultInfo.Info = obj;


            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTopicTypeBySubjectTypeForQuestionPanel(long ExamID, long SubjectID)
        {
            ResultInfo<List<ExamTopic>> ResultInfo = new ResultInfo<List<ExamTopic>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ExamTopic> nm = new List<ExamTopic>();
            Social_Student PageObj = new Social_Student();
            if (ExamID > 0)
            {

                nm = PageObj.GetTopicTypeBySubjectTypeForMockTest(ExamID, SubjectID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult InsertQuesBySiteAdmin(AddTestQuestion Info)
        {
            var text1 = "";
            var text2 = "";
            var text3 = "";
            var demo1 = "topic";
            var demo2 = "year";
            var demo3 = "both";



            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    User_Backend PageObj = new User_Backend();
                    if (Info.ExamPattern == "both")
                    {
                        ResultInfo.Info = PageObj.InsertQuestionTopicWise(Info);
                        if (ResultInfo.Info != null)
                        {
                            text1 = ResultInfo.Info;
                        }

                        ResultInfo.Info = PageObj.InsertQuestionYearWise(Info);
                        if (ResultInfo.Info != null)
                        {
                            text2 = ResultInfo.Info;
                        }
                        text3 = text1 + "+" + text2;

                        if (ResultInfo.Info != null)
                        {
                            ResultInfo.Description = "Success| Insert Questions";
                            ResultInfo.Status = true;
                        }
                        ResultInfo.Info = demo3 + "|" + text3;

                    }
                    if (Info.ExamPattern == "topic")
                    {
                        ResultInfo.Info = PageObj.InsertQuestionTopicWise(Info);

                        if (ResultInfo.Info != null)
                        {
                            ResultInfo.Description = "Success| Insert Questions";
                            ResultInfo.Status = true;



                        }
                        ResultInfo.Info = demo1 + "|" + ResultInfo.Info;
                    }
                    if (Info.ExamPattern == "year")
                    {
                        ResultInfo.Info = PageObj.InsertQuestionYearWise(Info);

                        if (ResultInfo.Info != null)
                        {
                            ResultInfo.Description = "Success| Insert Questions";
                            ResultInfo.Status = true;
                        }
                        ResultInfo.Info = demo2 + "|" + ResultInfo.Info;

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

        #region SubjectDetails
        [HttpPost]
        public JsonResult SubjectDetails()

        {
            ResultInfo<SubjectDetails_SiteAdmin> ResultInfo = new ResultInfo<SubjectDetails_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SubjectDetails_SiteAdmin temp = new SubjectDetails_SiteAdmin();
            User_Backend PageObj = new User_Backend();

            temp.ExamSubject = PageObj.SubjectDetailsForSiteAdmin();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSubject()
        {
            ResultInfo<SubjectDetails_SiteAdmin> ResultInfo = new ResultInfo<SubjectDetails_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SubjectDetails_SiteAdmin temp = new SubjectDetails_SiteAdmin();
            User_Backend PageObj = new User_Backend();


            temp.ExamTypeDetails = PageObj.GetExamTypeForSiteAdmin();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Result ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddSubjectBySiteAdmin(ExamSubject Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Data != null)
                {
                    User_Backend Obj = new User_Backend();


                    ResultInfo.Info = Obj.AddSubjectBySiteAdmin(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insertion Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditSubject(long SID)
        {
            ResultInfo<EditSubject_SiteAdmin> ResultInfo = new ResultInfo<EditSubject_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            User_Backend PageObj = new User_Backend();
            EditSubject_SiteAdmin temp = new EditSubject_SiteAdmin();

            if (SID != 0)
            {
                temp = PageObj.EditSubjectBySiteAdmin(SID);
                temp.ExamTypeDetails = PageObj.GetExamTypeForSiteAdmin();
                ResultInfo.Info = temp;


                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSubjectBySiteAdmin(EditSubject_SiteAdmin Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Data != null)
                {
                    User_Backend Obj = new User_Backend();


                    ResultInfo.Info = Obj.UpdateSubjectBySiteAdmin(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insertion Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSubjectBySiteAdmin(long ID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (ID != 0)
                {
                    User_Backend Obj = new User_Backend();
                    ResultInfo.Info = Obj.DeleteSubjectBySiteAdmin(ID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubIDBySiteAdmin(long SID)
        {
            ResultInfo<SubjectDetails_SiteAdmin> ResultInfo = new ResultInfo<SubjectDetails_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            User_Backend PageObj = new User_Backend();
            SubjectDetails_SiteAdmin temp = new SubjectDetails_SiteAdmin();

            if (SID != 0)
            {

                temp.SID = PageObj.GetSubIDBySiteAdmin(SID);
                ResultInfo.Info = temp;


                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ReportView By Indranil
        public JsonResult getReportView(long ID)
        {
            User_Backend bck_end = new User_Backend();
            ReportTab result = bck_end.getReportView(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getReportCount()
        {
            User_Backend bck_end = new User_Backend();
            long res = bck_end.getReportCount();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add syllabus by sneha

        [HttpPost]
        public JsonResult AddSyllabusBySiteAdmin(AddSyllabus Info)
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
                    User_Backend PageObj = new User_Backend();
                    ResultInfo.Info = PageObj.InsertSyllabus(Info);

                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insert Syllabus";
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
        public JsonResult GetSyllabusList(SyllabusList Info)
        {
            ResultInfo<SyllabusList> ResultInfo = new ResultInfo<SyllabusList>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            try
            {
                SyllabusList temp = new SyllabusList();
                User_Backend back = new User_Backend();
                temp.sylllist = back.GetSyllabusListBySubId(Info);


                ResultInfo.Info = temp;

                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| List Of syllabus";
                    ResultInfo.Status = true;
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }





        [HttpPost]
        public JsonResult EditSyllabus(long ID, long TopicID)
        {
            ResultInfo<SyllabusList> ResultInfo = new ResultInfo<SyllabusList>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SyllabusList temp = new SyllabusList();
            User_Backend PageObj = new User_Backend();
            Social_Student social = new Social_Student();
            if (ID != 0)
            {


                temp = PageObj.EditSyllabusContent(ID, TopicID);
                if (temp.ExamID > 0)
                {
                    temp.ExamTypeDetails = social.ViewExamType();

                }
                if (temp.SubjectID > 0)
                {
                    temp.ExamSubject = social.ViewSubjectbyExamID(temp.ExamID);
                }

                ResultInfo.Info = temp;



                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Chapter ";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateSyllabusBySiteAdmin(AddSyllabus Info)
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
                    User_Backend PageObj = new User_Backend();
                    ResultInfo.Info = PageObj.UpdateSyllabus(Info);

                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insert Questions";
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
        public JsonResult DeleteSyllabusBySiteAdmin(AddSyllabus Info)
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
                    User_Backend PageObj = new User_Backend();
                    ResultInfo.Info = PageObj.DeleteSyllabus(Info);

                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insert Questions";
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

        #region Delete site admin reports by Indranil
        public JsonResult deleteSiteAdminReports(long id, string type)
        {
            User_Backend bck_end = new User_Backend();
            string response = bck_end.deleteSiteAdminReports(id, type);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit Newsfeed by Indranil

        public JsonResult getNewsForSiteAdminView(long ID)
        {
            User_Backend bck_end = new User_Backend();
            Newsfeed result = bck_end.getNewsForSiteAdminView(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SiteAdmin last message 18.03.2019
        public JsonResult getSiteAdminLastMessageCount(long UserId)
        {
            long Count = 0;
            User_Backend bckend = new User_Backend();
            Count = bckend.getSiteAdminMessageCount(UserId);
            return Json(Count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSiteAdminLastMessage(long UserId)
        {
            User_Backend bckend = new User_Backend();
            SocialOrSiteAdminUserlastMessage mesasge = bckend.SiteAdminLastMessages(UserId);
            return Json(mesasge, JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkForSiteAdminNewMessages()
        {
            User_Backend bckend = new User_Backend();
            long Count = bckend.getAllSiteAdminMessagesCount();
            return Json(Count, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region User

        [HttpPost]
        public JsonResult UserDetails()

        {
            ResultInfo<UserDetails_SiteAdmin> ResultInfo = new ResultInfo<UserDetails_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            UserDetails_SiteAdmin temp = new UserDetails_SiteAdmin();
            User_Backend PageObj = new User_Backend();

            temp.UserDetails = PageObj.UserDetailsForSiteAdmin();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUser()
        {
            ResultInfo<UserDetails_SiteAdmin> ResultInfo = new ResultInfo<UserDetails_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            UserDetails_SiteAdmin temp = new UserDetails_SiteAdmin();
            User_Backend PageObj = new User_Backend();


            temp.Login_Type = PageObj.GetLoginTypeForSiteAdmin();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Result ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUserBySiteAdmin(Admin_User Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Data != null)
                {
                    User_Backend Obj = new User_Backend();


                    ResultInfo.Info = Obj.AddUserBySiteAdmin(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insertion Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUser(long UID)
        {
            ResultInfo<AddUser_SiteAdmin> ResultInfo = new ResultInfo<AddUser_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            User_Backend PageObj = new User_Backend();
            AddUser_SiteAdmin temp = new AddUser_SiteAdmin();

            if (UID != 0)
            {
                temp = PageObj.EditUserBySiteAdmin(UID);
                temp.Login_Type = PageObj.GetLoginTypeForSiteAdmin();
                ResultInfo.Info = temp;


                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUserBySiteAdmin(AddUser_SiteAdmin Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Data != null)
                {
                    User_Backend Obj = new User_Backend();


                    ResultInfo.Info = Obj.UpdateUserBySiteAdmin(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Update Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUserBySiteAdmin(long ID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (ID != 0)
                {
                    User_Backend Obj = new User_Backend();
                    ResultInfo.Info = Obj.DeleteUserBySiteAdmin(ID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

    

        [HttpPost]
        public JsonResult NextcollegeDetails()

        {
            ResultInfo<CollegeDetails_SiteAdmin> ResultInfo = new ResultInfo<CollegeDetails_SiteAdmin>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CollegeDetails_SiteAdmin temp = new CollegeDetails_SiteAdmin();
            User_Backend PageObj = new User_Backend();

            temp.CollegeDay = PageObj.collegeDetailsForSiteAdmin();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertCollegeDetails(CollegeDay Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Data != null)
                {
                    User_Backend Obj = new User_Backend();


                    ResultInfo.Info = Obj.InsertCollegeDetails(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insertion Sucess";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditCollegeDetails(long ID)
        {
            ResultInfo<CollegeDay> ResultInfo = new ResultInfo<CollegeDay>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            User_Backend PageObj = new User_Backend();


            if (ID != 0)
            {


                ResultInfo.Info = PageObj.EditCollegeDetails(ID);


                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateCollegeDetails(CollegeDay Data)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Data != null)
                {
                    User_Backend Obj = new User_Backend();


                    ResultInfo.Info = Obj.UpdateCollegeDetails(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Update Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCollegeBySiteAdmin(long ID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (ID != 0)
                {
                    User_Backend Obj = new User_Backend();
                    ResultInfo.Info = Obj.DeleteCollegeBySiteAdmin(ID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

    }
}