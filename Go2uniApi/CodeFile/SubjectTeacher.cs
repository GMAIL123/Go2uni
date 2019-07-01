using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.CodeFile
{
    public class SubjectTeacher
    {
        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();

        #region Edit Profile SUBJECT TEACHER

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