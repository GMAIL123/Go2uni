using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.Controllers
{
    public class ClassTeacherController : Controller
    {
        // GET: ClassTeacher
        public ActionResult Index()
        {
            return View();
        }

        #region EditProfile CLASS TEACHER
        [HttpPost]
        public JsonResult EditProfile(long TID)
        {
            ResultInfo<EditProfileTeacher> ResultInfo = new ResultInfo<EditProfileTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            ClassTeacher PageObj = new ClassTeacher();

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
                ClassTeacher PageObj = new ClassTeacher();
                ResultInfo.Info = PageObj.UpdateTeacherProfile(Data);
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Class Teacher Message
        [HttpPost]
        public JsonResult getCurrentTeachersList(long SchoolId, long Teacher_UID)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ClassTeacherMessageProp prop = new ClassTeacherMessageProp();
            prop.details = classteacher.getActiveTeacherList(SchoolId, Teacher_UID);
            return Json(prop, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getTeacherMessages(long SenderId, long ReceiverId, long SchoolId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ClassTeacherMessageProp prop = new ClassTeacherMessageProp();
            prop = classteacher.getClassTeacherMsg(SenderId, ReceiverId, SchoolId);
            return Json(prop, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertClassTeacherMessages(ClassTeacherMessages messages)
        {
            ClassTeacher classteacher = new ClassTeacher();
            string Result = classteacher.InsertClassteacherMessagesByUID(messages);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getMessageCount(long SenderId, long ReceiverId, long SchoolId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            long Count = classteacher.getMessageCount(SenderId, ReceiverId, SchoolId);
            return Json(Count, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getSubjectTeacherListCount(long UserId, long SchoolId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            long Count = classteacher.getSubjectTeacherListCount(UserId, SchoolId);
            return Json(Count, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region TimeTable

        [HttpPost]
        public JsonResult TimeTableOnInitialization(timetable_prop prop)
        {
            ClassTeacher classteacher = new ClassTeacher();
            AddStudentDetails result = new AddStudentDetails(){

                ClassTeacherLsiting=classteacher.GetAllAssignedClassTeachers(prop)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLevelDetailsOnDivisionId(int Div)
        {
            ClassTeacher classteacher = new ClassTeacher();
            AddStudentDetails result = classteacher.getLevelDetails(Div);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetClassByLevelAndDivisionId(long SchoolId, int DivId, int LvlId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            AddStudentDetails result = classteacher.getClassDetails(SchoolId, DivId, LvlId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BindSubjectOnClassIDAndLevelID(long SchoolId, long ClassId, int LvlId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            AddStudentDetails result = classteacher.getSubjectDetails(SchoolId, ClassId, LvlId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSemesterDetails()
        {
            ClassTeacher classteacher = new ClassTeacher();
            AddStudentDetails result = classteacher.getSemesterDetails();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTopicSemesterWise(long SubjectId, int SemesterId, long ClassId, int Level_Id, long SchoolID)
        {
            ClassTeacher classteacher = new ClassTeacher();
            AddStudentDetails result = classteacher.getTopicDetails(SubjectId, SemesterId, ClassId, Level_Id, SchoolID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetClassTeacherTimeTable(timetable_prop prop)
        {
            ClassTeacher classteacher = new ClassTeacher();
            List<ClassTeacherTimetable> list = classteacher.getClassTeacherTimeTable(prop);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
        [HttpPost]
        public JsonResult CheckForMailExist(EditProfileTeacher prfle){
            bool Result = true;
            ClassTeacher teacher = new ClassTeacher();
            if (teacher.checkForExistingmail(prfle) == false)
            {
                Result = false;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #region Class Teacher Forum
        [HttpPost]
        public JsonResult GetTeacherForumGroupsList(long SchoolID)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.GetTeacherForumGroupsList(SchoolID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateOnlineForumGroup(ClassTeacherForumGroups Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.CreateOnlineGroup(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetForumTopicListGroupIDWise(long Forum_ID, long SchoolID, string Forum_Name)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.GetGroupWiseTopic(Forum_ID, SchoolID, Forum_Name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateClassTeacherForumTopic(ClassTeacherTopic Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.CreateGroupWiseTopic(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTopicWiseComment(long TopicId, long SchoolId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.GetTopicWiseComment(TopicId, SchoolId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertCommentGroupTopicWise(ClassTeacherTopicComment Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.InsertCommentGroupTopicWise(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GroupCommentLikeOrDislike(ClassTeacherTopicComment Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.GroupCommentLikeOrDislike(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditClassTeacherCommunityComment(ClassTeacherTopicComment Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.EditOnlineForumComment(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteCommunityComment(long CommentId)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.DeleteCommunityComment(CommentId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReportCommunityComment(ClassTeacherCommunityCommentReport Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherForum> result = classteacher.ReportCommunityComment(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Curriculum

        [HttpPost]
        public JsonResult GetSubjectClassWise(TeachersCurriculum Info){
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<TeachersCurriculum> result = classteacher.GetSubjectDetailsClassWise(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCurriculumClassWise(TeachersCurriculum Info){
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<TeachersCurriculum> result = classteacher.GetCurriculumClassWise(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Feedback

        [HttpPost]
        public JsonResult GetTeachersAssignmentSubjectWise(ClassTeacherAssignmentDetails Info){
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherAssignmentDetails> result = classteacher.GetTeachersAssignmentSubjectWise(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTeachersTopicClassWise(ClassTeacherAssignmentDetails Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherAssignmentDetails> result = classteacher.GetSubjectWiseTopic(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentsAssignmentFeedback(ClassTeacherAssignmentDetails Info)
        {
            ClassTeacher classteacher = new ClassTeacher();
            ResultInfo<ClassTeacherAssignmentDetails> result = classteacher.GetTeacherAssignmentDetails(Info);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}