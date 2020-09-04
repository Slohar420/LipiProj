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
using LipiRMS;
using System.Data;
using Newtonsoft.Json;

public partial class IssueTerminalList : System.Web.UI.Page
{
    string add = "";
    public static string LastTokenNo = "";
    public static int rowNo = 0;
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetServiceList();         
            
            try
            {
                WebClient objWC = new WebClient();
                objWC.Headers[HttpRequestHeader.ContentType] = "text/json";

                string result = objWC.UploadString(URL + "/GetIssueTerminalsList", "POST", "");

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespIssueTerminalsList));
                RespIssueTerminalsList objRespGetProcessingTerminalsList = (RespIssueTerminalsList)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objRespGetProcessingTerminalsList.Result && objRespGetProcessingTerminalsList.Terminals.Count() > 0)
                {
                    for (int iIndex = 0; iIndex < objRespGetProcessingTerminalsList.Terminals.Count(); iIndex++)
                    {
                        ddlIssueTerminalList.Items.Add(objRespGetProcessingTerminalsList.Terminals[iIndex].TerminalName);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                add = " Terminal List Not Found ! ";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                Log.Write("Terminal Create Failed", "");
                Log.Write("Exception in GetProcessingTerminalsList Error :- " + ex.ToString(), "");
            }
        }        
        
    }
    protected void GetServiceList()
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

                    DataTable dt = new DataTable();
                    dt.Columns.Add("S No");
                    dt.Columns.Add("Service");
                    dt.Columns.Add("pk_SM_Id");
                    
                    for (int i = 0; i < objReply.DS.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["Service"] = objReply.DS.Tables[0].Rows[i]["ServiceName"].ToString();
                        row["pk_SM_Id"] = objReply.DS.Tables[0].Rows[i]["pk_SM_Id"].ToString();                        
                        dt.Rows.Add(row);
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
                    cell.Style.Add("padding", " 15px");

                }
            }

        }
        catch (Exception ex)
        {
            Log.Write("Exception in GV_UserList_RowDataBound Error :- " + ex.ToString(), "");
        }
    }
    protected void GV_ServiceList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (ddlIssueTerminalList.SelectedIndex == 0)
            {
                add = "Please Select Terminal Id !";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Warning('" + add + "');", true);
                return;

            }
            ReqIssueToken objReqIssueToken = new ReqIssueToken();
            RespIssueToken objRespIssueToken = new RespIssueToken();
            if (e.CommandName == "IssueToken")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GV_ServiceList.Rows[rowIndex];
                string ServiceName = row.Cells[1].Text;

                objReqIssueToken.TerminalID = ddlIssueTerminalList.Text;
                objReqIssueToken.ServiceType = ServiceName;
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "Application/Json";

                var data = JsonConvert.SerializeObject(objReqIssueToken);
                string result1 = client.UploadString(URL + "/IssueToken", "POST", data);

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result1));
                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespIssueToken));
                objRespIssueToken = (RespIssueToken)objJsonSerRecv.ReadObject(memstrToReceive);

                if (objRespIssueToken.Result)
                {
                    Label tokenNo = (Label)row.FindControl("lblTokenNo");
                    tokenNo.Text = objRespIssueToken.TokenNo;
                    add = "Token Issue Successfully !";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Success('" + add + "');", true);
                }
                else
                {
                    add = "Token Issue Failed !";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Failed('" + add + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertmsg", ex.ToString(), true);
        }
    }

}