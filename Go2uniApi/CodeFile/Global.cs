using Go2uniApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Go2uniApi.CodeFile
{
    public class Global
    {
        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();

        public List<Newsfeed> ShowAllNewsfeed()
        {
            List<Newsfeed> obj = new List<Newsfeed>();
            using (SqlConnection con = new SqlConnection(conn))
            {
                string Query = @"select admin.Newsfeed.ID,Isnull(admin.Newsfeed.Title,'') as Title,Isnull(admin.Newsfeed.Description,'') as Description,
                                    Isnull(admin.Newsfeed.EventImage,'') as EventImage,convert(varchar,admin.Newsfeed.CreatedDate,121) as CreatedDate,Status 
                                    from admin.Newsfeed where admin.Newsfeed.Status=1 order by admin.Newsfeed.CreatedDate desc";
                try
                {
                    SqlCommand command = new SqlCommand(Query, con);

                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Newsfeed tempfeed = new Newsfeed()
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                Title = Convert.ToString(reader["Title"]),
                                Description = Convert.ToString(reader["Description"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                EventImage = Convert.ToString(reader["EventImage"]),
                                Status = Convert.ToBoolean(reader["Status"]),

                            }; obj.Add(tempfeed);
                        }
                    }
                }
                catch (SqlException exc) { Console.WriteLine(exc.Message); }
            }
            return obj;
        }

        #region  Next College Day and event 
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
                        temp.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);
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
        public List<CollegeDay> GetAllNextCollegeUpcoming()
        {
            List<CollegeDay> Result = new List<CollegeDay>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllNextCollegeUpcoming", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CollegeDay temp = new CollegeDay();
                        temp.ID = Convert.ToInt16(reader["NxtCollege_ID"]);
                        temp.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                        temp.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);
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

        public List<CollegeDay> GetAllNextCollegePast()
        {
            List<CollegeDay> Result = new List<CollegeDay>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllNextCollegePast", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CollegeDay temp = new CollegeDay();
                        temp.ID = Convert.ToInt16(reader["NxtCollege_ID"]);
                        temp.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                        temp.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);
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

        public List<Event> GetAllEventUpcoming()
        {
            List<Event> Result = new List<Event>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllEventUpcoming", con);
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


        public List<Event> GetAllEventCurrent()
        {
            List<Event> Result = new List<Event>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllEventCurrent", con);
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

        public List<Event> GetEventPastData()
        {
            List<Event> Result = new List<Event>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllEventPast", con);
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

        public List<NextCollegeDayComment> GetCollegeComments(long id)
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


        public Event GetEventDetailsByID(long id)
        {
            Event Result = new Event();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetEventDetails", con);
                    com.Parameters.AddWithValue("@id", id);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.ID = Convert.ToInt16(reader["ID"]);
                        Result.Title = Convert.ToString(reader["Title"]);
                        Result.EventDate = Convert.ToDateTime(reader["EventDate"]);
                        Result.IsActive = Convert.ToBoolean(reader["IsActive"]);
                        Result.EventData = Convert.ToString(reader["EventData"]);
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

        public List<EventComment> GetEventComments(long id)
        {
            List<EventComment> Result = new List<EventComment>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetAllEventCommentByEventID", con);
                    com.Parameters.AddWithValue("@EventID", id);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        EventComment temp = new EventComment();
                        temp.CommentID = Convert.ToInt16(reader["ID"]);
                        temp.Text = Convert.ToString(reader["Text"]);
                        temp.CreatedDate = Convert.ToDateTime(reader["InsertedDate"]);
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

        public string InsertCollegeDayComment(NextCollegeDayComment Info)
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
                    if (com.ExecuteNonQuery() > 0)
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

        public string InsertEventComment(EventComment Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_InsertEventComment", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Text", Info.Text);
                    com.Parameters.AddWithValue("@StudentID", Info.StudentID);
                    com.Parameters.AddWithValue("@TeacherID", Info.TeacherID);
                    com.Parameters.AddWithValue("@EventID", Info.EventID);
                    com.Parameters.Add("@Commentid", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
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

        public List<GoalDetails> goaldetailsForNewsfeed(long SID)
        {
            List<GoalDetails> Result = new List<GoalDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select top 4 studentinfo.Goal.ID,studentinfo.Goal.StartDate,studentinfo.Goal.EndDate,studentinfo.Goal.UserID_FK,studentinfo.Goal.ExamTopicID_FK,
                                    studentinfo.ExamTopics.Topic,studentinfo.ExamTopics.SubjectID,studentinfo.Examsubjects.Subject,studentinfo.ExamTopics.ExamID,studentinfo.ExamType.ExamType from studentinfo.Goal
                                    inner join studentinfo.ExamTopics on studentinfo.Goal.ExamTopicID_FK=studentinfo.ExamTopics.ID
                                    inner join studentinfo.Examsubjects on studentinfo.ExamTopics.SubjectID=studentinfo.Examsubjects.ID
                                    inner join studentinfo.ExamType on studentinfo.ExamTopics.ExamID=studentinfo.ExamType.ID
                                    where StartDate>= GETDATE() and UserID_Fk=@UID order by StartDate asc";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", SID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GoalDetails temp = new GoalDetails();
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

        public List<ExamDetails_Newsfeed> ExamForNewsfeed()
        {
            List<ExamDetails_Newsfeed> Result = new List<ExamDetails_Newsfeed>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select ID,Exam,ExamDate from admin.ExamDetails where ExamDate>=getdate() and Status=1 order by ExamDate asc";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamDetails_Newsfeed temp = new ExamDetails_Newsfeed();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Exam = Convert.ToString(reader["Exam"]);
                        temp.tempdate = Convert.ToString(reader["ExamDate"]);

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

        #region USER DETAILS UPDATION
        public string UserDetailsUpdate(EditProfile Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" Update admin.Users set Email=@Email, Password=@Password where UID=@UID";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@Email", Info.UserDetails.Email);
                    com.Parameters.AddWithValue("@Password", Info.UserDetails.Password);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                        Result = " User Profile Updated ";
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


            }
            return Result;
        }

        #endregion


        #region OBJECT NULL CHECKING 
        public string ObjectNullChecking(object ob)
        {
            foreach (var propertyInfo in ob.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(ob, null) == null)
                    {
                        propertyInfo.SetValue(ob, string.Empty, null);
                    }
                }
            }
            return JsonConvert.SerializeObject(ob);
        }
        #endregion


        #region GENERATE TOKEN 
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh"; // Generated at https://www.random.org/strings
        public string GenerateToken(long Id, string Email, string Password)
        {

            string Token = "";
            try
            {
                string hash = string.Join(":", new string[] { Email, Id.ToString() });
                string hashLeft = "";
                string hashRight = "";
                using (HMAC hmac = HMACSHA256.Create(_alg))
                {
                    hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(Password));
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                    hashLeft = Convert.ToBase64String(hmac.Hash);
                    hashRight = string.Join(":", new string[] { Email, Id.ToString() });
                }
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
            }
            catch (Exception)
            {

            }
            return Token;
        }
        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }
        #endregion

        #region CREATE TOKEN 
        public void UpdateTokenId(long ID, string Token)
        {
            try
            {
                string Query = @"delete from [dbo].[Token_Details] where UserID=@UserID;
                                insert into [dbo].[Token_Details](UserID,Token,IsActive,IsDelete,CreatedDate) values(@UserID,@Token,1,0,GETDATE())";
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd = new SqlCommand(Query,con);
                cmd.Parameters.AddWithValue("@TokenId", Token);
                cmd.Parameters.AddWithValue("@UserID", ID);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                int AffectedRow = cmd.ExecuteNonQuery();
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region TOKEN CHECKING 
        public bool CheckTokenExist(string Token)
        {
            DataTable dt = new DataTable();
            bool Res = false;
            try
            {
                if (Token != "" && Token != null)
                {
                    string Query = @"Select * from [dbo].[Token_Details] where Token=@Token AND IsActive=1 AND IsDelete=@IsDelete";
                    SqlConnection con = new SqlConnection(conn);
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@Token", Token);
                    cmd.Parameters.AddWithValue("@IsDelete", 0);
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Res = true;
                    }
                }

            }
            catch (Exception)
            {

            }
          
            return Res;
        }
        #endregion

        #region get Token by User id
        public string GetTokenByID(long UserID)
        {
            string ResId = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ID from [dbo].[Token_Details] where UserID=@UserID";
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        ResId = dt.Rows[0]["TokenId"].ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
            return ResId;
        }
        #endregion

        public string InsertNote(Notes Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[dbo].[Insert_Note]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", Info.UserID_Fk);
                    com.Parameters.AddWithValue("@Note", Info.Note);

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

        public List<Notes> GetNote(long UID)
        {
            List<Notes> Result = new List<Notes>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_getNote", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UID", UID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Notes temp = new Notes();
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

        public string RemoveNote(long UserID, long NoteID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"DELETE FROM global.Notes WHERE ID=@NoteID AND UserID_Fk=@UID";

                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", @UserID);
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

        public List<LhsFeedDetails> LeftpanelFeedDetails(long SID)
        {
            List<LhsFeedDetails> res = new List<LhsFeedDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select ct.Topic_ID as ID,ct.Topic_CreatedDate as DateCreated,ct.Topic_Discussion as LatestNews 
                        ,ct.Community_Fk_ID as SectionID,comm.Community_Name as SectionName,'Online CommunityTopic' as Type,ct.UID_CreatedBY as UID,'' as SubSection,''as SubSectionID
                        from studentinfo.Online_Community_Topic as ct
                        inner join studentinfo.Online_Community as comm 
                        on ct.Community_Fk_ID=comm.Community_ID
                        where ct.Topic_ID=(select max(Topic_ID) from studentinfo.Online_Community_Topic) 
                        Union 
                        select cmt.Community_Comment_ID as ID,cmt.Community_Comment_CommentedDate as DateCreated,cmt.Community_Comment_Text as LatestNews,
                        comm.Community_ID as SectionID,comm.Community_Name as SectionName,'Online CommunityComment' as Type,cmt.UID as UID,ct.Topic_Discussion as SubSection,ct.Topic_ID as SubSectionID
                        from studentinfo.Online_Community_Comment as cmt
                        inner join studentinfo.Online_Community_Topic as ct on ct.Topic_ID=cmt.Topic_Fk_ID
                        inner join studentinfo.Online_Community as comm on comm.Community_ID=ct.Community_Fk_ID
                        where Community_Comment_ID=(select max(Community_Comment_ID) from
                        studentinfo.Online_Community_Comment) 
                        Union
                        select  top 1 T.ID as ID,T.Createddate as DateCreated,T.Topic as LatestNews,(select Group_ID from studentinfo.Study_Group as G where G.Group_ID=T.GroupID_FK) as SectionID,  
                        (select Group_Name from studentinfo.Study_Group as G where G.Group_ID=T.GroupID_FK) as SectionName,
                        'StudyGroupTopic' as Type,T.UID_Stu as UID,'' as SubSection,'' as SubSectionID
                        from  studentinfo.Study_Group_Topic  
                         as T where T.ID=(select max(ID) from 
                         studentinfo.Study_Group_Topic
                          where GroupID_FK in(select Group_Fk_ID from studentinfo.Study_Group_Member where UID_Stu=@uid) ) 
                        union
                        select top 1 C.Group_Comment_ID as ID,C.Group_Comment_CommentedDate as DateCreated,C.Group_Comment_Text as LatestNews,C.Group_Fk_ID as SectionID,
                        (select Group_Name from studentinfo.Study_Group as G where G.Group_ID=C.Group_Fk_ID) as SectionName
                        ,'StudyGroupComment' as Type,C.[UID(St)] as UID,
                        (select Topic  from studentinfo.Study_Group_Topic as T where T.ID=C.TopicID_FK) as SubSection,
                        C.TopicID_FK as SubSectionID
                        from 
                        studentinfo.Study_Group_Comment C
                        where C.Group_Comment_ID=(select max(Group_Comment_ID) from
                        studentinfo.Study_Group_Comment where Group_Fk_ID in(select Group_Fk_ID from studentinfo.Study_Group_Member where UID_Stu=@uid))
                        union
                        select top 1 ID as ID, Createddate as DateCreated, Title as LatestNews,'' as SectionName,
                        '' as SectionID,'Events' as Type,'' as UID,'' as SubSection,'' as SubsectionID
                        from collegeEvents.Events where ID=(select max(ID) from collegeEvents.Events)
                        union
                        select top 1 C.ID as ID,C.CreatedDate as DateCreated,C.Comments as LatestNews,C.Eventid as SectionID
                        ,(select Event from collegeEvents.Events as E where E.ID=C.Eventid) as SectionName,
                        'EventComment' as Type,C.userid as UID,'' as Subsection,'' as SubsectionID
                        from collegeEvents.EventComments as C where 
                         C.ID=(select max(ID) from collegeEvents.EventComments) 
                         Order by DateCreated desc";


                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@uid", SID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        LhsFeedDetails ss = new LhsFeedDetails();
                        ss.ID = Convert.ToInt64(reader["ID"]);
                        ss.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                        ss.LatestNews = Convert.ToString(reader["LatestNews"]);
                        ss.SectionID = Convert.ToInt64(reader["SectionID"]);
                        ss.SectionName = Convert.ToString(reader["SectionName"]);
                        ss.Type = Convert.ToString(reader["Type"]);
                        ss.UID = Convert.ToInt64(reader["UID"]);
                        ss.SubSection = Convert.ToString(reader["SubSection"]);
                        ss.SubSectionID = Convert.ToInt64(reader["SubSectionID"]);
                        res.Add(ss);
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            return res;
        }

        #region Calender Date
        public List<Data_Calender> DataForCalender(long UserID)
        {
            List<Data_Calender> Result = new List<Data_Calender>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @" select ID as id,CONVERT(VARCHAR(10),EventDate,101) as Date,Title as Text,'Event' as Type from collegeEvents.Events 
                  union
                  select NxtCollege_ID as id,CONVERT(VARCHAR(8),NxtCollege_AddedDate,1) as Date,NxtCollege_name as Text,'College' as Type from collegeEvents.NextCollege 
                  union
                  select ID as id ,CONVERT(VARCHAR(8),ExamDate,1) as Date ,Exam as Text,'Exam' as Type from admin.ExamDetails 
                  union                 
                  select studentinfo.Goal.ID,CONVERT(VARCHAR(8),studentinfo.Goal.StartDate,1),
                  (studentinfo.ExamType.ExamType+'-'+studentinfo.Examsubjects.Subject+'-'+studentinfo.ExamTopics.Topic) as Text,
                  'Goal' as Type from studentinfo.Goal
                    inner join studentinfo.ExamTopics on studentinfo.Goal.ExamTopicID_FK=studentinfo.ExamTopics.ID
                    inner join studentinfo.Examsubjects on studentinfo.ExamTopics.SubjectID=studentinfo.Examsubjects.ID
                    inner join studentinfo.ExamType on studentinfo.Examtopics.ExamID=studentinfo.ExamType.ID
                    where studentinfo.Goal.UserID_FK=@UID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@UID", UserID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Data_Calender temp = new Data_Calender();

                        temp.id = Convert.ToInt32(reader["id"]);
                        temp.TempDate = Convert.ToString(reader["Date"]);
                        temp.Text = Convert.ToString(reader["Text"]);
                        temp.Type = Convert.ToString(reader["Type"]);


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
    }

}