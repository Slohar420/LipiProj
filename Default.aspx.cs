using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Utilities.Encoders;
using LipiRMS;

public partial class _Default : System.Web.UI.Page
{
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    string add = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        

        //if (Session["Username"] == null)
        //{
        //    Session.Clear();

            try
            {
                WebClient objWC = new WebClient();
                objWC.Headers[HttpRequestHeader.ContentType] = "text/json";
                                
                string result = objWC.UploadString(URL + "/GetProcessingTerminalsList", "POST", "");
                
                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespGetProcessingTerminalsList));
                RespGetProcessingTerminalsList objRespGetProcessingTerminalsList = (RespGetProcessingTerminalsList)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objRespGetProcessingTerminalsList.Result && objRespGetProcessingTerminalsList.Terminals.Count() > 0)
                {
                    cmbTerminalID.Items.Clear();
                    for (int iIndex = 0; iIndex < objRespGetProcessingTerminalsList.Terminals.Count(); iIndex++)
                    {
                        cmbTerminalID.Items.Add(objRespGetProcessingTerminalsList.Terminals[iIndex].TerminalName);
                    }
                    txtUserName.Enabled = txtPassword.Enabled = true;
                }
                else
                {
                    txtUserName.Enabled = txtPassword.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                add = " Terminal List Not Found ! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                Log.Write("Terminal Create Failed", "");
                Log.Write("Exception in GetProcessingTerminalsList Error :- "+ex.ToString(), "");
            }
       //}
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text == "" || txtUserName.Text == null)
            {
                txtUserName.Text = "";
                txtUserName.Text = "";
                //Label1.Text = "Please Enter User Name";
                //Label1.Visible = true;             
                add = " Kindly enter username ! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);               
                return;
            }
            if (txtPassword.Text == "" || txtPassword.Text == null)
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                //Label1.Text = "Please Enter Password";
                //Label1.Visible = true;
                add = " Kindly enter Password ! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                return;
                
            }
            var decodedString1 = Base64.Decode(txtUserName.Text);
            string Uname = TrimString(System.Text.Encoding.UTF8.GetString(decodedString1), '&');

            var decodedString2 = Base64.Decode(txtPassword.Text);
            string Pwd = TrimString(System.Text.Encoding.UTF8.GetString(decodedString2), '*');

            ReqLogin objReqLogin = new ReqLogin();
            objReqLogin.UserName = Uname;
            objReqLogin.Password = Pwd;
            objReqLogin.TerminalID = cmbTerminalID.Text;

            WebClient objWC = new WebClient();
            objWC.Headers[HttpRequestHeader.ContentType] = "text/json";

            DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(ReqLogin));
            MemoryStream memStrToSend = new MemoryStream();
            objJsonSerSend.WriteObject(memStrToSend, objReqLogin);

            string data = Encoding.Default.GetString(memStrToSend.ToArray());

            string result = objWC.UploadString(URL + "/Login", "POST", data);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespLogin));
            RespLogin objRespLogin = (RespLogin)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objRespLogin.Result)
            {
                if (objRespLogin.OtherData == "success")
                {
                    Session["Username"] = Uname;
                    Session["Role"] = Uname;
                    Session["Location"] = null;
                    Session["TerminalID"] = cmbTerminalID.Text;
                    Session["Privileges"] = objRespLogin.Privileges;
                    Response.Redirect("Dashboard//DashboardUser.aspx", false);
                }
                else if (objRespLogin.OtherData == "location")
                {
                    Session["Username"] = Uname;
                    //Session["Role"] = response[1];
                    //Session["Location"] = response[2];
                    Response.Redirect("Dashboard//DashboardUser.aspx", false);
                }
                else if (objRespLogin.OtherData.Contains("redirect"))
                {
                    string[] uid = objRespLogin.OtherData.Split('#');
                    Session["Username"] = Uname;
                    Session["Role"] = Uname;
                    Session["TerminalID"] = cmbTerminalID.Text;
                    Response.Redirect("Dashboard/ResetPassword.aspx?Username=" + Convert.ToBase64String(Encoding.ASCII.GetBytes(Uname)) +"&UID="+Convert.ToBase64String(Encoding.ASCII.GetBytes(uid[1])), false);
                }
            }            
            else
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                add = " Enter User Name & Password  Wrong ! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            //  Session["Username"] = null;
            txtUserName.Text = "";
            txtPassword.Text = "";
        }
    }
    private string TrimString(string text, char char1)
    {
        string str = null;
        string[] strArr = null;

        str = text;
        char[] splitchar = { char1 };
        strArr = str.Split(splitchar);

        string output = strArr[0];
        return output;
    }
}

public class RespGetProcessingTerminalsList
{
    public bool Result { get; set; }

    public string Error { get; set; }

    public Terminal[] Terminals { get; set; }
}

public class Terminal
{
    public string TerminalName { get; set; }

    public int TerminalID { get; set; }
}

public class ReqLogin
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string TerminalID { get; set; }
}

public class RespLogin
{
    public bool Result { get; set; }

    public string Error { get; set; }

    public string OtherData { get; set; }
    public string Privileges { get; set; }
}
