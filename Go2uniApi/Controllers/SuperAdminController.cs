using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2uniApi.Controllers
{
    public class SuperAdminController : Controller
    {
        #region SUPER_ADMIN EDIT AND UPDATE


        #region SUPER ADMIN EDIT PROFILE
        /// <summary>
        /// done by Sneha
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SuperEditProfile(int ID)

        {
            ResultInfo<SupEditprofile> ResultInfo = new ResultInfo<SupEditprofile>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SupEditprofile obj = new SupEditprofile();
            SuperAdmin SupAdm = new SuperAdmin();
            try
            {

                //  ResultInfo.Info = SupAdm.GetDetailsOFSuperAdmin(ID);
                if (ID > 0)
                {
                    obj = SupAdm.GetDetailsOFSuperAdmin(ID);
                    ResultInfo.Info = obj;

                }
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details Of SuperAdmin";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {


            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);


        }
        #endregion

        [HttpPost]
        public JsonResult SuperUpdateProfile(SupEditprofile Info)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateSuperAdminProfile(Info);
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

        #endregion

        #region Students And Redirection to students profile page
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        #region GET ALL STUDENTS
        [HttpPost]

        public JsonResult Student()
        {
            ResultInfo<SupGetAllStudent> ResultInfo = new ResultInfo<SupGetAllStudent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                SupGetAllStudent temp = new SupGetAllStudent();
                SuperAdmin obj = new SuperAdmin();
                temp.NameAndRegs = obj.GetAllStuList();
                temp.ClassSpecificatons = obj.GetJuniorSeniorList();
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get ALL Students ";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region STUDENT PROFILE
        [HttpPost]

        public JsonResult StudentProfile(long StudentId)
        {

            ResultInfo<StudentProfile> ResultInfo = new ResultInfo<StudentProfile>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                StudentProfile profObj = new StudentProfile();
                // StudentProfile temp = new StudentProfile();
                SuperAdmin obj = new SuperAdmin();
                profObj = obj.GetProfDetBYStuID(StudentId);

                ResultInfo.Info = profObj;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Profile Info By StudentID";
                    ResultInfo.Status = true;
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GET ALL STUDENTS BY DIV ID
        [HttpPost]
        public JsonResult GetStudentsByDivId(int ID)
        {
            ResultInfo<List<NameAndRegs>> ResultInfo = new ResultInfo<List<NameAndRegs>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<NameAndRegs> nm = new List<NameAndRegs>();
            SuperAdmin PageObj = new SuperAdmin();
            if (ID > 0)
            {

                nm = PageObj.GetStudentsByDivId(ID);
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
        public JsonResult GetStudents()
        {
            ResultInfo<List<NameAndRegs>> ResultInfo = new ResultInfo<List<NameAndRegs>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<NameAndRegs> nm = new List<NameAndRegs>();
            SuperAdmin PageObj = new SuperAdmin();

            nm = PageObj.GetAllStudents();
            ResultInfo.Info = nm;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion

        public JsonResult GetClassByDivID(int ID)
        {
            ResultInfo<List<ClassInfo>> ResultInfo = new ResultInfo<List<ClassInfo>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
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

        #region GET STUDENTS BY CLASS ID

        public JsonResult GetStudentsByClassId(int ClassID)
        {
            ResultInfo<List<NameAndRegs>> ResultInfo = new ResultInfo<List<NameAndRegs>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            if (ClassID > 0)
            {
                ResultInfo.Info = PageObj.GetStudentsByClassId(ClassID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TeacherBySuperadmin

        public JsonResult Addteacher()
        {
            ResultInfo<AddTeacher> ResultInfo = new ResultInfo<AddTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            AddTeacher temp = new AddTeacher();
            SuperAdmin PageObj = new SuperAdmin();

            temp.Login_Type = PageObj.GetLoginType();
            temp.GenderInfo = PageObj.GetAllGender();

            //temp.AddTeacherDetails = PageObj.GetTeachersDetails();


            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubForAutosuggestion(string Keyword)
        {
            ResultInfo<List<Subject_Syllabus>> ResultInfo = new ResultInfo<List<Subject_Syllabus>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            if (Keyword != "")
            {
                ResultInfo.Info = PageObj.SubForAutosuggestion(Keyword);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTeacherBySuperAdmin(AddTeacherDetails Data)
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
                    SuperAdmin Obj = new SuperAdmin();


                    ResultInfo.Info = Obj.AddTeacherBySuperAdmin(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insertion Sucess ";
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
        public JsonResult Teacher(long SID)

        {
            ResultInfo<AddTeacher> ResultInfo = new ResultInfo<AddTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            AddTeacher temp = new AddTeacher();
            SuperAdmin PageObj = new SuperAdmin();

            temp.AddTeacherDetails = PageObj.GetTeacherDetails(SID);

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteTeacherInfo(long UserId)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (UserId > 0)
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteTeacherInfo(UserId);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditTeacher(long UID)
        {
            ResultInfo<EditTeacher> ResultInfo = new ResultInfo<EditTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            SuperAdmin PageObj = new SuperAdmin();
            EditTeacher temp = new EditTeacher();

            if (UID != 0)
            {
                temp = PageObj.EditTeacher(UID);

                temp.Login_Type = PageObj.GetLoginType();

                temp.GenderInfo = PageObj.GetAllGender();

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
        public JsonResult UpdateTeacherBySuperAdmin(EditTeacher Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateTeacherBySuperAdmin(Data);
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

        //teacher view
        [HttpPost]
        public JsonResult ViewTeacherDetails(long ID)
        {
            ResultInfo<AddTeacherDetails> ResultInfo = new ResultInfo<AddTeacherDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            AddTeacherDetails temp = new AddTeacherDetails();
            SuperAdmin PageObj = new SuperAdmin();
            temp = PageObj.ViewDetailsOfTeacher(ID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region StudentTab

        public JsonResult Addstudent()
        {
            AddStudent temp = new AddStudent();
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo<AddStudent> ResultInfo = new ResultInfo<AddStudent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                temp.Division = PageObj.GetAllDivision();
                temp.GenderInfo = PageObj.GetAllGender();
                //temp.Level = PageObj.GetLevel(ID);
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success! Get Result ";
                    ResultInfo.Status = true;
                }

            }
            catch (Exception ex)
            {

            }


            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }





        #endregion

        #region CLASS TAB

        public JsonResult AddClass()
        {
            ResultInfo<AddClass> ResultInfo = new ResultInfo<AddClass>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            AddClass temp = new AddClass();
            SuperAdmin PageObj = new SuperAdmin();


            temp.Divisions = PageObj.GetDivisionList();

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult InsertClassInfo(AddClassInfo Data)
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
                    SuperAdmin Obj = new SuperAdmin();

                    bool res = Obj.ChkClass(Data);
                    if (!res)
                    {
                        ResultInfo.Info = Obj.InsertClassInfo(Data);
                        if (ResultInfo.Info != null)
                        {
                            ResultInfo.Description = "Success|Insertion Sucess ";
                            ResultInfo.Status = true;
                        }
                    }
                    else
                    {
                        ResultInfo.Info = "Failed|Already exists this class.";
                        ResultInfo.Description = "Failed|Already exists this class.";
                        ResultInfo.Status = true;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        #region CLASS LIST

        [HttpPost]
        public JsonResult GetClassList(string schoolId)
        {
            ResultInfo<Class> ResultInfo = new ResultInfo<Class>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                Class temp = new Class();
                SuperAdmin obj = new SuperAdmin();
                temp.ClassInfo = obj.ClassList(schoolId);

                ResultInfo.Info = temp;

                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| List Of Classes Sucessfully Binded";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region ramashree
        [HttpPost]
        public JsonResult DeleteClassInfo(string Id)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (Id != null && Id != "")
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteClass(Convert.ToInt64(Id));
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
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
        public JsonResult GetClass(long ID)
        {
            ResultInfo<EditClass> ResultInfo = new ResultInfo<EditClass>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            EditClass temp = new EditClass();
            SuperAdmin PageObj = new SuperAdmin();
            ClassDetails cd = new ClassDetails();

            cd = PageObj.SelectClassById(ID);
            temp.Divisions = PageObj.GetDivisionList();
            temp.Levels = PageObj.GetlevelListByDivID(cd.Div_ID);
            temp.cldetls = cd;
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }
        // class view
        [HttpPost]
        public JsonResult ViewClass(long ID)
        {
            ResultInfo<ClassDetails> ResultInfo = new ResultInfo<ClassDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Class temp = new Class();
            SuperAdmin PageObj = new SuperAdmin();
            ClassDetails cdet = new ClassDetails();
            cdet = PageObj.CountNoStudents(ID);
            temp.ClassDetails = cdet;
            ResultInfo.Info = cdet;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateClassInfo(AddClassInfo Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateClassInfo(Data);
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

        [HttpPost]
        public JsonResult AddStudentDetails(AddStudentDetails Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.InsertStudentDetails(Data);
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
        public JsonResult GetLevelListByDivisionID(int ID)
        {
            ResultInfo<List<Level>> ResultInfo = new ResultInfo<List<Level>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Level> nm = new List<Level>();
            SuperAdmin PageObj = new SuperAdmin();
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

        public JsonResult GetClassForAddStudent(int ID)
        {
            ResultInfo<List<Class_Details>> ResultInfo = new ResultInfo<List<Class_Details>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Class_Details> nm = new List<Class_Details>();
            SuperAdmin PageObj = new SuperAdmin();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetClass(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sneha

        [HttpPost]
        public JsonResult EditStudentDetails(long ID)

        {
            ResultInfo<AddStudentDetails> ResultInfo = new ResultInfo<AddStudentDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            AddStudentDetails obj = new AddStudentDetails();
            Division div = new Division();
            Level lvl = new Level();
            SuperAdmin SupAdm = new SuperAdmin();
            try
            {
                if (ID > 0)
                {
                    obj = SupAdm.GetDetailsOfStudent(ID);
                    if (obj.DivisionID > 0)
                    {
                        obj.Levels = SupAdm.GetlevelListByDivID(obj.DivisionID);
                    }
                    if (obj != null && obj.Level_ID > 0)
                    {
                        obj.ClassStream = SupAdm.GetStreamList(obj.Level_ID);
                    }
                    if (obj.Level_ID > 0)
                    {
                        obj.ClassDetails = SupAdm.GetAllClassSt(obj.Level_ID);
                    }
                    obj.GenderInfo = SupAdm.GetAllGender();
                    obj.Divisions = SupAdm.GetDivisionList();
                    ResultInfo.Info = obj;

                }
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Student details found";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ViewStudentDetails(long ID)
        {
            ResultInfo<StudentListing> ResultInfo = new ResultInfo<StudentListing>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            StudentListing temp = new StudentListing();
            SuperAdmin PageObj = new SuperAdmin();
            temp = PageObj.ViewDetailsOfStudent(ID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStudentRecord(AddStudentDetails Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            if (Info != null)
            {
                SuperAdmin obj = new SuperAdmin();
                ResultInfo.Info = obj.UpdateUserTable(Info);
                if (Info != null)
                {
                    SuperAdmin PageObj = new SuperAdmin();
                    ResultInfo.Info = PageObj.UpdateStudentRecord(Info);
                }
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success! Get student record ";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteStudentInfo(long UID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                bool var = false;
                if (UID > 0)
                {
                    SuperAdmin Obj = new SuperAdmin();
                    var = Obj.DeleteStudentInfo(UID);
                    if (var == true)
                    {
                        ResultInfo.Info = "Success| required student record deleted";
                        ResultInfo.Description = "Success";
                        ResultInfo.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StudentList(long School_ID)
        {
            ResultInfo<AddStudentDetails> ResultInfo = new ResultInfo<AddStudentDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                AddStudentDetails temp = new AddStudentDetails();
                SuperAdmin obj = new SuperAdmin();

                temp.StudentListing = obj.StudentList(School_ID);
                temp.Divisions = obj.GetDivisionList();
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get ALL Students ";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception)
            {

            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetStreamForAddStudent(int ID)
        {
            ResultInfo<List<ClassStream>> ResultInfo = new ResultInfo<List<ClassStream>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
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
        public JsonResult GetClassBylevelID(int ID)
        {
            ResultInfo<List<ClassDetails>> ResultInfo = new ResultInfo<List<ClassDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
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

        public JsonResult GetSubjectByLevelIDSub(int ID)
        {
            ResultInfo<List<Syllabus_Subject>> ResultInfo = new ResultInfo<List<Syllabus_Subject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            if (ID > 0)
            {
                ResultInfo.Info = PageObj.GetAllSubject(ID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ramashree 05/12/2018
        [HttpPost]
        public JsonResult EditProfileSuperAdmin(long ID)

        {
            ResultInfo<SuperAdminEditprofile> ResultInfo = new ResultInfo<SuperAdminEditprofile>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdminEditprofile obj = new SuperAdminEditprofile();
            SuperAdmin SupAdm = new SuperAdmin();
            try
            {
                if (ID > 0)
                {
                    obj = SupAdm.GetDetailsOFEditProfileSuperAdmin(ID);
                    obj.GenderInfo = SupAdm.GetAllGender();
                    ResultInfo.Info = obj;
                }
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| Get Details Of SuperAdmin";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SuperAdminUpdateProfile(SuperAdminEditprofile Info)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateSuperAdminEditProfile(Info);
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
        #endregion

        #region Subject Tab, SM
        [HttpPost]
        public JsonResult GetSubjectList(string schoolId)
        {
            ResultInfo<SubjectTab> ResultInfo = new ResultInfo<SubjectTab>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                SubjectTab temp = new SubjectTab();
                SuperAdmin obj = new SuperAdmin();
                temp.SubjectList = obj.SubjectList(schoolId);
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success| List Of Subjects Sucessfully Binded";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult InsertSubject(AddsubjectInfo Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    bool res = Obj.Chksubj(Data);
                    if (!res)
                    {
                        ResultInfo.Info = Obj.InsertSubject(Data);
                        if (ResultInfo.Info != null)
                        {
                            ResultInfo.Description = "Success|Insertion Sucess";
                            ResultInfo.Status = true;
                        }
                    }
                    else
                    {
                        ResultInfo.Info = "Failed| This Subject previouly entered";
                        ResultInfo.Description = "Failed| This Subject previously entered";
                        ResultInfo.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddSub()
        {
            ResultInfo<AddSub> ResultInfo = new ResultInfo<AddSub>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            AddSub temp = new AddSub();
            SuperAdmin PageObj = new SuperAdmin();
            temp.Levels = PageObj.GetAllLevel();
            temp.Depertment_Subject = PageObj.GetAllDepertment();
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        //sub edit
        [HttpPost]
        public JsonResult EditSubject(int ID)
        {
            ResultInfo<EditSubject> ResultInfo = new ResultInfo<EditSubject>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            EditSubject temp = new EditSubject();
            SuperAdmin PageObj = new SuperAdmin();
            SubjectDetails sub = new SubjectDetails();
            sub = PageObj.SubjectEdit(ID);
            temp.Levels = PageObj.GetAllLevel();
            temp.Depertment_Subject = PageObj.GetAllDepertment();
            temp.SubjectDetails = sub;
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }       
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSubjectInfo(AddsubjectInfo Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateSubInfo(Data);
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

        [HttpPost]
        public JsonResult DeleteSubjectInfo(string Id)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (Id != null && Id != "")
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteSubject(Convert.ToInt32(Id));
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess";
                        ResultInfo.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        // subject view
        [HttpPost]
        public JsonResult ViewSubject(int ID)
        {
            ResultInfo<SubjectDetails> ResultInfo = new ResultInfo<SubjectDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SubjectTab temp = new SubjectTab();
            SuperAdmin PageObj = new SuperAdmin();
            SubjectDetails subdet = new SubjectDetails();
            subdet = PageObj.ViewSubjectDetails(ID);
            temp.SubjectbyId = subdet;
            ResultInfo.Info = subdet;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Super AddParentBySuperAdmin

        [HttpPost]
        public JsonResult AddParentBySuperAdmin(ParentDetails Data)
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
                    SuperAdmin Obj = new SuperAdmin();


                    ResultInfo.Info = Obj.AddParentBySuperAdmin(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success| Insertion Sucess ";
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
        public JsonResult Parent(long SID)

        {
            ResultInfo<Parent> ResultInfo = new ResultInfo<Parent>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Parent temp = new Parent();
            SuperAdmin PageObj = new SuperAdmin();

            temp.ParentDetails = PageObj.GetParentDetails(SID);

            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get Results";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditParent(long UserID, long DetailsID)
        {
            ResultInfo<EditParent> ResultInfo = new ResultInfo<EditParent>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            SuperAdmin PageObj = new SuperAdmin();
            EditParent temp = new EditParent();

            if (UserID != 0 && DetailsID != 0)
            {
                temp = PageObj.EditParent(UserID, DetailsID);
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
        public JsonResult UpdateParent(EditParent Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateParent(Data);
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

        [HttpPost]
        public JsonResult DeleteParent(long UserID, long DetailsID)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (UserID != 0 && DetailsID != 0)
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteParent(UserID, DetailsID);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
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

        #region Ramashree 07/12/2018
        [HttpPost]
        public JsonResult ViewParent(int ID)
        {
            ResultInfo<ParentDetails> ResultInfo = new ResultInfo<ParentDetails>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            Parent temp = new Parent();
            SuperAdmin PageObj = new SuperAdmin();
            ParentDetails subdet = new ParentDetails();
            subdet = PageObj.ViewParentDetails(ID);
            temp.ParentbyId = subdet;
            ResultInfo.Info = subdet;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }

            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Assiogn Privilege Ramashree   
        public JsonResult GetTeacher(long SchoolID)
        {
            AssignPrivilege temp = new AssignPrivilege();
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo<AssignPrivilege> ResultInfo = new ResultInfo<AssignPrivilege>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                temp.TeacherListing = PageObj.GetTeacher(SchoolID);
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success! Get Result ";
                    ResultInfo.Status = true;
                }           
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDepertment(long SchoolID)
        {
            ResultInfo<List<Depertment_Subject>> ResultInfo = new ResultInfo<List<Depertment_Subject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Depertment_Subject> nm = new List<Depertment_Subject>();
            SuperAdmin PageObj = new SuperAdmin();
            nm = PageObj.GetDepertment(SchoolID);
            ResultInfo.Info = nm;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubject(long SchoolID)
        {
            ResultInfo<List<Syllabus_Subject>> ResultInfo = new ResultInfo<List<Syllabus_Subject>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Syllabus_Subject> nm = new List<Syllabus_Subject>();
            SuperAdmin PageObj = new SuperAdmin();
            nm = PageObj.GetSubject(SchoolID);
            ResultInfo.Info = nm;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDivision(long SchoolID)
        {
            ResultInfo<List<Division>> ResultInfo = new ResultInfo<List<Division>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Division> nm = new List<Division>();
            SuperAdmin PageObj = new SuperAdmin();

            nm = PageObj.GetDivision(SchoolID);
            ResultInfo.Info = nm;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult InsertAssign(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    if(Data.IschkedVal_hod == "HOD") 
                    {
                        ResultInfo.Info = Obj.InsertAssignHod(Data);
                    }
                    if(Data.IschkedVal_classTeacher == "CLASSTEACHER")
                    {
                        ResultInfo.Info = Obj.InsertAssignClassTeacher(Data);
                    }
                    if(Data.IschkedVal_subjectTeacher == "SUBJECTTEACHER")
                    {
                        Data.DeptID_FK = Obj.GetDeptIDforSubjectTeacher(Data.SubjectID_SubjectTeacher);
                        ResultInfo.Info = Obj.InsertAssignSubjectTeacher(Data);
                    }
                 
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
        #endregion

        #region Assign Privilege List
        public JsonResult GetTeacherType(long SchoolID)
        {
            AssignPrivilege temp = new AssignPrivilege();
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo<AssignPrivilege> ResultInfo = new ResultInfo<AssignPrivilege>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            try
            {
                temp.TeacherListing = PageObj.GetTeacherType();
                ResultInfo.Info = temp;
                if (ResultInfo.Info != null)
                {
                    ResultInfo.Description = "Success! Get Result ";
                    ResultInfo.Status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeacherTypeListHod(long TeacherTypeID)
        {
            ResultInfo<List<TeacherListing>> ResultInfo = new ResultInfo<List<TeacherListing>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            if (TeacherTypeID==2)
            {
                ResultInfo.Info = PageObj.GetTeacherTypeListHod(TeacherTypeID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeacherTypeListClassTeacher(long TeacherTypeID)
        {
            ResultInfo<List<TeacherListing>> ResultInfo = new ResultInfo<List<TeacherListing>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            if (TeacherTypeID == 3)
            {
                ResultInfo.Info = PageObj.GetTeacherTypeListClassTeacher(TeacherTypeID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeacherTypeListSubjectTeacher(long TeacherTypeID)
        {
            ResultInfo<List<TeacherListing>> ResultInfo = new ResultInfo<List<TeacherListing>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            if (TeacherTypeID == 6)
            {
                ResultInfo.Info = PageObj.GetTeacherTypeListSubjectTeacher(TeacherTypeID);
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditHod(long UID_FK)
        {
            ResultInfo<EditHod> ResultInfo = new ResultInfo<EditHod>()
            {
                Status = false,
                Description = "Failed|Login"
            };

            EditHod temp = new EditHod();
            SuperAdmin PageObj = new SuperAdmin();
            temp = PageObj.EditHod(UID_FK);
            temp.Depertment_Subject = PageObj.GetDepertmentList();
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateHod(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateHod(Data);
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

        [HttpPost]
        public JsonResult DeleteHod(string Id)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (Id != null && Id != "")
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteHod(Convert.ToInt64(Id));
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
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
        public JsonResult UpdateStatusAssignHod(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateStatusAssignHod(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull ";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatusUnAssignHod(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateStatusUnAssignHod(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull ";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditClassTeacher(long UID_FK)
        {
            ResultInfo<EditClassTeacher> ResultInfo = new ResultInfo<EditClassTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            EditClassTeacher temp = new EditClassTeacher();
            SuperAdmin PageObj = new SuperAdmin();
            temp = PageObj.EditClassTeacher(UID_FK);
            temp.Divisions = PageObj.GetDivisionList();           
            temp.Levels = PageObj.GetlevelListByDivID(temp.DivisionID);
            temp.ClassInfo = PageObj.GetAllClassSt(temp.LevelID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateClassTeacher(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateClassTeacher(Data);
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

        [HttpPost]
        public JsonResult DeleteClassTeacher(string Id)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (Id != null && Id != "")
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteClassTeacher(Convert.ToInt64(Id));
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
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
        public JsonResult UpdateStatusAssignClassTeacher(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateStatusAssignClassTeacher(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatusUnAssignClassTeacher(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateStatusUnAssignClassTeacher(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull ";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditSubjectTeacher(long UID_FK)
        {
            ResultInfo<EditSubjectTeacher> ResultInfo = new ResultInfo<EditSubjectTeacher>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            EditSubjectTeacher temp = new EditSubjectTeacher();
            SuperAdmin PageObj = new SuperAdmin();
            temp = PageObj.EditSubjectTeacher(UID_FK);
            temp.Divisions = PageObj.GetDivisionList();
            temp.Levels = PageObj.GetlevelListByDivID(temp.DivisionID);
            temp.ClassInfo = PageObj.GetAllClassSt(temp.LevelID);
            temp.Syllabus_Subjects = PageObj.GetAllSubject(temp.LevelID);
            ResultInfo.Info = temp;
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success! Get Result ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSubjectTeacher(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateSubjectTeacher(Data);
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

        [HttpPost]
        public JsonResult DeleteSubjectTeacher(string Id)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Failed Deleted"
            };
            try
            {
                if (Id != null && Id != "")
                {
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.DeleteSubjectTeacher(Convert.ToInt64(Id));
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Delete Sucess ";
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
        public JsonResult UpdateStatusAssignSubjectTeacher(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateStatusAssignSubjectTeacher(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatusUnAssignSubjectTeacher(AssignPrivilege Data)
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
                    SuperAdmin Obj = new SuperAdmin();
                    ResultInfo.Info = Obj.UpdateStatusUnAssignSubjectTeacher(Data);
                    if (ResultInfo.Info != null)
                    {
                        ResultInfo.Description = "Success|Update Sucessfull ";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Feedback Ramashree
        [HttpPost]
        public JsonResult GetTopicListBySubjectID(int ID)
        {
            ResultInfo<List<TeachersSyllabusTopicDetails>> ResultInfo = new ResultInfo<List<TeachersSyllabusTopicDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<TeachersSyllabusTopicDetails> nm = new List<TeachersSyllabusTopicDetails>();
            SuperAdmin PageObj = new SuperAdmin();
            if (ID > 0)
            {

                nm = PageObj.GetTopic(ID);
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
        public JsonResult GetAssignmentListByTopicID(int ID)
        {
            ResultInfo<List<ClassTeacherAssignmentDetails>> ResultInfo = new ResultInfo<List<ClassTeacherAssignmentDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<ClassTeacherAssignmentDetails> nm = new List<ClassTeacherAssignmentDetails>();
            SuperAdmin PageObj = new SuperAdmin();
            if (ID > 0)
            {

                nm = PageObj.GetAssignment(ID);
                ResultInfo.Info = nm;
            }
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeedbackListAssignment(long AssignmentID)
        {
            ResultInfo<List<ClassTeacherAssignmentDetails>> ResultInfo = new ResultInfo<List<ClassTeacherAssignmentDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo.Info = PageObj.GetFeedbackListAssignment(AssignmentID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeedbackListAttendence(long SubjectID,long ClassID)
        {
            ResultInfo<List<ClassTeacherAssignmentDetails>> ResultInfo = new ResultInfo<List<ClassTeacherAssignmentDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo.Info = PageObj.GetFeedbackListAttendence(SubjectID,ClassID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSemester(long SchoolID)
        {
            ResultInfo<List<Semester>> ResultInfo = new ResultInfo<List<Semester>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            List<Semester> nm = new List<Semester>();
            SuperAdmin PageObj = new SuperAdmin();

            nm = PageObj.GetSemester(SchoolID);
            ResultInfo.Info = nm;

            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeedbackListExam(long SubjectID,long ClassID,long SemesterID)
        {
            ResultInfo<List<ClassTeacherAssignmentDetails>> ResultInfo = new ResultInfo<List<ClassTeacherAssignmentDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo.Info = PageObj.GetFeedbackListExam(SubjectID,ClassID,SemesterID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }









        public JsonResult GetFeedbackListStudent(long SubjectID)
        {
            ResultInfo<List<ClassTeacherAssignmentDetails>> ResultInfo = new ResultInfo<List<ClassTeacherAssignmentDetails>>()
            {
                Status = false,
                Description = "Failed|Login"
            };
            SuperAdmin PageObj = new SuperAdmin();
            ResultInfo.Info = PageObj.GetFeedbackListStudent(SubjectID);
            if (ResultInfo.Info != null)
            {
                ResultInfo.Description = "Success| Get class name with id ";
                ResultInfo.Status = true;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}