using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Text;
using System.Net;
using System.IO;
using LipiRMS;
using Newtonsoft.Json;
public partial class Dashboard_Dashboard : System.Web.UI.Page
{

    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public static string Interval = System.Configuration.ConfigurationManager.AppSettings["TimerInterval"].ToString();
    public static DataSet objds;
    int CashConnected = 0, CashDisconnected = 0;
    int RecConnected = 0, RecDisconnected = 0;
    string add = "";
    Reply objGlobal = new Reply();
    //public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    //public static DataSet objds;

    public class UserDetailsCreation
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string UserCreationDate { get; set; }
        public string Pwd { get; set; }
    }
    public struct UserDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Location { get; set; }
        public string Role { get; set; }

    }

    public class ReqAddUser
    {

        public string UserName { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }
    }


    public class RespAddUser
    {

        public bool Result { get; set; }

        public string Error { get; set; }


        public string Password { get; set; }
    }
    public class Reply
    {
        public DataSet DS { get; set; }

        public bool res { get; set; }


        public string strError { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["Username"] == null)
        //{
        //    Response.Redirect("../Default.aspx");
        //    return;
        //}
       

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.Form["UserName"]))
            {
                btnsubmit.Text = "Update";
                if (Request.Form.Count > 0)
                {
                  
                    txtusername.Text = Request.Form["UserName"];
                    txtusername.Enabled = false;

                    // txtLocation.Text = Request.Form["Location"];

                    //btnCancel.Visible = false;


                }
                
            }
            GetUserList();
        }
        else
        {
            btnsubmit.Text = "Submit";
        }
        //bindTemplateName();

    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtusername.Text == "" || txtusername.Text == null)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('Please Enter The UserName');", true);
                txtusername.Focus();
                return;
            }

            ReqAddUser objUserReq = new ReqAddUser();
            objUserReq.UserName = txtusername.Text;
            objUserReq.FirstName = txtFirstName.Text;
            objUserReq.LastName = txtLastName.Text;

            if (objds == null)
                objds = new DataSet();

            RespAddUser objRes = new RespAddUser();

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";

                DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(ReqAddUser));

                MemoryStream memStrToSend = new MemoryStream();

                objJsonSerSend.WriteObject(memStrToSend, objUserReq);

                string data = Encoding.Default.GetString(memStrToSend.ToArray());

                string result = client.UploadString(URL + "/AddUser", "POST", data);

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespAddUser));

                objRes = (RespAddUser)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objRes.Result == true)
                {
                    var page1 = HttpContext.Current.CurrentHandler as Page;
                    add = "User Create Successfully ! User Password = @" + objRes.Password;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Success('" + add + "');", true);
                    Log.Write("User Create Successfully", "");
                }
                else if (objRes.Error.ToLower().Contains("username already added"))
                {
                    add = "Username Allready added Please try diffrent ! ";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                    Log.Write("Username Allready added", "");
                }
                else
                {
                    add = "User Create Failed ! ";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                    Log.Write("User Create Failed", "");
                }
            }
        }
        catch (Exception excp)
        {
            Log.Write("Exception in Creat User :- " + excp, "");
        }
    }
    protected void GetUserList()
    {
        try
        {
            UserList objUserList = new UserList();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            string result = client.UploadString(URL + "/GetUserList", "POST", "");

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(UserList));

            objUserList = (UserList)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objUserList.Result == true)
            {
                if (objUserList.DS.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("S No");
                    dt.Columns.Add("Name");
                    dt.Columns.Add("UserName");
                    dt.Columns.Add("pk_UserId");
                    for (int i = 0; i < objUserList.DS.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["Name"] = objUserList.DS.Tables[0].Rows[i]["FirstName"].ToString() + " " + objUserList.DS.Tables[0].Rows[i]["LastName"].ToString();
                        row["UserName"] = objUserList.DS.Tables[0].Rows[i]["UserName"].ToString();
                        row["pk_UserId"] = objUserList.DS.Tables[0].Rows[i]["pk_UserId"].ToString();
                        dt.Rows.Add(row);
                    }
                    GV_UserList.DataSource = dt;
                    GV_UserList.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetUserList Error :- " + ex.ToString(), "");
        }
    }

    protected void GV_UserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int j = e.Row.Cells.Count;
            for (int i = 0; i < j; i++)
            {
                TableCell cell = e.Row.Cells[i];
                if (cell.Text != "")
                {
                    cell.Style.Add("text-align", "center");
                    cell.Style.Add("font-weight", " normal");
                    cell.Style.Add("padding", " 15px");

                }
            }

        }
        catch (Exception ex)
        {
            Log.Write("Exception in GV_UserList_RowDataBound Error :- " + ex.ToString(), "");
        }
    }

    protected void GV_UserList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            UserDetailsReq objUserDetails = new UserDetailsReq();
            UserDetailsRes objUserDetailsRes = new UserDetailsRes();

            if (e.CommandName == "EditPassword")
            {               
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GV_UserList.Rows[rowIndex];               
                objUserDetails.Type = "ResetPassword";
                objUserDetails.UserId = row.Cells[5].Text;
                string randompassword = "";
                Random random = new Random();
                randompassword = random.Next(1001, 9999).ToString();

                objUserDetails.Password = "@" + randompassword;
                
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "Application/Json";
                DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(UserDetailsReq));
                MemoryStream memStrToSend = new MemoryStream();
                objJsonSerSend.WriteObject(memStrToSend, objUserDetails);
                string data = Encoding.Default.GetString(memStrToSend.ToArray());

                string result1 = client.UploadString(URL + "/EditUser", "POST", data);
                string obj = JsonConvert.DeserializeObject<string>(result1);
                if (obj == "true")
                {
                    add = "Password Reset Successfully New Password  =  @"+randompassword + "  ";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Success('" + add + "');", true);
                }
                else
                {
                    add = "Password Reset Failed !";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                }
            }
            else
            if (e.CommandName == "Delete_User")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GV_UserList.Rows[rowIndex];
                string UserId = row.Cells[5].Text;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Delete('" + UserId + "');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertmsg", ex.ToString(), true);
        }
    }
    [System.Web.Services.WebMethod]
    public static string RegisterUser(string UserId)
    {
        UserDetailsReq objUserDetails = new UserDetailsReq();
        UserDetailsRes objUserDetailsRes = new UserDetailsRes();

        objUserDetails.Type = "delete";
        objUserDetails.UserId = UserId;       
        string result = "";        
 
        WebClient client = new WebClient();
        client.Headers[HttpRequestHeader.ContentType] = "text/json";

        DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(UserDetailsReq));
        MemoryStream memStrToSend = new MemoryStream();
        objJsonSerSend.WriteObject(memStrToSend, objUserDetails);
        string data = Encoding.Default.GetString(memStrToSend.ToArray());        

        string result1 = client.UploadString(URL + "/EditUser", "POST", data);

        string res = JsonConvert.DeserializeObject<string>(result1);
        if (res == "true")
        {          
            result = "Project Type Delete Successfully !";
        }
        else
        {
            result = "Project Type Delete Failed !";

        }
        return result;
    }   
}