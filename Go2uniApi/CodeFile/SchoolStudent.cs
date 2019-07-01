using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Go2uniApi.Models;
using Newtonsoft.Json;

namespace Go2uniApi.CodeFile
{
    public class SchoolStudent
    {

        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();


        public SchoolStudentEdit EditSchoolStudentProfile(long UID)
        {
            SchoolStudentEdit Result = new SchoolStudentEdit();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Isnull(Sd.UserID_FK,'')as UserID_FK,Isnull(Sd.Stu_Name,'')as Stu_Name,
                                    Isnull(Sd.Stu_Gender_FK,'')as Stu_Gender_FK,
                                    Isnull(Sd.Stu_DOB,'') as Stu_DOB,Isnull(Sd.Stu_MObile_No,'')as Stu_MObile_No,
                                    Isnull(Sd.Stu_Profile_Image,'')as Stu_Profile_Image,
                                    Isnull(Sd.DivisionID,'') as DivisionID,Isnull(Sd.Level_ID,'')as Level_ID,
                                    isnull(Sd.Stu_RegistrationNo,'') as Stu_RegistrationNo,isnull(Sd.Class_ID,'') as Class_ID,
                                    Isnull(Users.Email,'')as Email,Isnull(Users.password,'')as Password,
                                    Isnull(schl.ID,'') as SchoolID,Isnull(schl.School_Name,'') as School_Name
                                    from studentinfo.Student_Details as Sd
                                    inner join admin.users as users on Sd.UserID_FK=Users.UID 
                                    inner join admin.School_Info as schl ON Sd.School_ID=schl.ID
                                    where UserID_FK=@UID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UID", UID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while(reader.Read())
                    {
                        Result.UID = Convert.ToInt64(reader["UserID_FK"]);
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.Stu_Gender_FK = Convert.ToInt16(reader["Stu_Gender_FK"]);
                        Result.Stu_DOB = Convert.ToDateTime(reader["Stu_DOB"]);
                        Result.Stu_MObile_No = Convert.ToString(reader["Stu_MObile_No"]);
                        Result.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
                        Result.DivisionID = Convert.ToInt16(reader["DivisionID"]);
                        Result.Level_ID = Convert.ToInt16(reader["Level_ID"]);
                        Result.Stu_RegistrationNo = Convert.ToString(reader["Stu_RegistrationNo"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);
                        Result.School_ID = Convert.ToInt64(reader["SchoolID"]);
                        Result.SchoolName = Convert.ToString(reader["School_Name"]);
                        Result.Class_ID = Convert.ToInt64(reader["Class_ID"]);

                    }
                }
            }
            catch (Exception ex)
            {

                
            }
            return Result;
        }
        public List<Gender> GetAllGender()
        {
            List<Gender> Result = new List<Gender>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllGender", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Gender temp = new Gender();
                        temp.GenderID = Convert.ToInt16(reader["GenderID"]);
                        temp.GenderName = Convert.ToString(reader["GenderName"]);
                        Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }


        public List<Division> GetDivisionList()
        {
            List<Division> obj = new List<Division>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                string query = @"select ID,Division_Name from admin.Division";

                SqlCommand cmd = new SqlCommand(query, connt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<Division>>(str);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }


        public List<Level> GetlevelListByDivID(int DivID)
        {
            List<Level> Result = new List<Level>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" select ID,Level_Name,Division_ID  from admin.Level where Division_ID=@Division_ID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Division_ID", DivID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Level temp = new Level();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Level_Name = Convert.ToString(reader["Level_Name"]);
                        temp.Division_ID = Convert.ToInt16(reader["Division_ID"]);
                        Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }


        public List<ClassDetails> GetClassByLevelID(int LevelID,long SchoolID)
        {
            List<ClassDetails> obj = new List<ClassDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,Class_Name from admin.Class
                                    where Level_ID=@Level_ID and School_ID=@SchoolID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Level_ID", LevelID);
                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassDetails temp = new ClassDetails();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.Class_Name = Convert.ToString(reader["Class_Name"]);
                        obj.Add(temp);    
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {

                
            }
            return obj;
        }


        public List<SchoolSubjects> SubjectByLevelID(int LevelID, long SchoolID)
        {
            List<SchoolSubjects> listObj = new List<SchoolSubjects>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select  ID,Subject from admin.Syllabus_Subject where Level_ID=@levelID and School_ID=@schlid";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@levelID", LevelID);
                    cmd.Parameters.AddWithValue("@schlid", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SchoolSubjects temp = new SchoolSubjects();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        listObj.Add(temp);
                    }
                    con.Close();
                }

                }
            catch (Exception ex)
            {

                
            }
            return listObj;
        }

        public string UpdateSchoolStudentProfile(SchoolStudentEdit Info)
        {
            string str = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    if (Info.demoEmail == Info.Email)
                    {
                        string Query = string.Empty;
                        if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                        {
                            Query = @"Update[admin].[users]set Email=@Email,Password=@Password
                                 where UID=@UserID_FK
                                 update studentinfo.Student_Details
                                SET Stu_Name=@Stu_Name,
                                Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                Stu_MObile_No=@Stu_MObile_No,Stu_Profile_Image=@Stu_Profile_Image,
                                DivisionID=@DivisionID,
                                Level_ID=@Level_ID,Class_ID=@Class_ID
                                 where UserID_FK=@UserID_FK";
                        }
                        else
                        {
                            Query = @"Update[admin].[users]set Email=@Email,Password=@Password
                                 where UID=@UserID_FK
                                 update studentinfo.Student_Details
                                SET Stu_Name=@Stu_Name,
                                Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                Stu_MObile_No=@Stu_MObile_No,
                                DivisionID=@DivisionID,
                                Level_ID=@Level_ID,Class_ID=@Class_ID

                                 where UserID_FK=@UserID_FK";
                        }

                        SqlCommand cmd = new SqlCommand(Query,con);
                        cmd.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                        cmd.Parameters.AddWithValue("@Stu_Name", Info.Stu_Name);
                        cmd.Parameters.AddWithValue("@Stu_Gender_FK", Info.Stu_Gender_FK);
                        cmd.Parameters.AddWithValue("@Stu_DOB", Info.Stu_DOB);
                        cmd.Parameters.AddWithValue("@Email", Info.Email);
                        cmd.Parameters.AddWithValue("@Password", Info.Password);
                        
                        cmd.Parameters.AddWithValue("@Stu_MObile_No", Info.Stu_MObile_No);
                        cmd.Parameters.AddWithValue("@DivisionID", Info.DivisionID);
                        cmd.Parameters.AddWithValue("@Level_ID", Info.Level_ID);
                        cmd.Parameters.AddWithValue("@Class_ID", Info.Class_ID);
                        if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                        {
                            cmd.Parameters.AddWithValue("@Stu_Profile_Image", Info.Stu_Profile_Image);
                        }
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                str = Info.Stu_Profile_Image;
                            }
                            else
                            {
                                str = "Success! Profile Sucessfuly Updated";
                            }
                        }
                        else
                        {
                            str = "Social student Profile Updation Failed";
                        }
                        con.Close();
                    }

                    else
                    {
                        if (!(CheckEmailExists(Info.Email)))
                        {
                            string Query = string.Empty;
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                Query = @"Update[admin].[users]set Email=@Email,Password=@Password
                                 where UID=@UserID_FK
                                 update studentinfo.Student_Details
                                SET Stu_Name=@Stu_Name,
                                Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                Stu_MObile_No=@Stu_MObile_No,Stu_Profile_Image=@Stu_Profile_Image,
                                DivisionID=@DivisionID,
                                Level_ID=@Level_ID,Class_ID=@Class_ID
                                 where UserID_FK=@UserID_FK";
                            }
                            else
                            {
                                Query = @"Update[admin].[users]set Email=@Email,Password=@Password
                                 where UID=@UserID_FK
                                 update studentinfo.Student_Details
                                SET Stu_Name=@Stu_Name,
                                Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                Stu_MObile_No=@Stu_MObile_No,
                                DivisionID=@DivisionID,
                                Level_ID=@Level_ID,Class_ID=@Class_ID

                                 where UserID_FK=@UserID_FK";
                            }
                            SqlCommand cmd = new SqlCommand(Query, con);
                            cmd.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                            cmd.Parameters.AddWithValue("@Stu_Name", Info.Stu_Name);
                            cmd.Parameters.AddWithValue("@Stu_Gender_FK", Info.Stu_Gender_FK);
                            cmd.Parameters.AddWithValue("@Stu_DOB", Info.Stu_DOB);
                            cmd.Parameters.AddWithValue("@Email", Info.Email);
                            cmd.Parameters.AddWithValue("@Password", Info.Password);
                            cmd.Parameters.AddWithValue("@Stu_MObile_No", Info.Stu_MObile_No);
                            cmd.Parameters.AddWithValue("@DivisionID", Info.DivisionID);
                            cmd.Parameters.AddWithValue("@Level_ID", Info.Level_ID);
                            cmd.Parameters.AddWithValue("@Class_ID", Info.Class_ID);
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                cmd.Parameters.AddWithValue("@Stu_Profile_Image", Info.Stu_Profile_Image);
                            }
                            con.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                                {
                                    str = Info.Stu_Profile_Image;
                                }
                                else
                                {
                                    str = "Success! Profile Sucessfuly Updated";
                                }
                            }
                            else
                            {
                                str = "Social student Profile Updation Failed";
                            }

                        }
                        else
                        {
                            str = "This mail already exists";
                        }

                    }

                }
            }
            catch (Exception)
            {

                
            }
            return str;
        }



        public bool CheckEmailExists(string MailID)
        {
            bool result = false;

            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    string query = "select UID,Email,Password from admin.users where Email=@Email";
                    SqlCommand cmd = new SqlCommand(query, connt);
                    cmd.Parameters.AddWithValue("@Email", MailID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public List<ClassTopic> GetSchoolCurriculum(long Subid,long SchoolID)
        {
            List<ClassTopic> resList = new List<ClassTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    // change this query fetch values from select * from admin.Curriculum
                    // 9th/may/2019...... 

                    string Query = @"select curr.TopicId_FK,tpc.Topic_Name,curr.SubjectId_FK,sub.Subject,
                                    curr.StartDate,curr.EndDate,curr.Teacher_ID_FK,curr.Class_ID_FK,curr.SchoolID,curr.Semester_ID_FK
                                    from admin.Curriculum as curr 
                                    inner join admin.Syllabus_Topic as tpc on curr.TopicId_FK=tpc.Topic_ID
                                    inner join admin.Syllabus_Subject as sub on curr.SubjectId_FK=sub.ID
                                    where curr.SubjectId_FK=@subid and curr.SchoolID=@schlid";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@subid", Subid);
                    cmd.Parameters.AddWithValue("@schlid", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTopic res = new ClassTopic();
                        res.TopicID = Convert.ToInt64(reader["TopicId_FK"]);
                        res.TopicName = Convert.ToString(reader["Topic_Name"]);
                        res.SubjectID = Convert.ToInt64(reader["SubjectId_FK"]);
                        res.Subject = Convert.ToString(reader["Subject"]);
                        res.StartDate = Convert.ToString(reader["StartDate"]);
                        res.EndDate = Convert.ToString(reader["EndDate"]);
                        //res.DivisionID = Convert.ToInt16(reader["DivisionID"]);
                       // res.Level_ID = Convert.ToInt16(reader["LevelID"]);
                        res.Class_ID = Convert.ToInt64(reader["Class_ID_FK"]);
                       
                        resList.Add(res);
                    }

                }
            }
            catch (Exception ex)
            {

                
            }
            return resList;
        }


        public List<SchoolSubjects> SubjectByClassId(long SchoolID, long ClassID)
        {
            List<SchoolSubjects> listobj = new List<SchoolSubjects>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select  ID,Subject from admin.Syllabus_Subject
                                    where School_ID=@SchoolID and
                                    Class_ID=@ClassID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                  
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SchoolSubjects res = new SchoolSubjects();
                        res.ID = Convert.ToInt64(reader["ID"]);
                        res.Subject = Convert.ToString(reader["Subject"]);
                        listobj.Add(res);
                        
                    }


                }
            }
            catch (Exception ex)
            {

               
            }

            return listobj;
        }



        #region CLASS FORUM BY SNEHA

        public List<SubjectForum> SubjectForumByClassID(long SchoolID, long ClassID)
        {
            List<SubjectForum> listobj = new List<SubjectForum>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select forum.ForumID,forum.ForumName,forum.ForumAbout,forum.SubjectID,
                                    forum.LevelID,forum.ClassID,forum.SchoolID,forum.CreatedByUID,forum.CreatedDate,
                                    tchr.Name
                                    from [studentinfo].[SubjectForum] as forum 
                                    inner join Teacher.TeachersDetails as tchr ON forum.CreatedByUID=tchr.UID_FK
                                    where forum.ClassID=@classid and forum.SchoolID=@schoolid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    cmd.Parameters.AddWithValue("@schoolid", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SubjectForum res = new SubjectForum();
                        res.ForumID = Convert.ToInt64(reader["ForumID"]);
                        res.ForumName = Convert.ToString(reader["ForumName"]);
                        res.CreatedByUID = Convert.ToInt64(reader["CreatedByUID"]);
                        res.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        res.SubjectID = Convert.ToInt64(reader["SubjectID"]);
                        res.Level_ID = Convert.ToInt16(reader["LevelID"]);
                        res.Class_ID = Convert.ToInt64(reader["ClassID"]);
                        res.School_ID = Convert.ToInt64(reader["SchoolID"]);
                        res.CreatedByName = Convert.ToString(reader["Name"]);
                        listobj.Add(res);

                    }


                }
            }
            catch (Exception ex)
            {


            }

            return listobj;
        }


        public List<SubjectForumTopics> TopicListByForumID(long ForumID, long SchoolID, long ClassID)
        {
            List<SubjectForumTopics> listobj = new List<SubjectForumTopics>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    
                    string Query = @"select ft.TopicID,ft.TopicName,ft.ForumID,sf.ForumName,sf.ClassID,sf.SchoolID,ft.CreatedByUID,ft.UserType,ft.CreatedDate,ft.IsTeacher,ft.IsStudent,
                                    (case when ft.UserType=4 then (select isnull(stu.Stu_Name,'') from studentinfo.Student_Details stu where stu.UserID_FK=ft.CreatedByUID )
                                    else(select isnull(td.Name,'') Name from teacher.TeachersDetails td where td.UID_FK=ft.CreatedByUID) end) as CreatedName
                                    from studentinfo.SubjectForumTopic as ft 
                                    inner join [studentinfo].[SubjectForum] as sf ON ft.ForumID=sf.ForumID
                                    where ft.ForumID=@forumid and sf.ClassID=@ClassID and sf.SchoolID=@schoolid 
                                        ORDER BY ft.CreatedDate DESC";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@forumid", ForumID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    cmd.Parameters.AddWithValue("@schoolid", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SubjectForumTopics res = new SubjectForumTopics();
                        res.TopicID = Convert.ToInt64(reader["TopicID"]);
                        res.TopicName = Convert.ToString(reader["TopicName"]);
                        res.ForumID = Convert.ToInt64(reader["ForumID"]);
                        res.CreatedByUID = Convert.ToInt64(reader["CreatedByUID"]);
                        res.CreatedByName = Convert.ToString(reader["CreatedName"]);
                        res.TopicCreatedDate = Convert.ToString(reader["CreatedDate"]);
                        res.IsTeacher = Convert.ToBoolean(reader["IsTeacher"]);
                        res.IsStudent = Convert.ToBoolean(reader["IsStudent"]);
                        res.ForumName = Convert.ToString(reader["ForumName"]);
                        res.Class_ID = Convert.ToInt64(reader["ClassID"]);
                        res.School_ID = Convert.ToInt64(reader["SchoolID"]);
                        listobj.Add(res);
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return listobj;
        }




        public string InsertTopicByForumID(SubjectForumTopics Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[dbo].[SP_InsertForumTopic]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TopicName", Info.TopicName);
                    com.Parameters.AddWithValue("@ForumID", Info.ForumID);
                    com.Parameters.AddWithValue("@CreatedByUID", Info.CreatedByUID);
                    // com.Parameters.AddWithValue("@CreatedByName", Info.CreatedByName);
                    com.Parameters.AddWithValue("@UserType", Info.LoginType);
                    com.Parameters.AddWithValue("@IsStudent", Info.IsStudent);
                    com.Parameters.Add("@TopicID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        string id = com.Parameters["@TopicID"].Value.ToString();
                        Result = "Success!" + id;
                    }
                    else
                    {
                        Result = "Failed! ";
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }
            return Result;
        }


        public List<SubjectForumTopicComment> AllCommentsOfTopic(long TopicID)
        {
            List<SubjectForumTopicComment> Result = new List<SubjectForumTopicComment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {


                   
                    string query = @"select sfc.CommentID,sfc.Comment_LIKE,sfc.Comment_DISLIKE,
                                        sfc.Comment_TEXT,sfc.Commented_DATE,sfc.Status,
                                        sfc.ForumTopicID,sfc.IsDeleted,sfc.IsReported,
                                        sfc.UserProfileImg,sfc.IsTeacher,
                                        sfc.IsStudent,sft.TopicName,isnull(sft.CreatedDate, '') as TopicCreatedDate,
                                        sfc.CreatedByUID,sf.ForumName,sf.ForumID,
                                        (case when sfc.UserType=4 then (select isnull(stu.Stu_Name,'') from studentinfo.Student_Details stu where stu.UserID_FK=sfc.CreatedByUID )
                                        else(select isnull(td.Name,'') Name from teacher.TeachersDetails td where td.UID_FK=sfc.CreatedByUID) end) as CreatedByName
                                        from studentinfo.SubjectForumComment as sfc
                                        inner join studentinfo.SubjectForumTopic as sft on sfc.ForumTopicID=sft.TopicID
                                        inner join [studentinfo].[SubjectForum] as sf ON sft.ForumID=sf.ForumID
                                        where sfc.ForumTopicID=@tpcid and sfc.IsDeleted=0 order by sfc.CommentID desc";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@tpcid", TopicID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        SubjectForumTopicComment temp = new SubjectForumTopicComment();
                        temp.CommentID = Convert.ToInt64(reader["CommentID"]);

                        temp.Comment_LIKE = Convert.ToInt64(reader["Comment_LIKE"]);
                        temp.Comment_DISLIKE = Convert.ToInt64(reader["Comment_DISLIKE"]);
                        temp.Comment_TEXT = Convert.ToString(reader["Comment_TEXT"]);
                        temp.CommentCommented_DATE = Convert.ToString(reader["Commented_DATE"]);
                        temp.CommentCreatedByUID = Convert.ToInt64(reader["CreatedByUID"]);
                        temp.Status = Convert.ToBoolean(reader["Status"]);
                        temp.ForumTopicID = Convert.ToInt64(reader["ForumTopicID"]);
                        temp.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                        temp.IsReported = Convert.ToBoolean(reader["IsReported"]);
                        temp.CommentCreatedByName = Convert.ToString(reader["CreatedByName"]);
                        temp.UserProfileImg = Convert.ToString(reader["UserProfileImg"]);
                        temp.IsTeacher = Convert.ToBoolean(reader["IsTeacher"]);
                        temp.IsStudent = Convert.ToBoolean(reader["IsStudent"]);
                        temp.TopicName = Convert.ToString(reader["TopicName"]);
                        temp.TopicCreatedDate = Convert.ToString(reader["TopicCreatedDate"]);
                        // temp.TopicCreatedByUID = Convert.ToInt64(reader["TopicCreatedByUID"]);
                        temp.ForumName = Convert.ToString(reader["ForumName"]);
                        temp.ForumID = Convert.ToInt64(reader["ForumID"]);

                        Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }





        public List<SubjectForumTopics> getDetailsOfComment(long TopicID)
        {
            List<SubjectForumTopics> Result = new List<SubjectForumTopics>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                  
                    string Query = @"select  sft.TopicID,sft.ForumID,sft.TopicName,
                                    sft.CreatedByUID,sft.CreatedDate,sft.Status,
                                    IsTeacher,IsStudent,
                                    (case when sft.UserType=4 then (select isnull(stu.Stu_Name,'') from studentinfo.Student_Details stu where stu.UserID_FK=sft.CreatedByUID)
                                    else(select isnull(td.Name,'') Name from teacher.TeachersDetails td where td.UID_FK=sft.CreatedByUID) end) as CreatedName
                                    from studentinfo.SubjectForumTopic as sft where sft.TopicID=@Topic_ID";
                    SqlCommand com = new SqlCommand(Query, con);
                   
                    com.Parameters.AddWithValue("@Topic_ID", TopicID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        SubjectForumTopics temp = new SubjectForumTopics();
                        temp.TopicID = Convert.ToInt64(reader["TopicID"]);
                        temp.ForumID = Convert.ToInt64(reader["ForumID"]);
                        temp.TopicName = Convert.ToString(reader["TopicName"]);
                        temp.CreatedByUID = Convert.ToInt32(reader["CreatedByUID"]);
                        temp.TopicCreatedDate = Convert.ToString(reader["CreatedDate"]);
                        temp.Status = Convert.ToBoolean(reader["Status"]);
                        temp.CreatedByName = Convert.ToString(reader["CreatedName"]);
                        temp.IsTeacher = Convert.ToBoolean(reader["IsTeacher"]);
                        temp.IsStudent = Convert.ToBoolean(reader["IsStudent"]);
                        Result.Add(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }


        public string InsertCommentByTopicID(SubjectForumTopicComment Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"Insert into studentinfo.SubjectForumComment
                                    (Comment_LIKE,Comment_DISLIKE,Comment_TEXT,Commented_DATE,
                                    CreatedByUID,ForumTopicID,UserType,UserProfileImg,IsTeacher,IsStudent)
                                    values(@like,@dislike,@text,getdate(),@UID,@TopicID,@usertype,@profimg,@isteacher,@isStudent)";
                    SqlCommand com = new SqlCommand(query, con);

                    com.Parameters.AddWithValue("@like", 0);
                    com.Parameters.AddWithValue("@dislike", 0);
                    com.Parameters.AddWithValue("@text", Info.Comment_TEXT);
                    com.Parameters.AddWithValue("@UID", Info.CommentCreatedByUID);
                    com.Parameters.AddWithValue("@TopicID", Info.ForumTopicID);
                    com.Parameters.AddWithValue("@usertype", Info.LoginType);
                    if (Info.UserProfileImg != null && Info.UserProfileImg != "")
                    {
                        com.Parameters.AddWithValue("@profimg", Info.UserProfileImg);
                    }
                    else
                    {
                        com.Parameters.AddWithValue("@profimg", "");
                    }

                    com.Parameters.AddWithValue("@isteacher", 0);
                    com.Parameters.AddWithValue("@isStudent", 1);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Inserted";
                    }
                    else
                    {
                        Result = "Failed!";
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }





        public string InsertLikeOrDislikeValueOfSubjectForum(SubjectForum_Like_OR_Dislike Info)
        {
            string Result = string.Empty;
            try
            {
                if (!(CheckLikeOrDisLikeExists(Info.CommentID, Info.UID)))
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        string Query = @"Insert into studentinfo.SubjectForum_LikeOrDislike
                                        (UID,CommentID,Likes,Dislikes,CreatedDate,WhoLiked)
                                         values(@UID,@Comment_ID,@Likes,@DisLike,getdate(),@userName)";

                        SqlCommand com = new SqlCommand(Query, con);

                        com.Parameters.AddWithValue("@UID", Info.UID);
                        com.Parameters.AddWithValue("@Comment_ID", Info.CommentID);
                        if (Info.LikeVal == "Like")
                        {
                            com.Parameters.AddWithValue("@Likes", 1);
                            com.Parameters.AddWithValue("@DisLike", 0);
                        }
                        else if (Info.LikeVal == "Dislike")
                        {
                            com.Parameters.AddWithValue("@Likes", 0);
                            com.Parameters.AddWithValue("@DisLike", 1);
                        }
                        com.Parameters.AddWithValue("@userName", Info.WhoLiked);
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success! Inserted";
                        }
                        else
                        {
                            Result = "Failed! ";
                        }
                        con.Close();
                    }
                }

                else
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        if (Info.LikeVal == "Like")
                        {
                            if (CheckIfLikeisActiveOrnot(Info.CommentID, Info.UID))
                            {

                                string Query = @" Update studentinfo.SubjectForum_LikeOrDislike
                                              set Likes=@like,DisLikes=@dislike where CommentID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@comID", Info.CommentID);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@like", 0);
                                com.Parameters.AddWithValue("@dislike", 0);
                                  con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    Result = "Success! Inserted";
                                }
                                else
                                {
                                    Result = "Failed! ";
                                }
                                con.Close();
                            }

                            else
                            {
                                string Query = @"Update studentinfo.SubjectForum_LikeOrDislike
                                               set Likes=@like,DisLikes=@dislike 
                                               where CommentID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@comID", Info.CommentID);
                                com.Parameters.AddWithValue("@like", 1);
                                com.Parameters.AddWithValue("@dislike", 0);

                                con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    Result = "Success! Inserted";
                                }
                                else
                                {
                                    Result = "Failed! ";
                                }
                                con.Close();
                            }

                        }



                        if (Info.LikeVal == "Dislike")
                        {
                            if (CheckIfDislikeisActiveOrnot(Info.CommentID, Info.UID))
                            {
                                string Query = @"Update studentinfo.SubjectForum_LikeOrDislike set 
                                                Likes=@like,DisLikes=@dislike where CommentID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@comID", Info.CommentID);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                               


                                com.Parameters.AddWithValue("@like", 0);
                                com.Parameters.AddWithValue("@dislike", 0);


                                con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    Result = "Success! Inserted";
                                }
                                else
                                {
                                    Result = "Failed! ";
                                }
                                con.Close();
                            }

                            else
                            {
                                string Query = @" Update studentinfo.SubjectForum_LikeOrDislike 
                                             set Likes=@like,DisLikes=@dislike where CommentID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@comID", Info.CommentID);
                                com.Parameters.AddWithValue("@like", 0);
                                com.Parameters.AddWithValue("@dislike", 1);

                                con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    Result = "Success! Inserted";
                                }
                                else
                                {
                                    Result = "Failed! ";
                                }
                                con.Close();
                            }

                        }


                    }

                }
            }
            catch (Exception ex)
            {

            }
            return Result;
        }

        public bool CheckLikeOrDisLikeExists(long CommID, long UID)
        {
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "select UID,CommentID,Likes,Dislikes from  studentinfo.SubjectForum_LikeOrDislike where CommentID = @CommID and UID = @UID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CommID", CommID);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }



        public bool CheckIfLikeisActiveOrnot(long commID, long UID)
        {
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = " select Likes from studentinfo.SubjectForum_LikeOrDislike where Likes=1 and CommentID=@commID and UID=@UID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@commID", commID);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;

        }

        public bool CheckIfDislikeisActiveOrnot(long commID, long UID)
        {
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "select Dislikes from [studentinfo].[SubjectForum_LikeOrDislike] where DisLikes=1 and CommentID=@commID and UID=@UID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@commID", commID);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }



        internal string UpdateLikesDislikesInCommentTable(long comID, string value, long UID)


        {
            string Result = string.Empty;

            Online_Community_LikeORDisLike val = new Online_Community_LikeORDisLike();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {




                    string Query = @"update studentinfo.SubjectForumComment
                                    set Comment_Like=(
                                    select count(Likes) from studentinfo.SubjectForum_LikeOrDislike 
                                    where studentinfo.SubjectForum_LikeOrDislike.CommentID=studentinfo.SubjectForumComment.CommentID and Likes=1 and UID=@UID),
                                    Comment_Dislike=(select count(DisLikes) from studentinfo.SubjectForum_LikeOrDislike 
                                    where studentinfo.SubjectForum_LikeOrDislike.CommentID=studentinfo.SubjectForumComment.CommentID and DisLikes=1 and UID=@UID) 
                                    where CommentID=@comID";

                    SqlCommand com = new SqlCommand(Query, con);


                    com.Parameters.AddWithValue("@comID", comID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();

                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success! Inserted";
                    }
                    else
                    {
                        Result = "Failed! ";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return Result;
        }


        public SubjectForumTopicComment GetLikeDislikeCount(long CommID)
        {
            SubjectForumTopicComment res = new SubjectForumTopicComment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select CommentID,Comment_Like,Comment_Dislike,CreatedByUID from 
                                    studentinfo.SubjectForumComment where CommentID=@CommId";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@CommId", CommID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Comment_LIKE = Convert.ToInt64(reader["Comment_LIKE"]);
                        res.Comment_DISLIKE = Convert.ToInt64(reader["Comment_DISLIKE"]);
                        res.CommentID = Convert.ToInt64(reader["CommentID"]);
                        res.CommentCreatedByUID= Convert.ToInt64(reader["CreatedByUID"]);
                    }
                }

            }
            catch (Exception ex)
            {

              
            }

            return res;
        }


        //Edit comment SM//
        public SubjectForumTopicComment EditSubForumComment(long ForumCommID, long UID)
        {
            SubjectForumTopicComment Result = new SubjectForumTopicComment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select CommentID,Comment_TEXT,CreatedByUID from studentinfo.SubjectForumComment
                                    where CommentID=@comID and CreatedByUID=@UID";


                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@comID", ForumCommID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        //Comment temp = new Comment();
                        // temp.GroupID = Convert.ToInt32(reader["Group_ID"]);
                        Result.CommentID = Convert.ToInt64(reader["CommentID"]);
                        Result.Comment_TEXT = Convert.ToString(reader["Comment_TEXT"]);
                        Result.CommentCreatedByUID = Convert.ToInt64(reader["CreatedByUID"]);
                        //  Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result;
        }


        public string UpdateCommentByUID(SubjectForumTopicComment Info)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update studentinfo.SubjectForumComment
                                set Comment_TEXT=@CommentTxt
                                where CommentID=@commid and CreatedByUID=@UID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@commid", Info.CommentID);
                    com.Parameters.AddWithValue("@UID", Info.CommentCreatedByUID);
                    com.Parameters.AddWithValue("@CommentTxt", Info.Comment_TEXT);
                    con.Open();

                    if (com.ExecuteNonQuery() > 0)
                    {
                        res = "Success|Comment Updated Successfully";
                    }
                    else
                    {
                        res = "Failed| Failed to Update comment";
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {


            }
            return res;
        }

        public SubjectForumTopicComment RefreshCommentByUserID(long CommID, long UID)
        {
            SubjectForumTopicComment res = new SubjectForumTopicComment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select CommentID,Comment_TEXT from studentinfo.SubjectForumComment
                                      where CommentID=@comID and CreatedByUID=@UID";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@comID", CommID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        res.CommentID = Convert.ToInt64(reader["CommentID"]);
                        res.Comment_TEXT = Convert.ToString(reader["Comment_TEXT"]);
                        //res.CommentCreatedByUID = Convert.ToInt64(reader["CreatedByUID"]);
                        //res.Comment_LIKE = Convert.ToInt64(reader["Comment_LIKE"]);
                        //  res.Comment_DISLIKE = Convert.ToInt64(reader["Comment_DISLIKE"]);
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }


        //delete  comment sm//
        public string DeleteComment(long UID, long CommID)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"Update studentinfo.SubjectForumComment set IsDeleted=1 
                                        where CommentID=@CommID
                                        and CreatedByUID=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    cmd.Parameters.AddWithValue("@CommID", CommID);
                    //cmd.Parameters.AddWithValue("@Comment",)
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success|Comment Deleted";
                    }
                    else
                    {
                        res = "Failed|Failed to Delete";

                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }


        #endregion

        #region ASSIGNMENT

        public int GetDivIdByClassID(long ClassID , long SchoolID)
        {
            int res = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string Query = @"select  Level_ID from admin.Class where ID=@ID and School_ID=@SchoolID";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@ID", ClassID);
                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        res = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                }

            }
            catch (Exception ex )
            {

                
            }
            return res;
        }

        public List<SubjectDetails> GetSubjectByLevelID(int LevelID)
        {
            List<SubjectDetails> listObj = new List<SubjectDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,Subject from admin.Syllabus_Subject
                                    where Level_ID=@Level_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                  //  cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    cmd.Parameters.AddWithValue("@Level_ID", LevelID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SubjectDetails res = new SubjectDetails();
                        res.SubID = Convert.ToInt64(reader["ID"]);
                        res.Subject = Convert.ToString(reader["Subject"]);
                        listObj.Add(res);

                    }


                }
            }
            catch (Exception ex)
            {

                
            }
            return listObj;
        }


        public List<SchoolSyllabusTopic> GetTopicsForAssignment(long SubjectID)
        {
            List<SchoolSyllabusTopic> list = new List<SchoolSyllabusTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Topic_ID,Topic_Name from admin.Syllabus_Topic
                                    where Subject_ID_FK=@subid";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@subid", SubjectID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SchoolSyllabusTopic res = new SchoolSyllabusTopic();
                        res.Topic_ID = Convert.ToInt64(reader["Topic_ID"]);
                        res.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                        list.Add(res);

                    }
                }

            }
            catch (Exception ex)
            {

               // throw;
            }
            return list;
        }

        public List<ClassTeacherAssignmentDetails> Assignments(long TopicID)
        {
            List<ClassTeacherAssignmentDetails> list = new List<ClassTeacherAssignmentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ID,AssignmentName from 
                                    teacher.AssignmentDetails where TopicID_FK=@topic_id";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@topic_id", TopicID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherAssignmentDetails res = new ClassTeacherAssignmentDetails();
                        res.ID = Convert.ToInt64(reader["ID"]);
                        res.AssignmentName = Convert.ToString(reader["AssignmentName"]);
                        list.Add(res);

                    }
                }

            }
            catch (Exception ex)
            {

                // throw;
            }
            return list;
        }

        public string UploadAssignment(StudentUploadAssignment Info)
        {
            string str = "";
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"insert into studentinfo.StudentUploadAssignment
                                    (UserID,SubjectID,TopicID,AssignmentID,AssignmentFile,StudentComments,SchoolID,ClassID)
                                    values(@UID,@Subid,@tpcid,@assignid,@assignfile,@stucomm,@schlid,@classid)";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@UID", Info.UserID);
                    cmd.Parameters.AddWithValue("@Subid", Info.SubjectID);
                    cmd.Parameters.AddWithValue("@tpcid", Info.TopicID);
                    cmd.Parameters.AddWithValue("@assignid", Info.AssignmentID);
                    cmd.Parameters.AddWithValue("@assignfile", Info.AssignmentFile);
                    if (Info.StudentComments != null && Info.StudentComments != "")
                    {
                        cmd.Parameters.AddWithValue("@stucomm", Info.StudentComments);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@stucomm", "");
                    }
                    cmd.Parameters.AddWithValue("@schlid", Info.SchoolID);
                    cmd.Parameters.AddWithValue("@classid", Info.ClassID);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        str = "Success|Assignment Uploaded Successfuly";
                    }
                    else
                    {
                        str = "Failed| Upload Failed";
                    }

                }
            }
            catch (Exception ex)
            {

               // throw;
            }

            return str;
        }


        public List<AssignmentList> HomeworkList(long SubjectID)
        {
            List<AssignmentList> listobj = new List<AssignmentList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ass.ID,ass.AssignmentName,ass.EndDate,sub.Subject,ass.SubjectID_FK,tpc.Topic_Name,
                                    ass.TopicID_FK,ass.AssignmentFile
                                    from teacher.AssignmentDetails as ass 
                                    inner join admin.Syllabus_Subject as sub ON ass.SubjectID_FK=sub.ID
                                    inner join admin.Syllabus_Topic as tpc ON ass.TopicID_FK=tpc.Topic_ID
                                    where ass.SubjectID_FK=@subid";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    //cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    //cmd.Parameters.AddWithValue("@topicID", TopicID);
                    cmd.Parameters.AddWithValue("@subid", SubjectID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AssignmentList res = new AssignmentList();
                        res.ID= Convert.ToInt64(reader["ID"]);
                        res.AssignmentName = Convert.ToString(reader["AssignmentName"]);
                        res.CreatedDate = Convert.ToString(reader["EndDate"]); 
                        res.Subject= Convert.ToString(reader["Subject"]);
                        res.SubID= Convert.ToInt64(reader["SubjectID_FK"]);
                        res.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                        res.TopicID= Convert.ToInt64(reader["TopicID_FK"]);
                        res.AssignmentFile = Convert.ToString(reader["AssignmentFile"]);
                        listobj.Add(res);

                    }

                    con.Close();
                }

            }
            catch (Exception ex)
            {

              //  throw;
            }
            return listobj;
        }

        #endregion


        #region RATE LESSON

        public List<SubjectDetails> SubForTeacherRating(long ClassID, long  SchoolID)
        {
            List<SubjectDetails> listObj = new List<SubjectDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select stchr.SubjectID,sub.Subject
                                 from admin.AssignPrivilegeSubjectTeacher as stchr
                                 inner join admin.Syllabus_Subject as sub on stchr.SubjectID=sub.ID
                                    where ClassID=@classid and SchoolID=@schoolid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    //  cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    cmd.Parameters.AddWithValue("@classid", ClassID);
                    cmd.Parameters.AddWithValue("@schoolid", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SubjectDetails res = new SubjectDetails();
                        res.SubID = Convert.ToInt64(reader["SubjectID"]);
                        res.Subject = Convert.ToString(reader["Subject"]);
                        listObj.Add(res);

                    }


                }
            }
            catch (Exception ex)
            {


            }
            return listObj;

        }



        public long GetSubIdByTopicID(long TopicID)
        {
            long res = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string Query = @"select Subject_ID_FK from admin.Syllabus_Topic where Topic_ID=@topicid";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@topicid", TopicID);
                    //cmd.Parameters.AddWithValue("@ID", ClassID);
                   // cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        res = Convert.ToInt64(dt.Rows[0][0].ToString());
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return res;
        }



        public long GetTeacherIDBySubID(long SubID)
        {
            long res = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string Query = @"select TeacherID from admin.AssignPrivilegeSubjectTeacher where SubjectID=@subid";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@subid", SubID);
                      SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        res = Convert.ToInt64(dt.Rows[0][0].ToString());
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return res;
        }


        public string InsertRating(StudentRateLesson Info)
        {
            string str = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"insert into [studentinfo].[StudentRateLesson] 
                                (TopicID,SchoolID,RatingStar,StudentID,FeedBackComment,ClassID,SubjectID,TeacherID)
                                values(@topicid,@schoolid,@ratingstar,@studentid,@feedbackcomment,@classid,@subjectid,@Teacherid)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@topicid", Info.TopicID);
                    cmd.Parameters.AddWithValue("@schoolid", Info.SchoolID);
                    cmd.Parameters.AddWithValue("@ratingstar", Info.RatingStar);
                    cmd.Parameters.AddWithValue("@classid", Info.ClasssID);
                    cmd.Parameters.AddWithValue("@studentid", Info.StudentID);
                    cmd.Parameters.AddWithValue("@feedbackcomment", Info.FeedBackComment);
                    cmd.Parameters.AddWithValue("@subjectid", Info.SubjectID);
                    cmd.Parameters.AddWithValue("@Teacherid", Info.TeacherID);

                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        str = "Success|You have rated " + Info.RatingStar + " Stars";
                    }
                    else
                    {
                        str = "Failed|Rating Failed";
                    }
                    con.Close();
                }


            }
            catch (Exception ex)
            {


            }

            return str;
        }


        #endregion
    }


}