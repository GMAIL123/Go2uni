using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Go2uniApi.CodeFile;
using Go2uniApi.Models;
using Newtonsoft.Json;

using static Go2uniApi.Models.User_Part;

namespace Go2uniApi.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        //  Global CM = new Global(ConfigurationManager.ConnectionStrings["ConnString"].ToString());

                
        [HttpPost]
        public JsonResult CheckEmailExistance(string Email)
        {
            User_Backend PageObj = new User_Backend();
            if (PageObj.Checkemailexistance(Email))
            {
                ResultInfo<string> result = new ResultInfo<string>()
                {
                    Status = true,
                    Description = "Success|This email exist",
                    Info = "NO DATA"
                };
                return Json(result);
            }
            else
            {
                ResultInfo<string> result = new ResultInfo<string>()
                {
                    Status = false,
                    Description = "Failed|This email does not exist",
                    Info = "NO DATA"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult StudentUpdatePassword(string Email, string Password)
        {
            User_Backend PageObj = new User_Backend();
            if (PageObj.StudentUpdatePassword(Email, Password))
            {
                ResultInfo<string> result = new ResultInfo<string>()
                {
                    Status = true,
                    Description = "Success|Password Changed Successfully",
                    Info = "NO DATA"
                };
                return Json(result);
            }
            else
            {
                ResultInfo<string> result = new ResultInfo<string>()
                {
                    Status = false,
                    Description = "Failed|Internal Error Occured",
                    Info = "NO DATA"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Login(string DeviceToken, Login Info)
        {
            ResultInfo<Login> ResultInfo = new ResultInfo<Login>()
            {
                Status = false,
                Description = "Failed|Login",

            };
            Global Glb = new Global();
            // Login lgn = new Login();
            User_Backend PageObj = new User_Backend();
            Login objDett = new Login();
            string Token = "";
            try
            {
                if (Info != null)
                {
                    #region CHECK OBJECT IS NULL 
                    string CheckedOb = Glb.ObjectNullChecking(Info);
                    Info = JsonConvert.DeserializeObject<Login>(CheckedOb);
                    #endregion

                    Info = PageObj.Login(Info.Email, Info.Password);
                    if (Info != null && Info.UID != 0)
                    {
                        objDett = PageObj.GetLoginDetails(Info.UID, Info.UserType_FK);
                       
                        //objDett.IsOnline = PageObj.UpdateIsOnline(Convert.ToInt32(Info.UID));
                       
                        ResultInfo.Info = objDett;
                        Token = Glb.GenerateToken(Info.UID, Info.Email, Info.Password);
                        if (!string.IsNullOrEmpty(Token))
                        {
                            Glb.UpdateTokenId(Info.UID, Token);
                        }

                        ResultInfo.TokenId = Token;
                        ResultInfo.ErrorCode = 200;
                        //  ResultInfo.IsSuccess = true;
                        ResultInfo.Description = "Success! User authenticated";
                    }
                    else
                    {
                        if (Info.Email != Info.Email)
                        {
                            ResultInfo.Description = "Failed!Wrong email address or password";
                        }
                        else if (Info.Password != Info.Password)
                        {
                            ResultInfo.Description = "Failed!Wrong  password";
                        }
                        else
                        {
                            ResultInfo.Description = "Failed!Wrong email address or password";
                        }
                    }

                }
                else
                {
                    ResultInfo.Description = "Failed!Invalid parameter";
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Description = "Failed!" + ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
                          

        [HttpPost]
        public JsonResult Registration(Registration Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>()
            {
                Status = false,
                Description = "Failed|Login",
                Info = "NO DATA"
            };
            User_Backend PageObj = new User_Backend();
            if (Info != null)
            {
                ResultInfo.Info = PageObj.Registration(Info);

                if (!string.IsNullOrEmpty(ResultInfo.Info))
                {
                    ResultInfo.Description = "Success|Login Successfully";
                    ResultInfo.Status = true;
                }
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);

        }

        #region user SOCIAL REGISTRATION facebook
        [HttpPost]
        public JsonResult PatientSocialLogIn(string DeviceToken, SocialUserInfo Info)
        {
            ResultInfo<string> ResultInfo = new ResultInfo<string>();
            ResultInfo.ErrorCode = 400;
            ResultInfo.Status = false;
            ResultInfo.Description = "Patient social login";
            ResultInfo.Info = "Login Failed";
            Global Glb = new Global();
            User_Backend PageObj = new User_Backend();

            try
            {
                if (Info != null && Info.SocialUserId != "" && Info.SocialUserId != null)
                {
                    #region CHECK OBJECT IS NULL BY IMAD
                    string CheckedOb = Glb.ObjectNullChecking(Info);
                    Info = JsonConvert.DeserializeObject<SocialUserInfo>(CheckedOb);
                    #endregion

                    ResultInfo.Info = PageObj.UserSocialRegistration(Info);    // Function call for database store
                    string[] arr = ResultInfo.Info.Split('!');
                    if (arr[0] == "Success")
                    {
                        long insertedId = 0;
                        if (arr[1] != "")
                        {
                            insertedId = Convert.ToInt64(arr[1]);
                        }
                        if (ResultInfo.Info.Contains("NewUser"))
                        {
                            string Password = "";
                            if (arr[3] != "")
                            {
                                Password = arr[3];
                            }
                            string Token = Glb.GenerateToken(insertedId, Info.Email, Password);
                            if (!string.IsNullOrEmpty(Token))
                            {
                                Glb.UpdateTokenId(insertedId, Token);
                                ResultInfo.Info = "User Registration successfull.";
                            }
                            else
                            {
                                ResultInfo.Info = "Token is not generated.";
                            }
                            ResultInfo.TokenId = Token;
                            ResultInfo.ErrorCode = 200;
                           ResultInfo.Status = true;
                            ResultInfo.LastModifiedId = insertedId;
                            ResultInfo.Description = "Success! User authenticated";
                        }
                        else
                        {
                            if (insertedId > 0)
                            {
                                string Token = Glb.GetTokenByID(insertedId);
                                ResultInfo.TokenId = Token;
                                ResultInfo.ErrorCode = 200;
                                ResultInfo.Status = true;
                                ResultInfo.LastModifiedId = insertedId;
                                ResultInfo.Info = "User registration successfull.";
                            }
                        }
                    }
                }
                else
                {
                    ResultInfo.Info = "Details is not provided.";
                }
            }
            catch (Exception ex)
            {
                ResultInfo.Info = ex.Message;
            }
            return Json(ResultInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult forgetPass(Registration info)
        {
            ResultInfo<string> resultInfo = new ResultInfo<string>();
            User_Backend backend = new User_Backend();
            if (info != null)
            {
                if (backend.CheckEMailExists(info.Email) == true)
                {
                    resultInfo.Description = "Email Exists";
                    resultInfo.Status = true;
                }
                else if (backend.CheckEMailExists(info.Email) == false)
                {
                    resultInfo.Description = "Email Does Not Exist";
                    resultInfo.Status = false;
                }
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult updateOTP(Reginfo info)
        {

            User_Backend backend = new User_Backend();
            if (backend.InsertOrUpdateOTP(info))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult updatePassword(Reginfo info)
        {

            string status = String.Empty;
            User_Backend backend = new User_Backend();

            if (backend.checkForOtp(info) == "OtpFound")
            {
                if (backend.updateDetails(info))
                {

                    status = "success";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = "failed";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }
            else if (backend.checkForOtp(info) == "OtpExpired")
            {
                status = "OtpExpired";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            else if (backend.checkForOtp(info) == "OtpNotMatch")
            {
                status = "OtpNotMatch";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            else
            {
                status = "failed";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
        }

    }










}