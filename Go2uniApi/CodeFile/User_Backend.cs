using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Go2uniApi.Models;
using static Go2uniApi.Models.User_Part;
using System.Data;

namespace Go2uniApi.CodeFile
{
    public class User_Backend
    {
        readonly SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());
        private static string email = string.Empty;

        public bool Checkemailexistance(string email)
        {
            bool result = false;
            try
            {
                using (con)
                {
                    string query = "getstudentdatabyemail";
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("email", email));
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
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return result;
        }

        public string GetstudentdatabyEmail(string email)
        {
            string result = string.Empty;
            try
            {
                using (con)
                {
                    string query = "getstudentdatabyemail";
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("email", email));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = JsonConvert.SerializeObject(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return result;
        }

        //Additional
        public string GetstudentdatabyID(string id)
        {
            string result = string.Empty;
            try
            {
                using (con)
                {
                    string query = "SP_GetallDatafromStudentsByID";
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("id", id));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = JsonConvert.SerializeObject(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return result;
        }

        public string StudentLogin(Login studentBasic)
        {
            string Result = string.Empty;
            try
            {
                using (con)
                {
                    SqlCommand com = new SqlCommand("SP_StudentLogIn", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserName", studentBasic.Email);
                    com.Parameters.AddWithValue("@Password", studentBasic.Password);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result = Convert.ToString(reader["StaticVariable"]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result;
        }

        public string TeacherLogin(Login TeacherBasic)
        {
            string Result = string.Empty;
            try
            {
                using (con)
                {
                    SqlCommand com = new SqlCommand("[teacher].[SP_TeacherLogIn]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserName", TeacherBasic.Email);
                    com.Parameters.AddWithValue("@Password", TeacherBasic.Password);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result = Convert.ToString(reader["StaticVariable"]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result;
        }
               
        public Login Login(string Username, string password)
        {
            Login Info = new Login();
            DataTable dt = new DataTable();
            try
            {
                string Query = @"select UID,UserType_FK,Email,Password,Status as IsActive from admin.users Where Email=@UserName and Password=@Password";
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Info = JsonConvert.DeserializeObject<List<Login>>(JsonConvert.SerializeObject(dt))[0];
                }
            }
            catch (Exception)
            {

            }
            return Info;
        }
                     
        public Login GetLoginDetails(long UID, long UserTypeId)
        {
            Login Info = new Login();
            DataTable dt = new DataTable();
            try
            {
                using (con)
                {
                    string query = String.Empty;
                    if (UserTypeId == 1)
                    {
                        query = @"select au.UID as UserID_FK ,au.UserType_FK,sd.ID as SuperAdminID,lgn.LoginType,sd.Name,sd.Profile_Image,sd.SchoolID_FK,ISNULL(sd.STATUS, 0) as IsActive,0 as IsFirstStepComplete
                                from admin.users as au
                               left join admin.Login_Type as lgn on au.UserType_FK = lgn.ID
                               left join admin.SuperAdmin_Details as sd on au.UID = sd.UserID_FK
                               where au.UserType_FK = @UserTypeId and
                               au.UID = @UID";
                    }

                    else if (UserTypeId == 8)
                    {
                        query = @"select au.UID as UserID_FK ,au.UserType_FK,st.UserID_FK as StudentID,lgn.LoginType, st.Stu_Name as Name,st.Stu_Profile_Image as Profile_image,IsNull(st.School_ID,'') as SchoolID_FK,ISNULL(st.STATUS, 0) as IsActive,IsNull(st.IsFirstStepComplete,0) as IsFirstStepComplete
                                from admin.users as au
                               left join admin.Login_Type as lgn on au.UserType_FK = lgn.ID
                               left join studentinfo.Student_Details as st on au.UID = st.UserID_FK
                               where au.UserType_FK =@UserTypeId and
                               au.UID =@UID ";
                    }
                    else if (UserTypeId == 4)
                    {
                        query = @"select au.UID as UserID_FK ,au.UserType_FK,st.UserID_FK as StudentID,lgn.LoginType, st.Stu_Name as Name,st.Stu_Profile_Image as Profile_image,st.School_ID as SchoolID_FK,ISNULL(st.STATUS, 0) as IsActive,IsNull(st.IsFirstStepComplete,0) as IsFirstStepComplete,isnull(st.Class_ID,'') as ClassID
                               from admin.users as au
                              left join admin.Login_Type as lgn on au.UserType_FK = lgn.ID
                              left join studentinfo.Student_Details as st on au.UID = st.UserID_FK
                              where au.UserType_FK = @UserTypeId and
                              au.UID=@UID";
                    }
                    else if (UserTypeId == 0)
                    {
                        query = @"select au.UID as UserID_FK ,au.UserType_FK,side.ID as SiteAdminID,lgn.LoginType,side.Name,side.ProfileImage as Profile_image,IsNull(side.School_ID,'') as SchoolID_FK,ISNULL(side.STATUS, 0) as IsActive,0 as IsFirstStepComplete
                               from admin.users as au
                               left join admin.Login_Type as lgn on au.UserType_FK = lgn.ID
                               left join [site_admin].[SiteAdminDetails] as side on au.UID = side.UserID_FK
                               where au.UserType_FK = @UserTypeId and
                               au.UID =@UID";
                    }
                    else if (UserTypeId == 9)
                    {
                        query = @"select admin.users.UID as UserID_FK,admin.users.UserType_FK,admin.Login_Type.LoginType
							   from admin.users left join admin.Login_Type on admin.users.UserType_FK=admin.Login_Type.ID
							   where admin.users.UID=@UID and admin.users.UserType_FK=@UserTypeId";
                    }
                    else if (UserTypeId == 3)
                    {
                        query = @"select au.UID UserID_FK,au.UserType_FK,
                                 td.ID TeacherID,lgn.LoginType,
                                 td.Name,td.Profile_Image,
                                 td.SchoolID_FK,ISNULL(td.STATUS,0) as IsActive,
                                 0 as IsFirstStepComplete,0 as Dept_SubID,
                                 ap.ClassID
                                 from admin.Users au
                                 left join admin.Login_Type lgn on au.UserType_FK = lgn.ID
                                 left join teacher.TeachersDetails td on au.UID=td.UID_FK
                                 left join admin.AssignPrivilegeClassTeacher ap on ap.TeacherID=td.UID_FK
                                 where au.UserType_FK=@UserTypeId and au.UID=@UID";
                    }
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@UserTypeId", UserTypeId);
                    cmd.Parameters.AddWithValue("@UID", UID);

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string Res = JsonConvert.SerializeObject(dt);
                        Info = JsonConvert.DeserializeObject<List<Login>>(Res)[0];
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return Info;
        }

        public bool StudentUpdatePassword(string email, string password)
        {
            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_StudentUpdatePassword", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("UserName", email));
                    cmd.Parameters.Add(new SqlParameter("Password", password));
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
                return false;
            }

        }

        public string Registration(Registration Info)
        {
            string Result = string.Empty;
            try
            {
                if (!(CheckEMailExists(Info.Email)))
                {
                    using (con)
                    {
                        // SqlCommand com = new SqlCommand("SP_StudentRegistration", con);
                        SqlCommand com = new SqlCommand("[studentinfo].[St_Social_Registration]", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Email", Info.Email);
                        com.Parameters.AddWithValue("@Password", Info.Password);
                        // com.Parameters.AddWithValue("@UserName", Info.UserName);
                        com.Parameters.AddWithValue("@Name", Info.Name);
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
                else
                {
                    Result = "Email Id already exists";
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }

            return Result;
        }
               
        public bool CheckEMailExists(string MailID)
        {
            bool result = false;

            try
            {
                string query = "select UID,Email,Password from admin.users where Email=@Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", MailID);
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
               
        #region User SOCIAL REGISTRATION facebook
        public string UserSocialRegistration(SocialUserInfo Info)
        {
            string Res = "";
            long NormalLogInId = 0;
            //int Pass = 0;
            if (!string.IsNullOrEmpty(Info.Email))
            {
                NormalLogInId = checkNormailLogin(Info.Email);
            }
            if (Info.SocialUserId != null && Info.SocialUserId != "")
            {
                NormalLogInId = checkNormailSocialLogin(Info.SocialUserId);
            }
            if (NormalLogInId > 0)
            {
                Res = "Success!" + NormalLogInId;
            }
            else if (NormalLogInId == -1)
            {
                Res = "Failed! The email address is already exist. This account is not activated.";
            }
            else
            {
                long RegisteredId = checkSocialUser(Info.SocialUserId);
                if (RegisteredId <= 0)
                {
                    if (!string.IsNullOrEmpty(Info.Email) && checkPatientDuplicateEmailID(Info.Email) > 0)
                    {
                        return "Failed! The email address is already exist.";
                    }
                    try
                    {
                        string Query = @"insert into ClientInfo (FirstName,SurName,Email,Password,Address1,Address2,Address3,PostCode,MobileNo,AccountType,IsVerified,
IsActive,CreatedDate,LastModifiedDate,IsDeleted,ProfileImg,IsCompleted,Identification,LoginStatus,IsChecked,Subscription,
ExpiryDt,Amount,Refund,SocialUserId,RegistrationType,GeoLocation) values
(@FirstName,@SurName,@Email,@Password,@Address1,@Address2,@Address3,@PostCode,@MobileNo,@AccountType,@IsVerified,
@IsActive,getdate(),getdate(),@IsDeleted,@ProfileImg,@IsCompleted,@Identification,@LoginStatus,getdate(),@Subscription,
@ExpiryDt,@Amount,@Refund,@SocialUserId,@RegistrationType,@GeoLocation)
                                SELECT SCOPE_IDENTITY()";
                        SqlCommand cmd = new SqlCommand(Query, con);
                        //cmd.Parameters.AddWithValue("@FirstName", "unknown");
                        if (Info.Name != null && Info.Name != "")
                        {
                            cmd.Parameters.AddWithValue("@FirstName", Info.Name);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@FirstName", "Unknown");
                        }
                        cmd.Parameters.AddWithValue("@SurName", "");
                        cmd.Parameters.AddWithValue("@Email", Info.Email);
                        cmd.Parameters.AddWithValue("@Password", Info.Password);
                        cmd.Parameters.AddWithValue("@Address1", "");
                        cmd.Parameters.AddWithValue("@Address2", "");
                        cmd.Parameters.AddWithValue("@Address3", "");
                        cmd.Parameters.AddWithValue("@PostCode", "");
                        cmd.Parameters.AddWithValue("@MobileNo", "");
                        cmd.Parameters.AddWithValue("@AccountType", "");
                        cmd.Parameters.AddWithValue("@IsVerified", 1);
                        cmd.Parameters.AddWithValue("@IsActive", 1);
                        cmd.Parameters.AddWithValue("@IsDeleted", 0);
                        cmd.Parameters.AddWithValue("@ProfileImg", Info.ProfileImg);
                        cmd.Parameters.AddWithValue("@IsCompleted", 0);
                        cmd.Parameters.AddWithValue("@Identification", "");
                        cmd.Parameters.AddWithValue("@LoginStatus", 1);
                        cmd.Parameters.AddWithValue("@Subscription", "");
                        cmd.Parameters.AddWithValue("@ExpiryDt", "");
                        cmd.Parameters.AddWithValue("@Amount", "");
                        cmd.Parameters.AddWithValue("@Refund", "");
                        cmd.Parameters.AddWithValue("@SocialUserId", Info.SocialUserId);
                        cmd.Parameters.AddWithValue("@RegistrationType", Info.RegistrationType);
                        cmd.Parameters.AddWithValue("@GeoLocation", Info.Geolocation);

                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        int AffectedRow = Convert.ToInt32(cmd.ExecuteScalar());
                        Res = "Success!" + AffectedRow + "!NewUser" + "!" + Info.Password;
                        if (con.State != ConnectionState.Closed)
                        {
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Res = ex.Message;
                    }
                }
                else
                {
                    Res = "Success!" + RegisteredId;
                }
            }
            return Res;
        }
        #endregion
        
        public long checkNormailLogin(string Email)
        {
            long ResId = 0;
            DataTable dt = new DataTable();
            try
            {
                string Query = @"select UID,Status as ISACTIVE from admin.users where Email=@Email ";
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@Email", Email);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        ResId = Convert.ToInt64(dt.Rows[0]["UID"].ToString());
                    }
                    else
                    {
                        ResId = -1;
                    }
                }
            }
            catch (Exception)
            {
            }
            return ResId;
        }

        public long checkNormailSocialLogin(string SocialUserId)
        {
            long ResId = 0;
            DataTable dt = new DataTable();
            try
            {
                string Query = @"select ID,IsActive from ClientInfo where SocialUserId=@SocialUserId AND IsDeleted=0";
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@SocialUserId", SocialUserId);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        ResId = Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                    }
                    else
                    {
                        ResId = -1;
                    }
                }
            }
            catch (Exception)
            {
            }
            return ResId;
        }
        
        #region SOCIAL patient CHECK 
        public long checkSocialUser(string SocialId)
        {
            long Res = 0;
            DataTable dt = new DataTable();
            try
            {
                string Query = @"select ID from ClientInfo where SocialUserId=@SocialUserId AND IsDelete=0";
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@SocialUserId", SocialId);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Res = Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                }
            }
            catch (Exception)
            {
            }
            return Res;
        }

        public long checkPatientDuplicateEmailID(string Email)
        {
            long ResId = 0;
            DataTable dt = new DataTable();
            try
            {
                string Query = @"select ID,IsActive from ClientInfo where Email=@Email AND IsDeleted=0";

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@Email", Email);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        ResId = Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                    }
                    else
                    {
                        ResId = -1;
                    }
                }
            }
            catch (Exception)
            {
            }
            return ResId;
        }
        #endregion

        private bool OTPExistatance(string email)
        {
            using (con)
            {
                string query = @"select * from User_OTP where email=@email";
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                DataTable tbl = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(tbl);
                if (tbl.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool InsertOrUpdateOTP(Reginfo info)
        {
            email = info.Email;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                bool result; string query; SqlCommand cmd;

                if (OTPExistatance(info.Email))
                {
                    query = @"update User_OTP set otptext=@OtpText where email=@email";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OtpText", info.Otp);
                    cmd.Parameters.AddWithValue("@email", info.Email);

                    OtpExpiry();
                }
                else
                {
                    query = @"insert into User_OTP values(@OtpText,@email)";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OtpText", info.Otp);
                    cmd.Parameters.AddWithValue("@email", info.Email);

                    OtpExpiry();
                }
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }

        public bool updateDetails(Reginfo info)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                if (checkForOtp(info) == "OtpFound")
                {
                    string query = @"update [Go2uni].[admin].[users] set Password=@password where Email=@Email;
                                        update User_OTP set otptext=null where email=@Email";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@password", info.Password);
                    cmd.Parameters.AddWithValue("@Email", info.Email);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else { return false; }
            }
        }

        public string checkForOtp(Reginfo info)
        {
            string userOtp = string.Empty;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                string query = @"select dbo.User_OTP.otpid,isnull(dbo.User_OTP.otptext,'') as otptext,
                                    dbo.User_OTP.email from dbo.User_OTP where dbo.User_OTP.email=@email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", info.Email);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userOtp = Convert.ToString(reader["otptext"]);
                }
                if (userOtp.Equals(""))
                {
                    return "OtpExpired";
                }
                else if (!userOtp.Equals(info.Otp))
                {

                    return "OtpNotMatch";
                }
                else
                {

                    return "OtpFound";
                }
            }
        }

        private static void OtpExpiry()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 600000;
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
        }

        private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                string query = @"delete from dbo.User_OTP where dbo.User_OTP.email=@Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                if (con.State == ConnectionState.Broken ||
                         con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
            }
        }

        #region Site Admin By Indranil
        public List<studentList> fetchallStudentList()
        {
            List<studentList> studentlist = new List<studentList>();
            string Query = @"select UserID_FK,Stu_Name,Stu_Profile_Image,LastLoginTime from studentinfo.Student_Details where LoginTpe_FK=8";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        studentList temp = new studentList()
                        {
                            ID = Convert.ToInt64(reader["UserID_FK"].ToString()),
                            student_name = reader["Stu_Name"].ToString(),
                            profilepic = reader["Stu_Profile_Image"].ToString(),
                            lastlogin = Convert.ToDateTime(reader["LastLoginTime"].ToString()),
                        };
                        studentlist.Add(temp);
                    }
                }
            }
            return studentlist;
        }

        #endregion

        #region Site Admin By Indranil 04/02/2019

        public studentList getStudentById(long eid)
        {
            studentList student = new studentList();
            string Query = @"select Stu_ID,Stu_Name,Stu_Profile_Image,LastLoginTime from studentInfo.Student_Details where UserID_FK=@userid";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@userid", eid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        student.ID = Convert.ToInt64(reader["Stu_ID"].ToString());
                        student.student_name = Convert.ToString(reader["Stu_Name"]);
                        student.profilepic = Convert.ToString(reader["Stu_Profile_Image"]);
                        student.lastlogin = Convert.ToDateTime(reader["LastLoginTime"].ToString());
                        student.UserID_FK = eid;
                        student.Msg = fetchStudentMessagesById(eid);
                    }
                }
                return student;
            }
        }

        private static List<Messages_Admin> fetchStudentMessagesById(long eid)
        {
            List<Messages_Admin> messages = new List<Messages_Admin>();
            string Query = @"
                        select admin.LiveMessageInfo.ID,admin.LiveMessageInfo.SenderID,admin.LiveMessageInfo.ReceiverID,admin.LiveMessageInfo.Message
                        ,admin.LiveMessageInfo.CreatedDate,admin.LiveMessageInfo.IsDelete,admin.LiveMessageInfo.IsRead,admin.LiveMessageInfo.ReceiverTypeID 
                         from admin.LiveMessageInfo where SenderTypeID=0 and ReceiverID=@ReceiverID or SenderID=@ReceiverSameID and ReceiverTypeID=0
                        order by admin.LiveMessageInfo.CreatedDate desc";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ReceiverID", eid);
                    command.Parameters.AddWithValue("@ReceiverSameID", eid);
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Messages_Admin temp = new Messages_Admin()
                            {
                                ID = Convert.ToInt64(reader["ID"].ToString()),
                                SenderID = Convert.ToInt64(reader["SenderID"].ToString()),
                                ReceiverID = Convert.ToInt64(reader["ReceiverID"].ToString()),
                                Message = Convert.ToString(reader["Message"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsDelete = Convert.ToBoolean(reader["IsDelete"]),
                                IsRead = Convert.ToBoolean(reader["IsRead"]),
                                ReceiverTypeID = Convert.ToInt32(reader["ReceiverTypeID"].ToString())
                            };
                            messages.Add(temp);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return messages;
        }

        public studentList getStudentByIdForUserPart(long eid)
        {
            studentList student = new studentList();
            string Query = @"select Stu_ID,Stu_Name,Stu_Profile_Image,LastLoginTime from studentInfo.Student_Details where UserID_FK=@userid";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@userid", eid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        student.ID = Convert.ToInt64(reader["Stu_ID"].ToString());
                        student.student_name = Convert.ToString(reader["Stu_Name"]);
                        student.profilepic = Convert.ToString(reader["Stu_Profile_Image"]);
                        student.lastlogin = Convert.ToDateTime(reader["LastLoginTime"].ToString());
                        student.UserID_FK = eid;
                        student.Msg = fetchStudentMessagesByIdForUserPart(eid);
                    }
                }
                return student;
            }
        }

        private static List<Messages_Admin> fetchStudentMessagesByIdForUserPart(long eid)
        {
            List<Messages_Admin> messages = new List<Messages_Admin>();
            string Query = @"
                        select admin.LiveMessageInfo.ID,admin.LiveMessageInfo.SenderID,admin.LiveMessageInfo.ReceiverID,admin.LiveMessageInfo.Message
                        ,admin.LiveMessageInfo.CreatedDate,admin.LiveMessageInfo.IsDelete,admin.LiveMessageInfo.IsRead,admin.LiveMessageInfo.ReceiverTypeID 
                         from admin.LiveMessageInfo where SenderTypeID=0 and ReceiverID=@ReceiverID or SenderID=@ReceiverSameID and ReceiverTypeID=0
                        order by admin.LiveMessageInfo.CreatedDate asc";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ReceiverID", eid);
                    command.Parameters.AddWithValue("@ReceiverSameID", eid);
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Messages_Admin temp = new Messages_Admin()
                            {
                                ID = Convert.ToInt64(reader["ID"].ToString()),
                                SenderID = Convert.ToInt64(reader["SenderID"].ToString()),
                                ReceiverID = Convert.ToInt64(reader["ReceiverID"].ToString()),
                                Message = Convert.ToString(reader["Message"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsDelete = Convert.ToBoolean(reader["IsDelete"]),
                                IsRead = Convert.ToBoolean(reader["IsRead"]),
                                ReceiverTypeID = Convert.ToInt32(reader["ReceiverTypeID"].ToString())
                            };
                            messages.Add(temp);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return messages;
        }

        public bool InsertSiteAdminMessages(Messages_Admin list)
        {
            bool response = false;
            string Query = @"Insert into admin.LiveMessageInfo(SenderID,ReceiverID,Message,IsDelete,IsRead,SenderTypeID,ReceiverTypeID)
                            values(142,@receiverid,@message,0,0,0,8)";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@receiverid", list.ReceiverID);
                    command.Parameters.AddWithValue("@message", list.Message);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        response = true;
                    }
                }
            }
            return response;
        }
       
        #endregion

        #region Talk to tutor by Indranil 05/02/2019

        public bool InsertUserMessages(Messages_Admin Info)
        {
            bool response = false;
            string Query = @"Insert into admin.LiveMessageInfo(SenderID,ReceiverID,Message,IsDelete,IsRead,SenderTypeID,ReceiverTypeID)
                            values(@senderid,142,@message,0,0,8,0)";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@senderid", Info.SenderID);
                    command.Parameters.AddWithValue("@message", Info.Message);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        response = true;
                    }
                }
            }
            return response;
        }
        #endregion

        #region talk to tutor By Indranil 06/02/2019
        public List<studentList> fetchLiveUserMesagesByName()
        {
            List<studentList> list = new List<studentList>();
            string Query = @"select studentinfo.Student_Details.Stu_Name,studentinfo.Student_Details.Stu_ID,studentinfo.Student_Details.UserID_FK,studentinfo.Student_Details.Stu_Profile_Image
                                from studentinfo.Student_Details left join admin.LiveMessageInfo on studentinfo.Student_Details.UserID_FK=admin.LiveMessageInfo.SenderID
                                    where ReceiverTypeID=0 and LiveMessageInfo.CreatedDate >= DateAdd(Day, DateDiff(Day, 0, GetDate()), 0)
                                        group by studentinfo.Student_Details.Stu_ID,Stu_Name,UserID_FK,Stu_Profile_Image
                                            order by MAX(admin.LiveMessageInfo.CreatedDate) desc";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        studentList tempList = new studentList()
                        {
                            student_name = Convert.ToString(reader["Stu_Name"]),
                            ID = Convert.ToInt64(reader["UserID_FK"]),
                            message = getUserLastMessage(Convert.ToInt64(reader["UserID_FK"])),
                            profilepic = Convert.ToString(reader["Stu_Profile_Image"])
                        }; list.Add(tempList);
                    }
                }
            }
            return list;
        }
        private static string getUserLastMessage(long id)
        {
            string Message = string.Empty;
            string Query = @"select top 1 admin.LiveMessageInfo.Message from admin.LiveMessageInfo where SenderID=@senderid and ReceiverTypeID=0
                              and LiveMessageInfo.CreatedDate >= DateAdd(Day, DateDiff(Day, 0, GetDate()), 0)
                                   group by admin.LiveMessageInfo.Message order by 
                                        MAX(admin.LiveMessageInfo.CreatedDate) desc";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@senderid", id);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Message = Convert.ToString(reader["Message"]);
                        break;
                    }
                }
            }
            return Message;
        }
        #endregion

        #region REPORT COMMENT FROM Site ADMIN by SM

        public List<CommentReport> GetAllReports()
        {
            List<CommentReport> Result = new List<CommentReport>();
            try
            {
                using (con)
                {
                    string Query = @"select site_admin.CommentReport.ReportID,site_admin.CommentReport.CommentText,
                                site_admin.CommentReport.Go2UniSection,site_admin.CommentReport.CommentID,
                                site_admin.CommentReport.CommentByMail,site_admin.CommentReport.ReportReason,
                                site_admin.CommentReport.IsRead,studentinfo.Student_Details.Stu_Name,site_admin.CommentReport.ReportedBy
                                from site_admin.CommentReport left join studentinfo.Student_Details on
                                site_admin.CommentReport.CommentById=studentinfo.Student_Details.UserID_FK where site_admin.CommentReport.IsDeleted=0
                                group by site_admin.CommentReport.CreatedDate,site_admin.CommentReport.IsRead,
                                site_admin.CommentReport.ReportID,site_admin.CommentReport.CommentText,
                                site_admin.CommentReport.Go2UniSection,site_admin.CommentReport.CommentID,
                                site_admin.CommentReport.CommentByMail,site_admin.CommentReport.ReportReason,
                                site_admin.CommentReport.IsRead,studentinfo.Student_Details.Stu_Name,site_admin.CommentReport.ReportedBy
                                order by site_admin.CommentReport.IsRead asc,site_admin.CommentReport.CreatedDate desc";

                    SqlCommand com = new SqlCommand(Query, con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CommentReport temp = new CommentReport();
                        temp.ReportId = Convert.ToInt64(reader["ReportId"]);
                        temp.CommentID = Convert.ToInt64(reader["CommentID"]);
                        temp.CommentText = Convert.ToString(reader["CommentText"]);
                        temp.Go2UniSection = Convert.ToString(reader["Go2UniSection"]);
                        temp.CommentByName = Convert.ToString(reader["Stu_Name"]);
                        temp.CommentByMail = Convert.ToString(reader["CommentByMail"]);
                        temp.ReportReason = Convert.ToString(reader["ReportReason"]);
                        temp.IsRead = Convert.ToBoolean(reader["IsRead"]);
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
       
        #region UPDATE REPORT STATUS

        public String ChangeReportStatus(ReportTab Info)
        {
            string Res = string.Empty;
            try
            {
                using (con)
                {
                    string Query = @"update [site_admin].[CommentReport] set ReportStatus=0 where ReportId=@ReportId";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    // cmd.Parameters.AddWithValue("@Comment", Info.Group_Comment_Text);
                    // cmd.Parameters.AddWithValue("")
                    cmd.Parameters.AddWithValue("@ReportId", Info.ReportId);


                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        Res = "Sucess| mail sent";
                    }
                    else
                    {
                        Res = "Failed ";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {


            }

            return Res;

        }
        #endregion

        #region Report view by Indranil
        public ReportTab getReportView(long ID)
        {
            ReportTab report = new ReportTab();
            string Query = @"select aa.ReportID,aa.CommentText,aa.Go2UniSection,aa.CommentID,aa.CommentByMail,
                            aa.ReportReason,aa.IsRead,MAX(aa.CommentByName) as CommentByName,MAX(aa.ReportedBy) as ReportedBy from(
                            select site_admin.CommentReport.ReportID,site_admin.CommentReport.CommentText,
                            site_admin.CommentReport.Go2UniSection,site_admin.CommentReport.CommentID,
                            site_admin.CommentReport.CommentByMail,site_admin.CommentReport.ReportReason,
                            site_admin.CommentReport.IsRead,studentinfo.Student_Details.Stu_Name as CommentByName,
                            '' as ReportedBy
                            from site_admin.CommentReport left join studentinfo.Student_Details on
                            site_admin.CommentReport.CommentById=studentinfo.Student_Details.UserID_FK
                            where site_admin.CommentReport.ReportStatus=1 and
                            site_admin.CommentReport.ReportID=@ReportID
                            union
                            select site_admin.CommentReport.ReportID,site_admin.CommentReport.CommentText,
                            site_admin.CommentReport.Go2UniSection,site_admin.CommentReport.CommentID,
                            site_admin.CommentReport.CommentByMail,site_admin.CommentReport.ReportReason,
                            site_admin.CommentReport.IsRead,
                            '' as CommentByName,studentinfo.Student_Details.Stu_Name as ReportedBy
                            from site_admin.CommentReport left join studentinfo.Student_Details on
                            site_admin.CommentReport.ReportedBy=studentinfo.Student_Details.UserID_FK
                            where site_admin.CommentReport.ReportStatus=1 and
                            site_admin.CommentReport.ReportID=@ReportID
                            ) as aa
                            group by aa.ReportID,aa.CommentText,aa.Go2UniSection,aa.CommentID,aa.CommentByMail,
                            aa.ReportReason,aa.IsRead;
                            update site_admin.CommentReport set site_admin.CommentReport.IsRead=1 where site_admin.CommentReport.ReportID=@ReportID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ReportID", ID);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            report = new ReportTab()
                            {
                                ReportId = Convert.ToInt64(reader["ReportID"].ToString()),
                                CommentText = reader["CommentText"].ToString(),
                                Go2UniSection = reader["Go2UniSection"].ToString(),
                                CommentID = Convert.ToInt64(reader["CommentID"].ToString()),
                                CommentByMail = reader["CommentByMail"].ToString(),
                                ReportReason = reader["ReportReason"].ToString(),
                                IsRead = Convert.ToBoolean(reader["IsRead"].ToString()),
                                CommentByName = reader["CommentByName"].ToString(),
                                ReportedBy = reader["ReportedBy"].ToString()
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return report;
        }

        public long getReportCount()
        {
            long res = 0;
            string Query = @"select count(*) as ReportCount from site_admin.CommentReport where site_admin.CommentReport.IsRead=0 and site_admin.CommentReport.IsDeleted=0";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = Convert.ToInt64(reader["ReportCount"]);
                    }
                }
            }
            return res;
        }
        #endregion 

        #endregion

        public string UpdateIsOnline(long UID, string state)
        {
            string Result = ""; string query = "";
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());
                if (state == "unload")
                {
                    query = @"update admin.users set IsOnline=0 where UID=@UID";
                }
                else if (state == "load")
                {
                    query = @"update admin.users set IsOnline=1 where UID=@UID";
                }
                else
                {
                    return Result;
                }
                SqlCommand com = new SqlCommand(query, con);
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
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckIfIsonlineActiveOrnot(long UID)
        {
            bool result = false;

            try
            {

                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());
                string query = "select IsOnline from admin.users where UID=@UID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UID", UID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = Convert.ToBoolean(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        
        #region Newsfeed bt Indranil

        public List<Newsfeed> getAllNewsfeedForSiteAdmin()
        {
            List<Newsfeed> result = new List<Newsfeed>();
            string Query = @"select admin.Newsfeed.ID,admin.Newsfeed.Title,
                            admin.Newsfeed.Description,admin.Newsfeed.EventImage,
                            convert(varchar, admin.Newsfeed.CreatedDate,121) as CreatedDate,
                            admin.Newsfeed.Status from admin.Newsfeed order by
                            admin.Newsfeed.CreatedDate desc";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Newsfeed tempfeed = new Newsfeed()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Title = Convert.ToString(reader["Title"]),
                            Description = Convert.ToString(reader["Description"]),
                            Created_Date = Convert.ToString(reader["CreatedDate"]),
                            EventImage = Convert.ToString(reader["EventImage"]),
                            Status = Convert.ToBoolean(reader["Status"])
                        }; result.Add(tempfeed);
                    }
                }
            }
            return result;
        }

        public string toggleStatus(long id)
        {
            string res = string.Empty;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("admin.toggleCheckboxStatus", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = "success";
                    }
                    else
                    {
                        res = "false";
                    }
                }
            }
            return res;
        }

        public string UpdateNews(Newsfeed Feed)
        {
            string res = String.Empty;
            Global global = new Global();
            string __obj = global.ObjectNullChecking(Feed);
            Newsfeed feed = JsonConvert.DeserializeObject<Newsfeed>(__obj);
            string Query = @"update admin.Newsfeed set admin.Newsfeed.Description=@description,admin.Newsfeed.Title=@title,admin.Newsfeed.EventImage=@EventImage
                                where admin.Newsfeed.ID=@ID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@title", feed.Title);
                    command.Parameters.AddWithValue("@description", feed.Description);
                    command.Parameters.AddWithValue("@EventImage", feed.EventImage);
                    command.Parameters.AddWithValue("@ID", feed.ID);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = "success";
                    }
                    else
                    {
                        res = "failed";
                    }
                }
            }
            return res;
        }

        public string DeleteNews(long id)
        {
            string res = string.Empty;
            string Query = @"delete from admin.Newsfeed where admin.Newsfeed.ID=@ID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", id);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = "success";
                    }
                    else
                    {
                        res = "failed";
                    }
                }
            }
            return res;
        }

        public string insertNews(Newsfeed Feed)
        {
            string res = string.Empty;
            Global global = new Global();
            string __obj = global.ObjectNullChecking(Feed);
            Newsfeed feed = JsonConvert.DeserializeObject<Newsfeed>(__obj);

            string Query = @"insert into admin.Newsfeed(Title,Description,EventImage) 
                            values(@title,@description,@image)";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@title", feed.Title);
                    command.Parameters.AddWithValue("@description", feed.Description);
                    command.Parameters.AddWithValue("@image", feed.EventImage);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = "success";
                    }
                    else
                    {
                        res = "failed";
                    }
                }
            }
            return res;
        }

        #endregion

        #region Events for site admin By Indranil

        public List<ViewEvent> getEventsforSiteAdmin()
        {
            List<ViewEvent> events = new List<ViewEvent>();
            string Query = @"select collegeEvents.Events.ID,collegeEvents.Events.Title,collegeEvents.Events.Event,
                            convert(varchar, collegeEvents.Events.EventDate, 121) as EventDate,ISNULL(collegeEvents.Events.EventVideo,'') as EventVideo
                            from collegeEvents.Events where collegeEvents.Events.IsActive=1 order by collegeEvents.Events.CreatedDate desc";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewEvent temp = new ViewEvent()
                        {
                            ID = Convert.ToInt64(reader["ID"].ToString()),
                            Title = Convert.ToString(reader["Title"]),
                            Event = Convert.ToString(reader["Event"]),
                            Event_Date = Convert.ToString(reader["EventDate"]),
                            EventVideo = Convert.ToString(reader["EventVideo"])
                        }; events.Add(temp);
                    }
                }
            }
            return events;
        }

        public ViewEvent getEventsView(long ID)
        {
            ViewEvent Ev = new ViewEvent();
            string Query = @"select collegeEvents.Events.ID,collegeEvents.Events.Title,collegeEvents.Events.Event,
                            convert(varchar, collegeEvents.Events.EventDate, 121) as EventDate,ISNULL(collegeEvents.Events.EventVideo,'') as EventVideo
                            from collegeEvents.Events where collegeEvents.Events.ID=@ID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", ID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Ev = new ViewEvent()
                        {
                            ID = Convert.ToInt64(reader["ID"].ToString()),
                            Title = Convert.ToString(reader["Title"]),
                            Event = Convert.ToString(reader["Event"]),
                            Event_Date = Convert.ToString(reader["EventDate"]),
                            EventVideo = Convert.ToString(reader["EventVideo"])
                        };
                    }
                }
            }
            return Ev;
        }

        public string updateEvent(ViewEvent Obj)
        {
            string result = string.Empty;
            User_Backend bck_end = new User_Backend();
            Global global = new Global();
            string res = global.ObjectNullChecking(Obj);
            ViewEvent obj = JsonConvert.DeserializeObject<ViewEvent>(res);
            string Query = @"update collegeEvents.Events set collegeEvents.Events.Title=@title,
                                    collegeEvents.Events.Event=@events,collegeEvents.Events.EventDate=@date,
                                        collegeEvents.Events.EventVideo=@video where collegeEvents.Events.ID=@ID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", obj.ID);
                    command.Parameters.AddWithValue("@title", obj.Title);
                    command.Parameters.AddWithValue("@events", obj.Event);
                    command.Parameters.AddWithValue("@date", Convert.ToDateTime(obj.Event_Date));
                    command.Parameters.AddWithValue("@video", obj.EventVideo);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        result = "Successfully updated";
                    }
                }
            }
            return result;
        }

        public bool deleteEvent(long ID)
        {
            bool res = false;
            string Query = @"delete from collegeEvents.Events where collegeEvents.Events.ID=@ID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", ID);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                    else { res = false; }
                }
            }
            return res;
        }

        public string InsertEvent(ViewEvent Obj)
        {
            Global global = new Global();
            string __obj = global.ObjectNullChecking(Obj);
            ViewEvent obj = JsonConvert.DeserializeObject<ViewEvent>(__obj);
            string result = string.Empty;
            string Query = @"insert into collegeEvents.Events
                            (collegeEvents.Events.Event,collegeEvents.Events.EventDate,
                            collegeEvents.Events.CreatedDate,collegeEvents.Events.Title,
                            collegeEvents.Events.CreatedBy,collegeEvents.Events.EventVideo) values(@event,@event_date,getDate(),
                            @title,@createdBy,@EventVideo)";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@event", obj.Event);
                    command.Parameters.AddWithValue("@event_date", Convert.ToDateTime(obj.Event_Date));
                    command.Parameters.AddWithValue("@title", obj.Title);
                    command.Parameters.AddWithValue("@createdBy", obj.CreatedBy);
                    command.Parameters.AddWithValue("@EventVideo", obj.EventVideo);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        result = "Successfully Inserted!";
                    }
                }
            }
            return result;
        }

        #endregion

        #region ADD QUESTIONS BY SITE ADMIN Indranil
        public List<string> GetYearBySubjectTypeForQuestionPanel()
        {
            List<string> Result = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))

                {

                    string query = @"with yearlist as 
                                (
                                    select 1970 as year
                                    union all
                                    select yl.year + 1 as year
                                    from yearlist yl
                                    where yl.year + 1 <= YEAR(GetDate())
                                )

                                select Year from yearlist order by Year desc;";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //QuestionPanel temp = new QuestionPanel();
                        //temp.Year = Convert.ToString(reader["Year"]);
                        //Result.Add(temp);

                        Result.Add(Convert.ToString(reader["Year"]));
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

        public List<AddTestQuestion> GetfilteredQuestionsByYear(Questionlist Info)
        {
            List<AddTestQuestion> QuestionList = new List<AddTestQuestion>();
            string Query = @"select ID,Question,ISNULL(Diagram,'n.a') As Diagram,ISNULL(Option1,'') as Option1
                            ,ISNULL(Option2,'') as Option2,ISNULL(Option3,'') as Option3
                            ,ISNULL(Option4,'') as Option4,ISNULL(Option5,'') as Option5,
                            ISNULL(CorrectOption,'') as CorrectOption,
                            ISNULL(QuestionTypeID,'') as QuestionTypeID
                            from admin.QuestionsByYear
                            where admin.QuestionsByYear.Year=@year and admin.QuestionsByYear.ExamID=@examId
                            and admin.QuestionsByYear.SubjectID=@subId order by CreatedDate desc";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                    con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand(Query, con);
                command.Parameters.AddWithValue("@year", Convert.ToInt64(Info.Year));
                command.Parameters.AddWithValue("@examId", Convert.ToInt64(Info.ExamType));
                command.Parameters.AddWithValue("@subId", Convert.ToInt64(Info.Subject));
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AddTestQuestion temp = new AddTestQuestion()
                        {
                            DescID = Convert.ToInt64(reader["ID"].ToString()),
                            Question = reader["Question"].ToString(),
                            Diagram = reader["Diagram"].ToString(),
                            Option1 = reader["Option1"].ToString(),
                            Option2 = reader["Option2"].ToString(),
                            Option3 = reader["Option3"].ToString(),
                            Option4 = reader["Option4"].ToString(),
                            Option5 = reader["Option5"].ToString(),
                            CorrectOption = reader["CorrectOption"].ToString(),
                            QuestionType = reader["QuestionTypeID"].ToString(),
                        }; QuestionList.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return QuestionList;
        }

        public List<newTopicList> GetfilteredQuestionsByTopic(long SubId)
        {
            List<newTopicList> QuestionList = new List<newTopicList>();
            string Query = @"select ID,Topic from studentinfo.ExamTopics 
                                where studentinfo.ExamTopics.SubjectID=@SubId";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@SubId", SubId);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        newTopicList temp = new newTopicList()
                        {
                            TopicID = Convert.ToInt64(reader["ID"].ToString()),
                            TopicText = Convert.ToString(reader["Topic"])
                        }; QuestionList.Add(temp);
                    }
                }
            }
            return QuestionList;
        }

        public List<AddTestQuestion> GetfilteredQuestionByTopic(Questionlist Info)
        {
            List<AddTestQuestion> QuestionList = new List<AddTestQuestion>();
            string Query = @"select ID,Question,ISNULL(Diagram,'n.a') As Diagram,ISNULL(Option1,'') as Option1
                            ,ISNULL(Option2,'') as Option2,ISNULL(Option3,'') as Option3
                            ,ISNULL(Option4,'') as Option4,ISNULL(Option5,'') as Option5,
                            ISNULL(CorrectOption,'') as CorrectOption,
                            ISNULL(QuestionTypeID,'') as QuestionTypeID
                            from admin.QuestionsByTopic
                            where admin.QuestionsByTopic.ExamID=@ExamId and admin.QuestionsByTopic.SubjectID=@SubId
                            and admin.QuestionsByTopic.TopicID=@TopicId order by CreatedDate desc";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@TopicId", Convert.ToInt64(Info.TopicId));
                    command.Parameters.AddWithValue("@ExamId", Convert.ToInt64(Info.ExamType));
                    command.Parameters.AddWithValue("@SubId", Convert.ToInt64(Info.Subject));
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            AddTestQuestion temp = new AddTestQuestion()
                            {
                                DescID = Convert.ToInt64(reader["ID"].ToString()),
                                Question = reader["Question"].ToString(),
                                Diagram = reader["Diagram"].ToString(),
                                Option1 = reader["Option1"].ToString(),
                                Option2 = reader["Option2"].ToString(),
                                Option3 = reader["Option3"].ToString(),
                                Option4 = reader["Option4"].ToString(),
                                Option5 = reader["Option5"].ToString(),
                                CorrectOption = reader["CorrectOption"].ToString(),
                                QuestionType = reader["QuestionTypeID"].ToString(),
                            };
                            QuestionList.Add(temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return QuestionList;

        }

        public AddTestQuestion getQuestionView(string ID, string QuestionType)
        {
            string Query = string.Empty;
            AddTestQuestion questions = new AddTestQuestion();
            if (QuestionType == "year")
            {
                Query = @"select admin.QuestionsByYear.ID,admin.QuestionsByYear.Question,
                        ISNULL(admin.QuestionsByYear.Diagram,'n.a') as Diagram,ISNULL(admin.QuestionsByYear.Option1,'') as Option1,
                        ISNULL(admin.QuestionsByYear.Option2,'') as Option2,ISNULL(admin.QuestionsByYear.Option3,'') as Option3,
                        ISNULL(admin.QuestionsByYear.Option4,'') as Option4,ISNULL(admin.QuestionsByYear.Option5,'') as Option5,
                        ISNULL(admin.QuestionsByYear.CorrectOption,'') as CorrectOption
                        from admin.QuestionsByYear where ID=@ID";
                questions.QuestionType = "year";
            }
            else if (QuestionType == "topic")
            {
                Query = @"select admin.QuestionsByTopic.ID,admin.QuestionsByTopic.Question,
                        ISNULL(admin.QuestionsByTopic.Diagram,'n.a') as Diagram,ISNULL(admin.QuestionsByTopic.Option1,'') as Option1,
                        ISNULL(admin.QuestionsByTopic.Option2,'') as Option2,ISNULL(admin.QuestionsByTopic.Option3,'') as Option3,
                        ISNULL(admin.QuestionsByTopic.Option4,'') as Option4,ISNULL(admin.QuestionsByTopic.Option5,'') as Option5,
                        ISNULL(admin.QuestionsByTopic.CorrectOption,'') as CorrectOption
                        from admin.QuestionsByTopic where ID=@ID";
                questions.QuestionType = "topic";
            }
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt64(ID));
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        questions.DescID = Convert.ToInt64(reader["ID"].ToString());
                        questions.Question = reader["Question"].ToString();
                        questions.Diagram = reader["Diagram"].ToString();
                        questions.Option1 = reader["Option1"].ToString();
                        questions.Option2 = reader["Option2"].ToString();
                        questions.Option3 = reader["Option3"].ToString();
                        questions.Option4 = reader["Option4"].ToString();
                        questions.Option5 = reader["Option5"].ToString();
                        questions.CorrectOption = reader["CorrectOption"].ToString();
                    }
                }
            }
            return questions;
        }

        public bool UpdateQuestions(AddTestQuestion questions)
        {
            bool res = false;
            Global global = new Global();
            string ques = global.ObjectNullChecking(questions);
            AddTestQuestion question = JsonConvert.DeserializeObject<AddTestQuestion>(ques);
            string Query = string.Empty;
            if (question.QuestionType == "year")
            {
                Query = @"update admin.QuestionsByYear set
                    admin.QuestionsByYear.Question=@Question,admin.QuestionsByYear.Diagram=@Diagram,
                    admin.QuestionsByYear.Option1=@Option1,admin.QuestionsByYear.Option2=@Option2,
                    admin.QuestionsByYear.Option3=@Option3,admin.QuestionsByYear.Option4=@Option4,
                    admin.QuestionsByYear.Option5=@Option5,admin.QuestionsByYear.CorrectOption=@CorrectOption
                    where admin.QuestionsByYear.ID=@ID";
            }
            else if (question.QuestionType == "topic")
            {
                Query = @"update admin.QuestionsByTopic set
                    admin.QuestionsByTopic.Question=@Question,admin.QuestionsByTopic.Diagram=@Diagram,
                    admin.QuestionsByTopic.Option1=@Option1,admin.QuestionsByTopic.Option2=@Option2,
                    admin.QuestionsByTopic.Option3=@Option3,admin.QuestionsByTopic.Option4=@Option4,
                    admin.QuestionsByTopic.Option5=@Option5,admin.QuestionsByTopic.CorrectOption=@CorrectOption
                    where admin.QuestionsByTopic.ID=@ID";
            }
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                    command.Parameters.AddWithValue("@Question", question.Question);
                    command.Parameters.AddWithValue("@Diagram", question.Diagram);
                    command.Parameters.AddWithValue("@Option1", question.Option1);
                    command.Parameters.AddWithValue("@Option2", question.Option2);
                    command.Parameters.AddWithValue("@Option3", question.Option3);
                    command.Parameters.AddWithValue("@Option4", question.Option4);
                    command.Parameters.AddWithValue("@Option5", question.Option5);
                    command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                }
            }
            return res;
        }

        public bool deleteQuestions(string ID, string Type)
        {
            bool res = false;
            string Query = string.Empty;
            if (Type == "year")
            {
                Query = @"delete from admin.QuestionsByYear where ID=@ID";
            }
            else if (Type == "topic")
            {
                Query = @"delete from admin.QuestionsByTopic where ID=@ID";
            }
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt64(ID));
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                }
            }
            return res;
        }
        #endregion

        #region ADD QUESTIONS BY SITE ADMIN

        public string InsertQuestion(AddTestQuestion Info)
        {
            string Res = string.Empty;
            try
            {

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string Query = string.Empty;
                    if (Info.ExamPattern == "year")
                    {
                        if (!(CheckYearWiseQuesExits(Info.Question)))
                        {
                            if (!(CheckSerialNoYearWise(Info.SerialNo, Info.Year, Info.SubjectID)))
                            {
                                if (Info.DescriptionText != "" && Info.DescriptionText != null)
                                {

                                    Info.DescID = CheckDesctextExits(Info.DescriptionText);
                                    if (Info.DescID == 0)
                                    {
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText)
                                    values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText)
                                    values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,'',@SerialNo)";
                                            }
                                        }
                                        else
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText)
                                    values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,@Option5,,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText)
                                    values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,'',@SerialNo)";
                                            }
                                        }

                                        SqlCommand cmd = new SqlCommand(Query, con);
                                        cmd.Parameters.AddWithValue("@DescText", Info.DescriptionText);
                                        cmd.Parameters.AddWithValue("@Question", Info.Question);
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            cmd.Parameters.AddWithValue("@Diagram", Info.Diagram);
                                        }
                                        cmd.Parameters.AddWithValue("@Year", Info.Year);
                                        cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                        cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                        cmd.Parameters.AddWithValue("@Option1", Info.Option1);
                                        cmd.Parameters.AddWithValue("@Option2", Info.Option2);
                                        cmd.Parameters.AddWithValue("@Option3", Info.Option3);
                                        cmd.Parameters.AddWithValue("@Option4", Info.Option4);
                                        cmd.Parameters.AddWithValue("@QuestionTypeID", Info.QuestionTypeID);
                                        cmd.Parameters.AddWithValue("@CorrectOption", Info.CorrectOption);
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            cmd.Parameters.AddWithValue("@Option5", Info.Option5);
                                        }
                                        cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                        con.Open();
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            Res = "Success! Question Added";
                                        }
                                        else
                                        {
                                            Res = "Failed ! Failed to add question";
                                        }
                                        con.Close();
                                    }
                                    else
                                    {
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                            }
                                        }
                                        else
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                            }
                                        }

                                        SqlCommand cmd = new SqlCommand(Query, con);

                                        cmd.Parameters.AddWithValue("@Question", Info.Question);
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            cmd.Parameters.AddWithValue("@Diagram", Info.Diagram);
                                        }
                                        cmd.Parameters.AddWithValue("@Year", Info.Year);
                                        cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                        cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                        cmd.Parameters.AddWithValue("@Option1", Info.Option1);
                                        cmd.Parameters.AddWithValue("@Option2", Info.Option2);
                                        cmd.Parameters.AddWithValue("@Option3", Info.Option3);
                                        cmd.Parameters.AddWithValue("@Option4", Info.Option4);
                                        cmd.Parameters.AddWithValue("@DescID", Info.DescID);
                                        cmd.Parameters.AddWithValue("@QuestionTypeID", Info.QuestionTypeID);
                                        cmd.Parameters.AddWithValue("@CorrectOption", Info.CorrectOption);
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            cmd.Parameters.AddWithValue("@Option5", Info.Option5);
                                        }
                                        cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                        con.Open();
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            Res = "Success! Question Added";
                                        }
                                        else
                                        {
                                            Res = "Failed ! Failed to add question";
                                        }
                                        con.Close();

                                    }
                                }
                                else
                                {
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        if ((Info.Option5 != "" && Info.Option5 != null))
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                        }

                                    }
                                    else
                                    {
                                        if ((Info.Option5 != "" && Info.Option5 != null))
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                        }
                                    }

                                    SqlCommand cmd = new SqlCommand(Query, con);

                                    cmd.Parameters.AddWithValue("@Question", Info.Question);
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Diagram", Info.Diagram);
                                    }
                                    cmd.Parameters.AddWithValue("@Year", Info.Year);
                                    cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@Option1", Info.Option1);
                                    cmd.Parameters.AddWithValue("@Option2", Info.Option2);
                                    cmd.Parameters.AddWithValue("@Option3", Info.Option3);
                                    cmd.Parameters.AddWithValue("@Option4", Info.Option4);
                                    //cmd.Parameters.AddWithValue("@DescID", Info.DescID);
                                    cmd.Parameters.AddWithValue("@QuestionTypeID", Info.QuestionTypeID);
                                    cmd.Parameters.AddWithValue("@CorrectOption", Info.CorrectOption);
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Option5", Info.Option5);
                                    }
                                    cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                    con.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        Res = "Success! Question Added";
                                    }
                                    else
                                    {
                                        Res = "Failed ! Failed to add question";
                                    }
                                    con.Close();


                                }
                            }
                            else
                            {
                                Res = "Failed! Question SerialNo already exists";
                            }



                        }
                        else
                        {
                            Res = "Failed! you have already added this question";
                        }
                        con.Close();


                    }
                    else
                    //add topic wise ques//
                    {
                        if (!(CheckTopicWiseQuesExits(Info.Question)))
                        {

                            if (!(CheckSerialNoTopicWise(Info.SerialNo, Info.TopicID)))
                            {
                                if (Info.DescriptionText != "" && Info.DescriptionText != null)
                                {
                                    Info.DescID = CheckDesctextExits(Info.DescriptionText);
                                    if (Info.DescID == 0)
                                    {

                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @" DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                (DescriptionText)
                                                values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @" DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                (DescriptionText)
                                                values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                            }
                                        }
                                        else
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {

                                                Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                    (DescriptionText)
                                                    values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                    (DescriptionText)
                                                    values(@DescText);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                            }
                                        }

                                        SqlCommand cmd = new SqlCommand(Query, con);

                                        cmd.Parameters.AddWithValue("@ques", Info.Question);
                                        cmd.Parameters.AddWithValue("@DescText", Info.DescriptionText);
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            cmd.Parameters.AddWithValue("@diag", Info.Diagram);
                                        }
                                        cmd.Parameters.AddWithValue("@topic", Info.Topic);
                                        cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                                        cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                                        cmd.Parameters.AddWithValue("@topicid", Info.TopicID);
                                        cmd.Parameters.AddWithValue("@opt1", Info.Option1);
                                        cmd.Parameters.AddWithValue("@opt2", Info.Option2);
                                        cmd.Parameters.AddWithValue("@opt3", Info.Option3);
                                        cmd.Parameters.AddWithValue("@opt4", Info.Option4);
                                        cmd.Parameters.AddWithValue("@questype", Info.QuestionTypeID);
                                        cmd.Parameters.AddWithValue("@correctopn", Info.CorrectOption);
                                        cmd.Parameters.AddWithValue("@marks", Info.Marks);
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            cmd.Parameters.AddWithValue("@optn5", Info.Option5);
                                        }
                                        cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                        con.Open();
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            Res = "Success! Question Added";
                                        }
                                        else
                                        {
                                            Res = "Failed ! to add question";
                                        }
                                    }

                                    else
                                    {
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                            }

                                        }
                                        else
                                        {
                                            if (Info.Option5 != "" && Info.Option5 != null)
                                            {
                                                Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                            }
                                            else
                                            {
                                                Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                            }


                                        }
                                        SqlCommand cmd = new SqlCommand(Query, con);

                                        cmd.Parameters.AddWithValue("@ques", Info.Question);
                                        if (Info.Diagram != "" && Info.Diagram != null)
                                        {
                                            cmd.Parameters.AddWithValue("@diag", Info.Diagram);
                                        }
                                        cmd.Parameters.AddWithValue("@topic", Info.Topic);
                                        cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                                        cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                                        cmd.Parameters.AddWithValue("@topicid", Info.TopicID);


                                        cmd.Parameters.AddWithValue("@opt1", Info.Option1);
                                        cmd.Parameters.AddWithValue("@opt2", Info.Option2);
                                        cmd.Parameters.AddWithValue("@opt3", Info.Option3);
                                        cmd.Parameters.AddWithValue("@opt4", Info.Option4);

                                        cmd.Parameters.AddWithValue("@DescID", Info.DescID);
                                        cmd.Parameters.AddWithValue("@questype", Info.QuestionTypeID);
                                        cmd.Parameters.AddWithValue("@correctopn", Info.CorrectOption);
                                        cmd.Parameters.AddWithValue("@marks", Info.Marks);
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            cmd.Parameters.AddWithValue("@optn5", Info.Option5);
                                        }
                                        cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);

                                        con.Open();
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            Res = "Success! Question Added";
                                        }
                                        else
                                        {
                                            Res = "Failed ! to add question";
                                        }
                                        con.Close();




                                    }
                                }

                                else
                                {
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@questype,@correctopn,@marks,'',@SerialNo)";

                                        }
                                    }
                                    else
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@questype,@correctopn,@marks,'',@SerialNo)";


                                        }
                                    }
                                    SqlCommand cmd = new SqlCommand(Query, con);

                                    cmd.Parameters.AddWithValue("@ques", Info.Question);
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        cmd.Parameters.AddWithValue("@diag", Info.Diagram);
                                    }
                                    cmd.Parameters.AddWithValue("@topic", Info.Topic);
                                    cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@topicid", Info.TopicID);


                                    cmd.Parameters.AddWithValue("@opt1", Info.Option1);
                                    cmd.Parameters.AddWithValue("@opt2", Info.Option2);
                                    cmd.Parameters.AddWithValue("@opt3", Info.Option3);
                                    cmd.Parameters.AddWithValue("@opt4", Info.Option4);


                                    cmd.Parameters.AddWithValue("@questype", Info.QuestionTypeID);
                                    cmd.Parameters.AddWithValue("@correctopn", Info.CorrectOption);
                                    cmd.Parameters.AddWithValue("@marks", Info.Marks);
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        cmd.Parameters.AddWithValue("@optn5", Info.Option5);

                                    }

                                    cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                    con.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        Res = "Success! Question Added";
                                    }
                                    else
                                    {
                                        Res = "Failed ! to add question";
                                    }
                                    con.Close();

                                }
                            }
                            else
                            {
                                Res = "Failed! This Question SerialNo already exists";
                            }

                        }
                        else
                        {
                            Res = "Failed! you have already added this question";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Res;
        }

        public List<TypeOfQues> GetQuestionType()
        {
            List<TypeOfQues> Result = new List<TypeOfQues>();
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select * from admin.QuestionType where Status=1";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TypeOfQues temp = new TypeOfQues();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.QuestionType = Convert.ToString(reader["QuestionType"]);
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

        public bool CheckTopicWiseQuesExits(string Question)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string query = @"select ID,Question from 
                                admin.QuestionsByTopic
                                where  Question=@ques";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ques", Question);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public bool CheckYearWiseQuesExits(string Question)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    //string query = @"select ID,Question from admin.QuestionsByTopic where ID=@quesid";
                    string query = @"select Question from admin.QuestionsByYear where Question=@ques";
                    SqlCommand cmd = new SqlCommand(query, con);
                    //  cmd.Parameters.AddWithValue("@quesid", QuestionID);
                    cmd.Parameters.AddWithValue("@ques", Question);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public long CheckDesctextExits(string Description)
        {
            long result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string query = @"select ID from admin.QuestionsDescription where DescriptionText=@desc";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@desc", Description);

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

        #region SubjectPannel

        public List<ExamSubject> SubjectDetailsForSiteAdmin()
        {
            List<ExamSubject> Result = new List<ExamSubject>();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"select studentinfo.ExamSubjects.ID,studentinfo.ExamSubjects.Subject,studentinfo.ExamSubjects.ExamID,
studentinfo.ExamType.ExamType from studentinfo.Examsubjects inner join studentinfo.ExamType on studentinfo.ExamSubjects.ExamID=studentinfo.ExamType.ID";

                SqlCommand com = new SqlCommand(Query, con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ExamSubject temp = new ExamSubject();

                    temp.ID = Convert.ToInt32(reader["ID"]);
                    temp.Subject = Convert.ToString(reader["Subject"]);
                    temp.ExamID = Convert.ToInt32(reader["ExamID"]);
                    temp.ExamType = Convert.ToString(reader["ExamType"]);


                    Result.Add(temp);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }
        public List<ExamTypeDetails> GetExamTypeForSiteAdmin()
        {
            List<ExamTypeDetails> Result = new List<ExamTypeDetails>();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

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
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string AddSubjectBySiteAdmin(ExamSubject Data)
        {
            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());
                if (!(CheckSubjectExists(Data.ExamID, Data.Subject)))
                {
                    string Query = @" insert into studentinfo.ExamSubjects(Subject,ExamID)values(@Sub,@EID)";


                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Sub", Data.Subject);
                    com.Parameters.AddWithValue("@EID", Data.ExamID);

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Subject Inserted";
                    }
                    else
                    {
                        Result = "Failed!Failed subject Insertion";
                    }
                }
                else
                {
                    Result = "!Already exists";
                }
                con.Close();


            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckSubjectExists(long ExamID, string Subject)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select Subject from studentinfo.ExamSubjects
                                    where ExamID=@exmid and Subject=@sub";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@exmid", ExamID);
                    cmd.Parameters.AddWithValue("@sub", Subject);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public EditSubject_SiteAdmin EditSubjectBySiteAdmin(long SID)
        {
            EditSubject_SiteAdmin Result = new EditSubject_SiteAdmin();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string query = @"select ID,Subject,ExamID from studentinfo.ExamSubjects Where ID=@SID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SID", SID);


                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Result.SubjectID = Convert.ToInt32(reader["ID"]);
                    Result.Subject = Convert.ToString(reader["Subject"]);
                    Result.ExamID = Convert.ToInt32(reader["ExamID"]);


                    break;
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string UpdateSubjectBySiteAdmin(EditSubject_SiteAdmin Data)
        {
            string Result = string.Empty;
            try
            {
                if (Data.OldSub == Data.Subject && Data.ExamID == Data.OldExamID)
                {
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                    string Query = @"update studentinfo.ExamSubjects set Subject=@Sub, ExamID=@EID where ID=@SID";


                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Sub", Data.Subject);
                    com.Parameters.AddWithValue("@EID", Data.ExamID);
                    com.Parameters.AddWithValue("@SID", Data.SubjectID);

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Subject Updated";
                    }
                    else
                    {
                        Result = "Failed!Failed subject Update";
                    }
                    con.Close();

                }
                else
                {
                    if (!CheckSubExists(Data.ExamID, Data.Subject))
                    {
                        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                        string Query = @"update studentinfo.ExamSubjects set Subject=@Sub, ExamID=@EID where ID=@SID";


                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@Sub", Data.Subject);
                        com.Parameters.AddWithValue("@EID", Data.ExamID);
                        com.Parameters.AddWithValue("@SID", Data.SubjectID);

                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Subject Updated";
                        }
                        else
                        {
                            Result = "Failed!Failed subject Update";
                        }
                        con.Close();

                    }
                    else
                    {
                        Result = "Failed!This Subject already exists";

                    }



                }



            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckSubExists(long ExamID, string subject)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select Subject from studentinfo.ExamSubjects Where ExamID=@EID and Subject=@Sub";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EID", ExamID);
                    cmd.Parameters.AddWithValue("@Sub", subject);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public string DeleteSubjectBySiteAdmin(long ID)
        {
            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());


                SqlCommand com = new SqlCommand("admin.DELT_SUB", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SubID", ID);
                con.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Result = "Success!Deleted";
                }
                else
                {
                    Result = "Failed!Deleted";
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public long GetSubIDBySiteAdmin(long ID)
        {
            SubjectDetails_SiteAdmin Result = new SubjectDetails_SiteAdmin();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string query = @"select ID from studentinfo.ExamSubjects where ID=@ID";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", ID);


                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Result.SID = Convert.ToInt64(reader["ID"]);

                    break;
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result.SID;
        }

        #endregion

        #region ADD syllabus by sneha

        //public string InsertSyllabus(AddSyllabus Info)
        //{
        //    string result = string.Empty;
        //    string[] contentList = Info.TopicContent.Split('|');
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
        //        {

        //            string Query = string.Empty;
        //            long TopicId = HasExamTopic(Info.Topic);

        //            if (TopicId == 0)
        //            {
        //                long TID = InsertOnlyTopicTable(Info.ExamID, Info.SubjectID, Info.Topic);
        //                for (var i = 0; i < contentList.Length - 1; i++)
        //                {
        //                    Query = @"insert into  studentinfo.ExamContents 
        //                        (TopicID,TopicContent,ExamID,SubjectID)
        //                        values(@TopicID,@TopicContent,@ExamID,@SubjectID)";

        //                    SqlCommand cmd = new SqlCommand(Query, con);

        //                    cmd.Parameters.AddWithValue("@TopicID", TID);

        //                    cmd.Parameters.AddWithValue("@TopicContent", contentList[i]);

        //                    cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
        //                    cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
        //                    con.Open();
        //                    if (cmd.ExecuteNonQuery() > 0)
        //                    {
        //                        result = "Success! Syllabus Content added";
        //                    }
        //                    else
        //                    {
        //                        result = "Failed ! to add Syllabus Content";
        //                    }
        //                    con.Close();
        //                }


        //            }
        //            else
        //            {
        //                if (!(CheckContentExists(Info)))
        //                {
        //                    for (var i = 0; i < contentList.Length - 1; i++)
        //                    {
        //                        Query = @" insert into  studentinfo.ExamContents 
        //                                (TopicID,TopicContent,ExamID,SubjectID)
        //                                values(@TopicID,@TopicContent,@ExamID,@SubjectID)";

        //                        SqlCommand cmd = new SqlCommand(Query, con);

        //                        cmd.Parameters.AddWithValue("@TopicID", TopicId);

        //                        cmd.Parameters.AddWithValue("@TopicContent", contentList[i]);

        //                        cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
        //                        cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
        //                        con.Open();
        //                        if (cmd.ExecuteNonQuery() > 0)
        //                        {
        //                            result = "Success! Syllabus Content added";
        //                        }
        //                        else
        //                        {
        //                            result = "Failed ! to add Syllabus Content";
        //                        }
        //                        con.Close();
        //                    }
        //                }
        //                else
        //                {
        //                    result = "! You can't add same content twice under a particular topic";
        //                }
        //            }



        //        }


        //    }
        //    catch (Exception ex)
        //    {


        //    }

        //    return result;
        //}

        public string InsertSyllabus(AddSyllabus Info)
        {
            string result = string.Empty;
            string[] contentList = Info.TopicContent.Split('|');

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string Query = string.Empty;
                    long TopicId = HasExamTopic(Info.Topic);

                    if (TopicId == 0)
                    {
                        long TID = InsertOnlyTopicTable(Info.ExamID, Info.SubjectID, Info.Topic);
                        for (var i = 0; i < contentList.Length - 1; i++)
                        {
                            Query = @"insert into  studentinfo.ExamContents 
                                (TopicID,TopicContent,ExamID,SubjectID)
                                values(@TopicID,@TopicContent,@ExamID,@SubjectID)";

                            SqlCommand cmd = new SqlCommand(Query, con);

                            cmd.Parameters.AddWithValue("@TopicID", TID);

                            cmd.Parameters.AddWithValue("@TopicContent", contentList[i]);

                            cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                            cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                            con.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                result = "Success! Syllabus Content added";
                            }
                            else
                            {
                                result = "Failed! to add Syllabus Content";
                            }
                            con.Close();
                        }


                    }
                    else
                    {


                        if ((CheckContentExists(Info)))
                        {
                            for (var i = 0; i < contentList.Length - 1; i++)
                            {
                                Query = @" insert into  studentinfo.ExamContents 
                                        (TopicID,TopicContent,ExamID,SubjectID)
                                        values(@TopicID,@TopicContent,@ExamID,@SubjectID)";

                                SqlCommand cmd = new SqlCommand(Query, con);

                                cmd.Parameters.AddWithValue("@TopicID", TopicId);

                                cmd.Parameters.AddWithValue("@TopicContent", contentList[i]);

                                cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                con.Open();
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    result = "Success! Syllabus Content added";
                                }
                                else
                                {
                                    result = "Failed ! to add Syllabus Content";
                                }
                                con.Close();
                            }
                        }
                        else
                        {
                            result = "! You can't add same content twice under a particular topic";
                        }
                    }



                }


            }
            catch (Exception ex)
            {


            }

            return result;
        }


        //public long HasExamTopic(string Topic)
        //{
        //    long result = 0;
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
        //        {
        //            string query = @"select ID from studentinfo.ExamTopics where Topic=@Topic";
        //            SqlCommand cmd = new SqlCommand(query, con);
        //            cmd.Parameters.AddWithValue("@Topic", Topic);

        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                result = Convert.ToInt64(dt.Rows[0][0].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return result;
        //    }
        //    return result;
        //}

        //public long InsertOnlyTopicTable(long ExamID, long SubjectID, string Topic)
        //{
        //    long Result = 0;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
        //        {
        //            SqlCommand com = new SqlCommand("SP_InsertonlyTopic", con);
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@Topic", Topic);
        //            com.Parameters.AddWithValue("@EID", ExamID);
        //            com.Parameters.AddWithValue("@SID", SubjectID);

        //            com.Parameters.Add("@ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
        //            con.Open();
        //            if (com.ExecuteNonQuery() > 0)
        //            {
        //                string id = com.Parameters["@ID"].Value.ToString();
        //                Result = Convert.ToInt32(id);
        //            }
        //            else
        //            {
        //                Result = 0;
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

        //public bool CheckContentExists(AddSyllabus Info)
        //{
        //    bool res = false;
        //    string[] contentList = Info.TopicContent.Split('|');
        //    Info.TopicId = HasExamTopic(Info.Topic);


        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
        //        {

        //            string query = @"select TopicContent
        //                            from studentinfo.ExamContents
        //                            where TopicID=@topicid and ExamID=@examid and SubjectID=@subid and TopicContent=@TopicContent";
        //            SqlCommand cmd = new SqlCommand(query, con);

        //            cmd.Parameters.AddWithValue("@topicid", Info.TopicId);
        //            cmd.Parameters.AddWithValue("@examid", Info.ExamID);
        //            cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
        //            cmd.Parameters.AddWithValue("TopicContent", Info.TopicContent);


        //            SqlDataAdapter da = new SqlDataAdapter(cmd);

        //            DataTable dt = new DataTable();
        //            da.Fill(dt);
        //            DataRow dr = dt.NewRow();
        //            dr[0] = contentList.Length - 1;
        //            dt.Rows.Add(dr);
        //            if (dt.Rows.Count > 0)
        //            {
        //                res = true;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        res = false;
        //    }
        //    return res;
        //}


        public long HasExamTopic(string Topic)
        {
            long result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string query = @"select ID from studentinfo.ExamTopics where Topic=@Topic";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Topic", Topic);

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




        public long InsertOnlyTopicTable(long ExamID, long SubjectID, string Topic)
        {
            long Result = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    SqlCommand com = new SqlCommand("SP_InsertonlyTopic", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Topic", Topic);
                    com.Parameters.AddWithValue("@EID", ExamID);
                    com.Parameters.AddWithValue("@SID", SubjectID);

                    com.Parameters.Add("@ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        string id = com.Parameters["@ID"].Value.ToString();
                        Result = Convert.ToInt64(id);
                    }
                    else
                    {
                        Result = 0;
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


        public bool CheckContentExists(AddSyllabus Info)
        {

            bool res = false;



            long TopicId = HasExamTopic(Info.Topic);


            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {


                    SqlCommand cmd = new SqlCommand("SP_CheckContent", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@topicid", TopicId);
                    cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                    cmd.Parameters.AddWithValue("@TopicContent", Info.TopicContent);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DataRow dr = dt.NewRow();
                    //dr[0] = contentList.Length - 1;
                    dt.Rows.Add(dr);

                    if (dt.Rows.Count > 0)
                    {
                        res = Convert.ToBoolean(dt.Rows[0]["chkStatus"]);
                    }


                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }




        public List<AddSyllabus> GetSyllabusListBySubId(SyllabusList Info)
        {
            List<AddSyllabus> List = new List<AddSyllabus>();

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                try
                {
                    string Query = @"select isnull(tpc.Topic,'') Topic,isnull(tpc.ID,'') as TopicID,isnull(cnt.TopicContent,'') TopicContent,isnull(cnt.ID,'')as ContentID
                                    from  studentinfo.ExamContents as cnt
                                    inner join studentinfo.ExamTopics as tpc
                                    on cnt.TopicID=tpc.ID where cnt.ExamId=@emxId and cnt.SubjectID=@subid";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@emxId", Info.ExamID);
                    com.Parameters.AddWithValue("@subid", Info.SubjectID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AddSyllabus temp = new AddSyllabus();
                        temp.ContentID = Convert.ToInt64(reader["ContentID"]);
                        temp.Topic = Convert.ToString(reader["Topic"]);
                        temp.TopicContent = Convert.ToString(reader["TopicContent"]);
                        temp.TopicId = Convert.ToInt64(reader["TopicID"]);
                        List.Add(temp);
                    }
                    con.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }



            }
            return List;
        }

        public SyllabusList EditSyllabusContent(long ID, long TopicID)
        {
            SyllabusList Result = new SyllabusList();
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    string Query = @"select isnull(exm.Examtype,'') Examtype,exm.ID as ExamID,isnull(sub.Subject,'') Subject,sub.ID as SubjectID,isnull(tpc.Topic,'') Topic,tpc.ID as TopicID,isnull(cnt.TopicContent,'') TopicContent,cnt.ID as ContentID
                                        from studentinfo.ExamContents as cnt  
										 left join studentinfo.ExamType as exm on cnt.ExamID=exm.ID 
                                        left join studentinfo.ExamSubjects as sub on cnt.SubjectID=sub.ID
                                        left join studentinfo.ExamTopics as tpc on cnt.TopicID=tpc.ID where cnt.ID=@contid";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@contid", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        Result.ExamType = Convert.ToString(reader["ExamType"]);
                        Result.ExamID = Convert.ToInt64(reader["ExamID"]);
                        Result.Subject = Convert.ToString(reader["Subject"]);
                        Result.SubjectID = Convert.ToInt64(reader["SubjectID"]);
                        Result.Topic = Convert.ToString(reader["Topic"]);
                        Result.TopicId = Convert.ToInt64(reader["TopicId"]);
                        Result.TopicContent = Convert.ToString(reader["TopicContent"]);
                        Result.ContentID = Convert.ToInt64(reader["ContentID"]);
                        //Result.Add(temp);
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


        // syllabus update//
        public string UpdateSyllabus(AddSyllabus Info)
        {
            string result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {


                    string Query = @"update studentinfo.ExamTopics set Topic=@topic,ExamID=@exmid,SubjectID=@subid
                                        where ID=@topicid
                                        Update studentinfo.ExamContents set TopicID=@topicid,TopicContent=@content,ExamID=@exmid,SubjectID=@subid
                                        where TopicID=@topicid and ID=@contid";

                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.Parameters.AddWithValue("@topic", Info.Topic);
                    cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                    cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                    cmd.Parameters.AddWithValue("@topicid", Info.TopicId);
                    cmd.Parameters.AddWithValue("@content", Info.TopicContent);
                    cmd.Parameters.AddWithValue("@contid", Info.ContentID);

                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = "Success! Syllabus Content Updated";
                    }
                    else
                    {
                        result = "Failed ! to Update Syllabus Content";
                    }
                    con.Close();


                }


            }
            catch (Exception ex)
            {


            }

            return result;
        }



        // syllabus delete//
        public string DeleteSyllabus(AddSyllabus Info)
        {
            string result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {


                    string Query = @"delete from studentinfo.ExamContents where ID=@contentid";

                    SqlCommand cmd = new SqlCommand(Query, con);


                    cmd.Parameters.AddWithValue("@contentid", Info.ContentID);

                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = "Success! Syllabus Content deleted";
                    }
                    else
                    {
                        result = "Failed ! to delete Syllabus Content";
                    }
                    con.Close();


                }


            }
            catch (Exception ex)
            {


            }

            return result;
        }


        #endregion

        #region Delete site admin reports by Indranil

        public string deleteSiteAdminReports(long id, string type)
        {
            string res = string.Empty;
            string Query = string.Empty;
            if (type == "OnlineCommunity")
            {
                Query = @"update studentinfo.Online_Community_Comment set studentinfo.Online_Community_Comment.IsDeleted=1
                        from studentinfo.Online_Community_Comment inner join site_admin.CommentReport on
                        studentinfo.Online_Community_Comment.Community_Comment_ID=site_admin.CommentReport.CommentID
                        where site_admin.CommentReport.ReportId=@ReportId;update site_admin.CommentReport 
                        set site_admin.CommentReport.IsDeleted=1 where site_admin.CommentReport.ReportId=@ReportId";
            }
            else if (type == "StudyGroup")
            {
                Query = @"update studentinfo.Study_Group_Comment set studentinfo.Study_Group_Comment.IsActive=0
                        from studentinfo.Study_Group_Comment inner join site_admin.CommentReport on
                        studentinfo.Study_Group_Comment.Group_Comment_ID=site_admin.CommentReport.CommentID
                        where site_admin.CommentReport.ReportId=@ReportId;update site_admin.CommentReport 
                        set site_admin.CommentReport.IsDeleted=1 where site_admin.CommentReport.ReportId=@ReportId";
            }
            else if (type == "Event")
            {

                Query = @"update collegeEvents.EventComments set collegeEvents.EventComments.IsDeleted=1
                          from collegeEvents.EventComments inner join site_admin.CommentReport on
                          collegeEvents.EventComments.ID=site_admin.CommentReport.CommentID
                          where site_admin.CommentReport.ReportId=@ReportId;update site_admin.CommentReport 
                          set site_admin.CommentReport.IsDeleted=1 where site_admin.CommentReport.ReportId=@ReportId";
            }
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ReportId", id);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = "Comment successfully removed!";
                    }
                }
            }
            return res;
        }

        #endregion

        #region Edit news By Indranil
        public Newsfeed getNewsForSiteAdminView(long ID)
        {
            Newsfeed feed = new Newsfeed();
            string Query = @"select admin.Newsfeed.ID,admin.Newsfeed.Title,admin.Newsfeed.Description,ISNULL(admin.Newsfeed.EventImage,'') EventImage,
                            Convert(varchar,admin.Newsfeed.CreatedDate,121) CreatedDate,admin.Newsfeed.Status from admin.Newsfeed
                            where admin.Newsfeed.ID=@ID";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", ID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        feed.ID = Convert.ToInt64(reader["ID"]);
                        feed.Title = Convert.ToString(reader["Title"]);
                        feed.Description = Convert.ToString(reader["Description"]);
                        feed.Created_Date = Convert.ToString(reader["CreatedDate"]);
                        feed.EventImage = Convert.ToString(reader["EventImage"]);
                        feed.Status = Convert.ToBoolean(reader["Status"]);
                    }
                }
            }
            return feed;
        }
        #endregion

        #region SiteAdmin last message 18.03.2019

        public long getSiteAdminMessageCount(long UserId)
        {
            long Count = 0;
            string Query = @"select count(*) MessageCount
                            from admin.LiveMessageInfo
                            where admin.LiveMessageInfo.SenderTypeID=0 and
                            admin.LiveMessageInfo.ReceiverID=@Userid or
                            admin.LiveMessageInfo.SenderID=@Userid and
                            admin.LiveMessageInfo.ReceiverTypeID=0";
            using (SqlConnection connection =
                        new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand(Query);
                    command.Parameters.AddWithValue("@Userid", UserId);
                    command.Connection = connection;
                    if (connection.State == System.Data.ConnectionState.Broken ||
                           connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Count = Convert.ToInt64(reader["MessageCount"]);
                    }
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteErrorLog(String.Format("ErrorLog {0}", DateTime.Today), ex.Message);
                }
            }
            return Count;
        }

        public SocialOrSiteAdminUserlastMessage SiteAdminLastMessages(long UserId)
        {
            SocialOrSiteAdminUserlastMessage message = new SocialOrSiteAdminUserlastMessage();
            string Query = @"select top(1) 
                            ISNULL(studentinfo.Student_Details.Stu_Name,'Tutor') Stu_Name,
                            admin.LiveMessageInfo.ID,
                            admin.LiveMessageInfo.SenderID,
                            admin.LiveMessageInfo.ReceiverID,
                            ISNULL(admin.LiveMessageInfo.Message,'') Message,
                            CONVERT(varchar,admin.LiveMessageInfo.CreatedDate,121) CreatedDate,
                            admin.LiveMessageInfo.IsRead,
                            admin.LiveMessageInfo.IsDelete,
                            ISNULL(studentinfo.Student_Details.Stu_Profile_Image,'') ProfileImage,
                            admin.LiveMessageInfo.SenderTypeID,
                            admin.LiveMessageInfo.ReceiverTypeID
                            from admin.LiveMessageInfo
                            left join studentinfo.Student_Details on
                            admin.LiveMessageInfo.SenderID=studentinfo.Student_Details.UserID_FK
                            where 
                            admin.LiveMessageInfo.SenderID=@Userid and
                            admin.LiveMessageInfo.ReceiverID=142 or
                            admin.LiveMessageInfo.SenderID=142 and
                            admin.LiveMessageInfo.ReceiverID=@Userid
                            order by admin.LiveMessageInfo.CreatedDate desc";
            using (SqlConnection connection =
                        new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand(Query);
                    command.Parameters.AddWithValue("@Userid", UserId);
                    command.Connection = connection;
                    if (connection.State == System.Data.ConnectionState.Broken ||
                           connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        message.MessageId = Convert.ToInt64(reader["ID"]);
                        message.SenderName = Convert.ToString(reader["Stu_Name"]);
                        message.SenderId = Convert.ToInt64(reader["SenderID"]);
                        message.ReceiverId = Convert.ToInt64(reader["ReceiverID"]);
                        message.MessageText = Convert.ToString(reader["Message"]);
                        message.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        message.IsRead = Convert.ToBoolean(reader["IsRead"]);
                        message.IsDelete = Convert.ToBoolean(reader["IsDelete"]);
                        message.ProfileImage = Convert.ToString(reader["ProfileImage"]);
                        message.SenderTypeID = Convert.ToInt32(reader["SenderTypeID"]);
                        message.ReceiverTypeID = Convert.ToInt32(reader["ReceiverTypeID"]);
                    }
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteErrorLog(String.Format("ErrorLog {0}", DateTime.Today), ex.Message);
                }
            }
            return message;
        }

        public long getAllSiteAdminMessagesCount()
        {
            long Count = 0;
            string Query = @"select count(*) TotalAdminMessgeCount from admin.LiveMessageInfo
                                where admin.LiveMessageInfo.ReceiverTypeID=0 and 
                                    admin.LiveMessageInfo.SenderTypeID=8";
            using (SqlConnection connection =
                        new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand(Query);
                    command.Connection = connection;
                    if (connection.State == System.Data.ConnectionState.Broken ||
                           connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Count = Convert.ToInt64(reader["TotalAdminMessgeCount"]);
                    }
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteErrorLog(String.Format("ErrorLog {0}", DateTime.Today), ex.Message);
                }
            }
            return Count;
        }

        #endregion

        public string InsertQuestionYearWise(AddTestQuestion Info)
        {
            string Res = string.Empty;
            try
            {

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string Query = string.Empty;

                    if (!(CheckYearWiseQuesExits(Info.Question, Info.Year, Info.SubjectID)))
                    {
                        if (!(CheckSerialNoYearWise(Info.SerialNo, Info.Year, Info.SubjectID)))
                        {
                            if (Info.DescriptionText != "" && Info.DescriptionText != null)
                            {

                                Info.DescID = CheckDesctextExits(Info.DescriptionText);
                                if (Info.DescID == 0)
                                {
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText,ExamID,SubjectID,Year)
                                    values(@DescText,@EID,@SID,@YR);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText,ExamID,SubjectID,Year)
                                    values(@DescText,@EID,@SID,@YR);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,'',@SerialNo)";
                                        }
                                    }
                                    else
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText,ExamID,SubjectID,Year)
                                    values(@DescText,@EID,@SID,@YR);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                    (DescriptionText,ExamID,SubjectID,Year)
                                    values(@DescText,@EID,@SID,@YR);SET @DescID = SCOPE_IDENTITY() 
                                    insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@DescID,@QuestionTypeID,@CorrectOption,'',@SerialNo)";
                                        }
                                    }

                                    SqlCommand cmd = new SqlCommand(Query, con);
                                    cmd.Parameters.AddWithValue("@DescText", Info.DescriptionText);
                                    cmd.Parameters.AddWithValue("@Question", Info.Question);
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Diagram", Info.Diagram);
                                    }
                                    cmd.Parameters.AddWithValue("@Year", Info.Year);
                                    cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@Option1", Info.Option1);
                                    cmd.Parameters.AddWithValue("@Option2", Info.Option2);
                                    cmd.Parameters.AddWithValue("@Option3", Info.Option3);
                                    cmd.Parameters.AddWithValue("@Option4", Info.Option4);
                                    cmd.Parameters.AddWithValue("@QuestionTypeID", Info.QuestionTypeID);
                                    cmd.Parameters.AddWithValue("@CorrectOption", Info.CorrectOption);

                                    cmd.Parameters.AddWithValue("@EID", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@SID", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@YR", Info.Year);
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Option5", Info.Option5);
                                    }
                                    cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                    con.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        Res = "Success! Successfully Inserted By Year";
                                    }
                                    else
                                    {
                                        Res = "Failed ! Failed to add question By year";
                                    }
                                    con.Close();
                                }
                                else
                                {
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                        }
                                    }
                                    else
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,@descId,@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                        }
                                    }

                                    SqlCommand cmd = new SqlCommand(Query, con);

                                    cmd.Parameters.AddWithValue("@Question", Info.Question);
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Diagram", Info.Diagram);
                                    }
                                    cmd.Parameters.AddWithValue("@Year", Info.Year);
                                    cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@Option1", Info.Option1);
                                    cmd.Parameters.AddWithValue("@Option2", Info.Option2);
                                    cmd.Parameters.AddWithValue("@Option3", Info.Option3);
                                    cmd.Parameters.AddWithValue("@Option4", Info.Option4);
                                    cmd.Parameters.AddWithValue("@descId", Info.DescID);
                                    cmd.Parameters.AddWithValue("@QuestionTypeID", Info.QuestionTypeID);
                                    cmd.Parameters.AddWithValue("@CorrectOption", Info.CorrectOption);
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Option5", Info.Option5);
                                    }
                                    cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                    con.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        Res = "Success! Successfully Inserted By Year";
                                    }
                                    else
                                    {
                                        Res = "Failed ! Failed to add question By Year";
                                    }
                                    con.Close();

                                }
                            }
                            else
                            {
                                if (Info.Diagram != "" && Info.Diagram != null)
                                {
                                    if ((Info.Option5 != "" && Info.Option5 != null))
                                    {
                                        Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                    }
                                    else
                                    {
                                        Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,@Diagram,@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                    }

                                }
                                else
                                {
                                    if ((Info.Option5 != "" && Info.Option5 != null))
                                    {
                                        Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,@Option5,@SerialNo)";
                                    }
                                    else
                                    {
                                        Query = @"insert into admin.QuestionsByYear
                                    (Question,Diagram,Year,ExamID,SubjectID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Option5,Question_SerialNo)
                                    values(@Question,'',@Year,@ExamID,@SubjectID,@Option1,@Option2,@Option3,@Option4,'',@QuestionTypeID,@CorrectOption,'',@SerialNo)";

                                    }
                                }

                                SqlCommand cmd = new SqlCommand(Query, con);

                                cmd.Parameters.AddWithValue("@Question", Info.Question);
                                if (Info.Diagram != "" && Info.Diagram != null)
                                {
                                    cmd.Parameters.AddWithValue("@Diagram", Info.Diagram);
                                }
                                cmd.Parameters.AddWithValue("@Year", Info.Year);
                                cmd.Parameters.AddWithValue("@ExamID", Info.ExamID);
                                cmd.Parameters.AddWithValue("@SubjectID", Info.SubjectID);
                                cmd.Parameters.AddWithValue("@Option1", Info.Option1);
                                cmd.Parameters.AddWithValue("@Option2", Info.Option2);
                                cmd.Parameters.AddWithValue("@Option3", Info.Option3);
                                cmd.Parameters.AddWithValue("@Option4", Info.Option4);
                                //cmd.Parameters.AddWithValue("@DescID", Info.DescID);
                                cmd.Parameters.AddWithValue("@QuestionTypeID", Info.QuestionTypeID);
                                cmd.Parameters.AddWithValue("@CorrectOption", Info.CorrectOption);
                                if (Info.Option5 != "" && Info.Option5 != null)
                                {
                                    cmd.Parameters.AddWithValue("@Option5", Info.Option5);
                                }
                                cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                con.Open();
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    Res = "Success! Successfully Inserted By Year";
                                }
                                else
                                {
                                    Res = "Failed ! Failed to add question By Year";
                                }
                                con.Close();


                            }
                        }
                        else
                        {
                            Res = "Failed! Question SerialNo already exists In Year Table";
                        }



                    }
                    else
                    {
                        Res = "Failed! you have already added this question By year";
                    }
                    con.Close();





                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Res;
        }

        public string InsertQuestionTopicWise(AddTestQuestion Info)
        {
            string Res = string.Empty;
            try
            {

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string Query = string.Empty;


                    if (!(CheckTopicWiseQuesExits(Info.Question, Info.TopicID)))
                    {

                        if (!(CheckSerialNoTopicWise(Info.SerialNo, Info.TopicID)))
                        {
                            if (Info.DescriptionText != "" && Info.DescriptionText != null)
                            {
                                Info.DescID = CheckDesctextExits(Info.DescriptionText);
                                if (Info.DescID == 0)
                                {

                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @" DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                (DescriptionText,ExamID,SubjectID,TopicID)
                                                values(@DescText,@EID,@SID,@TID);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @" DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                (DescriptionText,ExamID,SubjectID,TopicID)
                                                values(@DescText,@EID,@SID,@TID);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                        }
                                    }
                                    else
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {

                                            Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                    (DescriptionText,ExamID,SubjectID,TopicID)
                                                    values(@DescText,@EID,@SID,@TID);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"DECLARE @DescID BIGINT insert into admin.QuestionsDescription
                                                    (DescriptionText,ExamID,SubjectID,TopicID)
                                                    values(@DescText,@EID,@SID,@TID);SET @DescID = SCOPE_IDENTITY() 
                                                    insert into admin.QuestionsByTopic
                                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                        }
                                    }

                                    SqlCommand cmd = new SqlCommand(Query, con);

                                    cmd.Parameters.AddWithValue("@ques", Info.Question);
                                    cmd.Parameters.AddWithValue("@DescText", Info.DescriptionText);
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        cmd.Parameters.AddWithValue("@diag", Info.Diagram);
                                    }
                                    cmd.Parameters.AddWithValue("@topic", Info.Topic);
                                    cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@topicid", Info.TopicID);
                                    cmd.Parameters.AddWithValue("@opt1", Info.Option1);
                                    cmd.Parameters.AddWithValue("@opt2", Info.Option2);
                                    cmd.Parameters.AddWithValue("@opt3", Info.Option3);
                                    cmd.Parameters.AddWithValue("@opt4", Info.Option4);
                                    cmd.Parameters.AddWithValue("@questype", Info.QuestionTypeID);
                                    cmd.Parameters.AddWithValue("@correctopn", Info.CorrectOption);
                                    cmd.Parameters.AddWithValue("@marks", Info.Marks);

                                    cmd.Parameters.AddWithValue("@EID", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@SID", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@TID", Info.TopicID);
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        cmd.Parameters.AddWithValue("@optn5", Info.Option5);
                                    }
                                    cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                    con.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        Res = "Success! Successfully Inserted By Topic";
                                    }
                                    else
                                    {
                                        Res = "Failed ! Failed to add question By Topic";
                                    }
                                }

                                else
                                {
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                        }

                                    }
                                    else
                                    {
                                        if (Info.Option5 != "" && Info.Option5 != null)
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                        }
                                        else
                                        {
                                            Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@DescID,@questype,@correctopn,@marks,'',@SerialNo)";
                                        }


                                    }
                                    SqlCommand cmd = new SqlCommand(Query, con);

                                    cmd.Parameters.AddWithValue("@ques", Info.Question);
                                    if (Info.Diagram != "" && Info.Diagram != null)
                                    {
                                        cmd.Parameters.AddWithValue("@diag", Info.Diagram);
                                    }
                                    cmd.Parameters.AddWithValue("@topic", Info.Topic);
                                    cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                                    cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                                    cmd.Parameters.AddWithValue("@topicid", Info.TopicID);


                                    cmd.Parameters.AddWithValue("@opt1", Info.Option1);
                                    cmd.Parameters.AddWithValue("@opt2", Info.Option2);
                                    cmd.Parameters.AddWithValue("@opt3", Info.Option3);
                                    cmd.Parameters.AddWithValue("@opt4", Info.Option4);

                                    cmd.Parameters.AddWithValue("@DescID", Info.DescID);
                                    cmd.Parameters.AddWithValue("@questype", Info.QuestionTypeID);
                                    cmd.Parameters.AddWithValue("@correctopn", Info.CorrectOption);
                                    cmd.Parameters.AddWithValue("@marks", Info.Marks);
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        cmd.Parameters.AddWithValue("@optn5", Info.Option5);
                                    }
                                    cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);

                                    con.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        Res = "Success! Successfully Inserted By Topic";
                                    }
                                    else
                                    {
                                        Res = "Failed ! Failed to add question By Topic";
                                    }
                                    con.Close();




                                }
                            }

                            else
                            {
                                if (Info.Diagram != "" && Info.Diagram != null)
                                {
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,'',@questype,@correctopn,@marks,@optn5,@SerialNo)";
                                    }
                                    else
                                    {
                                        Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,@diag,@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,'',@questype,@correctopn,@marks,'',@SerialNo)";

                                    }
                                }
                                else
                                {
                                    if (Info.Option5 != "" && Info.Option5 != null)
                                    {
                                        Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,@questype,'',@correctopn,@marks,@optn5,@SerialNo)";
                                    }
                                    else
                                    {
                                        Query = @"insert into admin.QuestionsByTopic
                                    (Question,Diagram,Topic,ExamID,SubjectID,TopicID,Option1,Option2,Option3,Option4,DescriptionID,QuestionTypeID,CorrectOption,Marks,Option5,Question_SerialNo)
                                    values(@ques,'',@topic,@exmid,@subid,@topicid,@opt1,@opt2,@opt3,@opt4,'',@questype,@correctopn,@marks,'',@SerialNo)";


                                    }
                                }
                                SqlCommand cmd = new SqlCommand(Query, con);

                                cmd.Parameters.AddWithValue("@ques", Info.Question);
                                if (Info.Diagram != "" && Info.Diagram != null)
                                {
                                    cmd.Parameters.AddWithValue("@diag", Info.Diagram);
                                }
                                cmd.Parameters.AddWithValue("@topic", Info.Topic);
                                cmd.Parameters.AddWithValue("@exmid", Info.ExamID);
                                cmd.Parameters.AddWithValue("@subid", Info.SubjectID);
                                cmd.Parameters.AddWithValue("@topicid", Info.TopicID);


                                cmd.Parameters.AddWithValue("@opt1", Info.Option1);
                                cmd.Parameters.AddWithValue("@opt2", Info.Option2);
                                cmd.Parameters.AddWithValue("@opt3", Info.Option3);
                                cmd.Parameters.AddWithValue("@opt4", Info.Option4);


                                cmd.Parameters.AddWithValue("@questype", Info.QuestionTypeID);
                                cmd.Parameters.AddWithValue("@correctopn", Info.CorrectOption);
                                cmd.Parameters.AddWithValue("@marks", Info.Marks);
                                if (Info.Option5 != "" && Info.Option5 != null)
                                {
                                    cmd.Parameters.AddWithValue("@optn5", Info.Option5);

                                }

                                cmd.Parameters.AddWithValue("@SerialNo", Info.SerialNo);
                                con.Open();
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    Res = "Success! Successfully Inserted By Topic";
                                }
                                else
                                {
                                    Res = "Failed ! Failed to add question By Topic";
                                }
                                con.Close();

                            }
                        }
                        else
                        {
                            Res = "Failed! This Question SerialNo already exists In Topic Table";
                        }

                    }
                    else
                    {
                        Res = "Failed! you have already added this question by Topic";
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Res;
        }
        
        public bool CheckSerialNoYearWise(long SerialNo, string Year, long SubjectID)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select Question_SerialNo from admin.QuestionsByYear where SubjectID=@SID and Year=@Year and Question_SerialNo=@SerialNo";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@SID", SubjectID);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@SerialNo", @SerialNo);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public bool CheckSerialNoTopicWise(long SerialNo, long TopicID)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select Question_SerialNo from admin.QuestionsByTopic where TopicID=@TID and  Question_SerialNo=@SerialNo";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@TID", TopicID);

                    cmd.Parameters.AddWithValue("@SerialNo", @SerialNo);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }
        
        public bool CheckTopicWiseQuesExits(string Question, long TopicID)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string query = @"select ID,Question from 
                                admin.QuestionsByTopic
                                where  Question=@ques and TopicID=@TID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ques", Question);
                    cmd.Parameters.AddWithValue("@TID", TopicID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public bool CheckYearWiseQuesExits(string Question, string Year, long SubjectID)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select Question from admin.QuestionsByYear where Question=@ques and Year=@Year and SubjectID=@SID";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@ques", Question);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@SID", SubjectID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        public AddTestQuestion getQuestionViewYearWise(string ID, string QuestionType)
        {
            string Query = string.Empty;
            AddTestQuestion questions = new AddTestQuestion();

            Query = @"select admin.QuestionsByYear.ID,admin.QuestionsByYear.Question,
                    ISNULL(admin.QuestionsByYear.Question_SerialNo,'') as Question_SerialNo,
                    ISNULL(admin.QuestionsByYear.QuestionTypeID,'') as QuestionTypeID,
                    ISNULL(admin.QuestionsByYear.Diagram,'n.a') as Diagram,ISNULL(admin.QuestionsByYear.Option1,'') as Option1,
                    ISNULL(admin.QuestionsByYear.Option2,'') as Option2,ISNULL(admin.QuestionsByYear.Option3,'') as Option3,
                    ISNULL(admin.QuestionsByYear.Option4,'') as Option4,
                    ISNULL((case when admin.QuestionsByYear.Option5='' then 'http://www.go2uni.co/images/camera.png' else admin.QuestionsByYear.Option5 end),'#') as Option5,
                    ISNULL(admin.QuestionsByYear.CorrectOption,'') as CorrectOption,
                    ISNULL(admin.QuestionsByYear.Year,'') as Year,
                    ISNULL(admin.QuestionsByYear.SubjectID,'') as SubjectID
                    from admin.QuestionsByYear where ID=@ID";
            questions.QuestionType = "year";


            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt64(ID));
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        questions.DescID = Convert.ToInt64(reader["ID"].ToString());
                        questions.Question = reader["Question"].ToString();
                        questions.SerialNo = Convert.ToInt64(reader["Question_SerialNo"].ToString());
                        questions.QuestionTypeID = Convert.ToInt64(reader["QuestionTypeID"].ToString());
                        questions.Diagram = reader["Diagram"].ToString();
                        questions.Option1 = reader["Option1"].ToString();
                        questions.Option2 = reader["Option2"].ToString();
                        questions.Option3 = reader["Option3"].ToString();
                        questions.Option4 = reader["Option4"].ToString();
                        questions.Option5 = reader["Option5"].ToString();
                        questions.CorrectOption = reader["CorrectOption"].ToString();
                        questions.Year = reader["Year"].ToString();
                        questions.SubjectID = Convert.ToInt64(reader["SubjectID"].ToString());
                    }
                }
            }
            return questions;
        }

        public AddTestQuestion getQuestionViewTopicWise(string ID, string QuestionType)
        {
            string Query = string.Empty;
            AddTestQuestion questions = new AddTestQuestion();



            Query = @"select admin.QuestionsByTopic.ID,admin.QuestionsByTopic.Question,
                    ISNULL(admin.QuestionsByTopic.Question_SerialNo,'') as Question_SerialNo,
                    ISNULL(admin.QuestionsByTopic.QuestionTypeID,'') as QuestionTypeID,
                    ISNULL(admin.QuestionsByTopic.Diagram,'n.a') as Diagram,ISNULL(admin.QuestionsByTopic.Option1,'') as Option1,
                    ISNULL(admin.QuestionsByTopic.Option2,'') as Option2,ISNULL(admin.QuestionsByTopic.Option3,'') as Option3,
                    ISNULL(admin.QuestionsByTopic.Option4,'') as Option4,
                    ISNULL((case when admin.QuestionsByTopic.Option5='' then 'http://www.go2uni.co/images/camera.png' else admin.QuestionsByTopic.Option5 end),'#') as Option5,
                    ISNULL(admin.QuestionsByTopic.CorrectOption,'') as CorrectOption,
                    ISNULL(admin.QuestionsByTopic.TopicID,'') as TopicID,
					ISNULL(admin.QuestionsByTopic.SubjectID,'') as SubjectID
                    from admin.QuestionsByTopic where ID=@ID";
            questions.QuestionType = "topic";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt64(ID));
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        questions.DescID = Convert.ToInt64(reader["ID"].ToString());
                        questions.Question = reader["Question"].ToString();
                        questions.SerialNo = Convert.ToInt64(reader["Question_SerialNo"].ToString());
                        questions.QuestionTypeID = Convert.ToInt64(reader["QuestionTypeID"].ToString());
                        questions.Diagram = reader["Diagram"].ToString();
                        questions.Option1 = reader["Option1"].ToString();
                        questions.Option2 = reader["Option2"].ToString();
                        questions.Option3 = reader["Option3"].ToString();
                        questions.Option4 = reader["Option4"].ToString();
                        questions.Option5 = reader["Option5"].ToString();
                        questions.CorrectOption = reader["CorrectOption"].ToString();
                        questions.TopicID = Convert.ToInt64(reader["TopicID"].ToString());
                        questions.SubjectID = Convert.ToInt64(reader["SubjectID"].ToString());
                    }
                }
            }
            return questions;
        }

        public string UpdateQuestionsYearWise(AddTestQuestion questions)
        {
            string res = "";
            Global global = new Global();
            string ques = global.ObjectNullChecking(questions);
            AddTestQuestion question = JsonConvert.DeserializeObject<AddTestQuestion>(ques);
            string Query = string.Empty;

            if (questions.RawQstn == questions.TempQuestion && questions.SerialNo == questions.TempSerialNo)
            {



                Query = @"update admin.QuestionsByYear set
                    admin.QuestionsByYear.Question=@Question,admin.QuestionsByYear.Diagram=@Diagram,
                    admin.QuestionsByYear.Option1=@Option1,admin.QuestionsByYear.Option2=@Option2,
                    admin.QuestionsByYear.Option3=@Option3,admin.QuestionsByYear.Option4=@Option4,
                    admin.QuestionsByYear.Option5=@Option5,admin.QuestionsByYear.CorrectOption=@CorrectOption,
                      admin.QuestionsByYear.Question_SerialNo=@SRLNO
                    where admin.QuestionsByYear.ID=@ID";


                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    if (con.State == System.Data.ConnectionState.Broken ||
                            con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand(Query, con);
                        command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                        command.Parameters.AddWithValue("@Question", question.Question);
                        command.Parameters.AddWithValue("@Diagram", question.Diagram);
                        command.Parameters.AddWithValue("@Option1", question.Option1);
                        command.Parameters.AddWithValue("@Option2", question.Option2);
                        command.Parameters.AddWithValue("@Option3", question.Option3);
                        command.Parameters.AddWithValue("@Option4", question.Option4);
                        command.Parameters.AddWithValue("@Option5", question.Option5);
                        command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                        command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                        if (command.ExecuteNonQuery() > 0)
                        {
                            res = "Success!Update Successfull";
                        }
                        else
                        {
                            res = "Failed!Update Failed";
                        }
                    }
                    else
                    {
                        res = "Failed!Update Failed";
                    }
                }
            }

            if (questions.RawQstn != questions.TempQuestion && questions.SerialNo != questions.TempSerialNo)
            {
                if (!(CheckYearWiseQuesExits(questions.Question, questions.Year, questions.SubjectID)))
                {
                    if (!(CheckSerialNoYearWise(questions.SerialNo, questions.Year, questions.SubjectID)))
                    {

                        Query = @"update admin.QuestionsByYear set
                    admin.QuestionsByYear.Question=@Question,admin.QuestionsByYear.Diagram=@Diagram,
                    admin.QuestionsByYear.Option1=@Option1,admin.QuestionsByYear.Option2=@Option2,
                    admin.QuestionsByYear.Option3=@Option3,admin.QuestionsByYear.Option4=@Option4,
                    admin.QuestionsByYear.Option5=@Option5,admin.QuestionsByYear.CorrectOption=@CorrectOption,
                    admin.QuestionsByYear.Question_SerialNo=@SRLNO
                    where admin.QuestionsByYear.ID=@ID";


                        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                        {
                            if (con.State == System.Data.ConnectionState.Broken ||
                                    con.State == System.Data.ConnectionState.Closed)
                            {
                                con.Open();
                                SqlCommand command = new SqlCommand(Query, con);
                                command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                                command.Parameters.AddWithValue("@Question", question.Question);
                                command.Parameters.AddWithValue("@Diagram", question.Diagram);
                                command.Parameters.AddWithValue("@Option1", question.Option1);
                                command.Parameters.AddWithValue("@Option2", question.Option2);
                                command.Parameters.AddWithValue("@Option3", question.Option3);
                                command.Parameters.AddWithValue("@Option4", question.Option4);
                                command.Parameters.AddWithValue("@Option5", question.Option5);
                                command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                                command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                                if (command.ExecuteNonQuery() > 0)
                                {
                                    res = "Success!Update Successfull";
                                }
                                else
                                {
                                    res = "Failed!Update Failed";
                                }
                            }
                            else
                            {
                                res = "Failed!Update Failed";
                            }
                        }

                    }
                    else
                    {
                        res = "Failed!Serial NO already exists";
                    }


                }
                else
                {
                    res = "Failed!Question already Exists";

                }



            }

            if (questions.RawQstn == questions.TempQuestion && questions.SerialNo != questions.TempSerialNo)
            {

                if (!(CheckSerialNoYearWise(questions.SerialNo, questions.Year, questions.SubjectID)))
                {


                    Query = @"update admin.QuestionsByYear set
                    admin.QuestionsByYear.Question=@Question,admin.QuestionsByYear.Diagram=@Diagram,
                    admin.QuestionsByYear.Option1=@Option1,admin.QuestionsByYear.Option2=@Option2,
                    admin.QuestionsByYear.Option3=@Option3,admin.QuestionsByYear.Option4=@Option4,
                    admin.QuestionsByYear.Option5=@Option5,admin.QuestionsByYear.CorrectOption=@CorrectOption,
                    admin.QuestionsByYear.Question_SerialNo=@SRLNO
                    where admin.QuestionsByYear.ID=@ID";


                    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                    {
                        if (con.State == System.Data.ConnectionState.Broken ||
                                con.State == System.Data.ConnectionState.Closed)
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand(Query, con);
                            command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                            command.Parameters.AddWithValue("@Question", question.Question);
                            command.Parameters.AddWithValue("@Diagram", question.Diagram);
                            command.Parameters.AddWithValue("@Option1", question.Option1);
                            command.Parameters.AddWithValue("@Option2", question.Option2);
                            command.Parameters.AddWithValue("@Option3", question.Option3);
                            command.Parameters.AddWithValue("@Option4", question.Option4);
                            command.Parameters.AddWithValue("@Option5", question.Option5);
                            command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                            command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                            if (command.ExecuteNonQuery() > 0)
                            {
                                res = "Success!Update Successfull";
                            }
                            else
                            {
                                res = "Failed!Update Failed";
                            }
                        }
                        else
                        {
                            res = "Failed!Update Failed";
                        }
                    }

                }
                else
                {
                    res = "Failed!Serial No already exists";
                }



            }

            if (questions.RawQstn != questions.TempQuestion && questions.SerialNo == questions.TempSerialNo)
            {

                if (!(CheckYearWiseQuesExits(questions.Question, questions.Year, questions.SubjectID)))
                {

                    Query = @"update admin.QuestionsByYear set
                    admin.QuestionsByYear.Question=@Question,admin.QuestionsByYear.Diagram=@Diagram,
                    admin.QuestionsByYear.Option1=@Option1,admin.QuestionsByYear.Option2=@Option2,
                    admin.QuestionsByYear.Option3=@Option3,admin.QuestionsByYear.Option4=@Option4,
                    admin.QuestionsByYear.Option5=@Option5,admin.QuestionsByYear.CorrectOption=@CorrectOption,
                     admin.QuestionsByYear.Question_SerialNo=@SRLNO
                    where admin.QuestionsByYear.ID=@ID";


                    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                    {
                        if (con.State == System.Data.ConnectionState.Broken ||
                                con.State == System.Data.ConnectionState.Closed)
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand(Query, con);
                            command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                            command.Parameters.AddWithValue("@Question", question.Question);
                            command.Parameters.AddWithValue("@Diagram", question.Diagram);
                            command.Parameters.AddWithValue("@Option1", question.Option1);
                            command.Parameters.AddWithValue("@Option2", question.Option2);
                            command.Parameters.AddWithValue("@Option3", question.Option3);
                            command.Parameters.AddWithValue("@Option4", question.Option4);
                            command.Parameters.AddWithValue("@Option5", question.Option5);
                            command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                            command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                            if (command.ExecuteNonQuery() > 0)
                            {
                                res = "Success!Update Successfull";
                            }
                            else
                            {
                                res = "Failed!Update Failed";
                            }
                        }
                        else
                        {
                            res = "Failed!Update Failed";
                        }
                    }

                }
                else
                {
                    res = "Failed!Question already exists";
                }


            }

            return res;
        }

        public string UpdateQuestionsTopicWise(AddTestQuestion questions)
        {
            string res = "";
            Global global = new Global();
            string ques = global.ObjectNullChecking(questions);
            AddTestQuestion question = JsonConvert.DeserializeObject<AddTestQuestion>(ques);
            string Query = string.Empty;

            if (questions.RawQstn == questions.TempQuestion && questions.SerialNo == questions.TempSerialNo)
            {
                Query = @"update admin.QuestionsByTopic set
                    admin.QuestionsByTopic.Question=@Question,admin.QuestionsByTopic.Diagram=@Diagram,
                    admin.QuestionsByTopic.Option1=@Option1,admin.QuestionsByTopic.Option2=@Option2,
                    admin.QuestionsByTopic.Option3=@Option3,admin.QuestionsByTopic.Option4=@Option4,
                    admin.QuestionsByTopic.Option5=@Option5,admin.QuestionsByTopic.CorrectOption=@CorrectOption,
                    admin.QuestionsByTopic.Question_SerialNo=@SRLNO
                    where admin.QuestionsByTopic.ID=@ID";
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    if (con.State == System.Data.ConnectionState.Broken ||
                            con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand(Query, con);
                        command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                        command.Parameters.AddWithValue("@Question", question.Question);
                        command.Parameters.AddWithValue("@Diagram", question.Diagram);
                        command.Parameters.AddWithValue("@Option1", question.Option1);
                        command.Parameters.AddWithValue("@Option2", question.Option2);
                        command.Parameters.AddWithValue("@Option3", question.Option3);
                        command.Parameters.AddWithValue("@Option4", question.Option4);
                        command.Parameters.AddWithValue("@Option5", question.Option5);
                        command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                        command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                        if (command.ExecuteNonQuery() > 0)
                        {
                            res = "Success!Update Successfull";
                        }
                        else
                        {
                            res = "Failed!Update Failed";
                        }
                    }
                    else
                    {
                        res = "Failed!Update Failed";
                    }
                }
            }

            if (questions.RawQstn != questions.TempQuestion && questions.SerialNo != questions.TempSerialNo)
            {
                if (!(CheckTopicWiseQuesExits(questions.Question, questions.TopicID)))
                {

                    if (!(CheckSerialNoTopicWise(questions.SerialNo, questions.TopicID)))
                    {


                        Query = @"update admin.QuestionsByTopic set
                    admin.QuestionsByTopic.Question=@Question,admin.QuestionsByTopic.Diagram=@Diagram,
                    admin.QuestionsByTopic.Option1=@Option1,admin.QuestionsByTopic.Option2=@Option2,
                    admin.QuestionsByTopic.Option3=@Option3,admin.QuestionsByTopic.Option4=@Option4,
                    admin.QuestionsByTopic.Option5=@Option5,admin.QuestionsByTopic.CorrectOption=@CorrectOption,
                    admin.QuestionsByTopic.Question_SerialNo=@SRLNO
                    where admin.QuestionsByTopic.ID=@ID";

                        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                        {
                            if (con.State == System.Data.ConnectionState.Broken ||
                                    con.State == System.Data.ConnectionState.Closed)
                            {
                                con.Open();
                                SqlCommand command = new SqlCommand(Query, con);
                                command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                                command.Parameters.AddWithValue("@Question", question.Question);
                                command.Parameters.AddWithValue("@Diagram", question.Diagram);
                                command.Parameters.AddWithValue("@Option1", question.Option1);
                                command.Parameters.AddWithValue("@Option2", question.Option2);
                                command.Parameters.AddWithValue("@Option3", question.Option3);
                                command.Parameters.AddWithValue("@Option4", question.Option4);
                                command.Parameters.AddWithValue("@Option5", question.Option5);
                                command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                                command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                                if (command.ExecuteNonQuery() > 0)
                                {
                                    res = "Failed!Update Successfull";
                                }
                                else
                                {
                                    res = "Failed!Update Failed";
                                }
                            }
                            else
                            {
                                res = "Failed!Update Failed";
                            }
                        }

                    }
                    else
                    {
                        res = "Failed!Serial No already Exist";


                    }



                }
                else
                {
                    res = "Failed!Question already Exist";

                }



            }

            if (questions.RawQstn == questions.TempQuestion && questions.SerialNo != questions.TempSerialNo)
            {
                if (!(CheckSerialNoTopicWise(questions.SerialNo, questions.TopicID)))
                {

                    Query = @"update admin.QuestionsByTopic set
                    admin.QuestionsByTopic.Question=@Question,admin.QuestionsByTopic.Diagram=@Diagram,
                    admin.QuestionsByTopic.Option1=@Option1,admin.QuestionsByTopic.Option2=@Option2,
                    admin.QuestionsByTopic.Option3=@Option3,admin.QuestionsByTopic.Option4=@Option4,
                    admin.QuestionsByTopic.Option5=@Option5,admin.QuestionsByTopic.CorrectOption=@CorrectOption,
                    admin.QuestionsByTopic.Question_SerialNo=@SRLNO
                    where admin.QuestionsByTopic.ID=@ID";


                    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                    {
                        if (con.State == System.Data.ConnectionState.Broken ||
                                con.State == System.Data.ConnectionState.Closed)
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand(Query, con);
                            command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                            command.Parameters.AddWithValue("@Question", question.Question);
                            command.Parameters.AddWithValue("@Diagram", question.Diagram);
                            command.Parameters.AddWithValue("@Option1", question.Option1);
                            command.Parameters.AddWithValue("@Option2", question.Option2);
                            command.Parameters.AddWithValue("@Option3", question.Option3);
                            command.Parameters.AddWithValue("@Option4", question.Option4);
                            command.Parameters.AddWithValue("@Option5", question.Option5);
                            command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                            command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                            if (command.ExecuteNonQuery() > 0)
                            {
                                res = "Success!Update Successfull";
                            }
                            else
                            {
                                res = "Failed!Update Failed";
                            }
                        }
                        else
                        {
                            res = "Failed!Update Failed";
                        }
                    }

                }
                else
                {
                    res = "Failed!Serial No already Exists";
                }


            }

            if (questions.RawQstn != questions.TempQuestion && questions.SerialNo == questions.TempSerialNo)
            {
                if (!(CheckTopicWiseQuesExits(questions.Question, questions.TopicID)))
                {

                    Query = @"update admin.QuestionsByTopic set
                    admin.QuestionsByTopic.Question=@Question,admin.QuestionsByTopic.Diagram=@Diagram,
                    admin.QuestionsByTopic.Option1=@Option1,admin.QuestionsByTopic.Option2=@Option2,
                    admin.QuestionsByTopic.Option3=@Option3,admin.QuestionsByTopic.Option4=@Option4,
                    admin.QuestionsByTopic.Option5=@Option5,admin.QuestionsByTopic.CorrectOption=@CorrectOption,
                     admin.QuestionsByTopic.Question_SerialNo=@SRLNO
                    where admin.QuestionsByTopic.ID=@ID";


                    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                    {
                        if (con.State == System.Data.ConnectionState.Broken ||
                                con.State == System.Data.ConnectionState.Closed)
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand(Query, con);
                            command.Parameters.AddWithValue("@ID", Convert.ToInt64(question.DescID));
                            command.Parameters.AddWithValue("@Question", question.Question);
                            command.Parameters.AddWithValue("@Diagram", question.Diagram);
                            command.Parameters.AddWithValue("@Option1", question.Option1);
                            command.Parameters.AddWithValue("@Option2", question.Option2);
                            command.Parameters.AddWithValue("@Option3", question.Option3);
                            command.Parameters.AddWithValue("@Option4", question.Option4);
                            command.Parameters.AddWithValue("@Option5", question.Option5);
                            command.Parameters.AddWithValue("@CorrectOption", question.CorrectOption);
                            command.Parameters.AddWithValue("@SRLNO", question.SerialNo);

                            if (command.ExecuteNonQuery() > 0)
                            {
                                res = "Success!Update Successfull";
                            }
                            else
                            {
                                res = "Failed!Update Failed";
                            }
                        }
                        else
                        {
                            res = "Failed!Update Failed";
                        }
                    }

                }
                else
                {
                    res = "Failed!Question already exists";
                }


            }





            return res;
        }


        #region AddUser For SiteAdmin

        public List<AddUser_SiteAdmin> UserDetailsForSiteAdmin()
        {
            List<AddUser_SiteAdmin> Result = new List<AddUser_SiteAdmin>();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"select UID,Email,'OnlyForAddQuestion' as UserType from admin.users where  UserType_FK=9";

                SqlCommand com = new SqlCommand(Query, con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AddUser_SiteAdmin temp = new AddUser_SiteAdmin();

                    temp.UID = Convert.ToInt64(reader["UID"]);
                    temp.Email = Convert.ToString(reader["Email"]);
                    temp.UserType = Convert.ToString(reader["UserType"]);



                    Result.Add(temp);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public List<Login_Type> GetLoginTypeForSiteAdmin()
        {
            List<Login_Type> Result = new List<Login_Type>();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"select ID, LoginType from admin.Login_Type where ID=9 ";

                SqlCommand com = new SqlCommand(Query, con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Login_Type temp = new Login_Type();

                    temp.ID = Convert.ToInt64(reader["ID"]);
                    temp.LoginType = Convert.ToString(reader["LoginType"]);


                    Result.Add(temp);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string AddUserBySiteAdmin(Admin_User Data)
        {
            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());
                if (!(CheckMailExists(Data.Email)))
                {
                    string Query = @"insert into admin.Users(Email,Password,UserType_FK)values(@Email,@PassWord,@UserTypeID)";


                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Email", Data.Email);
                    com.Parameters.AddWithValue("@PassWord", Data.PassWord);
                    com.Parameters.AddWithValue("@UserTypeID", Data.UserType_FK);

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!UserDetails Inserted";
                    }
                    else
                    {
                        Result = "Failed!Failed UserDetails Insertion";
                    }
                }
                else
                {
                    Result = "Failed!Email Already exists";
                }
                con.Close();


            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public AddUser_SiteAdmin EditUserBySiteAdmin(long UID)
        {
            AddUser_SiteAdmin Result = new AddUser_SiteAdmin();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string query = @"select UID,Email,Password,UserType_FK from admin.users where UID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", UID);


                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Result.UID = Convert.ToInt64(reader["UID"]);
                    Result.Email = Convert.ToString(reader["Email"]);
                    Result.Password = Convert.ToString(reader["Password"]);
                    Result.UserType_FK = Convert.ToInt64(reader["UserType_FK"]);


                    break;
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string UpdateUserBySiteAdmin(AddUser_SiteAdmin Data)
        {
            string Result = string.Empty;
            try
            {
                if (Data.Email == Data.TempEmail)
                {
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                    string Query = @"update admin.users set Email=@Email,Password=@Pass,UserType_FK=@USER where UID=@UID";


                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Email", Data.Email);
                    com.Parameters.AddWithValue("@Pass", Data.Password);
                    com.Parameters.AddWithValue("@USER", Data.UserType_FK);
                    com.Parameters.AddWithValue("@UID", Data.UID);

                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!UserDetails Updated";
                    }
                    else
                    {
                        Result = "Failed!Failed UserDetails Updated";
                    }

                    con.Close();


                }
                else
                {
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());
                    if (!(CheckMailExists(Data.Email)))
                    {
                        string Query = @"update admin.users set Email=@Email,Password=@Pass,UserType_FK=@USER where UID=@UID";


                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@Email", Data.Email);
                        com.Parameters.AddWithValue("@Pass", Data.Password);
                        com.Parameters.AddWithValue("@USER", Data.UserType_FK);
                        com.Parameters.AddWithValue("@UID", Data.UID);

                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!UserDetails Updated";
                        }
                        else
                        {
                            Result = "Failed!Failed UserDetails Update";
                        }
                    }
                    else
                    {
                        Result = "Failed!Email Already exists";
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

        public string DeleteUserBySiteAdmin(long ID)
        {
            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"Delete from admin.users Where UID=@Id;";
                SqlCommand com = new SqlCommand(Query, con);
                com.Parameters.AddWithValue("@Id", ID);
                con.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Result = "Success!Deleted";
                }
                else
                {
                    Result = "Failed!Deleted";
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckMailExists(string Email)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {

                    string query = @"select Email from admin.users Where Email=@email";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@email", Email);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        res = true;
                    }
                }

            }
            catch (Exception ex)
            {

                res = false;
            }
            return res;
        }

        #endregion



        public List<CollegeDay> collegeDetailsForSiteAdmin()
        {
            List<CollegeDay> Result = new List<CollegeDay>();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"select NxtCollege_ID,NxtCollege_Name,NxtCollege_AddedDate from collegeEvents.Nextcollege order by NxtCollege_AddedDate desc";

                SqlCommand com = new SqlCommand(Query, con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CollegeDay temp = new CollegeDay();

                    temp.ID = Convert.ToInt64(reader["NxtCollege_ID"]);
                    temp.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                    temp.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);



                    Result.Add(temp);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string InsertCollegeDetails(CollegeDay Data)

        {
            Global global = new Global();
            string __obj = global.ObjectNullChecking(Data);
            CollegeDay obj = JsonConvert.DeserializeObject<CollegeDay>(__obj);

            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"insert into collegeEvents.NextCollege(NxtCollege_Video,NxtCollege_AddedDate,NxtCollege_Name)values(@video,@Date,@Name)";


                SqlCommand com = new SqlCommand(Query, con);

                com.Parameters.AddWithValue("@video", obj.video);

                com.Parameters.AddWithValue("@Date", obj.Event_Date);
                com.Parameters.AddWithValue("@Name", obj.CollegeName);

                con.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Result = "Success!CollegeDetails Inserted";
                }
                else
                {
                    Result = "Failed!Failed CollegeDetails Insertion";
                }

                con.Close();


            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }


        public CollegeDay EditCollegeDetails(long UID)
        {
            CollegeDay Result = new CollegeDay();
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string query = @"Select NxtCollege_ID,NxtCollege_Video,NxtCollege_AddedDate,NxtCollege_Name from collegeEvents.NextCollege where NxtCollege_ID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", UID);


                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Result.ID = Convert.ToInt64(reader["NxtCollege_ID"]);
                    Result.video = Convert.ToString(reader["NxtCollege_Video"]);
                    Result.CollegeName = Convert.ToString(reader["NxtCollege_Name"]);
                    Result.Event_Date = Convert.ToString(reader["NxtCollege_AddedDate"]);


                    break;
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string UpdateCollegeDetails(CollegeDay Data)
        {
            Global global = new Global();
            string __obj = global.ObjectNullChecking(Data);
            CollegeDay obj = JsonConvert.DeserializeObject<CollegeDay>(__obj);
            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"update collegeEvents.NextCollege set NxtCollege_Video=@video, NxtCollege_AddedDate=@Date,NxtCollege_Name=@Name
 where NxtCollege_ID=@ID";


                SqlCommand com = new SqlCommand(Query, con);
                com.Parameters.AddWithValue("@ID", obj.ID);

                com.Parameters.AddWithValue("@video", obj.video);
                com.Parameters.AddWithValue("@Date", obj.Event_Date);
                com.Parameters.AddWithValue("@Name", obj.CollegeName);

                con.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Result = "Success!CollegeDetails Updated";
                }
                else
                {
                    Result = "Failed!Failed CollegeDetails Update";
                }

                con.Close();


            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string DeleteCollegeBySiteAdmin(long ID)
        {
            string Result = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                string Query = @"Delete from collegeEvents.NextCollege Where NxtCollege_ID=@ID;";
                SqlCommand com = new SqlCommand(Query, con);
                com.Parameters.AddWithValue("@ID", ID);
                con.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Result = "Success!Deleted";
                }
                else
                {
                    Result = "Failed!Deleted";
                }
                con.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

    }
}