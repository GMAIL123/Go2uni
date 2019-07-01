using Go2uniApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static Go2uniApi.Models.User_Part;

namespace Go2uniApi.CodeFile
{
    public class Social_Student
    {
        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();

        public List<CollegeDay> GetAllCollegeEvent()
        {
            List<CollegeDay> Result = new List<CollegeDay>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllCollegeEvent", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CollegeDay temp = new CollegeDay();
                        temp.ID = Convert.ToInt16(reader["NxtCollege_ID"]);
                        temp.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                        temp.EventDate = Convert.ToDateTime(reader["NxtCollege_AddedDate"]);
                        temp.IsUpcoming = Convert.ToBoolean(reader["IsUpcoming"]);
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

        public List<CollegeDay> GetUpcomingCollegeEvent()
        {
            List<CollegeDay> Result = new List<CollegeDay>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetNextCollegeEvent", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CollegeDay temp = new CollegeDay();
                        temp.ID = Convert.ToInt16(reader["NxtCollege_ID"]);
                        temp.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                        temp.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);
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

        public List<Event> UpcomingEventForRightSide()
        {
            List<Event> Result = new List<Event>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_UpcomingEventForRightSide", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Event temp = new Event();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Title = Convert.ToString(reader["Title"]);
                        temp.Event_Date = Convert.ToString(reader["EventDate"]);
                        temp.IsActive = Convert.ToBoolean(reader["IsActive"]);
                        temp.EventData = Convert.ToString(reader["EventData"]);
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

        public CollegeDay GetCollegeEventDetailsByID(long id)
        {
            CollegeDay Result = new CollegeDay();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetCollegeEventDetails", con);
                    com.Parameters.AddWithValue("@id", id);
                    com.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.ID = Convert.ToInt16(reader["NxtCollege_ID"]);
                        Result.video = Convert.ToString(reader["NxtCollege_Video"]);
                        Result.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                        Result.EventDate = Convert.ToDateTime(reader["NxtCollege_AddedDate"]);
                        break;
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

        public string InsertEventComment(NextCollegeDayComment Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_InsertNextCollegeComment", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Comment", Info.Comment);
                    com.Parameters.AddWithValue("@StudentID", Info.StudentID);
                    com.Parameters.AddWithValue("@TeacherID", Info.TeacherID);
                    com.Parameters.AddWithValue("@EventID", Info.EventID);
                    com.Parameters.Add("@Commentid", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    con.Open();
                    if (com.ExecuteNonQuery()>0)
                    {
                        Result = "Success!" + com.Parameters["@Commentid"].Value.ToString();
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

        public List<NextCollegeDayComment> GetEventComments(long id)
        {
            List<NextCollegeDayComment> Result = new List<NextCollegeDayComment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllNextCollegeCommentByEventID", con);
                    com.Parameters.AddWithValue("@EventID", id);
                    com.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        NextCollegeDayComment temp = new NextCollegeDayComment();
                        temp.CommentID = Convert.ToInt16(reader["NxtCollege_Comments_ID"]);
                        temp.Comment = Convert.ToString(reader["NxtCollege_Comments_Text"]);
                        temp.CreatedDate = Convert.ToDateTime(reader["NxtCollege_Comments_InsertedDate"]);
                        temp.StudentName = Convert.ToString(reader["Student_Name"]);
                        temp.TeacherName = Convert.ToString(reader["TeacherName"]);
                        temp.IsStudent = Convert.ToBoolean(reader["IsStudent"]);
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

        public List<Chapter_Syllabus> GetChapter(long SyllabusID)
        {
            List<Chapter_Syllabus> Result = new List<Chapter_Syllabus>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[admin].[SP_GetChapterBySyllabus]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@syllabusID", SyllabusID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Chapter_Syllabus temp = new Chapter_Syllabus();
                        temp.Chapter = Convert.ToString(reader["Chapter"]);
                        temp.Topics = Convert.ToString(reader["Topics"]);

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

        #region Online community

        
        #endregion

        #region Edit Profile 

        public EditProfile GetDetailsOFStudent(long ID)
        {
            EditProfile Result = new EditProfile();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                  

                    string Query = @"select Usr.UID,Usr.Email,Usr.Password,Stu.UserID_FK,Stu.Stu_Name,Isnull(Stu.Stu_DOB,'') as Stu_DOB,IsNull(Stu.Stu_JoiningDt,'') as Stu_JoiningDt,IsNull(Stu.Stu_MObile_No,'')as Stu_MObile_No,IsNull(Stu.Stu_Profile_Image,'') as Stu_Profile_Image,IsNull(Stu.Stu_RegistrationNo,'') as Stu_RegistrationNo,IsNull(Stu.Stu_FutureAmbition,'') as Stu_FutureAmbition,IsNull(Stu.Stu_Favourite_Books,'') As Stu_Favourite_Books,IsNull(Stu.Level_ID,'') as Level_ID,IsNull(Stu.DivisionID,'') as DivisionID,Isnull(Stu.Stu_Gender_FK,'') as Stu_Gender_FK,Isnull(Stu.Stream_ID_FK,'') as Stream_ID_FK,IsNull(Stu.School_ID,'') as School_ID,IsNull(Stu.Class_ID,'') as Class_ID,Stu.CreatedBy_FK
                                           from admin.users as Usr
                                           INNER JOIN  studentinfo.Student_Details as Stu
                                           ON stu.UserID_FK=Usr.UID
                                           where UID=@ID";
                    SqlCommand com = new SqlCommand(Query ,con);
                    com.Parameters.AddWithValue("@UID", ID);
                
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.UID = Convert.ToInt32(reader["UID"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.Stu_DOB = Convert.ToDateTime(reader["Stu_DOB"]);
                        Result.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
                        Result.Stu_Favourite_Books = Convert.ToString(reader["Stu_Favourite_Books"]);
                        Result.Stu_FutureAmbition = Convert.ToString(reader["Stu_FutureAmbition"]);
                        Result.Stu_Gender_FK = Convert.ToInt16(reader["Stu_Gender_FK"]);
                        Result.DivisionID = Convert.ToInt16(reader["DivisionID"]);

                        Result.Class_ID = Convert.ToInt16(reader["Class_ID"]);
                
                         
                        
                        Result.Stu_RegistrationNo = Convert.ToString(reader["Registration_No"]);
                        break;
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

        
        public string UpdateStudentProfile(EditProfile Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    //@StudentID,@Name,@Email,@Password,@Dob,@GenderID,@FavoriteBooks,@FutureAmbition,@SchoolName,@ClassID,@StreamID,@RegistrationNo 
                    string Query = string.Empty;
                    if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image !="")
                    {
                        if (Info.DivisionID != 1)
                        {
                            Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Name,Stu_Profile_Image=@Imagepath,School_Name=@SchoolName,Class_ID_FK=@ClassID,DivisionID=@DivID,Stream_ID_FK=@StreamID,Stu_DOB=@Dob,Stu_Gender_FK=@GenderID,Stu_RegistrationNo=@RegistrationNo,Stu_Favourite_Books=@FavoriteBooks,Stu_FutureAmbition=@FutureAmbition, IsFirstStepComplete=1  where UserID_FK=@StudentID";

                        }
                        else
                        {
                            Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Name,Stu_Profile_Image=@Imagepath,School_Name=@SchoolName,Class_ID_FK=@ClassID,DivisionID=@DivID,Stu_DOB=@Dob,Stu_Gender_FK=@GenderID,Stu_RegistrationNo=@RegistrationNo,Stu_Favourite_Books=@FavoriteBooks,Stu_FutureAmbition=@FutureAmbition, IsFirstStepComplete=1  where UserID_FK=@StudentID";
                        }
                    }
                    else 
                    {
                        if (Info.DivisionID != 1)
                        {
                            Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Name,School_Name=@SchoolName,Class_ID_FK=@ClassID,DivisionID=@DivID,Stream_ID_FK=@StreamID,Stu_DOB=@Dob,Stu_Gender_FK=@GenderID,Stu_RegistrationNo=@RegistrationNo,Stu_Favourite_Books=@FavoriteBooks,Stu_FutureAmbition=@FutureAmbition, IsFirstStepComplete=1  where UserID_FK=@StudentID";

                        }
                        else
                        {
                            Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Name,School_Name=@SchoolName,Class_ID_FK=@ClassID,DivisionID=@DivID,Stu_DOB=@Dob,Stu_Gender_FK=@GenderID,Stu_RegistrationNo=@RegistrationNo,Stu_Favourite_Books=@FavoriteBooks,Stu_FutureAmbition=@FutureAmbition, IsFirstStepComplete=1  where UserID_FK=@StudentID";

                        }
                    }
                    SqlCommand com = new SqlCommand(Query, con);
                   // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@StudentID", Info.UserID_FK);
                    com.Parameters.AddWithValue("@Name", Info.Stu_Name);
                    if (Info.Stu_Profile_Image !=null && Info.Stu_Profile_Image != "")
                    {
                        com.Parameters.AddWithValue("@Imagepath", Info.Stu_Profile_Image);
                    }
                   
                    // com.Parameters.AddWithValue("@Email", Info.Stu_Email);
                    // com.Parameters.AddWithValue("@Password", Info.Stu_Password);
                    com.Parameters.AddWithValue("@SchoolName", Info.School_Name);
                    com.Parameters.AddWithValue("@ClassID", Info.Class_ID);
                    com.Parameters.AddWithValue("@DivID", Info.DivisionID);
                    if (Info.DivisionID !=1)
                    {
                        com.Parameters.AddWithValue("@StreamID", Info.Stream_ID_FK);
                    }
                    
                    com.Parameters.AddWithValue("@Dob", Info.Stu_DOB);
                    com.Parameters.AddWithValue("@GenderID", Info.Stu_Gender_FK);
                    com.Parameters.AddWithValue("@RegistrationNo", Info.Stu_RegistrationNo == null ? "" : Info.Stu_RegistrationNo);
                    com.Parameters.AddWithValue("@FavoriteBooks", Info.Stu_Favourite_Books == null ? "" : Info.Stu_Favourite_Books);
                    com.Parameters.AddWithValue("@FutureAmbition", Info.Stu_FutureAmbition == null ? "" : Info.Stu_FutureAmbition);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        //User_Backend obj = new User_Backend();
                        //Login login = new Login()
                        //{
                        //    Email = Info.Stu_Email,
                        //    Password = Info.Stu_Password,
                        //    IsStudentLogin = true
                        //};
                        // Result = "Success! Profile is updated! " + obj.Login(login);
                        Result = " Student Profile Updated ";
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
                Result = "Failed!";
            }
            return Result;
        }


        //new 10,12,18
        public List<Level> GetlevelListByDivID(int ID)
        {
            List<Level> Result = new List<Level>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" select ID,Level_Name,Division_ID  from admin.Level where Division_ID=@Division_ID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Division_ID", ID);

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
        //new 10,12,18
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

        //new 10,dec,2018
        public List<Level> GetLevel(int ID)
        {
            List<Level> Result = new List<Level>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"SELECT ID,Level_Name ,Division_ID FROM admin.Level where Division_ID=@Division_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Division_ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Level temp = new Level();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Level_Name = Convert.ToString(reader["Level_Name"]);
                        temp.Div_ID = Convert.ToInt16(reader["Division_ID"]);
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


        //nov,29,2018
        public List<ClassDetails> GetAllClassSt(int ID)
        {
            List<ClassDetails> Result = new List<ClassDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"SELECT ID,Class_Name ,Div_ID,Level_ID,School_ID FROM admin.Class where Level_ID=@Level_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Level_ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassDetails temp = new ClassDetails();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Class_Name = Convert.ToString(reader["Class_Name"]);
                        temp.Div_ID = Convert.ToInt16(reader["Div_ID"]);
                        temp.Level_ID = Convert.ToInt16(reader["level_ID"]);
                        temp.School_ID = Convert.ToInt16(reader["School_ID"]);
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


        public List<ClassInfo> GetAllClass()
        {
            List<ClassInfo> Result = new List<ClassInfo>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllClass", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassInfo temp = new ClassInfo();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Name = Convert.ToString(reader["Class"]);

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
        public List<ClassInfo> GetAllClassByDivision(long Div)
        {
            List<ClassInfo> Result = new List<ClassInfo>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllClassByDivID", con);
                    com.Parameters.AddWithValue("@DivID", Div);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassInfo temp = new ClassInfo();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Name = Convert.ToString(reader["Class"]);
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

        //public List<ClassSpecificaton> GetAllDivision()
        //{
        //    List<ClassSpecificaton> Result = new List<ClassSpecificaton>();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {
        //            SqlCommand com = new SqlCommand("SP_GetAllDivision", con);
        //            com.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            SqlDataReader reader = com.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                ClassSpecificaton temp = new ClassSpecificaton();
        //                temp.ID = Convert.ToInt16(reader["ID"]);
        //                temp.Name = Convert.ToString(reader["Name"]);
        //                Result.Add(temp);
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
        //    }
        //    return Result;
        //}

        //public List<Semester> GetAllSemester()
        //{
        //    List<Semester> Result = new List<Semester>();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {
        //            SqlCommand com = new SqlCommand("SP_GetAllSemester", con);
        //            com.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            SqlDataReader reader = com.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                Semester temp = new Semester();
        //                temp.ID = Convert.ToInt16(reader["ID"]);
        //                temp.SemesterName = Convert.ToString(reader["Semester"]);
        //                Result.Add(temp);
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
        //    }
        //    return Result;
        //}


        public List<ClassStream> GetStreamList()
        {
            List<ClassStream> Result = new List<ClassStream>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllStream", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassStream temp = new ClassStream();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Stream = Convert.ToString(reader["Stream"]);
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

        public List<SectionInfo> GetSectionByClass(long ID)
        {
            List<SectionInfo> Result = new List<SectionInfo>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllStream", con);
                    com.Parameters.AddWithValue("@ClassID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        SectionInfo temp = new SectionInfo();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Name = Convert.ToString(reader["Stream"]);

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

        public List<ClassInfo> GetClassBydivID(long ID)
        {
            List<ClassInfo> Result = new List<ClassInfo>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetClassByDivID", con);
                    com.Parameters.AddWithValue("@DivID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassInfo temp = new ClassInfo();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Name = Convert.ToString(reader["Class"]);
                        temp.divID = Convert.ToInt16(reader["Class_Sp_ID"]);
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
        #endregion

        #region Goal
        public List<Level> GetAllLevel()
        {
            List<Level> Result = new List<Level>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllLevel", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                       Level temp = new Level();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Level_Name = Convert.ToString(reader["Level_Name"]);

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
        public List<ClassDetails> ClassByLevelID(int ID)
        {
            List<ClassDetails> Result = new List<ClassDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_ClassByID", con);
                    com.Parameters.AddWithValue("@LevelID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassDetails temp = new ClassDetails();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Class_Name = Convert.ToString(reader["Class_Name"]);
                       
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

        public List<Subject_Syllabus> SubjectByClassID(int ID)
        {
            List<Subject_Syllabus> Result = new List<Subject_Syllabus>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetSub", con);
                    com.Parameters.AddWithValue("@ClassID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Subject_Syllabus temp = new Subject_Syllabus();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);

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

        public List<Syllabus_Content> SubTopicByTopic(int ID)
        {
            List<Syllabus_Content> Result = new List<Syllabus_Content>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_SubTopicByTopic", con);
                    com.Parameters.AddWithValue("@TopicID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Syllabus_Content temp = new Syllabus_Content();
                        temp.Content_ID = Convert.ToInt16(reader["Content_ID"]);
                        temp.Content = Convert.ToString(reader["Content"]);

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


        public List<ClassStream> GetStreamList(int ID)
        {
            List<ClassStream> Result = new List<ClassStream>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllStream", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ClassID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassStream temp = new ClassStream();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Stream = Convert.ToString(reader["Stream"]);
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
        public List<Semester> GetAllSemester()
        {
            List<Semester> Result = new List<Semester>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllSemester", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Semester temp = new Semester();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.SemesterName = Convert.ToString(reader["Semester"]);
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

        //get SUBJECT FOR SYLLABUS
        public List<Student_Syllabus> GetSubject(long Stu_ID)
        {
            List<Student_Syllabus> Result = new List<Student_Syllabus>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Sub.Subject,Sub.ID As Sub_ID,mc.Class,st.Stu_ID,st.DivisionID,st.Class_ID_FK
                                        from admin.Syllabus_Subject as Sub
                                        JOIN  admin.MasterClass as mc on Sub.ClassID_FK=mc.ID 
                                        join [studentinfo].[Student_Details] as st on st.Class_ID_FK=mc.ID
                                        where Stu_ID=@StudentID";
                    SqlCommand com = new SqlCommand(Query, con);
                   // SqlCommand com = new SqlCommand("[admin].[SP_GetSyllabusByClass]", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@StudentID", Stu_ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Student_Syllabus temp = new Student_Syllabus();
                       // Syllabus_Subject ob = new Syllabus_Subject();
                       
                        
                        
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.Sub_Id = Convert.ToInt16(reader["Sub_ID"]);
                        temp.Class = Convert.ToString(reader["Class"]);
                        temp.Stu_Id = Convert.ToInt32(reader["Stu_ID"]);
                        temp.Div_ID = Convert.ToInt16(reader["Stu_ID"]);
                        temp.Class_ID = Convert.ToInt16(reader["Class_ID_FK"]);

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
        public List<Syllabus_TopicDetails> TopicBySubID(int SubID)
        {
            List<Syllabus_TopicDetails> Result = new List<Syllabus_TopicDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_TopicBySub", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@SubID", SubID);
                   
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Syllabus_TopicDetails temp = new Syllabus_TopicDetails();
                        temp.Topic_ID = Convert.ToInt16(reader["Topic_ID"]);
                        temp.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                    

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
        public List<Subject_Syllabus> GetSubjectByClassIDAndStreamID(int ID, int StreamID)
        {
            List<Subject_Syllabus> Result = new List<Subject_Syllabus>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[admin].[SP_GetSyllabusByClassAndStream]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@classID", ID);
                    com.Parameters.AddWithValue("@streamID", StreamID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Subject_Syllabus temp = new Subject_Syllabus();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);

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
        public List<Syllabus_Subtopic> GetSubTopicByTopicID(int ID)
        {
            List<Syllabus_Subtopic> Result = new List<Syllabus_Subtopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[admin].[SP_GetSubTopicByTopic]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TopicID", ID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Syllabus_Subtopic temp = new Syllabus_Subtopic();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Subtopic = Convert.ToString(reader["Subtopic"]);

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

        public string InsertGoalDetails(long UserID, string StartDate, string TopicID, string EndDate)
        {
            string Result = string.Empty;
            string[] SData = StartDate.Split('|');
            string[] EData = EndDate.Split('|');
            string[] TData = TopicID.Split('|');

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    if ((SData.Length == EData.Length) && (SData.Length == TData.Length) && (EData.Length == TData.Length))
                    {
                        for (int i = 0; i < SData.Length - 1; i++)
                        {
                            string query = "insert into studentinfo.Goal(StartDate,EndDate,UserID_FK,ExamTopicID_FK)values(@Sdate,@Edate,@UID,@TID)";
                            SqlCommand com = new SqlCommand(query, con);
                            com.Parameters.AddWithValue("@Sdate", SData[i]);
                            com.Parameters.AddWithValue("@Edate", EData[i]);
                            com.Parameters.AddWithValue("@UID", UserID);
                            com.Parameters.AddWithValue("@TID", Convert.ToInt32(TData[i]));

                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Inserted";
                            }
                            else
                            {
                                Result = "Failed!Process Failed ";
                            }
                            con.Close();
                        }
                    }
                    else
                    {
                        Result = "Failed!Fill the field Properly";
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string InsertClassInfo(AddClassInfo Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @" insert into admin.Class 
                                            (Class_Name,Div_ID,Level_ID,CreatedBy_ID,School_ID )
                                            values(@Class_Name,@Div_ID,@Level_ID,@CreatedBy_ID,@School_ID) ";



                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Class_Name", Data.Class_Name);
                    com.Parameters.AddWithValue("@Div_ID", Data.Div_ID);
                    com.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                    com.Parameters.AddWithValue("@CreatedBy_ID", Data.CreatedBy_ID);
                    com.Parameters.AddWithValue("@School_ID", Data.School_ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Inserted";
                    }
                    else
                    {
                        Result = "Failed!Failed Add Class";
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


        public List<ExamTypeDetails> GetExamTypeForGoal()
        {
            List<ExamTypeDetails> Result = new List<ExamTypeDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllExamType", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamTypeDetails temp = new ExamTypeDetails();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.ExamType = Convert.ToString(reader["ExamType"]);

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

        public List<ExamSubject> SubjectByExamID(long ID)
        {
            List<ExamSubject> Result = new List<ExamSubject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_SubForExam", con);
                    com.Parameters.AddWithValue("@ExamID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamSubject temp = new ExamSubject();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);

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

        public List<ExamTopic> TopicByExamSub(long ID)
        {
            List<ExamTopic> Result = new List<ExamTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_TopicForExam", con);
                    com.Parameters.AddWithValue("@SubjectID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamTopic temp = new ExamTopic();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);

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


        public ViewGoal EditGoal(long ID)
        {
            ViewGoal Result = new ViewGoal();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select studentinfo.Goal.ID,studentinfo.Goal.StartDate,studentinfo.Goal.EndDate,studentinfo.Goal.Completed_Status,studentinfo.Goal.ExamTopicID_FK,
                                    studentinfo.ExamTopics.Topic,studentinfo.ExamTopics.SubjectID,studentinfo.Examsubjects.Subject,studentinfo.Examsubjects.ExamID,
                                    studentinfo.ExamType.ExamType
                                    from studentinfo.Goal inner join studentinfo.ExamTopics on studentinfo.Goal.ExamTopicID_FK=studentinfo.ExamTopics.ID
                                    inner join studentinfo.Examsubjects on studentinfo.ExamTopics.SubjectID=studentinfo.Examsubjects.ID
                                    inner join studentinfo.ExamType on studentinfo.Examsubjects.ExamID=studentinfo.ExamType.ID
                                    where studentinfo.Goal.ID=@ID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", ID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.ID = Convert.ToInt32(reader["ID"]);

                        //Result.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        //Result.EndDate = Convert.ToDateTime(reader["EndDate"]);

                        Result.TempStartDate = Convert.ToString(reader["StartDate"]);
                        Result.TempEndDate = Convert.ToString(reader["EndDate"]);
                        Result.Completed_Status = Convert.ToBoolean(reader["Completed_Status"]);

                        Result.ExamTopicID_FK = Convert.ToInt32(reader["ExamTopicID_FK"]);
                        Result.Topic = Convert.ToString(reader["Topic"]);
                        Result.SubjectID = Convert.ToInt32(reader["SubjectID"]);
                        Result.Subject = Convert.ToString(reader["Subject"]);
                        Result.ExamID = Convert.ToInt32(reader["ExamID"]);
                        Result.ExamType = Convert.ToString(reader["ExamType"]);


                        break;
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

        public string UpdateGoal(ViewGoal Data)
        {
            string Result = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @"update studentinfo.Goal set Completed_Status=@Demo,StartDate=@Sdate, EndDate=@Date where ID=@ID ";



                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    com.Parameters.AddWithValue("@Sdate", Data.TempStartDate);
                    com.Parameters.AddWithValue("@Date", Data.TempEndDate);
                    if (Data.Tempdata == "Completed")
                    {
                        com.Parameters.AddWithValue("@Demo", "true");
                    }
                    if (Data.Tempdata == "NotCompleted")
                    {
                        com.Parameters.AddWithValue("@Demo", "false");
                    }

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Updated";
                    }
                    else
                    {
                        Result = "Failed!Process Failed";
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

        public ViewGoal ViewGoal(long ID)
        {
            ViewGoal Result = new ViewGoal();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select studentinfo.Goal.ID,studentinfo.Goal.StartDate,studentinfo.Goal.EndDate,studentinfo.Goal.Completed_Status,studentinfo.Goal.ExamTopicID_FK,
                                    studentinfo.ExamTopics.Topic,studentinfo.ExamTopics.SubjectID,studentinfo.Examsubjects.Subject,studentinfo.Examsubjects.ExamID,
                                    studentinfo.ExamType.ExamType
                                    from studentinfo.Goal inner join studentinfo.ExamTopics on studentinfo.Goal.ExamTopicID_FK=studentinfo.ExamTopics.ID
                                    inner join studentinfo.Examsubjects on studentinfo.ExamTopics.SubjectID=studentinfo.Examsubjects.ID
                                    inner join studentinfo.ExamType on studentinfo.Examsubjects.ExamID=studentinfo.ExamType.ID
                                    where studentinfo.Goal.ID=@ID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", ID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.ID = Convert.ToInt32(reader["ID"]);

                        //Result.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        //Result.EndDate = Convert.ToDateTime(reader["EndDate"]);

                        Result.TempStartDate = Convert.ToString(reader["StartDate"]);
                        Result.TempEndDate = Convert.ToString(reader["EndDate"]);
                        Result.Completed_Status = Convert.ToBoolean(reader["Completed_Status"]);

                        Result.ExamTopicID_FK = Convert.ToInt32(reader["ExamTopicID_FK"]);
                        Result.Topic = Convert.ToString(reader["Topic"]);
                        Result.SubjectID = Convert.ToInt32(reader["SubjectID"]);
                        Result.Subject = Convert.ToString(reader["Subject"]);
                        Result.ExamID = Convert.ToInt32(reader["ExamID"]);
                        Result.ExamType = Convert.ToString(reader["ExamType"]);


                        break;
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

        public List<GoalDetails> GetGoalDetails(long SID)
        {
            List<GoalDetails> Result = new List<GoalDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_DetailsofGoal", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserID", SID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        GoalDetails temp = new GoalDetails();
                        temp.ID = Convert.ToInt32(reader["ID"]);                        

                        temp.TempStartDate = Convert.ToString(reader["StartDate"]);
                        temp.TempEndDate = Convert.ToString(reader["EndDate"]);

                        temp.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        temp.ExamTopicID_FK = Convert.ToInt32(reader["ExamTopicID_FK"]);

                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.SubjectID = Convert.ToInt32(reader["ExamTopicID_FK"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);

                        temp.ExamTypeID = Convert.ToInt32(reader["ExamID"]);
                        temp.ExamType = Convert.ToString(reader["ExamType"]);

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

        #endregion

        #region ReportCard

        public List<ReportCardSub> GetSubForReportCard(long SID)
        {
            List<ReportCardSub> Result = new List<ReportCardSub>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetSubjectForReportcard", con);

                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@StudentID", SID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ReportCardSub temp = new ReportCardSub();
                        temp.ClassID = Convert.ToInt16(reader["ClassID"]);
                        temp.StudentID = Convert.ToInt32(reader["student_ID"]);
                        temp.Class = Convert.ToString(reader["Class"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.SubjectID = Convert.ToInt16(reader["SubjectID"]);
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
        public List<Year> GetYear()
        {
            List<Year> Result = new List<Year>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetYearForReportCard", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Year temp = new Year();
                        temp.year = Convert.ToInt16(reader["Year"]);


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
        public List<ReportCardChapter> GetChapterForReportCard(int SID)
        {
            List<ReportCardChapter> Result = new List<ReportCardChapter>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_TopicForReportcard", con);

                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@SubID", SID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ReportCardChapter temp = new ReportCardChapter();
                        temp.SubjectID = Convert.ToInt16(reader["SubjectID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.Chapter = Convert.ToString(reader["chapter"]);
                        temp.ChapterID = Convert.ToInt16(reader["ChapterID"]);
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

        public List<Record> RecordforTopicWise(long UID, long TID)
        {
            List<Record> Result = new List<Record>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Score,ExamDate,Percentage,Feedback from admin.MocktestRecordTopicWise where USerID=@UID and TopicID=@TID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@TID", TID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Record temp = new Record();


                        temp.Score = Convert.ToInt16(reader["Score"]);
                        temp.TempExamDate = Convert.ToString(reader["ExamDate"]);
                        temp.Percentage = Convert.ToDouble(reader["Percentage"]);
                        temp.Feedback = Convert.ToString(reader["Feedback"]);


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

        public double HighestScoreForTopicWise(long TID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select MAX(Score) as HighestScore from admin.MocktestRecordTopicWise Where TopicID=@TID ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TID", TID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.HighestScore = Convert.ToDouble(reader["HighestScore"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.HighestScore;
        }

        public double LowestScoreForTopicWise(long TID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select MIN(Score) as LowestScore from admin.MocktestRecordTopicWise Where TopicID=@TID ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TID", TID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.LowestScore = Convert.ToDouble(reader["LowestScore"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.LowestScore;
        }

        public double AvarageScoreForTopicWise(long TID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select AVG(Score) as AvarageScore from admin.MocktestRecordTopicWise Where TopicID=@TID ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TID", TID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.AvarageScore = Convert.ToDouble(reader["AvarageScore"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.AvarageScore;
        }

        public List<Record> RecordforYearWise(long UID, string Year, long SubjectID)
        {
            List<Record> Result = new List<Record>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select Score,ExamDate,Percentage,Feedback from admin.MocktestRecordYearWise where USerID=@UID and Year=@year and  SubjectID=@SID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@Year", Year);
                    com.Parameters.AddWithValue("@SID", SubjectID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Record temp = new Record();


                        temp.Score = Convert.ToInt16(reader["Score"]);
                        temp.TempExamDate = Convert.ToString(reader["ExamDate"]);
                        temp.Percentage = Convert.ToDouble(reader["Percentage"]);
                        temp.Feedback = Convert.ToString(reader["Feedback"]);


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

        public double HighestScoreForYearWise(string Year, long SubjectID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select MAX(Score) as HighestScore from admin.MocktestRecordYearWise Where Year=@Year and SubjectID=@SID ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Year", Year);
                    com.Parameters.AddWithValue("@SID", SubjectID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.HighestScore = Convert.ToDouble(reader["HighestScore"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.HighestScore;
        }

        public double LowestScoreForYearWise(string Year, long SubjectID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select MIN(Score) as LowestScore from admin.MocktestRecordYearWise Where Year=@Year and SubjectID=@SID ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Year", Year);
                    com.Parameters.AddWithValue("@SID", SubjectID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.LowestScore = Convert.ToDouble(reader["LowestScore"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.LowestScore;
        }

        public double AvarageScoreForYearWise(string Year, long SubjectID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                     select AVG(Score) as AvarageScore from admin.MocktestRecordYearWise Where Year=@Year and SubjectID=@SID ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Year", Year);
                    com.Parameters.AddWithValue("@SID", SubjectID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.AvarageScore = Convert.ToDouble(reader["AvarageScore"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.AvarageScore;
        }

        public int GetNumberOfStudentYearWise()
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                  select count(distinct UserID) as NumberOfStudent from admin.MocktestRecordYearWise";

                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.NumberOfStudent = Convert.ToInt16(reader["NumberOfStudent"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.NumberOfStudent;
        }

        public List<int> GraphPointsYearWise(long UID, long SID, string Year)
        {
            List<int> Result = new List<int>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Score from admin.MocktestRecordyearWise where USerID=@UID and SubjectID=@SID and Year=@Year";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@SID", SID);
                    com.Parameters.AddWithValue("@Year", Year);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.Add(Convert.ToInt16(reader["Score"]));
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

        public int GetNumberOfAttemptedYearWise(long UID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                  select count(UserID) as NumberOfAttempted from admin.MocktestRecordYearWise where UserID=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.NumberOfAttempted = Convert.ToInt16(reader["NumberOfAttempted"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.NumberOfAttempted;
        }

        public int GetNumberOfStudentTopicWise()
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                  select count(distinct UserID) as NumberOfStudent from admin.MocktestRecordTopicWise";

                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.NumberOfStudent = Convert.ToInt16(reader["NumberOfStudent"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.NumberOfStudent;
        }

        public List<int> GraphPointsTopicWise(long UID, long TID)
        {
            List<int> Result = new List<int>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Score from admin.MocktestRecordTopicWise where USerID=@UID and TopicID=@TID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@TID", TID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.Add(Convert.ToInt16(reader["Score"]));
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

        public int GetNumberOfAttemptedTopicWise(long UID)
        {
            ReportCard_SocialStudent Result = new ReportCard_SocialStudent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                 select count(UserID) as NumberOfAttempted from admin.MocktestRecordTopicWise where UserID=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.NumberOfAttempted = Convert.ToInt16(reader["NumberOfAttempted"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.NumberOfAttempted;
        }

        public List<double> PointsForTimeGraphTopicWise(long UID, long TID)
        {
            List<double> Result = new List<double>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select TimeTakenInMinutes from admin.MocktestRecordTopicWise where USerID=@UID and TopicID=@TID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@TID", TID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.Add(Convert.ToDouble(reader["TimeTakenInMinutes"]));
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

        public List<double> PointsForTimeGraphYearWise(long UID, long SubjectID, string Year)
        {
            List<double> Result = new List<double>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select TimeTakenInMinutes from admin.mocktestRecordYearWise where USerID=@UID and SubjectID=@SID and Year=@Year";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@SID", SubjectID);
                    com.Parameters.AddWithValue("@Year", Year);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.Add(Convert.ToDouble(reader["TimeTakenInMinutes"]));
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

        #endregion

        #region MockTest

        public int GetCorrectForTopicwiseExam(string GUID, long ID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"SELECT COUNT(IsCorrect) as CorrectAnswer FROM admin.StudentAnswerByTopic WHERE IsCorrect=1 and UID=@UserID and GUID=@GUID
                                    ";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", ID);
                    cmd.Parameters.AddWithValue("@GUID", GUID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.CorrectAnswer = Convert.ToInt16(reader["CorrectAnswer"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.CorrectAnswer;
        }

        public int GetWrongForTopicwiseExam(string GUID, long ID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                    SELECT COUNT(IsCorrect) as WrongAnswer FROM admin.StudentAnswerByTopic WHERE IsCorrect=0 and UID=@UserID and GUID=@GUID
                                     ";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", ID);
                    cmd.Parameters.AddWithValue("@GUID", GUID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.WrongAnswer = Convert.ToInt16(reader["WrongAnswer"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.WrongAnswer;
        }

        public int GetTotalForTopicwiseExam(string GUID, long ID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                    select count (IsCorrect) as TotalAttem from admin.StudentAnswerByTopic as TotalAttemed where UID=@UserID and GUID=@GUID ";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", ID);
                    cmd.Parameters.AddWithValue("@GUID", GUID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.AttemedQuestion = Convert.ToInt16(reader["TotalAttem"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.AttemedQuestion;
        }

        public int FeedbackForYearwiseExamCorrectAnswer(string GUID, long UID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT COUNT(IsCorrect) as CorrectAnswer FROM admin.StudentAnswerByYear WHERE IsCorrect=1 and UID=@UserID and GUID=@GUID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID", UID);
                    com.Parameters.AddWithValue("@GUID", GUID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.CorrectAnswer = Convert.ToInt32(reader["CorrectAnswer"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.CorrectAnswer;
        }

        public int FeedbackForYearwiseExamWrongAnswer(string GUID, long UID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT COUNT(IsCorrect) as WrongAnswer FROM admin.StudentAnswerByYear WHERE IsCorrect=0 and UID=@UserID and GUID=@GUID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID", UID);
                    com.Parameters.AddWithValue("@GUID", GUID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.WrongAnswer = Convert.ToInt32(reader["WrongAnswer"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.WrongAnswer;
        }


        public int FeedbackForYearwiseExamAttemedQuestion(string GUID, long UID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT COUNT(IsCorrect) as AttemedQuestion FROM admin.StudentAnswerByYear where UID=@UserID and GUID=@GUID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID", UID);
                    com.Parameters.AddWithValue("@GUID", GUID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.AttemedQuestion = Convert.ToInt32(reader["AttemedQuestion"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.AttemedQuestion;
        }

        public List<DifficultyLevel> GetDiffiCultyLevelList()
        {
            List<DifficultyLevel> obj = new List<DifficultyLevel>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("admin.SP_GetDiffLvlByID", connt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<DifficultyLevel>>(str);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }
        
        public List<SelectDiffiCulty> GetDiffiCultyBYTopicId(int TpcId)
        {
            List<SelectDiffiCulty> obj = new List<SelectDiffiCulty>();
            SqlConnection connct = new SqlConnection(conn);
            try
            {
                SqlCommand com = new SqlCommand("[studentinfo].[SP_SelectDiffByTopicId]", connct);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@TopicId", TpcId);

                connct.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectDiffiCulty temp = new SelectDiffiCulty();
                    //  temp.Topic_Id = Convert.ToInt16(reader["Topic_Id"]);

                    temp.Topic_ID = Convert.ToUInt16(reader["TopicId"]);
                    temp.Difficulty = Convert.ToString(reader["Diff_Level"]);
                    // temp.Subject = Convert.ToString(reader["Subject"]);
                    obj.Add(temp);
                }
                connct.Close();

            }
            catch (Exception ex)
            {


            }
            return obj;
        }
        
        public List<SelectDiffiCulty> GetDiffiCultyByYearnm(int YearNm)
        {
            List<SelectDiffiCulty> obj = new List<SelectDiffiCulty>();
            SqlConnection connct = new SqlConnection(conn);
            try
            {
                SqlCommand com = new SqlCommand("[studentinfo].[SP_SelectDifficultyByYearID]", connct);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@YearNm", YearNm);

                connct.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectDiffiCulty temp = new SelectDiffiCulty();
                    //  temp.Topic_Id = Convert.ToInt16(reader["Topic_Id"]);

                    temp.Year = Convert.ToUInt16(reader["Year"]);
                    temp.Difficulty = Convert.ToString(reader["Diff_Level"]);
                    // temp.Subject = Convert.ToString(reader["Subject"]);
                    obj.Add(temp);
                }
                connct.Close();

            }
            catch (Exception ex)
            {


            }
            return obj;
        }
        
        public List<Syllabus_Subject> GetSubjectByStudentID(long Id)
        {
            List<Syllabus_Subject> obj = new List<Syllabus_Subject>();

            SqlConnection connt = new SqlConnection(conn);
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query= @"select Sub.Subject,Sub.ID As Sub_ID,mc.Class,st.Stu_ID,st.DivisionID,st.Class_ID_FK
                                    from admin.Syllabus_Subject as Sub
                                    JOIN  admin.MasterClass as mc on Sub.ClassID_FK=mc.ID 
                                    join [studentinfo].[Student_Details] as st on st.Class_ID_FK=mc.ID
                                    where st.Stu_ID=@StudentID";
                    // SqlCommand com = new SqlCommand("SP_GetSubjectForReportcard", con);
                    SqlCommand com = new SqlCommand(query, con);
                  //  com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@StudentID", Id);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Syllabus_Subject temp = new Syllabus_Subject();
                        temp.ClassID = Convert.ToInt16(reader["Class_ID_FK"]);
                        temp.student_ID = Convert.ToInt32(reader["Stu_ID"]);
                        temp.Class = Convert.ToString(reader["Class"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.SubjectID = Convert.ToInt16(reader["Sub_ID"]);

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
        
        public List<YearWiseQues> GetYearList()
        {
            List<YearWiseQues> obj = new List<YearWiseQues>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("[admin].[Sp_GetYearList]", connt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<YearWiseQues>>(str);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }

        public List<CommonClass> SelectYearBYSubId(int Id)
        {
            List<CommonClass> obj = new List<CommonClass>();

            SqlConnection connt = new SqlConnection(conn);
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[studentinfo].[SP_SelectYearBYSubId]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@SubId", Id);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CommonClass temp = new CommonClass();
                        temp.Label = Convert.ToString(reader["Year"]);
                        temp.Value = Convert.ToInt16(reader["Sub_Id_Fk"]);
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
        
        public List<GetTopic> GetTopicList()
        {
            List<GetTopic> obj = new List<GetTopic>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("[admin].[SP_GetTopicsList]", connt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<GetTopic>>(str);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }

        public List<CommonClass> SelectTopicBySubId(int SubId)
        {
            List<CommonClass> obj = new List<CommonClass>();

            SqlConnection connt = new SqlConnection(conn);
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[admin].[Sp_SelectTopicBySubId]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@SubId", SubId);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CommonClass temp = new CommonClass();
                        //  temp.Topic_Id = Convert.ToInt16(reader["Topic_Id"]);

                        temp.Value = Convert.ToUInt16(reader["Topic_Id"]);
                        temp.Label = Convert.ToString(reader["Chapter"]);
                        // temp.Subject = Convert.ToString(reader["Subject"]);
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
        
        public List<StartExam> GetTopicWiseQues(int SubId, int TopicId, int Diffilevel)
        {
            List<StartExam> Res = new List<StartExam>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("[studentinfo].[SP_GetQuesTopicWise]", con);
                    cmd.Parameters.AddWithValue("@SubId", SubId);
                    cmd.Parameters.AddWithValue("@TopicId", TopicId);
                    cmd.Parameters.AddWithValue("@Diff_Level", Diffilevel);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StartExam temp = new StartExam();
                        // temp.Sub_Id_Fk = Convert.ToInt16(reader["Sub_id"]);
                        temp.QuesID = Convert.ToInt16(reader["QuesID"]);

                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);
                        temp.Value = Convert.ToInt16(reader["Topic_Id"]);
                        temp.Diff_Level = Convert.ToInt16(reader["Diff_Level"]);
                        // temp.Subject = Convert.ToString(reader["Subject"]);




                        Res.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {


            }
            return Res;
        }
        
        public List<StartExam> GetYearWiseQues(int SubId, int Year, int Diffilevel)
        {
            List<StartExam> Res = new List<StartExam>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("[studentinfo].[SP_GetQuesYearWise]", con);
                    cmd.Parameters.AddWithValue("@SubId", SubId);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@Diff_Level", Diffilevel);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StartExam temp = new StartExam();

                        temp.QuesID = Convert.ToInt16(reader["QuesID"]);
                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);
                        temp.Value = Convert.ToInt16(reader["Year"]);
                        temp.Diff_Level = Convert.ToInt16(reader["Diff_Level"]);
                        Res.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);

            }

            return Res;
        }


        #region Ramashree 29/12/2018 Mock Test

        public List<ExamSubject> GetSubjectType(long ExamID)
        {
            List<ExamSubject> Result = new List<ExamSubject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select ID,Subject,ExamID from studentinfo.ExamSubjects where ExamID=@ExamID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ExamID", ExamID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamSubject temp = new ExamSubject();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.ExamID = Convert.ToInt32(reader["ExamID"]);
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
        
        public List<ExamTopic> GetTopicTypeBySubjectTypeForMockTest(long ExamID, long SubjectID)
        {
            List<ExamTopic> Result = new List<ExamTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select ID,Topic from studentinfo.ExamTopics where ExamID=@ExamID and SubjectID=@SubjectID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ExamID", ExamID);
                    cmd.Parameters.AddWithValue("@SubjectID", SubjectID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamTopic temp = new ExamTopic();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
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

        //public List<QuestionsByTopic> ViewTopicWiseForMockTest(long TopicID)
        //{
        //    List<QuestionsByTopic> Result = new List<QuestionsByTopic>();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {

        //            string Query = @"select tp.ID as QuestionID,tp.Question,
        //                            isnull(tp.Question_SerialNo,0)as Question_SerialNo,
        //                           isnull(tp.Option1,'')as Option1,isnull(tp.Option2,'')as Option2,
        //                           isnull(tp.Option3,'')as Option3,isnull(tp.Option4,'')as Option4,
        //                           isnull(tp.Option5,'')as Option5,
        //                           isnull(tp.Diagram,'') as Diagram,
        //                           isnull(qd.DescriptionText, '') as DescriptionText,isnull(tp.DescriptionID,'')as DescriptionID
        //                           from admin.QuestionsByTopic as tp
        //                           left outer join admin.QuestionsDescription as qd 

        //                           on tp.DescriptionID=qd.ID
        //                           where tp.TopicID=@TopicID order by tp.Question_SerialNo";
        //            SqlCommand com = new SqlCommand(Query, con);
        //            com.Parameters.AddWithValue("@TopicID", TopicID);
        //            con.Open();
        //            SqlDataReader reader = com.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                QuestionsByTopic temp = new QuestionsByTopic();
        //                temp.QuestionID = Convert.ToInt32(reader["QuestionID"]);
        //                temp.Question = Convert.ToString(reader["Question"]);

        //                temp.Option1 = Convert.ToString(reader["Option1"]);
        //                temp.Option2 = Convert.ToString(reader["Option2"]);
        //                temp.Option3 = Convert.ToString(reader["Option3"]);
        //                temp.Option4 = Convert.ToString(reader["Option4"]);
        //                temp.Option5 = Convert.ToString(reader["Option5"]);
        //                temp.Diagram = Convert.ToString(reader["Diagram"]);
        //                temp.SerialNo = Convert.ToInt32(reader["Question_SerialNo"]);
        //                temp.DescriptionText = Convert.ToString(reader["DescriptionText"]);
        //                Result.Add(temp);
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
        //    }
        //    return Result;
        //}

        //public List<QuestionsByYear> ViewYearWiseForMockTest(string Year, long SubjectID)
        //{
        //    List<QuestionsByYear> Result = new List<QuestionsByYear>();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {



        //            string Query = @"select yr.ID as QuestionID,isnull(yr.Question,'')as Question,
        //                            isnull(yr.Question_SerialNo,0)as Question_SerialNo,
        //                           isnull(yr.Option1,'')as Option1,isnull(yr.Option2,'')as Option2,
        //                           isnull(yr.Option3,'')as Option3,isnull(yr.Option4,'')as Option4,
        //                           isnull(yr.Option5,'') as Option5,
        //                           isnull(yr.Diagram,'') as Diagram,isnull(qd.DescriptionText,'')as DescriptionText
        //                           from admin.QuestionsByYear as yr 
        //                           left outer join admin.QuestionsDescription as qd
        //                           on yr.DescriptionID=qd.ID
        //                           where yr.Year=@Year and yr.SubjectID=@SID order by yr.Question_SerialNo";
        //            SqlCommand com = new SqlCommand(Query, con);
        //            com.Parameters.AddWithValue("@Year", Year);
        //            com.Parameters.AddWithValue("@SID", SubjectID);
        //            con.Open();
        //            SqlDataReader reader = com.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                QuestionsByYear temp = new QuestionsByYear();
        //                temp.Question = Convert.ToString(reader["Question"]);
        //                temp.DescriptionText = Convert.ToString(reader["DescriptionText"]);
        //                temp.QuestionID = Convert.ToInt32(reader["QuestionID"]);
        //                temp.Option1 = Convert.ToString(reader["Option1"]);
        //                temp.Option2 = Convert.ToString(reader["Option2"]);
        //                temp.Option3 = Convert.ToString(reader["Option3"]);
        //                temp.Option4 = Convert.ToString(reader["Option4"]);
        //                temp.Option5 = Convert.ToString(reader["Option5"]);
        //                temp.Diagram = Convert.ToString(reader["Diagram"]);
        //                temp.SerialNo = Convert.ToInt32(reader["Question_SerialNo"]);

        //                Result.Add(temp);
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
        //    }
        //    return Result;
        //}


        public List<QuestionsByYear> ViewYearWiseForMockTest(string Year, long SubjectID)
        {
            List<QuestionsByYear> Result = new List<QuestionsByYear>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select yr.ID as QuestionID,isnull(yr.Question,'')as Question,
                                    isnull(yr.Question_SerialNo,0)as Question_SerialNo,
                                    isnull(yr.Option1,'')as Option1,isnull(yr.Option2,'')as Option2,
                                    isnull(yr.Option3,'')as Option3,isnull(yr.Option4,'')as Option4,
                                    isnull(yr.Option5,'') as Option5,
                                    isnull(yr.Diagram,'') as Diagram,
                                    isnull(yr.QuestionTypeID,'') as QuestionTypeID,
                                    isnull(qd.DescriptionText,'')as DescriptionText
                                    from admin.QuestionsByYear as yr 
                                    left outer join admin.QuestionsDescription as qd
                                    on yr.DescriptionID=qd.ID
                                    where yr.Year=@Year and yr.SubjectID=@SID order by yr.Question_SerialNo";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Year", Year);
                    com.Parameters.AddWithValue("@SID", SubjectID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionsByYear temp = new QuestionsByYear();
                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.DescriptionText = Convert.ToString(reader["DescriptionText"]);
                        temp.QuestionID = Convert.ToInt32(reader["QuestionID"]);
                        temp.Option1 = Convert.ToString(reader["Option1"]);
                        temp.Option2 = Convert.ToString(reader["Option2"]);
                        temp.Option3 = Convert.ToString(reader["Option3"]);
                        temp.Option4 = Convert.ToString(reader["Option4"]);
                        temp.Option5 = Convert.ToString(reader["Option5"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);
                        temp.SerialNo = Convert.ToInt32(reader["Question_SerialNo"]);

                        temp.QuestionTypeID = Convert.ToInt32(reader["QuestionTypeID"]);
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

        public List<QuestionsByTopic> ViewTopicWiseForMockTest(long TopicID)
        {
            List<QuestionsByTopic> Result = new List<QuestionsByTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select tp.ID as QuestionID,tp.Question,
                                    isnull(tp.Question_SerialNo,0)as Question_SerialNo,
                                    isnull(tp.Option1,'')as Option1,isnull(tp.Option2,'')as Option2,
                                    isnull(tp.Option3,'')as Option3,isnull(tp.Option4,'')as Option4,
                                    isnull(tp.Option5,'')as Option5,
                                    isnull(tp.Diagram,'') as Diagram,
                                    isnull(tp.QuestionTypeID,'') as QuestionTypeID,
                                    isnull(qd.DescriptionText, '') as DescriptionText,isnull(tp.DescriptionID,'')as DescriptionID
                                    from admin.QuestionsByTopic as tp
                                    left outer join admin.QuestionsDescription as qd 
                                    on tp.DescriptionID=qd.ID
                                    where tp.TopicID=@TopicID order by tp.Question_SerialNo";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TopicID", TopicID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionsByTopic temp = new QuestionsByTopic();
                        temp.QuestionID = Convert.ToInt32(reader["QuestionID"]);
                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.DescriptionText = Convert.ToString(reader["DescriptionText"]);
                        temp.Option1 = Convert.ToString(reader["Option1"]);
                        temp.Option2 = Convert.ToString(reader["Option2"]);
                        temp.Option3 = Convert.ToString(reader["Option3"]);
                        temp.Option4 = Convert.ToString(reader["Option4"]);
                        temp.Option5 = Convert.ToString(reader["Option5"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);
                        temp.SerialNo = Convert.ToInt32(reader["Question_SerialNo"]);
                        temp.DescriptionText = Convert.ToString(reader["DescriptionText"]);

                        temp.QuestionTypeID = Convert.ToInt32(reader["QuestionTypeID"]);
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

        public List<QuestionsByYear> GetYearBySubjectTypeForMockTest()
        {
            List<QuestionsByYear> Result = new List<QuestionsByYear>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select Distinct Year from admin.QuestionsByYear";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionsByYear temp = new QuestionsByYear();
                        temp.Year = Convert.ToString(reader["Year"]);
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

        public string InsertAnswerForTopicQues(StudentAnswerByTopic Info, string demo)
        {
            string Result = string.Empty;
            try
            {
                QuestionsByTopic quesyear = new QuestionsByTopic();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    if (!ChkTopicAnswerExits(Info.QuestionID, Info.UID,Info.GUID))
                    {
                        string Query = @"insert into [admin].[StudentAnswerByTopic] (Answer,UID,QuestionID,IsCorrect,GUID) values(@ans,@StuId,@quesId,@IsAns,@GUID)";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ans", Info.Answer);
                        com.Parameters.AddWithValue("@StuId", Info.UID);
                        com.Parameters.AddWithValue("@quesId", Info.QuestionID);
                        com.Parameters.AddWithValue("@GUID", Info.GUID);
                        if (Info.Answer == demo)
                        {
                            com.Parameters.AddWithValue("@IsAns", 1);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@IsAns", 0);
                        }
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success! Inserted";
                        }
                        else
                        {
                            Result = "Failed!";
                        }
                        con.Close();
                    }
                    else
                    {
                        string Query = @"update [admin].[StudentAnswerByTopic] set Answer=@ans, IsCorrect=@IsAns,GUID=@GUID  where UID=@UID and QuestionID=@QuestionID ";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ans", Info.Answer);
                        com.Parameters.AddWithValue("@UID ", Info.UID);
                        com.Parameters.AddWithValue("@QuestionID", Info.QuestionID);
                        com.Parameters.AddWithValue("@GUID", Info.GUID);
                        if (Info.Answer == demo)
                        {
                            com.Parameters.AddWithValue("@IsAns", 1);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@IsAns", 0);
                        }
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success! Updated";
                        }
                        else
                        {
                            Result = "Failed!";
                        }
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Result;
        }

        public bool ChkTopicAnswerExits(long QuestionID, long UID,string GUID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select Answer,UID,QuestionID,CreatedDate from [admin].[StudentAnswerByTopic] where QuestionID=@QuestionID and UID=@UID and GUID=@GUID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                cmd.Parameters.AddWithValue("@UID", UID);
                cmd.Parameters.AddWithValue("@GUID", GUID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public string InsertAnswerForYearQues(StudentAnswerByYear Info, string demo)
        {
            string Result = string.Empty;
            try
            {
                QuestionsByYear quesyear = new QuestionsByYear();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    if (!ChkYearAnswerExits(Info.QuestionID, Info.UID,Info.GUID))
                    {
                        string Query = @"insert into [admin].[StudentAnswerByYear] (Answer,UID,QuestionID,IsCorrect,GUID) values(@ans,@StuId,@quesId,@IsAns,@GUID)";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ans", Info.Answer);
                        com.Parameters.AddWithValue("@StuId", Info.UID);
                        com.Parameters.AddWithValue("@quesId", Info.QuestionID);
                        com.Parameters.AddWithValue("@GUID", Info.GUID);
                        if (Info.Answer == demo)
                        {
                            com.Parameters.AddWithValue("@IsAns", 1);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@IsAns", 0);
                        }
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
                        string Query = @"update [admin].[StudentAnswerByYear] set Answer=@ans, IsCorrect=@IsAns,GUID=@GUID  where UID=@UID and QuestionID=@QuestionID ";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ans", Info.Answer);
                        com.Parameters.AddWithValue("@UID ", Info.UID);
                        com.Parameters.AddWithValue("@QuestionID", Info.QuestionID);
                        com.Parameters.AddWithValue("@GUID", Info.GUID);
                        if (Info.Answer == demo)
                        {
                            com.Parameters.AddWithValue("@IsAns", 1);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@IsAns", 0);
                        }
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success! Updated";
                        }
                        else
                        {
                            Result = "Failed! ";
                        }
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Result;
        }

        public bool ChkYearAnswerExits(long QuestionID, long UID,string GUID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select Answer,UID,QuestionID,CreatedDate from [admin].[StudentAnswerByYear] where QuestionID=@QuestionID and UID=@UID and GUID=@GUID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                cmd.Parameters.AddWithValue("@UID", UID);
                cmd.Parameters.AddWithValue("@GUID", GUID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public string GetCorrectAnswerTopic(long ID)
        {
            StudentAnswerByTopic Result = new StudentAnswerByTopic();
            try
            {
                string Ret = string.Empty;
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select CorrectOption from admin.QuestionsByTopic where ID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.CorrectAnswer = Convert.ToString(reader["CorrectOption"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.CorrectAnswer;
        }

        public string GetCorrectAnswerYear(long ID)
        {
            StudentAnswerByYear Result = new StudentAnswerByYear();
            try
            {
                string Ret = string.Empty;
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select CorrectOption from admin.QuestionsByYear where ID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.CorrectAnswer = Convert.ToString(reader["CorrectOption"]);
                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.CorrectAnswer;
        }

        #endregion



        public string InsertMocktestRecordTopicWise(string GUID, long UserID, long TopicID, long ExamID, long SubjectID, int CorrectAns, int total, double Per, double Min)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @" insert into admin.MocktestRecordTopicWise 
                                            (UserID,ExamID,SubjectID,TopicID,Score,GUID,TotalQuestion,Percentage,Feedback,TimeTakenInMinutes)
                                            values(@UID,@EID,@SID,@TID,@Score,@GUID,@Total,@Per,@FDBK,@Time)";



                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UID", UserID);
                    com.Parameters.AddWithValue("@EID", ExamID);
                    com.Parameters.AddWithValue("@SID", SubjectID);
                    com.Parameters.AddWithValue("@TID", TopicID);
                    com.Parameters.AddWithValue("@Score", CorrectAns);
                    com.Parameters.AddWithValue("@GUID", GUID);
                    com.Parameters.AddWithValue("@Total", total);
                    com.Parameters.AddWithValue("@Per", Per);
                    com.Parameters.AddWithValue("@Time", Min);
                    if (Per >= 0 && Per <= 20)
                    {
                        com.Parameters.AddWithValue("@FDBK", "BAD");
                    }
                    if (Per > 20 && Per <= 40)
                    {
                        com.Parameters.AddWithValue("@FDBK", "NOT BAD");
                    }
                    if (Per > 40 && Per <= 60)
                    {
                        com.Parameters.AddWithValue("@FDBK", "AVERAGE");
                    }
                    if (Per > 60 && Per <= 70)
                    {
                        com.Parameters.AddWithValue("@FDBK", "GOOD");
                    }
                    if (Per > 70 && Per <= 90)
                    {
                        com.Parameters.AddWithValue("@FDBK", "EXCELENT");
                    }
                    if (Per > 90 && Per <= 100)
                    {
                        com.Parameters.AddWithValue("@FDBK", "OUT STANDING");
                    }
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Inserted";
                    }
                    else
                    {
                        Result = "Failed!Failed Data Record";
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

        public string InsertMocktestRecordYearWise(string GUID, long UserID, string Year, long ExamID, long SubjectID, int CorrectAns, int total, double Per, double Min)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @" insert into admin.MocktestRecordYearWise 
                                            (UserID,ExamID,SubjectID,Year,Score,GUID,TotalQuestion,Percentage,Feedback,TimeTakenInMinutes)
                                            values(@UID,@EID,@SID,@Year,@Score,@GUID,@Total,@Per,@FDBK,@Time)";



                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UID", UserID);
                    com.Parameters.AddWithValue("@EID", ExamID);
                    com.Parameters.AddWithValue("@SID", SubjectID);
                    com.Parameters.AddWithValue("@Year", Year);
                    com.Parameters.AddWithValue("@Score", CorrectAns);
                    com.Parameters.AddWithValue("@GUID", GUID);
                    com.Parameters.AddWithValue("@Total", total);
                    com.Parameters.AddWithValue("@Per", Per);
                    com.Parameters.AddWithValue("@Time", Min);
                    if (Per >= 0 && Per <= 20)
                    {
                        com.Parameters.AddWithValue("@FDBK", "BAD");
                    }
                    if (Per > 20 && Per <= 40)
                    {
                        com.Parameters.AddWithValue("@FDBK", "NOT BAD");
                    }
                    if (Per > 40 && Per <= 60)
                    {
                        com.Parameters.AddWithValue("@FDBK", "AVERAGE");
                    }
                    if (Per > 60 && Per <= 70)
                    {
                        com.Parameters.AddWithValue("@FDBK", "GOOD");
                    }
                    if (Per > 70 && Per <= 90)
                    {
                        com.Parameters.AddWithValue("@FDBK", "EXCELENT");
                    }
                    if (Per > 90 && Per <= 100)
                    {
                        com.Parameters.AddWithValue("@FDBK", "OUT STANDING");
                    }

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Inserted";
                    }
                    else
                    {
                        Result = "Failed!Failed Data Record";
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

        public int GetAllQuestionForTopicwiseExam(long ID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                    select count (Question) as AllQuestion from admin.QuestionsByTopic  where TopicID=@ID ";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", ID);


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.TotalQuestionForTopic = Convert.ToInt16(reader["AllQuestion"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.TotalQuestionForTopic;
        }

        public string ExamNameForReport(long EID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                   Select ExamType from studentinfo.ExamType where ID=@EID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EID", EID);



                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.ExamName = Convert.ToString(reader["ExamType"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.ExamName;
        }

        public string SubjectnameForReport(long SID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                   select Subject from studentinfo.Examsubjects where ID=@SID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SID", SID);



                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.SubjectName = Convert.ToString(reader["Subject"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.SubjectName;
        }

        public string TopicForReport(long TID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                   select Topic from studentinfo.ExamTopics where ID=@TID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@TID", TID);



                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.Topic = Convert.ToString(reader["Topic"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.Topic;
        }

        public int GetAllQuestionForYearwiseExam(string year, long SID)
        {
            Feedback Result = new Feedback();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
                                   select count (Question) as AllQuestion from admin.QuestionsByYear  where Year=@Year and SubjectID=@SID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SID", SID);
                    cmd.Parameters.AddWithValue("@Year", year);


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.TotalQuestionForYear = Convert.ToInt16(reader["AllQuestion"]);

                        break;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.TotalQuestionForYear;
        }


        #endregion

        #region STUDY GROUP SM, 19,dec

        public Topics RefreshSgTopicByUserID(long TopicID, long UID)
        {
            Topics res = new Topics();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,Topic from studentinfo.Study_Group_Topic 
                                   where ID=@topicid and UID_Stu=@uid";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@topicid", TopicID);
                    com.Parameters.AddWithValue("@uid", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        res.ID = Convert.ToInt64(reader["ID"]);
                        res.Topic = Convert.ToString(reader["Topic"]);

                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }


        public InitialInfo InitialInfo(long ID)
        {
            InitialInfo temp = new InitialInfo();
            try
            {
                using (SqlConnection con = new SqlConnection(this.conn))
                {
                    string Query = @"SELECT DISTINCT Usr.UID,st.Stu_Name,Isnull(st.Stu_Profile_Image,'') as Stu_Profile_Image
                                    from admin.users as Usr INNER JOIN   [studentinfo].[Student_Details] as st
                                    ON st.UserID_FK=Usr.UID
                                    WHERE Usr.UID=@ID AND st.STATUS=1 and st.LoginTpe_FK=8";
                    // SqlCommand com = new SqlCommand("SP_InitialInfo", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        temp.Name = Convert.ToString(reader["Stu_Name"]);
                        temp.ProfilePicture = Convert.ToString(reader["Stu_Profile_Image"]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StudentID"></param>
        /// <returns></returns>
        /// 


        // edited 3/june/2019 
        public List<StudyGroupName> GetGroupNameByID(long UID)
        {
            List<StudyGroupName> Result = new List<StudyGroupName>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select usr.UID,sg.Group_ID,sg.Group_Name,isnull(sg.Group_About,'') as Group_About,sg.Group_CreatedDate,sgm.IsAdmin,sg.CreatedByUsrName
                                    from admin.users as usr 
                                    Inner JOIN [studentinfo].[Study_Group_Member] as sgm ON sgm.UID_Stu=usr.UID
                                    LEFT JOIN [studentinfo].[Study_Group] AS sg ON sgm.Group_FK_ID=sg.Group_ID
                                    WHERE sgm.UID_Stu=@UserID_Stu AND sgm.IsPending='false' AND sg.Group_status=1 
                                    ORDER BY sg.Group_CreatedDate DESC";
                    SqlCommand com = new SqlCommand(query, con);
                    // SqlCommand com = new SqlCommand("SP_GetGroupNameByID", con); // Changes made to sp//
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserID_Stu", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        StudyGroupName temp = new StudyGroupName();
                        temp.GroupName = Convert.ToString(reader["Group_Name"]);
                        temp.GroupID = Convert.ToInt64(reader["Group_ID"]);
                        temp.AboutGroup = Convert.ToString(reader["Group_About"]);
                        temp.tempCreatedDate = Convert.ToString(reader["Group_CreatedDate"]);
                        temp.CreatedDate = Convert.ToDateTime(temp.tempCreatedDate);
                        temp.CreatedDate = Convert.ToDateTime(reader["Group_CreatedDate"]);
                        temp.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                        temp.CreatedBy = Convert.ToString(reader["CreatedByUsrName"]);
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

        public List<Comment> GetCommentByTopicID(long TopicID, long GroupID)
        {
            List<Comment> Result = new List<Comment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {


                    string Query = @"select sgc.Group_Comment_Text,sgc.Group_Comment_CommentedDate,sgc.Group_Fk_ID,sgc.TopicID_FK,sgc.IsReported,
                                   sgc.[UID(St)],sgc.Group_Comment_ID,sd.Stu_Name,sd.Stu_Profile_Image
                                   from studentinfo.Study_Group_Comment as sgc 
                                   Inner JOIN studentinfo.Student_Details as sd ON sgc.[UID(St)]=sd.UserID_FK
                                   where sgc.TopicID_FK=@TopicID and sgc.Group_Fk_ID=@GroupID and sgc.IsActive=1 ORDER BY sgc.Group_Comment_CommentedDate DESC";


                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@TopicID", TopicID);
                    com.Parameters.AddWithValue("@GroupID", GroupID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Comment temp = new Comment();

                        temp.UID = Convert.ToInt64(reader["UID(St)"]);
                        temp.StudentComment = Convert.ToString(reader["Group_Comment_Text"]);
                        temp.Group_Comment_ID = Convert.ToInt64(reader["Group_Comment_ID"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
                        temp.CommentDate = Convert.ToDateTime(reader["Group_Comment_CommentedDate"]);
                        temp.TopicID = Convert.ToInt64(reader["TopicID_FK"]);
                        temp.GroupID = Convert.ToInt64(reader["Group_Fk_ID"]);
                        temp.IsReported = Convert.ToBoolean(reader["IsReported"]);
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

        public List<Topics> GetTopicByGroupID(long GroupID, long UID)
        {
            List<Topics> Result = new List<Topics>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Usr.UID,sgt.ID,sgt.Topic,sgt.GroupID_FK,sgt.CreatedDate,sa.Stu_Name 
                                        from admin.users as Usr
                                        Left JOIN  [studentinfo].[Student_Details] AS sa ON sa.UserID_FK=Usr.UID
                                        left jOIN [studentinfo].[Study_Group_Topic]  AS sgt ON sgt.UID_Stu=Usr.UID
                                        WHERE sgt.GroupID_FK=@GroupID AND sgt.Status=1 
                                        ORDER BY sgt.CreatedDate DESC";
                    //  SqlCommand com = new SqlCommand("SP_GetStudyGroupTopic", con); // Changes made at columns Of SP 5th nov//
                    // com.CommandType = CommandType.StoredProcedure;
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@GroupID", GroupID);
                    //com.Parameters.AddWithValue("@StudentID", StudentID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Topics temp = new Topics();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.UID = Convert.ToInt64(reader["UID"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        temp.GroupID = Convert.ToInt64(reader["GroupID_FK"]);
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
        // Sp changed to Query 17,dec,18
        public List<StudyPlanGoalsByGroupID> GetStudyPlanGoalsByGroupID(long GroupID)
        {
            List<StudyPlanGoalsByGroupID> Result = new List<StudyPlanGoalsByGroupID>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT stp.ID, stp.Subject, stp.Chapter, stp.UID_Stu, stp.GoalID_FK, stp.GroupID_FK, stpg.Goal
                                        FROM [studentinfo].[SetStudyPlan] AS stp
                                        LEFT JOIN [studentinfo].[SetStudyPlanGoals] AS stpg ON stpg.ID=stp.GoalID_FK
                                        WHERE stp.GroupID_FK=@GroupID AND stp.Status=1";
                    //  SqlCommand com = new SqlCommand("[studentinfo].[GetStudyPlane]", con); // changes made to sp//
                    SqlCommand com = new SqlCommand(Query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GroupID", GroupID);
                    //com.Parameters.AddWithValue("@StudentID", StudentID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        StudyPlanGoalsByGroupID temp = new StudyPlanGoalsByGroupID();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Goal = Convert.ToString(reader["Goal"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.Chapter = Convert.ToString(reader["Chapter"]);
                        temp.UID_Stu = Convert.ToInt32(reader["UID_Stu"]);
                        temp.GoalID = Convert.ToInt16(reader["GoalID_FK"]);
                        temp.GroupID = Convert.ToInt32(reader["GroupID_FK"]);
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

        // Sp changed to Query 17,dec,18
        public Topics GetTopicByID(long TopicID)
        {
            Topics Result = new Topics();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT TOP 1  sgt.ID,sgt.Topic,sgt.GroupID_FK,sgt.CreatedDate,sa.Stu_Name,sg.Group_name
                                    from admin.users as Usr
                                    INNER JOIN [studentinfo].[Study_Group_Topic] AS sgt ON sgt.UID_Stu=Usr.UID
                                    LEFT JOIN [studentinfo].[Student_Details] AS sa ON sa.UserID_FK=Usr.UID
                                    inner join studentinfo.Study_Group as sg on sg.Group_ID=sgt.GroupID_FK
                                    WHERE sgt.ID=@TopicID";
                    //SqlCommand com = new SqlCommand("SP_GetTopicByID", con); // Altered column of sp//
                    SqlCommand com = new SqlCommand(Query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TopicID", TopicID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.ID = Convert.ToInt64(reader["ID"]);
                        Result.Topic = Convert.ToString(reader["Topic"]);
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        Result.GroupID = Convert.ToInt64(reader["GroupID_FK"]);
                        Result.GroupName = Convert.ToString(reader["Group_name"]);

                        break;
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

        // altered SP 17,dec,2018
        public string InsertTopicByGroupID(Topics Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_InsertStudyGroupTopic", con); // Changes to sp //
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Topic", Info.Topic);
                    com.Parameters.AddWithValue("@GroupID", Info.GroupID);
                    com.Parameters.AddWithValue("@UID_Stu", Info.UID);
                    com.Parameters.Add("@ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        string id = com.Parameters["@ID"].Value.ToString();
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

        //// sp changed 17,dec,18, stuid changed to UID
        //public string InsertCommentByTopicID(Comment Info)
        //{
        //    string Result = string.Empty;
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {
        //            SqlCommand com = new SqlCommand("SP_InsertGroupComment", con); // changes made to stuid//
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@comment", Info.StudentComment);
        //            com.Parameters.AddWithValue("@topicid", Info.TopicID);
        //            com.Parameters.AddWithValue("@UID", Info.UID);
        //            con.Open();
        //            if (com.ExecuteNonQuery() > 0)
        //            {
        //                Result = "Success! Inserted";
        //            }
        //            else
        //            {
        //                Result = "Failed! ";
        //            }
        //            con.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Result;
        //}
        public string InsertCommentByTopicID(Comment Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"INSERT INTO studentinfo.Study_Group_Comment(Group_Comment_Text,Group_Comment_CommentedDate,Group_Fk_ID,TopicID_FK,[UID(St)]) 
                                        VALUES(@comment,getdate(),@GroupID,@TopicID,@UID)";
                    SqlCommand com = new SqlCommand(query, con);

                    com.Parameters.AddWithValue("@comment", Info.StudentComment);
                    com.Parameters.AddWithValue("@GroupID", Info.GroupID);
                    com.Parameters.AddWithValue("@TopicID", Info.TopicID);
                    com.Parameters.AddWithValue("@UID", Info.UID);
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
        //exit group SM//
        public string ExitFromGroupById(long UID, long GroupID)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"delete from studentinfo.Study_Group_Member
                                    where UID_Stu=@UID and Group_Fk_ID=@GroupID
                                    update studentinfo.Study_Group_comment set IsActive=0
                                    where [UID(St)]=@UID and Group_Fk_ID=@GroupID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    cmd.Parameters.AddWithValue("@GroupID", GroupID);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success! deleted";
                    }
                    else
                    {
                        res = "Failed!";

                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }
        // update comment SM//
        public string UpdateStudygrpComment(long UID, long Group_Comment_ID, string Comment)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"Update studentinfo.Study_group_comment  set Group_Comment_Text=@Comment
                                     where Group_Comment_ID=@GroupCommID and [UID(St)]=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    cmd.Parameters.AddWithValue("@GroupCommID", Group_Comment_ID);
                    cmd.Parameters.AddWithValue("@Comment", Comment);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success! status updated";
                    }
                    else
                    {
                        res = "Failed!";

                    }
                }
            }
            catch (Exception ex)
            {


            }
            return res;
        }

        //Edit comment SM//
        public Comment EditStudyGroupComment(long GroupCommID, long UID)
        {
            Comment Result = new Comment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select [UID(St)],Group_Comment_ID,Group_Comment_Text from studentinfo.Study_group_comment 
                                     where Group_Comment_ID=@GroupCommID and [UID(St)]=@UID";


                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@GroupCommID", GroupCommID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        //Comment temp = new Comment();
                        // temp.GroupID = Convert.ToInt32(reader["Group_ID"]);
                        Result.Group_Comment_ID = Convert.ToInt32(reader["Group_Comment_ID"]);
                        Result.StudentComment = Convert.ToString(reader["Group_Comment_Text"]);
                        Result.UID = Convert.ToInt32(reader["UID(St)"]);
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

        //////////////////////////////////////////////////////////////////////

        //delete study grp comment sm//
        public string DeleteStudyGroupComment(long UID, long GroupCommID)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"Delete from studentinfo.Study_group_comment 
                                    where Group_Comment_ID=@GroupCommID and [UID(St)]=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    cmd.Parameters.AddWithValue("@GroupCommID", GroupCommID);
                    //cmd.Parameters.AddWithValue("@Comment",)
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success! deleted";
                    }
                    else
                    {
                        res = "Failed!";

                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }
        // sp changed to query 17,dec,18,
        public List<StudentInfo> GetMemberListByGroupID(long GroupID)
        {
            List<StudentInfo> Result = new List<StudentInfo>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Select distinct sgm.UID_Stu,sa.Stu_Name,sa.Stu_Profile_Image,sg.Group_Name,sgm.IsAdmin
                                        from admin.users as usr 
                                        INNER JOIN [studentinfo].[Student_Details] AS sa ON sa.UserID_FK=usr.UID
                                        INNER JOIN [studentinfo].[Study_Group_Member] AS sgm ON sgm.UID_Stu=usr.UID
                                        LEFT JOIN [studentinfo].[Study_Group] AS sg ON sg.Group_ID=sgm.Group_Fk_ID
                                        WHERE sgm.Group_Fk_ID=@GroupID AND sgm.IsPending=0
                                        ORDER BY sa.Stu_Name ASC";
                    // SqlCommand com = new SqlCommand("SP_GetMemberListByGroupID", con); // sp altred with columns//
                    // com.CommandType = CommandType.StoredProcedure;
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@GroupID", GroupID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        StudentInfo temp = new StudentInfo();

                        temp.UID = Convert.ToInt32(reader["UID_Stu"]);
                        temp.Name = Convert.ToString(reader["Stu_Name"]);
                        temp.GroupName = Convert.ToString(reader["Group_Name"]);
                        temp.ProfilePicture = Convert.ToString(reader["Stu_Profile_Image"]);
                        temp.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
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

        // Study group changes to sp// changes to sp 17,dec,18
        public string AcceptRequest(long GroupID, long UID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[dbo].[SP_AcceptPendingRequest]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GroupID", GroupID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!";
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

        // changes to sp// 17,dec,2018
        public string DeclineRequest(long GroupID, long UID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[dbo].[SP_DeclinePendingRequest]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GroupID", GroupID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!";
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

        // changes to sp// 17,dec,2018
        public List<StudyGroupName> GetPendingRequestForGroup(long UID)
        {
            List<StudyGroupName> Result = new List<StudyGroupName>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT sg.Group_Name,sg.Group_ID FROM [studentinfo].[Study_Group_Member] AS sgm
                                    LEFT JOIN [studentinfo].[Study_Group] AS sg on sg.Group_ID=sgm.Group_Fk_ID
                                    WHERE sgm.UID_Stu=@UID AND sgm.IsPending=1";
                    //SqlCommand com = new SqlCommand("[dbo].[SP_GetPendingRequestForGroup]", con);

                    SqlCommand com = new SqlCommand(Query, con);
                    //  com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        StudyGroupName temp = new StudyGroupName();
                        temp.GroupID = Convert.ToInt32(reader["Group_ID"]);
                        temp.GroupName = Convert.ToString(reader["Group_Name"]);
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

        // study group  changes to sp// create group,, // changes to sp 12,dec,18
        public string AddNewGroup(StudyGroupName Info)
        {
            string Result = "Failed!";
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_AddNewGroupNew", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GroupName", Info.GroupName);
                    com.Parameters.AddWithValue("@AboutGroup", string.IsNullOrEmpty(Info.AboutGroup) ? "" : Info.AboutGroup);
                    com.Parameters.AddWithValue("@UID", Info.UID);
                    com.Parameters.AddWithValue("@CreatedByUsrName", Info.CreatedBy);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success! New group is inserted";
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }
            return Result;
        }
        
        // delete study group
        public String DeleteStudyGroup(long id)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_DeleteStudyGroup", connt);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GroupID", id);

                    connt.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                        Res = "Success|Group deleted";
                    }
                    else
                    {
                        Res = "Failed|Updation Failed";
                    }
                    connt.Close();
                }
            }
            catch (Exception EX)
            {

                //throw;
            }

            return Res;
        }
               
        // new addition june 2019
        public StudyGroupName EditStudyGroup(long GroupID)
        {
            StudyGroupName obj = new StudyGroupName();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Group_ID,Group_Name,Group_About
                                    from [studentinfo].[Study_Group]
                                    where Group_ID=@Group_ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Group_ID", GroupID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        obj.GroupID = Convert.ToInt64(reader["Group_ID"]);
                        obj.GroupName = Convert.ToString(reader["Group_Name"]);
                        obj.AboutGroup = Convert.ToString(reader["Group_About"]);
                        break;
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }
            return obj;
        }

        public String UpdateStudyGroupInfo(StudyGroupName Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {

                    string Query = @"Update [studentinfo].[Study_Group] set Group_Name=@grpnm,Group_About=@grpabt
                                    where Group_ID=@grpid ";


                    SqlCommand cmd = new SqlCommand(Query, connt);
                    cmd.Parameters.AddWithValue("@grpid", Info.GroupID);
                    cmd.Parameters.AddWithValue("@grpnm", Info.GroupName);
                    if (Info.AboutGroup != null && Info.AboutGroup != "")
                    {

                        cmd.Parameters.AddWithValue("@grpabt", Info.AboutGroup);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@grpabt", "");
                    }
                    connt.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {

                        Res = "Success|Group Info Sucessfuly Updated";
                    }
                    else
                    {
                        Res = "Failed|Updation Failed";
                    }
                    connt.Close();




                }
            }
            catch (Exception ex)
            {

            }
            return Res;
        }
        
        // Group Study Changes to SP// 
        // changes to sp 17,dec,18//

        public string SentRequestGroupMember(RequestGroupMember Info)
        {
            string Result = "Failed!";
            try
            {
                if (IsSocialStudent(Info.Logintype, Info.UID))
                {
                    if (!(IsAlreadyMember(Info.GroupID, Info.UID)))
                    {
                        using (SqlConnection con = new SqlConnection(conn))
                        {
                            SqlCommand com = new SqlCommand("SP_SentRequestToAddGroup", con);
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@GroupID", Info.GroupID);
                            com.Parameters.AddWithValue("@UID", Info.UID);
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success! Request sent";
                            }
                            else
                            {
                                Result = "Already sent a request.";
                            }
                            con.Close();
                        }
                    }
                    else
                    {
                        Result = "Can't send request to existing member";
                    }
                }
                else
                {
                    Result = "Not a registered social student";
                }
            }

            catch (Exception ex)
            {
                Result = "Falied! Failed";
            }
            return Result;
        }
        
        //checking for already member 22march, 2019 sm//

        public bool IsAlreadyMember(long grpid, long uid)
        {
            bool result = false;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select IsPending from studentinfo.Study_Group_Member
                                    where Group_Fk_ID=@grpid and [UID_Stu]=@UID and IsPending=0";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@grpid", grpid);
                    com.Parameters.AddWithValue("@UID", uid);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }

                }

            }
            catch (Exception)
            {
                result = false;

            }
            return result;
        }
               
        //social student or not checking 22march,2019, sm//
        public bool IsSocialStudent(long usertype, long UID)
        {
            bool result = false;
            string Query = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    if (UID != 0)
                    {
                        Query = @"SELECT UserID_FK,Stu_Name
                                        FROM studentinfo.Student_Details

                                        where LoginTpe_FK=@logintype and UserID_FK=@UID";
                    }
                    else
                    {
                        Query = @"SELECT UserID_FK,Stu_Name
                                    FROM studentinfo.Student_Details
                                    WHERE NOT EXISTS (select UserID_FK,Stu_Name from  studentinfo.Student_Details
                                    where LoginTpe_FK=@logintype and UserID_FK=@UID)";
                    }
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@logintype", usertype);
                    com.Parameters.AddWithValue("@UID", UID);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (UID != 0)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                result = false;

            }
            return result;
        }


        // changes made to sp 17,dec,18
        public string InsertStudentNote(StudentNote Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("InsertStudentNote", con); // changes made to SP, 12th nov//
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", Info.UID);
                    com.Parameters.AddWithValue("@Note", Info.Note);
                    //com.Parameters.AddWithValue("@ID",ParameterDirection.ReturnValue);
                    com.Parameters.Add("@ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        string id = com.Parameters["@ID"].Value.ToString();
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
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        // altered sp with columns// sp altered 12,dec,18
        public List<StudentNote> GetStudentNotes(long UID)
        {
            List<StudentNote> Result = new List<StudentNote>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("GetStudentNote", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID ", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        StudentNote temp = new StudentNote();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Note = Convert.ToString(reader["Note"]);
                        temp.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
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

        // sp changed to query
        // query modified 17,dec,2018
        public List<AutoComplete> GetAllActiveStudent(string Keyword)
        {
            List<AutoComplete> Result = new List<AutoComplete>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @" SELECT usr.UID,st.Stu_Name
                                     from admin.users as usr 
                                     INNER JOIN  [studentinfo].[Student_Details]  as st ON st.UserID_FK=usr.UID
                                      WHERE st.STATUS=1 AND st.LoginTpe_FK=8 and Stu_Name LIKE '%'+@Keyword+'%'";
                    // SqlCommand com = new SqlCommand("[studentinfo].[SP_GetAllActiveStudent]", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Keyword", Keyword);
                    // com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AutoComplete temp = new AutoComplete();
                        temp.value = Convert.ToInt32(reader["UID"]);
                        temp.label = Convert.ToString(reader["Stu_Name"]);
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

        // sp changed to query//
        // query modified 17th,dec,18
        public string RemoveStudentNote(long UID, long NoteID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"DELETE FROM [studentinfo].[StudentNote] WHERE ID=@NoteID AND UID_Stu=@UID";
                    // SqlCommand com = new SqlCommand("SP_RemoveStudentNote", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", @UID);
                    com.Parameters.AddWithValue("@NoteID", NoteID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success! Deleted";
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

        // Study group made changes//
        //ok 17,12,18
        public List<StudyPlanGoals> GetStudyPlanGoals()
        {
            List<StudyPlanGoals> Result = new List<StudyPlanGoals>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT ID,Goal FROM [studentinfo].[SetStudyPlanGoals] WHERE Status=1";
                    SqlCommand com = new SqlCommand(Query, con);
                    // SqlCommand com = new SqlCommand("[studentinfo].[GetStudyPlanGoals]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        StudyPlanGoals temp = new StudyPlanGoals();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Goal = Convert.ToString(reader["Goal"]);
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

        // study group made changes//
        // query changed 17,dec,18
        public string InsertGroupStudyPlan(SetGroupStudyPlan Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    String Query = @"INSERT INTO [studentinfo].[SetStudyPlan](Subject,Chapter,TestDate,GoalID_FK,UID_Stu,GroupID_FK)VALUES(@Subject,@Chapter,@TestDate,@GoalID_FK,@UID,@GroupID_FK)";
                    SqlCommand com = new SqlCommand(Query, con);
                    //SqlCommand com = new SqlCommand("[studentinfo].[InsertStudyPlane]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Subject", Info.Subject);
                    com.Parameters.AddWithValue("@Chapter", Info.Chapter);
                    com.Parameters.AddWithValue("@TestDate", Info.TestDate = DateTime.Now);
                    com.Parameters.AddWithValue("@GoalID_FK", Info.GoalID);
                    com.Parameters.AddWithValue("@UID_Stu", Info.UID);
                    com.Parameters.AddWithValue("@GroupID_FK", Info.GroupID);

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Inserted";
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
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        //study grp register report SM//
        public String InsertSgReportInTable(TopicWiseComment Info)
        {
            string Res = string.Empty;
            try
            {
                if (!(CheckReportExists(Info.Group_Comment_ID, Info.ReportBy)))
                {
                    using (SqlConnection connt = new SqlConnection(conn))
                    {
                        string Query = @" Insert into [site_admin].[CommentReport]
                                     (CommentText,Go2UniSection,CommentID,CommentById,CommentByMail,ReportReason,ReportedBy)
                                     values(@Comment,'StudyGroup',@CommID,@CommById,@CommByMail,@reason,@ReportedBy)";
                        SqlCommand cmd = new SqlCommand(Query, connt);

                        cmd.Parameters.AddWithValue("@Comment", Info.Group_Comment_Text);
                        // cmd.Parameters.AddWithValue("")
                        cmd.Parameters.AddWithValue("@CommID", Info.Group_Comment_ID);
                        cmd.Parameters.AddWithValue("@CommById", Info.UserID_FK);
                        cmd.Parameters.AddWithValue("@CommByMail", Info.EmailId);
                        cmd.Parameters.AddWithValue("@reason", Info.ReportReason);
                        cmd.Parameters.AddWithValue("ReportedBy", Info.ReportBy);

                        connt.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Res = "Success! Your Report is being registered to the Go2uni admin";
                        }
                        else
                        {
                            Res = "Failed ! to register report";
                        }
                        connt.Close();
                    }
                }
                else
                {
                    Res = "You have already reported to admin against this Comment.";
                }
            }
            catch (Exception ex)
            {


            }

            return Res;

        }

        public Topics EditSgTopicName(long TopicID, long GroupID)
        {
            Topics res = new Topics();
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {

                    string Query = @"select ID,Topic,GroupID_FK from studentinfo.Study_Group_Topic
                                    where ID=@topicid and GroupID_FK=@grpid";

                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@TopicID", TopicID);
                    com.Parameters.AddWithValue("@grpid", GroupID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        res.ID = Convert.ToInt64(reader["ID"]);
                        res.Topic = Convert.ToString(reader["Topic"]);
                        res.GroupID = Convert.ToInt64(reader["GroupID_FK"]);
                        break;
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }
            return res;
        }
        
        public String UpdateStudyGroupTopicName(Topics Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {

                    string Query = @"update studentinfo.Study_Group_Topic set Topic=@topic where ID=@id and UID_Stu=@uid";


                    SqlCommand cmd = new SqlCommand(Query, connt);
                    cmd.Parameters.AddWithValue("@id", Info.ID);
                    cmd.Parameters.AddWithValue("@topic", Info.Topic);
                    cmd.Parameters.AddWithValue("@uid", Info.UID);
                    connt.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {

                        Res = "Success| Topic name Sucessfuly Updated";
                    }
                    else
                    {
                        Res = "|Updation Failed";
                    }
                    connt.Close();




                }
            }
            catch (Exception ex)
            {

            }
            return Res;
        }
               
                       
        // delete study group
        public String DeleteStudyGroupTopic(long id)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_DeleteStudyGroupTopic", connt);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TopicId", id);

                    connt.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                        Res = "Success|Topic deleted";
                    }
                    else
                    {
                        Res = "Failed|Deletion Failed";
                    }
                    connt.Close();
                }
            }
            catch (Exception EX)
            {


            }

            return Res;
        }
        #endregion

        #region Ramashree 17/12/2018    Edit Profile

        public SocialStudentEditprofile GetDetailsOFEditProfileSocialStudent(long ID)
        {
            SocialStudentEditprofile Result = new SocialStudentEditprofile();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Isnull(Sd.UserID_FK,'')as UserID_FK,Isnull(Sd.Stu_Name,'')as Stu_Name,Isnull(Sd.Stu_Gender_FK,'')as Stu_Gender_FK,
                                    Isnull(Sd.Stu_DOB,'') as Stu_DOB,Isnull(Sd.Stu_MObile_No,'')as Stu_MObile_No,Isnull(Sd.Stu_Profile_Image,'')as Stu_Profile_Image,
                                    Isnull(Sd.DivisionID,'') as DivisionID,Isnull(Sd.Level_ID,'')as Level_ID,
                                    Isnull(Users.Email,'')as Email,Isnull(Users.password,'')as password from studentinfo.Student_Details as Sd
                                    inner join admin.users as users on Sd.UserID_FK=Users.UID where UserID_FK=@UserID_FK";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID_FK", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.Stu_Gender_FK = Convert.ToInt16(reader["Stu_Gender_FK"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);

                        Result.tempDob = Convert.ToString(reader["Stu_DOB"]);
                        Result.Stu_DOB = Convert.ToDateTime(Result.tempDob);

                        Result.Stu_DOB = Convert.ToDateTime(reader["Stu_DOB"]);
                        Result.Stu_MObile_No = Convert.ToString(reader["Stu_MObile_No"]);
                        Result.DivisionID = Convert.ToInt16(reader["DivisionID"]);
                        Result.Level_ID = Convert.ToInt16(reader["Level_ID"]);
                        Result.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
                        break;
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

        public String UpdateSocialStudentEditProfile(SocialStudentEditprofile Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    if (Info.NewEmail == Info.Email)
                    {

                        string Query = string.Empty;

                        if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                        {
                            Query = @" Update[admin].[users]set Email=@Email,Password=@Password where UID=@UserID_FK
                                   update studentinfo.Student_Details set IsFirstStepComplete=1,UserID_FK=@UserID_FK,Stu_Name=@Stu_Name,Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                   Stu_MObile_No=@Stu_MObile_No,DivisionID=@DivisionID,Level_ID=@Level_ID,Stu_Profile_Image=@Stu_Profile_Image  where UserID_FK=@UserID_FK";
                        }
                        else
                        {
                            Query = @"Update[admin].[users]set Email=@Email,Password=@Password where UID=@UserID_FK
                                   update studentinfo.Student_Details set IsFirstStepComplete=1,UserID_FK=@UserID_FK,Stu_Name=@Stu_Name,Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                   Stu_MObile_No=@Stu_MObile_No,DivisionID=@DivisionID,Level_ID=@Level_ID where UserID_FK=@UserID_FK";
                        }
                        SqlCommand cmd = new SqlCommand(Query, connt);
                        cmd.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                        cmd.Parameters.AddWithValue("@Stu_Name", Info.Stu_Name);
                        cmd.Parameters.AddWithValue("@Stu_Gender_FK", Info.Stu_Gender_FK);
                        cmd.Parameters.AddWithValue("@Email", Info.Email);
                        cmd.Parameters.AddWithValue("@Password", Info.Password);
                        cmd.Parameters.AddWithValue("@Stu_DOB", Info.Stu_DOB);
                        cmd.Parameters.AddWithValue("@Stu_MObile_No", Info.Stu_MObile_No);
                        cmd.Parameters.AddWithValue("@DivisionID", Info.DivisionID);
                        cmd.Parameters.AddWithValue("@Level_ID", Info.Level_ID);
                        if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                        {
                            cmd.Parameters.AddWithValue("@Stu_Profile_Image", Info.Stu_Profile_Image);
                        }
                        connt.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Res = "Successfully! Profile Updated";
                        }
                        else
                        {
                            Res = "!Social student Profile Updation Failed";
                        }
                        connt.Close();

                    }
                    else
                    {

                        string Query = string.Empty;

                        if (!(CheckEMailExists(Info.Email)))
                        {
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                Query = @" Update[admin].[users]set Email=@Email,Password=@Password where UID=@UserID_FK
                                   update studentinfo.Student_Details set IsFirstStepComplete=1,UserID_FK=@UserID_FK,Stu_Name=@Stu_Name,Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                   Stu_MObile_No=@Stu_MObile_No,DivisionID=@DivisionID,Level_ID=@Level_ID,Stu_Profile_Image=@Stu_Profile_Image  where UserID_FK=@UserID_FK";
                            }
                            else
                            {
                                Query = @"Update[admin].[users]set Email=@Email,Password=@Password where UID=@UserID_FK
                                   update studentinfo.Student_Details set IsFirstStepComplete=1,UserID_FK=@UserID_FK,Stu_Name=@Stu_Name,Stu_Gender_FK=@Stu_Gender_FK,Stu_DOB=@Stu_DOB,
                                   Stu_MObile_No=@Stu_MObile_No,DivisionID=@DivisionID,Level_ID=@Level_ID where UserID_FK=@UserID_FK";
                            }
                            SqlCommand cmd = new SqlCommand(Query, connt);
                            cmd.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                            cmd.Parameters.AddWithValue("@Stu_Name", Info.Stu_Name);
                            cmd.Parameters.AddWithValue("@Stu_Gender_FK", Info.Stu_Gender_FK);
                            cmd.Parameters.AddWithValue("@Email", Info.Email);
                            cmd.Parameters.AddWithValue("@Password", Info.Password);
                            cmd.Parameters.AddWithValue("@Stu_DOB", Info.Stu_DOB);
                            cmd.Parameters.AddWithValue("@Stu_MObile_No", Info.Stu_MObile_No);
                            cmd.Parameters.AddWithValue("@DivisionID", Info.DivisionID);
                            cmd.Parameters.AddWithValue("@Level_ID", Info.Level_ID);
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                cmd.Parameters.AddWithValue("@Stu_Profile_Image", Info.Stu_Profile_Image);
                            }
                            connt.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                                {
                                    Res = Info.Stu_Profile_Image;
                                }
                                else
                                {
                                    Res = "Successfully! Profile Updated";
                                }
                            }
                            else
                            {
                                Res = "!Social student Profile Updation Failed";
                            }
                            connt.Close();
                        }
                        else
                        {
                            Res = "!Email ID already Exits";
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return Res;
        }

        public bool CheckEMailExists(string MailID)
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
        #endregion

        #region Ramashree 21/12/2018 Exam Type     
        public List<ExamTypeDetails> ViewExamType()
        {
            List<ExamTypeDetails> Result = new List<ExamTypeDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select * from studentinfo.ExamType";
                    SqlCommand com = new SqlCommand(Query, con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamTypeDetails temp = new ExamTypeDetails();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.ExamType = Convert.ToString(reader["ExamType"]);

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

        public List<ExamSubject> ViewSubjectbyExamID(long ExamID)
        {
            List<ExamSubject> Result = new List<ExamSubject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ID,Subject from studentinfo.Examsubjects where ExamID=@ExamID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ExamID", ExamID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamSubject temp = new ExamSubject();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
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

        public List<ExamTopic> ViewTopicbySubjectID(long SubjectID, long UserID)
        {
            List<ExamTopic> Result = new List<ExamTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select studentinfo.ExamTopics.ID,studentinfo.ExamTopics.Topic,
                                    studentinfo.ExamTopics.SubjectID,
                                    ISNULL(CASE WHEN CONVERT(DATE, studentinfo.Goal.StartDate) = '1900-01-01' THEN '' ELSE CONVERT(CHAR(10), studentinfo.Goal.StartDate, 121) END, '') AS StartDate,
                                    ISNULL(CASE WHEN CONVERT(DATE, studentinfo.Goal.EndDate) = '1900-01-01' THEN '' ELSE CONVERT(CHAR(10), studentinfo.Goal.EndDate, 121) END, '') AS EndDate,
                                    isnull(studentinfo.Goal.Completed_Status,'')as Completed_Status 
                                    from studentinfo.ExamTopics
                                    left join studentinfo.Goal on studentinfo.ExamTopics.ID=studentinfo.Goal.ExamTopicID_FK 
                                    and studentinfo.Goal.UserID_FK=@UID    
                                    where studentinfo.ExamTopics.SubjectID=@SubjectID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@SubjectID", SubjectID);
                    com.Parameters.AddWithValue("@UID", UserID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamTopic temp = new ExamTopic();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.SubjectID = Convert.ToInt32(reader["SubjectID"]);
                        temp.StartDate = Convert.ToString(reader["StartDate"]);
                        temp.EndDate = Convert.ToString(reader["EndDate"]);
                        temp.Completed_Status = Convert.ToBoolean(reader["Completed_Status"]);

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

        public List<ExamContent> ViewTopicContentbySubjectID(long SubjectID)
        {
            List<ExamContent> Result = new List<ExamContent>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select  ID, TopicContent,TopicID,SubjectID from studentinfo.ExamContents 
                                    where studentinfo.ExamContents.SubjectID=@SubjectID";


                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@SubjectID", SubjectID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamContent temp = new ExamContent();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.TopicID = Convert.ToInt32(reader["TopicID"]);
                        temp.TopicContent = Convert.ToString(reader["TopicContent"]);

                        temp.SubjectID = Convert.ToInt32(reader["SubjectID"]);

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

        #endregion

        #region Online community

        //altered proc 20,dec,18
        public string InsertTopicByStudentID(Online_Community Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"INSERT INTO studentinfo.Online_Community(Community_Name,Community_About,Community_CreatedDate,Community_status,Created_By_UsrID,UserType) VALUES(@Community_Name,@Community_About, GETDATE(),1,@CreatedBy,@UserType)";
                    SqlCommand com = new SqlCommand(Query, con);
                    // SqlCommand com = new SqlCommand("SP_InsertOnlineCommunity", con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Community_Name", Info.Community_Name);
                    com.Parameters.AddWithValue("@Community_About", "");
                    com.Parameters.AddWithValue("@CreatedBy", Info.CreatedBy);
                    com.Parameters.AddWithValue("@UserType", Info.Login_Type);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success! New community inserted";
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

        //changed on 8th june//
        public List<OnlineCommunityComment> GetAllCommentsOfTopic(long TopicID)
        {
            List<OnlineCommunityComment> Result = new List<OnlineCommunityComment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {


                    string query = @"select oct.Topic_ID ,oct.UID_CreatedBY,oct.Community_Fk_ID,oct.Topic_Discussion,oct.Topic_CreatedDate,  
                                       oc.Community_Name,occ.Community_Comment_ID,occ.Community_Comment_Like,  
                                       occ.Community_Comment_Dislike,occ.Community_Comment_Text,occ.Community_Comment_CommentedDate,occ.Community_Comment_status,occ.UID,occ.IsReported,  
                                       sa.Stu_Name as Commentby,sa.Stu_Profile_Image  from  studentinfo.Online_Community as oc    
                                       inner join studentinfo.Online_Community_Topic as oct on oct.Community_Fk_ID=oc.Community_ID    
                                       inner join studentinfo.Online_Community_Comment as occ on occ.Topic_Fk_ID=oct.Topic_ID   
                                       inner join admin.users as sb on oct.UID_CreatedBY=sb.UID
                                       inner join studentinfo.Student_Details as sa on sa.UserID_FK=occ.UID 
                                       where occ.Topic_Fk_ID=@Topic_ID and occ.IsDeleted=0 order by   
                                       occ.Community_Comment_ID desc";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@Topic_ID", TopicID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OnlineCommunityComment temp = new OnlineCommunityComment();
                        temp.Topic_Fk_ID = Convert.ToInt64(reader["Topic_ID"]);
                        temp.UID = Convert.ToInt64(reader["UID"]);
                        temp.Community_Fk_ID = Convert.ToInt64(reader["Community_Fk_ID"]);
                        temp.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);
                        temp.tempTopic_CreatedDate = Convert.ToString(reader["Topic_CreatedDate"]);
                        temp.Community_Name = Convert.ToString(reader["Community_Name"]);
                        temp.Community_Comment_ID = Convert.ToInt64(reader["Community_Comment_ID"]);
                        temp.Community_Comment_Like = Convert.ToInt64(reader["Community_Comment_Like"]);
                        temp.Community_Comment_Dislike = Convert.ToInt64(reader["Community_Comment_Dislike"]);
                        temp.Community_Comment_Text = Convert.ToString(reader["Community_Comment_Text"]);
                        temp.tempCommunity_Comment_CommentedDate = Convert.ToString(reader["Community_Comment_CommentedDate"]);
                        temp.Community_Comment_status = Convert.ToBoolean(reader["Community_Comment_status"]);
                        temp.Commentby = Convert.ToString(reader["Commentby"]);
                        temp.student_ProfileImage = Convert.ToString(reader["Stu_Profile_Image"]);
                        temp.IsReported = Convert.ToBoolean(reader["IsReported"]);
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

        //SP Changed to Query 20,dec,18
        public string InsertTopicByCommunityID(OnlineCommunityTopic Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Insert into studentinfo.Online_Community_Topic (UID_CreatedBY,Community_Fk_ID,Topic_Discussion,Topic_CreatedDate,
                                    Topic_status) values (@UID_CreatedBY,@Community_Fk_ID,@Topic_Discussion,getdate(),1)";
                    // SqlCommand com = new SqlCommand("proc_community.SP_InsertTopicInOnlineCommunity", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    //com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID_CreatedBY", Info.UID_CreatedBY);
                    com.Parameters.AddWithValue("@Community_Fk_ID", Info.Community_Fk_ID);
                    com.Parameters.AddWithValue("@Topic_Discussion", Info.Topic_Discussion);
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

        public List<OnlineCommunityTopic> GetTopicIDLastInserted(long UID)
        {
            List<OnlineCommunityTopic> Result = new List<OnlineCommunityTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select * from studentinfo.Online_Community_Topic where UID_CreatedBY=@UID_CreatedBY  order by Topic_ID desc";
                    //SqlCommand com = new SqlCommand("proc_community.SP_GetListOFTopicInDescOrder", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    //  com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID_CreatedBY", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OnlineCommunityTopic temp = new OnlineCommunityTopic();
                        temp.Topic_ID = Convert.ToInt32(reader["Topic_ID"]);
                        temp.UID_CreatedBY = Convert.ToInt32(reader["UID_CreatedBY"]);
                        temp.Community_Fk_ID = Convert.ToInt32(reader["Community_Fk_ID"]);
                        temp.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);
                        temp.Topic_CreatedDate = Convert.ToDateTime(reader["Topic_CreatedDate"]);
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

        //sp changed to query 24th dec,18
        //get comment details for  SG report by comment ID , SM//
        public TopicWiseComment GetDetailsOfComment(TopicWiseComment Info)
        {
            TopicWiseComment Result = new TopicWiseComment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select usr.UID,usr.Email,sgc.Group_Comment_ID,sgc.Group_Comment_Text
                                        from admin.users as usr
                                        inner join studentinfo.Study_Group_Comment as sgc
                                        ON sgc.[UID(St)]=usr.UID
                                        where sgc.Group_Comment_ID=@CommID";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@CommID", Info.Group_Comment_ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.UserID_FK = Convert.ToInt32(reader["UID"]);
                        Result.EmailId = Convert.ToString(reader["Email"]);
                        Result.Group_Comment_ID = Convert.ToInt32(reader["Group_Comment_ID"]);
                        Result.Group_Comment_Text = Convert.ToString(reader["Group_Comment_Text"]);
                    }

                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }
            return Result;
        }
        
        public string InsertCommentByTopicID(OnlineCommunityComment Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string Query = @"INSERT INTO studentinfo.Online_Community_Comment
                                    (Community_Comment_Like,Community_Comment_Dislike,Community_Comment_Text,Community_Comment_CommentedDate,UID,Community_Comment_status,Topic_Fk_ID,IsDeleted,IsReported)   
                                    VALUES(@like,@dislike,@text,getdate(),@UID,@status,@TopicID,@isdeleted,@isreported)";
                    SqlCommand com = new SqlCommand(Query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@like", 0);
                    com.Parameters.AddWithValue("@dislike", 0);
                    com.Parameters.AddWithValue("@text", Info.Community_Comment_Text);
                    com.Parameters.AddWithValue("@UID", Info.UID);
                    com.Parameters.AddWithValue("@status", 1);
                    com.Parameters.AddWithValue("@TopicID", Info.Topic_Fk_ID);
                    com.Parameters.AddWithValue("@isdeleted", 0);
                    com.Parameters.AddWithValue("@isreported", 0);
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


        // sp changed to query 26,dec,18
        public List<OnlineCommunityComment> GetAllCommentsOfTopicByID(long UID, long TopicID)
        {
            List<OnlineCommunityComment> Result = new List<OnlineCommunityComment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select oct.Topic_ID ,oct.UID_CreatedBY,oct.Community_Fk_ID,oct.Topic_Discussion,oct.Topic_CreatedDate,  

	                                oc.Community_Name,occ.Community_Comment_ID,occ.Community_Comment_Like,  

	                                occ.Community_Comment_Dislike,occ.Community_Comment_Text,occ.Community_Comment_CommentedDate,occ.Community_Comment_status,  

	                                occ.UID,  

	                                sd.Stu_Name as Commentby from  studentinfo.Online_Community as oc    

	                                inner join studentinfo.Online_Community_Topic as oct on oct.Community_Fk_ID=oc.Community_ID    

	                                inner join studentinfo.Online_Community_Comment as occ on occ.Topic_Fk_ID=oct.Topic_ID   

	                                inner join studentinfo.Student_Details as sd on sd.UserID_FK= occ.UID  

                                   inner join admin.users as usr on usr.UID=oct.UID_CreatedBY
   
                                    where occ.Topic_Fk_ID=@Topic_ID 

                                    and occ.UID=@UID order by occ.Community_Comment_ID desc";
                    // SqlCommand com = new SqlCommand("SP_GetlastCommentofTopic", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Topic_ID", TopicID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OnlineCommunityComment temp = new OnlineCommunityComment();

                        temp.Topic_Fk_ID = Convert.ToInt32(reader["Topic_ID"]);
                        temp.UID = Convert.ToInt32(reader["UID_CreatedBY"]);
                        temp.Community_Fk_ID = Convert.ToInt32(reader["Community_Fk_ID"]);
                        temp.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);
                        temp.Topic_CreatedDate = Convert.ToDateTime(reader["Topic_CreatedDate"]);
                        temp.Community_Name = Convert.ToString(reader["Community_Name"]);
                        temp.Community_Comment_ID = Convert.ToInt32(reader["Community_Comment_ID"]);
                        temp.Community_Comment_Like = Convert.ToInt32(reader["Community_Comment_Like"]);
                        temp.Community_Comment_Dislike = Convert.ToInt32(reader["Community_Comment_Dislike"]);
                        temp.Community_Comment_Text = Convert.ToString(reader["Community_Comment_Text"]);
                        temp.Community_Comment_CommentedDate = Convert.ToDateTime(reader["Community_Comment_CommentedDate"]);
                        temp.Community_Comment_status = Convert.ToBoolean(reader["Community_Comment_status"]);
                        temp.Commentby = Convert.ToString(reader["Commentby"]);
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
        //sp changed to query 26dec,18
        public List<Online_Community_Members> GetAllJoinedCommunityList(long UID)
        {
            List<Online_Community_Members> Result = new List<Online_Community_Members>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" select * from studentinfo.Online_Community_Members where Community_Members=@UID";
                    // SqlCommand com = new SqlCommand("proc_community.SP_GetallCommunityListByMember", con);
                    SqlCommand com = new SqlCommand(query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Online_Community_Members temp = new Online_Community_Members();
                        temp.Community_Member_ID = Convert.ToInt32(reader["Community_Member_ID"]);
                        temp.Community_Members = Convert.ToInt32(reader["Community_Members"]);
                        temp.Community_FK_ID = Convert.ToInt32(reader["Community_FK_ID"]);
                        temp.Community_Member_CreatedDate = Convert.ToDateTime(reader["Community_Member_CreatedDate"]);
                        temp.Community_Member_Status = Convert.ToBoolean(reader["Community_Member_Status"]);
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


        public string JoinedstudentToparticularCommunity(Online_Community Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"INSERT INTO studentinfo.Online_Community_Members VALUES(@UID,@CommunityID,getdate(),1)";
                    SqlCommand com = new SqlCommand(Query, con);
                    // SqlCommand com = new SqlCommand("proc_community.SP_InsertcommunityMember", con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", Info.CreatedBy);
                    com.Parameters.AddWithValue("@CommunityID", Info.Community_ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!";
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

        // sp changed to query//
        public string JoinNewCommunityByID(Online_Community_Members Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"INSERT INTO studentinfo.Online_Community_Members VALUES(@UID,@CommunityID,getdate(),1)";
                    SqlCommand com = new SqlCommand(Query, con);
                    // SqlCommand com = new SqlCommand("proc_community.SP_InsertcommunityMember", con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", Info.Community_Members);
                    com.Parameters.AddWithValue("@CommunityID", Info.Community_FK_ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success! Joined Successfully";
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

        //sp changed to query 26th,dec,18
        public string InsertDataForLikeAndUnLike(long UID, long CommID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Insert into studentinfo.Online_Community_LikeORDisLike values(@UID,@Comment_Fk_ID,@Likes,@DisLike,getdate())";
                    // SqlCommand com = new SqlCommand("proc_community.SP_InsertOnlineCommunityLikeORDislike", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@Comment_Fk_ID", CommID);
                    com.Parameters.AddWithValue("@Likes", 0);
                    com.Parameters.AddWithValue("@DisLike", 0);
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



        public List<Online_Community_LikeORDisLike> CheckValOFCommentLikeOrDislikeIsExist(long UID, long CommentID)
        {
            List<Online_Community_LikeORDisLike> Result = new List<Online_Community_LikeORDisLike>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select * from studentinfo.Online_Community_LikeORDisLike where UID=@UID and Comment_Fk_ID=@CommentID";
                    // SqlCommand com = new SqlCommand("proc_community.SP_GetLikeOrdislikeOFCommunityComment", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", UID);
                    com.Parameters.AddWithValue("@CommentID", CommentID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Online_Community_LikeORDisLike temp = new Online_Community_LikeORDisLike();
                        // temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.UID = Convert.ToInt32(reader["UID"]);
                        temp.Comment_Fk_ID = Convert.ToInt32(reader["Comment_Fk_ID"]);
                        temp.Likes = Convert.ToBoolean(reader["Likes"]);
                        temp.DisLike = Convert.ToBoolean(reader["DisLike"]);
                        temp.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
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

        //sp changed to query 26,12,18
        public string InsertLikeOrDislikeValueOfOnlineCommunity(Online_Community_LikeORDisLike Info)
        {
            string Result = string.Empty;
            try
            {
                if (!(CheckLikeOrDisLikeExists(Info.Comment_Fk_ID, Info.UID)))
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        string Query = @"Insert into studentinfo.Online_Community_LikeORDisLike 
                                        (UID,Comment_Fk_ID,Likes,DisLike,CreatedDate)
                                        values(@UID,@Comment_Fk_ID,@Likes,@DisLike,getdate())";

                        SqlCommand com = new SqlCommand(Query, con);

                        com.Parameters.AddWithValue("@UID", Info.UID);
                        com.Parameters.AddWithValue("@Comment_Fk_ID", Info.Comment_Fk_ID);
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
                            if (CheckIfLikeisActiveOrnot(Info.Comment_Fk_ID, Info.UID))
                            {

                                string Query = @"Update studentinfo.Online_Community_LikeORDisLike set Likes=@like,DisLike=@dislike where Comment_Fk_ID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@comID", Info.Comment_Fk_ID);


                                com.Parameters.AddWithValue("@like", 0);
                                com.Parameters.AddWithValue("@DisLike", 0);


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
                                string Query = @"Update studentinfo.Online_Community_LikeORDisLike set Likes=@like,DisLike=@dislike where Comment_Fk_ID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@comID", Info.Comment_Fk_ID);
                                com.Parameters.AddWithValue("@like", 1);
                                com.Parameters.AddWithValue("@DisLike", 0);

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
                            if (CheckIfDislikeisActiveOrnot(Info.Comment_Fk_ID, Info.UID))
                            {
                                string Query = @"Update studentinfo.Online_Community_LikeORDisLike set Likes=@like,DisLike=@dislike where Comment_Fk_ID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@comID", Info.Comment_Fk_ID);


                                com.Parameters.AddWithValue("@like", 0);
                                com.Parameters.AddWithValue("@DisLike", 0);


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
                                string Query = @"Update studentinfo.Online_Community_LikeORDisLike set Likes=@like,DisLike=@dislike where Comment_Fk_ID=@comID and UID=@UID";
                                SqlCommand com = new SqlCommand(Query, con);
                                com.Parameters.AddWithValue("@UID", Info.UID);
                                com.Parameters.AddWithValue("@comID", Info.Comment_Fk_ID);
                                com.Parameters.AddWithValue("@like", 0);
                                com.Parameters.AddWithValue("@DisLike", 1);

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

        //sp changed to query 26,12,18
        //public string UpdateCommentLikeOrDislikeCount(Online_Community_LikeORDisLike Info)


        //{
        //    string Result = string.Empty;
        //    List<OnlineCommunityComment> Data = new List<OnlineCommunityComment>();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {
        //            Data = GetdetailsofOnlineCommentByCommenID(Info.Comment_Fk_ID);
        //            string query = @"Insert into studentinfo.Online_Community_LikeORDisLike values(@UID,@Comment_Fk_ID,@Likes,@DisLike,getdate())";
        //            // SqlCommand com = new SqlCommand("proc_community.SP_InsertOnlineCommunityLikeORDislike", con);
        //            //com.CommandType = CommandType.StoredProcedure;
        //            SqlCommand com = new SqlCommand(query, con);
        //            com.Parameters.AddWithValue("@UID", Info.UID);
        //            com.Parameters.AddWithValue("@Comment_Fk_ID", Info.Comment_Fk_ID);
        //            if (Info.LikeVal == "Like")
        //            {
        //                com.Parameters.AddWithValue("@Likes", 1);
        //                com.Parameters.AddWithValue("@DisLike", 0);
        //            }
        //            else
        //            {
        //                com.Parameters.AddWithValue("@Likes", 0);
        //                com.Parameters.AddWithValue("@DisLike", 1);
        //            }

        //            con.Open();
        //            if (com.ExecuteNonQuery() > 0)
        //            {
        //                Result = "Success! Inserted";
        //            }
        //            else
        //            {
        //                Result = "Failed! ";
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Result;
        //}

        internal string UpdateLikesDislikesInCommentTable(long comID, string value)


        {
            string Result = string.Empty;

            Online_Community_LikeORDisLike val = new Online_Community_LikeORDisLike();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {




                    string Query = @"update studentinfo.Online_Community_Comment 
                                    set Community_Comment_Like=(
                                    select count(Likes) from studentinfo.Online_Community_LikeORDisLike 
                                    where studentinfo.Online_Community_LikeORDisLike.Comment_Fk_ID=studentinfo.Online_Community_Comment.Community_Comment_ID and Likes=1),
                                    Community_Comment_Dislike=(select count(DisLike) from studentinfo.Online_Community_LikeORDisLike
                                    where studentinfo.Online_Community_LikeORDisLike.Comment_Fk_ID=studentinfo.Online_Community_Comment.Community_Comment_ID and DisLike=1) 
                                    where Community_Comment_ID=@commId";

                    SqlCommand com = new SqlCommand(Query, con);


                    com.Parameters.AddWithValue("@commId", comID);
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
        //sp changed to query 26,12,18
        public List<OnlineCommunityComment> GetdetailsofOnlineCommentByCommenID(long CommentID)
        {
            List<OnlineCommunityComment> Result = new List<OnlineCommunityComment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select * from studentinfo.Online_Community_Comment where Community_Comment_ID=@CommentID";
                    // SqlCommand com = new SqlCommand("proc_community.SP_GetDetailsOFCommunityComment", con);
                    // com.CommandType = CommandType.StoredProcedure;
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@CommentID", CommentID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OnlineCommunityComment temp = new OnlineCommunityComment();
                        temp.Community_Comment_ID = Convert.ToInt32(reader["Community_Comment_ID"]);
                        temp.Community_Comment_Like = Convert.ToInt32(reader["Community_Comment_Like"]);
                        temp.Community_Comment_Dislike = Convert.ToInt32(reader["Community_Comment_Dislike"]);
                        temp.Community_Comment_Text = Convert.ToString(reader["Community_Comment_Text"]);
                        temp.UID = Convert.ToInt32(reader["UID"]);
                        temp.Community_Comment_CommentedDate = Convert.ToDateTime(reader["Community_Comment_CommentedDate"]);
                        temp.Community_Comment_status = Convert.ToBoolean(reader["Community_Comment_status"]);
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

        //changes made on  29,01,19 SM
        public List<Online_Community> GetAllCommunityListByID()
        {
            List<Online_Community> Result = new List<Online_Community>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {


                    string Query = @"select * from studentinfo.Online_Community";
                    SqlCommand com = new SqlCommand(Query, con);

                    con.Open();
                    // com.Parameters.AddWithValue("@UID", UID);
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Online_Community temp = new Online_Community();

                        temp.Community_ID = Convert.ToInt32(reader["Community_ID"]);
                        temp.Community_Name = Convert.ToString(reader["Community_Name"]);
                        temp.Community_CreatedDate = Convert.ToDateTime(reader["Community_CreatedDate"]);

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

        //Edit Community comment SM 29.jan//
        public OnlineCommunityComment EditCommunityComment(long Community_Comment_ID, long UID)
        {
            OnlineCommunityComment Result = new OnlineCommunityComment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Community_Comment_Text,Community_Comment_ID from studentinfo.Online_Community_Comment 
                                     where Community_Comment_ID=@CommId and UID=@UID";


                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@CommId", Community_Comment_ID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        //Comment temp = new Comment();
                        // temp.GroupID = Convert.ToInt32(reader["Group_ID"]);
                        Result.Community_Comment_ID = Convert.ToInt32(reader["Community_Comment_ID"]);
                        Result.Community_Comment_Text = Convert.ToString(reader["Community_Comment_Text"]);
                        //  Result.UID = Convert.ToInt32(reader["UID(St)"]);
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

        // update comment SM 29th,jan//
        public string UpdateCommunityComment(long UID, long CommunityCommID, string Comment)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"Update studentinfo.Online_Community_Comment
                                    Set Community_Comment_Text=@CommentText
                                    where Community_Comment_ID=@CommID and UID=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    cmd.Parameters.AddWithValue("@CommID", CommunityCommID);
                    cmd.Parameters.AddWithValue("@CommentText", Comment);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success! status updated";
                    }
                    else
                    {
                        res = "Failed!";

                    }
                }
            }
            catch (Exception ex)
            {


            }
            return res;
        }
                
        // query changed to sp 26,12,18
        public List<OnlineCommunityTopic> GetAllTopicListByCommunityID(long CommunityID)
        {
            List<OnlineCommunityTopic> Result = new List<OnlineCommunityTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {                    

                    string query = @"select ct.Topic_ID,ct.UID_CreatedBY,sd.Stu_Name,ct.Community_Fk_ID,ct.Topic_Discussion,ct.Topic_CreatedDate,cc.Community_Name as CommunityName,ct.Topic_status   
                                    from  studentinfo.Online_Community_Topic as ct   
                                    inner join admin.users as sa on sa.UID=ct.UID_CreatedBY   
                                    inner join studentinfo.Student_Details as sd on sd.UserID_FK=sa.UID
                                    inner join studentinfo.Online_Community as cc on cc.Community_ID=ct.Community_Fk_ID  
                                    where ct.Community_Fk_ID=@CommunityID and ct.Topic_status=1 order by ct.Topic_ID desc";
                    // SqlCommand com = new SqlCommand("SP_GetallCommunityTopic", con);
                    SqlCommand com = new SqlCommand(query, con);
                    // com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@CommunityID", CommunityID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OnlineCommunityTopic temp = new OnlineCommunityTopic();
                        temp.Topic_ID = Convert.ToInt64(reader["Topic_ID"]);
                        temp.UID_CreatedBY = Convert.ToInt64(reader["UID_CreatedBY"]);
                        temp.Community_Fk_ID = Convert.ToInt64(reader["Community_Fk_ID"]);
                        temp.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);
                        temp.Topic_CreatedDate = Convert.ToDateTime(reader["Topic_CreatedDate"]);
                        temp.CommunityName = Convert.ToString(reader["CommunityName"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.Topic_status = Convert.ToBoolean(reader["Topic_status"]);
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

        public bool CheckLikeOrDisLikeExists(long CommID, long UID)
        {
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "select UID,Comment_Fk_ID,Likes,Dislike from studentinfo.Online_Community_LikeORDisLike where Comment_Fk_ID=@CommID and UID=@UID";
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
                    string query = "select Likes from studentinfo.Online_Community_LikeORDisLike where Likes=1 and Comment_Fk_ID=@commID and UID=@UID";
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
                    string query = "select Likes from studentinfo.Online_Community_LikeORDisLike where DisLike=1 and Comment_Fk_ID=@commID and UID=@UID";
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

        //delete Community Comment grp comment sm//
        public string DeleteCommunityComment(long UID, long CommunityCommID)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"Delete from studentinfo.Online_Community_Comment
                                        where Community_Comment_ID=@CommID
                                        and UID=@UID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    cmd.Parameters.AddWithValue("@CommID", CommunityCommID);
                    //cmd.Parameters.AddWithValue("@Comment",)
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success! deleted";
                    }
                    else
                    {
                        res = "Failed!";

                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }

        //get details of online community comment SM 7th feb//
        public CommunityDetails GetDetailsOfCommunityComment(CommunityDetails Info)
        {

            CommunityDetails Result = new CommunityDetails();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select sd.UID,sd.Email,comm.Community_Comment_ID,Community_Comment_Text
                                    from admin.users as sd
                                    inner join studentinfo.Online_Community_Comment as comm
                                    On comm.UID=sd.UID
                                    where Community_Comment_ID=@CommentID";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@CommentID", Info.Community_Comment_ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {


                        Result.CommenterID = Convert.ToInt32(reader["UID"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Community_Comment_ID = Convert.ToInt32(reader["Community_Comment_ID"]);
                        Result.CommentText = Convert.ToString(reader["Community_Comment_Text"]);


                    }

                    con.Close();


                }

            }
            catch (Exception ex)
            {


            }
            return Result;
        }

        public String InsertCommunityReportInTable(CommunityDetails Info)
        {
            string Res = string.Empty;
            try
            {
                if (!(CheckReportExists(Info.Community_Comment_ID, Info.ReportBy)))
                {
                    using (SqlConnection connt = new SqlConnection(conn))
                    {
                        string Query = @" Insert into [site_admin].[CommentReport]
                                     (CommentText,Go2UniSection,CommentID,CommentById,CommentByMail,ReportReason,ReportedBy)
                                     values(@Comment,'OnlineCommunity',@CommID,@CommById,@CommByMail,@reason,@ReportedBy)";
                        SqlCommand cmd = new SqlCommand(Query, connt);

                        cmd.Parameters.AddWithValue("@Comment", Info.CommentText);

                        cmd.Parameters.AddWithValue("@CommID", Info.Community_Comment_ID);
                        cmd.Parameters.AddWithValue("@CommById", Info.CommenterID);
                        cmd.Parameters.AddWithValue("@CommByMail", Info.Email);
                        cmd.Parameters.AddWithValue("@reason", Info.ReportReason);
                        cmd.Parameters.AddWithValue("ReportedBy", Info.ReportBy);

                        connt.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Res = "Success ! Your Report is being registered to the GO2UNI admin ";
                        }
                        else
                        {
                            Res = "Failed ! to register report";
                        }
                        connt.Close();
                    }
                }
                else
                {
                    Res = " Failed ! You have already reported to admin against this comment";

                }
            }
            catch (Exception ex)
            {


            }

            return Res;

        }

        public bool CheckReportExists(long CommentID, long ReportBYId)
        {
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "select ReportId from [site_admin].CommentReport where CommentID=@CommID and ReportedBy=@ReportedBYID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CommID", CommentID);
                    cmd.Parameters.AddWithValue("@ReportedBYID", ReportBYId);



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

        //sp changed to query 24th dec,18// last changes 7th june
        public List<OnlineCommunityTopic> getDetailsOfComment(long TopicID)
        {
            List<OnlineCommunityTopic> Result = new List<OnlineCommunityTopic>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ct.*,sd.Stu_Name as CreatedBy  from studentinfo.Online_Community_Topic 
                                as ct left join
                                admin.users as sa on ct.UID_CreatedBY=sa.UID
                                INNER JOIN studentinfo.Student_Details as sd on sd.UserID_FK=sa.UID
                            where ct.Topic_ID=@Topic_ID";
                    // SqlCommand com = new SqlCommand("SP_GetallTopicDetailsByTopicID", con);
                    SqlCommand com = new SqlCommand(Query, con);
                    //  com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Topic_ID", TopicID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OnlineCommunityTopic temp = new OnlineCommunityTopic();
                        temp.Topic_ID = Convert.ToInt64(reader["Topic_ID"]);
                        temp.UID_CreatedBY = Convert.ToInt64(reader["UID_CreatedBY"]);
                        temp.Community_Fk_ID = Convert.ToInt64(reader["Community_Fk_ID"]);
                        temp.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);
                        temp.tempTopic_CreatedDate = Convert.ToString(reader["Topic_CreatedDate"]);
                        temp.Topic_status = Convert.ToBoolean(reader["Topic_status"]);
                        temp.CreatedBy = Convert.ToString(reader["CreatedBy"]);
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

        public OnlineCommunityTopic EditCommunityTopicName(long TopicID)
        {
            OnlineCommunityTopic res = new OnlineCommunityTopic();
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {

                    string Query = @"select Topic_ID,Topic_Discussion from studentinfo.Online_Community_Topic
                                    where Topic_ID=@Topic_ID";

                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@Topic_ID", TopicID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Topic_ID = Convert.ToInt64(reader["Topic_ID"]);
                        res.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);
                        break;
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }
            return res;
        }
        
        public String UpdateCommunityTopicName(OnlineCommunityTopic Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {

                    string Query = @"update studentinfo.Online_Community_Topic set Topic_Discussion=@topic where Topic_ID=@id and UID_CreatedBY=@uid";


                    SqlCommand cmd = new SqlCommand(Query, connt);

                    cmd.Parameters.AddWithValue("@id", Info.Topic_ID);
                    cmd.Parameters.AddWithValue("@topic", Info.Topic_Discussion);
                    cmd.Parameters.AddWithValue("@uid", Info.UID_CreatedBY);
                    connt.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {

                        Res = "Success|Topic name Sucessfuly Updated";
                    }
                    else
                    {
                        Res = "|Updation Failed";
                    }
                    connt.Close();




                }
            }
            catch (Exception ex)
            {

            }
            return Res;
        }
        
        public OnlineCommunityTopic RefreshCommunityTopicByUserID(long TopicID, long UID)
        {
            OnlineCommunityTopic res = new OnlineCommunityTopic();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select Topic_ID,Topic_Discussion from studentinfo.Online_Community_Topic 
                                    where Topic_ID=@topicid and UID_CreatedBY=@uid";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@topicid", TopicID);
                    com.Parameters.AddWithValue("@uid", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Topic_ID = Convert.ToInt64(reader["Topic_ID"]);
                        res.Topic_Discussion = Convert.ToString(reader["Topic_Discussion"]);

                    }
                }

            }
            catch (Exception ex)
            {


            }

            return res;
        }
               
        public String DeleteCommunityTopic(OnlineCommunityTopic Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_DeleteCommunityTopic", connt);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@topicid", Info.Topic_ID);

                    connt.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                        Res = "Success|Topic deleted";
                    }
                    else
                    {
                        Res = "Failed|Deletion Failed";
                    }
                    connt.Close();
                }
            }
            catch (Exception EX)
            {

                //throw;
            }

            return Res;
        }



        #endregion

        #region Study Guide

        public List<ViewGoal> GetCurrentStudyGuide(long ID)
        {
            List<ViewGoal> Result = new List<ViewGoal>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_CurrentStudyGuide", con);
                    com.Parameters.AddWithValue("@UserID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewGoal temp = new ViewGoal();
                        temp.ID = Convert.ToInt32(reader["ID"]);

                        temp.TempStartDate = Convert.ToString(reader["StartDate"]);
                        temp.TempEndDate = Convert.ToString(reader["EndDate"]);
                        temp.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        temp.ExamTopicID_FK = Convert.ToInt32(reader["ExamTopicID_FK"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.SubjectID = Convert.ToInt32(reader["SubjectID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.ExamID = Convert.ToInt32(reader["ExamID"]);
                        temp.ExamType = Convert.ToString(reader["ExamType"]);





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

        public List<ViewGoal> GetNotStartedStudyGuide(long ID)
        {
            List<ViewGoal> Result = new List<ViewGoal>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_NotStartedStudyGuide", con);
                    com.Parameters.AddWithValue("@UserID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewGoal temp = new ViewGoal();
                        temp.ID = Convert.ToInt32(reader["ID"]);

                        temp.TempStartDate = Convert.ToString(reader["StartDate"]);
                        temp.TempEndDate = Convert.ToString(reader["EndDate"]);
                        temp.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        temp.ExamTopicID_FK = Convert.ToInt32(reader["ExamTopicID_FK"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.SubjectID = Convert.ToInt32(reader["SubjectID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.ExamID = Convert.ToInt32(reader["ExamID"]);
                        temp.ExamType = Convert.ToString(reader["ExamType"]);





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

        public List<ViewGoal> GetCompletedStudyGuide(long ID)
        {
            List<ViewGoal> Result = new List<ViewGoal>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_CompletedStudyGuide", con);
                    com.Parameters.AddWithValue("@UserID", ID);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewGoal temp = new ViewGoal();
                        temp.ID = Convert.ToInt32(reader["ID"]);

                        temp.TempStartDate = Convert.ToString(reader["StartDate"]);
                        temp.TempEndDate = Convert.ToString(reader["EndDate"]);
                        temp.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        temp.ExamTopicID_FK = Convert.ToInt32(reader["ExamTopicID_FK"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.SubjectID = Convert.ToInt32(reader["SubjectID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.ExamID = Convert.ToInt32(reader["ExamID"]);
                        temp.ExamType = Convert.ToString(reader["ExamType"]);





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

        #endregion

        public string ProfileImgForSgTopicList(long UID)
        {
            GroupInfo Result = new GroupInfo();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Stu_Profile_Image,UserID_FK
                                        from studentinfo.Student_Details
                                        where UserID_FK=@UID";


                    SqlCommand com = new SqlCommand(Query, con);

                    // com.Parameters.AddWithValue("@GroupCommID", GroupCommID);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.ProfileImage = Convert.ToString(reader["Stu_Profile_Image"]);

                        Result.UID = Convert.ToInt64(reader["UserID_FK"]);
                        //  Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result.ProfileImage;
        }

        public string ProfileImgForSgCommentList(long CommentID)
        {
            TopicWiseComment Result = new TopicWiseComment();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select sd.Stu_Profile_Image
                                        from studentinfo.Student_Details as sd
                                        INNER JOIN studentinfo.Study_Group_Comment as sgc ON sd.UserID_FK=sgc.[UID(St)]
                                        where sgc.Group_Comment_ID=@CommID";


                    SqlCommand com = new SqlCommand(Query, con);

                    // com.Parameters.AddWithValue("@GroupCommID", GroupCommID);
                    com.Parameters.AddWithValue("@CommID", CommentID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);

                        Result.UserID_FK = Convert.ToInt64(reader["UserID_FK"]);


                        //  Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result.Stu_Profile_Image;
        }


        //24th,jan.19, SM
        public string GetGroupNameByGrpID(long Group_ID)
        {
            TopicWiseComment Result = new TopicWiseComment();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Group_Name from studentinfo.Study_Group where Group_ID=@GrpID";


                    SqlCommand com = new SqlCommand(Query, con);

                    // com.Parameters.AddWithValue("@GroupCommID", GroupCommID);
                    com.Parameters.AddWithValue("@GrpID", Group_ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.GroupName = Convert.ToString(reader["Group_Name"]);

                        // Result.UID = Convert.ToInt32(reader["UserID_FK"]);
                        //  Result.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result.GroupName;
        }

        #region Messages Ramashree 29/01/2019

        public List<FriendList> SocialFriendList(long UID)
        {
            List<FriendList> Result = new List<FriendList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select admin.users.IsOnline,ss.UserID_FK,ss.Stu_Name,ss.LastLoginTime,ss.Stu_Profile_Image                                            
                                        from admin.users
                                        inner join 
                                       (select top 100 percent UserID_FK,Stu_Name,LastLoginTime,Stu_Profile_Image from studentinfo.Student_Details
                                       left join admin.LiveMessageInfo on studentinfo.student_Details.UserID_FK=admin.LiveMessageInfo.SenderID
                                       where studentinfo.student_Details.LoginTpe_FK=8 and UserID_FK!=@UID
                                       group by studentinfo.student_Details.UserID_FK,Stu_Name,LastLoginTime,Stu_Profile_Image
                                       order by max(admin.LiveMessageInfo.CreatedDate)desc
                                       ) as ss
                                       on admin.users.UID=ss.UserID_FK order by admin.users.IsOnline desc;";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FriendList temp = new FriendList();
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.UserID = Convert.ToInt64(reader["UserID_FK"]);
                        temp.LastLoginTime = Convert.ToDateTime(reader["LastLoginTime"]);
                        temp.IsOnline = Convert.ToBoolean(reader["IsOnline"]);
                        temp.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
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


        public FriendList ViewFriend(long RecieverID)
        {
            FriendList Result = new FriendList();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ut.UID,st.Stu_Name from admin.Users as ut
                                     inner join studentinfo.Student_Details as st ON ut.UID=st.UserID_FK
                                     where ut.UserType_FK=8 and UID=@RecieverID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@RecieverID", RecieverID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.UserID = Convert.ToInt32(reader["UID"]);
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

        public string SendMessage(LiveMessageDetails Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    String Query = @"INSERT INTO [admin].[LiveMessageInfo](SenderID,ReceiverID,Message,IsRead)VALUES(@SenderID,@ReceiverID,@Message,0)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@SenderID", Info.SenderID);
                    com.Parameters.AddWithValue("@ReceiverID", Info.ReceiverID);
                    com.Parameters.AddWithValue("@Message", Info.Message);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Inserted";
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
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public List<LiveMessageDetails> ViewAllMesssageBySender(long SenderID, long ReceiverID)
        {
            List<LiveMessageDetails> Result = new List<LiveMessageDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select SenderID,ReceiverID,Message from admin.LiveMessageInfo where SenderID=@SenderID and ReceiverID=@ReceiverID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SenderID", SenderID);
                    cmd.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LiveMessageDetails temp = new LiveMessageDetails();
                        temp.SenderID = Convert.ToInt32(reader["SenderID"]);
                        temp.ReceiverID = Convert.ToInt32(reader["ReceiverID"]);
                        temp.Message = Convert.ToString(reader["Message"]);
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
        //------------------------------------------------------------------------------------------------------------------------------------------------

        public List<LiveMessageDetails> SendMesageBySender(long SenderID, long ReceiverID)
        {
            List<LiveMessageDetails> Result = new List<LiveMessageDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    //string Query1 = @"select SenderID,ReceiverID,Message,CreatedDate from admin.LiveMessageInfo where SenderID=@SenderID and ReceiverID=@ReceiverID";
                    string Query1 = @"select SenderID,ReceiverID,Message,LiveMessageInfo.CreatedDate,Stu_Profile_Image from admin.LiveMessageInfo
                                      inner join  studentinfo.Student_Details on 
                                      studentinfo.student_Details.UserID_FK=admin.LiveMessageInfo.SenderID where SenderID=@SenderID and ReceiverID=@ReceiverID";

                    SqlCommand com = new SqlCommand(Query1, con);
                    com.Parameters.AddWithValue("@SenderID", SenderID);
                    com.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        LiveMessageDetails temp = new LiveMessageDetails();
                        temp.SenderID = Convert.ToInt32(reader["SenderID"]);
                        temp.ReceiverID = Convert.ToInt32(reader["ReceiverID"]);
                        temp.Message = Convert.ToString(reader["Message"]);
                        temp.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
                        temp.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        temp.fromsender = true;
                        Result.Add(temp);
                    }
                    con.Close();


                    //string Query2 = @"select SenderID,ReceiverID,Message,CreatedDate from admin.LiveMessageInfo where SenderID=@ReceiverID and ReceiverID=@SenderID";
                    string Query2 = @"select SenderID,ReceiverID,Message,LiveMessageInfo.CreatedDate,Stu_Profile_Image from admin.LiveMessageInfo
                                      inner join  studentinfo.Student_Details on 
                                      studentinfo.student_Details.UserID_FK=admin.LiveMessageInfo.SenderID where SenderID=@ReceiverID and ReceiverID=@SenderID";
                    SqlCommand comm = new SqlCommand(Query2, con);
                    comm.Parameters.AddWithValue("@SenderID", SenderID);
                    comm.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                    con.Open();
                    SqlDataReader reader1 = comm.ExecuteReader();
                    while (reader1.Read())
                    {
                        LiveMessageDetails temp = new LiveMessageDetails();
                        temp.SenderID = Convert.ToInt32(reader1["SenderID"]);
                        temp.ReceiverID = Convert.ToInt32(reader1["ReceiverID"]);
                        temp.Message = Convert.ToString(reader1["Message"]);
                        temp.Stu_Profile_Image = Convert.ToString(reader1["Stu_Profile_Image"]);
                        temp.CreatedDate = Convert.ToDateTime(reader1["CreatedDate"]);
                        temp.fromsender = false;
                        Result.Add(temp);
                    }
                    con.Close();


                    string Query3 = @"update admin.LiveMessageInfo set IsRead=1 where ReceiverID=@ReceiverID";
                    SqlCommand commm = new SqlCommand(Query3, con);
                    commm.Parameters.AddWithValue("@ReceiverID", SenderID);
                    con.Open();
                    if (commm.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.OrderByDescending(d => d.CreatedDate).ToList();
        }

        public List<FriendList> FriendListRefresh(long UID)
        {
            List<FriendList> Result = new List<FriendList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select admin.users.IsOnline,ss.UserID_FK,ss.Stu_Name,ss.LastLoginTime,ss.Stu_Profile_Image                                            
                                        from admin.users
                                        inner join 
                                       (select top 100 percent UserID_FK,Stu_Name,LastLoginTime,Stu_Profile_Image from studentinfo.Student_Details
                                       left join admin.LiveMessageInfo on studentinfo.student_Details.UserID_FK=admin.LiveMessageInfo.SenderID
                                       where studentinfo.student_Details.LoginTpe_FK=8 and UserID_FK!=@UID
                                       group by studentinfo.student_Details.UserID_FK,Stu_Name,LastLoginTime,Stu_Profile_Image
                                       order by max(admin.LiveMessageInfo.CreatedDate)desc
                                       ) as ss
                                       on admin.users.UID=ss.UserID_FK order by admin.users.IsOnline desc";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FriendList temp = new FriendList();
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.UserID = Convert.ToInt64(reader["UserID_FK"]);
                        temp.LastLoginTime = Convert.ToDateTime(reader["LastLoginTime"]);
                        temp.IsOnline = Convert.ToBoolean(reader["IsOnline"]);
                        temp.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
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

        public string UpdateOnlineStatus(long UID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    String Query = @"update admin.users set IsOnline=0 where UID=@UID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success";
                    }
                    else
                    {
                        Result = "Failed";
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

        #endregion

        //#region Event Details by Indranil 29/01/2018

        //public ViewEvent getEventDetails(long id)
        //{
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    {
        //        ViewEvent EvObj = new ViewEvent();
        //        string Query = @"select * from collegeEvents.Events where ID=@ID";
        //        SqlCommand command = new SqlCommand(Query, con);
        //        command.Parameters.AddWithValue("@ID", id);
        //        if (con.State == System.Data.ConnectionState.Broken ||
        //            con.State == System.Data.ConnectionState.Closed)
        //        {
        //            con.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            try
        //            {
        //                while (reader.Read())
        //                {
        //                    EvObj.ID = Convert.ToInt64(reader["ID"].ToString());
        //                    EvObj.EventVideo = Convert.ToString(reader["EventVideo"]);
        //                    EvObj.Event = Convert.ToString(reader["Event"].ToString());
        //                    EvObj.EventDate = Convert.ToDateTime(reader["EventDate"].ToString());
        //                    EvObj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
        //                    //EvObj.IsActive = Convert.ToInt16(reader["IsActive"].ToString());
        //                    EvObj.Title = Convert.ToString(reader["Title"]);
        //                    EvObj.CreatedBy = Convert.ToString(reader["CreatedBy"]);
        //                    EvObj.Comments = getAllMessages(Convert.ToInt64(reader["ID"].ToString()));
        //                    break;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }
        //        return EvObj;
        //    }
        //}
        //public bool InsertEventsComment(EventComments comment)
        //{
        //    bool req = false;
        //    string Query = @"insert into collegeEvents.EventComments(CreatedDate,Comments,Userid,Eventid)
        //                            values(getDate(),@comment,@userid,@eventid)";
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand(Query, con);
        //        command.Parameters.AddWithValue("@comment", comment.Comments);
        //        command.Parameters.AddWithValue("@userid", comment.Userid);
        //        command.Parameters.AddWithValue("@eventid", comment.Eventid);
        //        if (con.State == System.Data.ConnectionState.Broken ||
        //        con.State == System.Data.ConnectionState.Closed)
        //        {
        //            con.Open();
        //            try
        //            {
        //                if (command.ExecuteNonQuery() > 0)
        //                {
        //                    req = true;
        //                }
        //                else
        //                {
        //                    req = false;
        //                }
        //            }
        //            catch (SqlException ex) { Console.WriteLine(ex.Message); }
        //        }
        //    }
        //    return req;
        //}

        //private static List<EventComments> getAllMessages(long id)
        //{
        //    List<EventComments> comments = new List<EventComments>();
        //    string Query = @"select collegeEvents.EventComments.ID,collegeEvents.EventComments.Eventid,collegeEvents.EventComments.Comments,
        //                   collegeEvents.EventComments.CreatedDate,collegeEvents.EventComments.Userid,collegeEvents.EventComments.IsReported,studentinfo.Student_Details.Stu_Profile_Image,studentinfo.Student_Details.Stu_Name
        //                   from collegeEvents.EventComments 
        //                   inner Join studentinfo.Student_Details
        //                   on collegeEvents.EventComments.Userid=studentinfo.Student_Details.UserID_FK
        //                   where collegeEvents.EventComments.Eventid=@eventid and collegeEvents.EventComments.IsDeleted=0
        //                   order by collegeEvents.EventComments.ID desc";
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand(Query, con);
        //        command.Parameters.AddWithValue("@eventid", id);
        //        if (con.State == System.Data.ConnectionState.Broken ||
        //            con.State == System.Data.ConnectionState.Closed)
        //        {

        //            con.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                EventComments temp = new EventComments()
        //                {
        //                    ID = Convert.ToInt64(reader["ID"].ToString()),
        //                    Userid = Convert.ToInt64(reader["Userid"].ToString()),
        //                    Eventid = Convert.ToInt64(reader["Eventid"].ToString()),
        //                    Comments = Convert.ToString(reader["Comments"]),
        //                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
        //                    Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]),
        //                    Stu_Name = Convert.ToString(reader["Stu_Name"]),
        //                    IsReported = Convert.ToBoolean(reader["IsReported"])

        //                };
        //                comments.Add(temp);
        //            }
        //        }
        //    }
        //    return comments;
        //}

        //public bool UpdateCommentByEventId(EventComments comments)
        //{
        //    bool req = false;
        //    string Query = @"update collegeEvents.EventComments set Comments=@comment where ID=@ID";
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand(Query, con);
        //        command.Parameters.AddWithValue("@comment", comments.Comments);
        //        command.Parameters.AddWithValue("@ID", comments.ID);
        //        if (con.State == System.Data.ConnectionState.Broken ||
        //            con.State == System.Data.ConnectionState.Closed)
        //        {
        //            con.Open();
        //            if (command.ExecuteNonQuery() > 0)
        //            {
        //                req = true;
        //            }
        //            else
        //            {
        //                req = false;
        //            }
        //        }
        //    }
        //    return req;
        //}

        //public bool DeleteCommentByMsgId(long id)
        //{
        //    bool status = false;
        //    string Query = @"delete from collegeEvents.EventComments where ID=@ID";
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand(Query, con);
        //        command.Parameters.AddWithValue("@ID", id);
        //        if (con.State == System.Data.ConnectionState.Broken ||
        //            con.State == System.Data.ConnectionState.Closed)
        //        {

        //            con.Open();
        //            if (command.ExecuteNonQuery() > 0)
        //            {
        //                status = true;
        //            }
        //            else
        //            {
        //                status = false;
        //            }
        //        }
        //    }
        //    return status;
        //}

        ////get comment details for EVENT, SM//
        //public EventComments EventCommentDetails(EventComments Info)
        //{
        //    EventComments Result = new EventComments();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        {

        //            string query = @"select usr.UID,usr.Email,evnt.Comments,evnt.ID,evnt.Userid
        //                            from admin.users as usr
        //                            inner join collegeEvents.EventComments as evnt
        //                            on usr.UID=evnt.Userid
        //                            where evnt.ID=@commid";
        //            SqlCommand com = new SqlCommand(query, con);
        //            com.Parameters.AddWithValue("@commid", Info.ID);
        //            con.Open();
        //            SqlDataReader reader = com.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                Result.Userid = Convert.ToInt64(reader["Userid"]);
        //                Result.EmailID = Convert.ToString(reader["Email"]);
        //                Result.Comments = Convert.ToString(reader["Comments"]);
        //                Result.ID = Convert.ToInt64(reader["ID"]);
        //                //Result.ReportBy = Convert.ToInt64(reader["UID"]);
        //            }

        //            con.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return Result;
        //}

        //// event register report SM//
        //public string InsertEventReportInTable(EventComments Info)
        //{
        //    string Res = string.Empty;
        //    try
        //    {
        //        if (!(CheckReportExists(Info.ID, Info.ReportBy)))
        //        {
        //            using (SqlConnection connt = new SqlConnection(conn))
        //            {
        //                string Query = @" Insert into [site_admin].[CommentReport]
        //                             (CommentText,Go2UniSection,CommentID,CommentById,CommentByMail,ReportReason,ReportedBy)
        //                             values(@Comment,'Event',@CommID,@CommById,@CommByMail,@reason,@ReportedBy)";
        //                SqlCommand cmd = new SqlCommand(Query, connt);

        //                cmd.Parameters.AddWithValue("@Comment", Info.Comments);

        //                cmd.Parameters.AddWithValue("@CommID", Info.ID);
        //                cmd.Parameters.AddWithValue("@CommById", Info.Userid);
        //                cmd.Parameters.AddWithValue("@CommByMail", Info.EmailID);
        //                cmd.Parameters.AddWithValue("@reason", Info.ReportReason);
        //                cmd.Parameters.AddWithValue("ReportedBy", Info.ReportBy);

        //                connt.Open();
        //                if (cmd.ExecuteNonQuery() > 0)
        //                {
        //                    Res = "Success! Your Report is being registered to the Go2uni admin";
        //                }
        //                else
        //                {
        //                    Res = "Failed! to register report";
        //                }
        //                connt.Close();
        //            }
        //        }
        //        else
        //        {
        //            Res = "Failed ! Your have already reported to admin against this comment";
        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }

        //    return Res;

        //}
        //#endregion

        #region Event Details by Indranil 29/01/2018

        public ViewEvent getEventDetails(long id)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                ViewEvent EvObj = new ViewEvent();
                string Query = @"select ev.ID,isnull(ev.Title,'') Title,isnull(ev.Event,'') Event,
                                isnull(convert(nvarchar,ev.EventDate,121),'') EventDate,
                                isnull(convert(nvarchar,ev.CreatedDate,121),'') CreatedDate,
                                isnull(ev.EventVideo,'') EventVideo from collegeEvents.Events ev 
                                where ev.IsActive=1 and ID=@ID";

                SqlCommand command = new SqlCommand(Query, con);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    if (con.State == ConnectionState.Broken ||
                            con.State == ConnectionState.Closed)
                    {

                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EvObj.ID = Convert.ToInt64(reader["ID"].ToString());
                                EvObj.Title = Convert.ToString(reader["Title"]);
                                EvObj.Event = Convert.ToString(reader["Event"].ToString());
                                EvObj.EventDate = Convert.ToString(reader["EventDate"].ToString());
                                EvObj.CreatedDate = Convert.ToString(reader["CreatedDate"].ToString());
                                EvObj.EventVideo = Convert.ToString(reader["EventVideo"]);
                                EvObj.Comments = getAllMessages(Convert.ToInt64(reader["ID"].ToString()));
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return EvObj;
            }
        }
        public bool InsertEventsComment(EventComments comment)
        {
            bool req = false;
            string Query = @"insert into collegeEvents.EventComments(CreatedDate,Comments,Userid,Eventid,UserTypeID)
                                    values(getDate(),@comment,@userid,@eventid,@UserTypeID)";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(Query, con);
                command.Parameters.AddWithValue("@comment", comment.Comments);
                command.Parameters.AddWithValue("@userid", comment.Userid);
                command.Parameters.AddWithValue("@eventid", comment.Eventid);
                command.Parameters.AddWithValue("@UserTypeID", comment.UserTypeID);

                try
                {
                    if (con.State == ConnectionState.Broken ||
                            con.State == ConnectionState.Closed)
                    {

                        con.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            req = true;
                        }
                        else
                        {
                            req = false;
                        }
                    }
                }
                catch (SqlException ex)
                {

                }
            }
            return req;
        }
        private List<EventComments> getAllMessages(long id)
        {
            List<EventComments> comments = new List<EventComments>();
            string Query = @"select ss.ID,ss.Eventid,ss.Comments,
	            (case when ss.UserTypeID=8 or ss.UserTypeID=4 then (select isnull(sd.Stu_Name,'')
		            from studentinfo.Student_Details sd where sd.UserID_FK=ss.Userid) 
			            else(select isnull(td.Name,'') Name from teacher.TeachersDetails td where td.UID_FK=ss.Userid) end) as Name,
	 
	             (case when ss.UserTypeID=8 or ss.UserTypeID=4 then (select isnull(sd.Stu_Profile_Image,'') 
		            from studentinfo.Student_Details sd where sd.UserID_FK=ss.Userid) 
			            else(select isnull(td.Profile_Image,'') from teacher.TeachersDetails td where td.UID_FK=ss.Userid) end) as ProfileImage 
	             ,ss.CreatedDate,ss.Userid,ss.UserTypeID,ss.IsReported from(
            select collegeEvents.EventComments.ID,collegeEvents.EventComments.Userid,
            collegeEvents.EventComments.Eventid,collegeEvents.EventComments.IsReported,
            isnull(convert(nvarchar,collegeEvents.EventComments.CreatedDate,121),'') CreatedDate,
            collegeEvents.EventComments.UserTypeID,
            isnull(collegeEvents.EventComments.Comments,'') Comments
            from collegeEvents.EventComments where collegeEvents.EventComments.Eventid=@eventid and 
            collegeEvents.EventComments.Status=1
            ) as ss order by ss.ID desc";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(Query, con);
                command.Parameters.AddWithValue("@eventid", id);
                try
                {
                    if (con.State == ConnectionState.Broken ||
                            con.State == ConnectionState.Closed)
                    {

                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EventComments temp = new EventComments()
                                {
                                    ID = Convert.ToInt64(reader["ID"].ToString()),
                                    Userid = Convert.ToInt64(reader["Userid"].ToString()),
                                    Eventid = Convert.ToInt64(reader["Eventid"].ToString()),
                                    Comments = Convert.ToString(reader["Comments"]),
                                    CreatedDate = Convert.ToString(reader["CreatedDate"]),
                                    Stu_Profile_Image = Convert.ToString(reader["ProfileImage"]),
                                    Stu_Name = Convert.ToString(reader["Name"]),
                                    IsReported = Convert.ToBoolean(reader["IsReported"])
                                };
                                comments.Add(temp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return comments;
        }
        public bool UpdateCommentByEventId(EventComments comments)
        {

            bool req = false;
            string Query = @"update collegeEvents.EventComments set Comments=@comment 
                                where collegeEvents.EventComments.Status=1 and collegeEvents.EventComments.ID=@ID";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@comment", comments.Comments);
                    command.Parameters.AddWithValue("@ID", comments.ID);
                    if (con.State == ConnectionState.Broken ||
                            con.State == ConnectionState.Closed)
                    {

                        con.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            req = true;
                        }
                        else
                        {
                            req = false;
                        }
                    }
                }
                catch (Exception ex) { }

            }
            return req;
        }
        public bool DeleteCommentByMsgId(long id)
        {
            bool status = false;
            string Query = @"delete from collegeEvents.EventComments where collegeEvents.EventComments.Status=1 
                                and collegeEvents.EventComments.ID=@ID";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", id);
                    if (con.State == ConnectionState.Broken ||
                            con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
                catch (Exception ex) { }

            }
            return status;
        }
        
        //get comment details for EVENT, SM//
        public EventComments EventCommentDetails(EventComments Info)
        {
            EventComments Result = new EventComments();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select usr.UID,usr.Email,evnt.Comments,evnt.ID,evnt.Userid
                                    from admin.users as usr
                                    inner join collegeEvents.EventComments as evnt
                                    on usr.UID=evnt.Userid
                                    where evnt.ID=@commid";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@commid", Info.ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.Userid = Convert.ToInt64(reader["Userid"]);
                        Result.EmailID = Convert.ToString(reader["Email"]);
                        Result.Comments = Convert.ToString(reader["Comments"]);
                        Result.ID = Convert.ToInt64(reader["ID"]);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }
            return Result;
        }

        // event register report SM//
        public string InsertEventReportInTable(EventComments Info)
        {
            string Res = string.Empty;
            try
            {
                if (!(CheckReportExists(Info.ID, Info.ReportBy)))
                {
                    using (SqlConnection connt = new SqlConnection(conn))
                    {
                        string Query = @" Insert into [site_admin].[CommentReport]
                                     (CommentText,Go2UniSection,CommentID,CommentById,CommentByMail,ReportReason,ReportedBy)
                                     values(@Comment,'Event',@CommID,@CommById,@CommByMail,@reason,@ReportedBy)";
                        SqlCommand cmd = new SqlCommand(Query, connt);

                        cmd.Parameters.AddWithValue("@Comment", Info.Comments);

                        cmd.Parameters.AddWithValue("@CommID", Info.ID);
                        cmd.Parameters.AddWithValue("@CommById", Info.Userid);
                        cmd.Parameters.AddWithValue("@CommByMail", Info.EmailID);
                        cmd.Parameters.AddWithValue("@reason", Info.ReportReason);
                        cmd.Parameters.AddWithValue("ReportedBy", Info.ReportBy);

                        connt.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Res = "Success! Your Report is being registered to the Go2uni admin";
                        }
                        else
                        {
                            Res = "Failed! to register report";
                        }
                        connt.Close();
                    }
                }
                else
                {
                    Res = "Failed ! Your have already reported to admin against this comment";
                }
            }
            catch (Exception ex)
            {

            }
            return Res;
        }
        #endregion

        public List<CollegeDay> collegeForRightSide()
        {
            List<CollegeDay> Result = new List<CollegeDay>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllCollegeforRightside", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CollegeDay temp = new CollegeDay();
                        temp.ID = Convert.ToInt16(reader["NxtCollege_ID"]);
                        temp.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                        temp.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);
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

        public long checkforNewSocialMessages(long SenderId, long ReceiverId)
        {
            long Count = 0;
            string Query = @"select count(admin.LiveMessageInfo.Message) MessageCount from admin.LiveMessageInfo where
                                admin.LiveMessageInfo.SenderID=@SenderId and admin.LiveMessageInfo.ReceiverID=@ReceiverId
			or admin.LiveMessageInfo.ReceiverID=@SenderId and admin.LiveMessageInfo.SenderID=@ReceiverId";
            using (SqlConnection con = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(Query, con);
                command.Parameters.AddWithValue("@SenderId", SenderId);
                command.Parameters.AddWithValue("@ReceiverId", ReceiverId);
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Count = Convert.ToInt64(reader["MessageCount"]);
                }
            }
            return Count;
        }

        public SocialOrSiteAdminUserlastMessage getSocialIdentityMessage(long SenderID, long ReceiverID)
        {
            SocialOrSiteAdminUserlastMessage message = new SocialOrSiteAdminUserlastMessage();
            string Query = @"select top(1) studentinfo.Student_Details.Stu_Name,admin.LiveMessageInfo.ID,
                            admin.LiveMessageInfo.SenderID,
                            admin.LiveMessageInfo.ReceiverID,
                            ISNULL(admin.LiveMessageInfo.Message,'') Message,
                            CONVERT(varchar,admin.LiveMessageInfo.CreatedDate,121) CreatedDate,
                            ISNULL(studentinfo.Student_Details.Stu_Profile_Image,'') ProfileImage,admin.LiveMessageInfo.IsRead
                            from admin.LiveMessageInfo left join studentinfo.Student_Details on
                            admin.LiveMessageInfo.SenderID=studentinfo.Student_Details.UserID_FK
                            where admin.LiveMessageInfo.IsDelete=0 and
                            admin.LiveMessageInfo.SenderID=@SenderID and admin.LiveMessageInfo.ReceiverID=@ReceiverID or
                            admin.LiveMessageInfo.SenderID=@ReceiverID and admin.LiveMessageInfo.ReceiverID=@SenderID
                            order by admin.LiveMessageInfo.CreatedDate desc";
            using (SqlConnection con = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(Query, con);
                command.Parameters.AddWithValue("@SenderID", SenderID);
                command.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    message.MessageId = Convert.ToInt64(reader["ID"].ToString());
                    message.SenderName = Convert.ToString(reader["Stu_Name"].ToString());
                    message.SenderId = Convert.ToInt64(reader["SenderID"].ToString());
                    message.ReceiverId = Convert.ToInt64(reader["ReceiverID"].ToString());
                    message.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    message.MessageText = Convert.ToString(reader["Message"]);
                    message.IsRead = Convert.ToBoolean(reader["IsRead"]);
                    message.ProfileImage = Convert.ToString(reader["ProfileImage"]);
                }
            }
            return message;
        }

        #region Syllabus Status Ramashree

        public string GetSubjectforContent(long SubjectID)
        {
            Syllabus_Subject res = new Syllabus_Subject();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select Subject from studentinfo.Examsubjects where ID=@ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", SubjectID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Subject = Convert.ToString(reader["Subject"]);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {


            }
            return res.Subject;
        }

        public string InsertOnGoing(SyllabusStatus Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"insert into studentinfo.SyllabusStatus(UserID_FK,TopicContentID,Status_OnGoing,SubjectID)
                                    values(@UserID_FK,@TopicContentID,1,@SubjectID)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID_FK", Data.UserID_FK);
                    com.Parameters.AddWithValue("@TopicContentID", Data.TopicContentID);
                    com.Parameters.AddWithValue("@SubjectID", Data.SubjectID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Inserted |" + Data.SubjectID;

                    }
                    else
                    {
                        Result = "Failed| Failed Insertion";
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

        public string StatusChange(long UID, long TopicContentID)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"update studentinfo.SyllabusStatus set Status_Completed=1,Status_OnGoing=0 where TopicContentID=@TopicContentID and UserID_FK=@UserID_FK";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID_FK", UID);
                    cmd.Parameters.AddWithValue("@TopicContentID", TopicContentID);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        res = "Success!updated";
                    }
                    else
                    {
                        res = "Failed!";
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return res;
        }

        public long GetSubjectID(long TopicContentID)
        {
            long result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string query = @"select SubjectID from studentinfo.ExamContents where ID=@ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", TopicContentID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = Convert.ToInt64(dt.Rows[0][0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        #endregion

        public List<Review_SocialStudent> GetReviewforTopicWiseExam(string GUID,long UserID)
        {
            List<Review_SocialStudent> Result = new List<Review_SocialStudent>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select admin.StudentAnswerByTopic.ID, admin.StudentAnswerByTopic.Answer,admin.StudentAnswerByTopic.UID,admin.StudentAnswerByTopic.QuestionID,
admin.StudentAnswerByTopic.IsCorrect,admin.StudentAnswerByTopic.GUID,
admin.QuestionsByTopic.Question,admin.QuestionsByTopic.Option1,admin.QuestionsByTopic.Option2,admin.QuestionsByTopic.Option3,admin.QuestionsByTopic.Option4,
admin.QuestionsByTopic.Option5,admin.QuestionsByTopic.CorrectOption,admin.QuestionsByTopic.QuestionTypeID,admin.QuestionsByTopic.Diagram
from admin.StudentAnswerByTopic left join admin.QuestionsByTopic on admin.StudentAnswerByTopic.QuestionID=admin.QuestionsByTopic.ID
where admin.StudentAnswerByTopic.IsCorrect=0 and admin.StudentAnswerByTopic.UID=@UID and admin.StudentAnswerByTopic.GUID=@GUID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@GUID",GUID);
                    cmd.Parameters.AddWithValue("@UID", UserID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Review_SocialStudent temp = new Review_SocialStudent();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.Answer = Convert.ToString(reader["Answer"]);
                        temp.UID = Convert.ToInt64(reader["UID"]);
                        temp.QuestionID = Convert.ToInt64(reader["QuestionID"]);
                        temp.IsCorrect = Convert.ToBoolean(reader["IsCorrect"]);
                        temp.GUID = Convert.ToString(reader["GUID"]);
                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.Option1 = Convert.ToString(reader["Option1"]);
                        temp.Option2 = Convert.ToString(reader["Option2"]);
                        temp.Option3 = Convert.ToString(reader["Option3"]);
                        temp.Option4 = Convert.ToString(reader["Option4"]);
                        temp.Option5 = Convert.ToString(reader["Option5"]);
                        temp.CorrectOption = Convert.ToString(reader["CorrectOption"]);
                        temp.QuestionTypeID = Convert.ToInt64(reader["QuestionTypeID"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);

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

        public List<Review_SocialStudent> GetReviewforYearcWiseExam(string GUID, long UserID)
        {
            List<Review_SocialStudent> Result = new List<Review_SocialStudent>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select admin.StudentAnswerByYear.ID, admin.StudentAnswerByYear.Answer,admin.StudentAnswerByYear.UID,admin.StudentAnswerByYear.QuestionID,
admin.StudentAnswerByYear.IsCorrect,admin.StudentAnswerByYear.GUID,
admin.QuestionsByYear.Question,admin.QuestionsByYear.Option1,admin.QuestionsByYear.Option2,admin.QuestionsByYear.Option3,admin.QuestionsByYear.Option4,
admin.QuestionsByYear.Option5,admin.QuestionsByYear.CorrectOption,admin.QuestionsByYear.QuestionTypeID,admin.QuestionsByYear.Diagram
from admin.StudentAnswerByYear left join admin.QuestionsByYear on admin.StudentAnswerByYear.QuestionID=admin.QuestionsByYear.ID
where admin.StudentAnswerByYear.IsCorrect=0 and admin.StudentAnswerByYear.UID=@UID and admin.StudentAnswerByYear.GUID=@GUID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@GUID", GUID);
                    cmd.Parameters.AddWithValue("@UID", UserID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Review_SocialStudent temp = new Review_SocialStudent();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.Answer = Convert.ToString(reader["Answer"]);
                        temp.UID = Convert.ToInt64(reader["UID"]);
                        temp.QuestionID = Convert.ToInt64(reader["QuestionID"]);
                        temp.IsCorrect = Convert.ToBoolean(reader["IsCorrect"]);
                        temp.GUID = Convert.ToString(reader["GUID"]);
                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.Option1 = Convert.ToString(reader["Option1"]);
                        temp.Option2 = Convert.ToString(reader["Option2"]);
                        temp.Option3 = Convert.ToString(reader["Option3"]);
                        temp.Option4 = Convert.ToString(reader["Option4"]);
                        temp.Option5 = Convert.ToString(reader["Option5"]);
                        temp.CorrectOption = Convert.ToString(reader["CorrectOption"]);
                        temp.QuestionTypeID = Convert.ToInt64(reader["QuestionTypeID"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);

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

        public List<Review_SocialStudent> ViewAllAnswerTopicWise(long SubjectID,long TopicID,string GUID)
        {
            List<Review_SocialStudent> Result = new List<Review_SocialStudent>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select admin.QuestionsByTopic.ID,admin.QuestionsByTopic.Question,ISnull(admin.QuestionsByTopic.Diagram,'')as Diagram, 
admin.QuestionsByTopic.Option1,admin.QuestionsByTopic.Option2,admin.QuestionsByTopic.Option3, 
admin.QuestionsByTopic.Option4,admin.QuestionsByTopic.Option5,admin.QuestionsByTopic.CorrectOption,admin.QuestionsByTopic.QuestionTypeID,
 ISnull(admin.StudentAnswerByTopic.Answer,'NONE') as Answer, ISnull(admin.StudentAnswerByTopic.GUID,'')as GUID,isnull(admin.StudentAnswerByTopic.IsCorrect,'')as IsCorrect 
from admin.QuestionsByTopic left join admin.StudentAnswerByTopic on admin.StudentAnswerByTopic.QuestionID=admin.QuestionsByTopic.ID 
 and admin.StudentAnswerByTopic.GUID=@GUID
where admin.QuestionsByTopic.SubjectID=@SID and admin.QuestionsByTopic.TopicID=@TID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SID", SubjectID);
                    cmd.Parameters.AddWithValue("@TID", TopicID);
                    cmd.Parameters.AddWithValue("@GUID", GUID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Review_SocialStudent temp = new Review_SocialStudent();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                      
                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.Option1 = Convert.ToString(reader["Option1"]);
                        temp.Option2 = Convert.ToString(reader["Option2"]);
                        temp.Option3 = Convert.ToString(reader["Option3"]);
                        temp.Option4 = Convert.ToString(reader["Option4"]);
                        temp.Option5 = Convert.ToString(reader["Option5"]);
                        temp.CorrectOption = Convert.ToString(reader["CorrectOption"]);
                        temp.QuestionTypeID = Convert.ToInt64(reader["QuestionTypeID"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);
                        temp.Answer = Convert.ToString(reader["Answer"]);
                        temp.GUID = Convert.ToString(reader["GUID"]);
                        temp.IsCorrect = Convert.ToBoolean(reader["IsCorrect"]);

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

        public List<Review_SocialStudent> ViewAllAnswerYearWise(long SubjectID, string year,string GUID)
        {
            List<Review_SocialStudent> Result = new List<Review_SocialStudent>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select admin.QuestionsByYear.ID,admin.QuestionsByYear.Question,ISnull(admin.QuestionsByYear.Diagram,'')as Diagram, 
admin.QuestionsByYear.Option1,admin.QuestionsByYear.Option2,admin.QuestionsByYear.Option3, 
admin.QuestionsByYear.Option4,admin.QuestionsByYear.Option5,admin.QuestionsByYear.CorrectOption,admin.QuestionsByYear.QuestionTypeID,
 ISnull(admin.StudentAnswerByYear.Answer,'NONE') as Answer, ISnull(admin.StudentAnswerByYear.GUID,'')as GUID,isnull(admin.StudentAnswerByYear.IsCorrect,'') as IsCorrect 
from admin.QuestionsByYear left join admin.StudentAnswerByYear on admin.StudentAnswerByYear.QuestionID=admin.QuestionsByYear.ID 
 and admin.StudentAnswerByYear.GUID=@GUID
where admin.QuestionsByYear.SubjectID=@SID and admin.QuestionsByYear.Year=@year";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SID", SubjectID);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@GUID", GUID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Review_SocialStudent temp = new Review_SocialStudent();
                        temp.ID = Convert.ToInt64(reader["ID"]);

                        temp.Question = Convert.ToString(reader["Question"]);
                        temp.Option1 = Convert.ToString(reader["Option1"]);
                        temp.Option2 = Convert.ToString(reader["Option2"]);
                        temp.Option3 = Convert.ToString(reader["Option3"]);
                        temp.Option4 = Convert.ToString(reader["Option4"]);
                        temp.Option5 = Convert.ToString(reader["Option5"]);
                        temp.CorrectOption = Convert.ToString(reader["CorrectOption"]);
                        temp.QuestionTypeID = Convert.ToInt64(reader["QuestionTypeID"]);
                        temp.Diagram = Convert.ToString(reader["Diagram"]);
                        temp.Answer = Convert.ToString(reader["Answer"]);
                        temp.GUID = Convert.ToString(reader["GUID"]);
                        temp.IsCorrect = Convert.ToBoolean(reader["IsCorrect"]);

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

    }
}