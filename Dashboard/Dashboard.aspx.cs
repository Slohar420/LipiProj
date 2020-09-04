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
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using System.Web.Services;

public partial class Dashboard_Dashboard : System.Web.UI.Page
{

    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public static string Interval = System.Configuration.ConfigurationManager.AppSettings["TimerInterval"].ToString();
    public static DataSet objds;
    int CashConnected = 0, CashDisconnected = 0;
    int RecConnected = 0, RecDisconnected = 0;
    Reply objGlobal = new Reply();
    string add = "";
    static string TID = "";
    private static bool isStatusUpdated = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetChartData();
        if (Session["TerminalID"] == null)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Session Null');window.location ='../Default.aspx';", true);
            return;
        }
        if (!IsPostBack)
        {
            TID = Session["TerminalID"].ToString();
            GetCount();
            lblLastFatchTokenNo.Text = "";
            if (Session["Username"].ToString().Contains("admin"))
            {
                lblTotalTokens.Visible = false;
                lblComplete.Visible = false;
                lblSkip.Visible = false;
                lblReject.Visible = false;

                lblTotalTokenAdmin.Visible = true;
                lblCompleteAdmin.Visible = true;
                lblSkipAdmin.Visible = true;
                lblRejectAdmin.Visible = true;

                CardFatchToken.Visible = false;
                cardTokenStatus.Visible = false;

                cardAdminTokenStatus.Visible = true;
                GetAdminCount();
                GetServiceListADmin();
               
            }
            else
            {
                lblTotalTokens.Visible = true;
                lblComplete.Visible = true;
                lblSkip.Visible = true;
                lblReject.Visible = true;

                lblTotalTokenAdmin.Visible = false;
                lblCompleteAdmin.Visible = false;
                lblSkipAdmin.Visible = false;
                lblRejectAdmin.Visible = false;

                CardFatchToken.Visible = true;
                cardTokenStatus.Visible = true;

                cardAdminTokenStatus.Visible = false;
            }
            for (int i = 0; i < chkStatusList.Items.Count; i++)
            {
                chkStatusList.Items[i].Attributes.Add("onclick", "MutExChkList(this)");
            }

            if (lblLastFatchTokenNo.Text.Length == 1)
            {
                lblLastFatchTokenNo.Text = "00" + lblLastFatchTokenNo.Text;
                lblLastFatchTokenNo.Style.Add("font-size", "1000%");
            }
            else if (lblLastFatchTokenNo.Text.Length == 2)
            {
                lblLastFatchTokenNo.Text = "0" + lblLastFatchTokenNo.Text;
                lblLastFatchTokenNo.Style.Add("font-size", "1000%");
            }
            else if (lblLastFatchTokenNo.Text.Length == 3)
            {
                lblLastFatchTokenNo.Style.Add("font-size", "1000%");
            }
            else if (lblLastFatchTokenNo.Text.Length == 4)
            {
                lblLastFatchTokenNo.Style.Add("font-size", "800%");
            }
            else if (lblLastFatchTokenNo.Text.Length == 0) {
                isStatusUpdated = true;
            }
            EnableAndDisableFunction(true);
        }
        BindServiceList();
        GetServiceList();
    }
    protected void EnableAndDisableFunction(bool btnStatus)
    {
        if (btnStatus)
        {
            btnUpdate.Enabled = false;
            chkStatusList.Enabled = false;
            btnFetchNextToken.Enabled = true;
            chkNeedToSkip.Enabled = true;
        }
        else
        {
            btnUpdate.Enabled = true;
            chkStatusList.Enabled = true;
            btnFetchNextToken.Enabled = false;
            chkNeedToSkip.Enabled = false;
        }
        if (btnUpdate.Enabled == false || btnFetchNextToken.Enabled == false)
        {
            btnUpdate.Style.Add("background-color", "#337ab7");
            btnUpdate.Style.Add("color", "white");
            btnUpdate.Style.Add("margin", "8px 0");
            btnUpdate.Style.Add("border", "none");
            btnUpdate.Style.Add("cursor", "pointer");
            btnUpdate.Style.Add("width", "200%");
            btnUpdate.Style.Add("opacity", "0.9");
            btnUpdate.Style.Add("padding", "14px 20px");

            btnFetchNextToken.Style.Add("background-color", "#337ab7");
            btnFetchNextToken.Style.Add("color", "white");
            btnFetchNextToken.Style.Add("margin", "8px 0");
            btnFetchNextToken.Style.Add("border", "none");
            btnFetchNextToken.Style.Add("cursor", "pointer");
            btnFetchNextToken.Style.Add("width", "200%");
            btnFetchNextToken.Style.Add("opacity", "0.9");
            btnFetchNextToken.Style.Add("padding", "14px 20px");
        }
    }
    protected void BindServiceList()
    {
        try
        {
            Reply objReply = new Reply();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            string data = JsonConvert.SerializeObject("Status");
            string result = client.UploadString(URL + "/GetServiceList", "POST", data);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objReply.Result == true)
            {
                if (objReply.DS.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < objReply.DS.Tables[0].Rows.Count; i++)
                    {
                        ddlServices.Items.Add(objReply.DS.Tables[0].Rows[i]["ServiceName"].ToString());
                    }
                    ddlServices.DataValueField = objReply.DS.Tables[0].Columns["pk_SM-Id"].ToString();
                    ddlServices.DataTextField = objReply.DS.Tables[0].Columns["ServiceName"].ToString();
                    //ddlServices.DataValueField = objReply.DS.Tables[0].Columns["pk_SM_Id"].ToString();
                    //ddlServices.DataTextField = objReply.DS.Tables[0].Columns["ServiceName"].ToString();

                    //ddlServices.DataSource = objReply.DS;
                    //ddlServices.DataBind();
                    //ddlServices.Items.Add("All");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in BindServiceList : - Error " + ex.ToString(), "");
        }
    }

    protected void btnFetchNextToken_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlServices.SelectedIndex == 0)
            {
                add = "Please Select Service Type !";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                return;
            }
            else
            {
                if (lblLastFatchTokenNo.Text.Length == 0 || isStatusUpdated)
                {
                    ReqFetchNextToken objReqFetchNextToken = new ReqFetchNextToken();
                    RespFetchNextToken objRespFetchNextToken = new RespFetchNextToken();

                    objReqFetchNextToken.TerminalID = Session["TerminalID"].ToString();
                    objReqFetchNextToken.UserName = Session["Username"].ToString();
                    //objReqFetchNextToken.ServiceType = ddlServices.SelectedItem.Text;
                    objReqFetchNextToken.ServiceType = Session["Privileges"].ToString();
                    if (chkNeedToSkip.Checked == true)
                        objReqFetchNextToken.IsNeedToSkip = true;
                    else
                        objReqFetchNextToken.IsNeedToSkip = false;

                    WebClient client = new WebClient();
                    client.Headers[HttpRequestHeader.ContentType] = "Application/Json";

                    var data = JsonConvert.SerializeObject(objReqFetchNextToken);
                    string result1 = client.UploadString(URL + "/FetchNextToken", "POST", data);

                    MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result1));
                    DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespFetchNextToken));
                    objRespFetchNextToken = (RespFetchNextToken)objJsonSerRecv.ReadObject(memstrToReceive);

                    if (objRespFetchNextToken.Result)
                    {
                        if (objRespFetchNextToken.TokenNo != "" && objRespFetchNextToken.TokenNo != null)
                            lblLastFatchTokenNo.Text = objRespFetchNextToken.TokenNo;
                        else
                        {
                            isStatusUpdated = true;
                        }
                        divLoader.Visible = false;
                        pMsgLoader.Visible = false;
                        divLblLastTokenFatechNo.Visible = true;
                        EnableAndDisableFunction(false);
                    }
                    else
                    {
                        add = "No Token Available This Time!";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                        divLoader.Visible = true;
                        pMsgLoader.Visible = true;
                        divLblLastTokenFatechNo.Visible = false;
                        EnableAndDisableFunction(true);
                    }
                    if (lblLastFatchTokenNo.Text.Length == 1)
                    {
                        lblLastFatchTokenNo.Text = "00" + lblLastFatchTokenNo.Text;
                        lblLastFatchTokenNo.Style.Add("font-size", "1000%");
                    }
                    else if (lblLastFatchTokenNo.Text.Length == 2)
                    {
                        lblLastFatchTokenNo.Text = "0" + lblLastFatchTokenNo.Text;
                        lblLastFatchTokenNo.Style.Add("font-size", "1000%");
                    }
                    else if (lblLastFatchTokenNo.Text.Length == 3)
                    {
                        lblLastFatchTokenNo.Style.Add("font-size", "1000%");
                    }
                    else if (lblLastFatchTokenNo.Text.Length == 4)
                    {
                        lblLastFatchTokenNo.Style.Add("font-size", "800%");
                    }
                }
                else { return;}
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in btnFetchNextToken Error : - " + ex.ToString(), "");
        }
    }

    
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            ReqUpdateTokenStatus objReqUpdateTokenStatus = new ReqUpdateTokenStatus();
            RespUpdateTokenStatus objRespUpdateTokenStatus = new RespUpdateTokenStatus();

            objReqUpdateTokenStatus.TokenNo = lblLastFatchTokenNo.Text;
            objReqUpdateTokenStatus.Status = chkStatusList.SelectedItem.Text;

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "Application/Json";

            var data = JsonConvert.SerializeObject(objReqUpdateTokenStatus);
            string result1 = client.UploadString(URL + "/UpdateTokenStatus", "POST", data);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result1));
            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespUpdateTokenStatus));
            objRespUpdateTokenStatus = (RespUpdateTokenStatus)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objRespUpdateTokenStatus.Result)
            {
                add = "Token Status Update Successfully !";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Success('" + add + "');", true);
                chkStatusList.ClearSelection();
                EnableAndDisableFunction(true);
                divLoader.Visible = true;
                pMsgLoader.Visible = true;
                divLblLastTokenFatechNo.Visible = false;
                GetServiceList();
                GetCount();
                lblLastFatchTokenNo.Text = "";
                isStatusUpdated = true;
            }
            else
            {
                add = "Token  Status Update  Failed !";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in btnUpdateToken Error : - " + ex.ToString(), "");
        }
    }

    protected void chkNeedToSkip_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkNeedToSkip.Checked)
            {

            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in chkNeedToSkip_CheckedChanged Error : - " + ex.ToString(), "");
        }
    }
    protected void GetServiceList()
    {
        try
        {
            Reply objReply = new Reply();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            string JsonString = JsonConvert.SerializeObject("Status");
            string result = client.UploadString(URL + "/GetServiceList", "POST", JsonString);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objReply.Result == true)
            {
                if (objReply.DS.Tables[0].Rows.Count != 0)
                {

                    Reply objReply1 = new Reply();
                    WebClient client1 = new WebClient();
                    ReqServiceWithTokenStatus objReqServiceWithTokenStatus = new ReqServiceWithTokenStatus();

                    objReqServiceWithTokenStatus.TerminalId = TID;
                    objReqServiceWithTokenStatus.ServiceId = "";
                    client1.Headers[HttpRequestHeader.ContentType] = "text/json";
                    string data = JsonConvert.SerializeObject(objReqServiceWithTokenStatus);
                    string result1 = client1.UploadString(URL + "/GetServiceStatusWithTokens", "POST", data);

                    MemoryStream memstrToReceive1 = new MemoryStream(Encoding.UTF8.GetBytes(result1));

                    DataContractJsonSerializer objJsonSerRecv1 = new DataContractJsonSerializer(typeof(Reply));

                    objReply1 = (Reply)objJsonSerRecv.ReadObject(memstrToReceive1);
                    DataTable dt = new DataTable();
                    if (objReply1.Result)
                    {
                        dt.Columns.Add("S No");
                        dt.Columns.Add("Service");
                        dt.Columns.Add("Complete");
                        dt.Columns.Add("Skip");
                        dt.Columns.Add("Reject");

                        for (int i = 0; i < objReply.DS.Tables[0].Rows.Count; i++)
                        {
                            int skip = 0, complete = 0, reject = 0;
                            DataRow row = dt.NewRow();
                            row["Service"] = objReply.DS.Tables[0].Rows[i]["ServiceName"].ToString();
                            string serviceId = objReply.DS.Tables[0].Rows[i]["pk_SM_Id"].ToString();

                            for (int j = 0; j < objReply1.DS.Tables[0].Rows.Count; j++)
                            {
                                string tokenServiceId = objReply1.DS.Tables[0].Rows[j]["fk_SM_Id"].ToString();
                                string terminalID = objReply1.DS.Tables[0].Rows[j]["fk_TID_Process"].ToString();

                                if (serviceId == tokenServiceId)
                                {
                                    if (objReply1.DS.Tables[0].Rows[j]["Status"].ToString() == "Skip")
                                        skip = skip + 1;
                                    if (objReply1.DS.Tables[0].Rows[j]["Status"].ToString() == "Complete")
                                        complete = complete + 1;
                                    if (objReply1.DS.Tables[0].Rows[j]["Status"].ToString() == "Reject")
                                        reject = reject + 1;
                                }
                            }
                            row["Skip"] = Convert.ToString(skip);
                            row["Complete"] = Convert.ToString(complete);
                            row["Reject"] = Convert.ToString(reject);
                            dt.Rows.Add(row);
                        }
                    }
                    GV_ServiceList.DataSource = dt;
                    GV_ServiceList.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetUserList Error :- " + ex.ToString(), "");
        }
    }

    protected void GetServiceListADmin()
    {
        try
        {
            Reply objReply = new Reply();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            string JsonData = JsonConvert.SerializeObject("Status");
            string result = client.UploadString(URL + "/GetServiceList", "POST", JsonData);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objReply.Result == true)
            {
                if (objReply.DS.Tables[0].Rows.Count != 0)
                {

                    Reply objReply1 = new Reply();
                    WebClient client1 = new WebClient();
                    ReqServiceWithTokenStatus objReqServiceWithTokenStatus = new ReqServiceWithTokenStatus();

                    objReqServiceWithTokenStatus.TerminalId = TID;
                    objReqServiceWithTokenStatus.ServiceId = "";
                    client1.Headers[HttpRequestHeader.ContentType] = "text/json";
                    string data = JsonConvert.SerializeObject(objReqServiceWithTokenStatus);
                    string result1 = client1.UploadString(URL + "/GetTokenList", "POST", data);

                    MemoryStream memstrToReceive1 = new MemoryStream(Encoding.UTF8.GetBytes(result1));

                    DataContractJsonSerializer objJsonSerRecv1 = new DataContractJsonSerializer(typeof(Reply));

                    objReply1 = (Reply)objJsonSerRecv.ReadObject(memstrToReceive1);
                    DataTable dt = new DataTable();
                    if (objReply1.Result)
                    {

                        dt.Columns.Add("S No");
                        dt.Columns.Add("Service");
                        dt.Columns.Add("Complete");
                        dt.Columns.Add("Skip");

                        dt.Columns.Add("Reject");
                        for (int i = 0; i < objReply.DS.Tables[0].Rows.Count; i++)
                        {
                            int skip = 0, complete = 0, reject = 0;
                            DataRow row = dt.NewRow();
                            row["Service"] = objReply.DS.Tables[0].Rows[i]["ServiceName"].ToString();
                            string serviceId = objReply.DS.Tables[0].Rows[i]["pk_SM_Id"].ToString();

                            for (int j = 0; j < objReply1.DS.Tables[0].Rows.Count; j++)
                            {
                                string tokenServiceId = objReply1.DS.Tables[0].Rows[j]["fk_SM_Id"].ToString();
                                string terminalID = objReply1.DS.Tables[0].Rows[j]["fk_TID_Process"].ToString();

                                if (serviceId == tokenServiceId)
                                {
                                    if (objReply1.DS.Tables[0].Rows[j]["Status"].ToString() == "Skip")
                                        skip = skip + 1;
                                    if (objReply1.DS.Tables[0].Rows[j]["Status"].ToString() == "Complete")
                                        complete = complete + 1;
                                    if (objReply1.DS.Tables[0].Rows[j]["Status"].ToString() == "Reject")
                                        reject = reject + 1;
                                }

                            }
                            row["Skip"] = Convert.ToString(skip);
                            row["Complete"] = Convert.ToString(complete);
                            row["Reject"] = Convert.ToString(reject);
                            dt.Rows.Add(row);
                        }
                    }
                    GV_AdminTokenStatus.DataSource = dt;
                    GV_AdminTokenStatus.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetUserList Error :- " + ex.ToString(), "");
        }
    }
    protected void GV_ServiceList_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    cell.Style.Add("padding", " 8px");
                }
            }

        }
        catch (Exception ex)
        {
            Log.Write("Exception in GV_UserList_RowDataBound Error :- " + ex.ToString(), "");
        }
    }
    protected void GetCount()
    {
        try
        {
            Reply objReply = new Reply();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            string data = JsonConvert.SerializeObject(TID);
            string result = client.UploadString(URL + "/GetCount", "POST", data);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);
            if (objReply.Result)
            {
                lblTotalTokens.Text = objReply.DS.Tables[0].Rows[0]["Total"].ToString();
                lblSkip.Text = objReply.DS.Tables[0].Rows[0]["Skipp"].ToString();
                lblComplete.Text = objReply.DS.Tables[0].Rows[0]["Complete"].ToString();
                lblReject.Text = objReply.DS.Tables[0].Rows[0]["Reject"].ToString();
                lblTotalTokenLabel.Text = "Total Fatch Tokens"; 
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetCount Error :- " + ex.ToString(), "");
        }
    }

    protected void GetAdminCount()
    {
        try
        {
            Reply objReply = new Reply();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            string data = JsonConvert.SerializeObject(TID);
            string result = client.UploadString(URL + "/GetAdminCount", "POST", data);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);
            if (objReply.Result)
            {
                lblTotalTokenAdmin.Text = objReply.DS.Tables[0].Rows[0]["Total"].ToString();
                lblSkipAdmin.Text = objReply.DS.Tables[0].Rows[0]["Skipp"].ToString();
                lblCompleteAdmin.Text = objReply.DS.Tables[0].Rows[0]["Complete"].ToString();
                lblRejectAdmin.Text = objReply.DS.Tables[0].Rows[0]["Reject"].ToString();
                lblTotalTokenLabel.Text = "Total Issue Tokens";
            }
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetCount Error :- " + ex.ToString(), "");
        }
    }
    [WebMethod]
   public static string GetChartData()
    {
        string response = "";
        ArrayList AllWeekDate = new ArrayList();
        ArrayList Skip = new ArrayList();
        ArrayList Complete = new ArrayList();
        ArrayList Reject = new ArrayList();
        ArrayList daysName = new ArrayList();
        ReqChartData objReqChartData = new ReqChartData();
        try
        {
            //DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            DateTime Last7DayName = DateTime.Now.AddDays(-7);
            DayOfWeek OldDay = Last7DayName.DayOfWeek;
           // int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            int daysTillCurrentDay = 7;
            //DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            //objReqChartData.ToDT = currentWeekStartDate.ToString("yyyy-MM-dd 23:59:59.999");

            //  objReqChartData.date = new string[daysTillCurrentDay + 1];
            for (int i = 0; i < daysTillCurrentDay ; i++)
            {
              
                
                DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay+i);
                //AllWeekDate.Add(currentWeekStartDate.ToString("yyyy-MM-dd"));                
                objReqChartData.ToDT = currentWeekStartDate.ToString("yyyy-MM-dd");

                DayOfWeek currentDay = currentWeekStartDate.DayOfWeek;
                daysName.Add(currentDay);
                Reply objReply = new Reply();
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                string data = JsonConvert.SerializeObject(objReqChartData);
                string result = client.UploadString(URL + "/GetChartData", "POST", data);

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

                objReply = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objReply.Result)
                {
                    Complete.Add(objReply.DS.Tables[0].Rows[0]["Complete"].ToString());
                    Skip.Add(objReply.DS.Tables[0].Rows[0]["Skipp"].ToString());
                    Reject.Add(objReply.DS.Tables[0].Rows[0]["Reject"].ToString());
                }
            }
            
            for (int j = 0; j < daysTillCurrentDay; j++)
            {
                response = response + "#" + daysName[j].ToString() + "#" + Complete[j] + "#" + Skip[j] + "#" + Reject[j];
            }
            //string response = daysName[0].ToString() + "#" + Complete[0] + "#" + Skip[0] + "#" + Reject[0];
            //response = response +"#" + daysName[1].ToString() + "#" + Complete[1] + "#" + Skip[1] + "#" + Reject[0];
        }
        catch (Exception ex)
        {
            Log.Write("Exception in GetChartData Error :- " + ex.ToString(), "");
        }
        return response;
    }
}