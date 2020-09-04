using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

public partial class Dashboard_DSMaster : System.Web.UI.MasterPage
{
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public const string AntiXsrfTokenKey = "__AntiXsrfToken";
    public const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    public string _antiXsrfTokenValue;
    string healthpanel = System.Configuration.ConfigurationManager.AppSettings["HealthPanel"].ToString();
    string transactionpanel = System.Configuration.ConfigurationManager.AppSettings["TransactionPanel"].ToString();
    string adminpanel1 = System.Configuration.ConfigurationManager.AppSettings["AdminPanel"].ToString();
    string calllogpanel = System.Configuration.ConfigurationManager.AppSettings["CallLogPanel"].ToString();
    string user = System.Configuration.ConfigurationManager.AppSettings["UserManagement"].ToString();
    string dashboard = System.Configuration.ConfigurationManager.AppSettings["Dashboard"].ToString();
    static string UserName = "";
    static string TerminalId = "";


    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches in the view state form field in the Page.PreLoad
            //method.
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                _antiXsrfTokenValue = requestCookie.Value;

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,

                    //Add the Anti-XSRF token to the cookie value
                    Value = _antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                    responseCookie.Secure = true;

                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error catch : '" + ex.Message + "')</script>");
            //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Logout();
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Session Null');window.location ='../Default.aspx';", true);
            return;
        }
        if (!IsPostBack)
        {
            string strRole = Session["Role"].ToString();
            if (strRole.ToLower().Contains("admin"))
            {
                adminpanel.Visible = true;
                usermanagement.Visible = true;
            }
            else
            {
                adminpanel.Visible = false;
                usermanagement.Visible = false;
            }            
            TerminalId = "";
            UserName = "";            
            lblUserName.Text = lblUSName.Text = lblUname.Text = Session["Username"].ToString();
            TerminalId =  Session["TerminalID"].ToString();
            UserName = Session["Username"].ToString();

        }
    }
    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        try
        {

            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                // LNKLogOut.Visible = true;
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");

                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error catch : '" + ex.Message + "')</script>");
        }
    }
    public void Logout()
    {
        try
        {
            ReqLogout objReqLogout = new ReqLogout();
            objReqLogout.UserName =UserName;
            objReqLogout.TerminalID = TerminalId;
            WebClient objWC = new WebClient();
            objWC.Headers[HttpRequestHeader.ContentType] = "text/json";

            DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(ReqLogout));
            MemoryStream memStrToSend = new MemoryStream();
            objJsonSerSend.WriteObject(memStrToSend, objReqLogout);

            string data = Encoding.Default.GetString(memStrToSend.ToArray());

            string result = objWC.UploadString(URL + "/Logout", "POST", data);

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(RespLogout));
            RespLogout objRespLogout = (RespLogout)objJsonSerRecv.ReadObject(memstrToReceive);

            if (objRespLogout.Result)
            {

            }
            Session["Username"] = null;
            Session["TerminalID"] = null;
            Response.Redirect("../Default.aspx");
            return;
        }
        catch (Exception)
        {

            throw;
        }

    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Logout();
    }

}
public class ReqLogout
{
  
    public string UserName { get; set; }

   
    public string TerminalID { get; set; }
}


public class RespLogout
{
  
    public bool Result { get; set; }

    
    public string Error { get; set; }
}