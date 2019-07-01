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
    public class SuperAdmin
    {
        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();


        #region Students and Redirection to Students profile 



        public List<NameAndRegs> GetAllStuList()
        {
            List<NameAndRegs> obj = new List<NameAndRegs>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                string Query = @"select Stu_ID,Stu_Name,Stu_RegistrationNo from studentinfo.Student_Details";
                SqlCommand cmd = new SqlCommand(Query, connt);
                // SqlCommand cmd = new SqlCommand("[superadmin].[Sp_GetStudentsList]", connt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<NameAndRegs>>(str);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }


        public List<ClassSpecificaton> GetJuniorSeniorList()
        {
            List<ClassSpecificaton> obj = new List<ClassSpecificaton>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                SqlCommand cmd = new SqlCommand("SuperAdmin.SP_JuniorSenior", connt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<ClassSpecificaton>>(str);
                }
            }
            catch (Exception ex)
            {


            }
            return obj;
        }



        public StudentProfile GetProfDetBYStuID(long StudentID)
        {
            StudentProfile obj = new StudentProfile();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select distinct sdet.Stu_ID,sdet.Stu_Name,sdet.Stu_Email,Stu_RegistrationNo,sdet.Stu_Profile_Image,sdet.School_Name,cls.ID as ClassID,cls.Class AS ClassName,div.ID as DivisionId,div.Name as Division_Name,gen.GenderName as Gender,IsNull(sdet.Stream_ID_FK,'') AS StreamID,IsNull(strm.Stream,'') As Stream
                                    from studentinfo.Student_Details sdet
                                    Left JOIN admin.MasterClass as cls ON sdet.Class_ID_FK=cls.ID
                                    Left Join admin.ClassStream as strm ON sdet.Stream_ID_FK=strm.ID
                                    left JOIN admin.ClassSpecificaton as div ON sdet.DivisionID=div.ID
                                    left JOIN admin.GenderInfo as gen On sdet.Stu_Gender_FK=gen.GenderID
                                    where Stu_ID=@StudentID";
                    //SqlCommand cmd = new SqlCommand("[studentinfo].[SP_GetStudentProfileInfo]", con);
                    SqlCommand cmd = new SqlCommand(Query, con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", StudentID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        obj.ID = Convert.ToInt32(reader["Stu_ID"]);
                        obj.Student_Email = Convert.ToString(reader["Stu_Email"]);
                        obj.Student_Name = Convert.ToString(reader["Stu_Name"]);
                        obj.student_ProfileImage = Convert.ToString(reader["Stu_Profile_Image"]);
                        obj.RegistrationNo = Convert.ToString(reader["Stu_RegistrationNo"]);
                        obj.GenderName = Convert.ToString(reader["Gender"]);
                        obj.SchoolName = Convert.ToString(reader["School_Name"]);
                        obj.Class = Convert.ToString(reader["ClassName"]);
                        obj.Stream = Convert.ToString(reader["Stream"]);




                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);

            }
            return obj;
        }








        public List<NameAndRegs> GetStudentsByDivId(int ID)
        {
            List<NameAndRegs> Res = new List<NameAndRegs>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select sdet.Stu_Name,sdet.Stu_ID,Stu_RegistrationNo,cls.ID as ClassID,cls.Class AS ClassName,div.ID as DivisionId,div.Name as Division_Name from studentinfo.Student_Details sdet
                                    INNER JOIN admin.MasterClass as cls ON sdet.Class_ID_FK=cls.ID
                                    INNER JOIN admin.ClassSpecificaton as div ON sdet.DivisionID=div.ID
                                    where div.ID=@ClassSpId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ClassSpId", ID);
                    // cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        NameAndRegs temp = new NameAndRegs();
                        temp.DivID = Convert.ToInt16(reader["DivisionId"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.Class = Convert.ToString(reader["ClassName"]);
                        temp.DivName = Convert.ToString(reader["Division_Name"]);
                        // temp.student_Fk_ID = Convert.ToUInt32(reader["student_Fk_ID"]);
                        temp.Stu_RegistrationNo = Convert.ToString(reader["Stu_RegistrationNo"]);
                        // Student_Name,student_Fk_ID,RegistrationNo,Class,Name,DivID


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


        public List<NameAndRegs> GetStudentsByClassId(int ClassID)
        {
            List<NameAndRegs> Res = new List<NameAndRegs>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select sdet.Stu_Name,sdet.Stu_ID,Stu_RegistrationNo,cls.ID as ClassID,cls.Class AS ClassName,div.ID as DivisionId,div.Name as Division_Name from studentinfo.Student_Details sdet
                                    INNER JOIN admin.MasterClass as cls ON sdet.Class_ID_FK=cls.ID
                                    INNER JOIN admin.ClassSpecificaton as div ON sdet.DivisionID=div.ID
                                    where cls.ID=@ClassID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    // cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        NameAndRegs temp = new NameAndRegs();
                        temp.DivID = Convert.ToInt16(reader["DivisionId"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.Class = Convert.ToString(reader["ClassName"]);
                        temp.DivName = Convert.ToString(reader["Division_Name"]);
                        // temp.student_Fk_ID = Convert.ToUInt32(reader["student_Fk_ID"]);
                        temp.Stu_RegistrationNo = Convert.ToString(reader["Stu_RegistrationNo"]);
                        // Student_Name,student_Fk_ID,RegistrationNo,Class,Name,DivID


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






        public List<NameAndRegs> GetAllStudents()
        {
            List<NameAndRegs> Res = new List<NameAndRegs>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Select sdet.Stu_Name,sdet.Stu_ID,sdet.Stu_RegistrationNo,sdet.Class_ID_FK AS ClassID ,cls.Class,sdet.DivisionID,clsSpe.Name AS Division
                                    from studentinfo.Student_Details AS sdet 
                                    INNER JOIN admin.MasterClass AS cls ON sdet.Class_ID_FK=cls.ID
                                    INNER JOIN admin.ClassSpecificaton As clsSpe ON  sdet.DivisionID=clsSpe.ID";
                    //SqlCommand cmd = new SqlCommand("SP_SA_GetStuden", con);
                    SqlCommand cmd = new SqlCommand(Query, con);
                    // cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        NameAndRegs temp = new NameAndRegs();
                        temp.DivID = Convert.ToInt16(reader["DivisionID"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.Class = Convert.ToString(reader["Class"]);
                        temp.DivName = Convert.ToString(reader["Division"]);
                        // temp.student_Fk_ID = Convert.ToUInt32(reader["student_Fk_ID"]);
                        temp.Stu_RegistrationNo = Convert.ToString(reader["Stu_RegistrationNo"]);
                        // Student_Name,student_Fk_ID,RegistrationNo,Class,Name,DivID


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
        #endregion

        #region SUPER ADMIN EDIT/UPDATE  PROFILE

        public SupEditprofile GetDetailsOFSuperAdmin(int ID)
        {
            SupEditprofile Result = new SupEditprofile();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select IsNull(ID,'') as ID, IsNull(Name,'') As Name, IsNull(Email,'') Email, IsNull(Password,'') as Password, Isnull(Mobile_No,'') As Mobile_No, IsNull(Profile_Image,'') as Profile_Image from[admin].[SuperAdmin_Details] where ID = @SupAdID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@SupAdID", ID);
                    //  com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.ID = Convert.ToInt16(reader["ID"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);
                        Result.Name = Convert.ToString(reader["Name"]);
                        Result.PhoneNo = Convert.ToString(reader["Mobile_No"]);
                        Result.ProfileImage = Convert.ToString(reader["Profile_Image"]);

                        //break;
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

        public String UpdateSuperAdminProfile(SupEditprofile Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    string Query = string.Empty;
                    if (Info.ProfileImage != null && Info.ProfileImage != "")
                    {
                        Query = @"Update [admin].[SuperAdmin_Details] Set Name=@SupName,Email=@SupEmail,Password=@SupPass,Mobile_No=@SupPhone,Profile_Image=@Imagepath where ID=@SupAdID";
                    }

                    else
                    {

                        Query = @"Update [admin].[SuperAdmin_Details] Set Name=@SupName,Email=@SupEmail,Password=@SupPass,Mobile_No=@SupPhone where ID=@SupAdID";

                    }
                    SqlCommand cmd = new SqlCommand(Query, connt);
                    // cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SupAdID", Info.ID);

                    cmd.Parameters.AddWithValue("@SupName", Info.Name);
                    cmd.Parameters.AddWithValue("@SupEmail", Info.Email);
                    cmd.Parameters.AddWithValue("@SupPass", Info.Password);
                    cmd.Parameters.AddWithValue("@SupPhone", Info.PhoneNo);
                    if (Info.ProfileImage != null && Info.ProfileImage != "")
                    {
                        cmd.Parameters.AddWithValue("@Imagepath", Info.ProfileImage);
                    }
                    connt.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        Res = "Sucess SuperADmin Profile Updated";
                    }
                    else
                    {
                        Res = "SuperAdmin Profile UpdationFailed!!";
                    }
                    connt.Close();
                }
            }
            catch (Exception ex)
            {


            }
            return Res;
        }


        #endregion

        public List<ClassInfo> GetClassBydivID(int ID)
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


        #region AddTeacherBySuperadmin

        public List<Login_Type> GetLoginType()
        {
            List<Login_Type> Result = new List<Login_Type>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand com = new SqlCommand("SP_GetLoginType", con);
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Login_Type temp = new Login_Type();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.LoginType = Convert.ToString(reader["LoginType"]);

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


        public List<Subject_Syllabus> SubForAutosuggestion(string Key)
        {
            List<Subject_Syllabus> List = new List<Subject_Syllabus>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string LikeKey = Key + "%";
                    string Query = "select distinct subject,ID from admin.Syllabus_Subject where subject like @key";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.Parameters.AddWithValue("@key", LikeKey);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Subject_Syllabus temp = new Subject_Syllabus();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["subject"]);

                        List.Add(temp);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return List;
        }
        public string AddTeacherBySuperAdmin(AddTeacherDetails Data)
        {
            string Result = string.Empty;
            try
            {
                if (!(CheckEMailExists(Data.Email)))
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        SqlCommand com = new SqlCommand("SP_TeacherbyPrincipal", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Email", Data.Email);
                        com.Parameters.AddWithValue("@Password", Data.Password);
                        com.Parameters.AddWithValue("@UserType", Data.TeacherType_FK);
                        com.Parameters.AddWithValue("@Gender", Data.Gender_FK);
                        com.Parameters.AddWithValue("@Dob", Data.TempStudentDOB);
                        com.Parameters.AddWithValue("@JoiningDate", Data.TempJoiningDate);
                        com.Parameters.AddWithValue("@Mobile", Data.MObile_No);
                        if (Data.Profile_Image != null && Data.Profile_Image != "")
                        {
                            com.Parameters.AddWithValue("@ProfileImage", Data.Profile_Image);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@ProfileImage", "");
                        }
                        com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                        com.Parameters.AddWithValue("@Registration", Data.Registration_No);
                        com.Parameters.AddWithValue("@Name", Data.Name);
                        com.Parameters.AddWithValue("@Subject", Data.Subject);
                        com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Inserted";
                        }
                        else
                        {
                            Result = "Failed!Failed Add Teacher";
                        }
                        con.Close();
                    }
                }
                else
                {
                    Result = "Failed!Email already exist";
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
            SqlConnection con = new SqlConnection(conn);
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

        public List<AddTeacherDetails> GetTeacherDetails(long SID)
        {
            List<AddTeacherDetails> Result = new List<AddTeacherDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select teacher.TeachersDetails.ID, isnull(teacher.TeachersDetails.Name,'') as Name,isnull(admin.users.Email,'')as Email,isnull(teacher.TeachersDetails.Subject,'') as Subject,admin.Login_Type.LoginType,teacher.TeachersDetails.TeacherType_FK,teacher.TeachersDetails.UID_FK
                                     from teacher.TeachersDetails
                                    inner join admin.users on  teacher.TeachersDetails.UID_FK = admin.users.UID
                                    inner join admin.Login_Type on  teacher.TeachersDetails.TeacherType_FK = admin.Login_Type.ID where teacher.TeachersDetails.SchoolID_FK =@SchoolID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@SchoolID", SID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AddTeacherDetails temp = new AddTeacherDetails();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.Name = Convert.ToString(reader["Name"]);
                        temp.Email = Convert.ToString(reader["Email"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.TeacherType_FK = Convert.ToInt64(reader["TeacherType_FK"]);
                        temp.LoginType = Convert.ToString(reader["LoginType"]);
                        temp.UID_FK = Convert.ToInt64(reader["UID_FK"]);

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

        public string DeleteTeacherInfo(long Id)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    SqlCommand com = new SqlCommand("SP_DeleteTeacher", con);
                    com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@UID", Id);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Deleted TeacherDetails";
                    }
                    else
                    {
                        Result = "Failed!Deleted";
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

        public EditTeacher EditTeacher(long ID)
        {
            EditTeacher Result = new EditTeacher();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select isnull(teacher.TeachersDetails.Gender_FK,'') as Gender_FK,
                                    isnull(teacher.TeachersDetails.DOB,'') as DOB, 
                                    isnull(teacher.TeachersDetails.Joining_Date,'') as Joining_Date,
                                    isnull(teacher.TeachersDetails.MObile_No,'') as MObile_No, 
                                    teacher.TeachersDetails.TeacherType_FK, 
                                    isnull(teacher.TeachersDetails.Registration_No,'') as Registration_No,
                                    teacher.TeachersDetails.UID_FK,
                                    isnull(teacher.TeachersDetails.Name,'') as Name,isnull(teacher.TeachersDetails.Subject,'') as Subject,
                                    isnull(teacher.TeachersDetails.Profile_Image,'') as Profile_Image, isnull(admin.users.Email,'') as Email, 
                                    isnull(admin.users.Password,'') as Password
                                    from teacher.TeachersDetails inner join admin.users on teacher.TeachersDetails.UID_FK = admin.users.UID
                                    where teacher.TeachersDetails.UID_FK = @UID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.Gender_FK = Convert.ToInt32(reader["Gender_FK"]);
                        Result.TempStudentDOB = Convert.ToString(reader["DOB"]);
                        Result.TempJoiningDate = Convert.ToString(reader["Joining_Date"]);
                        Result.MObile_No = Convert.ToString(reader["MObile_No"]);
                        Result.TeacherType_FK = Convert.ToInt32(reader["TeacherType_FK"]);
                        Result.Registration_No = Convert.ToString(reader["Registration_No"]);
                        Result.UID_FK = Convert.ToInt32(reader["UID_FK"]);
                        Result.Profile_Image = Convert.ToString(reader["Profile_Image"]);
                        Result.Name = Convert.ToString(reader["Name"]);
                        Result.Subject = Convert.ToString(reader["Subject"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);
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

    
        public string UpdateTeacherBySuperAdmin(EditTeacher Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = string.Empty;
                    if (Data.TempEmail == Data.Email)
                    {
                        if (Data.Profile_Image != null && Data.Profile_Image != "")
                        {
                            Query = @"Update [admin].[users] set Email=@Email,Password=@Password,UserType_FK=@UserType    
                                    where UID=@UID       
                                    update teacher.TeachersDetails set Gender_FK=@Gender,DOB=@Dob,Joining_Date=@JoiningDate,MObile_No=@Mobile,
                                    TeacherType_FK=@UserType,Profile_Image=@ProfileImage, CreatedBy=@CreatedBy, 
                                    Registration_No=@Registration,Name=@Name, Subject=@Subject,SchoolID_FK=@SchoolID                                        
                                    where UID_FK=@UID";
                        }
                        else
                        {
                            Query = @"Update [admin].[users] set Email=@Email,Password=@Password,UserType_FK=@UserType    
                                    where UID=@UID       
                                    update teacher.TeachersDetails set Gender_FK=@Gender,DOB=@Dob,Joining_Date=@JoiningDate,MObile_No=@Mobile,
                                    TeacherType_FK=@UserType,CreatedBy=@CreatedBy,Registration_No=@Registration,Name=@Name,                                         
                                    Subject=@Subject,SchoolID_FK=@SchoolID
                                    where UID_FK=@UID";
                        }
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@UID", Data.UID_FK);
                        com.Parameters.AddWithValue("@Email", Data.Email);
                        com.Parameters.AddWithValue("@Password", Data.Password);
                        com.Parameters.AddWithValue("@UserType", Data.TeacherType_FK);
                        com.Parameters.AddWithValue("@Gender", Data.Gender_FK);
                        com.Parameters.AddWithValue("@Dob", Data.TempStudentDOB);
                        com.Parameters.AddWithValue("@JoiningDate", Data.TempJoiningDate);
                        com.Parameters.AddWithValue("@Mobile", Data.MObile_No);
                        com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                        com.Parameters.AddWithValue("@Registration", Data.Registration_No);
                        com.Parameters.AddWithValue("@Name", Data.Name);
                        com.Parameters.AddWithValue("@Subject", Data.Subject);
                        com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                        if (Data.Profile_Image != null && Data.Profile_Image != "")
                        {
                            com.Parameters.AddWithValue("@ProfileImage", Data.Profile_Image);
                        }
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Successfully Updated";
                        }
                        else
                        {
                            Result = "Failed!Process Failed ";
                        }
                        con.Close();
                    }
                    else
                    {
                        if (!(CheckEMailExists(Data.Email)))
                        {
                            if (Data.Profile_Image != null && Data.Profile_Image != "")
                            {
                                Query = @"Update [admin].[users] set Email=@Email,Password=@Password,UserType_FK=@UserType    
                                        where UID=@UID       
                                        update teacher.TeachersDetails set Gender_FK=@Gender,DOB=@Dob,Joining_Date=@JoiningDate,MObile_No=@Mobile,
                                        TeacherType_FK=@UserType, Profile_Image=@ProfileImage,CreatedBy=@CreatedBy,  
                                        Registration_No=@Registration,Name=@Name, Subject=@Subject,SchoolID_FK=@SchoolID                                       
                                        where UID_FK=@UID";
                            }
                            else
                            {
                                Query = @"Update [admin].[users] set Email=@Email,Password=@Password,UserType_FK=@UserType    
                                        where UID=@UID       
                                        update teacher.TeachersDetails set Gender_FK=@Gender,DOB=@Dob,Joining_Date=@JoiningDate,MObile_No=@Mobile,
                                        TeacherType_FK=@UserType,CreatedBy=@CreatedBy,Registration_No=@Registration,Name=@Name,                                           
                                        Subject=@Subject,SchoolID_FK=@SchoolID                                 
                                        where UID_FK=@UID";
                            }
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@UID", Data.UID_FK);
                            com.Parameters.AddWithValue("@Email", Data.Email);
                            com.Parameters.AddWithValue("@Password", Data.Password);
                            com.Parameters.AddWithValue("@UserType", Data.TeacherType_FK);
                            com.Parameters.AddWithValue("@Gender", Data.Gender_FK);
                            com.Parameters.AddWithValue("@Dob", Data.TempStudentDOB);
                            com.Parameters.AddWithValue("@JoiningDate", Data.TempJoiningDate);
                            com.Parameters.AddWithValue("@Mobile", Data.MObile_No);
                            com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                            com.Parameters.AddWithValue("@Registration", Data.Registration_No);
                            com.Parameters.AddWithValue("@Name", Data.Name);
                            com.Parameters.AddWithValue("@Subject", Data.Subject);
                            com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                            if (Data.Profile_Image != null && Data.Profile_Image != "")
                            {
                                com.Parameters.AddWithValue("@ProfileImage", Data.Profile_Image);
                            }
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Successfully Updated";
                            }
                            else
                            {
                                Result = "Failed!Process Failed ";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "MailID already exits";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }





        //teacher view
        public AddTeacherDetails ViewDetailsOfTeacher(long ID)
        {
            AddTeacherDetails Result = new AddTeacherDetails();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Select UID_FK,DOB,Subject,Name from teacher.TeachersDetails
                                        where UID_FK=@UID_FK";

                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UID_FK", ID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.TempStudentDOB = Convert.ToString(reader["DOB"]);
                        Result.Subject = Convert.ToString(reader["Subject"]);
                        Result.Name = Convert.ToString(reader["Name"]);

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
        #endregion

        #region AddStudentBySuperadmin
        public List<ClassStream> GetStreamList(int ID)
        {
            List<ClassStream> Result = new List<ClassStream>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" select ID,Stream from admin.ClassStream where Level_ID=@Level_ID ";
                    SqlCommand com = new SqlCommand(query, con);
                    // SqlCommand com = new SqlCommand("SP_GetAllStream", con);
                    //  com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@Level_ID", ID);
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

        public string InsertStudentDetails(AddStudentDetails Data)
        {
            string Result = string.Empty;
            try
            {
                if (!(ChkRegNoExits(Data.Stu_RegistrationNo)))
                {
                    if (!(ChkMailExists(Data.Email)))
                    {
                        using (SqlConnection con = new SqlConnection(conn))
                        {
                            string Query = string.Empty;
                            if (Data.Stu_Profile_Image != null && Data.Stu_Profile_Image != "")
                            {
                                Query = @"DECLARE @UID BIGINT INSERT INTO admin.users (Email,Password,UserType_FK) VALUES (@Email,
                                        @Password,4);SET @UID = SCOPE_IDENTITY() 
                                        INSERT INTO studentinfo.Student_Details (Stu_Name,Stu_DOB,Stu_JoiningDt,Stu_MObile_No,
                                        Stu_Profile_Image,Stu_RegistrationNo,Stu_FutureAmbition,Stu_Favourite_Books,Level_ID,
                                        DivisionID,CreatedDate,Stu_Gender_FK,
                                        IsFirstStepComplete,UserID_FK,CreatedBy_FK,School_ID,Class_ID,LoginTpe_FK) 
                                        VALUES (@Stu_Name,@Stu_DOB,@Stu_JoiningDt,@Stu_MObile_No,@Stu_Profile_Image,@Stu_RegistrationNo,
                                        @Stu_FutureAmbition,@Stu_Favourite_Books,@Level_ID,@DivisionID,Getdate(),@Stu_Gender_FK,1,@UID,
                                        @CreatedBy_FK,@School_ID,@Class_ID,4)";
                            }
                            else
                            {
                                Query = @"DECLARE @UID BIGINT INSERT INTO admin.users (Email,Password,UserType_FK) VALUES (@Email,
                                        @Password,4);SET @UID = SCOPE_IDENTITY() 
                                        INSERT INTO studentinfo.Student_Details (Stu_Name,Stu_DOB,Stu_JoiningDt,
                                        Stu_MObile_No,Stu_RegistrationNo,Stu_FutureAmbition,Stu_Favourite_Books,Level_ID,
                                        DivisionID,CreatedDate,Stu_Gender_FK,
                                        IsFirstStepComplete,UserID_FK,CreatedBy_FK,School_ID,Class_ID,LoginTpe_FK) 
                                        VALUES (@Stu_Name,@Stu_DOB,@Stu_JoiningDt,@Stu_MObile_No,@Stu_RegistrationNo,
                                        @Stu_FutureAmbition,@Stu_Favourite_Books,@Level_ID,@DivisionID,Getdate(),@Stu_Gender_FK,1,@UID,
                                        @CreatedBy_FK,@School_ID,@Class_ID,4)";
                            }
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@Email", Data.Email);
                            com.Parameters.AddWithValue("@Password", Data.Password);
                            com.Parameters.AddWithValue("@Stu_Name", Data.Stu_Name);
                            com.Parameters.AddWithValue("@Stu_DOB", Data.Stu_DOB);
                            com.Parameters.AddWithValue("@Stu_JoiningDt", Data.Stu_JoiningDt);
                            com.Parameters.AddWithValue("@Stu_MObile_No", Data.Stu_MObile_No);
                            com.Parameters.AddWithValue("@Stu_RegistrationNo", Data.Stu_RegistrationNo);
                            com.Parameters.AddWithValue("@Stu_Favourite_Books", Data.Stu_Favourite_Books);
                            com.Parameters.AddWithValue("@Stu_FutureAmbition", Data.Stu_FutureAmbition);
                            com.Parameters.AddWithValue("@DivisionID", Data.DivisionID);
                            com.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                            com.Parameters.AddWithValue("@Stu_Gender_FK", Data.Stu_Gender_FK);
                            com.Parameters.AddWithValue("@CreatedBy_FK", Data.CreatedBy_FK);
                            com.Parameters.AddWithValue("@School_ID", Data.School_ID);
                            com.Parameters.AddWithValue("@Class_ID", Data.Class_ID);
                            if (Data.Stu_Profile_Image != null && Data.Stu_Profile_Image != "")
                            {
                                com.Parameters.AddWithValue("@Stu_Profile_Image", Data.Stu_Profile_Image);
                            }
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                if (Data.Stu_Profile_Image != null && Data.Stu_Profile_Image != "")
                                {
                                    Result = Data.Stu_Profile_Image;
                                    Result = "Success| Student Insert Sucessfully";
                                }
                                else
                                {
                                    Result = "Success|Student Insert Sucessfully";
                                }
                            }
                            else
                            {
                                Result = "Failed|Failed to add Student";
                            }
                            con.Close();
                        }
                    }
                    else
                    {
                        Result = "Failed|Entered Email Already Exists";
                    }
                }
                else
                {
                    Result = "Failed|Entered Student Registration No Already Exists";
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }


        public bool ChkMailExists(string MailID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
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


        public bool ChkRegNoExits(string RegNo)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select * from studentinfo.Student_Details where Stu_RegistrationNo=@Stu_RegistrationNo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Stu_RegistrationNo", RegNo);
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

        #endregion



        #region CLASS 

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

        public string InsertClassInfo(AddClassInfo Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"insert into admin.Class 
                                    (Class_Name,Div_ID,Level_ID,CreatedBy_ID,School_ID )
                                    values(@Class_Name,@Div_ID,@Level_ID,@CreatedBy_ID,@School_ID)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Class_Name", Data.Class_Name);
                    com.Parameters.AddWithValue("@Div_ID", Data.Div_ID);
                    com.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                    com.Parameters.AddWithValue("@CreatedBy_ID", Data.CreatedBy_ID);
                    com.Parameters.AddWithValue("@School_ID", Data.School_ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Class Inserted";
                    }
                    else
                    {
                        Result = "Failed| Failed class Insertion";
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

        public bool ChkClass(AddClassInfo Data)
        {
            bool Result = false;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" select ID from admin.Class where Class_Name=@Class_Name And Div_ID=@Div_ID And Level_ID=@Level_ID And School_ID=@School_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Class_Name", Data.Class_Name);
                    cmd.Parameters.AddWithValue("@Div_ID", Data.Div_ID);
                    cmd.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                    cmd.Parameters.AddWithValue("@School_ID", Data.School_ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result = true;
                        return Result;
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

        #region ClassList

        public List<ClassDetails> ClassList(string schoolId)
        {
            List<ClassDetails> obj = new List<ClassDetails>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                string query = @"select admin.Class.*,admin.Division.Division_Name, admin.Level.Level_Name 
                                from admin.Class 
                                left join admin.Division On admin.Class.Div_ID=admin.Division.ID 
                                left join admin.Level on admin.Class.Level_ID=admin.Level.ID 
                                where admin.Class.School_ID=@SchoolID";
                SqlCommand cmd = new SqlCommand(query, connt);
                cmd.Parameters.AddWithValue("@SchoolID", Convert.ToInt64(schoolId));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<ClassDetails>>(str);
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        #endregion

        public string DeleteClass(long Id)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @"Delete from studentinfo.Student_Details Where Class_ID =@Id
                                     Delete from admin.Class Where Id =@Id";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Id", Id);
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
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public ClassDetails SelectClassById(long Id)
        {
            ClassDetails Result = new ClassDetails();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select admin.Class.*,admin.Division.Division_Name, admin.Level.Level_Name from admin.Class 
                                    left join admin.Division On admin.Class.Div_ID=admin.Division.ID 
                                    left join admin.Level on admin.Class.Level_ID=admin.Level.ID 
                                    where admin.Class.ID=@Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        Result = JsonConvert.DeserializeObject<List<ClassDetails>>(str)[0];
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

        //view class
        public ClassDetails CountNoStudents(long Class_ID)
        {
            ClassDetails Result = new ClassDetails();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"SELECT COUNT(UserID_FK) as No_Of_Students
                                    FROM studentinfo.Student_Details where Class_ID=@Class_ID";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@Class_ID", Class_ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        Result = JsonConvert.DeserializeObject<List<ClassDetails>>(str)[0];
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


        public ViewClassInfo ViewClassInfo(long Class_ID)
        {
            ViewClassInfo Result = new ViewClassInfo();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Isnull(cls.Class_Name,'') as Class_Name ,Isnull(lvl.Level_Name,'') as Level_Name
                                        from admin.Class as cls 
                                        INNER JOIN admin.Level as lvl on cls.Level_ID=lvl.ID
                                        where cls.ID=@Class_ID";


                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@Class_ID", Class_ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        Result = JsonConvert.DeserializeObject<List<ViewClassInfo>>(str)[0];
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


        public string UpdateClassInfo(AddClassInfo Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @"update admin.Class set Class_Name=@ClassName,Div_ID=@DivID,Level_ID=@LevelID where ID=@ID and CreatedBy_ID=@CreatedBy and School_ID=@SchoolID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ClassName", Data.Class_Name);
                    com.Parameters.AddWithValue("@DivID", Data.Div_ID);
                    com.Parameters.AddWithValue("@LevelID", Data.Level_ID);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy_ID);
                    com.Parameters.AddWithValue("@SchoolID", Data.School_ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success!Successfully Updated";
                    }
                    else
                    {
                        Result = "Failed!Failed Update Class";
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

        #region ramashree
        public List<Division> GetAllDivision()
        {
            List<Division> Result = new List<Division>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    // SqlCommand com = new SqlCommand("SP_GetAllDivision", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    string query = @"SELECT ID,Division_Name FROM admin.Division";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Division temp = new Division();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Division_Name = Convert.ToString(reader["Division_Name"]);
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

        public List<Class_Details> GetClass(int ID)
        {
            List<Class_Details> Result = new List<Class_Details>();
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
                        Class_Details temp = new Class_Details();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Class_Name = Convert.ToString(reader["Class_Name"]);
                        temp.Div_ID = Convert.ToInt16(reader["Div_ID"]);
                        temp.Level_ID = Convert.ToInt16(reader["Level_ID"]);
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

        #endregion

        #region sneha
        public List<StudentListing> StudentList(long School_ID)
        {
            List<StudentListing> obj = new List<StudentListing>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                string Query = @"select stu.UserID_FK,stu.School_ID,stu.CreatedBy_FK,stu.Stu_RegistrationNo,stu.Stu_Name,stu.DivisionID,stu.Level_ID,stu.Class_ID,cls.Class_Name,div.Division_Name,lvl.Level_Name
	                            from studentinfo.Student_Details as stu
	                            Inner JOIN admin.Class as cls ON stu.Class_ID=cls.ID
	                            Inner Join admin.Division as div ON stu.DivisionID=div.ID
	                            Inner Join admin.Level as lvl ON stu.Level_ID=lvl.ID
                                where stu.School_ID=@School_ID                                
                                order by stu.UserID_FK desc";
                SqlCommand cmd = new SqlCommand(Query, connt);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@School_ID", School_ID);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<StudentListing>>(str);
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public AddStudentDetails GetDetailsOfStudent(long ID)
        {
            AddStudentDetails Result = new AddStudentDetails();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Usr.Email,Usr.Password,Stu.UserID_FK,Stu.Stu_Name,
                                    Isnull(Stu.Stu_DOB,'') as Stu_DOB,IsNull(Stu.Stu_JoiningDt,'') as Stu_JoiningDt,
                                    IsNull(Stu.Stu_MObile_No,'')as Stu_MObile_No,
                                    IsNull(Stu.Stu_Profile_Image,'') as Stu_Profile_Image,
                                    IsNull(Stu.Stu_RegistrationNo,'') as Stu_RegistrationNo,
                                    IsNull(Stu.Stu_FutureAmbition,'') as Stu_FutureAmbition,
                                    IsNull(Stu.Stu_Favourite_Books,'') As Stu_Favourite_Books,
                                    IsNull(Stu.Level_ID,'') as Level_ID,IsNull(Stu.DivisionID,'') as DivisionID,
                                    Isnull(Stu.Stu_Gender_FK,'') as Stu_Gender_FK,
                                    Isnull(Stu.Stream_ID_FK,'') as Stream_ID_FK,IsNull(Stu.School_ID,'') as School_ID,
                                    IsNull(Stu.Class_ID,'') as Class_ID,Stu.CreatedBy_FK
                                    from admin.users as Usr
                                    INNER JOIN  studentinfo.Student_Details as Stu
                                    ON stu.UserID_FK=Usr.UID
                                    where UserID_FK=@UserID_FK";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID_FK", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        Result.Password = Convert.ToString(reader["Password"]);
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.TempStudentDOB = Convert.ToString(reader["Stu_DOB"]);
                        Result.TempJoiningDate = Convert.ToString(reader["Stu_JoiningDt"]);
                        Result.Stu_MObile_No = Convert.ToString(reader["Stu_MObile_No"]);
                        Result.Stu_Profile_Image = Convert.ToString(reader["Stu_Profile_Image"]);
                        Result.Stu_RegistrationNo = Convert.ToString(reader["Stu_RegistrationNo"]);
                        Result.Stu_Favourite_Books = Convert.ToString(reader["Stu_Favourite_Books"]);
                        Result.Stu_FutureAmbition = Convert.ToString(reader["Stu_FutureAmbition"]);
                        Result.Level_ID = Convert.ToInt16(reader["Level_ID"]);
                        Result.DivisionID = Convert.ToInt16(reader["DivisionID"]);
                        Result.Stu_Gender_FK = Convert.ToInt32(reader["Stu_Gender_FK"]);
                        Result.Stream_ID_FK = Convert.ToInt16(reader["Stream_ID_FK"]);
                        Result.Class_ID = Convert.ToInt32(reader["Class_ID"]);
                        Result.School_ID = Convert.ToInt32(reader["School_ID"]);
                        Result.CreatedBy_FK = Convert.ToInt32(reader["CreatedBy_FK"]);
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

        public StudentListing ViewDetailsOfStudent(long ID)
        {
            StudentListing Result = new StudentListing();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string Query = @"select IsNull(Stu.Stu_Name,'') as Stu_Name,IsNull(Div.Division_Name,'') as Division_Name,
                                    IsNull(lvl.Level_Name,'') as Level_Name,Isnull(Cls.Class_Name,'') as Class_Name 
                                    from studentinfo.Student_Details   as Stu                         
                                    inner Join admin.Division as Div on Stu.DivisionID=Div.ID inner  JOIn admin.Level as lvl on
                                    Stu.Level_ID=lvl.ID inner Join admin.Class as Cls on Stu.Class_ID=Cls.ID
                                    where Stu.UserID_FK=@UserID_FK ";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID_FK", ID);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        Result.Division_Name = Convert.ToString(reader["Division_Name"]);
                        Result.Level_Name = Convert.ToString(reader["Level_Name"]);
                        Result.Class_Name = Convert.ToString(reader["Class_Name"]);
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




        public bool DeleteStudentInfo(long UID)
        {
            bool ret = true;
            string query = @" alter table studentinfo.Student_Details nocheck constraint all
            alter table studentinfo.Study_Group nocheck constraint all
            alter table studentinfo.Study_Group_Comment nocheck constraint all
            alter table studentinfo.Study_Group_Member nocheck constraint all
            alter table studentinfo.Study_Group_Topic nocheck constraint all
            delete from studentinfo.Student_Details where UserID_FK=@UID
			delete from  admin.users  where UID=@UID
            alter table studentinfo.Student_Details check constraint all;
            alter table studentinfo.Study_Group check constraint all
            alter table studentinfo.Study_Group_Comment check constraint all
            alter table studentinfo.Study_Group_Member check constraint all
            alter table studentinfo.Study_Group_Topic check constraint all";
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    cmd.Parameters.AddWithValue("@UID", UID);
                    int z = cmd.ExecuteNonQuery();
                    if (z > 0)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch (Exception ex)
                {
                    ret = false;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return ret;
        }



        public string UpdateUserTable(AddStudentDetails data)
        {
            string res = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.users set Email=@Email, Password=@Password where UID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", data.UserID_FK);
                    com.Parameters.AddWithValue("@Email", data.Email);
                    com.Parameters.AddWithValue("@Password", data.Password);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        res = "Success! User record Updated ";
                    }
                    else
                    {
                        res = "Failed!";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public string UpdateStudentRecord(AddStudentDetails Info)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = string.Empty;
                    if (Info.TempEmail == Info.Email)
                    {
                        if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                        {
                            Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Stu_Name,Stu_Profile_Image=@Stu_Profile_Image,
                                Level_ID=@Level_ID,DivisionID=@DivisionID,Stu_DOB=@Stu_DOB,Stu_MObile_No=@Stu_MObile_No,
                                Stu_JoiningDt=@Stu_JoiningDt,Stu_Gender_FK=@Stu_Gender_FK,Stu_RegistrationNo=@Stu_RegistrationNo,
                                Stu_Favourite_Books=@Stu_Favourite_Books,Stu_FutureAmbition=@Stu_FutureAmbition,Class_ID=@Class_ID,
                                IsFirstStepComplete=1  where UserID_FK=@UserID_FK
                                Update parent.Parents_Details SET Stu_Registration_NO=@Stu_RegistrationNo where 
                                Stu_Registration_NO=@SID ";
                        }
                        else
                        {
                            Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Stu_Name,Level_ID=@Level_ID,
                                    DivisionID=@DivisionID,Stu_DOB=@Stu_DOB,
                                    Stu_MObile_No=@Stu_MObile_No,Stu_JoiningDt=@Stu_JoiningDt,
                                    Stu_Gender_FK=@Stu_Gender_FK,Stu_RegistrationNo=@Stu_RegistrationNo,
                                    Stu_Favourite_Books=@Stu_Favourite_Books,Stu_FutureAmbition=@Stu_FutureAmbition,
                                    Class_ID=@Class_ID, IsFirstStepComplete=1  where UserID_FK=@UserID_FK
                                    Update parent.Parents_Details SET Stu_Registration_NO=@Stu_RegistrationNo where 
                                    Stu_Registration_NO=@SID";
                        }
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                        com.Parameters.AddWithValue("@Stu_Name", Info.Stu_Name);
                        com.Parameters.AddWithValue("@Stu_DOB", Info.Stu_DOB);
                        com.Parameters.AddWithValue("@Stu_JoiningDt", Info.Stu_JoiningDt);
                        com.Parameters.AddWithValue("@Stu_MObile_No", Info.Stu_MObile_No);
                        com.Parameters.AddWithValue("@Stu_RegistrationNo", Info.Stu_RegistrationNo == null ? "" : Info.Stu_RegistrationNo);
                        com.Parameters.AddWithValue("@SID", Info.tempStu_RegistrationNo);
                        com.Parameters.AddWithValue("@Stu_Favourite_Books", Info.Stu_Favourite_Books == null ? "" : Info.Stu_Favourite_Books);
                        com.Parameters.AddWithValue("@Stu_FutureAmbition", Info.Stu_FutureAmbition == null ? "" : Info.Stu_FutureAmbition);
                        com.Parameters.AddWithValue("@DivisionID", Info.DivisionID);
                        com.Parameters.AddWithValue("@Stu_Gender_FK", Info.Stu_Gender_FK);
                        com.Parameters.AddWithValue("@Level_ID", Info.Level_ID);
                        com.Parameters.AddWithValue("@Class_ID", Info.Class_ID);
                        if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                        {
                            com.Parameters.AddWithValue("@Stu_Profile_Image", Info.Stu_Profile_Image);
                        }
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {

                            Result = "Success!Successfully Updated";
                        }
                        else
                        {
                            Result = "Failed!";
                        }
                        con.Close();
                    }
                    else
                    {
                        if (!(CheckEMailExists(Info.Email)))
                        {
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Stu_Name,Stu_Profile_Image=@Stu_Profile_Image,
                                        Level_ID=@Level_ID,DivisionID=@DivisionID,Stu_DOB=@Stu_DOB,Stu_MObile_No=@Stu_MObile_No,
                                        Stu_JoiningDt=@Stu_JoiningDt,Stu_Gender_FK=@Stu_Gender_FK,Stu_RegistrationNo=@Stu_RegistrationNo,
                                        Stu_Favourite_Books=@Stu_Favourite_Books,Stu_FutureAmbition=@Stu_FutureAmbition,Class_ID=@Class_ID,
                                        IsFirstStepComplete=1  where UserID_FK=@UserID_FK
                                        Update parent.Parents_Details SET Stu_Registration_NO=@Stu_RegistrationNo where 
                                        Stu_Registration_NO=@SID";
                            }
                            else
                            {
                                Query = @"Update [studentinfo].[Student_Details] SET Stu_Name=@Stu_Name,Level_ID=@Level_ID,
                                        DivisionID=@DivisionID,Stu_DOB=@Stu_DOB,
                                        Stu_MObile_No=@Stu_MObile_No,Stu_JoiningDt=@Stu_JoiningDt,
                                        Stu_Gender_FK=@Stu_Gender_FK,Stu_RegistrationNo=@Stu_RegistrationNo,
                                        Stu_Favourite_Books=@Stu_Favourite_Books,Stu_FutureAmbition=@Stu_FutureAmbition,
                                        Class_ID=@Class_ID, IsFirstStepComplete=1  where UserID_FK=@UserID_FK
                                        Update parent.Parents_Details SET Stu_Registration_NO=@Stu_RegistrationNo where 
                                        Stu_Registration_NO=@SID";
                            }
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                            com.Parameters.AddWithValue("@Stu_Name", Info.Stu_Name);
                            com.Parameters.AddWithValue("@Stu_DOB", Info.Stu_DOB);
                            com.Parameters.AddWithValue("@Stu_JoiningDt", Info.Stu_JoiningDt);
                            com.Parameters.AddWithValue("@Stu_MObile_No", Info.Stu_MObile_No);
                            com.Parameters.AddWithValue("@Stu_RegistrationNo", Info.Stu_RegistrationNo == null ? "" : Info.Stu_RegistrationNo);
                            com.Parameters.AddWithValue("@SID", Info.tempStu_RegistrationNo);
                            com.Parameters.AddWithValue("@Stu_Favourite_Books", Info.Stu_Favourite_Books == null ? "" : Info.Stu_Favourite_Books);
                            com.Parameters.AddWithValue("@Stu_FutureAmbition", Info.Stu_FutureAmbition == null ? "" : Info.Stu_FutureAmbition);
                            com.Parameters.AddWithValue("@DivisionID", Info.DivisionID);
                            com.Parameters.AddWithValue("@Stu_Gender_FK", Info.Stu_Gender_FK);
                            com.Parameters.AddWithValue("@Level_ID", Info.Level_ID);
                            com.Parameters.AddWithValue("@Class_ID", Info.Class_ID);
                            if (Info.Stu_Profile_Image != null && Info.Stu_Profile_Image != "")
                            {
                                com.Parameters.AddWithValue("@Stu_Profile_Image", Info.Stu_Profile_Image);
                            }
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Successfully Updated";
                            }
                            else
                            {
                                Result = "Failed!";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "MailID already exits";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }
        #endregion


        #region Ramashree 05/12/2018    
        public SuperAdminEditprofile GetDetailsOFEditProfileSuperAdmin(long ID)
        {
            SuperAdminEditprofile Result = new SuperAdminEditprofile();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select Isnull(Sad.UserID_FK,'')as UserID_FK,Isnull(Sad.Name,'')as Name,
                                    Isnull(Sad.Gender_FK,'')as Gender_FK,Isnull(Sad.DOB,'') as DOB,
                                    Isnull(Sad.Mobile_No,'')as Mobile_No, Isnull(Sad.Profile_Image,'') as Profile_Image,
                                    Isnull(Users.Email,'') as Email,Isnull(Users.Password,'') as Password
                                    from   admin.SuperAdmin_Details  as Sad                        
                                    inner Join admin.users as Users on 
                                    Sad.UserID_FK=Users.UID 
                                    where UserID_FK=@UserID_FK";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@UserID_FK", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.UserID_FK = Convert.ToInt32(reader["UserID_FK"]);
                        Result.Name = Convert.ToString(reader["Name"]);
                        Result.Gender_FK = Convert.ToInt16(reader["Gender_FK"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);
                        Result.TempDOB = Convert.ToString(reader["DOB"]);
                        Result.Mobile_No = Convert.ToString(reader["Mobile_No"]);
                        Result.Profile_Image = Convert.ToString(reader["Profile_Image"]);
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

        public String UpdateSuperAdminEditProfile(SuperAdminEditprofile Info)
        {
            string Res = string.Empty;
            try
            {
                using (SqlConnection connt = new SqlConnection(conn))
                {
                    string Query = string.Empty;
                    if (Info.TempEmail == Info.Email)
                    {
                        if (Info.Profile_Image != null && Info.Profile_Image != "")
                        {
                            Query = @" Update [admin].[users] set Password=@Password where UID=@UserID_FK
                                   update admin.SuperAdmin_Details set UserID_FK=@UserID_FK,Name=@Name,
                                   Gender_FK=@Gender_FK,DOB=@DOB,MObile_No=@Mobile_No,
                                   Profile_Image=@Profile_Image 
                                   where UserID_FK=@UserID_FK";
                        }
                        else
                        {
                            Query = @" Update [admin].[users] set Password=@Password where UID=@UserID_FK
                                   update admin.SuperAdmin_Details set UserID_FK=@UserID_FK,Name=@Name,
                                   Gender_FK=@Gender_FK,DOB=@DOB,MObile_No=@Mobile_No
                                   where UserID_FK=@UserID_FK";
                        }
                        SqlCommand cmd = new SqlCommand(Query, connt);
                        cmd.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                        cmd.Parameters.AddWithValue("@Name", Info.Name);
                        cmd.Parameters.AddWithValue("@Gender_FK", Info.Gender_FK);
                        cmd.Parameters.AddWithValue("@Password", Info.Password);
                        cmd.Parameters.AddWithValue("@DOB", Info.DOB);
                        cmd.Parameters.AddWithValue("@Mobile_No", Info.Mobile_No);
                        if (Info.Profile_Image != null && Info.Profile_Image != "")
                        {
                            cmd.Parameters.AddWithValue("@Profile_Image", Info.Profile_Image);
                        }
                        connt.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Res = "Success!Successfully Updated";
                        }
                        else
                        {
                            Res = "Failed!Process Failed ";
                        }
                        connt.Close();
                    }
                    else
                    {
                        if (!(CheckEMailExists(Info.Email)))
                        {
                            if (Info.Profile_Image != null && Info.Profile_Image != "")
                            {
                                Query = @" Update [admin].[users] set Password=@Password where UID=@UserID_FK
                                   update admin.SuperAdmin_Details set UserID_FK=@UserID_FK,Name=@Name,
                                   Gender_FK=@Gender_FK,DOB=@DOB,MObile_No=@Mobile_No,
                                   Profile_Image=@Profile_Image 
                                   where UserID_FK=@UserID_FK";
                            }
                            else
                            {
                                Query = @" Update [admin].[users] set Password=@Password where UID=@UserID_FK
                                   update admin.SuperAdmin_Details set UserID_FK=@UserID_FK,Name=@Name,
                                   Gender_FK=@Gender_FK,DOB=@DOB,MObile_No=@Mobile_No
                                   where UserID_FK=@UserID_FK";
                            }
                            SqlCommand cmd = new SqlCommand(Query, connt);
                            cmd.Parameters.AddWithValue("@UserID_FK", Info.UserID_FK);
                            cmd.Parameters.AddWithValue("@Name", Info.Name);
                            cmd.Parameters.AddWithValue("@Gender_FK", Info.Gender_FK);
                            cmd.Parameters.AddWithValue("@Password", Info.Password);
                            cmd.Parameters.AddWithValue("@DOB", Info.DOB);
                            cmd.Parameters.AddWithValue("@Mobile_No", Info.Mobile_No);
                            if (Info.Profile_Image != null && Info.Profile_Image != "")
                            {
                                cmd.Parameters.AddWithValue("@Profile_Image", Info.Profile_Image);
                            }
                            connt.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                Res = "Success!Successfully Updated";
                            }
                            else
                            {
                                Res = "Failed!Process Failed";
                            }
                            connt.Close();
                        }

                        else
                        {
                            Res = "MailID already exits";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Res;
        }
        #endregion

        #region super  SUBJECT TAB, S.M

        public bool Chksubj(AddsubjectInfo Data)
        {
            bool Result = false;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @" select ID from admin.Syllabus_Subject where Subject=@Subject  And Level_ID=@Level_ID And School_ID=@School_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Subject", Data.Subject);
                    cmd.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                    cmd.Parameters.AddWithValue("@School_ID", Data.School_ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result = true;
                        return Result;
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


        public List<Level> GetAllLevel()
        {
            List<Level> Result = new List<Level>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"SELECT ID,Level_Name FROM admin.Level";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
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

        //edit subject

       

        //subject delete
        public string DeleteSubject(int Id)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Delete from admin.Syllabus_Subject Where ID=@Id";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Selected Subject Deleted";
                    }
                    else
                    {
                        Result = "Failed|Delete failed";
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

      

        //29,2018
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
                        temp.Div_ID = Convert.ToInt32(reader["Div_ID"]);
                        temp.Level_ID = Convert.ToInt32(reader["level_ID"]);
                        temp.School_ID = Convert.ToInt32(reader["School_ID"]);
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

        public List<Syllabus_Subject> GetAllSubject(int ID)
        {
            List<Syllabus_Subject> Result = new List<Syllabus_Subject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"SELECT ID,Subject,Level_ID,School_ID,Dept_SubID FROM admin.Syllabus_Subject where Level_ID=@Level_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Level_ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Syllabus_Subject temp = new Syllabus_Subject();
                        temp.SubjectID = Convert.ToInt32(reader["ID"]);
                        temp.Subject = Convert.ToString(reader["Subject"]);                      
                        temp.Level_ID = Convert.ToInt32(reader["Level_ID"]);
                        temp.School_ID = Convert.ToInt32(reader["School_ID"]);
                        temp.Dept_SubID = Convert.ToInt32(reader["Dept_SubID"]);
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

        #region AddparentBySuperadmin
        public bool CheckRegistrationExists(string RegNo)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select Stu_ID,Stu_Name from studentinfo.Student_Details where Stu_RegistrationNo=@RegNo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RegNo", RegNo);
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

        public string AddParentBySuperAdmin(ParentDetails Data)
        {
            string Result = string.Empty;
            try
            {
                if ((CheckRegistrationExists(Data.Stu_Registration_NO)))
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        if (!(CheckEMailExists(Data.Email)))
                        {
                            if (!(CheckStudentRegs(Data.Stu_Registration_NO)))
                            {
                                SqlCommand com = new SqlCommand("SP_ParentbyPrincipal", con);
                                com.CommandType = CommandType.StoredProcedure;
                                com.Parameters.AddWithValue("@Email", Data.Email);
                                com.Parameters.AddWithValue("@Password", Data.Password);
                                com.Parameters.AddWithValue("@UserType", 5);
                                com.Parameters.AddWithValue("@Mobile", Data.Mobile_No);
                                if (Data.Profile_Image != null && Data.Profile_Image != "")
                                {
                                    com.Parameters.AddWithValue("@Profile_Image", Data.Profile_Image);
                                }
                                else
                                {
                                    com.Parameters.AddWithValue("@Profile_Image", "");
                                }
                                com.Parameters.AddWithValue("@Registration", Data.Stu_Registration_NO);
                                com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                                com.Parameters.AddWithValue("@Name", Data.Name);
                                com.Parameters.AddWithValue("@Address", Data.Address);
                                com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                                con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {

                                    Result = "Success!Inserted";
                                }
                                else
                                {
                                    Result = "Failed!Failed Add Teacher";
                                }
                                con.Close();
                            }
                            else
                            {
                                Result = "Failed!Student's Registration No alredy exists";
                            }
                        }
                        else
                        {
                            if (!(CheckStudentRegs(Data.Stu_Registration_NO)))
                            {
                                SqlCommand com = new SqlCommand("SP_ParentbyPrincipal2", con);
                                com.CommandType = CommandType.StoredProcedure;
                                com.Parameters.AddWithValue("@Email", Data.Email);
                                com.Parameters.AddWithValue("@Mobile", Data.Mobile_No);
                                if (Data.Profile_Image != null && Data.Profile_Image != "")
                                {
                                    com.Parameters.AddWithValue("@Profile_Image", Data.Profile_Image);
                                }
                                else
                                {
                                    com.Parameters.AddWithValue("@Profile_Image", "");
                                }
                                com.Parameters.AddWithValue("@Registration", Data.Stu_Registration_NO);
                                com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                                com.Parameters.AddWithValue("@Name", Data.Name);
                                com.Parameters.AddWithValue("@Address", Data.Address);
                                com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                                con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    Result = "Success!Inserted";
                                }
                                else
                                {
                                    Result = "Failed!Failed Add Teacher";
                                }
                                con.Close();
                            }
                            else
                            {
                                Result = "Failed!Student's Registration No alredy exists";
                            }
                        }
                    }
                }
                else
                {
                    Result = "Failed!Student's Registration No is not Valid";
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public List<ParentDetails> GetParentDetails(long SID)
        {
            List<ParentDetails> Result = new List<ParentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"
                                    select parent.Parents_Details.ID, isnull(parent.Parents_Details.Name,'')as Name,isnull(admin.users.Email,'')as Email,isnull(parent.Parents_Details.Address,'')as Address,isnull(parent.Parents_Details.Stu_Registration_NO,'')as Stu_Registration_NO, parent.Parents_Details.UID_FK,studentinfo.Student_Details.Stu_ID,isnull(studentinfo.Student_Details.Stu_Name,'')as Stu_Name
                                    from parent.Parents_Details 
									inner join admin.users on parent.Parents_Details.UID_FK=admin.users.UID
									inner join studentinfo.Student_Details on parent.Parents_Details.Stu_Registration_NO=studentinfo.Student_Details.Stu_RegistrationNo
	                                where parent.Parents_Details.SchoolID_FK=@SchoolID";
                    SqlCommand com = new SqlCommand(Query, con);

                    com.Parameters.AddWithValue("@SchoolID", SID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ParentDetails temp = new ParentDetails();

                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.Name = Convert.ToString(reader["Name"]);
                        temp.Email = Convert.ToString(reader["Email"]);
                        temp.Address = Convert.ToString(reader["Address"]);
                        temp.Stu_Registration_NO = Convert.ToString(reader["Stu_Registration_NO"]);
                        temp.UID_FK = Convert.ToInt64(reader["UID_FK"]);
                        temp.Stu_Name = Convert.ToString(reader["Stu_Name"]);
                        temp.Stu_ID = Convert.ToInt64(reader["Stu_ID"]);


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

        public EditParent EditParent(long UserID, long DetailsID)
        {
            EditParent Result = new EditParent();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select parent.Parents_Details.ID,isnull(parent.Parents_Details.Mobile_No, '') as Mobile_No, isnull(parent.Parents_Details.Stu_Registration_NO, '') as Stu_Registration_NO, parent.Parents_Details.UID_FK,
                                    isnull(parent.Parents_Details.Name, '') as Name,isnull(parent.Parents_Details.Address, '') as Address,isnull(parent.Parents_Details.Profile_Image, '') as Profile_Image, isnull(admin.users.Email, '') as Email,isnull(admin.users.password, '') as Password from parent.Parents_Details
                                              inner join admin.users on Parents_Details.UID_FK = admin.users.UID where Parents_Details.UID_FK = @UID and parent.Parents_Details.ID = @DID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UID", UserID);
                    cmd.Parameters.AddWithValue("@DID", DetailsID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {


                        Result.ID = Convert.ToInt32(reader["ID"]);
                        Result.Mobile_No = Convert.ToString(reader["MObile_No"]);

                        Result.Stu_Registration_NO = Convert.ToString(reader["Stu_Registration_NO"]);
                        Result.UID_FK = Convert.ToInt32(reader["UID_FK"]);

                        Result.Name = Convert.ToString(reader["Name"]);
                        Result.Address = Convert.ToString(reader["Address"]);
                        Result.Profile_Image = Convert.ToString(reader["Profile_Image"]);
                        Result.Email = Convert.ToString(reader["Email"]);
                        Result.Password = Convert.ToString(reader["Password"]);

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
       

        public string UpdateParent(EditParent Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = string.Empty;
                    if (Data.TempEmail == Data.Email)
                    {
                        if (Data.Profile_Image != null && Data.Profile_Image != "")
                        {
                            Query = @" Update [admin].[users] set Email=@Email,Password=@Password   
                                    where UID=@UID   
                                    update parent.Parents_Details set MObile_No=@Mobile,  
                                    Profile_Image=@ProfileImage,
                                    CreatedBy=@CreatedBy,Name=@Name,Address=@Address,SchoolID_FK=@SchoolID     
                                    where UID_FK=@UID";
                        }
                        else
                        {
                            Query = @"Update [admin].[users] set Email=@Email,Password=@Password  
                                     where UID=@UID   
                                     update parent.Parents_Details set MObile_No=@Mobile,  
                                     CreatedBy=@CreatedBy,Name=@Name,Address=@Address,SchoolID_FK=@SchoolID    
                                     where UID_FK=@UID";
                        }
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@UID", Data.UID_FK);
                        com.Parameters.AddWithValue("@Email", Data.Email);
                        com.Parameters.AddWithValue("@Password", Data.Password);
                        com.Parameters.AddWithValue("@Name", Data.Name);
                        com.Parameters.AddWithValue("@Address", Data.Address);
                        com.Parameters.AddWithValue("@Mobile", Data.Mobile_No);
                        com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                        com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                        if (Data.Profile_Image != null && Data.Profile_Image != "")
                        {
                            com.Parameters.AddWithValue("@ProfileImage", Data.Profile_Image);
                        }
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Successfully Updated";
                        }
                        else
                        {
                            Result = "Failed!Process Failed ";
                        }
                        con.Close();
                    }
                    else
                    {
                        if (!(CheckEMailExists(Data.Email)))
                        {
                            if (Data.Profile_Image != null && Data.Profile_Image != "")
                            {
                                Query = @" Update [admin].[users] set Email=@Email,Password=@Password   
                                    where UID=@UID   
                                    update parent.Parents_Details set MObile_No=@Mobile,  
                                    Profile_Image=@ProfileImage,
                                    CreatedBy=@CreatedBy,Name=@Name,Address=@Address,SchoolID_FK=@SchoolID     
                                    where UID_FK=@UID";
                            }
                            else
                            {
                                Query = @"Update [admin].[users] set Email=@Email,Password=@Password  
                                     where UID=@UID   
                                     update parent.Parents_Details set MObile_No=@Mobile,  
                                     CreatedBy=@CreatedBy,Name=@Name,Address=@Address,SchoolID_FK=@SchoolID    
                                     where UID_FK=@UID";
                            }
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@UID", Data.UID_FK);
                            com.Parameters.AddWithValue("@Email", Data.Email);
                            com.Parameters.AddWithValue("@Password", Data.Password);
                            com.Parameters.AddWithValue("@Name", Data.Name);
                            com.Parameters.AddWithValue("@Address", Data.Address);
                            com.Parameters.AddWithValue("@Mobile", Data.Mobile_No);
                            com.Parameters.AddWithValue("@CreatedBy", Data.CreatedBy);
                            com.Parameters.AddWithValue("@SchoolID", Data.SchoolID_FK);
                            if (Data.Profile_Image != null && Data.Profile_Image != "")
                            {
                                com.Parameters.AddWithValue("@ProfileImage", Data.Profile_Image);
                            }
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Successfully Updated";
                            }
                            else
                            {
                                Result = "Failed!Process Failed ";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "MailID already exits";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }






        public string DeleteParent(long UserID, long DetailsID)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    if ((Checkdetails(UserID)))
                    {
                        SqlCommand com = new SqlCommand("Deleteparent", con);
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@ID", DetailsID);
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Deleted Parent Details";
                        }
                        else
                        {
                            Result = "Failed!Deleted";
                        }
                        con.Close();

                    }
                    else
                    {
                        SqlCommand com = new SqlCommand("SP_DeleteParent", con);
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@UID", UserID);
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Deleted Parent Details";
                        }
                        else
                        {
                            Result = "Failed!Deleted";
                        }
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool Checkdetails(long UserID)
        {

            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "Select * from parent.Parents_Details where UID_FK=@ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return result;
        }

        public bool CheckStudentRegs(string Regs)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select * from parent.Parents_Details where Stu_Registration_NO=@Regs";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Regs", Regs);
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








        #endregion

        #region Ramashree 07/12/2018 parent view
        public ParentDetails ViewParentDetails(int ID)
        {
            ParentDetails Result = new ParentDetails();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select IsNull(par.Name,'') as Name,IsNull(par.Address,'') as Address,
                                    IsNull(par.MObile_No,'') as MObile_No,IsNull(stu.Stu_Name,'') as Stu_Name,
                                    IsNull(stu.Class_ID,'') as Class_ID,IsNull(cls.Class_Name,'') as Class_Name
                                    from parent.Parents_Details as par
                                    INNER JOIN studentinfo.Student_Details as stu ON par.Stu_Registration_NO=stu.Stu_RegistrationNo
                                    INNER JOIN admin.Class as cls ON stu.Class_ID=cls.ID 
                                    where par.UID_FK=@UID_FK";
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@UID_FK", ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        Result = JsonConvert.DeserializeObject<List<ParentDetails>>(str)[0];
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

        #region Assiogn Privilege Ramashree   
        public List<TeacherListing> GetTeacher(long SchoolID)
        {
            List<TeacherListing> Result = new List<TeacherListing>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select UID_FK,Name from teacher.TeachersDetails where SchoolID_FK=@SchoolID_FK";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SchoolID_FK", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherListing temp = new TeacherListing();
                        temp.Name = Convert.ToString(reader["Name"]);
                        temp.UserID = Convert.ToInt32(reader["UID_FK"]);
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

        public List<Depertment_Subject> GetDepertment(long SchoolID)
        {
            List<Depertment_Subject> Result = new List<Depertment_Subject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,DeptName from admin.Depertment_Subject
                                    where 
                                    SchoolID=@SchoolID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Depertment_Subject temp = new Depertment_Subject();
                        temp.Depertment = Convert.ToString(reader["DeptName"]);
                        temp.DepertmentID = Convert.ToInt32(reader["ID"]);
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

        public List<Syllabus_Subject> GetSubject(long SchoolID)
        {
            List<Syllabus_Subject> Result = new List<Syllabus_Subject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,Subject from admin.Syllabus_Subject
                                    where 
                                    School_ID=@School_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@School_ID", SchoolID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Syllabus_Subject temp = new Syllabus_Subject();
                        temp.Subject = Convert.ToString(reader["Subject"]);
                        temp.SubjectID = Convert.ToInt32(reader["ID"]);
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

        public List<Division> GetDivision(long SchoolID)
        {
            List<Division> Result = new List<Division>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"SELECT ID,Division_Name FROM admin.Division";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Division temp = new Division();
                        temp.ID = Convert.ToInt16(reader["ID"]);
                        temp.Division_Name = Convert.ToString(reader["Division_Name"]);
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

        public string InsertAssignHod(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                if (!(CheckTeacherExists(Data.TeacherID)))
                {
                    if (!(CheckDepartmentExists(Data.DepertmentID_Hod)))
                    {
                        using (SqlConnection con = new SqlConnection(conn))
                    {
                    string Query = @"insert into admin.AssignPrivilegeHod(TeacherID,TeacherTypeID,Dept_SubID,SchoolID,AssignedBy)
                                    values (@TeacherID,2,@Dept_SubID,@SchoolID,@AssignedBy)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TeacherID", Data.TeacherID);
                    com.Parameters.AddWithValue("@Dept_SubID", Data.DepertmentID_Hod);
                    com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                    com.Parameters.AddWithValue("@AssignedBy", Data.AssignedBy);
                            con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Assign Hod Inserted";
                    }
                    else
                    {
                        Result = "Failed| Failed Assign Hod Insertion";
                    }
                    con.Close();
                }
                    }
                    else
                    {
                        Result = "Failed!Department already exist";
                    }
                }
                    else
                {
                    Result = "Failed!Teacher Name already exist";
                }        
        }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckTeacherExists(long TeacherID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select ID,TeacherID from admin.AssignPrivilegeHod where TeacherID=@TeacherID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
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

        public bool CheckDepartmentExists(long DepertmentID_Hod)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select ID,Dept_SubID from admin.AssignPrivilegeHod where Dept_SubID=@Dept_SubID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dept_SubID", DepertmentID_Hod);
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

        public string InsertAssignClassTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
              if (!(CheckClassTeacherExists(Data.TeacherID)))
                {
                    if (!(CheckClassExists(Data.ClassID_ClassTeacher)))
                    {
                        using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Insert into admin.AssignPrivilegeClassTeacher (TeacherID,TeacherTypeID,DivisionID,LevelID,ClassID,SchoolID,AssignedBy)
                                    values (@TeacherID,3,@DivisionID,@LevelID,@ClassID,@SchoolID,@AssignedBy)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TeacherID", Data.TeacherID);
                    com.Parameters.AddWithValue("@DivisionID", Data.DivisionID_ClassTeacher);
                    com.Parameters.AddWithValue("@LevelID", Data.LevelID_ClassTeacher);
                    com.Parameters.AddWithValue("@ClassID", Data.ClassID_ClassTeacher);
                    com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                    com.Parameters.AddWithValue("@AssignedBy", Data.AssignedBy);
                            con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Assign Class Teacher Inserted";
                    }
                    else
                    {
                        Result = "Failed| Failed Assign Class Teacher Insertion";
                    }
                    con.Close();
                    }
                }
                    else
                    {
                        Result = "Failed!Class Name already exist";
                    }
                }
                else
                {
                    Result = "Failed!Teacher Name already exist";
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckClassTeacherExists(long TeacherID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select ID,TeacherID from admin.AssignPrivilegeClassTeacher where TeacherID=@TeacherID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
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

        public bool CheckClassExists(long ClassID_ClassTeacher)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select ID,ClassID from admin.AssignPrivilegeClassTeacher where ClassID=@ClassID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ClassID", ClassID_ClassTeacher);
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

        public long GetDeptIDforSubjectTeacher(long ID)
        {
            long result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString()))
                {
                    string query = @"select Dept_SubID from admin.Syllabus_Subject where ID=@ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", ID);
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

        public string InsertAssignSubjectTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                if (!(CheckSubjectTeacherExists(Data.TeacherID)))
                  {
                   using (SqlConnection con = new SqlConnection(conn))
                    {
                    string Query = @"Insert into admin.AssignPrivilegeSubjectTeacher(TeacherID,TeacherTypeID,SubjectID,DivisionID,
                                    LevelID,ClassID,SchoolID,AssignedBy,DeptID_FK)
                                    values(@TeacherID,6,@SubjectID,@DivisionID,@LevelID,@ClassID,@SchoolID,@AssignedBy,@DeptID_FK)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TeacherID", Data.TeacherID);
                    com.Parameters.AddWithValue("@SubjectID", Data.SubjectID_SubjectTeacher);
                    com.Parameters.AddWithValue("@DivisionID", Data.DivisionID_SubjectTeacher);
                    com.Parameters.AddWithValue("@LevelID", Data.LevelID_SubjectTeacher);
                    com.Parameters.AddWithValue("@ClassID", Data.ClassID_SubjectTeacher);
                    com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                    com.Parameters.AddWithValue("@AssignedBy", Data.AssignedBy);
                    com.Parameters.AddWithValue("@DeptID_FK", Data.DeptID_FK);
                        con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Assign Subject Teacher Inserted";
                    }
                    else
                    {
                        Result = "Failed| Failed Assign Subject Teacher Insertion";
                    }
                    con.Close();
                    }
                }
                else
                {
                    Result = "Failed!Teacher Name already exist";
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public bool CheckSubjectTeacherExists(long TeacherID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "select ID,TeacherID from admin.AssignPrivilegeSubjectTeacher where TeacherID=@TeacherID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
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
        #endregion

        #region Assign Privilege List Ramashree
        public List<TeacherListing> GetTeacherType()
        {
            List<TeacherListing> Result = new List<TeacherListing>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,LoginType from admin.Login_Type 
                                    where Catagory ='Teacher'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherListing temp = new TeacherListing();
                        temp.UserID = Convert.ToInt32(reader["ID"]);
                        temp.LoginType = Convert.ToString(reader["LoginType"]);                      
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

        public List<TeacherListing> GetTeacherTypeListHod(long TeacherTypeID)
        {
            List<TeacherListing> Result = new List<TeacherListing>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select UID_FK,Name,admin.AssignPrivilegeHod.Status,admin.Depertment_Subject.DeptName from admin.AssignPrivilegeHod
                                    inner join
                                    teacher.TeachersDetails
                                    on admin.AssignPrivilegeHod.TeacherID=teacher.TeachersDetails.UID_FK
                                    inner join
                                    admin.Depertment_Subject
                                    on admin.AssignPrivilegeHod.Dept_SubID=Depertment_Subject.ID
                                    where TeacherTypeID=2";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherListing temp = new TeacherListing();
                        temp.UserID = Convert.ToInt64(reader["UID_FK"]);
                        temp.Name_Hod = Convert.ToString(reader["Name"]);
                        temp.Depertment_Hod = Convert.ToString(reader["DeptName"]);
                        temp.Status = Convert.ToBoolean(reader["Status"]);
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

        public List<TeacherListing> GetTeacherTypeListClassTeacher(long TeacherTypeID)
        {
            List<TeacherListing> Result = new List<TeacherListing>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select UID_FK,Name,admin.AssignPrivilegeClassTeacher.Status,admin.Division.Division_Name,admin.Level.Level_Name,admin.Class.Class_Name
                                    from admin.AssignPrivilegeClassTeacher
                                    inner join
                                    teacher.TeachersDetails
                                    on admin.AssignPrivilegeClassTeacher.TeacherID=teacher.TeachersDetails.UID_FK
                                    inner join
                                    admin.Division
                                    on admin.AssignPrivilegeClassTeacher.DivisionID=admin.Division.ID
                                    inner join
                                    admin.Level
                                    on admin.AssignPrivilegeClassTeacher.LevelID=admin.Level.ID
                                    inner join 
                                    admin.Class
                                    on admin.AssignPrivilegeClassTeacher.ClassID=admin.Class.ID
                                    where TeacherTypeID=3";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherListing temp = new TeacherListing();
                        temp.UserID = Convert.ToInt32(reader["UID_FK"]);
                        temp.Name_ClassTeacher = Convert.ToString(reader["Name"]);
                        temp.Division_ClassTeacher = Convert.ToString(reader["Division_Name"]);
                        temp.Level_ClassTeacher = Convert.ToString(reader["Level_Name"]);
                        temp.Class_ClassTeacher = Convert.ToString(reader["Class_Name"]);
                        temp.Status = Convert.ToBoolean(reader["Status"]);
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

        public List<TeacherListing> GetTeacherTypeListSubjectTeacher(long TeacherTypeID)
        {
            List<TeacherListing> Result = new List<TeacherListing>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select UID_FK,Name,admin.AssignPrivilegeSubjectTeacher.Status,admin.Syllabus_Subject.Subject,admin.Division.Division_Name,admin.Level.Level_Name,admin.Class.Class_Name
                                    from admin.AssignPrivilegeSubjectTeacher
                                    inner join
                                    teacher.TeachersDetails
                                    on admin.AssignPrivilegeSubjectTeacher.TeacherID=teacher.TeachersDetails.UID_FK
                                    inner join
                                    admin.Syllabus_Subject
                                    on admin.AssignPrivilegeSubjectTeacher.SubjectID=admin.Syllabus_Subject.ID
                                    inner join
                                    admin.Division
                                    on admin.AssignPrivilegeSubjectTeacher.DivisionID=admin.Division.ID
                                    inner join
                                    admin.Level
                                    on admin.AssignPrivilegeSubjectTeacher.LevelID=admin.Level.ID
                                    inner join 
                                    admin.Class
                                    on admin.AssignPrivilegeSubjectTeacher.ClassID=admin.Class.ID
                                    where TeacherTypeID=6";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherListing temp = new TeacherListing();
                        temp.UserID = Convert.ToInt32(reader["UID_FK"]);
                        temp.Name_SubjectTeacher = Convert.ToString(reader["Name"]);
                        temp.Subject_SubjectTeacher = Convert.ToString(reader["Subject"]);
                        temp.Division_SubjectTeacher = Convert.ToString(reader["Division_Name"]);
                        temp.Level_SubjectTeacher = Convert.ToString(reader["Level_Name"]);
                        temp.Class_SubjectTeacher = Convert.ToString(reader["Class_Name"]);
                        temp.Status = Convert.ToBoolean(reader["Status"]);
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

        public List<TeacherListing> GetTeacherList()
        {
            List<TeacherListing> Result = new List<TeacherListing>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select UID_FK,Name from teacher.TeachersDetails";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeacherListing temp = new TeacherListing();
                        temp.Name = Convert.ToString(reader["Name"]);
                        temp.UserID = Convert.ToInt32(reader["UID_FK"]);
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

        public List<Depertment_Subject> GetDepertmentList()
        {
            List<Depertment_Subject> Result = new List<Depertment_Subject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,DeptName from admin.Depertment_Subject";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Depertment_Subject temp = new Depertment_Subject();
                        temp.Depertment = Convert.ToString(reader["DeptName"]);
                        temp.DepertmentID = Convert.ToInt32(reader["ID"]);
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
    
        public EditHod EditHod(long ID)
        {
            EditHod Result = new EditHod();
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    string Query = @"select TeacherID,Dept_SubID,teacher.TeachersDetails.Name
                                    from admin.AssignPrivilegeHod 
                                    inner join
                                    teacher.TeachersDetails on
                                    admin.AssignPrivilegeHod.TeacherID=teacher.TeachersDetails.UID_FK where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {                        
                        Result.TeacherID = Convert.ToInt64(reader["TeacherID"]);                       
                        Result.DepertmentID = Convert.ToInt64(reader["Dept_SubID"]);
                        Result.Teacher = Convert.ToString(reader["Name"]);
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

        public string UpdateHod(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    if (Data.TempDepertmentID_Hod == Data.DepertmentID_Hod)
                    {
                        string Query = @"update admin.AssignPrivilegeHod set Dept_SubID=@Dept_SubID
                                    where TeacherID=@ID";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ID", Data.ID);
                        com.Parameters.AddWithValue("@Dept_SubID", Data.DepertmentID_Hod);

                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success| Infomation Updated";
                        }
                        else
                        {
                            Result = "Failed| Failed to update";
                        }
                        con.Close();
                    }
                    else
                    {
                        if (!(CheckDepartmentExists(Data.DepertmentID_Hod)))
                             {
                            string Query = @"update admin.AssignPrivilegeHod set Dept_SubID=@Dept_SubID
                                    where TeacherID=@ID";
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@ID", Data.ID);
                            com.Parameters.AddWithValue("@Dept_SubID", Data.DepertmentID_Hod);
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success| Infomation Updated";
                            }
                            else
                            {
                                Result = "Failed| Failed to update";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "Failed| Department already exist";
                        }
                    }
                }
           
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string DeleteHod(long Id)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @" Delete from admin.AssignPrivilegeHod where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Id);
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
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string UpdateStatusAssignHod(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.AssignPrivilegeHod set Status=1 where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();
                }
            }
            catch
            {

            }
            return Result;
        }

        public string UpdateStatusUnAssignHod(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.AssignPrivilegeHod set Status=0 where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();
                }
            }
            catch
            {
                    
            }
            return Result;
        }
            
        public EditClassTeacher EditClassTeacher(long ID)
        {
            EditClassTeacher Result = new EditClassTeacher();
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    string Query = @"select TeacherID,DivisionID,LevelID,ClassID,teacher.TeachersDetails.Name
                                    from admin.AssignPrivilegeClassTeacher 
                                    inner join
                                    teacher.TeachersDetails on
                                    admin.AssignPrivilegeClassTeacher.TeacherID=teacher.TeachersDetails.UID_FK 
                                     where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.TeacherID = Convert.ToInt64(reader["TeacherID"]);
                        Result.DivisionID = Convert.ToInt32(reader["DivisionID"]);
                        Result.LevelID = Convert.ToInt32(reader["LevelID"]);
                        Result.ClassID = Convert.ToInt64(reader["ClassID"]);
                        Result.Teacher = Convert.ToString(reader["Name"]);
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

        public string UpdateClassTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    if (Data.TempClassID_ClassTeacher == Data.ClassID_ClassTeacher)
                    {
                        string Query = @"update admin.AssignPrivilegeClassTeacher set DivisionID=@DivisionID,
                                        LevelID=@LevelID,ClassID=@ClassID
                                        where TeacherID=@ID and SchoolID=@SchoolID";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ID", Data.ID);
                        com.Parameters.AddWithValue("@DivisionID", Data.DivisionID_ClassTeacher);
                        com.Parameters.AddWithValue("@LevelID", Data.LevelID_ClassTeacher);
                        com.Parameters.AddWithValue("@ClassID", Data.ClassID_ClassTeacher);
                        com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success| Infomation Updated";
                        }
                        else
                        {
                            Result = "Failed| Failed to update";
                        }
                        con.Close();
                    }
                    else
                    {
                        if (!(CheckClassExists(Data.ClassID_ClassTeacher)))
                        {
                            string Query = @"update admin.AssignPrivilegeClassTeacher set DivisionID=@DivisionID,
                                          LevelID=@LevelID,ClassID=@ClassID
                                          where TeacherID=@ID and SchoolID=@SchoolID";
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@ID", Data.ID);
                            com.Parameters.AddWithValue("@DivisionID", Data.DivisionID_ClassTeacher);
                            com.Parameters.AddWithValue("@LevelID", Data.LevelID_ClassTeacher);
                            com.Parameters.AddWithValue("@ClassID", Data.ClassID_ClassTeacher);
                            com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success| Infomation Updated";
                            }
                            else
                            {
                                Result = "Failed| Failed to update";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "Failed| Class already exist";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string DeleteClassTeacher(long Id)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @"Delete from admin.AssignPrivilegeClassTeacher where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Id);
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
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string UpdateStatusAssignClassTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.AssignPrivilegeClassTeacher set Status=1 where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();
                }
            }
            catch
            {

            }
            return Result;
        }

        public string UpdateStatusUnAssignClassTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.AssignPrivilegeClassTeacher set Status=0 where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();
                }
            }
            catch
            {

            }
            return Result;
        }

        public EditSubjectTeacher EditSubjectTeacher(long ID)
        {
            EditSubjectTeacher Result = new EditSubjectTeacher();
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    string Query = @"select TeacherID,SubjectID,DivisionID,levelID,ClassID,teacher.TeachersDetails.Name
                                    from admin.AssignPrivilegeSubjectTeacher 
                                    inner join
                                    teacher.TeachersDetails on
                                    admin.AssignPrivilegeSubjectTeacher.TeacherID =teacher.TeachersDetails.UID_FK 
                                    where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Result.TeacherID = Convert.ToInt64(reader["TeacherID"]);
                        Result.DivisionID = Convert.ToInt32(reader["DivisionID"]);
                        Result.LevelID = Convert.ToInt32(reader["LevelID"]);
                        Result.ClassID = Convert.ToInt64(reader["ClassID"]);
                        Result.SubjectID = Convert.ToInt64(reader["SubjectID"]);
                        Result.Teacher = Convert.ToString(reader["Name"]);
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

        public bool CheckingClassAndSubjectExists(long ClassID, long SubjectID)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(conn);
            try
            {
                string query = "Select * from admin.AssignPrivilegeSubjectTeacher where ClassID=@ClassID and SubjectID=@SubjectID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ClassID", ClassID);
                cmd.Parameters.AddWithValue("@SubjectID", SubjectID);
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

        public string UpdateSubjectTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                if (Data.ClassID_SubjectTeacher == Data.TempClassID_SubjectTeacher && Data.SubjectID_SubjectTeacher == Data.TempSubjectID_SubjectTeacher)
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        string Query = @"update admin.AssignPrivilegeSubjectTeacher set SubjectID=@SubID,DivisionID=@DivID,LevelID=@LevelID,ClassID=@ClassID,
                                        SchoolID=@SchoolID,AssignedBy=@AsID where TeacherID=@ID ";
                        SqlCommand com = new SqlCommand(Query, con);
                        com.Parameters.AddWithValue("@ID", Data.TeacherID);
                        com.Parameters.AddWithValue("@DivID", Data.DivisionID_SubjectTeacher);
                        com.Parameters.AddWithValue("@LevelID", Data.LevelID_SubjectTeacher);
                        com.Parameters.AddWithValue("@ClassID", Data.ClassID_SubjectTeacher);
                        com.Parameters.AddWithValue("@SubID", Data.SubjectID_SubjectTeacher);
                        com.Parameters.AddWithValue("@AsID", Data.AssignedBy);
                        com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);                     
                        con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Infomation Updated";
                        }
                        else
                        {
                            Result = "Failed!Update Failed";
                        }
                        con.Close();
                    }
                }
                if (Data.ClassID_SubjectTeacher != Data.TempClassID_SubjectTeacher && Data.SubjectID_SubjectTeacher != Data.TempSubjectID_SubjectTeacher)
                {
                    using (SqlConnection con = new SqlConnection(conn))

                    {
                        if (!CheckingClassAndSubjectExists(Data.ClassID_SubjectTeacher, Data.SubjectID_SubjectTeacher))
                        {
                            string Query = @"update admin.AssignPrivilegeSubjectTeacher set SubjectID=@SubID,DivisionID=@DivID,LevelID=@LevelID,ClassID=@ClassID,
                                            SchoolID=@SchoolID,AssignedBy=@AsID where TeacherID=@ID";
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@ID", Data.TeacherID);
                            com.Parameters.AddWithValue("@DivID", Data.DivisionID_SubjectTeacher);
                            com.Parameters.AddWithValue("@LevelID", Data.LevelID_SubjectTeacher);
                            com.Parameters.AddWithValue("@ClassID", Data.ClassID_SubjectTeacher);
                            com.Parameters.AddWithValue("@SubID", Data.SubjectID_SubjectTeacher);
                            com.Parameters.AddWithValue("@AsID", Data.AssignedBy);
                            com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Infomation Updated";
                            }
                            else
                            {
                                Result = "Failed!Update Failed";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "Failed!This Data already Assigned";
                        }
                    }
                }
                if (Data.ClassID_SubjectTeacher != Data.TempClassID_SubjectTeacher && Data.SubjectID_SubjectTeacher == Data.TempSubjectID_SubjectTeacher)
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        if (!CheckingClassAndSubjectExists(Data.ClassID_SubjectTeacher, Data.SubjectID_SubjectTeacher))
                        {
                            string Query = @"update admin.AssignPrivilegeSubjectTeacher set SubjectID=@SubID,DivisionID=@DivID,LevelID=@LevelID,ClassID=@ClassID,
                                            SchoolID=@SchoolID,AssignedBy=@AsID where TeacherID=@ID";
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@ID", Data.TeacherID);
                            com.Parameters.AddWithValue("@DivID", Data.DivisionID_SubjectTeacher);
                            com.Parameters.AddWithValue("@LevelID", Data.LevelID_SubjectTeacher);
                            com.Parameters.AddWithValue("@ClassID", Data.ClassID_SubjectTeacher);
                            com.Parameters.AddWithValue("@SubID", Data.SubjectID_SubjectTeacher);
                            com.Parameters.AddWithValue("@AsID", Data.AssignedBy);
                            com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Infomation Updated";
                            }
                            else
                            {
                                Result = "Failed!Update Failed";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "Failed!Class already Assigned";
                        }
                    }
                }
                if (Data.ClassID_SubjectTeacher == Data.TempClassID_SubjectTeacher && Data.SubjectID_SubjectTeacher != Data.TempSubjectID_SubjectTeacher)
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        if (!CheckingClassAndSubjectExists(Data.ClassID_SubjectTeacher, Data.SubjectID_SubjectTeacher))
                        {
                            string Query = @"update admin.AssignPrivilegeSubjectTeacher set SubjectID=@SubID,DivisionID=@DivID,LevelID=@LevelID,ClassID=@ClassID,
                                            SchoolID=@SchoolID,AssignedBy=@AsID where TeacherID=@ID";
                            SqlCommand com = new SqlCommand(Query, con);
                            com.Parameters.AddWithValue("@ID", Data.TeacherID);
                            com.Parameters.AddWithValue("@DivID", Data.DivisionID_SubjectTeacher);
                            com.Parameters.AddWithValue("@LevelID", Data.LevelID_SubjectTeacher);
                            com.Parameters.AddWithValue("@ClassID", Data.ClassID_SubjectTeacher);
                            com.Parameters.AddWithValue("@SubID", Data.SubjectID_SubjectTeacher);
                            com.Parameters.AddWithValue("@AsID", Data.AssignedBy);
                            com.Parameters.AddWithValue("@SchoolID", Data.SchoolID);
                            con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                Result = "Success!Infomation Updated";
                            }
                            else
                            {
                                Result = "Failed!Update Failed";
                            }
                            con.Close();
                        }
                        else
                        {
                            Result = "Failed!Subject already Assigned";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string DeleteSubjectTeacher(long Id)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))

                {
                    string Query = @"Delete from admin.AssignPrivilegeSubjectTeacher where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Id);
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
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return Result;
        }

        public string UpdateStatusAssignSubjectTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.AssignPrivilegeSubjectTeacher set Status=1 where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();
                }
            }
            catch
            {

            }
            return Result;
        }

        public string UpdateStatusUnAssignSubjectTeacher(AssignPrivilege Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"update admin.AssignPrivilegeSubjectTeacher set Status=0 where TeacherID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                    }
                    con.Close();
                }
            }
            catch
            {

            }
            return Result;
        }
        #endregion

        #region Subject Tab Ramashree
        public List<Depertment_Subject> GetAllDepertment()
        {
            List<Depertment_Subject> Result = new List<Depertment_Subject>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"select ID,DeptName from admin.Depertment_Subject";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Depertment_Subject temp = new Depertment_Subject();
                        temp.DepertmentID = Convert.ToInt32(reader["ID"]);
                        temp.Depertment = Convert.ToString(reader["DeptName"]);
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

        public string InsertSubject(AddsubjectInfo Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Insert into admin.Syllabus_Subject(Subject,Level_ID,CreatedBy_ID,School_ID,Dept_SubID)
                                    values(@Subject,@Level_ID,@CreatedBy_ID,@School_ID,@Dept_SubID)";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Subject", Data.Subject);
                    com.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                    com.Parameters.AddWithValue("@CreatedBy_ID", Data.CreatedBy_ID);
                    com.Parameters.AddWithValue("@School_ID", Data.School_ID);
                    com.Parameters.AddWithValue("@Dept_SubID", Data.DepartmentID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Subject Inserted";
                    }
                    else
                    {
                        Result = "Failed| Failed subject Insertion";
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

        public List<SubjectDetails> SubjectList(string schoolId)
        {
            List<SubjectDetails> obj = new List<SubjectDetails>();
            DataTable dt = new DataTable();
            SqlConnection connt = new SqlConnection(conn);
            try
            {
                string query = @"select admin.Syllabus_Subject.ID as ID,admin.Syllabus_Subject.Subject,admin.Level.ID as Level_ID ,
                                admin.Level.Level_Name,admin.Depertment_Subject.ID as Department_ID,admin.Depertment_Subject.DeptName as Department_Name
                                from  admin.Syllabus_Subject 
                                inner join admin.Level 
                                ON admin.Syllabus_Subject.Level_ID=admin.Level.ID
                                inner join admin.Depertment_Subject 
                                ON admin.Syllabus_Subject.Dept_SubID=admin.Depertment_Subject.ID
                                where admin.Depertment_Subject.SchoolID=@SchoolID";
                SqlCommand cmd = new SqlCommand(query, connt);
                cmd.Parameters.AddWithValue("@SchoolID", Convert.ToInt64(schoolId));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                {
                    string str = JsonConvert.SerializeObject(dt);
                    obj = JsonConvert.DeserializeObject<List<SubjectDetails>>(str);
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public SubjectDetails ViewSubjectDetails(int ID)
        {
            SubjectDetails Result = new SubjectDetails();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select admin.Syllabus_Subject.ID as ID,admin.Syllabus_Subject.Subject,admin.Level.ID as Level_ID ,
                                    admin.Level.Level_Name,admin.Depertment_Subject.ID as Department_ID,admin.Depertment_Subject.DeptName as Department_Name
                                    from  admin.Syllabus_Subject 
                                    inner join admin.Level 
                                    ON admin.Syllabus_Subject.Level_ID=admin.Level.ID
                                    inner join admin.Depertment_Subject 
                                    ON admin.Syllabus_Subject.Dept_SubID=admin.Depertment_Subject.ID
                                    where  admin.Syllabus_Subject.ID=@ID";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        Result = JsonConvert.DeserializeObject<List<SubjectDetails>>(str)[0];
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


        public SubjectDetails SubjectEdit(int ID)
        {
            SubjectDetails Result = new SubjectDetails();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select ID,IsNull(Subject,'') as Subject,ISnull(Level_ID,'') as Level_ID,
                                    Isnull(CreatedBy_ID,'') as CreatedBy_ID, IsNull(School_ID,'') School_ID,
                                    Isnull (Dept_SubID,'') as Department_ID
                                    from admin.Syllabus_Subject
                                    where ID=@ID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        Result = JsonConvert.DeserializeObject<List<SubjectDetails>>(str)[0];
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
        public string UpdateSubInfo(AddsubjectInfo Data)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"Update admin.Syllabus_Subject set Subject=@Subject,Level_ID=@Level_ID,Dept_SubID=@Dept_SubID where ID=@ID";
                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@Subject", Data.Subject);
                    com.Parameters.AddWithValue("@Level_ID", Data.Level_ID);
                    com.Parameters.AddWithValue("@Dept_SubID", Data.DepartmentID);
                    com.Parameters.AddWithValue("@ID", Data.ID);
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Result = "Success| Subject Info Updated ";
                    }
                    else
                    {
                        Result = "Failed| Failed to update Subject";
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

        #region  Feedback Ramashree
        public List<TeachersSyllabusTopicDetails> GetTopic(int ID)
        {
            List<TeachersSyllabusTopicDetails> Result = new List<TeachersSyllabusTopicDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"SELECT Topic_ID,Topic_Name,Subject_ID_FK FROM admin.Syllabus_Topic where Subject_ID_FK=@Subject_ID_FK";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Subject_ID_FK", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TeachersSyllabusTopicDetails temp = new TeachersSyllabusTopicDetails();
                        temp.ID = Convert.ToInt32(reader["Topic_ID"]);
                        temp.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                        temp.Subject_ID_FK = Convert.ToInt32(reader["Subject_ID_FK"]);
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

        public List<ClassTeacherAssignmentDetails> GetAssignment(int ID)
        {
            List<ClassTeacherAssignmentDetails> Result = new List<ClassTeacherAssignmentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {

                    string query = @"SELECT ID,AssignmentName,SubjectID_FK,TopicID_FK FROM teacher.AssignmentDetails 
                                    where TopicID_Fk=@TopicID_FK";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@TopicID_Fk", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherAssignmentDetails temp = new ClassTeacherAssignmentDetails();
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.AssignmentName = Convert.ToString(reader["AssignmentName"]);
                        temp.SubjectID_FK = Convert.ToInt32(reader["SubjectID_FK"]);
                        temp.TopicID_FK = Convert.ToInt32(reader["TopicID_FK"]);                      
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

        public List<ClassTeacherAssignmentDetails> GetFeedbackListAssignment(long AssignmentID)
        {
            List<ClassTeacherAssignmentDetails> Result = new List<ClassTeacherAssignmentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select  studentinfo.Student_Details.Stu_Name,teacher.TeachersAssignmentFeedback.FeedbackComment,                                   
                                    teacher.TeachersAssignmentFeedback.Obtained_Marks, teacher.TeachersAssignmentFeedback.Rating
                                    from teacher.TeachersAssignmentFeedback
                                    inner join
                                    studentinfo.Student_Details
                                    on teacher.TeachersAssignmentFeedback.StudentID_FK =  studentinfo.Student_Details.Stu_ID                                   
                                    where teacher.TeachersAssignmentFeedback.AssignmentID_FK=@AssignmentID_FK";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AssignmentID_FK", AssignmentID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherAssignmentDetails temp = new ClassTeacherAssignmentDetails();
                        temp.StudentName = Convert.ToString(reader["Stu_Name"]);
                        temp.FeedbackComment = Convert.ToString(reader["FeedbackComment"]);
                        temp.Obtained_Marks = Convert.ToString(reader["Obtained_Marks"]);
                        temp.Rating = Convert.ToString(reader["Rating"]);
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

        public List<ClassTeacherAssignmentDetails> GetFeedbackListAttendence(long SubjectID,long ClassID)
        {
            List<ClassTeacherAssignmentDetails> Result = new List<ClassTeacherAssignmentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select studentinfo.Student_Details.Stu_Name,admin.Class.Class_Name,
                                    admin.Syllabus_Subject.Subject,admin.Attendence.Attendence
                                    from admin.Attendence
                                    inner join
                                    studentinfo.Student_Details
                                    on admin.Attendence.StudentID = studentinfo.Student_Details.Stu_ID
                                    inner join
                                    admin.Class
                                    on admin.Attendence.ClassID = admin.Class.ID
                                    inner join 
                                    admin.Syllabus_Subject
                                    on admin.Attendence.SubjectID = admin.Syllabus_Subject.ID
                                    where admin.Attendence.SubjectID =@SubjectID and admin.Attendence.ClassID =@ClassID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SubjectID", SubjectID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherAssignmentDetails temp = new ClassTeacherAssignmentDetails();
                        temp.StudentName = Convert.ToString(reader["Stu_Name"]);
                        temp.ClassName = Convert.ToString(reader["Class_Name"]);
                        temp.SubjectName = Convert.ToString(reader["Subject"]);
                        temp.Attendence = Convert.ToString(reader["Attendence"]);
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

        public List<Semester> GetSemester(long SchoolID)
        {
            List<Semester> Result = new List<Semester>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"SELECT ID,Semester FROM admin.Semester";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
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

        public List<ClassTeacherAssignmentDetails> GetFeedbackListExam(long SubjectID, long ClassID,long SemesterID)
        {
            List<ClassTeacherAssignmentDetails> Result = new List<ClassTeacherAssignmentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select studentinfo.Student_Details.Stu_Name,admin.Class.Class_Name,
                                    admin.Syllabus_Subject.Subject,admin.Semester.Semester,admin.Exam.Marks
                                    from admin.Exam
                                    inner join
                                    studentinfo.Student_Details
                                    on admin.Exam.StudentID = studentinfo.Student_Details.Stu_ID
                                    inner join
                                    admin.Class
                                    on admin.Exam.ClassID = admin.Class.ID
                                    inner join 
                                    admin.Syllabus_Subject
                                    on admin.Exam.SubjectID = admin.Syllabus_Subject.ID
                                    inner join 
                                    admin.Semester
                                    on admin.Exam.SemesterID = admin.Semester.ID
                                    where admin.Exam.SubjectID =@SubjectID and admin.Exam.ClassID =@ClassID
                                    and admin.Exam.SemesterID = @SemesterID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SubjectID", SubjectID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    cmd.Parameters.AddWithValue("@SemesterID", SemesterID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherAssignmentDetails temp = new ClassTeacherAssignmentDetails();
                        temp.StudentName = Convert.ToString(reader["Stu_Name"]);
                        temp.ClassName = Convert.ToString(reader["Class_Name"]);
                        temp.SubjectName = Convert.ToString(reader["Subject"]);
                        temp.Semester = Convert.ToString(reader["Semester"]);
                        temp.Marks = Convert.ToString(reader["Marks"]);
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








        public List<ClassTeacherAssignmentDetails> GetFeedbackListStudent(long SubjectID)
        {
            List<ClassTeacherAssignmentDetails> Result = new List<ClassTeacherAssignmentDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"select teacher.TeachersDetails.Name,admin.Syllabus_Topic.Topic_Name,
                                    studentinfo.StudentRateLesson.RatingStar
                                    from studentinfo.StudentRateLesson
                                    inner join
                                    teacher.TeachersDetails
                                    on studentinfo.StudentRateLesson.TeacherID = teacher.TeachersDetails.UID_FK
                                    inner join
                                    admin.Syllabus_Topic
                                    on studentinfo.StudentRateLesson.TopicID = admin.Syllabus_Topic.Topic_ID
                                    where studentinfo.StudentRateLesson.SubjectID=@SubjectID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SubjectID", SubjectID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherAssignmentDetails temp = new ClassTeacherAssignmentDetails();
                        temp.TeacherName = Convert.ToString(reader["Name"]);
                        temp.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                        temp.Rating = Convert.ToString(reader["RatingStar"]);
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
