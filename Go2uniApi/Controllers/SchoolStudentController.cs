using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go2uniApi.CodeFile;
using Go2uniApi.Models;


namespace Go2uniApi.Controllers
{
    public class SchoolStudentController : Controller
    {
        // GET: School_Student
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetLevelListByDivisionID(int DivID)
        {
            ResultInfo<List<Level>> ResultInfo = new ResultInfo<List<Level>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Level> nm = new List<Level>();
            SchoolStudent PageObj = new SchoolStudent();
            if (DivID > 0)
            {

                nm = PageObj.GetlevelListByDivID(DivID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get level name with id";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult GetClassListByLevelID(int Level_ID,long School_ID)
        {
            ResultInfo<List<ClassDetails>> ResultInfo = new ResultInfo<List<ClassDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ClassDetails> nm = new List<ClassDetails>();
            SchoolStudent PageObj = new SchoolStudent();
            if (Level_ID > 0)
            {

                nm = PageObj.GetClassByLevelID(Level_ID,School_ID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EditProfileSchoolStu(long UID,long SchoolID)
        {
            ResultInfo<SchoolStudentEdit> ResultInfo = new ResultInfo<SchoolStudentEdit>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent obj = new SchoolStudent();
            SchoolStudentEdit demo = new SchoolStudentEdit();
            try
            {
                if (UID > 0)
                {
                    demo = obj.EditSchoolStudentProfile(UID);
                    if (demo.DivisionID > 0)
                    {
                        demo.Levels = obj.GetlevelListByDivID(demo.DivisionID);
                    }
                    if (demo.Level_ID > 0)
                    {
                        demo.ClassObj = obj.GetClassByLevelID(demo.Level_ID,demo.School_ID);
                    }
                    demo.GenderInfo = obj.GetAllGender();
                    demo.Divisions = obj.GetDivisionList();
                }
                ResultInfo.Info = demo;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get all School Student Details";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {

               
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SchoolStudentUpdateProfile(SchoolStudentEdit Info)
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
                    SchoolStudent Obj = new SchoolStudent();
                    ResultInfo.Info = Obj.UpdateSchoolStudentProfile(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Updation Sucess";
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
        public JsonResult SchlStdntCurriculum(long UID, long SchoolID)
        {
            ResultInfo<SchlStuCurriCulum> ResultInfo = new ResultInfo<SchlStuCurriCulum>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent obj = new SchoolStudent();
            SchlStuCurriCulum demo = new SchlStuCurriCulum();
            try
            {
                
                if (UID > 0)
                {
                   // demo = obj.GetSchoolCurriculum(UID, SchoolID);
                   // if (demo.DivisionID > 0)
                    //{
                    //    demo.Levels = obj.GetlevelListByDivID(demo.DivisionID);
                    //}
                    demo.Divisions = obj.GetDivisionList();
                    //if (demo.Divisions.FirstOrDefault().ID > 0)
                    //{
                    //    demo.Levels = obj.GetlevelListByDivID(demo.Divisions.FirstOrDefault().ID);
                    //    if (demo.Levels.FirstOrDefault().ID > 0)
                    //    {
                    //        demo.SchoolSub = obj.SubjectByLevelID(demo.Levels.FirstOrDefault().ID, SchoolID);
                    //        if (demo.SchoolSub.FirstOrDefault().ID > 0)
                    //            demo = obj.GetSchoolCurriculum(demo.SchoolSub.FirstOrDefault().ID, SchoolID);
                    //    }
                    //}
                }
                ResultInfo.Info = demo;
            }
            catch (Exception ex)
            {

                
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSubjectListByLevel(int LevelID,long SchoolID)
        {
            ResultInfo<List<SchoolSubjects>> ResultInfo = new ResultInfo<List<SchoolSubjects>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<SchoolSubjects> nm = new List<SchoolSubjects>();
            SchoolStudent PageObj = new SchoolStudent();
            if (LevelID > 0)
            {

                nm = PageObj.SubjectByLevelID(LevelID, SchoolID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get  all Sub list";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public JsonResult GetCurriculum(long subjectID, long SchoolID)
        {
            ResultInfo<SchlStuCurriCulum> ResultInfo = new ResultInfo<SchlStuCurriCulum>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            //  List<SchoolSubjects> nm = new List<SchoolSubjects>();
            SchlStuCurriCulum demo = new SchlStuCurriCulum();
            SchoolStudent PageObj = new SchoolStudent();
            if (subjectID > 0 && SchoolID>0)
            {

                demo.classTopic = PageObj.GetSchoolCurriculum(subjectID, SchoolID);
                
            }
            ResultInfo.Info = demo;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get  all Sub list";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SubjectsByClassID(long SchoolID, long ClassID)
        {
            ResultInfo<ClassForum> ResultInfo = new ResultInfo<ClassForum>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ClassForum obj = new ClassForum();
            SchoolStudent pageobj = new SchoolStudent();
            if (SchoolID > 0 && ClassID > 0)
            {
                obj.subs = pageobj.SubjectByClassId(SchoolID, ClassID);
            }
            ResultInfo.Info = obj;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        #region CLASS FORUM BY SNEHA

        [HttpPost]
        public JsonResult SubForumListByClassID(long SchoolID,long ClassID)
        {
            ResultInfo<List<SubjectForum>> ResultInfo = new ResultInfo<List<SubjectForum>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent PageObj = new SchoolStudent();


            ResultInfo.Info = PageObj.SubjectForumByClassID(SchoolID, ClassID);

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get all community By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TopicsListByForumID(long ForumID,long SchoolID, long ClassID)
        {
            ResultInfo<ForumDetails> TopicList = new ResultInfo<ForumDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<SubjectForumTopics> AllTopicList = new List<SubjectForumTopics>();
            ForumDetails DetailsInfo = new ForumDetails();
            SchoolStudent PageObj = new SchoolStudent();
            AllTopicList = PageObj.TopicListByForumID(ForumID,SchoolID,ClassID); // topics list

            if (AllTopicList != null && AllTopicList.Count > 0)
            {
                DetailsInfo.topicList = AllTopicList;
                DetailsInfo.Forum = AllTopicList.FirstOrDefault().ForumName;
                TopicList.Info = DetailsInfo;
                TopicList.Description = "Success| Get all topic by communityID";
                TopicList.Status = true;
            }
            return Json(TopicList, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult InsertTopicByForumID(SubjectForumTopics Info)
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
                    SchoolStudent PageObj = new SchoolStudent();
                    ResultInfo.Info = PageObj.InsertTopicByForumID(Info);
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



        //[HttpPost]
        //public JsonResult GetCommentByTopicID(long TopicID, long GroupID)
        //{
        //    TopicWiseComment temp = new TopicWiseComment();
        //    ResultInfo<TopicWiseComment> ResultInfo = new ResultInfo<TopicWiseComment>()
        //    {
        //        Status = false,
        //        Description = "Failed|Login"
        //    };
        //    if (TopicID > 0)
        //    {
        //        Social_Student PageObj = new Social_Student();
        //        temp.Comments = PageObj.GetCommentByTopicID(TopicID, GroupID);
        //        temp.Topics = PageObj.GetTopicByID(TopicID);
        //        //temp.ProfileImage = PageObj.ProfileImgForSgTopicList(temp.comm.UID);
        //        temp.Stu_Profile_Image = PageObj.ProfileImgForSgCommentList(temp.Group_Comment_ID);
        //        ResultInfo.Info = temp;
        //        if (ResultInfo.Info != null)
        //        {
        //            ResultInfo.Description = "Success| Get Comments by Topic ID";
        //            ResultInfo.Status = true;
        //        }
        //    }
        //    return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        //}



        [HttpPost]
        public JsonResult ForumTopicComments(long TopicID)
        {
            ResultInfo<ForumDetails> CommentList = new ResultInfo<ForumDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<SubjectForumTopicComment> AllCommentList = new List<SubjectForumTopicComment>();
            ForumDetails DetailsInfo = new ForumDetails();
           // List<ForumTopicComment> CommentDetails = new List<ForumTopicComment>();
            List<SubjectForumTopics> TopicDetails = new List<SubjectForumTopics>();

            SchoolStudent PageObj = new SchoolStudent();
            AllCommentList = PageObj.AllCommentsOfTopic(TopicID);
            DetailsInfo.comments = AllCommentList;
          //  DetailsInfo.ComID=AllCommentLi
           // DetailsInfo.singleComment=
            TopicDetails = PageObj.getDetailsOfComment(TopicID);
            if (TopicDetails != null && TopicDetails.Count > 0)
            {
                DetailsInfo.Topic = TopicDetails.FirstOrDefault().TopicName;
                DetailsInfo.TopicCreatedByName = TopicDetails.FirstOrDefault().CreatedByName;
                DetailsInfo.TopicCreatedDate = TopicDetails.FirstOrDefault().TopicCreatedDate;
                //DetailsInfo.Topic_CreatedDate = TopicDetails.FirstOrDefault().Topic_CreatedDate;
                DetailsInfo.TopicID = TopicDetails.FirstOrDefault().TopicID;
                DetailsInfo.IsStudent = TopicDetails.FirstOrDefault().IsStudent;
                DetailsInfo.IsTeacher = TopicDetails.FirstOrDefault().IsTeacher;
            }
            CommentList.Info = DetailsInfo;
            CommentList.Description = "Success| Get all comment by TopicId";
            CommentList.Status = true;
            return Json(CommentList, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult InsertCommentByTopicID(SubjectForumTopicComment Info)
        {
            //List<SubjectForumTopicComment> CommentInfo = new List<SubjectForumTopicComment>();
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    SchoolStudent PageObj = new SchoolStudent();
                    ResultInfo.Info = PageObj.InsertCommentByTopicID(Info);
                    if (ResultInfo.Info != null)
                    {



                        ResultInfo.Description = "Success| Insert comment";
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
        public JsonResult AddLikeORDislikeOFComment(SubjectForum_Like_OR_Dislike Info)
        {
            OnlineCommunityComment CommentInfo = new OnlineCommunityComment();
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    SchoolStudent PageObj = new SchoolStudent();
                    ResultInfo.Info = PageObj.InsertLikeOrDislikeValueOfSubjectForum(Info);
                    ResultInfo.Info = PageObj.UpdateLikesDislikesInCommentTable(Info.CommentID, Info.LikeVal,Info.UID);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Insert Like or dislike";
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




        public JsonResult RefreshLikeDislike(long CommentID)
        {
            ResultInfo<SubjectForumTopicComment> ResultInfo = new ResultInfo<SubjectForumTopicComment>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            SchoolStudent PageObj = new SchoolStudent();
            SubjectForumTopicComment temp = new SubjectForumTopicComment();

            if (CommentID != 0)
            {
                temp = PageObj.GetLikeDislikeCount(CommentID);
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
        public JsonResult EditSubForumComment(long ForumCommID, long UID)

        {
            ResultInfo<ForumDetails> ResultInfo = new ResultInfo<ForumDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ForumDetails temp = new ForumDetails();
            SchoolStudent obj = new SchoolStudent();
            try
            {
                if (ForumCommID > 0 && UID > 0)
                {


                    temp.singleComment = obj.EditSubForumComment(ForumCommID, UID);
                    ResultInfo.Info = temp;
                }
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details Of SocialStudent";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSubForumComment(SubjectForumTopicComment Info)
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
                    SchoolStudent Obj = new SchoolStudent();
                    ResultInfo.Info = Obj.UpdateCommentByUID(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Updation Sucess";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }



        public JsonResult RefreshComment(long CommentID, long UID)
        {
            ResultInfo<SubjectForumTopicComment> ResultInfo = new ResultInfo<SubjectForumTopicComment>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            SchoolStudent PageObj = new SchoolStudent();
            SubjectForumTopicComment temp = new SubjectForumTopicComment();

            if (CommentID != 0)
            {
                temp = PageObj.RefreshCommentByUserID(CommentID, UID);
                ResultInfo.Info = temp;


                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        //Community Comment delete//
        [HttpPost]
        public JsonResult DeleteSubForumComment(long UID, long CommID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (UID > 0 && CommID > 0)
                {
                    SchoolStudent PageObj = new SchoolStudent();
                    ResultInfo.Info = PageObj.DeleteComment(UID, CommID);
                    // ResultInfo.Info = PageObj.UpdateStudygrpCommentstatus(Info);
                    
                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| deleted";
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

        #region ASSIGNMENT BY SNEHA

        [HttpPost]
        public JsonResult GetSubjectsForAssignment(long ClassID , long SchoolID)
        {
            ResultInfo<STUDENT_ASSIGNMENTS> ResultInfo = new ResultInfo<STUDENT_ASSIGNMENTS>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent pageobj = new SchoolStudent();
            STUDENT_ASSIGNMENTS assign = new STUDENT_ASSIGNMENTS();
            if(SchoolID>0 && ClassID>0)
            {
                assign.Level_ID = pageobj.GetDivIdByClassID(ClassID, SchoolID);
                if (assign.Level_ID > 0)
                {
                    assign.subjs = pageobj.GetSubjectByLevelID(assign.Level_ID);
                }
            }
            ResultInfo.Info = assign;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SubForAssignmentUpload(int level_id)
        {
            ResultInfo<StudentUploadAssignment> ResultInfo = new ResultInfo<StudentUploadAssignment>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent pageobj = new SchoolStudent();
            StudentUploadAssignment upld = new StudentUploadAssignment();
            if (level_id > 0)
            {
                upld.subjs = pageobj.GetSubjectByLevelID(level_id);
            }
            ResultInfo.Info = upld;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult TopicListBySubID(long SubjectID)
        {
            ResultInfo<List<SchoolSyllabusTopic>> ResultInfo = new ResultInfo<List<SchoolSyllabusTopic>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<SchoolSyllabusTopic> nm = new List<SchoolSyllabusTopic>();
            SchoolStudent PageObj = new SchoolStudent();
            if (SubjectID > 0)
            {

                nm = PageObj.GetTopicsForAssignment(SubjectID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get level name with id";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetAssignmentList(long Topic_ID)
        {
            ResultInfo<List<ClassTeacherAssignmentDetails>> ResultInfo = new ResultInfo<List<ClassTeacherAssignmentDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ClassTeacherAssignmentDetails> nm = new List<ClassTeacherAssignmentDetails>();
            SchoolStudent PageObj = new SchoolStudent();
            if (Topic_ID > 0)
            {

                // nm = PageObj.GetTopicsForAssignment(SubjectID);
                nm = PageObj.Assignments(Topic_ID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get level name with id";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UploadAssignments(StudentUploadAssignment Info)
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
                    SchoolStudent Obj = new SchoolStudent();
                    ResultInfo.Info = Obj.UploadAssignment(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Updation Sucess";
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
        public JsonResult HomeWorkList(long SubjectID)
        {
            ResultInfo<STUDENT_ASSIGNMENTS> ResultInfo = new ResultInfo<STUDENT_ASSIGNMENTS>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent pageobj = new SchoolStudent();
            STUDENT_ASSIGNMENTS upld = new STUDENT_ASSIGNMENTS();
            if (SubjectID > 0)
            {
                upld.Asslist = pageobj.HomeworkList(SubjectID);
            }
            ResultInfo.Info = upld;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }



        #endregion


        #region RATE LESSON
        [HttpPost]
        public JsonResult SubjectsForLessonRating(long ClassID, long SchoolID)
        {
            ResultInfo<StudentRateLesson> ResultInfo = new ResultInfo<StudentRateLesson>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent pageobj = new SchoolStudent();
            StudentRateLesson assign = new StudentRateLesson();
            if (SchoolID > 0 && ClassID > 0)
            {
                //assign.LevelID = pageobj.GetDivIdByClassID(ClassID, SchoolID);
                //if (assign.LevelID > 0)
                //{
                assign.subjs = pageobj.SubForTeacherRating(ClassID, SchoolID);
                //}
            }
            ResultInfo.Info = assign;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult TopicsForStudentRating(long SubjectID)
        //{
        //    ResultInfo<StudentRateLesson> ResultInfo = new ResultInfo<StudentRateLesson>()
        //    {
        //        Status = false,
        //        Description = "Failed|Login"
        //    };
        //    SchoolStudent obj = new SchoolStudent();
        //    StudentRateLesson rate = new StudentRateLesson();
        //    if (SubjectID > 0)
        //    {
        //        rate.tpcs = obj.GetTopicsForAssignment(SubjectID);
        //    }

        //    return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        //}

            
        [HttpPost]
        public JsonResult GetTeacherID(long TopicID)
        {
            ResultInfo<StudentRateLesson> ResultInfo = new ResultInfo<StudentRateLesson>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SchoolStudent pageobj = new SchoolStudent();
            StudentRateLesson assign = new StudentRateLesson();
            if (TopicID > 0)
            {
                assign.SubjectID = pageobj.GetSubIdByTopicID(TopicID);
                if (assign.SubjectID > 0)
                {
                    assign.TeacherID = pageobj.GetTeacherIDBySubID(assign.SubjectID);
                }
            }
            ResultInfo.Info = assign;
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertRatingByStudent(StudentRateLesson Info)
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
                    SchoolStudent Obj = new SchoolStudent();
                    ResultInfo.Info = Obj.InsertRating(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Updation Sucess";
                        ResultInfo.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
        
               // throw;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region KeyDates

        
        #endregion

    }
}