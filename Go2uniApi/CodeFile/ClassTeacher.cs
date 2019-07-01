using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data;

namespace Go2uniApi.CodeFile
{
    public class ClassTeacher
    {

        private string conn => System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        #region Edit Profile
        public EditProfileTeacher EditProfile(long TID)
        {
            EditProfileTeacher result = new EditProfileTeacher();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    string Query = @"select td.ID,
                                    ISNULL(td.Name,'') Name,
                                    au.Email,
                                    au.Password,
                                    CONVERT(varchar,Joining_Date,121) Joining_Date,
                                    ISNULL(MObile_No,'') Mobile_NO,
                                    ISNULL(Profile_Image,'') Profile_Image
                                    from teacher.TeachersDetails td left join
                                    admin.users au on au.UID=td.UID_FK
                                    where td.UID_FK=@TeacherID";

                    SqlCommand com = new SqlCommand(Query, con);
                    com.Parameters.AddWithValue("@TeacherID", TID);
                    if (con.State == System.Data.ConnectionState.Broken ||
                            con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        result.ID = Convert.ToInt64(reader["ID"]);
                        result.Teacher_Name = Convert.ToString(reader["Name"]);
                        result.Email = Convert.ToString(reader["Email"]);
                        result.Password = Convert.ToString(reader["Password"]);
                        result.TempJoiningDate = Convert.ToString(reader["Joining_Date"]);
                        result.MobileNo = Convert.ToString(reader["Mobile_NO"]);
                        result.ProfileImage = Convert.ToString(reader["Profile_Image"]);
                    }
                    Global global = new Global();
                    var res = global.ObjectNullChecking(result);
                    result = JsonConvert.DeserializeObject<EditProfileTeacher>(res);
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteErrorLog("Error.txt", ex.Message);
            }
            return result;
        }
        public Boolean checkForExistingmail(EditProfileTeacher prfle)
        {
            bool result = true;
            using (SqlConnection con = new SqlConnection(conn))
            {
                if (con.State == ConnectionState.Broken ||
                            con.State == ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[teacher].[checkForExistingEmail]",
                        Connection = con
                    };
                    command.Parameters.AddWithValue("@Id", prfle.ID);
                    command.Parameters.AddWithValue("@Email", prfle.Email);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToBoolean(reader["Count"]);
                    }
                }
            }
            return result;
        }
        public string UpdateTeacherProfile(EditProfileTeacher Data)
        {
            string Result = string.Empty;
                using (SqlConnection con = new SqlConnection(conn))
                {
                    try {
                        string Query = string.Empty;
                        if (Data.ProfileImage != null && Data.ProfileImage != "")
                        {
                            Query = @" update teacher.TeachersDetails set  
                                     teacher.TeachersDetails.Name=@TeacherName ,
                                     teacher.TeachersDetails.Joining_Date=@JoiningDate, 
                                     teacher.TeachersDetails.MObile_No=@MobileNo, 
                                     teacher.TeachersDetails.Profile_Image=@ProfileImage  
                                     where UID_FK=@TeacherID 

                                     update admin.users set
                                     admin.users.Email=@Email,
                                     admin.users.Password=@Password
                                     where admin.users.UID=@TeacherID";

                        }
                        else
                        {
                            Query = @" update teacher.TeachersDetails set  
                                     teacher.TeachersDetails.Name=@TeacherName ,
                                     teacher.TeachersDetails.Joining_Date=@JoiningDate, 
                                     teacher.TeachersDetails.MObile_No=@MobileNo
                                     where UID_FK=@TeacherID 

                                     update admin.users set
                                     admin.users.Email=@Email,
                                     admin.users.Password=@Password
                                     where admin.users.UID=@TeacherID";

                        }
                        SqlCommand com = new SqlCommand(Query, con);
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
                        if (con.State == System.Data.ConnectionState.Broken ||
                                con.State == System.Data.ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        if (com.ExecuteNonQuery() > 0)
                        {
                            Result = "Success!Successfully Updated.";
                        }
                        else
                        {
                            Result = "Failed!Process Failed.";
                        }
                    } catch (Exception ex) { }
                    
                }
            return Result;
        }
        #endregion
        #region Messaging
        public List<ClassTeacherListingDetails> getActiveTeacherList(long SchoolId, long Teacher_UID)
        {
            List<ClassTeacherListingDetails> list = new List<ClassTeacherListingDetails>();
            string Query = @"select ss.ID,ss.Name,
                            ss.UID_FK,ss.ProfileImage,
                            ss.STATUS,ss.LastLogin,
                            ss.IsOnline from(
                            select distinct teacher.TeachersDetails.ID,
                            teacher.TeachersDetails.Name,
                            teacher.TeachersDetails.UID_FK,
                            ISNULL(teacher.TeachersDetails.Profile_Image,'') as ProfileImage,
                            teacher.TeachersDetails.STATUS,
                            CONVERT(varchar,admin.users.LastLogin,121) LastLogin,
                            admin.users.IsOnline,
                            MAX(teacher.LiveMessageInfo.CreatedDate) CreatedDate

                            from teacher.TeachersDetails left join
                            admin.users on
                            teacher.TeachersDetails.UID_FK=admin.users.UID

                            left join teacher.LiveMessageInfo on 
                            teacher.LiveMessageInfo.SenderID=admin.users.UID and
                            teacher.LiveMessageInfo.ReceiverID<>admin.users.UID
                            or
                            teacher.LiveMessageInfo.ReceiverID=admin.users.UID and
                            teacher.LiveMessageInfo.SenderID<>admin.users.UID

                            where teacher.TeachersDetails.SchoolID_FK=@SchoolId
                            and admin.users.UID<>@Teacher_UID
                            and teacher.TeachersDetails.STATUS=1

                            group by teacher.TeachersDetails.ID,
                            Teacher.TeachersDetails.Name,
                            teacher.TeachersDetails.UID_FK,
                            teacher.TeachersDetails.Profile_Image,
                            teacher.TeachersDetails.STATUS,
                            admin.users.LastLogin,
                            admin.users.IsOnline
                            ) as ss
                            order by ss.CreatedDate desc";

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {

                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@SchoolId", SchoolId);
                    command.Parameters.AddWithValue("@Teacher_UID", Teacher_UID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherListingDetails temp = new ClassTeacherListingDetails();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.UserId = Convert.ToInt64(reader["UID_FK"]);
                        temp.Teacher_Name = Convert.ToString(reader["Name"]);
                        temp.ProfileImage = Convert.ToString(reader["ProfileImage"]);
                        temp.STATUS = Convert.ToBoolean(reader["STATUS"]);
                        temp.LastLoginTime = Convert.ToString(reader["LastLogin"]);
                        temp.IsOnline = Convert.ToBoolean(reader["IsOnline"]);
                        list.Add(temp);
                    }
                }
                catch (Exception ex) { }

            }

            return list;
        }
        public ClassTeacherMessageProp getClassTeacherMsg(long SenderID, long ReceiverID, long SchoolID)
        {
            ClassTeacherMessageProp prop = new ClassTeacherMessageProp();
            string Query = @"select teacher.TeachersDetails.UID_FK,
                            teacher.TeachersDetails.Name
                            from teacher.TeachersDetails
                            where teacher.TeachersDetails.UID_FK=@ReceiverId
                            and teacher.TeachersDetails.SchoolID_FK=@SchoolId";

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@ReceiverId", ReceiverID);
                    command.Parameters.AddWithValue("@SchoolId", SchoolID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        prop.UserId = Convert.ToInt64(reader["UID_FK"]);
                        prop.Teacher_Name = Convert.ToString(reader["Name"]);
                        prop.messages = getTeacherMessages(SenderID, ReceiverID, SchoolID);
                    }
                }
                catch (Exception ex) { }

            }


            return prop;
        }
        private List<ClassTeacherMessages> getTeacherMessages(long SenderID, long ReceiverID, long SchoolID)
        {
            List<ClassTeacherMessages> list = new List<ClassTeacherMessages>();
            string Query = @"select teacher.LiveMessageInfo.ID,
                                teacher.LiveMessageInfo.SenderID,
                                teacher.LiveMessageInfo.ReceiverID,
                                ISNULL(teacher.LiveMessageInfo.Message,'') Message,
                                ISNULL(teacher.TeachersDetails.Profile_Image,'') Profile_Image,
                                CONVERT(varchar,teacher.LiveMessageInfo.CreatedDate,121) CreatedDate,
                                teacher.LiveMessageInfo.IsDelete,
                                teacher.LiveMessageInfo.IsRead,
                                teacher.LiveMessageInfo.SchoolID
                                from teacher.LiveMessageInfo
                                left join teacher.TeachersDetails
                                on teacher.LiveMessageInfo.SenderID=teacher.TeachersDetails.UID_FK

                                where teacher.LiveMessageInfo.SenderID=@SenderId and
                                teacher.LiveMessageInfo.ReceiverID=@ReceiverId 
                                or
                                teacher.LiveMessageInfo.SenderID=@ReceiverId and
                                teacher.LiveMessageInfo.ReceiverID=@SenderId 
                                
                                and
                                teacher.LiveMessageInfo.SchoolID=@SchoolId and
                                teacher.LiveMessageInfo.IsDelete=0
                                order by teacher.LiveMessageInfo.CreatedDate desc";

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@SenderId", SenderID);
                    command.Parameters.AddWithValue("@ReceiverId", ReceiverID);
                    command.Parameters.AddWithValue("@SchoolId", SchoolID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassTeacherMessages temp = new ClassTeacherMessages();
                        temp.ID = Convert.ToInt64(reader["ID"]);
                        temp.SenderID = Convert.ToInt64(reader["SenderID"]);
                        temp.ReceiverID = Convert.ToInt64(reader["ReceiverID"]);
                        temp.Message = Convert.ToString(reader["Message"]);
                        temp.Profile_Image = Convert.ToString(reader["Profile_Image"]);
                        temp.tmpDate = Convert.ToString(reader["CreatedDate"]);
                        temp.IsRead = Convert.ToBoolean(reader["IsRead"]);
                        temp.IsDelete = Convert.ToBoolean(reader["IsDelete"]);
                        temp.SchoolID = Convert.ToInt64(reader["SchoolID"]);
                        list.Add(temp);
                    }
                }
                catch (Exception ex) { }
            }

            return list;
        }
        public string InsertClassteacherMessagesByUID(ClassTeacherMessages messageProp)
        {
            string Result = string.Empty;
            string Query = @"insert into teacher.LiveMessageInfo(SenderID,ReceiverID,SchoolID,Message)
                            values(@SenderId,@ReceiverId,@SchoolId,@Message)";

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {

                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@SenderId", messageProp.SenderID);
                    command.Parameters.AddWithValue("@ReceiverId", messageProp.ReceiverID);
                    command.Parameters.AddWithValue("@SchoolId", messageProp.SchoolID);
                    command.Parameters.AddWithValue("@Message", messageProp.Message);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        Result = "Success|Message successfully inserted.";
                    }
                    else
                    {
                        Result = "Failed|Something went wrong.";
                    }
                }
                catch (Exception ex) { }

            }

            return Result;
        }
        public long getMessageCount(long SenderId, long ReceiverId, long SchoolId)
        {

            long Count = 0;
            string Query = @"select count(*) Count from teacher.LiveMessageInfo
                            where teacher.LiveMessageInfo.SenderID=@SenderId
                            and teacher.LiveMessageInfo.ReceiverID=@ReceiverId
                            or teacher.LiveMessageInfo.SenderID=@ReceiverId and
                            teacher.LiveMessageInfo.ReceiverID=@SenderId
                            and teacher.LiveMessageInfo.SchoolID=@SchoolId";

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@SenderId", SenderId);
                    command.Parameters.AddWithValue("@ReceiverId", ReceiverId);
                    command.Parameters.AddWithValue("@SchoolId", SchoolId);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Count = Convert.ToInt64(reader["Count"]);
                    }
                }
                catch (Exception ex) { }

            }

            return Count;
        }
        #endregion
        #region Time Table

        public long getSubjectTeacherListCount(long UserId, long SchoolId)
        {
            long Count = 0;
            string Query = @"select count(*) Count from
                                teacher.LiveMessageInfo where 
                                teacher.LiveMessageInfo.SchoolID=@SchoolId and
                                teacher.LiveMessageInfo.SenderID=@UserId
                                or teacher.LiveMessageInfo.ReceiverID=@UserId";
            using (SqlConnection con = new SqlConnection(conn))
            {
                try {
                    if (con.State == System.Data.ConnectionState.Broken ||
                        con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand(Query, con);
                    command.Parameters.AddWithValue("@SchoolId", SchoolId);
                    command.Parameters.AddWithValue("@UserId", UserId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Count = Convert.ToInt64(reader["Count"]);
                    }
                } catch (Exception ex) { }
                
            }
            return Count;
        }
        public List<ClassTeacherTimetable> getClassTeacherTimeTable(timetable_prop prop)
        {
            DataTable dt = new DataTable();
            List<ClassTeacherTimetable> result=new List<ClassTeacherTimetable>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try {
                    if (con.State == ConnectionState.Broken ||
                        con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand()
                    {
                        CommandText = "[teacher].[Teacher_Timetable]",
                        CommandType = CommandType.StoredProcedure,
                        Connection = con
                    };
                    command.Parameters.AddWithValue("@TeacherID", prop.TeacherId);
                    command.Parameters.AddWithValue("@SchoolID", prop.SchoolId);
                    command.Parameters.AddWithValue("@MonthID", prop.MonthId);
                    command.Parameters.AddWithValue("@ClassId", prop.ClassId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                    result = getListModel<ClassTeacherTimetable>(dt);
                }catch(Exception ex){ }
                
            }
            return result;
        }
        private List<T> getListModel<T>(DataTable dt)
        {
            List<T> Result = new List<T>();
            try {
                var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
                {
                    PropertyInfo = propertyInfo,
                    Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
                }).ToList();

                foreach (var row in dt.Rows.Cast<DataRow>())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var typeProperty in typeProperties)
                    {
                        object value = row[typeProperty.PropertyInfo.Name];
                        object safeValue = value == null || DBNull.Value.Equals(value)
                            ? null
                            : Convert.ChangeType(value, typeProperty.Type);

                        typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                    }
                    Result.Add(obj);
                }
            } catch (Exception ex) { }
            
            return Result;
        }
        public List<ClassTeacherListingDetails> GetAllAssignedClassTeachers(timetable_prop prop){
            List<ClassTeacherListingDetails> clsteacherlist = new List<ClassTeacherListingDetails>();
            string Query = @"select td.UID_FK,isnull(td.Name,'') TeacherName,isnull(td.Profile_Image,'') Profile_Image 
                                from teacher.TeachersDetails td inner join teacher.TimeTable tt on td.UID_FK=tt.TeacherID 
                                    where td.UID_FK<>@TeacherId and status=1 group by td.UID_FK,td.Name,td.Profile_Image";
            using(SqlConnection connection=new SqlConnection(conn)){

                try {
                    if (connection.State == ConnectionState.Broken ||
                        connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand()
                    {
                        CommandText = Query,
                        Connection = connection
                    };
                    command.Parameters.AddWithValue("@TeacherId", prop.TeacherId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        ClassTeacherListingDetails temp;
                        while (reader.Read())
                        {
                            temp = new ClassTeacherListingDetails()
                            {
                                UserId = Convert.ToInt64(reader["UID_FK"]),
                                Teacher_Name = Convert.ToString(reader["TeacherName"]),
                                ProfileImage = Convert.ToString(reader["Profile_Image"])
                            }; clsteacherlist.Add(temp);
                        }
                    }
                } catch (Exception ex) { }
                
            }return clsteacherlist;
        }


        #endregion
        #region Get Division,Level & Class

        public AddStudentDetails getDivisionDetails()
        {
            AddStudentDetails res = new AddStudentDetails();
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                    SqlCommand Command = new SqlCommand()
                    {
                        Connection = Connection,
                        CommandText = @"select admin.Division.ID,admin.Division.Division_Name,
                                    admin.Division.IsActive from admin.Division
                                        where admin.Division.IsActive = 1"
                    };
                    SqlDataReader reader = Command.ExecuteReader();
                    res.Division = new List<Division>();
                    while (reader.Read())
                    {
                        Division temp = new Division()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Division_Name = Convert.ToString(reader["Division_Name"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        }; res.Division.Add(temp);
                    }
                }
            }
            return res;
        }
        public AddStudentDetails getLevelDetails(int Div)
        {
            AddStudentDetails res = new AddStudentDetails();
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                SqlCommand Command = new SqlCommand()
                {
                    Connection = Connection,
                    CommandText = @"select admin.Level.ID,admin.Level.Level_Name,
                                    admin.Level.Division_ID from admin.Level where 
                                        admin.Level.Division_ID=@Div and admin.Level.Status=1"
                };
                Command.Parameters.AddWithValue("@Div", Div);
                if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                    SqlDataReader reader = Command.ExecuteReader();
                    res.Level = new List<Level>();
                    while (reader.Read())
                    {
                        Level temp = new Level()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Level_Name = Convert.ToString(reader["Level_Name"]),
                            Division_ID = Convert.ToInt32(reader["Division_ID"])
                        }; res.Level.Add(temp);
                    }
                }
            }
            return res;
        }
        public AddStudentDetails getClassDetails(long SchoolId, int DivId, int LvlId)
        {
            AddStudentDetails res = new AddStudentDetails();
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                SqlCommand Command = new SqlCommand()
                {
                    Connection = Connection,
                    CommandText = @"select admin.Class.ID,admin.Class.Class_Name,
		                            admin.Class.Div_ID,admin.Class.Level_ID,admin.CLass.CreatedBy_ID
    		                            from admin.Class where admin.Class.School_ID=@SchoolId
                                            and admin.Class.Div_ID=@DivId and 
                                                admin.Class.Level_ID=@LvlId"
                };
                Command.Parameters.AddWithValue("@SchoolId", SchoolId);
                Command.Parameters.AddWithValue("@DivId", DivId);
                Command.Parameters.AddWithValue("@LvlId", LvlId);
                if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                    res.ClassDetails = new List<ClassDetails>();
                    SqlDataReader reader = Command.ExecuteReader();

                    while (reader.Read())
                    {
                        ClassDetails temp = new ClassDetails()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Class_Name = Convert.ToString(reader["Class_Name"]),
                            Div_ID = Convert.ToInt32(reader["Div_ID"]),
                            Level_ID = Convert.ToInt32(reader["Level_ID"])
                        }; res.ClassDetails.Add(temp);
                    }
                }
            }
            return res;
        }
        public AddStudentDetails getSubjectDetails(long SchoolId, long ClassId, int LvlId)
        {
            AddStudentDetails res = new AddStudentDetails();
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                SqlCommand Command = new SqlCommand()
                {
                    Connection = Connection,
                    CommandText = @"select admin.Syllabus_Subject.ID,admin.Syllabus_Subject.Subject,
                                        admin.Syllabus_Subject.Level_ID,admin.Syllabus_Subject.Class_ID,
                                        admin.Syllabus_Subject.CreatedBy_ID,admin.Syllabus_Subject.School_ID
                                            from admin.Syllabus_Subject where admin.Syllabus_Subject.Level_ID=@LvlId and
                                            admin.Syllabus_Subject.Class_ID=@ClsId and admin.Syllabus_Subject.School_ID=@SchoolId
                                                and admin.Syllabus_Subject.Status=1"
                };
                Command.Parameters.AddWithValue("@SchoolId", SchoolId);
                Command.Parameters.AddWithValue("@LvlId", LvlId);
                Command.Parameters.AddWithValue("@ClsId", ClassId);
                if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                    res.ClassDetails = new List<ClassDetails>();
                    SqlDataReader reader = Command.ExecuteReader();
                    res.SubjectInfo = new List<AddsubjectInfo>();
                    while (reader.Read())
                    {
                        AddsubjectInfo temp = new AddsubjectInfo()
                        {

                            ID = Convert.ToInt32(reader["ID"]),
                            Subject = Convert.ToString(reader["Subject"]),
                            Level_ID = Convert.ToInt32(reader["Level_ID"]),
                            Class_ID = Convert.ToInt32(reader["Class_ID"]),
                            CreatedBy_ID = Convert.ToInt32(reader["CreatedBy_ID"]),
                            School_ID = Convert.ToInt32(reader["School_ID"])
                        }; res.SubjectInfo.Add(temp);
                    }
                }
            }
            return res;
        }
        public AddStudentDetails getSemesterDetails()
        {
            AddStudentDetails res = new AddStudentDetails();
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                    SqlCommand Command = new SqlCommand()
                    {
                        Connection = Connection,
                        CommandText = @"select admin.Semester.ID,
		                                    admin.Semester.Semester,
		                                        admin.Semester.CreatedDate from admin.Semester
		                                            where admin.Semester.IsActive=1"
                    };
                    SqlDataReader reader = Command.ExecuteReader();
                    res.Semester = new List<Semester>();
                    while (reader.Read())
                    {
                        Semester temp = new Semester()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            SemesterName = Convert.ToString(reader["Semester"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        }; res.Semester.Add(temp);
                    }
                }
                return res;
            }
        }
        public AddStudentDetails getTopicDetails(long SubjectId,int SemesterId,long ClassId,int Level_Id,long SchoolID){
            AddStudentDetails res = new AddStudentDetails();
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed){
                    Connection.Open();
                    SqlCommand Command = new SqlCommand(){
                        Connection = Connection,
                        CommandText = @"select MAX(admin.Syllabus_Topic.Topic_ID) Topic_ID,admin.Syllabus_Topic.Topic_Name,
                                        CONVERT(VARCHAR,admin.Syllabus_Topic.CreatedDate,121) CreatedDate
                                        from admin.Syllabus_Topic
                                        inner join admin.Syllabus_Subject ss
                                        on admin.Syllabus_Topic.Class_ID_FK=ss.Class_ID
                                        inner join admin.Syllabus_Subject ss1
                                        on admin.Syllabus_Topic.Level_ID_Fk=ss1.Level_ID
                                        inner join admin.Syllabus_Subject ss2
                                        on admin.Syllabus_Topic.SchoolID=ss2.School_ID
                                        where admin.Syllabus_Topic.Subject_ID_FK=@SubID and admin.Syllabus_Topic.Semester_ID_FK=@SemID
                                        and admin.Syllabus_Topic.Class_ID_FK=@ClsID and admin.Syllabus_Topic.Level_ID_Fk=@LvlID
                                        and admin.Syllabus_Topic.SchoolID=@SchoolID and admin.Syllabus_Topic.IsActive=1
                                        and ss.Status=1 group by admin.Syllabus_Topic.Topic_ID,admin.Syllabus_Topic.Topic_Name,
                                        admin.Syllabus_Topic.CreatedDate"
                    };
                    
                    Command.Parameters.AddWithValue("@SubID",SubjectId);
                    Command.Parameters.AddWithValue("@SemID",SemesterId);
                    Command.Parameters.AddWithValue("@ClsID", ClassId);
                    Command.Parameters.AddWithValue("@LvlID", Level_Id);
                    Command.Parameters.AddWithValue("@SchoolID", SchoolID);
                    SqlDataReader reader = Command.ExecuteReader();
                    res.SchoolTopic = new List<SchoolTopic>();
                    while (reader.Read())
                    {
                        SchoolTopic temp = new SchoolTopic()
                        {
                            ID = Convert.ToInt64(reader["Topic_ID"]),
                            Topic_Name = Convert.ToString(reader["Topic_Name"]),
                            CreatedDate = Convert.ToString(reader["CreatedDate"])
                        }; res.SchoolTopic.Add(temp);
                    }
                }
                return res;
            }
        }
        #endregion
        #region Forum
        public ResultInfo<ClassTeacherForum> GetTeacherForumGroupsList(long SchoolID){
            ResultInfo<ClassTeacherForum> list = new ResultInfo<ClassTeacherForum>();
            string Query = @"select teacher.ClassTeacher_OnlineForum.Forum_ID,
                            teacher.ClassTeacher_OnlineForum.Forum_Name,
                            ISNULL(teacher.ClassTeacher_OnlineForum.Forum_About,'') Forum_About,
                            CONVERT(VARCHAR,teacher.ClassTeacher_OnlineForum.CreatedDate,121) CreatedDate,
                                ISNULL(td.Name,'') as CreatedBy from teacher.ClassTeacher_OnlineForum
                                right outer join teacher.TeachersDetails td on
                                teacher.ClassTeacher_OnlineForum.CreatedBy_TeacherID=td.UID_FK
                                where teacher.ClassTeacher_OnlineForum.Status=1 and td.SchoolID_FK=@SchoolId
                                    group by teacher.ClassTeacher_OnlineForum.Forum_ID,
                                    teacher.ClassTeacher_OnlineForum.Forum_Name,
                                    teacher.ClassTeacher_OnlineForum.CreatedDate,
                                        teacher.ClassTeacher_OnlineForum.Forum_About,
                                        td.Name order by teacher.ClassTeacher_OnlineForum.CreatedDate desc";
            using (SqlConnection Connection=new SqlConnection(conn)){
                try {
                    SqlCommand Command = new SqlCommand(){
                        CommandText=Query,
                        Connection=Connection
                    };
                    if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed) {
                        Connection.Open();
                        Command.Parameters.AddWithValue("@SchoolId",SchoolID);
                        SqlDataReader Reader = Command.ExecuteReader();
                        list.Info = new ClassTeacherForum();
                        list.Info.grouplist = new List<ClassTeacherForumGroups>();
                        while (Reader.Read()){
                            ClassTeacherForumGroups tempobj = new ClassTeacherForumGroups(){
                                Forum_ID = Convert.ToInt64(Reader["Forum_ID"]),
                                Forum_Name = Convert.ToString(Reader["Forum_Name"]),
                                CreatedDate = Convert.ToString(Reader["CreatedDate"]),
                                CreatedByName = Convert.ToString(Reader["CreatedBy"]),
                                Forum_About = Convert.ToString(Reader["Forum_About"])
                            };
                            list.Info.grouplist.Add(tempobj);
                        }list.ErrorCode = 200;
                        list.Description = "Success|Teacher's Forum Group List";
                    }
                } catch(SqlException exp){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt",DateTime.Today),exp.ErrorCode+':'+exp.Message);
                    list.ErrorCode = 500;
                    list.Description = "Failed|" + exp.Message;
                }
            }return list;
        }
        public ResultInfo<ClassTeacherForum> CreateOnlineGroup(ClassTeacherForumGroups Info){
            ResultInfo<ClassTeacherForum> list = new ResultInfo<ClassTeacherForum>();
            foreach (var inf in Info.GetType().GetProperties()){
                if (inf.PropertyType == typeof(string)){
                    if (inf.GetValue(Info, null) == null){
                        inf.SetValue(Info, string.Empty, null);
                    }
                }
            }
            string Query = @"[teacher].[CreateClassTeacherOnlineGroup]";
            using (SqlConnection Connection=new SqlConnection(conn)){
                try {
                    SqlCommand Command = new SqlCommand() {
                        CommandText = Query,
                        Connection = Connection,
                        CommandType=CommandType.StoredProcedure
                    };
                    if (Connection.State==ConnectionState.Broken||Connection.State==ConnectionState.Closed){
                        Connection.Open();
                        Command.Parameters.AddWithValue("@Forum_Name",Info.Forum_Name);
                        Command.Parameters.AddWithValue("@Forum_About",Info.Forum_About);
                        Command.Parameters.AddWithValue("@CreatedByID", Info.CreatedByID);
                        Command.Parameters.AddWithValue("@SchoolID", Info.SchoolID);
                        SqlDataReader Reader = Command.ExecuteReader();
                        list.Info = new ClassTeacherForum();
                        list.Info.grouplist = new List<ClassTeacherForumGroups>();
                        while (Reader.Read()) {
                            ClassTeacherForumGroups temp = new ClassTeacherForumGroups(){
                                Forum_ID=Convert.ToInt64(Reader["Forum_ID"]),
                                Forum_Name=Convert.ToString(Reader["Forum_Name"]),
                                Forum_About=Convert.ToString(Reader["Forum_About"]),
                                CreatedDate=Convert.ToString(Reader["CreatedDate"]),
                                CreatedByName=Convert.ToString(Reader["CreatedBy"]),

                            };
                            list.Info.grouplist.Add(temp);
                        }list.ErrorCode = 200;
                        list.Description = "Success|Group created successfully.";
                    }
                } catch(SqlException exp) {
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    list.ErrorCode = 500;
                    list.Description = "Failed|"+exp.Message;
                }
            }return list;
        }
        public ResultInfo<ClassTeacherForum> GetGroupWiseTopic(long Forum_ID, long SchoolID, string Forum_Name){
            ResultInfo<ClassTeacherForum> class_forum = new ResultInfo<ClassTeacherForum>();
            string Query = @"select tf.ID,ISNULL(tf.Topic_Name,'') Topic_Name,tf.Forum_ID_FK,
                            CONVERT(VARCHAR,tf.CreatedDate,121) CreatedDate,
                            ISNULL(teacher.ClassTeacher_OnlineForum.Forum_Name,'') Forum_Name, 
                            ISNULL(teacher.TeachersDetails.Name,'') Name from teacher.ClassTeacher_OnlineForum_GroupTopic tf
                            right outer join teacher.TeachersDetails on
                            teacher.TeachersDetails.UID_FK=tf.CreatedBy_TeacherID
                            inner join teacher.ClassTeacher_OnlineForum on
                            teacher.ClassTeacher_OnlineForum.Forum_ID=tf.Forum_ID_FK
                            where tf.Status=1 
                            and tf.SchoolID=@SchoolID
                            and tf.Forum_ID_FK=@ForumID";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    SqlCommand Command = new SqlCommand(){
                        CommandText = Query,
                        Connection = Connection,
                    };
                    if(Connection.State==ConnectionState.Broken||
                        Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        Command.Parameters.AddWithValue("@SchoolID", SchoolID);
                        Command.Parameters.AddWithValue("@ForumID", Forum_ID);
                        SqlDataReader Reader = Command.ExecuteReader();
                        
                        class_forum.Info = new ClassTeacherForum();
                        class_forum.Info.topic = new List<ClassTeacherTopic>();
                        class_forum.Info.Forum_ID = Forum_ID;
                        class_forum.Info.Forum_Name = Forum_Name;
                        if (Reader.HasRows){
                            while (Reader.Read()){
                                class_forum.Info.Forum_Name = Convert.ToString(Reader["Forum_Name"]);
                                ClassTeacherTopic temp = new ClassTeacherTopic()
                                {
                                    Topic_ID = Convert.ToInt64(Reader["ID"]),
                                    Topic_Name = Convert.ToString(Reader["Topic_Name"]),
                                    Forum_ID_FK = Convert.ToInt64(Reader["Forum_ID_FK"]),
                                    CreatedByName = Convert.ToString(Reader["Name"]),
                                    CreatedDate = Convert.ToString(Reader["CreatedDate"])
                                };
                                class_forum.Info.topic.Add(temp);
                            }
                            class_forum.ErrorCode = 200;
                            class_forum.Description = "Success|Topic's list retrieved successfully.";
                        }
                        class_forum.ErrorCode = 200;
                        class_forum.Description = "Success|No topic found.";

                    }
                }
                catch (SqlException exp){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    class_forum.ErrorCode = 500;
                    class_forum.Description = "Failed|" + exp.Message;
                }
            }return class_forum;
        }
        public ResultInfo<ClassTeacherForum> CreateGroupWiseTopic(ClassTeacherTopic Info){
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"[teacher].[CreateClassTeacherForumTopics]";
            using(SqlConnection Connection=new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand() {
                            CommandText = Query,
                            Connection=Connection,
                            CommandType=CommandType.StoredProcedure
                        };
                        Command.Parameters.AddWithValue("@TopicName", Info.Topic_Name);
                        Command.Parameters.AddWithValue("@Forum_ID_FK", Info.Forum_ID_FK);
                        Command.Parameters.AddWithValue("@CreatedBy_TeacherID", Info.CreatedById);
                        Command.Parameters.AddWithValue("@SchoolID", Info.SchoolID);
                        SqlDataReader Reader = Command.ExecuteReader();
                        result.Info = new ClassTeacherForum();
                        result.Info.topic = new List<ClassTeacherTopic>();
                        while (Reader.Read()){
                        result.Info.Forum_Name = Convert.ToString(Reader["Forum_Name"]);
                        ClassTeacherTopic temp = new ClassTeacherTopic(){
                             Topic_ID = Convert.ToInt64(Reader["ID"]),
                             Topic_Name = Convert.ToString(Reader["Topic_Name"]),
                             Forum_ID_FK = Convert.ToInt64(Reader["Forum_ID_FK"]),
                             CreatedByName = Convert.ToString(Reader["Name"]),
                             CreatedDate = Convert.ToString(Reader["CreatedDate"])
                           };
                        result.Info.topic.Add(temp);
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success|Topic's list retrieved successfully.";
                    }
                }catch(SqlException exp){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }return result;
        }
        private ClassTeacherTopic GetCommentTopic(long ID) {
            ClassTeacherTopic res = new ClassTeacherTopic();
            string Query = @"select gt.ID,ISNULL(gt.Topic_Name,'') Topic_Name,
                                gt.Forum_ID_FK,CONVERT(VARCHAR,gt.CreatedDate,121) CreatedDate,
                                ISNULL(td.Name,'') Name 
                                  from teacher.ClassTeacher_OnlineForum_GroupTopic gt
                                    left outer join teacher.TeachersDetails td
                                    on gt.CreatedBy_TeacherID=td.UID_FK
                                        left outer join teacher.TeachersDetails td1
                                        on gt.SchoolID=td1.SchoolID_FK
                                            where gt.Status=1 and gt.ID=@ID
                                                group by gt.ID,gt.Topic_Name,
                                                gt.Forum_ID_FK,gt.CreatedDate,
                                                    td.Name";

            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                        SqlCommand Command = new SqlCommand(Query, Connection);
                        Command.Parameters.AddWithValue("@ID", ID);
                        SqlDataReader reader = Command.ExecuteReader();
                        while (reader.Read())
                        {
                            res.Topic_ID = Convert.ToInt64(reader["ID"]);
                            res.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                            res.Forum_ID_FK = Convert.ToInt64(reader["Forum_ID_FK"]);
                            res.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                            res.CreatedByName = Convert.ToString(reader["Name"]);

                        }
                    }
                }catch(SqlException ex){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), ex.ErrorCode + ':' + ex.Message);
                }
            }return res;
        }
        private ClassTeacherForumGroups GetGroupSchoolWise(long SchoolId, long Id){
            ClassTeacherForumGroups res = new ClassTeacherForumGroups();
            string Query = @"DECLARE @var BIGINT
                                SET @var=CONVERT(BIGINT,(select teacher.ClassTeacher_OnlineForum_GroupTopic.Forum_ID_FK 
				                                            from teacher.ClassTeacher_OnlineForum_GroupTopic where
					                                            teacher.ClassTeacher_OnlineForum_GroupTopic.ID=@ID))
                                select teacher.ClassTeacher_OnlineForum.Forum_ID,
                                teacher.ClassTeacher_OnlineForum.Forum_Name,
                                teacher.ClassTeacher_OnlineForum.Forum_About,
                                CONVERT(VARCHAR,teacher.ClassTeacher_OnlineForum.CreatedDate,121) CreatedDate,
                                td.Name as CreatedBy
                                    from teacher.ClassTeacher_OnlineForum 
                                    right outer join
                                    teacher.TeachersDetails td1 on
                                    teacher.ClassTeacher_OnlineForum.SchoolID=td1.SchoolID_FK
                                        inner join
                                        teacher.TeachersDetails td on
                                        teacher.ClassTeacher_OnlineForum.CreatedBy_TeacherID=td.UID_FK
                                        where teacher.ClassTeacher_OnlineForum.Status=1
                                            and td1.SchoolID_FK=@SchoolId
							                and teacher.ClassTeacher_OnlineForum.Forum_ID=@var
                                            group by teacher.ClassTeacher_OnlineForum.Forum_ID,
                                                teacher.ClassTeacher_OnlineForum.Forum_Name,
                                                teacher.ClassTeacher_OnlineForum.CreatedDate,
                                                teacher.ClassTeacher_OnlineForum.Forum_About,
                                                td.Name";

            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand(Query, Connection);
                        Command.Parameters.AddWithValue("@SchoolId", SchoolId);
                        Command.Parameters.AddWithValue("@ID", Id);
                        SqlDataReader reader = Command.ExecuteReader();
                        while (reader.Read()){
                            res.Forum_ID = Convert.ToInt64(reader["Forum_ID"]);
                            res.Forum_Name = Convert.ToString(reader["Forum_Name"]);
                            res.Forum_About = Convert.ToString(reader["Forum_About"]);
                            res.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                            res.CreatedByName = Convert.ToString(reader["CreatedBy"]);
                        }
                    }
                }catch (SqlException ex){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), ex.ErrorCode + ':' + ex.Message);
                }
            }
            return res;
        }
        public ResultInfo<ClassTeacherForum> GetTopicWiseComment(long TopicId,long SchoolId){
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"select tc.CommentId,ISNULL(tc.CommentText,'') CommentText,
                                CONVERT(VARCHAR,tc.CreatedDate,121) CreatedDate,
                                tc.Forum_ID_FK,tc.Topic_ID_FK,ISNULL(td.Name,'') Name,
                                tc.TeacherID_FK,ISNULL(td.Profile_Image,'') Profile_Image,
                                    tc.IsReported,tc.TopicCommentLikes,tc.TopicCommentDislikes
                                    from teacher.ClassTeacher_OnlineForum_GroupComment tc
                                    left outer join teacher.TeachersDetails td
                                        on tc.TeacherID_FK=td.UID_FK where tc.Status=1 
                                        and tc.Topic_ID_FK=@TopicID and td.SchoolID_FK=@SchoolID
                                        order by CreatedDate desc";

            using(SqlConnection Connection = new SqlConnection(conn)){
                try{
                    SqlCommand Command = new SqlCommand(Query,Connection);
                    Command.Parameters.AddWithValue("@TopicID",TopicId);
                    Command.Parameters.AddWithValue("@SchoolID", SchoolId);
                    if (Connection.State == ConnectionState.Broken ||
                            Connection.State == ConnectionState.Closed){
                            Connection.Open();
                            result.Info = new ClassTeacherForum();
                            result.Info.Comment = new List<ClassTeacherTopicComment>();
                            result.Info.CommentTopic = new ClassTeacherTopic();
                            result.Info.TopicGroup = new ClassTeacherForumGroups();

                        SqlDataReader reader = Command.ExecuteReader();
                        while (reader.Read()){
                            if (reader.HasRows){
                                ClassTeacherTopicComment temp = new ClassTeacherTopicComment() {
                                    CommentId = Convert.ToInt64(reader["CommentId"]),
                                    CommentText = Convert.ToString(reader["CommentText"]),
                                    CreatedDate = Convert.ToString(reader["CreatedDate"]),
                                    Forum_ID_FK = Convert.ToInt64(reader["Forum_ID_FK"]),
                                    Topic_ID_FK = Convert.ToInt64(reader["Topic_ID_FK"]),
                                    Commentator_Name = Convert.ToString(reader["Name"]),
                                    TeacherID_FK=Convert.ToInt64(reader["TeacherID_FK"]),
                                    Profile_Image = Convert.ToString(reader["Profile_Image"]),
                                    IsReported = Convert.ToBoolean(reader["IsReported"]),
                                    TopicCommentLikes = Convert.ToInt64(reader["TopicCommentLikes"]),
                                    TopicCommentDislikes = Convert.ToInt64(reader["TopicCommentDislikes"])
                                }; result.Info.Comment.Add(temp);
                            }
                        }
                        result.Info.TopicGroup = GetGroupSchoolWise(SchoolId,TopicId);
                        result.Info.CommentTopic = GetCommentTopic(TopicId);
                        result.ErrorCode = 200;
                        result.Description = "Success|Comment's list retrieved successfully.";
                    }
                }catch (SqlException exp){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }return result;
        }
        public ResultInfo<ClassTeacherForum> InsertCommentGroupTopicWise(ClassTeacherTopicComment Info){
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"[teacher].[ClassTeacherGroupTopicComment]";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand()
                        {
                            CommandText = Query,
                            Connection = Connection,
                            CommandType = CommandType.StoredProcedure
                        };
                        Command.Parameters.AddWithValue("@CommentText", Info.CommentText);
                        Command.Parameters.AddWithValue("@ForumID", Info.Forum_ID_FK);
                        Command.Parameters.AddWithValue("@TopicID", Info.Topic_ID_FK);
                        Command.Parameters.AddWithValue("@TeacherID", Info.TeacherID_FK);
                        Command.Parameters.AddWithValue("@SchoolID", Info.School_ID_FK);

                        result.Info = new ClassTeacherForum();
                        result.Info.Comment = new List<ClassTeacherTopicComment>();
                        result.Info.CommentTopic = new ClassTeacherTopic();
                        result.Info.TopicGroup = new ClassTeacherForumGroups();
                        result.Info.TopicsComment = new ClassTeacherTopicComment();
                        SqlDataReader reader = Command.ExecuteReader();
                        while (reader.Read()){
                            result.Info.TopicsComment.CommentId = Convert.ToInt64(reader["CommentId"]);
                            result.Info.TopicsComment.CommentText = Convert.ToString(reader["CommentText"]);
                            result.Info.TopicsComment.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                            result.Info.TopicsComment.Forum_ID_FK = Convert.ToInt64(reader["Forum_ID_FK"]);
                            result.Info.TopicsComment.Topic_ID_FK = Convert.ToInt64(reader["Topic_ID_FK"]);
                            result.Info.TopicsComment.Commentator_Name = Convert.ToString(reader["Name"]);
                            result.Info.TopicsComment.TeacherID_FK = Convert.ToInt64(reader["TeacherID_FK"]);
                            result.Info.TopicsComment.Profile_Image = Convert.ToString(reader["Profile_Image"]);
                            result.Info.TopicsComment.IsReported = Convert.ToBoolean(reader["IsReported"]);
                            result.Info.TopicsComment.TopicCommentLikes = Convert.ToInt64(reader["TopicCommentLikes"]);
                            result.Info.TopicsComment.TopicCommentDislikes = Convert.ToInt64(reader["TopicCommentDislikes"]);
                            break;
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success|Comment's list retrieved successfully.";
                    }
                }
                catch (SqlException exp)
                {
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }
            return result;
        }
        public ResultInfo<ClassTeacherForum> GroupCommentLikeOrDislike(ClassTeacherTopicComment Info){
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"[teacher].[OnlineGroupTopicCommentLikeOrDislike]";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand(){
                            CommandText = Query,
                            Connection = Connection,
                            CommandType = CommandType.StoredProcedure
                        };
                        Command.Parameters.AddWithValue("@Comment_ID", Info.CommentId);
                        Command.Parameters.AddWithValue("@UID_FK", Info.TeacherID_FK);
                        Command.Parameters.AddWithValue("@SchoolID_FK", Info.School_ID_FK);
                        Command.Parameters.AddWithValue("@Topic_ID", Info.Topic_ID_FK);
                        if (Info.TopicCommentLikes == 1){
                            Command.Parameters.AddWithValue("@Lk",Info.TopicCommentLikes);
                        } else{
                            Command.Parameters.AddWithValue("@Lk",Info.TopicCommentLikes);
                        }
                        
                        result.Info = new ClassTeacherForum();
                        result.Info.Comment = new List<ClassTeacherTopicComment>();
                        result.Info.CommentTopic = new ClassTeacherTopic();
                        result.Info.TopicGroup = new ClassTeacherForumGroups();
                        result.Info.TopicsComment = new ClassTeacherTopicComment();

                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows) {
                            while (reader.Read()){
                                result.Info.TopicsComment.CommentId = Convert.ToInt64(reader["CommentId"]);
                                result.Info.TopicsComment.CommentText = Convert.ToString(reader["CommentText"]);
                                result.Info.TopicsComment.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                                result.Info.TopicsComment.Forum_ID_FK = Convert.ToInt64(reader["Forum_ID_FK"]);
                                result.Info.TopicsComment.Topic_ID_FK = Convert.ToInt64(reader["Topic_ID_FK"]);
                                result.Info.TopicsComment.Commentator_Name = Convert.ToString(reader["Name"]);
                                result.Info.TopicsComment.Profile_Image = Convert.ToString(reader["Profile_Image"]);
                                result.Info.TopicsComment.TopicCommentLikes = Convert.ToInt64(reader["Likes"]);
                                result.Info.TopicsComment.TopicCommentDislikes = Convert.ToInt64(reader["Dislikes"]);
                                result.Info.TopicsComment.IsReported = Convert.ToBoolean(reader["IsReported"]);

                                break;
                            }
                        }result.ErrorCode = 200;
                        result.Description = "Success|Comment retrieved successfully.";
                    }
                }
                catch (SqlException exp)
                {
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }
            return result;
        }
        public ResultInfo<ClassTeacherForum> EditOnlineForumComment(ClassTeacherTopicComment Info){
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"[teacher].[ClassTeacherOnlineCommunityCommentUpdate]";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand(){
                            CommandText = Query,
                            Connection = Connection,
                            CommandType = CommandType.StoredProcedure
                        };
                        Command.Parameters.AddWithValue("@CommentId", Info.CommentId);
                        Command.Parameters.AddWithValue("@CommentText", Info.CommentText);
                        result.Info = new ClassTeacherForum();
                        result.Info.Comment = new List<ClassTeacherTopicComment>();
                        result.Info.CommentTopic = new ClassTeacherTopic();
                        result.Info.TopicGroup = new ClassTeacherForumGroups();
                        result.Info.TopicsComment = new ClassTeacherTopicComment();

                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows){
                            while (reader.Read()){
                                result.Info.TopicsComment.CommentId = Convert.ToInt64(reader["CommentId"]);
                                result.Info.TopicsComment.CommentText = Convert.ToString(reader["CommentText"]);
                                result.Info.TopicsComment.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                                result.Info.TopicsComment.Forum_ID_FK = Convert.ToInt64(reader["Forum_ID_FK"]);
                                result.Info.TopicsComment.Topic_ID_FK = Convert.ToInt64(reader["Topic_ID_FK"]);
                                result.Info.TopicsComment.Commentator_Name = Convert.ToString(reader["Name"]);
                                result.Info.TopicsComment.Profile_Image = Convert.ToString(reader["Profile_Image"]);
                                result.Info.TopicsComment.TopicCommentLikes = Convert.ToInt64(reader["Likes"]);
                                result.Info.TopicsComment.TopicCommentDislikes = Convert.ToInt64(reader["Dislikes"]);
                                result.Info.TopicsComment.IsReported = Convert.ToBoolean(reader["IsReported"]);
                                break;
                            }
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success|Comment successfully updated.";
                    }
                }
                catch (SqlException exp)
                {
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }
            return result;
        }
        public ResultInfo<ClassTeacherForum> DeleteCommunityComment(long Id) {
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"update [teacher].[ClassTeacher_OnlineForum_GroupComment]
                            set [teacher].[ClassTeacher_OnlineForum_GroupComment].Status=0
                            where [teacher].[ClassTeacher_OnlineForum_GroupComment].CommentId=@CommentId";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand()
                        {
                            CommandText = Query,
                            Connection = Connection
                        };
                        Command.Parameters.AddWithValue("@CommentId", Id);
                        if(Command.ExecuteNonQuery()>0){
                            result.ErrorCode = 200;
                            result.Description = "Success| Comment has been deleted successfully.";
                        }
                    }
                }catch(SqlException exp){
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }return result;
        }
        private bool CheckForCommunityReportedComment(long ReportedBy_ID_FK){
            bool resp = false;
            string Query = @"select * from [teacher].[ClassTeacher_Community_CommentReport]
                                where [teacher].[ClassTeacher_Community_CommentReport].ReportedBy_ID_FK=@ReportedBy
                                    and [teacher].[ClassTeacher_Community_CommentReport].Status=1";
            using (SqlConnection Connection = new SqlConnection(conn)) {
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand(){
                            CommandText = Query,
                            Connection = Connection
                        };
                        Command.Parameters.AddWithValue("@ReportedBy",ReportedBy_ID_FK);
                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows){
                            resp = true;
                        }
                    }
                }
                catch (SqlException exp){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                }
            }return resp;
        }
        public ResultInfo<ClassTeacherForum> ReportCommunityComment(ClassTeacherCommunityCommentReport Info){
            ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
            string Query = @"insert into [teacher].[ClassTeacher_Community_CommentReport]
                            (ReportReason,Comment_ID_FK,ReportedBy_ID_FK,SchoolID)
                             values(@Reason,@CommentId,@ReportedById,@SchoolId)

                             update [teacher].[ClassTeacher_OnlineForum_GroupComment]
                                set [teacher].[ClassTeacher_OnlineForum_GroupComment].IsReported=1
                                where [teacher].[ClassTeacher_OnlineForum_GroupComment].CommentId=@CommentId";

            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    if (Connection.State == ConnectionState.Broken ||
                           Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        SqlCommand Command = new SqlCommand()
                        {
                            CommandText = Query,
                            Connection = Connection
                        };
                        Command.Parameters.AddWithValue("@Reason",Info.ReportReason);
                        Command.Parameters.AddWithValue("@CommentId", Info.Comment_ID_FK);
                        Command.Parameters.AddWithValue("@ReportedById", Info.ReportedBy_ID_FK);
                        Command.Parameters.AddWithValue("@SchoolId", Info.SchoolID);
                        if (CheckForCommunityReportedComment(Info.ReportedBy_ID_FK) !=true){
                            if (Command.ExecuteNonQuery() > 0){
                                result.ErrorCode = 200;
                                result.Description = "Success| Comment has been reported successfully.";
                            }
                        }else{
                            result.ErrorCode = 200;
                            result.Description = "Failed| You already reported this comment.";
                        }
                    }
                }
                catch (SqlException exp){
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }
            return result;
        }
        //public ResultInfo<List<EditProfileTeacher>> GetListOfTeacherOnAddMember(long Id,long SchoolId){
        //    ResultInfo<List<EditProfileTeacher>> result = new ResultInfo<List<EditProfileTeacher>>();
        //    string Query = @"select td.Name,td.UID_FK,td.Profile_Image from [teacher].[TeachersDetails] td
        //                    where td.UID_FK<>@TeacherId and td.SchoolID_FK=@SchoolId and td.TeacherType_FK=3 
        //                    or td.TeacherType_FK=6 or td.TeacherType_FK=2 and td.Status=1";
        //    using (SqlConnection Connection = new SqlConnection(conn)){
        //        try{
        //            if (Connection.State == ConnectionState.Broken ||
        //                   Connection.State == ConnectionState.Closed){
        //                Connection.Open();
        //                SqlCommand Command = new SqlCommand()
        //                {
        //                    CommandText = Query,
        //                    Connection = Connection
        //                };Command.Parameters.AddWithValue("@TeacherId",Id);
        //                Command.Parameters.AddWithValue("@SchoolId",SchoolId);
        //                result.Info = new List<EditProfileTeacher>();
        //                SqlDataReader reader = Command.ExecuteReader();
        //                if(reader.HasRows){
        //                    while (reader.Read()){
        //                        EditProfileTeacher temp = new EditProfileTeacher(){
        //                            ID=Convert.ToInt64(reader["UID_FK"]),
        //                            Teacher_Name= Convert.ToString(reader["Name"]),
        //                            ProfileImage= Convert.ToString(reader["Profile_Image"])
        //                        }; result.Info.Add(temp);
        //                    }
        //                }
        //            }
        //            result.ErrorCode = 200;
        //            result.Description = "Success|List has been retrieved successfully.";
        //        }
        //        catch (SqlException exp){
        //            WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
        //            result.ErrorCode = 500;
        //            result.Description = "Failed|" + exp.Message;
        //        }
        //    }
        //    return result;
        //}
        //public ResultInfo<string> CommunityAddTeacher(ClassTeacherForumGroups Info){
        //    ResultInfo<string> result = new ResultInfo<string>();
        //    string Query = @"insert into [teacher].[ClassTeacher_OnlineForum_GroupMember]
        //                        (Forum_ID_FK,IsAdmin,TeacherID_FK,SchoolID) values
        //                                (@ForumID,@IsAdmin,@TeacherID_FK,@SchoolID)";
        //    using (SqlConnection Connection = new SqlConnection(conn)){
        //        try{
        //            if (Connection.State == ConnectionState.Broken ||
        //                   Connection.State == ConnectionState.Closed){
        //                Connection.Open();
        //                SqlCommand Command = new SqlCommand(){
        //                    CommandText = Query,
        //                    Connection = Connection
        //                };
        //                Command.Parameters.AddWithValue("@ForumID", Info.Forum_ID);
        //                Command.Parameters.AddWithValue("@IsAdmin", 0);
        //                Command.Parameters.AddWithValue("@TeacherID_FK", Info.CreatedByID);
        //                Command.Parameters.AddWithValue("@SchoolID", Info.SchoolID);
        //                if (CheckForAlreadyExistingCommunityMember(Info) == false){
        //                    if (Command.ExecuteNonQuery() > 0){
        //                        result.ErrorCode = 200;
        //                        result.Description = "Success|Request has been sent successfully.";
        //                    }
        //                }
        //                else{
        //                    result.ErrorCode = 500;
        //                    result.Description = "Failed|Member already assigned to this group.";
        //                }

        //            }
        //        }catch (SqlException exp){
        //            WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
        //            result.ErrorCode = 500;
        //            result.Description = "Failed|" + exp.Message;
        //        }
        //    }return result;
        //}
        //private bool CheckForAlreadyExistingCommunityMember(ClassTeacherForumGroups Info){
        //    Boolean result = false;
        //    string Query = @"select * from [teacher].[ClassTeacher_OnlineForum_GroupMember]
        //                        where [teacher].[ClassTeacher_OnlineForum_GroupMember].Forum_ID_FK=@ForumId
        //                            and [teacher].[ClassTeacher_OnlineForum_GroupMember].TeacherID_FK=@TeacherId";

        //    using (SqlConnection Connection = new SqlConnection(conn)){
        //        try{
        //            if (Connection.State == ConnectionState.Broken ||
        //                   Connection.State == ConnectionState.Closed){
        //                Connection.Open();
        //                SqlCommand Command = new SqlCommand(){
        //                    CommandText = Query,
        //                    Connection = Connection
        //                };
        //                Command.Parameters.AddWithValue("@ForumID", Info.Forum_ID);
        //                Command.Parameters.AddWithValue("@TeacherId", Info.CreatedByID);
        //                SqlDataReader reader = Command.ExecuteReader();
        //                while (reader.HasRows){
        //                    result = true;
        //                    break;
        //                }
        //            }
        //        }
        //        catch (SqlException exp){
        //            WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
        //        }
        //    }
        //    return result;
        //}

        //public ResultInfo<ClassTeacherForum> GetCommunityPendingGroupRequests(long TeacherId, long SchoolId){
        //    ResultInfo<ClassTeacherForum> result = new ResultInfo<ClassTeacherForum>();
        //    string Query = @"select teacher.ClassTeacher_OnlineForum.Forum_ID,
        //                        teacher.ClassTeacher_OnlineForum.Forum_Name,
        //                        ISNULL(teacher.ClassTeacher_OnlineForum.Forum_About,'') Forum_About,
        //                        CONVERT(VARCHAR,teacher.ClassTeacher_OnlineForum.CreatedDate,121) CreatedDate,
        //                        ISNULL(td.Name,'') as CreatedBy
        //                        from teacher.ClassTeacher_OnlineForum
        //                        inner join teacher.TeachersDetails td on
        //                        teacher.ClassTeacher_OnlineForum.CreatedBy_TeacherID=td.UID_FK
        //                        left join teacher.ClassTeacher_OnlineForum_GroupMember tm
        //                        on teacher.ClassTeacher_OnlineForum.Forum_ID=tm.Forum_ID_FK
        //                        where teacher.ClassTeacher_OnlineForum.Status=1
        //                        and td.SchoolID_FK=@SchoolId and tm.TeacherID_FK=@TeacherId
        //                        and tm.IsRequestPending=1
        //                        group by teacher.ClassTeacher_OnlineForum.Forum_ID,
        //                        teacher.ClassTeacher_OnlineForum.Forum_Name,
        //                        teacher.ClassTeacher_OnlineForum.CreatedDate,
        //                        teacher.ClassTeacher_OnlineForum.Forum_About,
        //                        td.Name order by teacher.ClassTeacher_OnlineForum.CreatedDate desc";

        //    using (SqlConnection Connection = new SqlConnection(conn)){
        //        try{
        //            SqlCommand Command = new SqlCommand(){
        //                CommandText = Query,
        //                Connection = Connection
        //            };
        //            if (Connection.State == ConnectionState.Broken 
        //                    || Connection.State == ConnectionState.Closed){
        //                Connection.Open();
        //                Command.Parameters.AddWithValue("@SchoolId", SchoolId);
        //                Command.Parameters.AddWithValue("@TeacherId", TeacherId);
        //                SqlDataReader Reader = Command.ExecuteReader();
        //                result.Info = new ClassTeacherForum();
        //                result.Info.grouplist = new List<ClassTeacherForumGroups>();
        //                while (Reader.Read()){
        //                    ClassTeacherForumGroups temp = new ClassTeacherForumGroups(){
        //                        Forum_ID = Convert.ToInt64(Reader["Forum_ID"]),
        //                        Forum_Name = Convert.ToString(Reader["Forum_Name"]),
        //                        Forum_About = Convert.ToString(Reader["Forum_About"]),
        //                        CreatedDate = Convert.ToString(Reader["CreatedDate"]),
        //                        CreatedByName = Convert.ToString(Reader["CreatedBy"])

        //                    };
        //                    result.Info.grouplist.Add(temp);
        //                    result.ErrorCode = 200;
        //                    result.Description = "Success|Pending group list.";
        //                }
        //            }
        //        }catch (SqlException exp){
        //            WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
        //            result.ErrorCode = 500;
        //            result.Description = "Failed|" + exp.Message;
        //        }
        //    }return result;
        //}
        //public ResultInfo<string> ClassTeacherAcceptOrDeclineForumRequests(long TeacherId,long SchoolId,long ForumId,bool Btnaccpt){
        //    ResultInfo<string> result = new ResultInfo<string>();
        //    string Query = string.Empty;
        //    using (SqlConnection Connection = new SqlConnection(conn)){
        //        try{
        //            if (Connection.State == ConnectionState.Broken ||
        //                   Connection.State == ConnectionState.Closed){
        //                if (Btnaccpt == true){
        //                 Query = @"update [teacher].[ClassTeacher_OnlineForum_GroupMember]
        //                            set [teacher].[ClassTeacher_OnlineForum_GroupMember].IsRequestPending=0
        //                                where [teacher].[ClassTeacher_OnlineForum_GroupMember].Forum_ID_FK=@forumid
        //                                and [teacher].[ClassTeacher_OnlineForum_GroupMember].TeacherID_FK=@teacherid
        //                                    and [teacher].[ClassTeacher_OnlineForum_GroupMember].SchoolID=@schoolid
        //                                    and [teacher].[ClassTeacher_OnlineForum_GroupMember].IsAdmin=@IsAdmin
        //                                        and [teacher].[ClassTeacher_OnlineForum_GroupMember].Status=1";
        //                    Connection.Open();
        //                    SqlCommand Command = new SqlCommand(){
        //                        CommandText = Query,
        //                        Connection = Connection
        //                    };
        //                    Command.Parameters.AddWithValue("@forumid", ForumId);
        //                    Command.Parameters.AddWithValue("@IsAdmin", 0);
        //                    Command.Parameters.AddWithValue("@teacherid", TeacherId);
        //                    Command.Parameters.AddWithValue("@schoolid", SchoolId);
        //                    if (Command.ExecuteNonQuery() > 0){
        //                        result.ErrorCode = 200;
        //                        result.Description = "Accepted|Request accepted successfully.";
        //                    }

        //                }else{
        //                    Query = @"delete from [teacher].[ClassTeacher_OnlineForum_GroupMember]
        //                                where [teacher].[ClassTeacher_OnlineForum_GroupMember].Forum_ID_FK=@forumid
        //                                and [teacher].[ClassTeacher_OnlineForum_GroupMember].TeacherID_FK=@teacherid
        //                                    and [teacher].[ClassTeacher_OnlineForum_GroupMember].SchoolID=@schoolid
        //                                    and [teacher].[ClassTeacher_OnlineForum_GroupMember].IsAdmin=0
        //                                        and [teacher].[ClassTeacher_OnlineForum_GroupMember].Status=1";
        //                    Connection.Open();
        //                    SqlCommand Command = new SqlCommand(){
        //                        CommandText = Query,
        //                        Connection = Connection
        //                    };
        //                    Command.Parameters.AddWithValue("@forumid", ForumId);
        //                    Command.Parameters.AddWithValue("@IsAdmin", 0);
        //                    Command.Parameters.AddWithValue("@teacherid", TeacherId);
        //                    Command.Parameters.AddWithValue("@schoolid", SchoolId);
        //                        if (Command.ExecuteNonQuery() > 0){
        //                            result.ErrorCode = 200;
        //                            result.Description = "Declined|Request declined.";
        //                        }
        //                    }
        //                }
        //            }
        //        catch(SqlException exp){
        //            WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
        //            result.ErrorCode = 500;
        //            result.Description = "Failed|" + exp.Message;
        //        }
        //    }
        //    return result;
        //}

        #endregion
        #region Curriculum
        public ResultInfo<TeachersCurriculum> GetSubjectDetailsClassWise(TeachersCurriculum Info) {
            ResultInfo<TeachersCurriculum> result = new ResultInfo<TeachersCurriculum>();
            string Query = @"select admin.Syllabus_Subject.ID,
                            ISNULL(admin.Syllabus_Subject.Subject,'') Subject,
                            CONVERT(varchar,admin.Syllabus_Subject.CreatedDate,121) CreatedDate
                                from admin.Syllabus_Subject inner join admin.AssignPrivilegeClassTeacher
                                on admin.AssignPrivilegeClassTeacher.LevelID=admin.Syllabus_Subject.Level_ID
                                where admin.Syllabus_Subject.School_ID=@SchoolId and 
                                    admin.AssignPrivilegeClassTeacher.TeacherID=@TeacherId and
                                    admin.AssignPrivilegeClassTeacher.ClassID=@ClassId
                                        and admin.Syllabus_Subject.Status=1 and
                                        admin.AssignPrivilegeClassTeacher.Status=1";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    SqlCommand Command = new SqlCommand(){
                        CommandText = Query,
                        Connection = Connection
                    };
                    if (Connection.State == ConnectionState.Broken ||
                                Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        Command.Parameters.AddWithValue("@SchoolId",Info.SchoolId);
                        Command.Parameters.AddWithValue("@TeacherId",Info.Teacher_ID_FK);
                        Command.Parameters.AddWithValue("@ClassId", Info.ClassID);
                        result.Info = new TeachersCurriculum();
                        result.Info.Subjectdetails = new List<SubjectDetails>();
                        SqlDataReader reader = Command.ExecuteReader();
                        if(reader.HasRows) {
                            while (reader.Read()){
                                SubjectDetails temp = new SubjectDetails() {
                                    ID=Convert.ToInt32(reader["ID"]),
                                    Subject=Convert.ToString(reader["Subject"])
                                };
                                result.Info.Subjectdetails.Add(temp);
                            }
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success| subject's list successfully retrieved";
                    }
                }
                catch (SqlException exp){
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }return result;
        }
        public ResultInfo<TeachersCurriculum> GetCurriculumClassWise(TeachersCurriculum Info) {
            ResultInfo<TeachersCurriculum> result = new ResultInfo<TeachersCurriculum>();
            string Query = @"select st.Topic_ID,ISNULL(st.Topic_Name,'') Topic_Name,
                            CONVERT(nvarchar,cr.StartDate,121) StartDate,
                            CONVERT(nvarchar,cr.EndDate,121) EndDate,
                            ISNULL(td.Name,'') AssignedTo,
                                ISNULL(wd.Days,'') Days,
                                CONVERT(nvarchar,st.CreatedDate,121) CreatedDate
                                from admin.Curriculum cr right outer join
                                    admin.Syllabus_Topic st on st.Topic_ID=cr.TopicId_FK
                                    right outer join Teacher.TeachersDetails td
                                    on td.UID_FK=cr.Teacher_ID_FK
                                        inner join admin.WeekDays wd
                                        on wd.ID=cr.Week_ID_FK
                                        where cr.SubjectId_FK=@SubjectId and cr.Class_ID_FK=@ClassId and
                                            cr.SchoolId=@SchoolId and cr.Status=1";
            using (SqlConnection Connection = new SqlConnection(conn)) {
                try{
                    SqlCommand Command = new SqlCommand(){
                        CommandText = Query,
                        Connection = Connection
                    };if (Connection.State == ConnectionState.Broken ||
                                Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        Command.Parameters.AddWithValue("@SubjectId", Info.SubjectID_FK);
                        Command.Parameters.AddWithValue("@ClassId", Info.ClassID);
                        Command.Parameters.AddWithValue("@SchoolId", Info.SchoolId);
                        result = new ResultInfo<TeachersCurriculum>();
                        result.Info = new TeachersCurriculum();
                        result.Info.CurriculumDetails = new List<TeachersCurriculum>();

                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows){
                            result.Info.SubjectName = Info.SubjectName;
                            while (reader.Read()){
                                TeachersCurriculum temp = new TeachersCurriculum() {
                                    Topic_ID_FK = Convert.ToInt64(reader["Topic_ID"]),
                                    Topic_Name=Convert.ToString(reader["Topic_Name"]),
                                    StartDate=Convert.ToString(reader["StartDate"]),
                                    EndDate=Convert.ToString(reader["EndDate"]),
                                    TeacherName=Convert.ToString(reader["AssignedTo"]),
                                    Week_Name=Convert.ToString(reader["Days"]),
                                    CreatedDate=Convert.ToString(reader["CreatedDate"])
                                };
                                result.Info.CurriculumDetails.Add(temp);
                            }
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success|Topic's list successfully retrieved";
                    }
                }
                catch (SqlException exp){
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }return result;
        }
        #endregion
        #region Feedback
        public ResultInfo<ClassTeacherAssignmentDetails> GetTeachersAssignmentSubjectWise(ClassTeacherAssignmentDetails Info){
            ResultInfo<ClassTeacherAssignmentDetails> result = new ResultInfo<ClassTeacherAssignmentDetails>();
            string Query = @"select ta.ID,ISNULL(ta.AssignmentName,'') AssignmentName,
                        CONVERT(nvarchar,ta.StartDate,121) StartDate,CONVERT(nvarchar,ta.EndDate,121) EndDate,
                            CONVERT(nvarchar,ta.CreatedDate,121) CreatedDate,ISNULL(td.Name,'') as Assignedby from teacher.AssignmentDetails ta
                            right outer join teacher.TeachersDetails td on td.UID_FK=ta.TeacherID_FK where ta.SubjectID_FK=@SubjectId and 
                               ta.TopicID_FK=@TopicId and ta.ClassID_FK=@ClassId and ta.schoolID=@SchoolId and ta.Status=1";
            using (SqlConnection Connection = new SqlConnection(conn)){
                try{
                    SqlCommand Command = new SqlCommand(){
                        CommandText = Query,
                        Connection = Connection
                     }; if (Connection.State == ConnectionState.Broken ||
                                 Connection.State == ConnectionState.Closed){
                        Connection.Open();
                        Command.Parameters.AddWithValue("@SubjectId", Info.SubjectID_FK);
                        Command.Parameters.AddWithValue("@TopicId", Info.TopicID_FK);
                        Command.Parameters.AddWithValue("@ClassId", Info.ClassID_FK);
                        Command.Parameters.AddWithValue("@SchoolId", Info.SchoolID);
                        result.Info = new ClassTeacherAssignmentDetails();
                        result.Info.AssignmentDetails = new List<ClassTeacherAssignmentDetails>();
                        SqlDataReader reader = Command.ExecuteReader();
                        if(reader.HasRows){
                            while (reader.Read()){
                                ClassTeacherAssignmentDetails temp = new ClassTeacherAssignmentDetails() {
                                    ID=Convert.ToInt64(reader["ID"]),
                                    AssignmentName = Convert.ToString(reader["AssignmentName"]),
                                    StartDate = Convert.ToString(reader["StartDate"]),
                                    EndDate =Convert.ToString(reader["EndDate"]),
                                    TeacherName=Convert.ToString(reader["Assignedby"]),
                                    CreatedDate = Convert.ToString(reader["CreatedDate"])
                                }; result.Info.AssignmentDetails.Add(temp);
                            }
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success | Assignment's list successfully retrieved";
                    }
                }
                catch (SqlException exp){
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }return result;
        }
        public ResultInfo<ClassTeacherAssignmentDetails> GetTeacherAssignmentDetails(ClassTeacherAssignmentDetails Info){
            ResultInfo<ClassTeacherAssignmentDetails> result = new ResultInfo<ClassTeacherAssignmentDetails>();
            string Query = @"select ISNULL(ta.AssignmentName,'') AssignmentName,
                            ISNULL(aa.Topic_Name,'') Topic_Name,ISNULL(s.Semester,'') Semester,
                            ISNULL(td.Name,'') Assignedby,CONVERT(nvarchar,ta.StartDate,121) StartDate,
                                CONVERT(nvarchar,ta.EndDate,121) EndDate,CONVERT(nvarchar,ta.CreatedDate,121) CreatedDate,
                                CAST(ta.FullMarks as float) FullMarks,MAX(tf.Obtained_Marks) as HighestMarks,
                                    MIN(tf.Obtained_Marks) as LowestMarks,CAST(AVG(tf.Obtained_Marks) as decimal(18,2)) as AvgMarks
                                    from teacher.AssignmentDetails ta right outer join
                                    admin.Syllabus_Topic aa on aa.Topic_ID=ta.TopicID_FK
                                        right outer join admin.Semester s on s.ID=ta.SemesterID_FK
                                        right outer join teacher.TeachersDetails td on td.UID_FK=ta.TeacherID_FK
                                        inner join teacher.TeachersAssignmentFeedback tf on tf.AssignmentID_FK=ta.ID
                                            where ta.ID=@AssignmentId and ta.SubjectID_FK=@SubjectId and ta.ClassID_FK=@ClassId and 
                                            ta.SchoolID=@SchoolId and ta.Status=1 group by ta.AssignmentName,aa.Topic_Name,
                                                s.Semester,td.Name,ta.StartDate,ta.EndDate,ta.CreatedDate,ta.FullMarks";
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                try
                {
                    SqlCommand Command = new SqlCommand()
                    {
                        CommandText = Query,
                        Connection = Connection
                    }; if (Connection.State == ConnectionState.Broken ||
                                Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                        Command.Parameters.AddWithValue("@AssignmentId", Info.ID);
                        Command.Parameters.AddWithValue("@SubjectId", Info.SubjectID_FK);
                        Command.Parameters.AddWithValue("@ClassId", Info.ClassID_FK);
                        Command.Parameters.AddWithValue("@SchoolId", Info.SchoolID);
                        result.Info = new ClassTeacherAssignmentDetails();
                        result.Info.AssignmentFeedbackDetails = new List<ClassTeacherAssignmentFeedbackDetails>();
                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Info.AssignmentName = Convert.ToString(reader["AssignmentName"]);
                                result.Info.Topic_Name = Convert.ToString(reader["Topic_Name"]);
                                result.Info.Semester = Convert.ToString(reader["Semester"]);
                                result.Info.TeacherName = Convert.ToString(reader["Assignedby"]);
                                result.Info.StartDate = Convert.ToString(reader["StartDate"]);
                                result.Info.EndDate = Convert.ToString(reader["EndDate"]);
                                result.Info.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                                result.Info.FullMarks = float.Parse(Convert.ToString(reader["FullMarks"]));
                                result.Info.HighestMarks = Convert.ToDecimal(reader["HighestMarks"]);
                                result.Info.LowestMarks = Convert.ToDecimal(reader["LowestMarks"]);
                                result.Info.AverageMarks = Convert.ToDecimal(reader["AvgMarks"]);
                                result.Info.AssignmentFeedbackDetails = GetStudentAssignmentFeedback(Info);

                                break;
                            }
                        }
                    }
                }
                catch (SqlException exp)
                {
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }
            return result;
        }
        private List<ClassTeacherAssignmentFeedbackDetails> GetStudentAssignmentFeedback(ClassTeacherAssignmentDetails Info)
        {
            List<ClassTeacherAssignmentFeedbackDetails> result = new List<ClassTeacherAssignmentFeedbackDetails>();
            string Query = @"select ISNULL(ss.Stu_Name,'') Stu_Name,
                            ISNULL(ss.Stu_RegistrationNo,'') Stu_RegistrationNo, tf.Obtained_Marks,
                            tf.Rating,tf.FeedbackComment from teacher.TeachersAssignmentFeedback
                                tf left outer join studentinfo.Student_Details ss on
                                    tf.StudentID_FK=ss.Stu_ID where tf.AssignmentID_FK=@AssignmentId
                                      and tf.SchoolID=@SchoolId and ss.Class_ID=@ClassId and tf.Status=1";
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                try
                {
                    SqlCommand Command = new SqlCommand()
                    {
                        CommandText = Query,
                        Connection = Connection
                    }; if (Connection.State == ConnectionState.Broken ||
                                Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                        Command.Parameters.AddWithValue("@AssignmentId", Info.ID);
                        Command.Parameters.AddWithValue("@SchoolId", Info.SchoolID);
                        Command.Parameters.AddWithValue("@ClassId", Info.ClassID_FK);
                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ClassTeacherAssignmentFeedbackDetails temp = new ClassTeacherAssignmentFeedbackDetails()
                                {
                                    StudentName = Convert.ToString(reader["Stu_Name"]),
                                    StudentRegNo = Convert.ToString(reader["Stu_RegistrationNo"]),
                                    Obtained_Marks = Convert.ToDecimal(reader["Obtained_Marks"]),
                                    FeebackComment = Convert.ToString(reader["FeedbackComment"]),
                                    Rating = Convert.ToDecimal(reader["Rating"]),
                                }; result.Add(temp);
                            }
                        }
                    }
                }
                catch (SqlException exp)
                {
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                }
            }
            return result;
        }
        public ResultInfo<ClassTeacherAssignmentDetails> GetSubjectWiseTopic(ClassTeacherAssignmentDetails Info){

            ResultInfo<ClassTeacherAssignmentDetails> result = new ResultInfo<ClassTeacherAssignmentDetails>();
            string Query = @"select st.Topic_ID,ISNULL(st.Topic_Name,'') Topic_Name,
                            CONVERT(nvarchar,st.CreatedDate,121) CreatedDate
                                from admin.Syllabus_Topic st where st.Subject_ID_FK=@SubjectId
                                    and st.SchoolID=@SchoolId and st.IsActive=1";
            using (SqlConnection Connection = new SqlConnection(conn))
            {
                try
                {
                    SqlCommand Command = new SqlCommand()
                    {
                        CommandText = Query,
                        Connection = Connection
                    }; if (Connection.State == ConnectionState.Broken ||
                                Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                        Command.Parameters.AddWithValue("@SubjectId", Info.SubjectID_FK);
                        Command.Parameters.AddWithValue("@SchoolId", Info.SchoolID);
                        result.Info = new ClassTeacherAssignmentDetails();
                        result.Info.TopicDetails = new List<ClassTeacherTopicDetails>();
                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ClassTeacherTopicDetails temp = new ClassTeacherTopicDetails()
                                {
                                    Topic_ID = Convert.ToInt64(reader["Topic_ID"]),
                                    Topic_Name = Convert.ToString(reader["Topic_Name"]),
                                    CreatedDate = Convert.ToString(reader["CreatedDate"])
                                }; result.Info.TopicDetails.Add(temp);
                            }
                        }
                        result.ErrorCode = 200;
                        result.Description = "Success | Topic's list successfully retrieved";
                    }
                }
                catch (SqlException exp)
                {
                    WriteLogFile.WriteErrorLog(string.Format("Errorlog{0}.txt", DateTime.Today), exp.ErrorCode + ':' + exp.Message);
                    result.ErrorCode = 500;
                    result.Description = "Failed|" + exp.Message;
                }
            }
            return result;
        }
        #endregion
    }
}