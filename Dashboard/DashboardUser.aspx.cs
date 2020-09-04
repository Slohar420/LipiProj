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
    protected void Page_Load(object sender, EventArgs e)
    {
     
        if (Session["TerminalID"] == null)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Session Null');window.location ='../Default.aspx';", true);
            return;
        }
        if (!IsPostBack)
        {
            TID = Session["TerminalID"].ToString();      
            
            

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

                    divLoader.Visible = false;
                    pMsgLoader.Visible = false;
                    divLblLastTokenFatechNo.Visible = true;
                   
                }
                else
                {
                    add = "No Token Available This Time!";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                    divLoader.Visible = true;
                    pMsgLoader.Visible = true;
                    divLblLastTokenFatechNo.Visible = false;
                
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
              
                divLoader.Visible = true;
                pMsgLoader.Visible = true;
                divLblLastTokenFatechNo.Visible = false;
              
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

   
}