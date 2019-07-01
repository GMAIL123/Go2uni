using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.Controllers
{
    public class Social_StudentController : Controller
    {
        [HttpPost]
        public JsonResult GetSubject(long Stu_ID)
        {
            ResultInfo<List<Student_Syllabus>> ResultInfo = new ResultInfo<List<Student_Syllabus>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            ResultInfo.Info = PageObj.GetSubject(Stu_ID);

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Subject ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChapter(long SyllabusID)
        {
            ResultInfo<List<Chapter_Syllabus>> ResultInfo = new ResultInfo<List<Chapter_Syllabus>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            ResultInfo.Info = PageObj.GetChapter(SyllabusID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Chapter ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditProfile(long ID)
        {
            ResultInfo<EditProfile> ResultInfo = new ResultInfo<EditProfile>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            EditProfile temp = new EditProfile();
            Social_Student PageObj = new Social_Student();
            if (ID != 0)
            {

                //else
                //{
                //    temp.ClassInfo = PageObj.GetAllClass();
                //}
                temp = PageObj.GetDetailsOFStudent(ID);
                if (temp.DivisionID > 0)
                {
                    temp.Levels = PageObj.GetlevelListByDivID(temp.DivisionID);
                }
                if (temp.Level_ID > 0)
                {
                    temp.ClassDetails = PageObj.GetAllClassSt(temp.Level_ID);
                }



                //if (obj.Level_ID > 0)
                //{
                //    obj.ClassDetails = SupAdm.GetAllClassSt(obj.Level_ID);
                //}
                temp.GenderInfo = PageObj.GetAllGender();
                temp.Divisions = PageObj.GetDivisionList();
                ResultInfo.Info = temp;


                //temp.GenderInfo = PageObj.GetAllGender();
                //temp.Division = PageObj.GetAllDivision();
                ////temp.SemList = PageObj.GetAllSemester();
                //temp.StreamList = PageObj.GetStreamList();
                //ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Chapter ";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLevelListByDivisionID(int ID)
        {
            ResultInfo<List<Level>> ResultInfo = new ResultInfo<List<Level>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Level> nm = new List<Level>();
            Social_Student PageObj = new Social_Student();
            if (ID > 0)
            {

                nm = PageObj.GetLevel(ID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetClassBylevelID(int ID)
        {
            ResultInfo<List<ClassDetails>> ResultInfo = new ResultInfo<List<ClassDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetAllClassSt(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStudentProfile(EditProfile Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                // Studfoent PageObj = new Student();
                //ResultInfo.Info = PageObj.UpdateStudentProfile(In);
                Global obj = new Global();
                ResultInfo.Info = obj.UserDetailsUpdate(Info);
                if (Info != null)
                {
                    // Global obj = new Global();
                    //ResultInfo.Info = obj.UserDetailsUpdate(Info);
                    CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
                    ResultInfo.Info = PageObj.UpdateStudentProfile(Info);
                }
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Chapter ";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSectionByClass(long ID)
        {
            ResultInfo<List<SectionInfo>> ResultInfo = new ResultInfo<List<SectionInfo>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetSectionByClass(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get section name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetClassByDivID(long ID)
        {
            ResultInfo<List<ClassInfo>> ResultInfo = new ResultInfo<List<ClassInfo>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetClassBydivID(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MailSend()
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ResultInfo.Info = BaseClass.Mail("aion.das@esolzmail.com", new EmailTemplate());
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #region Goal
        [HttpPost]
        public JsonResult Goal(long SID)
        {
            ResultInfo<Goal> ResultInfo = new ResultInfo<Goal>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Goal temp = new Goal();
            Social_Student PageObj = new Social_Student();

            temp.Levels = PageObj.GetAllLevel();
            //temp.GoalDetails = PageObj.GetGoalDetails(SID);

            temp.Semester = PageObj.GetAllSemester();
            temp.ExamTypeDetails = PageObj.GetExamTypeForGoal();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Divition ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ClassByLevelID(int ID)
        {
            ResultInfo<List<ClassDetails>> ResultInfo = new ResultInfo<List<ClassDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.ClassByLevelID(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStreamByClassID(int ID)
        {
            ResultInfo<List<ClassStream>> ResultInfo = new ResultInfo<List<ClassStream>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetStreamList(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Streme name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubjectByClassID(int ID)
        {
            ResultInfo<List<Subject_Syllabus>> ResultInfo = new ResultInfo<List<Subject_Syllabus>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.SubjectByClassID(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get sub name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubjectByClassIDAndStreamID(int ID, int StreamID)
        {
            ResultInfo<List<Subject_Syllabus>> ResultInfo = new ResultInfo<List<Subject_Syllabus>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetSubjectByClassIDAndStreamID(ID, StreamID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get sub name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TopicBySubID(int SubID)
        {
            ResultInfo<List<Syllabus_TopicDetails>> ResultInfo = new ResultInfo<List<Syllabus_TopicDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            if (SubID > 0)
            {
                ResultInfo.Info = PageObj.TopicBySubID(SubID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Topic with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubTopicByTopic(int ID)
        {
            ResultInfo<List<Syllabus_Content>> ResultInfo = new ResultInfo<List<Syllabus_Content>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.SubTopicByTopic(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get sub name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubTopicByTopicID(int ID)
        {
            ResultInfo<List<Syllabus_Subtopic>> ResultInfo = new ResultInfo<List<Syllabus_Subtopic>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetSubTopicByTopicID(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get SubTopic with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertGoalDetails(long UserID, string StartDate, string TopicID, string EndDate)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (UserID != 0)
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.InsertGoalDetails(UserID, StartDate, TopicID, EndDate);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success!Insert Successfull";
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

        public JsonResult SubjectByExamID(long ID)
        {
            ResultInfo<List<ExamSubject>> ResultInfo = new ResultInfo<List<ExamSubject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.SubjectByExamID(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get sub name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TopicByExamSub(long ID)
        {
            ResultInfo<List<ExamTopic>> ResultInfo = new ResultInfo<List<ExamTopic>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.TopicByExamSub(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get topic name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GoalDetails(long UID)

        {
            ResultInfo<Goal> ResultInfo = new ResultInfo<Goal>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Goal temp = new Goal();
            Social_Student PageObj = new Social_Student();

            temp.GoalDetails = PageObj.GetGoalDetails(UID);

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewGoal(long ID)
        {
            ResultInfo<ViewGoal> ResultInfo = new ResultInfo<ViewGoal>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            ViewGoal temp = new ViewGoal();

            if (ID != 0)
            {
                temp = PageObj.ViewGoal(ID);



                ResultInfo.Info = temp;

                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditGoal(long GID)
        {
            ResultInfo<ViewGoal> ResultInfo = new ResultInfo<ViewGoal>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            ViewGoal temp = new ViewGoal();

            if (GID != 0)
            {
                temp = PageObj.EditGoal(GID);
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
        public JsonResult UpdateGoal(ViewGoal Data)

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
                    Social_Student Obj = new Social_Student();



                    ResultInfo.Info = Obj.UpdateGoal(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull ";
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

        #region ReportCard

        

        [HttpPost]
        public JsonResult ReportCard(long SID)
        {
            ResultInfo<ReportCard_SocialStudent> ResultInfo = new ResultInfo<ReportCard_SocialStudent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ReportCard_SocialStudent temp = new ReportCard_SocialStudent();
            Social_Student PageObj = new Social_Student();

            temp.ExamTypeDetails = PageObj.GetExamTypeForGoal();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Divition ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReportCardRecordTopicWise(long UID, long TopicID, string Exam, string Subject, string Topic)

        {
            ResultInfo<ReportCard_SocialStudent> ResultInfo = new ResultInfo<ReportCard_SocialStudent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ReportCard_SocialStudent temp = new ReportCard_SocialStudent();
            Social_Student PageObj = new Social_Student();

            temp.Record = PageObj.RecordforTopicWise(UID, TopicID);
            temp.HighestScore = PageObj.HighestScoreForTopicWise(TopicID);
            temp.LowestScore = PageObj.LowestScoreForTopicWise(TopicID);
            temp.AvarageScore = PageObj.AvarageScoreForTopicWise(TopicID);

            temp.NumberOfStudent = PageObj.GetNumberOfStudentTopicWise();

            temp.Points = PageObj.GraphPointsTopicWise(UID, TopicID);
            temp.NumberOfAttempted = PageObj.GetNumberOfAttemptedTopicWise(UID);

            temp.Exam = Exam;
            temp.Subject = Subject;
            temp.Topic = Topic;

            temp.PointsForTimeGraph = PageObj.PointsForTimeGraphTopicWise(UID, TopicID);

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReportCardRecordYearWise(long UID, string Year, long SubjectID, string Exam, string Subject)

        {
            ResultInfo<ReportCard_SocialStudent> ResultInfo = new ResultInfo<ReportCard_SocialStudent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ReportCard_SocialStudent temp = new ReportCard_SocialStudent();
            Social_Student PageObj = new Social_Student();

            temp.Record = PageObj.RecordforYearWise(UID, Year, SubjectID);
            temp.HighestScore = PageObj.HighestScoreForYearWise(Year, SubjectID);
            temp.LowestScore = PageObj.LowestScoreForYearWise(Year, SubjectID);
            temp.AvarageScore = PageObj.AvarageScoreForYearWise(Year, SubjectID);

            temp.NumberOfStudent = PageObj.GetNumberOfStudentYearWise();

            temp.Points = PageObj.GraphPointsYearWise(UID, SubjectID, Year);
            temp.NumberOfAttempted = PageObj.GetNumberOfAttemptedYearWise(UID);

            temp.Exam = Exam;
            temp.Subject = Subject;
            temp.Year = Year;

            temp.PointsForTimeGraph = PageObj.PointsForTimeGraphYearWise(UID, SubjectID, Year);


            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MOCKTEST

        [HttpPost]
        public JsonResult ReportForTopicwiseExam(string GUID, long UserID, long TopicID, long ExamID, long SubjectID, double Time)
        {
            ResultInfo<Feedback> ResultInfo = new ResultInfo<Feedback>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            Feedback temp = new Feedback();


            if (UserID != 0)
            {
                
                //temp.WrongAnswer = PageObj.GetWrongForTopicwiseExam(GUID, UserID);
                //temp.AttemedQuestion = PageObj.GetTotalForTopicwiseExam(GUID, UserID);


                temp.CorrectAnswer = PageObj.GetCorrectForTopicwiseExam(GUID, UserID);
              
                temp.TotalQuestionForTopic = PageObj.GetAllQuestionForTopicwiseExam(TopicID);

                double total = Convert.ToInt32(temp.TotalQuestionForTopic);
                double correct = Convert.ToInt32(temp.CorrectAnswer);

                double per = correct * 100 / total;
                double Percentage = Math.Round(per, 2);

                double Minutes = Time / 60;
                double Min = Math.Round(Minutes, 2);

                temp.RecordStore = PageObj.InsertMocktestRecordTopicWise(GUID, UserID, TopicID, ExamID, SubjectID, temp.CorrectAnswer, temp.TotalQuestionForTopic, Percentage, Min);

                //temp.ExamName = PageObj.ExamNameForReport(ExamID);
                //temp.SubjectName = PageObj.SubjectnameForReport(SubjectID);
                //temp.Topic = PageObj.TopicForReport(TopicID);

                temp.Review = PageObj.GetReviewforTopicWiseExam(GUID, UserID);
                temp.SubjectID = SubjectID;
                temp.TopicID = TopicID;
                temp.GUID = GUID;

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
        public JsonResult ReportForYearwiseExam(string GUID, long UID, string Year, long SubjectID, long ExamID, double Time)
        {
            ResultInfo<Feedback> ResultInfo = new ResultInfo<Feedback>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            Feedback temp = new Feedback();
            if (UID != 0)
            {
                //temp.CorrectAnswer = PageObj.FeedbackForYearwiseExamCorrectAnswer(GUID, UID);
                //temp.WrongAnswer = PageObj.FeedbackForYearwiseExamWrongAnswer(GUID, UID);
                temp.AttemedQuestion = PageObj.FeedbackForYearwiseExamAttemedQuestion(GUID, UID);

                temp.TotalQuestionForYear = PageObj.GetAllQuestionForYearwiseExam(Year, SubjectID);

                double total = Convert.ToInt32(temp.TotalQuestionForYear);
                double correct = Convert.ToInt32(temp.CorrectAnswer);

                double per = correct * 100 / total;
                double Percentage = Math.Round(per, 2);

                double minutes = Time / 60;
                double Min = Math.Round(minutes, 2);

                temp.RecordStore = PageObj.InsertMocktestRecordYearWise(GUID, UID, Year, ExamID, SubjectID, temp.CorrectAnswer, temp.TotalQuestionForYear, Percentage, Min);
                temp.Review = PageObj.GetReviewforYearcWiseExam(GUID, UID);

                //temp.ExamName = PageObj.ExamNameForReport(ExamID);
                //temp.SubjectName = PageObj.SubjectnameForReport(SubjectID);
                //temp.Year = Year;

                temp.SubjectID = SubjectID;
                temp.Year = Year;
                temp.GUID = GUID;

                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Results";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMockTest(long Id)
        {
            ResultInfo<MockQuestions> ResultInfo = new ResultInfo<MockQuestions>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                MockQuestions temp = new MockQuestions();
                CodeFile.Social_Student obj = new CodeFile.Social_Student();
                temp.Syllabus_Subjects = obj.GetSubjectByStudentID(Id);
                //temp.YearWiseQuess = obj.GetYearList();
                //temp.GetTopics = obj.GetTopicList();
                temp.DifficultyLevels = obj.GetDiffiCultyLevelList();
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get ALL Subject List ";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSubjectByStudentId(long Id)
        {
            ResultInfo<List<Syllabus_Subject>> ResultInfo = new ResultInfo<List<Syllabus_Subject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Syllabus_Subject> nm = new List<Syllabus_Subject>();
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (Id > 0)
            {

                nm = PageObj.GetSubjectByStudentID(Id);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Year BY SubjectId ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SelectMockTestType(int Id, string ExamPattern)
        {
            ResultInfo<List<CommonClass>> ResultInfo = new ResultInfo<List<CommonClass>>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            if (Id > 0)
            {
                CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
                if (ExamPattern == "year")
                {
                    ResultInfo.Info = PageObj.SelectYearBYSubId(Id);
                }
                else if (ExamPattern == "topic")
                {
                    ResultInfo.Info = PageObj.SelectTopicBySubId(Id);
                }

            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Year BY SubjectId ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult SelectDifficultyById(int IDtype, int TpcId, int Yearnm)
        //{
        //    ResultInfo<List<SelectDiffiCulty>> ResultInfo = new ResultInfo<List<SelectDiffiCulty>>()
        //    {
        //        Status = false,
        //        Description = "Failed|Login"
        //    };
        //     SelectDiffiCulty diif = new SelectDiffiCulty();
        //    if (IDtype > 0)
        //    {
        //        Student PageObj = new Student();
        //        if (IDtype == diif.Topic_ID)
        //        {
        //            ResultInfo.Info = PageObj.GetDiffiCultyBYTopicId(IDtype);
        //        }
        //        else if (IDtype==diif.Year)
        //        {
        //            ResultInfo.Info = PageObj.GetDiffiCultyByYearnm(IDtype);
        //        }

        //    }
        //    if (ResultInfo.Info != null)
        //    {
        //        ResultInfo.Description = "Success| Get Year BY SubjectId ";
        //        ResultInfo.Status = true;
        //    }
        //    return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult StartExam(int SubId, int Value, int DiffLevel, string ExamPattern)
        {
            ResultInfo<List<StartExam>> ResultInfo = new ResultInfo<List<StartExam>>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            CodeFile.Social_Student obj = new CodeFile.Social_Student();

            if (ExamPattern == "year")
            {

                ResultInfo.Info = obj.GetYearWiseQues(SubId, Value, DiffLevel);
            }
            else if (ExamPattern == "topic")
            {
                ResultInfo.Info = obj.GetTopicWiseQues(SubId, Value, DiffLevel);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Year BY SubjectId ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #region Ramashree 29/12/2018 Mock Test

        [HttpPost]
        public JsonResult GetSubjectTypeByExamType(long ExamID)
        {
            ResultInfo<List<ExamSubject>> ResultInfo = new ResultInfo<List<ExamSubject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ExamSubject> nm = new List<ExamSubject>();
            Social_Student PageObj = new Social_Student();
            if (ExamID > 0)
            {

                nm = PageObj.GetSubjectType(ExamID);
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
        public JsonResult ViewTopicWiseForMockTest(long ExamID, long SubjectID, long TopicID)
        {
            ResultInfo<List<QuestionsByTopic>> ResultInfo = new ResultInfo<List<QuestionsByTopic>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<QuestionsByTopic> temp = new List<QuestionsByTopic>();
            Social_Student PageObj = new Social_Student();
            temp = PageObj.ViewTopicWiseForMockTest(TopicID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetYearBySubjectTypeForMockTest()
        {
            ResultInfo<List<QuestionsByYear>> ResultInfo = new ResultInfo<List<QuestionsByYear>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<QuestionsByYear> nm = new List<QuestionsByYear>();
            Social_Student PageObj = new Social_Student();


            nm = PageObj.GetYearBySubjectTypeForMockTest();
            ResultInfo.Info = nm;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ViewYearWiseForMockTest(long ExamID, long SubjectID, string Year)
        {
            ResultInfo<List<QuestionsByYear>> ResultInfo = new ResultInfo<List<QuestionsByYear>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<QuestionsByYear> temp = new List<QuestionsByYear>();
            Social_Student PageObj = new Social_Student();
            temp = PageObj.ViewYearWiseForMockTest(Year, SubjectID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertAnswerForTopicQues(StudentAnswerByTopic Info)
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
                    Social_Student PageObj = new Social_Student();
                    StudentAnswerByTopic temp = new StudentAnswerByTopic();
                    temp.CorrectAnswer = PageObj.GetCorrectAnswerTopic(Info.QuestionID);
                    ResultInfo.Info = PageObj.InsertAnswerForTopicQues(Info, temp.CorrectAnswer);
                    if (ResultInfo.Info != null)
                    {
                        long var = Info.QuestionID;
                        ResultInfo.Description = "Success| answers inserted";
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
        public JsonResult InsertAnswerForYearQues(StudentAnswerByYear Info)
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
                    Social_Student PageObj = new Social_Student();
                    StudentAnswerByYear temp = new StudentAnswerByYear();
                    temp.CorrectAnswer = PageObj.GetCorrectAnswerYear(Info.QuestionID);
                    ResultInfo.Info = PageObj.InsertAnswerForYearQues(Info, temp.CorrectAnswer);
                    if (ResultInfo.Info != null)
                    {
                        long var = Info.QuestionID;
                        ResultInfo.Description = "Success| answers inserted";
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

        #endregion

        public JsonResult GetChapterForReportCard(int ID)
        {
            ResultInfo<List<ReportCardChapter>> ResultInfo = new ResultInfo<List<ReportCardChapter>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetChapterForReportCard(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #region STUDY GROUP, SM


        [HttpPost]
        public JsonResult GetGroupList(long UID)
        {

            ResultInfo<List<StudyGroupName>> ResultInfo = new ResultInfo<List<StudyGroupName>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetGroupNameByID(UID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get group list  By USERID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupNameByID(long UID, long gi)
        {
            GroupInfo groupTopic = new GroupInfo();
            ResultInfo<GroupInfo> ResultInfo = new ResultInfo<GroupInfo>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            groupTopic.Topics = PageObj.GetTopicByGroupID(gi, UID);
            groupTopic.ProfileImage = PageObj.ProfileImgForSgTopicList(UID);
            groupTopic.GroupID = gi;
            ResultInfo.Info = groupTopic;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Package By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetTopicByGroupID(long GroupID, long UID)
        {
            ResultInfo<GroupInfo> ResultInfo = new ResultInfo<GroupInfo>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            GroupInfo temp = new GroupInfo();
            Social_Student PageObj = new Social_Student();
            temp.Topics = PageObj.GetTopicByGroupID(GroupID, UID);
            temp.StudyPlanGoalsByGroupID = PageObj.GetStudyPlanGoalsByGroupID(GroupID);
            temp.GroupID = GroupID;
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Topic";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertTopicByGroupID(Topics Info)
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
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.InsertTopicByGroupID(Info);
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
        public JsonResult ExitStudyGroup(long UID, long GroupID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (UID > 0 && GroupID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.ExitFromGroupById(UID, GroupID);
                    // ResultInfo.Info = PageObj.UpdateStudygrpCommentstatus(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Exited from group";
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
        public JsonResult EditStudyGrpComment(long GroupCommID, long UID)

        {
            ResultInfo<TopicWiseComment> ResultInfo = new ResultInfo<TopicWiseComment>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            TopicWiseComment temp = new TopicWiseComment();
            Social_Student obj = new Social_Student();
            try
            {
                if (GroupCommID > 0 && UID > 0)
                {


                    temp.comm = obj.EditStudyGroupComment(GroupCommID, UID);
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
        public JsonResult UpdateStudyGroupComment(long UID, long GroupCommID, string Comment)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (UID > 0 && GroupCommID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.UpdateStudygrpComment(UID, GroupCommID, Comment);
                    // ResultInfo.Info = PageObj.UpdateStudygrpCommentstatus(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Exited from group";
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
        public JsonResult DeleteStudyGroupComment(long UID, long GroupCommID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (UID > 0 && GroupCommID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.DeleteStudyGroupComment(UID, GroupCommID);
                    // ResultInfo.Info = PageObj.UpdateStudygrpCommentstatus(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Exited from group";
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
        public JsonResult InsertCommentByTopicID(Comment Info)
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
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.InsertCommentByTopicID(Info);
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
        public JsonResult GetMemberListByGroupID(long GroupID)
        {
            ResultInfo<List<StudentInfo>> ResultInfo = new ResultInfo<List<StudentInfo>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            GroupInfo temp = new GroupInfo();
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetMemberListByGroupID(GroupID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Members";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertStudentNote(StudentNote Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.InsertStudentNote(Info);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Inserted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentNotes(long UID)
        {
            ResultInfo<List<StudentNote>> ResultInfo = new ResultInfo<List<StudentNote>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetStudentNotes(UID);

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Notes By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveStudentNote(long UID, long NoteID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (UID > 0 && NoteID > 0)
            {
                Social_Student PageObj = new CodeFile.Social_Student();
                ResultInfo.Info = PageObj.RemoveStudentNote(UID, NoteID);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Deleted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        //study group //
        [HttpPost]
        public JsonResult GetStudyPlanGoals()
        {
            ResultInfo<List<StudyPlanGoals>> ResultInfo = new ResultInfo<List<StudyPlanGoals>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetStudyPlanGoals();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        // study group//
        [HttpPost]
        public JsonResult InsertGroupStudyPlan(SetGroupStudyPlan Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.InsertGroupStudyPlan(Info);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Inserted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllCollegeEvent()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetAllCollegeEvent();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUpcomingCollegeEvent()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetUpcomingCollegeEvent();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpcomingEventForRightSide()
        {
            ResultInfo<List<Event>> ResultInfo = new ResultInfo<List<Event>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.UpcomingEventForRightSide();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetCollegeEventDetailsByID(long id)
        //{
        //    NextCollegeDay temp = new NextCollegeDay();
        //    ResultInfo<NextCollegeDay> ResultInfo = new ResultInfo<NextCollegeDay>()
        //    {
        //        Status = false,
        //        Description = "Failed|Login"
        //    };
        //    Student PageObj = new Student();
        //    temp.CollegeEvent = PageObj.GetCollegeEventDetailsByID(id);
        //    temp.EventComment = PageObj.GetEventComments(id);
        //    ResultInfo.Info = temp;
        //    return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult InsertEventComment(NextCollegeDayComment Info)
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
                    CodeFile.Social_Student PageObj = new CodeFile.Social_Student();
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

        // Group study create new group// 
        [HttpPost]
        public JsonResult AddNewGroup(StudyGroupName Info)
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
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.AddNewGroup(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Add New Group";
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
        public JsonResult GetAllActiveStudent(string Keyword)
        {
            ResultInfo<List<AutoComplete>> ResultInfo = new ResultInfo<List<AutoComplete>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.GetAllActiveStudent(Keyword);
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get all active student";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Description = ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        // Study Group//
        // [HttpPost]
        public JsonResult SentRequestGroupMember(RequestGroupMember Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.SentRequestGroupMember(Info);
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Request for join";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Description = ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPendingRequestForGroup(long UID)
        {
            ResultInfo<List<StudyGroupName>> ResultInfo = new ResultInfo<List<StudyGroupName>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.GetPendingRequestForGroup(UID);
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Pending request by student";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Description = ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptRequest(long GroupID, long UID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (GroupID > 0 && UID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.AcceptRequest(GroupID, UID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Accept Request";
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
        public JsonResult DeclineRequest(long GroupID, long UID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (GroupID > 0 && UID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.DeclineRequest(GroupID, UID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Decline Request";
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

        //delete study group
        [HttpPost]
        public JsonResult DeleteStudyGroup(long GroupID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (GroupID > 0)
                {
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.DeleteStudyGroup(GroupID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Group Info Updated";
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
        public JsonResult EditStudyGroup(long GroupID)
        {
            ResultInfo<StudyGroupName> ResultInfo = new ResultInfo<StudyGroupName>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            StudyGroupName obj = new StudyGroupName();
            Social_Student sst = new Social_Student();
            try
            {

                if (GroupID > 0)
                {
                    obj = sst.EditStudyGroup(GroupID);
                }
                ResultInfo.Info = obj;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details Of Group";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }
               
        // Update study group
        [HttpPost]
        public JsonResult UpdateGroupInfo(StudyGroupName Info)
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
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.UpdateStudyGroupInfo(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Group Info Updated";
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
        public JsonResult GetGroupTopicList(long UID, long gi)
        {
            GroupInfo groupTopic = new GroupInfo();
            ResultInfo<GroupInfo> ResultInfo = new ResultInfo<GroupInfo>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            groupTopic.Topics = PageObj.GetTopicByGroupID(gi, UID);
            groupTopic.ProfileImage = PageObj.ProfileImgForSgTopicList(UID);
            groupTopic.GroupID = gi;
            ResultInfo.Info = groupTopic;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Package By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        
        // edit topic name sm,, 3rd june//
        [HttpPost]
        public JsonResult EditSgTopicName(long TopicID, long GroupID)
        {
            ResultInfo<GroupInfo> ResultInfo = new ResultInfo<GroupInfo>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            GroupInfo temp = new GroupInfo();
            Social_Student obj = new Social_Student();
            try
            {
                if (TopicID > 0 && GroupID > 0)
                {
                    temp.TopicSingle = obj.EditSgTopicName(TopicID, GroupID);
                    ResultInfo.Info = temp;
                }
            }
            catch (Exception ex)
            {


            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
                     
        [HttpPost]
        public JsonResult UpdateSgTopicName(Topics Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (Info.ID > 0)
                {

                    Social_Student PageObj = new Social_Student();

                    ResultInfo.Info = PageObj.UpdateStudyGroupTopicName(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Updated topic name";
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
               
        //delete study group
        [HttpPost]
        public JsonResult DeleteStudyGroupTopic(long TopicID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (TopicID > 0)
                {
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.DeleteStudyGroupTopic(TopicID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Group Info Updated";
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
        public JsonResult GetCommentByTopicID(long TopicID, long GroupID)
        {
            TopicWiseComment temp = new TopicWiseComment();
            ResultInfo<TopicWiseComment> ResultInfo = new ResultInfo<TopicWiseComment>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (TopicID > 0)
            {
                Social_Student PageObj = new Social_Student();
                temp.Comments = PageObj.GetCommentByTopicID(TopicID, GroupID);
                temp.Topics = PageObj.GetTopicByID(TopicID);
                temp.Stu_Profile_Image = PageObj.ProfileImgForSgCommentList(temp.Group_Comment_ID);
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Comments by Topic ID";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RefreshSgTopic(long TopicID, long UID)
        {
            ResultInfo<Topics> ResultInfo = new ResultInfo<Topics>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            Topics temp = new Topics();

            if (TopicID != 0)
            {
                temp = PageObj.RefreshSgTopicByUserID(TopicID, UID);
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

        #region Ramashree 17/12/2018 EDIT profile
        [HttpPost]
        public JsonResult EditProfileSocialStudent(long ID)

        {
            ResultInfo<SocialStudentEditprofile> ResultInfo = new ResultInfo<SocialStudentEditprofile>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SocialStudentEditprofile obj = new SocialStudentEditprofile();
            Social_Student sst = new Social_Student();
            try
            {
                if (ID > 0)
                {
                    obj = sst.GetDetailsOFEditProfileSocialStudent(ID);
                    if (obj.DivisionID > 0)
                    {
                        obj.Levels = sst.GetlevelListByDivID(obj.DivisionID);
                    }
                    obj.GenderInfo = sst.GetAllGender();
                    obj.Divisions = sst.GetDivisionList();

                    ResultInfo.Info = obj;
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
        public JsonResult SocialStudentUpdateProfile(SocialStudentEditprofile Info)
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
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.UpdateSocialStudentEditProfile(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Updation Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        // get details of sg comment by comment id sm//
        [HttpPost]
        public JsonResult GetCommentDetails(TopicWiseComment Info)
        {

            ResultInfo<TopicWiseComment> ResultInfo = new ResultInfo<TopicWiseComment>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            TopicWiseComment temp = new TopicWiseComment();
            Social_Student Obj = new Social_Student();
            try
            {
                if (Info != null)
                {



                    temp = Obj.GetDetailsOfComment(Info);
                    ResultInfo.Info = temp;


                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "StudyGroup";
                        ResultInfo.Status = true;

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        //register report  from sg to report table sm//
        [HttpPost]
        public JsonResult ReportSgComment(TopicWiseComment Info)
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
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.InsertSgReportInTable(Info);
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

        #region Ramashree 21/12/2018 Exam Type
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
        public JsonResult ViewTopic(long SubjectID, long UserID_FK)
        {
            ResultInfo<Student_Syllabus> ResultInfo = new ResultInfo<Student_Syllabus>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            Student_Syllabus obj = new Student_Syllabus();
            obj.ExamTopic = PageObj.ViewTopicbySubjectID(SubjectID, UserID_FK);
            obj.ExamContent = PageObj.ViewTopicContentbySubjectID(SubjectID);
            obj.Subject = PageObj.GetSubjectforContent(SubjectID);
            ResultInfo.Info = obj;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Online Community

        [HttpPost]
        public JsonResult InsertTopicByStudentID(Online_Community Info)
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
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.InsertTopicByStudentID(Info);
                    //if (ResultInfo.Info.Split('!')[0] == "Success")
                    //{
                    //    Info.Community_ID = Convert.ToInt64(ResultInfo.Info.Split('!')[1]);
                    //    string Res = PageObj.JoinedstudentToparticularCommunity(Info);
                    //}
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
        public JsonResult AddLikeORDislikeOFComment(Online_Community_LikeORDisLike Info)
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
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.InsertLikeOrDislikeValueOfOnlineCommunity(Info);
                    ResultInfo.Info = PageObj.UpdateLikesDislikesInCommentTable(Info.Comment_Fk_ID, Info.LikeVal);

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



        // Get all community comments
        [HttpPost]
        public JsonResult GetallCommunityComments(long TopicID)
        {
            ResultInfo<CommunityDetails> CommentList = new ResultInfo<CommunityDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<OnlineCommunityComment> AllCommentList = new List<OnlineCommunityComment>();
            CommunityDetails DetailsInfo = new CommunityDetails();
            List<OnlineCommunityComment> CommentDetails = new List<OnlineCommunityComment>();
            List<OnlineCommunityTopic> TopicDetails = new List<OnlineCommunityTopic>();
            Social_Student PageObj = new Social_Student();
            AllCommentList = PageObj.GetAllCommentsOfTopic(TopicID);
            DetailsInfo.CommentList = AllCommentList;
            TopicDetails = PageObj.getDetailsOfComment(TopicID);
            if (TopicDetails != null && TopicDetails.Count > 0)
            {
                DetailsInfo.Topic = TopicDetails.FirstOrDefault().Topic_Discussion;
                DetailsInfo.CreatedBy = TopicDetails.FirstOrDefault().CreatedBy;
                DetailsInfo.tempTopic_CreatedDate = TopicDetails.FirstOrDefault().tempTopic_CreatedDate;
                DetailsInfo.Topic_ID = TopicDetails.FirstOrDefault().Topic_ID;
            }
            CommentList.Info = DetailsInfo;
            CommentList.Description = "Success| Get all comment by TopicId";
            CommentList.Status = true;
            return Json(CommentList, JsonRequestBehavior.AllowGet);
        }

        // Add new topic
        [HttpPost]
        public JsonResult InsertTopicByCommunityID(OnlineCommunityTopic Info)
        {
            List<OnlineCommunityTopic> TopicInfo = new List<OnlineCommunityTopic>();
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.InsertTopicByCommunityID(Info);
                    if (ResultInfo.Info != null)
                    {
                        // var studentID = Info.UID_CreatedBY;
                        var UID = Info.UID_CreatedBY;
                        TopicInfo = PageObj.GetTopicIDLastInserted(UID);
                        var TopicID = TopicInfo.FirstOrDefault().Topic_ID;
                        var Topic_Discussion = TopicInfo.FirstOrDefault().Topic_Discussion;
                        ResultInfo.Info = "Success!" + TopicID + "!" + Topic_Discussion;
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

        // Add new comment to a particular topic
        [HttpPost]
        public JsonResult InsertCommentByStudentIDForTopic(OnlineCommunityComment Info)
        {
            List<OnlineCommunityComment> CommentInfo = new List<OnlineCommunityComment>();
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info != null)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.InsertCommentByTopicID(Info);
                    if (ResultInfo.Info != null)
                    {



                        ResultInfo.Description = "Success| Insert comment ";
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


        //Join new community
        [HttpPost]
        public JsonResult JoinNewCommunity(Online_Community_Members Info)
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
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.JoinNewCommunityByID(Info);
                    if (ResultInfo.Info.Split('!')[0] == "Success")
                    {
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
        public JsonResult GetAllTopicsByCommunityID(long CommunityID)
        {
            ResultInfo<CommunityDetails> TopicList = new ResultInfo<CommunityDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<OnlineCommunityTopic> AllTopicList = new List<OnlineCommunityTopic>();
            CommunityDetails DetailsInfo = new CommunityDetails();
            Social_Student PageObj = new Social_Student();
            AllTopicList = PageObj.GetAllTopicListByCommunityID(CommunityID); //get all topic by community id

            if (AllTopicList != null && AllTopicList.Count > 0)
            {
                DetailsInfo.TopicList = AllTopicList;
                DetailsInfo.Community = AllTopicList.FirstOrDefault().CommunityName;
                TopicList.Info = DetailsInfo;
                TopicList.Description = "Success| Get all topic by communityID";
                TopicList.Status = true;
            }
            return Json(TopicList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCommunityDetailsWithID()
        {
            ResultInfo<List<Online_Community>> ResultInfo = new ResultInfo<List<Online_Community>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetAllCommunityListByID();//get list of community by user id edited 27thdec,18sm

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get all community By StudentID";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }



        // get community comment details by id sm 7th,feb//
        [HttpPost]
        public JsonResult GetCommunityCommentDetails(CommunityDetails Info)
        {

            ResultInfo<CommunityDetails> ResultInfo = new ResultInfo<CommunityDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CommunityDetails temp = new CommunityDetails();
            Social_Student Obj = new Social_Student();
            try
            {
                if (Info.Community_Comment_ID > 0)
                {



                    temp = Obj.GetDetailsOfCommunityComment(Info);
                    ResultInfo.Info = temp;


                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "OnlineCommunity";
                        ResultInfo.Status = true;

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        //register report of online community, SM//
        [HttpPost]
        public JsonResult ReportCommunityComment(CommunityDetails Info)
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
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.InsertCommunityReportInTable(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success! Report Registered";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        // edit topic name sm,, 3rd june//
        [HttpPost]
        public JsonResult EditCommunityTopicName(long TopicID)
        {
            ResultInfo<CommunityDetails> ResultInfo = new ResultInfo<CommunityDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CommunityDetails temp = new CommunityDetails();
            Social_Student obj = new Social_Student();
            try
            {
                if (TopicID > 0)
                {
                    temp.Singletopic = obj.EditCommunityTopicName(TopicID);
                    ResultInfo.Info = temp;
                }
            }
            catch (Exception ex)
            {


            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
               
        [HttpPost]
        public JsonResult UpdateCommunitytopic(OnlineCommunityTopic Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (Info.Topic_ID > 0)
                {

                    Social_Student PageObj = new Social_Student();

                    ResultInfo.Info = PageObj.UpdateCommunityTopicName(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Updated topic name";
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
                     
        public JsonResult RefreshCommunityTopic(long TopicID, long UID)
        {
            ResultInfo<OnlineCommunityTopic> ResultInfo = new ResultInfo<OnlineCommunityTopic>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            OnlineCommunityTopic temp = new OnlineCommunityTopic();

            if (TopicID != 0)
            {
                temp = PageObj.RefreshCommunityTopicByUserID(TopicID, UID);
                ResultInfo.Info = temp;


                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
               
        //delete study group
        [HttpPost]
        public JsonResult DeleteCommunityTopic(OnlineCommunityTopic Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (Info.Topic_ID > 0)
                {
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.DeleteCommunityTopic(Info);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Group Info Updated";
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

        #region Study Guide

        [HttpPost]
        public JsonResult GetCurrentStudyGuide(long ID)
        {
            ResultInfo<List<ViewGoal>> ResultInfo = new ResultInfo<List<ViewGoal>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetCurrentStudyGuide(ID);
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNotStartedStudyGuide(long ID)
        {
            ResultInfo<List<ViewGoal>> ResultInfo = new ResultInfo<List<ViewGoal>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetNotStartedStudyGuide(ID);
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCompletedStudyGuide(long ID)
        {
            ResultInfo<List<ViewGoal>> ResultInfo = new ResultInfo<List<ViewGoal>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.GetCompletedStudyGuide(ID);
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //Community Comment EDIT// 
        [HttpPost]
        public JsonResult EditCommunityComment(long CommunityCommID, long UID)

        {
            ResultInfo<CommunityDetails> ResultInfo = new ResultInfo<CommunityDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            CommunityDetails temp = new CommunityDetails();
            Social_Student obj = new Social_Student();
            try
            {
                if (CommunityCommID > 0 && UID > 0)
                {


                    //tem.Co = obj.EditCommunityComment()
                    //ResultInfo.Info = temp;
                    temp.Comm = obj.EditCommunityComment(CommunityCommID, UID);
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

        // Community Comment update//
        [HttpPost]
        public JsonResult UpdateCommunityComment(long UID, long CommunityCommID, string Comment)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (UID > 0 && CommunityCommID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.UpdateCommunityComment(UID, CommunityCommID, Comment);
                    // ResultInfo.Info = PageObj.UpdateStudygrpCommentstatus(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Exited from group";
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

        //Community Comment delete//
        [HttpPost]
        public JsonResult DeleteCommunityComment(long UID, long CommunityCommID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {

                if (UID > 0 && CommunityCommID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.DeleteCommunityComment(UID, CommunityCommID);
                    // ResultInfo.Info = PageObj.UpdateStudygrpCommentstatus(Info);

                    if (ResultInfo.Info != null)
                    {

                        ResultInfo.Description = "Success| Exited from group";
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

        #region  Messages Ramashree 29/01/2019
        [HttpPost]
        public JsonResult SocialFriendList(long UID)
        {
            ResultInfo<Messages> ResultInfo = new ResultInfo<Messages>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            Messages temp = new Messages();
            temp.FriendList = PageObj.SocialFriendList(UID);
            ResultInfo.Info = temp;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Details ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ViewFriend(long ReceiverID, long SenderID)
        {
            ResultInfo<Messages> ResultInfo = new ResultInfo<Messages>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            Messages obj = new Messages();
            obj.Friend = PageObj.ViewFriend(ReceiverID);
            obj.LiveMessageDetails = PageObj.SendMesageBySender(SenderID, ReceiverID);
            ResultInfo.Info = obj;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutoMessage(long ReceiverID, long SenderID)
        {
            ResultInfo<Messages> ResultInfo = new ResultInfo<Messages>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            Messages obj = new Messages();
            obj.LiveMessageDetails = PageObj.SendMesageBySender(SenderID, ReceiverID);
            ResultInfo.Info = obj;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendMessage(LiveMessageDetails Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                Social_Student PageObj = new Social_Student();
                ResultInfo.Info = PageObj.SendMessage(Info);
                if (ResultInfo.Info.Split('!')[0] == "Success")
                {
                    ResultInfo.Description = "Success| Inserted";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FriendListRefresh(long UID)
        {
            ResultInfo<Messages> ResultInfo = new ResultInfo<Messages>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            Messages temp = new Messages();
            temp.FriendList = PageObj.FriendListRefresh(UID);
            ResultInfo.Info = temp;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Details ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateOnlineStatus(long UID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.UpdateOnlineStatus(UID);
            if (ResultInfo.Info.Split('!')[0] == "Success")
            {
                ResultInfo.Description = "Success| Inserted";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult getEvents(long eid)
        {
            Social_Student bck_end = new Social_Student();
            ViewEvent returnObj = new ViewEvent();
            try
            {
                returnObj = bck_end.getEventDetails(eid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertCommentsByEventID(EventComments comment)
        {
            Social_Student bck_end = new Social_Student();
            bool req = bck_end.InsertEventsComment(comment);
            if (req == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateCommentById(EventComments comment)
        {
            Social_Student bck_end = new Social_Student();
            bool req = bck_end.UpdateCommentByEventId(comment);
            if (req == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult deleteEventComment(long eid)
        {
            bool status = false;
            Social_Student bck_end = new Social_Student();
            if (bck_end.DeleteCommentByMsgId(eid))
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckOnline(long UID, string state)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            User_Backend PageObj = new User_Backend();

            try
            {
                
                    if (UID > 0)
                    {
                        string res = PageObj.UpdateIsOnline(UID, state);
                        if (res == "Success")
                        {
                            ResultInfo.Description = "Success";
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
        public JsonResult CollegeDaysForRightSide()
        {
            ResultInfo<List<CollegeDay>> ResultInfo = new ResultInfo<List<CollegeDay>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Social_Student PageObj = new Social_Student();
            ResultInfo.Info = PageObj.collegeForRightSide();
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        // get details of event comment by comment id sm//
        [HttpPost]
        public JsonResult GetEventCommentDetails(EventComments Info)
        {

            ResultInfo<ViewEvent> ResultInfo = new ResultInfo<ViewEvent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            ViewEvent temp = new ViewEvent();
            Social_Student Obj = new Social_Student();
            try
            {
                if (Info != null)
                {



                    temp.singleComm = Obj.EventCommentDetails(Info);
                    ResultInfo.Info = temp;


                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Event";
                        ResultInfo.Status = true;

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        //register report  from event to report table sm//
        [HttpPost]
        public JsonResult RegisterEventReport(EventComments Info)
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
                    Social_Student Obj = new Social_Student();
                    ResultInfo.Info = Obj.InsertEventReportInTable(Info);
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

        [HttpPost]
        public JsonResult getSocialUserLastMessageCount(long SenderId, long ReceiverId)
        {
            Social_Student ss = new Social_Student();
            long Count = ss.checkforNewSocialMessages(SenderId, ReceiverId);
            return Json(Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getSocialUserLastMessage(long SenderID, long ReceiverID)
        {
            Social_Student ss = new Social_Student();
            SocialOrSiteAdminUserlastMessage details = ss.getSocialIdentityMessage(SenderID, ReceiverID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        #region Syllabus Status Ramashree
        [HttpPost]
        public JsonResult InsertOnGoing(SyllabusStatus Data)
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
                    Social_Student Obj = new Social_Student();
                    Data.SubjectID = Obj.GetSubjectID(Data.TopicContentID);
                    //Data.TopicID = Obj.GetTopicID(Data.TopicContentID);
                    //Data.ExamID = Obj.GetExamID(Data.TopicContentID);
                    ResultInfo.Info = Obj.InsertOnGoing(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Insertion Success";
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
        public JsonResult StatusChange(long UID, long TopicContentID)
        {
            long SubjectID = 0;
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                if (UID > 0 && TopicContentID > 0)
                {
                    Social_Student PageObj = new Social_Student();
                    ResultInfo.Info = PageObj.StatusChange(UID, TopicContentID);

                    if (ResultInfo.Info == "Success!updated")
                    {
                        SubjectID = PageObj.GetSubjectID(TopicContentID);
                        ResultInfo.Info = ResultInfo.Info + '!' + SubjectID;

                    }

                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Exited from group";
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
        public JsonResult ViewAllAnswerTopicWise(long SubjectID, long TopicID,string GUID)
        {
            ResultInfo<Feedback> ResultInfo = new ResultInfo<Feedback>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            Feedback temp = new Feedback();


            if (SubjectID != 0 && TopicID!=0)
            {


                temp.Review = PageObj.ViewAllAnswerTopicWise(SubjectID, TopicID,GUID);
               

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
        public JsonResult ViewAllAnswerYearWise(long SubjectID, string Year,string GUID)
        {
            ResultInfo<Feedback> ResultInfo = new ResultInfo<Feedback>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            Social_Student PageObj = new Social_Student();
            Feedback temp = new Feedback();


            if (SubjectID != 0)
            {


                temp.Review = PageObj.ViewAllAnswerYearWise(SubjectID,Year,GUID);


                ResultInfo.Info = temp;

                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details ";
                    ResultInfo.Status = true;
                }



            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


    }
}