using Go2uniApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Go2uniApi.CodeFile
{
    public class Hod
    {
        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();

        public string InsertTeacherNote(TeacherNote Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[teacher].[SP_InsertTeacherNote]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TeacherID_FK", Info.TeacherID_FK);
                    com.Parameters.AddWithValue("@Note", Info.Note);
                    con.Open();
                    int LastRow = Convert.ToInt32(com.ExecuteScalar());
                    if (LastRow > 0)
                    {
                        Result = "Success!" + LastRow; ;
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
        public List<TeacherNote> GetTeacherNotes(long TeacherID_FK)
        {
            List<TeacherNote> Result = new List<TeacherNote>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[teacher].[SP_GetTeacherNotes]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TeacherID", TeacherID_FK);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherNote temp = new TeacherNote();
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
        public string RemoveTeacherNote(long TeacherID_FK, long NoteID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[teacher].[SP_RemoveTeacherNote]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TeacherID_FK", TeacherID_FK);
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

        public List<TopicCoverDate> GetAllTeacherTopics(long ID)
        {
            List<TopicCoverDate> obj = new List<TopicCoverDate>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("[teacher].[sp_GetTeacherTopics]", con);
                cmd.Parameters.AddWithValue("@TeacherID", ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                {
                    string res = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<TopicCoverDate>>(res);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }

        public List<AssignmentDueDate> GetAssignmentDetails(long TeacherID_FK)
        {
            List<AssignmentDueDate> Result = new List<AssignmentDueDate>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[teacher].[SP_GetAssignmentDetails]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TeacherID_FK", TeacherID_FK);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AssignmentDueDate temp = new AssignmentDueDate();
                        //temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.Chapter = Convert.ToString(reader["Chapter"]);
                        temp.Standard = Convert.ToInt32(reader["Standard"]);
                        temp.SetDate = Convert.ToDateTime(reader["SetDate"]);
                        temp.DueDate = Convert.ToDateTime(reader["DueDate"]);
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

        public List<TestDueDate> GetAllTestDueDate(long ID)
        {
            List<TestDueDate> obj = new List<TestDueDate>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("[teacher].[SP_GetAllTestDueDateById]", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDE", ID);
                da.Fill(dt);
                {
                    string res = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<TestDueDate>>(res);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }

        #region Time Table 
        public List<WeekDays> GetAllWeekdays()
        {
            List<WeekDays> obj = new List<WeekDays>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllWeekDays", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                {
                    string res = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<WeekDays>>(res);
                }
            }
            catch (Exception)
            {
            }
            return obj;
        }

        public List<PeriodDetails> GetAllperiods()
        {
            List<PeriodDetails> obj = new List<PeriodDetails>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllPeriods", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                {
                    string res = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<PeriodDetails>>(res);
                }
            }
            catch (Exception)
            {
            }
            return obj;
        }

        public List<TimeTable> GetAllTimeTableList(long ID)
        {
            List<TimeTable> Result = new List<TimeTable>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("[teacher].[SP_GetTimeTable]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        TimeTable temp = new TimeTable();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.TeacherID = Convert.ToInt32(reader["TeacherID"]);
                        temp.WeekID = Convert.ToInt32(reader["WeekID"]);
                        temp.TimeID = Convert.ToInt32(reader["TimeID"]);
                        temp.Day = Convert.ToString(reader["Day"]);
                        temp.SubID = Convert.ToInt32(reader["SubID"]);
                        temp.Period = Convert.ToString(reader["Period"]);
                        temp.Sub = Convert.ToString(reader["Sub"]);
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


        #region Edit Profile
        public EditProfileTeacher EditProfile(long TID)
        {
            EditProfileTeacher result = new EditProfileTeacher();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select IsNull(ID,'') AS TeacherID, IsNull(Name,'') As Name,IsNull(Email,'') AS Email,IsNull(Password,'') As Password,IsNull(Joining_Date,'') AS Joining_Date,IsNull(MObile_No,'') AS Mobile_NO,IsNUll(Profile_Image,'') AS Profile_Image from teacher.TeachersDetails where ID=@TeacherID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TeacherID", TID);
                   // com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {


                        result.Teacher_Name = Convert.ToString(reader["Name"]);
                        result.Email = Convert.ToString(reader["Email"]);
                        result.Password = Convert.ToString(reader["Password"]);
                        result.JoiningDate = Convert.ToDateTime(reader["Joining_Date"]);

                        result.MobileNo = Convert.ToString(reader["Mobile_NO"]);
                        result.ProfileImage = Convert.ToString(reader["Profile_Image"]);
                        result.ID = Convert.ToInt32(reader["TeacherID"]);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return result;
        }

        public string UpdateTeacherProfile(EditProfileTeacher Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = string.Empty;
                    if (Data.ProfileImage != null && Data.ProfileImage != "")
                    {
                        Query = @" update  [teacher].TeachersDetails set  Name=@TeacherName ,Email=@Email, Password=@Password, Joining_Date=@JoiningDate, MObile_No=@MobileNo, Profile_Image=@ProfileImage  where ID=@TeacherID ";

                    }
                    else
                    {
                        Query = @" update  [teacher].TeachersDetails set  Name=@TeacherName ,Email=@Email, Password=@Password, Joining_Date=@JoiningDate, MObile_No=@MobileNo  where ID=@TeacherID ";

                    }
                    SqlCommand com = new SqlCommand(Query, con);
                    //com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TeacherID", Data.ID);
                    com.Parameters.AddWithValue("@TeacherName", Data.Teacher_Name);
                    com.Parameters.AddWithValue("@Email", Data.Email);
                    com.Parameters.AddWithValue("@Password", Data.Password);

                    
                   
                    com.Parameters.AddWithValue("@JoiningDate", Data.JoiningDate);
                    com.Parameters.AddWithValue("@MobileNo", Data.MobileNo);
                   
                    if (Data.ProfileImage != null && Data.ProfileImage != "")
                    {
                        com.Parameters.AddWithValue("@ProfileImage", Data.ProfileImage);
                    }


                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Updated";
                    }
                    else
                    {
                        Result = "Failed!Process Failed ";
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