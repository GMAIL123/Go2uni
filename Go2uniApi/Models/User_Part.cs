using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Go2uniApi.Models
{
    public static class WebConfigElement
    {
        public static string BaseImagePath => ConfigurationManager.AppSettings["ImagePath"].ToString();
    }

    public class User_Part
    {
        [Table("StudentBasic")]
        public class Login
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool IsRemember { get; set; }
            public bool IsActive { get; set; }
            public long UID { get; set; }
            public long UserType_FK { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }
            public string Profile_image { get; set; }
            public long UserID_FK { get; set; }
            public long SchoolID_FK { get; set; }
            public bool IsFirstStepComplete { get; set; }
            public string IsOnline { get; set; }
            public long Dept_SubID { get; set; }
            public long ClassId { get; set; }

        }

        #region SOCIAL USER INFO 
        public class SocialUserInfo
        {
            public string SocialUserId { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public string Email { get; set; }
            public string DOB { get; set; }
            public string ProfileImg { get; set; }
            public string Geolocation { get; set; }
            public string RegistrationType { get; set; }
            public string Password { get; set; }


        }
        #endregion

        [Table("GlobalTask")]
        public class GlobalTask
        {
            [JsonProperty("GlobalTask_ID")]
            public long GlobalTaskID { get; set; }
             
            [JsonProperty("GlobalTask_Name")]
            public string GlobalTaskName { get; set; }

            [JsonProperty("GlobalTask_allTeamID")]
            public string GlobalTaskallTeamID { get; set; }

            [JsonProperty("GlobalTask_CreatedDate")]
            public DateTime GlobalTaskCreatedDate { get; set; }

            [JsonProperty("GlobalTask_status")]
            public bool GlobalTaskstatus { get; set; }
        }

        [Table("GlobalTask_Team")]
        public class GlobalTaskTeam
        {
            [JsonProperty("GlobalTask_Team_ID")]
            public long GlobalTaskTeamID { get; set; }

            [JsonProperty("student_ID")]
            public int studentID { get; set; }

            [JsonProperty("GlobalTask_Team_Image")]
            public string GlobalTaskTeamImage { get; set; }

            [JsonProperty("GlobalTask_Team_CreatedDate")]
            public DateTime GlobalTaskTeamCreatedDate { get; set; }

            [JsonProperty("GlobalTask_Team_Vote")]
            public int GlobalTaskTeamVote { get; set; }

            [JsonProperty("GlobalTask_ID")]
            public long? GlobalTaskID { get; set; }

            [JsonProperty("GlobalTask_Team_status")]
            public bool GlobalTaskTeamStatus { get; set; }
        }

        [Table("Guidance")]
        public class Guidance
        {
            [JsonProperty("Guidance_ID")]
            public long GuidanceID { get; set; }

            [JsonProperty("Guidance_Video")]
            public string GuidanceVideo { get; set; }

            [JsonProperty("Guidance_AddedDate")]
            public DateTime GuidanceAddedDate { get; set; }

            [JsonProperty("Guidance_Status")]
            public bool GuidanceStatus { get; set; }
        }

        [Table("Guidance_Comments")]
        public class GuidanceComments
        {
            [JsonProperty("Guidance_Comments_ID")]
            public long GuidanceCommentsID { get; set; }

            [JsonProperty("Guidance_Comments_Text")]
            public string GuidanceCommentsText { get; set; }

            [JsonProperty("Guidance_Comments_InsertedDate")]
            public DateTime GuidanceCommentsInsertedDate { get; set; }

            [JsonProperty("Guidance_ID")]
            public long? GuidanceID { get; set; }
        }

        [Table("Guidance_Comments_Reply")]
        public class GuidanceCommentsReply
        {
            [JsonProperty("Guidance_Comments_Reply_ID")]
            public long GuidanceCommentsReplyID { get; set; }

            [JsonProperty("Guidance_Comments_Reply_Text")]
            public string GuidanceCommentsReplyText { get; set; }

            [JsonProperty("Guidance_Comments_Reply_InsertedDate")]
            public DateTime GuidanceCommentsReplyInsertedDate { get; set; }

            [JsonProperty("student_Fk_ID")]
            public int? StudentID { get; set; }

            [JsonProperty("Guidance_Comments_ID")]
            public long? GuidanceCommentsID { get; set; }
        }

        [Table("NextCollege")]
        public class NextCollege
        {
            [JsonProperty("NxtCollege_ID")]
            public int NxtCollegeID { get; set; }

            [JsonProperty("NxtCollege_Video")]
            public string NxtCollegeVideo { get; set; }

            [JsonProperty("NxtCollege_AddedDate")]
            public DateTime NxtCollegeAddedDate { get; set; }

            [JsonProperty("NxtCollege_Status")]
            public bool? NxtCollegeStatus { get; set; }
        }

        [Table("NextCollege_Comments")]
        public class NextCollegeComments
        {
            [JsonProperty("NxtCollege_Comments_ID")]
            public int NxtCollegeCommentsID { get; set; }

            [JsonProperty("NxtCollege_Comments_Text")]
            public string NxtCollegeCommentsText { get; set; }

            [JsonProperty("NxtCollege_Comments_InsertedDate")]
            public DateTime NxtCollegeCommentsInsertedDate { get; set; }

            [JsonProperty("student_Fk_ID")]
            public int? StudentID { get; set; }

            [JsonProperty("NxtCollege_Fk_ID")]
            public int? NxtCollegeID { get; set; }
        }

        [Table("NextCollege_Comments_Reply")]
        public class NextCollegeCommentsReply
        {
            [JsonProperty("NxtCollege_Comments_Reply_ID")]
            public int NxtCollegeCommentsReplyID { get; set; }

            [JsonProperty("NxtCollege_Comments_Reply_Text")]
            public string NxtCollegeCommentsReplyText { get; set; }

            [JsonProperty("NxtCollege_Comments_Reply_InsertedDate")]
            public DateTime NxtCollegeCommentsReplyInsertedDate { get; set; }

            [JsonProperty("student_Fk_ID")]
            public int? StudentID { get; set; }

            [JsonProperty("Comments_Fk_ID")]
            public int? CommentsID { get; set; }
        }

        [Table("Student_Additional")]
        public class StudentAdditional
        {
            [JsonProperty("studnet_ad_ID")]
            public int SudnetAdID { get; set; }

            [JsonProperty("student_ad_username")]
            public string StudentAdUsername { get; set; }

            [JsonProperty("Student_Name")]
            public string StudentName { get; set; }

            [JsonProperty("student_ad_joiningDate")]
            public DateTime? StudentAdJoiningDate { get; set; }

            [JsonProperty("student_mobile")]
            public string StudentMobile { get; set; }

            [JsonProperty("student_ProfileImage")]
            public string StudentProfileImage { get; set; }

            [JsonProperty("student_Fk_ID")]
            public int? StudentID { get; set; }
        }



        [Table("Study_Group_Comment")]
        public class StudyGroupComment
        {
            [JsonProperty("Group_Comment_ID")]
            public long GroupCommentID { get; set; }

            [JsonProperty("Group_Comment_Text")]
            public string GroupCommentText { get; set; }

            [JsonProperty("Group_Comment_CommentedDate")]
            public DateTime? GroupCommentCommentedDate { get; set; }

            [JsonProperty("Group_Fk_ID")]
            public int? GrouppID { get; set; }

            [JsonProperty("student_Fk_ID")]
            public int? StudentID { get; set; }
        }

        [Table("Study_Group_Member")]
        public class StudyGroupMember
        {
            [JsonProperty("Group_Member_ID")]
            public int GroupMemberID { get; set; }

            [JsonProperty("Group_ActiveMembers")]
            public string GroupActiveMembers { get; set; }

            [JsonProperty("Group_PendingMembers")]
            public string GroupPendingMembers { get; set; }

            [JsonProperty("Group_Fk_ID")]
            public int? GroupID { get; set; }
        }

        [Table("Study_Group_Test")]
        public class StudyGroupTest
        {
            [JsonProperty("Group_Test_ID")]
            public int GroupTestID { get; set; }

            [JsonProperty("Group_Test_Date")]
            public DateTime? GroupTestDate { get; set; }

            [JsonProperty("Group_Comment_ID")]
            public string GroupCommentID { get; set; }

            [JsonProperty("Group_Fk_ID")]
            public int? GroupID { get; set; }
        }
    }

    //Abhirup
    public class OnlineCommunity
    {
        public class Online_Test
        {
            [JsonProperty("Test_ID")]
            public int ID { get; set; }
            [JsonProperty("Test_Time")]
            public Nullable<System.TimeSpan> TestTime { get; set; }
            [JsonProperty("Online_Subjects")]
            public string Subjects { get; set; }
            [JsonProperty("Test_Name")]
            public string TestName { get; set; }
            [JsonProperty("Online_Subject_ID")]
            public Nullable<int> Subject_ID { get; set; }
        }

        public class Online_Test_Modules
        {
            [JsonProperty("QuestionModules_ID")]
            public int ID { get; set; }
            [JsonProperty("QuestionModules_Name")]
            public string Modules_Name { get; set; }
            [JsonProperty(" QuestionModules_Questions")]
            public string Modules_Questions { get; set; }
            [JsonProperty("Online_Subject_ID")]
            public Nullable<int> Subject_ID { get; set; }
        }

        public class Online_Test_Question
        {
            [JsonProperty("Question_ID")]
            public long ID { get; set; }
            [JsonProperty("Question_Question")]
            public string Question { get; set; }
            [JsonProperty("Question_Answers")]
            public string Answers { get; set; }
            [JsonProperty("Question_CorrectAnswer")]
            public string CorrectAnswer { get; set; }
            [JsonProperty("Question_CreatedDate")]
            public System.DateTime CreatedDate { get; set; }
            [JsonProperty("Question_ScoreMarks")]
            public decimal ScoreMarks { get; set; }
            [JsonProperty("Question_Status")]
            public Nullable<bool> Status { get; set; }
            [JsonProperty("QuestionModules_ID")]
            public Nullable<int> Modules_ID { get; set; }
        }

        public class Online_Test_Subject
        {
            [JsonProperty("Online_Subject_ID")]
            public int Subject_ID { get; set; }
            [JsonProperty("Online_Subject_Name")]
            public string Subject_Name { get; set; }
            [JsonProperty("Online_Subject_ModulesSet")]
            public string Subject_ModulesSet { get; set; }
        }
    }

    #region Registration
    public class Registration
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
    #endregion

    #region Left Side Panel
    public class StudentNote
    {
        public long ID { get; set; }
        public long UID { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public long StudentID { get; set; }
    }
    #endregion

    #region Student  StudyGroup AND SYllabus

    public class StudyGroup
    {
        public List<StudyGroupName> GroupList { get; set; }
        public GroupInfo GroupTopic { get; set; }
    }

    public class StudyGroupName
    {
        public long GroupID { get; set; }
        public string GroupName { get; set; }
        public string AboutGroup { get; set; }
        public DateTime CreatedDate { get; set; }
        public string tempCreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long StudentID { get; set; }
        public bool IsAdmin { get; set; }
        public long UID { get; set; }
    }

    public class RequestGroupMember
    {
        public long GroupID { get; set; }
        public long UID { get; set; }
        public long Logintype { get; set; }
    }

    public class TopicWiseComment
    {
        public long ReportID { get; set; }
        public long ReportBy { get; set; }
        public string Desc { get; set; }
        public string ReportReason { get; set; }
        public string EmailId { get; set; }
        public string SingleImg { get; set; }
        public string GroupName { get; set; }
        public string Stu_Profile_Image { get; set; }
        public long UserID_FK { get; set; }
        public long Group_Comment_ID { get; set; }
        public string Group_Comment_Text { get; set; }
        public List<Comment> Comments { get; set; }
        public Topics Topics { get; set; }
        public Comment comm { get; set; }
        public string ReportMail { get; set; }

    }

    public class Comment
    {

        public long UID { get; set; }
        public string Stu_Name { get; set; }
        public string Stu_Profile_Image { get; set; }
        public string StudentComment { get; set; }
        public DateTime CommentDate { get; set; }
        public long TopicID { get; set; }
        public long Group_Comment_ID { get; set; }
        public long GroupID { get; set; }
        public string EmailId { get; set; }
        public bool IsReported { get; set; }
    }

    public class GroupInfo
    {
        public string GroupName { get; set; }
        public long GroupID { get; set; }
        public long UID { get; set; }
        public string ProfileImage { get; set; }
        public Topics TopicSingle { get; set; }
        public List<Topics> Topics { get; set; }
        public List<StudyPlanGoalsByGroupID> StudyPlanGoalsByGroupID { get; set; }
    }


    public class Topics
    {
        public long UID { get; set; }
        public long ID { get; set; }
        public string Topic { get; set; }
        public string Stu_Name { get; set; }
        public long GroupID { get; set; }
        // public long StudentID { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        
        public string GroupName { get; set; }

    }

    public class StudentInfo
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string GroupName { get; set; }
        public bool IsAdmin { get; set; }
        public long UID { get; set; }
    }

    public class AutoComplete
    {
        public long value { get; set; }
        public string label { get; set; }
    }

    public class SetGroupStudyPlan
    {
        public long ID { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TestDate { get; set; }
        public int GoalID { get; set; }
        public long StudentID { get; set; }
        public long GroupID { get; set; }
        public long UID { get; set; }
    }

    public class StudyPlanGoals
    {
        public long ID { get; set; }
        public string Goal { get; set; }
    }

    public class StudyPlanGoalsByGroupID
    {
        public long ID { get; set; }
        public string Goal { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }
        public long UID_Stu { get; set; }
        public int GoalID { get; set; }
        public long GroupID { get; set; }
    }

    public class Subject_Syllabus
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ClassID_FK { get; set; }
    }
    public class Chapter_Syllabus
    {
        public int ID { get; set; }
        public string Chapter { get; set; }
        public string Topics { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SyllabusID_FK { get; set; }
    }

    public class Student_Syllabus
    {
        public List<Syllabus_Subject> Syllabus_Subjects { get; set; }
        public List<MasterClass> Master_Class { get; set; }
        public string Subject { get; set; }
        public long Stu_Id { get; set; }
        public int Sub_Id { get; set; }
        public string Class { get; set; }
        public int Class_ID { get; set; }
        public int Div_ID { get; set; }
        public List<SocialStudentSyllabus> SocialStudentSyllabus { get; set; }
        public SocialStudentSyllabus Social_Student_Details { get; set; }
        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public List<ExamSubject> ExamSubject { get; set; }
        public List<ExamTopic> ExamTopic { get; set; }
        public string Exam { get; set; }
        public List<ExamContent> ExamContent { get; set; }
        public List<SyllabusStatus> SyllabusStatus { get; set; }
    }


    public class ExamContent
    {
        public long UserID_FK { get; set; }
        public long ID { get; set; }
        public long TopicID { get; set; }
        public string Topic { get; set; }
        public string TopicContent { get; set; }
        public long ExamID { get; set; }
        public long SubjectID { get; set; }
        public string Subject { get; set; }
        public bool Status_Completed { get; set; }
        public bool Status_OnGoing { get; set; }
        public bool Status_NotStarted { get; set; }
    }

    public class SyllabusStatus
    {
        public long UserID_FK { get; set; }
        public long ID { get; set; }
        public long TopicContentID { get; set; }
        public string Subject { get; set; }
        public long SubjectID { get; set; }
        public bool Status_Completed { get; set; }
        public bool Status_OnGoing { get; set; }
        public bool Status_NotStarted { get; set; }
    }

    #endregion

    #region Student EDIT profile,Class And Section model INFO

    public class ClassInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int divID { get; set; }
    }

    public class SectionInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ClassID { get; set; }
    }

    public class Division
    {
        public int ID { get; set; }
        public string Division_Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Level
    {
        public int ID { get; set; }
        public string Level_Name { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Division_ID { get; set; }
        public int Div_ID { get; set; }
        public bool IsActive { get; set; }
    }

    public class EditProfile
    {
        public long UID { get; set; }
        // public long ID { get; set; }
        // public long Stu_ID { get; set; }
        public long UserID_FK { get; set; }
        public string Stu_Name { get; set; }
        public string Stu_Profile_Image { get; set; }
        public string TempStudentDOB { get; set; }
        public DateTime Stu_DOB { get; set; }
        public string Stu_Favourite_Books { get; set; }
        public string Stu_FutureAmbition { get; set; }
        public int Stu_Gender_FK { get; set; }
        public string School_Name { get; set; }
        // public int Class_ID_FK { get; set; }
        public int Stream_ID_FK { get; set; }
        public int Level_ID { get; set; }
        public int DivisionID { get; set; }
        public long Class_ID { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassDetails { get; set; }
        //  public string Stu_Email { get; set; }
        //  public List<ClassInfo> ClassInfo { get; set; }
        // public List<SectionInfo> SectionInfo { get; set; }
        //  public string Stu_Password { get; set; }
        // public List<Gender> GenderInfo { get; set; }
        public string Stu_RegistrationNo { get; set; }
        // public List<ClassSpecificaton> Division { get; set; }
        //  public int DivisionID { get; set; }
        //  public List<Semester> SemList { get; set; }
        public int SemID { get; set; }
        // public List<ClassStream> StreamList { get; set; }
        public UserDetails UserDetails { get; set; }
        // public long ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

    }



    public class Semester
    {
        public int ID { get; set; }
        public string SemesterName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ClassSpecificaton
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    #endregion

    #region Gender
    public class Gender
    {
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion



    #region Global

    #region NEWS FEED
    //public class NewsFeed
    //{
    //    public long ID { get; set; }
    //    public string News { get; set; }

    //    private string _ImagePath;
    //    public string ImagePath
    //    {
    //        get
    //        {
    //            if (!string.IsNullOrEmpty(_ImagePath))
    //            {
    //                return WebConfigElement.BaseImagePath + "" + _ImagePath;
    //            }
    //            return "";
    //        }
    //        set
    //        {
    //            _ImagePath = value;
    //        }
    //    }

    //    public bool Status { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //}

    public class InitialInfo
    {
        public string Name { get; set; }

        private string _ProfilePicture;
        public string ProfilePicture
        {
            get
            {
                if (!string.IsNullOrEmpty(_ProfilePicture))
                {
                    return WebConfigElement.BaseImagePath + "" + _ProfilePicture;
                }
                return "";
            }
            set
            {
                _ProfilePicture = value;
            }
        }
    }


    #endregion

    #endregion

    #region Mail Template
    public class EmailTemplate
    {
        public string TemplateName { get; set; }
        public string Header { get; set; }
        public string TemplateBody { get; set; }
    }
    #endregion

    #region Teacher

    #region Teacher Notes
    public class TeacherNote
    {
        public long ID { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public long TeacherID_FK { get; set; }
    }
    #endregion

    #region TEACHER PLANNER
    public class TopicCoverDate
    {
        public long ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public bool Status { get; set; }
        public string subject { get; set; }
        public string Chapter { get; set; }
        public long TopicCoverID { get; set; }
    }

    public class AssignmentDueDate
    {
        public long ID { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }
        public int Standard { get; set; }
        public DateTime SetDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreateDate { get; set; }
        public long TeacherID_FK { get; set; }
    }

    public class TestDueDate
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public int Standard { get; set; }
        public DateTime DueDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public long TeacherID_FK { get; set; }
    }

    public class TeacherPlan
    {
        public List<TopicCoverDate> TopicCoverDate { get; set; }
        public List<AssignmentDueDate> AssignmentDueDate { get; set; }
        public List<TestDueDate> TestDueDate { get; set; }
    }
    #endregion

    #region Teacher Time Table
    public class TimeTable
    {
        public long ID { get; set; }
        public long TeacherID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int WeekID { get; set; }
        public long TimeID { get; set; }
        public long SubID { get; set; }
        public string Day { get; set; }
        public string Period { get; set; }
        public string Sub { get; set; }
    }
    #endregion

    #region Period Details
    public class PeriodDetails
    {
        public long ID { get; set; }
        public string Period { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion

    #region WeekDays
    public class WeekDays
    {
        public int ID { get; set; }
        public string Days { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion

    #region Time table details  
    public class TimeTableDetails
    {
        public List<WeekDays> WeekList { get; set; }
        public List<PeriodDetails> PeriodList { get; set; }
        public List<TimeTable> TimeTableList { get; set; }
    }
    #endregion

    #endregion

    #region Goal
    public class Syllabus_TopicDetails
    {
        public int Topic_ID { get; set; }
        public string Topic_Name { get; set; }
        public int Subject_ID_FK { get; set; }
        public int Semester_ID_FK { get; set; }
    }

    public class Syllabus_Subtopic
    {
        public long ID { get; set; }
        public string Subtopic { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TopicID { get; set; }
    }
    public class Syllabus_Content
    {
        public int Content_ID { get; set; }
        public string Content { get; set; }
        public int Topic_ID_FK { get; set; }
    }
    public class Subtopics
    {
        public List<Syllabus_Subtopic> Syllabus_Subtopic { get; set; }
        public int Count { get; set; }
        public List<Syllabus_Content> Syllabus_Content { get; set; }
    }

    public class Goal
    {
        public Subtopics Subtopics { get; set; }
        public List<Syllabus_TopicDetails> Topic { get; set; }
        public List<ClassInfo> ClassInfo { get; set; }
        public List<Subject_Syllabus> Subject { get; set; }
        public List<ClassSpecificaton> ClassSpecificaton { get; set; }
        public List<Semester> Semester { get; set; }
        public List<ClassStream> StreamList { get; set; }

        public List<GoalDetails> GoalDetails { get; set; }
        public List<Syllabus_Subtopic> SubTopic { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassDetails { get; set; }
        public int Count { get; set; }
        public List<Syllabus_Content> Syllabus_Content { get; set; }

        public List<ExamTypeDetails> ExamTypeDetails { get; set; }

        public List<ExamSubject> ExamSubject { get; set; }

        public List<ExamTopic> ExamTopic { get; set; }

    }

    public class GoalDetails
    {
        public long ID { get; set; }
        public long SubTopicID_FK { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public long StudentID_FK { get; set; }
        public string Subtopic { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }

        public string Content { get; set; }
        public int Content_ID_FK { get; set; }
        public long UserID_FK { get; set; }
        public long ExamTopicID_FK { get; set; }
        public long SubjectID { get; set; }

        public string TempStartDate { get; set; }
        public string TempEndDate { get; set; }
        public string ExamType { get; set; }
        public long ExamTypeID { get; set; }

    }

    #endregion

    #region Event
    public class CollegeDay
    {
        public long ID { get; set; }
        public string video { get; set; }
        public DateTime EventDate { get; set; }
        public string CollegeName { get; set; }
        public bool IsUpcoming { get; set; }
        public string Event_Date { get; set; }
    }

    public class NextCollegeDayComment
    {
        public long CommentID { get; set; }
        public string Comment { get; set; }
        public long StudentID { get; set; }
        public string StudentName { get; set; }
        public long TeacherID { get; set; }
        public string TeacherName { get; set; }
        public bool IsStudent { get; set; }
        public DateTime CreatedDate { get; set; }
        public long EventID { get; set; }
    }

    public class NextCollegeDay
    {
        public List<CollegeDay> CollegeList { get; set; }
        public List<Event> EventList { get; set; }
        public CollegeDay CollegeDetails { get; set; }
        public List<NextCollegeDayComment> CollegeCommentList { get; set; }
        public Event EventDetails { get; set; }
        public List<EventComment> EventCommentList { get; set; }
    }

    public class Event
    {
        public long ID { get; set; }
        public string EventVideo { get; set; }
        public string EventData { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string Event_Date { get; set; }
    }
    public class EventComment
    {
        public long CommentID { get; set; }
        public string Text { get; set; }
        public long StudentID { get; set; }
        public string StudentName { get; set; }
        public long TeacherID { get; set; }
        public string TeacherName { get; set; }
        public bool IsStudent { get; set; }
        public DateTime CreatedDate { get; set; }
        public long EventID { get; set; }
    }
    #endregion



    #region ReportCard
    public class ReportCard
    {
        public List<ReportCardSub> ReportCardSub { get; set; }
        public List<Year> Year { get; set; }
        public List<ReportCardChapter> ReportCardChapter { get; set; }

    }
    public class ReportCardSub
    {
        public int ClassID { get; set; }
        public long StudentID { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public int SubjectID { get; set; }
    }
    public class Year
    {
        public int year { get; set; }
    }
    public class ReportCardChapter
    {
        public int ChapterID { get; set; }
        public string Chapter { get; set; }
        public string Topics { get; set; }



        public string Subject { get; set; }
        public int SubjectID { get; set; }

    }
    public class FeedbackByTopic
    {
        public long StudentID { get; set; }
        public int SubjectID { get; set; }
        public string Subject { get; set; }
        public int Year { get; set; }
        public int ChapterID { get; set; }
        public string Chapter { get; set; }
        public string Time { get; set; }
        public string Desire_Time { get; set; }
        public long Score { get; set; }
        public string Feedback { get; set; }
        public int FeedbackID { get; set; }
    }

    #endregion




    #region SUPER ADMIN
    public class SupGetAllStudent
    {
        public List<NameAndRegs> NameAndRegs { get; set; }
        public List<SupGetAllStudentByDiv> SupGetAllStudentByDivs { get; set; }
        public List<ClassSpecificaton> ClassSpecificatons { get; set; }
    }

    public class NameAndRegs
    {
        public long Stu_ID { get; set; }
        public string Stu_Name { get; set; }
        public string Stu_RegistrationNo { get; set; }
        public string Class { get; set; }
        public string DivName { get; set; }
        public int DivID { get; set; }
    }


    public class SupGetAllStudentByDiv
    {
        public int DivID { get; set; }
        public string Student_Name { get; set; }
        public string RegistrationNo { get; set; }
        public string Class { get; set; }
        public string DivName { get; set; }
    }



    #region STUDENT PROFILE
    public class StudentProfile
    {
        //public List<StudentBasic> studentBasics { get; set; }
        //public List<Student_Additional> student_Additionals { get; set; } 
        //public List<GenderInfo> GenderInfos { get; set; }
        //public List<StudentSchoolInfo> StudentSchoolInfos { get; set; }
        //public List<MasterClass> MasterClasses { get; set; }
        //public List<ClassStream> ClassStreams { get; set; }
        //// ClassStream
        public long ID { get; set; }
        public string Student_Email { get; set; }
        public string Student_Name { get; set; }
        public string student_ProfileImage { get; set; }
        public string RegistrationNo { get; set; }
        public string GenderName { get; set; }
        public string SchoolName { get; set; }
        public string Class { get; set; }
        public string Stream { get; set; }
        public List<ClassStream> ClassStreams { get; set; }

    }

    public class StudentBasic
    {
        public long ID { get; set; }
        public string Student_Email { get; set; }
    }

    public class Student_Additional
    {
        public string Student_Name { get; set; }
        public string student_ProfileImage { get; set; }
        public string RegistrationNo { get; set; }
        public string GenderName { get; set; }
    }

    public class GenderInfo
    {
        // public int GenderID { get; set; }
        public string GenderName { get; set; }

    }

    public class StudentSchoolInfo
    {
        public string SchoolName { get; set; }
    }

    public class MasterClass
    {
        public string Class { get; set; }

    }


    #endregion



    #region EDIT/UPDATE SUPER ADMIN PROFILE


    public class SupEditprofile
    {


        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string ProfileImage { get; set; }
    }
    public class SuperAdminBasic
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

    }

    public class SuperAdminAdditional
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int AdminID_FK { get; set; }
    }


    #endregion


    #region AddstudentBySuperadmin




    public class ClassStream
    {
        public int ID { get; set; }
        public string Stream { get; set; }
        public bool IsActive { get; set; }
        public int Level_ID { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    #endregion

    #region TeacherBySuperadmin
    public class Login_Type
    {
        public long ID { get; set; }
        public string LoginType { get; set; }

    }
    public class AddTeacherDetails
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int Gender_FK { get; set; }
        public DateTime DOB { get; set; }
        public string TempStudentDOB { get; set; }
        public DateTime Joining_Date { get; set; }
        public string TempJoiningDate { get; set; }
        public string MObile_No { get; set; }
        public long TeacherType_FK { get; set; }
        public string Profile_Image { get; set; }


        public long CreatedBy { get; set; }
        public string Registration_No { get; set; }

        public long SchoolID_FK { get; set; }
        public string Subject { get; set; }
        public long UID_FK { get; set; }
        public string LoginType { get; set; }
    }
    public class AddTeacher
    {
        public List<Login_Type> Login_Type { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public List<AddTeacherDetails> AddTeacherDetails { get; set; }
        public AddTeacherDetails TeacherById { get; set; }
    }

    public class EditTeacher
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TempEmail { get; set; }
        public int Gender_FK { get; set; }
        public DateTime DOB { get; set; }
        public string TempStudentDOB { get; set; }
        public DateTime Joining_Date { get; set; }
        public string TempJoiningDate { get; set; }
        public string MObile_No { get; set; }
        public long TeacherType_FK { get; set; }
        public string Profile_Image { get; set; }
        public string TempImage { get; set; }

        public long CreatedBy { get; set; }
        public string Registration_No { get; set; }

        public long SchoolID_FK { get; set; }
        public string Subject { get; set; }
        public long UID_FK { get; set; }

        public List<Login_Type> Login_Type { get; set; }
        public List<Gender> GenderInfo { get; set; }

    }
    #endregion

    #region ADD CLASS BY SUPERADMIN

    public class AddClassInfo
    {
        public long ID { get; set; }
        public string Class_Name { get; set; }
        public int Div_ID { get; set; }
        public int Level_ID { get; set; }
        public long CreatedBy_ID { get; set; }
        public long School_ID { get; set; }

    }

    public class ClassDetails
    {
        public long ID { get; set; }
        public string Class_Name { get; set; }
        public string Division_Name { get; set; }
        public string Level_Name { get; set; }
        public int Div_ID { get; set; }
        public int Level_ID { get; set; }
        public long CreatedBy_ID { get; set; }
        public long School_ID { get; set; }
        public string Stu_Name { get; set; }
        public long No_Of_Students { get; set; }
    }

    public class AddClass
    {
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }

    }

    public class Class
    {

        public AddStudentDetails AddStudentDetails { get; set; }
        public List<ClassDetails> ClassInfo { get; set; }

        public ClassDetails ClassDetails { get; set; }
        public ViewClassInfo ViewClassInfo { get; set; }
    }
    public class ViewClassInfo
    {
        public long ID { get; set; }
        public string Class_Name { get; set; }
        public string Division_Name { get; set; }
        public string Level_Name { get; set; }
        public int Div_ID { get; set; }
        public int Level_ID { get; set; }
        public long CreatedBy_ID { get; set; }
        public long School_ID { get; set; }
        public string Stu_Name { get; set; }
        public long No_Of_Students { get; set; }
    }
    public class EditClass
    {
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public ClassDetails cldetls { get; set; }
    }
    #endregion
    #endregion

    #region Edit Profile teacher


    public class EditProfileTeacher
    {
        public long ID { get; set; }
        public string Teacher_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TempJoiningDate { get; set; }
        public DateTime JoiningDate { get; set; }

        public string MobileNo { get; set; }
        public string ProfileImage { get; set; }
        // public long TeacherID_FK { get; set; }

        // public string Email { get; set; }
        // public long ID { get; set; }
    }
    #endregion


    #region MOCKTEST 

    //public class MockQuestions
    //{
    //    public List<Syllabus_Subject> Syllabus_Subjects { get; set; }
    //    public List<DifficultyLevel> DifficultyLevels { get; set; }
    //    public List<GetYear> GetYears { get; set; }
    //    public List<YearWiseQues> YearWiseQuess { get; set; }
    //    public List<GetTopic> GetTopics { get; set; }
    //    public List<GetQuestions> GetQuestions { get; set; }
    //    public List<TopicWiseQues> TopicWiseQues { get; set; }
    //    public List<CommonClass> CommonClasses { get; set; }
    //    public List<SelectDiffiCulty> SelectDiffiCulties { get; set; }
    //    public List<StartExam> StartExams { get; set; }
    //}


    public class StartExam
    {
        public long QuesID { get; set; }
        public string Question { get; set; }
        public string Diagram { get; set; }
        public int Sub_Id_Fk { get; set; }
        public int Year { get; set; }
        public int Topic_ID { get; set; }
        public int Diff_Level { get; set; }
        public int Ide { get; set; }
        public int Value { get; set; }
    }


    public class GetQuestions
    {
        public long ID { get; set; }
        public string Question { get; set; }
        public string Diagram { get; set; }
        public string Topic { get; set; }
        public int Topic_ID { get; set; }
        public string Year { get; set; }


    }



    public class TopicWiseQues
    {
        public long QuesID { get; set; }
        public string Question { get; set; }

        public string Diagram { get; set; }
        public int Sub_Id_Fk { get; set; }

        public int Topic_ID { get; set; }
        public int Diff_Level { get; set; }


    }
    public class YearWiseQues
    {
        //public long Year_ID { get; set; }

        public long QuesID { get; set; }
        public string Question { get; set; }
        public string Diagram { get; set; }
        public int Sub_Id_Fk { get; set; }
        public int Year { get; set; }
        public int Diff_Level { get; set; }


    }

    public class Syllabus_Subject
    {
        public int ClassID { get; set; }
        public long student_ID { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public int SubjectID { get; set; }

       
        public int Level_ID { get; set; }
        public int School_ID { get; set; }
        public long Dept_SubID { get; set; }
        public long Syllabus_SubjectID { get; set; }

    }

    public class DifficultyLevel
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public string Diff_Level { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GetYear
    {
        // public  int ID { get; set; }
        public int Year { get; set; }
        public int SubjectID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }


    }
    //public class QuestionsByYear
    //{
    //    public long ID { get; set; }
    //    public string Questions { get; set; }
    //    public string Diagram { get; set; }
    //    public string Year { get; set; }
    //    public string Diffi_Level { get; set; }
    //    public bool Status { get; set; }
    //    public DateTime CreatedDate { get; set; }

    //}

    public class TestQuestion
    {
        public long ID { get; set; }
        public string Questions { get; set; }
        public string Diagram { get; set; }
        public int Year { get; set; }
        public bool Status { get; set; }
        public int Diff_Level { get; set; }
        public int Topic_ID { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GetTopic
    {
        public int ID { get; set; }
        public string Chapter { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SyllabusID_FK { get; set; }
        public int Topic_Id { get; set; }
        public int Subject_ID { get; set; }
        public string Subject { get; set; }

    }
    public class CommonClass
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }

    public class SelectDiffiCulty
    {
        public int Topic_ID { get; set; }
        public string Difficulty { get; set; }
        public int Year { get; set; }
    }

    public class MockQuestions
    {
        public List<Syllabus_Subject> Syllabus_Subjects { get; set; }
        public List<DifficultyLevel> DifficultyLevels { get; set; }
        public List<GetYear> GetYears { get; set; }
        public List<YearWiseQues> YearWiseQuess { get; set; }
        public List<GetTopic> GetTopics { get; set; }
        public List<GetQuestions> GetQuestions { get; set; }
        public List<TopicWiseQues> TopicWiseQues { get; set; }
        public List<CommonClass> CommonClasses { get; set; }
        public List<SelectDiffiCulty> SelectDiffiCulties { get; set; }
        public List<StartExam> StartExams { get; set; }



        public List<SocialStudentSyllabus> SocialStudentSyllabus { get; set; }
        public SocialStudentSyllabus Social_Student_Details { get; set; }
        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public List<ExamSubject> ExamSubject { get; set; }
        public List<ExamTopic> ExamTopic { get; set; }
        public string Exam { get; set; }
        public List<QuestionsByTopic> QuestionsByTopic { get; set; }
        public string Topic { get; set; }
        public List<QuestionsByYear> QuestionsByYear { get; set; }
        public string GUID { get; set; }
        public string CorrectAnswer { get; set; }

    }

    #region MyRegion Mock Test

    #region Ramashree 31/12/2018 Mock Test

    public class QuestionsByTopic
    {
        public long ID { get; set; }
        public string Question { get; set; }
        public string QuestionsOption { get; set; }
        public string Topic { get; set; }
        public long ExamID { get; set; }
        public long SubjectID { get; set; }
        public long TopicID { get; set; }
        public string DescriptionText { get; set; }
        public long QuestionID { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Diagram { get; set; }
        public long SerialNo { get; set; }

        public long QuestionTypeID { get; set; }
    }

    #endregion

    #region Ramashree 02/01/2019 Mock Test

    public class QuestionsByYear
    {
        public long ID { get; set; }
        public string Question { get; set; }
        public string QuestionsOption { get; set; }
        public string Year { get; set; }
        public long ExamID { get; set; }
        public long SubjectID { get; set; }
        public long TopicID { get; set; }
        public string DescriptionText { get; set; }
        public long QuestionID { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Diagram { get; set; }
        public long SerialNo { get; set; }

        public long QuestionTypeID { get; set; }
    }
    #endregion
    public class StudentAnswerByTopic
    {
        public long ID { get; set; }
        public string Answer { get; set; }
        public long UID { get; set; }
        public long QuestionID { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string GUID { get; set; }
        public string CorrectAnswer { get; set; }
    }
    public class StudentAnswerByYear
    {
        public long ID { get; set; }
        public string Answer { get; set; }
        public long UID { get; set; }
        public long QuestionID { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string GUID { get; set; }
        public string CorrectAnswer { get; set; }
    }
    #endregion

    #endregion

    #region GLOBAL UPDATE
    public class UserDetails
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion


    #region USER DETAILS

    #endregion

    public class Class_Details
    {
        public int ID { get; set; }
        public string Class_Name { get; set; }
        public int Div_ID { get; set; }
        public int Level_ID { get; set; }
        public long School_ID { get; set; }
    }
    public class School
    {
        public long ID { get; set; }
        public string School_Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class AddStudentDetails
    {
        public string Email { get; set; }
        public string TempEmail { get; set; }
        public string Password { get; set; }
        public string Stu_Name { get; set; }
        public DateTime Stu_DOB { get; set; }
        public string TempStudentDOB { get; set; }
        public DateTime Stu_JoiningDt { get; set; }
        public string TempJoiningDate { get; set; }
        public string Stu_MObile_No { get; set; }
        public string Stu_Profile_Image { get; set; }
        public string Stu_RegistrationNo { get; set; }
        public string tempStu_RegistrationNo { get; set; }
        public string Stu_FutureAmbition { get; set; }
        public string Stu_Favourite_Books { get; set; }
        public DateTime LastLoginTime { get; set; }
        public long LoginTpe_FK { get; set; }
        public int Level_ID { get; set; }
        public int DivisionID { get; set; }
        public bool STATUS { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Stu_Gender_FK { get; set; }
        public string School_Name { get; set; }
        public int Stream_ID_FK { get; set; }
        public long UserID_FK { get; set; }
        public long UID { get; set; }
        public long CreatedBy_FK { get; set; }
        public long School_ID { get; set; }
        public long Class_ID { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public List<Division> Division { get; set; }
        public List<Level> Level { get; set; }
        public List<Class_Details> Class { get; set; }
        public List<School> School { get; set; }


        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassDetails { get; set; }
        public List<ClassStream> ClassStream { get; set; }
        public List<StudentListing> StudentListing { get; set; }
        public string Class_Name { get; set; }
        public string Division_Name { get; set; }
        public string Level_Name { get; set; }

        public StudentListing Student_Details { get; set; }

        public List<Semester> Semester { get; set; }
        public List<ClassTeacherTimetable> Classtimetable { get; set; }
        public List<ClassTeacherListingDetails> ClassTeacherLsiting { get; set; }
        public List<AddsubjectInfo> SubjectInfo { get; set; }
        public List<SchoolTopic> SchoolTopic { get; set; }


       
     
        
    }

    public class AddStudent
    {
        public List<ClassInfo> ClassInfo { get; set; }
        public List<ClassStream> ClassStream { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public List<AddStudentDetails> AddStudentDetails { get; set; }
        public List<Division> Division { get; set; }
        public List<Level> Level { get; set; }
        public List<UserDetails> UserDetails { get; set; }
        public List<ClassSpecificaton> ClassSpecificaton { get; set; }

    }

    public class StudentListing
    {
        public long UID { get; set; }
        public long UserID_FK { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public int DivisionID { get; set; }
        public string Stu_RegistrationNo { get; set; }
        public string Stu_Name { get; set; }
        public string Class_Name { get; set; }
        public string Division_Name { get; set; }
        public string Level_Name { get; set; }
        public AddStudentDetails Student_Details { get; set; }
    }

    #region Ramashree 05/12/2018

    public class SuperAdminEditprofile
    {
        public int ID { get; set; }
        public long UserID_FK { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Gender_FK { get; set; }
        public DateTime DOB { get; set; }
        public string TempDOB { get; set; }
        public string Mobile_No { get; set; }
        public string Profile_Image { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public string TempEmail { get; set; }
    }

    #endregion

    #region Super, SUBJECT TAB
    public class SubjectTab
    {
        public List<SubjectDetails> SubjectList { get; set; }
        public SubjectDetails SubjectbyId { get; set; }
    }

    public class SubjectDetails
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public long SubID { get; set; }

        public int Division_ID { get; set; }
        public string Division_Name { get; set; }
        public int Level_ID { get; set; }
        public string Level_Name { get; set; }
        public long Class_ID { get; set; }
        public string Class_Name { get; set; }
        public long CreatedBy_ID { get; set; }
        public long School_ID { get; set; }

        public long Department_ID { get; set; }
        public string Department_Name { get; set; }
    }


    public class AddsubjectInfo
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public long CreatedBy_ID { get; set; }
        public long School_ID { get; set; }

        
        public long DepartmentID { get; set; }

    }

    public class AddSub
    {
       
        public List<Level> Levels { get; set; }
        public List<Class_Details> Class_Details { get; set; }

   
        public List<Depertment_Subject> Depertment_Subject { get; set; }

    }
    public class EditSubject
    {
        public SubjectDetails SubjectDetails { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> Class_Details { get; set; }
        public ClassDetails ClassInfo { get; set; }

       
        public List<Depertment_Subject> Depertment_Subject { get; set; }
    }

    #endregion

    #region Super, Parent
    public class ParentDetails
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile_No { get; set; }
        public string Profile_Image { get; set; }
        public string Stu_Registration_NO { get; set; }
        public bool STATUS { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public string Address { get; set; }
        public long SchoolID_FK { get; set; }
        public long UID_FK { get; set; }
        public long Stu_ID { get; set; }
        public string Stu_Name { get; set; }
        public long Class_ID { get; set; }
        public string Class_Name { get; set; }
    }
    public class EditParent
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TempEmail { get; set; }
        public string Password { get; set; }
        public string Mobile_No { get; set; }
        public string Profile_Image { get; set; }
        public string TempImage { get; set; }
        public string Stu_Registration_NO { get; set; }
        public bool STATUS { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public string Address { get; set; }
        public long SchoolID_FK { get; set; }
        public long UID_FK { get; set; }
        public long Stu_ID { get; set; }
        public string Stu_Name { get; set; }
    }
    public class Parent
    {
        public List<ParentDetails> ParentDetails { get; set; }
        public ParentDetails ParentbyId { get; set; }
    }
    #endregion

    public class SocialStudentEditprofile
    {
        public int ID { get; set; }
        public long UserID_FK { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
        public string Password { get; set; }
        public string Stu_Name { get; set; }
        public int Stu_Gender_FK { get; set; }
        public DateTime Stu_DOB { get; set; }
        public string Stu_MObile_No { get; set; }
        public string Stu_Profile_Image { get; set; }
        public int DivisionID { get; set; }
        public int Level_ID { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public string tempDob { get; set; }
        public string tempEmail { get; set; }
    }

    public class ExamTypeDetails
    {
        public long ID { get; set; }
        public string ExamType { get; set; }
    }
    public class ExamSubject
    {
        public long ID { get; set; }
        public string Subject { get; set; }
        public long ExamID { get; set; }
        public string name { get; set; }
        public string ExamType { get; set; }
    }


    public class ExamTopic
    {
        public long ID { get; set; }
        public long SubjectID { get; set; }
        public long ExamID { get; set; }
        public string Topic { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Completed_Status { get; set; }

    }
    #region Ramashree 20/12/2018
    public class SocialStudentSyllabus
    {
        public long ID { get; set; }
        public long ExamID { get; set; }
        public string ExamType { get; set; }
        public int SubjectID { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion

    #region Community 
    public class Online_Community
    {
        public long Community_ID { get; set; }
        public string Community_Name { get; set; }
        public string Community_About { get; set; }
        public DateTime Community_CreatedDate { get; set; }
        public bool Community_status { get; set; }
        public long CreatedBy { get; set; }
        public string User_Name { get; set; }
        public bool Join { get; set; }
        public long Login_Type { get; set; }
        public long UID { get; set; }
        public long Community_Member_ID { get; set; }
    }

    public class OnlineCommunityTopic
    {
        public long Topic_ID { get; set; }
        public long UID_CreatedBY { get; set; }
        public long Community_Fk_ID { get; set; }
        public string Topic_Discussion { get; set; }
        public DateTime Topic_CreatedDate { get; set; }
        public string tempTopic_CreatedDate { get; set; }
        public bool Topic_status { get; set; }
        public string CreatedBy { get; set; }
        public string student_ProfileImage { get; set; }
        public string CommunityName { get; set; }
        public string Stu_Name { get; set; }

    }

    public class OnlineCommunityComment
    {
        public long Community_Comment_ID { get; set; }
        public long Community_Comment_Like { get; set; }
        public long Community_Comment_Dislike { get; set; }
        public string Community_Comment_Text { get; set; }
        public DateTime Community_Comment_CommentedDate { get; set; }
        public string tempCommunity_Comment_CommentedDate { get; set; }

        public long UID { get; set; }
        public bool Community_Comment_status { get; set; }
        public long Topic_Fk_ID { get; set; }
        public string Topic_Discussion { get; set; }
        public long Community_Fk_ID { get; set; }
        public DateTime Topic_CreatedDate { get; set; }
        public string tempTopic_CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string student_ProfileImage { get; set; }
        public string Community_Name { get; set; }
        public string Commentby { get; set; }
        public bool IsReported { get; set; }
    }

    public class CommunityDetails
    {
        public long ReportID { get; set; }
        public long ReportBy { get; set; }
        public long Community_Comment_ID { get; set; }
        public string Email { get; set; }
        public long CommenterID { get; set; }
        public long ReporterID { get; set; }
        public string CommentText { get; set; }
        public string ReportReason { get; set; }
        public OnlineCommunityTopic Singletopic { get; set; }
        public List<Online_Community> CommunityList { get; set; }
        public List<OnlineCommunityTopic> TopicList { get; set; }
        public List<OnlineCommunityComment> CommentList { get; set; }
        public string Community { get; set; }
        public string Topic { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Topic_CreatedDate { get; set; }

        public long Topic_ID { get; set; }
        public long CommunityID { get; set; }
        public OnlineCommunityComment Comm { get; set; }
        public string tempTopic_CreatedDate { get; set; }
    }


    public class Online_Community_Members
    {
        public long Community_Member_ID { get; set; }
        public long Community_Members { get; set; }
        public long Community_FK_ID { get; set; }
        public DateTime Community_Member_CreatedDate { get; set; }
        public bool Community_Member_Status { get; set; }
    }

    public class Online_Community_LikeORDisLike
    {
        public long ID { get; set; }
        // public long Student_Fk_ID { get; set; }
        public long UID { get; set; }
        public long Comment_Fk_ID { get; set; }
        public bool Likes { get; set; }
        public bool DisLike { get; set; }
        public DateTime CreatedDate { get; set; }
        public String LikeVal { get; set; }
    }
    #endregion

    public class StudyGuide
    {
        public List<ViewGoal> ViewGoal { get; set; }
    }
    public class ViewGoal
    {
        public long ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ExamTopicID_FK { get; set; }
        public string Topic { get; set; }
        public string Subject { get; set; }
        public long SubjectID { get; set; }
        public string ExamType { get; set; }
        public long ExamID { get; set; }
        public bool Completed_Status { get; set; }
        public string TempStartDate { get; set; }
        public string TempEndDate { get; set; }
        public long UserID_FK { get; set; }
        public string Tempdata { get; set; }

    }


    public class Notes
    {
        public long ID { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UserID_Fk { get; set; }
    }

    public class Feedback
    {
        public int WrongAnswer { get; set; }
        public string GUID { get; set; }
        public long UID { get; set; }
        public int CorrectAnswer { get; set; }
        public string Year { get; set; }
        public string Topic { get; set; }
        public int AttemedQuestion { get; set; }
        public int Score { get; set; }
        public int TotalQuestionForTopic { get; set; }
        public int TotalQuestionForYear { get; set; }
        public string RecordStore { get; set; }
        public string ExamName { get; set; }
        public string SubjectName { get; set; }
        public List<Review_SocialStudent> Review { get; set; }
        public long SubjectID { get; set; }
        public long TopicID { get; set; }

    }

    public class Reginfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Otp { get; set; }
        public long uid { get; set; }

    }
    public class ReportCard_SocialStudent
    {
        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public List<ExamSubject> ExamSubject { get; set; }

        public List<ExamTopic> ExamTopic { get; set; }
        public List<QuestionsByYear> QuestionsByYear { get; set; }
        public List<Record> Record { get; set; }
        public double HighestScore { get; set; }
        public double LowestScore { get; set; }
        public double AvarageScore { get; set; }
        public int NumberOfStudent { get; set; }
        public List<int> Points { get; set; }
        public int NumberOfAttempted { get; set; }
        public string Exam { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string Year { get; set; }
        public List<double> PointsForTimeGraph { get; set; }
    }

    public class Record
    {
        public int Score { get; set; }
        public DateTime ExamDate { get; set; }
        public string TempExamDate { get; set; }
        public double Percentage { get; set; }
        public string Feedback { get; set; }
    }

    #region Site Admin By Indranil 01/02/2019

    public class studentList
    {
        public long ID { get; set; }
        public string student_name { get; set; }
        public string profilepic { get; set; }
        public System.DateTime lastlogin { get; set; }
        public List<Messages_Admin> Msg { get; set; }
        public long UserID_FK { get; set; }
        public string message { get; set; }
    }
    public class Messages_Admin
    {
        public long ID { get; set; }
        public long SenderID { get; set; }
        public long ReceiverID { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }
        public int ReceiverTypeID { get; set; }
    }
    #endregion

    #region REPORT COMMENT BY Site ADMIN by SM

    public class ReportTab
    {
        public long ReportId { get; set; }
        public string CommentText { get; set; }
        public string Go2UniSection { get; set; }
        public long CommentID { get; set; }
        public long CommentById { get; set; }
        public string CommentByMail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ReportStatus { get; set; }
        public string ReportReason { get; set; }
        public bool IsRead { get; set; }
        public List<CommentReport> comrep { get; set; }
        public string CommentByName { get; set; }
        public string ReportedBy { get; set; }

    }

    public class CommentReport
    {
        public long ReportId { get; set; }
        public string CommentText { get; set; }
        public string Go2UniSection { get; set; }
        public long CommentID { get; set; }
        public long CommentById { get; set; }
        public string CommentByMail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ReportStatus { get; set; }
        public string ReportReason { get; set; }
        public long ReportedBy { get; set; }
        public string CommentByName { get; set; }
        public bool IsRead { get; set; }
    }

    #endregion

    #region Messages Ramashree 29/01/2019
    public class Messages
    {
        public long SenderID { get; set; }
        public long ReceiverID { get; set; }
        public List<FriendList> FriendList { get; set; }
        public FriendList Friend { get; set; }
        public List<LiveMessageDetails> LiveMessageDetails { get; set; }
    }

    public class FriendList
    {
        public string Stu_Name { get; set; }
        public long UserID { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string Message { get; set; }
        public bool IsOnline { get; set; }
        public string Stu_Profile_Image { get; set; }
    }

    public class LiveMessageDetails
    {
        public long ID { get; set; }
        public long ConvID { get; set; }
        public long SenderID { get; set; }
        public long ReceiverID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SenderType { get; set; }
        public Boolean fromsender { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string Stu_Profile_Image { get; set; }
    }
    #endregion

    #region Event Details by Indranil 29/01/2018

    public class ViewEvent
    {
        public long ID { get; set; }
        public string EventVideo { get; set; }
        public string Event { get; set; }
        public string EventDate { get; set; }
        public string CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public List<EventComments> Comments { get; set; }
        public string Event_Date { get; set; }
        public EventComments singleComm { get; set; }
    }

    public class EventComments
    {
        public long ID { get; set; }
        public string CreatedDate { get; set; }
        public string Comments { get; set; }
        public long Userid { get; set; }
        public long Eventid { get; set; }
        public string Stu_Profile_Image { get; set; }
        public string Stu_Name { get; set; }
        public string EmailID { get; set; }
        public string ReportReason { get; set; }
        public long ReportBy { get; set; }
        public bool IsReported { get; set; }
        public long UserTypeID { get; set; }
    }

    #endregion

    public class Newsfeed
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventImage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public string Created_Date { get; set; }
    }

    public class ExamDetails_Newsfeed
    {
        public long ID { get; set; }
        public string Exam { get; set; }
        public DateTime ExamDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string tempdate { get; set; }

    }

    public class NewsfeedDetails
    {
        public List<Newsfeed> Newsfeed { get; set; }
        public List<GoalDetails> GoalDetails { get; set; }
        public List<ExamDetails_Newsfeed> ExamForNewsfeed { get; set; }
        public List<LhsFeedDetails> feedlist { get; set; }
    }

    #region site admin question panel

    public class Questionlist
    {
        public string ExamType { get; set; }
        public string Subject { get; set; }
        public string Exampattern { get; set; }
        public string Year { get; set; }
        public string TopicId { get; set; }
    }

    public class AddTestQuestion
    {
        public string ExamPattern { get; set; }
        public long DescID { get; set; }
        public string Description { get; set; }
        public string DescriptionText { get; set; }
        public string Question { get; set; }
        public string Diagram { get; set; }
        public string Year { get; set; }
        public long ExamID { get; set; }
        public long SubjectID { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string CorrectOption { get; set; }
        public string QuestionType { get; set; }
        public long QuestionID { get; set; }
        public long QuestionTypeID { get; set; }
        public long TopicID { get; set; }
        public string Topic { get; set; }
        public long Marks { get; set; }
        public long SerialNo { get; set; }
        public long TempSerialNo { get; set; }
        public string TempQuestion { get; set; }
        public string RawQstn { get; set; }

    }

    public class newTopicList
    {
        public long TopicID { get; set; }
        public string TopicText { get; set; }
    }

    public class QuestionPanel
    {
        public List<string> Year { get; set; }
        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public List<ExamSubject> ExamSubject { get; set; }

        public List<ExamTopic> ExamTopic { get; set; }
        public List<TypeOfQues> typeOfQues { get; set; }
    }

    public class TypeOfQues
    {
        public long ID { get; set; }
        public string QuestionType { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }

    }

    #endregion

    public class SubjectDetails_SiteAdmin
    {
        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public List<ExamSubject> ExamSubject { get; set; }
        public long SID { get; set; }
    }
    public class EditSubject_SiteAdmin
    {
        public long SubjectID { get; set; }
        public string Subject { get; set; }
        public long ExamID { get; set; }
        public string ExamType { get; set; }
        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public string OldSub { get; set; }
        public long OldExamID { get; set; }
    }

    #region ADD Syllabus by sneha
    public class SyllabusList
    {
        public string ExamType { get; set; }
        public string Subject { get; set; }
        public long ContentID { get; set; }
        public long TopicId { get; set; }
        public string Topic { get; set; }
        public long ExamID { get; set; }
        public long SubjectID { get; set; }
        public string TopicContent { get; set; }
        public List<ExamSubject> ExamSubject { get; set; }

        public List<ExamTopic> ExamTopic { get; set; }

        public List<ExamTypeDetails> ExamTypeDetails { get; set; }
        public List<AddSyllabus> sylllist { get; set; }
        public AddSyllabus singlesyllabus { get; set; }
    }

    public class AddSyllabus
    {
        public long ContentID { get; set; }
        public long TopicId { get; set; }
        public string Topic { get; set; }
        public long ExamID { get; set; }
        public long SubjectID { get; set; }
        public string TopicContent { get; set; }
        public string Subject { get; set; }
        public string NewTopicContent { get; set; }

    }

    #endregion

    public class Data_Calender
    {
        public int id { get; set; }
        public string TempDate { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }

    public class SocialOrSiteAdminUserlastMessage
    {
        public long MessageId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public int SenderTypeID { get; set; }
        public int ReceiverTypeID { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProfileImage { get; set; }
        public bool IsRead { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddUser_SiteAdmin
    {
        public long UID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long UserType_FK { get; set; }
        public string UserType { get; set; }
        public List<Login_Type> Login_Type { get; set; }
        public string TempEmail { get; set; }
    }

    public class UserDetails_SiteAdmin
    {
        public List<AddUser_SiteAdmin> UserDetails { get; set; }
        public List<Login_Type> Login_Type { get; set; }

    }

    public class Admin_User
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public long UserType_FK { get; set; }
    }
    public class CollegeDetails_SiteAdmin
    {
        public List<CollegeDay> CollegeDay { get; set; }
    }
    #region Class Teacher Messages
    public class ClassTeacherListingDetails
    {
        public long ID { get; set; }
        public long UserId { get; set; }
        public string Teacher_Name { get; set; }
        public string ProfileImage { get; set; }
        public bool STATUS { get; set; }
        public bool IsOnline { get; set; }
        public string LastLoginTime { get; set; }
    }

    public class ClassTeacherMessages
    {
        public long ID { get; set; }
        public long SenderID { get; set; }
        public long ReceiverID { get; set; }
        public string Message { get; set; }
        public string Profile_Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public string tmpDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }
        public long SchoolID { get; set; }
    }

    public class ClassTeacherMessageProp
    {
        public long UserId { get; set; }
        public string Teacher_Name { get; set; }
        public List<ClassTeacherListingDetails> details { get; set; }
        public List<ClassTeacherMessages> messages { get; set; }
    }
    #endregion

    #region Class Teacher Time Table 29.03.2019

    public class ClassTeacherTimetable
    {
        public long ID { get; set; }
        public string Period { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

    }

    public class timetable_prop
    {
        public long SchoolId { get; set; }
        public long TeacherId { get; set; }
        public int DivisionId { get; set; }
        public int LevelId { get; set; }
        public long ClassId { get; set; }
        public int SemesterId { get; set; }
        public int MonthId { get; set; }
    }

    #endregion



    //public class AddStudentDetails
    //{
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //    public string Stu_Name { get; set; }
    //    public DateTime Stu_DOB { get; set; }
    //    public string TempStudentDOB { get; set; }
    //    public DateTime Stu_JoiningDt { get; set; }
    //    public string TempJoiningDate { get; set; }
    //    public string Stu_MObile_No { get; set; }
    //    public string Stu_Profile_Image { get; set; }
    //    public string Stu_RegistrationNo { get; set; }
    //    public string tempStu_RegistrationNo { get; set; }
    //    public string Stu_FutureAmbition { get; set; }
    //    public string Stu_Favourite_Books { get; set; }
    //    public DateTime LastLoginTime { get; set; }
    //    public long LoginTpe_FK { get; set; }
    //    public int Level_ID { get; set; }
    //    public int DivisionID { get; set; }
    //    public bool STATUS { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public int Stu_Gender_FK { get; set; }
    //    public string School_Name { get; set; }
    //    public int Stream_ID_FK { get; set; }
    //    public long UserID_FK { get; set; }
    //    public long UID { get; set; }
    //    public long CreatedBy_FK { get; set; }
    //    public long School_ID { get; set; }
    //    public long Class_ID { get; set; }
    //    public List<Gender> GenderInfo { get; set; }

    //    public List<Division> Division { get; set; }
    //    public List<Level> Level { get; set; }
    //    public List<Class_Details> Class { get; set; }
    //    public List<Semester> Semester { get; set; }
    //    public List<School> School { get; set; }
    //    public List<AddsubjectInfo> SubjectInfo { get; set; }
    //    public List<SchoolTopic> SchoolTopic { get; set; }

    //    public List<Division> Divisions { get; set; }
    //    public List<Level> Levels { get; set; }
    //    public List<ClassDetails> ClassDetails { get; set; }
    //    public List<ClassStream> ClassStream { get; set; }
    //    public List<StudentListing> StudentListing { get; set; }
    //    public string Class_Name { get; set; }
    //    public string Division_Name { get; set; }
    //    public string Level_Name { get; set; }

    //    public StudentListing Student_Details { get; set; }
    //    public List<ClassTeacherTimetable> Classtimetable { get; set; }
    //    public List<ClassTeacherListingDetails> ClassTeacherLsiting { get; set; }

    //}
    //public class AddStudent
    //{
    //    public List<ClassInfo> ClassInfo { get; set; }
    //    public List<ClassStream> ClassStream { get; set; }
    //    public List<Gender> GenderInfo { get; set; }
    //    public List<AddStudentDetails> AddStudentDetails { get; set; }
    //    public List<Division> Division { get; set; }
    //    public List<Level> Level { get; set; }
    //    public List<UserDetails> UserDetails { get; set; }
    //    public List<ClassSpecificaton> ClassSpecificaton { get; set; }

    //}

    //public class StudentListing
    //{
    //    public long UID { get; set; }
    //    public long UserID_FK { get; set; }
    //    public int Level_ID { get; set; }
    //    public long Class_ID { get; set; }
    //    public int DivisionID { get; set; }
    //    public string Stu_RegistrationNo { get; set; }
    //    public string Stu_Name { get; set; }
    //    public string Class_Name { get; set; }
    //    public string Division_Name { get; set; }
    //    public string Level_Name { get; set; }
    //    public AddStudentDetails Student_Details { get; set; }

    //}
    public class SchoolTopic
    {
        public long ID { get; set; }
        public string Topic_Name { get; set; }
        public string CreatedDate { get; set; }
    }


    #region Class Teacher Forum

    public class ClassTeacherForum
    {
        public string Forum_Name { get; set; }
        public long Forum_ID { get; set; }
        public List<ClassTeacherForumGroups> grouplist { get; set; }
        public List<ClassTeacherTopic> topic { get; set; }
        public List<ClassTeacherTopicComment> Comment { get; set; }

        public ClassTeacherForumGroups TopicGroup { get; set; }
        public ClassTeacherTopic CommentTopic { get; set; }
        public ClassTeacherTopicComment TopicsComment { get; set; }
    }

    public class ClassTeacherForumGroups
    {
        public long Forum_ID { get; set; }
        public string Forum_Name { get; set; }
        public string Forum_About { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public long CreatedByID { get; set; }
        public long SchoolID { get; set; }
    }
    public class ClassTeacherTopic
    {
        public long ID { get; set; }
        public long Topic_ID { get; set; }
        public string Topic_Name { get; set; }
        public long Forum_ID_FK { get; set; }
        public string CreatedDate { get; set; }
        public Boolean Status { get; set; }
        public long CreatedById { get; set; }
        public long SchoolID { get; set; }
        public string CreatedByName { get; set; }
    }
    public class ClassTeacherTopicComment
    {
        public long CommentId { get; set; }
        public string CommentText { get; set; }
        public string CreatedDate { get; set; }
        public long Forum_ID_FK { get; set; }
        public long Topic_ID_FK { get; set; }
        public string Commentator_Name { get; set; }
        public string Profile_Image { get; set; }
        public Boolean IsReported { get; set; }

        public long TeacherID_FK { get; set; }
        public long School_ID_FK { get; set; }

        public long TopicCommentLikes { get; set; }
        public long TopicCommentDislikes { get; set; }
    }
    public class ClassTeacherCommunityCommentReport
    {
        public long ReportId { get; set; }
        public string ReportReason { get; set; }
        public long Comment_ID_FK { get; set; }
        public long ReportedBy_ID_FK { get; set; }
        public long SchoolID { get; set; }
        public string CreatedDate { get; set; }
        public Boolean Status { get; set; }
        public Boolean IsRead { get; set; }
    }
    #endregion
    #region Class Teacher Curriculum

    public class TeachersSyllabusTopicDetails
    {
        public long ID { get; set; }
        public string Topic_Name { get; set; }
        public long Subject_ID_FK { get; set; }
        public long Unit_ID_FK { get; set; }
        public long Class_ID_FK { get; set; }
        public string CreatedDate { get; set; }
        public Boolean IsActive { get; set; }



    
    }


    public class TeachersCurriculum
    {
        public long ID { get; set; }
        public long Topic_ID_FK { get; set; }
        public string Topic_Name { get; set; }
        public long SubjectID_FK { get; set; }
        public string SubjectName { get; set; }
        public long ClassID { get; set; }
        public string ClassName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long Teacher_ID_FK { get; set; }
        public string TeacherName { get; set; }
        public long SchoolId { get; set; }
        public short Semester_ID_FK { get; set; }
        public short Week_ID_FK { get; set; }
        public string Week_Name { get; set; }
        public long DeptId_FK { get; set; }
        public string CreatedDate { get; set; }
        public List<SubjectDetails> Subjectdetails { get; set; }
        public List<TeachersCurriculum> CurriculumDetails { get; set; }
    }
    #endregion

    #region Class Teacher Assignment Details

    public class ClassTeacherTopicDetails
    {
        public long Topic_ID { get; set; }
        public string Topic_Name { get; set; }
        public long Subject_ID_FK { get; set; }
        public Boolean IsActive { get; set; }
        public string CreatedDate { get; set; }
        public long SchoolID { get; set; }
        public long Unit_ID_FK { get; set; }
    }
    
    public class ClassTeacherAssignmentDetails
    {
        public long ID { get; set; }
        public string AssignmentName { get; set; }
        public long SubjectID_FK { get; set; }
        public long TopicID_FK { get; set; }
        public string Topic_Name { get; set; }
        public string Semester { get; set; }
        public long SchoolID { get; set; }
        public long ClassID_FK { get; set; }
        public long TeacherID_FK { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Boolean Status { get; set; }
        public string CreatedDate { get; set; }
        public short SemesterID_FK { get; set; }
        public string TeacherName { get; set; }
        public float FullMarks { get; set; }
        public decimal HighestMarks { get; set; }
        public decimal LowestMarks { get; set; }
        public decimal AverageMarks { get; set; }
        public List<ClassTeacherAssignmentDetails> AssignmentDetails { get; set; }
        public List<ClassTeacherAssignmentFeedbackDetails> AssignmentFeedbackDetails { get; set; }
        public List<ClassTeacherTopicDetails> TopicDetails { get; set; }



        
        public string StudentName { get; set; }
        public string FeedbackComment { get; set; }
        public string Obtained_Marks { get; set; }
        public string Rating { get; set; }

        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string Attendence { get; set; }
        public string Marks { get; set; }
    }

    public class ClassTeacherAssignmentFeedbackDetails
    {
        public long ID { get; set; }
        public long AssignmentID_FK { get; set; }
        public string StudentName { get; set; }
        public string StudentRegNo { get; set; }
        public string FeebackComment { get; set; }
        public long TeacherID_FK { get; set; }
        public long SchoolID { get; set; }
        public Boolean Status { get; set; }
        public string CreatedDate { get; set; }
        public long StudentID_FK { get; set; }
        public Decimal Obtained_Marks { get; set; }
        public Decimal Rating { get; set; }
    }

    #endregion

    #region SCHOOL STUDENT PANEL
    public class SchoolStudentEdit
    {
        public long UID { get; set; }
        public string Email { get; set; }
        public string demoEmail { get; set; }
        public long UserID_FK { get; set; }

        public string Password { get; set; }
        public string Stu_Name { get; set; }
        public int Stu_Gender_FK { get; set; }
        public DateTime Stu_DOB { get; set; }
        public string strDob { get; set; }
        public string Stu_MObile_No { get; set; }
        public long School_ID { get; set; }
        public string SchoolName { get; set; }
        public string Stu_RegistrationNo { get; set; }
        public string Stu_Profile_Image { get; set; }
        public int DivisionID { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public List<Gender> GenderInfo { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassObj { get; set; }
        public string tempDob { get; set; }
        public string tempEmail { get; set; }
    }


    public class SchlStuCurriCulum
    {
        public long UID { get; set; }
        public long TopicID { get; set; }
        public string TopicName { get; set; }
        public long SubjectID { get; set; }
        public string Subject { get; set; }
        public int DivisionID { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long SchoolID { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassObj { get; set; }
        public List<SchoolSubjects> SchoolSub { get; set; }
        public List<ClassTopic> classTopic { get; set; }
    }

    public class ClassTopic
    {
        public long UID { get; set; }
        public long TopicID { get; set; }
        public string TopicName { get; set; }
        public long SubjectID { get; set; }
        public string Subject { get; set; }
        public int DivisionID { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long SchoolID { get; set; }
    }

    public class SchoolSubjects
    {
        public long ID { get; set; }
        public string Subject { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public long School_ID { get; set; }
    }

    public class ClassForum
    {
        public List<SchoolSubjects> subs { get; set; }
        public List<SubjectForum> SubForum { get; set; }
    }

    public class SubjectForum
    {
        public long ForumID { get; set; }
        public string ForumName { get; set; }
        public string ForumAbout { get; set; }
        public string Subject { get; set; }
        public long SubjectID { get; set; }
        public int Level_ID { get; set; }
        public long Class_ID { get; set; }
        public long School_ID { get; set; }
        public long CreatedByUID { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ForumStatus { get; set; }
    }

    public class SubjectForumTopics
    {
        public long TopicID { get; set; }
        public long ForumID { get; set; }

        public string TopicName { get; set; }
        public long CreatedByUID { get; set; }
        public string CreatedByName { get; set; }
        //public DateTime TopicCreatedDate { get; set; }
        public string TopicCreatedDate { get; set; }
        public bool Status { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public string ForumName { get; set; }
        public long Class_ID { get; set; }
        public long School_ID { get; set; }
        public long LoginType { get; set; }
        


    }

    public class ForumDetails
    {
        public long ComID { get; set; }
        public string Topic { get; set; }
        public long TopicID { get; set; }
        public string TopicCreatedByName { get; set; }
        public long Class_ID { get; set; }
        public long School_ID { get; set; }
        public long ForumID { get; set; }
        public string Forum { get; set; }
        public string TopicCreatedDate { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public DateTime SubjectForumCreatedDate { get; set; }
        public List<SubjectForum> subForum { get; set; }
        public List<SubjectForumTopics> topicList { get; set; }
        public List<SubjectForumTopicComment> comments { get; set; }
        public SubjectForumTopicComment singleComment { get; set; }
    }
    // public class SingleSubjectForumTopicSingle

    public class ForumTopicWiseComment
    {
        public long TopicID { get; set; }
        public string Topic { get; set; }
        public string Forum { get; set; }

        public List<SubjectForumTopicComment> comments { get; set; }
    }




    public class SubjectForumTopicComment
    {
        public long CommentID { get; set; }
        public long Comment_LIKE { get; set; }
        public long Comment_DISLIKE { get; set; }
        public string Comment_TEXT { get; set; }
        public string CommentCommented_DATE { get; set; }
        public long CommentCreatedByUID { get; set; }
        public bool Status { get; set; }
        public long ForumTopicID { get; set; }
        public string CommentCreatedByName { get; set; }
        public string UserProfileImg { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsReported { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public string TopicName { get; set; }
        public string TopicCreatedDate { get; set; }
        public long TopicCreatedByUID { get; set; }
        public string ForumName { get; set; }
        public long ForumID { get; set; }
        public long LoginType { get; set; }
    }


    public class SubjectForum_Like_OR_Dislike
    {
        public long ID { get; set; }
        public long UID { get; set; }
        public long CommentID { get; set; }
        public string WhoLiked { get; set; }
        public bool Likes { get; set; }
        public bool DisLike { get; set; }
        public DateTime CreatedDate { get; set; }
        public String LikeVal { get; set; }
        public bool Status { get; set; }
    }


    #region ASSIGMMENT
    public class STUDENT_ASSIGNMENTS
    {
        public long ID { get; set; }

        public long UserID { get; set; }
        public long SubjectID { get; set; }
        public long TopicID { get; set; }
        public string AssignmentGiven { get; set; }
        public string AssignmentUploaded { get; set; }
        public string AssignmentFile { get; set; }
        public string StudentComments { get; set; }
        public long SchoolID { get; set; }
        public long ClassID { get; set; }
        public long DIV_ID { get; set; }
        public int Level_ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ISActive { get; set; }
        public List<SubjectDetails> subjs { get; set; }
        public List<AssignmentList> Asslist { get; set; }
    }

    public class StudentUploadAssignment
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long SubjectID { get; set; }
        public long TopicID { get; set; }
        public long AssignmentID { get; set; }
        public string AssignmentUploaded { get; set; }
        public string AssignmentFile { get; set; }
        public string StudentComments { get; set; }
        public long SchoolID { get; set; }
        public long ClassID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ISActive { get; set; }
        public List<SubjectDetails> subjs { get; set; }

    }
    public class SchoolSyllabusTopic
    {
        public long Topic_ID { get; set; }
        public string Topic_Name { get; set; }
        public long Subject_ID_FK { get; set; }
        public bool IsActive { get; set; }
        public long SchoolID { get; set; }
        public long Unit_ID_FK { get; set; }
        public short TermID { get; set; }
    }


    public class AssignmentList
    {
        public long ID { get; set; }
        public string AssignmentName { get; set; }
        public string Subject { get; set; }
        public long SubID { get; set; }
        // public long TopicID_FK { get; set; }
        public long TopicID { get; set; }
        public string Topic_Name { get; set; }
        public string AssignmentFile { get; set; }
        public string CreatedDate { get; set; }

    }
    #endregion


    #region RATE LESSON
    public class StudentRateLesson
    {
        public long ID { get; set; }
        public int LevelID { get; set; }
        public long ClasssID { get; set; }
        public long TopicID { get; set; }
        public long SubjectID { get; set; }
        public long TeacherID { get; set; }
        public string Topic { get; set; }
        public long SchoolID { get; set; }
        public long StudentID { get; set; }
        public long RatingStar { get; set; }
        public string FeedBackComment { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public List<SubjectDetails> subjs { get; set; }
        public List<SchoolSyllabusTopic> tpcs { get; set; }
    }
    #endregion

    #endregion

    public class AssignPrivilege
    {
        public List<TeacherListing> TeacherListing { get; set; }
        public List<Syllabus_Subject> Syllabus_Subjects { get; set; }
        public List<Depertment_Subject> Depertment_Subject { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<Class_Details> Class_Details { get; set; }

        public long DepertmentID_Hod { get; set; }
        public long TempDepertmentID_Hod { get; set; }

        public int DivisionID_ClassTeacher { get; set; }
        public int LevelID_ClassTeacher { get; set; }
        public long ClassID_ClassTeacher { get; set; }
        public long TempClassID_ClassTeacher { get; set; }

        public int DivisionID_SubjectTeacher { get; set; }
        public int LevelID_SubjectTeacher { get; set; }
        public long ClassID_SubjectTeacher { get; set; }
        public long SubjectID_SubjectTeacher { get; set; }
        public long TempClassID_SubjectTeacher { get; set; }
        public long TempSubjectID_SubjectTeacher { get; set; }
        public long DeptID_FK { get; set; }

        public long ID { get; set; }
        public long TeacherID { get; set; }
        public long SchoolID { get; set; }
        public long AssignedBy { get; set; }
        public bool Status { get; set; }

        public string IschkedVal_hod { get; set; }
        public string IschkedVal_classTeacher { get; set; }
        public string IschkedVal_subjectTeacher { get; set; }
    }

    public class EditHod
    {
        public List<TeacherListing> TeacherListing { get; set; }
        public List<Depertment_Subject> Depertment_Subject { get; set; }
        public long TeacherID { get; set; }
        public long DepertmentID { get; set; }
        public string Teacher { get; set; }
    }

    public class EditClassTeacher
    {
        public List<TeacherListing> TeacherListing { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassInfo { get; set; }
        public long TeacherID { get; set; }
        public int DivisionID { get; set; }
        public int LevelID { get; set; }
        public long ClassID { get; set; }
        public string Teacher { get; set; }
    }

    public class EditSubjectTeacher
    {
        public List<TeacherListing> TeacherListing { get; set; }
        public List<Syllabus_Subject> Syllabus_Subjects { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<ClassDetails> ClassInfo { get; set; }
        public long TeacherID { get; set; }
        public long SubjectID { get; set; }
        public int DivisionID { get; set; }
        public int LevelID { get; set; }
        public long ClassID { get; set; }
        public string Teacher { get; set; }
    }

    public class Depertment_Subject
    {
        public string Depertment { get; set; }
        //public int DepertmentID { get; set; }
        public long DepertmentID { get; set; }
    }

    public class Feedback_SuperAdmin
    {
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<Class_Details> Class_Details { get; set; }
        public List<Syllabus_Subject> Syllabus_Subjects { get; set; }
        public List<Semester> Semester { get; set; }
    }

    public class TeacherListing
    {
        public string Name { get; set; }
        public long UserID { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
        public bool IsOnline { get; set; }
        public string Profile_Image { get; set; }
        public DateTime LastLoginTime { get; set; }

        public string LoginType { get; set; }
        public bool Status { get; set; }


        public long TeacherTypeID { get; set; }
        public string Name_Hod { get; set; }
        //public string Subject_Hod { get; set; }
        public string Depertment_Hod { get; set; }

        public string Name_ClassTeacher { get; set; }
        public string Division_ClassTeacher { get; set; }
        public string Level_ClassTeacher { get; set; }
        public string Class_ClassTeacher { get; set; }

        public string Name_SubjectTeacher { get; set; }
        public string Subject_SubjectTeacher { get; set; }
        public string Division_SubjectTeacher { get; set; }
        public string Level_SubjectTeacher { get; set; }
        public string Class_SubjectTeacher { get; set; }
    }
    public class Review_SocialStudent
    {
        public long ID { get; set; }
        public string Answer { get; set; }
        public long UID { get; set; }
        public bool IsCorrect { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string CorrectOption { get; set; }
        public long SubjectID { get; set; }
        public string Topic { get; set; }
        public string Year { get; set; }
        public long QuestionID { get; set; }
        public string GUID { get; set; }
        public string Diagram { get; set; }
        public long QuestionTypeID { get; set; }

    }

    public class LhsFeedDetails
    {
        public long ID { get; set; }
        public DateTime DateCreated { get; set; }
        public string LatestNews { get; set; }
        public long SectionID { get; set; }
        public string SectionName { get; set; }
        public string Type { get; set; }
        public long UID { get; set; }
        public string SubSection { get; set; }
        public long SubSectionID { get; set; }
    }

   
}