﻿using System;
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

   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Dashboard_DSMaster objMasterPage = new Dashboard_DSMaster();
            objMasterPage.Logout();
           // Response.Redirect("../Default.aspx");
            return;
        }
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.Form["UserName"]))
            {
                btnsubmit.Text = "Update";
                if (Request.Form.Count > 0)
                {
                    //txtusername.Text = Request.Form["UserName"];
                    //txtusername.Enabled = false;

                    // txtLocation.Text = Request.Form["Location"];

                    //btnCancel.Visible = false;
                }
                
            }
            GetTerminalList();
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
            if (txtTerminal.Text == "" || txtTerminal.Text == null)
            {
                add = "Please Enter Service Name ! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                txtTerminal.Focus();
                return;
            }
            else if (ddlTerminalType.SelectedIndex == 0)
            {
                add = "Please Select Terminal Type! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                ddlTerminalType.Focus();
                return;
            }        

            if (objds == null)
                objds = new DataSet();

            RespAddTerminal objRes = new RespAddTerminal();
            ReqAddTerminal objReq = new ReqAddTerminal();
            objReq.TerminalID = txtTerminal.Text;
            //objReq.Type = ddlTerminalType.SelectedValue;
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";

                DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(ReqAddTerminal));

                MemoryStream memStrToSend = new MemoryStream();

                objJsonSerSend.WriteObject(memStrToSend, objReq);

                string data = Encoding.Default.GetString(memStrToSend.ToArray());


                string result = client.UploadString(URL + "/AddTerminal", "POST", data);

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespAddTerminal));

                objRes = (RespAddTerminal)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objRes.Result == true)
                {
                    var page1 = HttpContext.Current.CurrentHandler as Page;
                    add = "Terminal Create Successfully ! ";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Success('" + add + "');", true);
                    Log.Write("Terminal Create Successfully", "");
                }
                else if (objRes.Error.ToLower().Contains("Service name already added"))
                {
                    add = "Terminal Allready Create Please try diffrent ! ";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                    Log.Write("Terminal Allready added", "");
                }
                else
                {
                    add = "Terminal Create Failed ! ";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                    Log.Write("Terminal Create Failed", "");
                }
            }
        }
        catch (Exception excp)
        {
            Log.Write("Exception in Creat Terminal :- " + excp, "");
        }
    }
    protected void GetTerminalList()
    {
        try
        {
            Reply objReply = new Reply();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            string result = client.UploadString(URL + "/GetTerminalList", "POST", "");

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objReply.Result == true)
            {
                if (objReply.DS.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("S No");
                    dt.Columns.Add("Terminal_Id");
                    dt.Columns.Add("TerminalType");
                    dt.Columns.Add("Status");
                    dt.Columns.Add("pk_TM_Id");
                    for (int i = 0; i < objReply.DS.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();                        
                        row["Terminal_Id"] = objReply.DS.Tables[0].Rows[i]["Terminal_ID"].ToString();
                        row["TerminalType"] = objReply.DS.Tables[0].Rows[i]["TerminalType"].ToString();
                        row["Status"] = objReply.DS.Tables[0].Rows[i]["Status"].ToString();
                        row["pk_TM_Id"] = objReply.DS.Tables[0].Rows[i]["pk_TM_Id"].ToString();
                        dt.Rows.Add(row);
                    }
                    GV_TerminalList.DataSource = dt;
                    GV_TerminalList.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetUserList Error :- " + ex.ToString(), "");
        }
    }

    protected void GV_TerminalList_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void GV_TerminalList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            UserDetailsReq objUserDetails = new UserDetailsReq();
            UserDetailsRes objUserDetailsRes = new UserDetailsRes();

            if (e.CommandName == "ChangeStatus")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GV_TerminalList.Rows[rowIndex];

                string status = row.Cells[3].Text;

                if (status == "Active")
                    status = "Inactive";
                else
                    status = "Active";
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "Application/Json";

                var data = JsonConvert.SerializeObject(row.Cells[5].Text+"#"+status);
                string result1 = client.UploadString(URL + "/GetTerminalList", "POST", data);

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result1));
                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));
                Reply objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objReply.Result)
                {
                    add = "Status Change Successfully !";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Success('" + add + "');", true);
                }
                else
                {
                    add = "Status Change Failed !";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                }
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