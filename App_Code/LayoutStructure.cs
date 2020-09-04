using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for LayoutStructure
/// </summary>
public class LayoutStructure
{
    public int height;
    public int width;
    public int xPos;
    public int yPos;
    public string Desc;
    public string name;
    public string type;

}
public class TickerStructure
{
    public string TickDesc;
    public string TickFunc;
    public string divId;
    public string labID;
}
public class Reply1
{
    public DataSet DS { get; set; }

    public bool res { get; set; }

    public int DeviceCount { get; set; }
    public string strError { get; set; }
}

public class BroadcastTemplate
{
    public string[] KioksId;
    public string TemplateName { get; set; }
    public string StartDateTime { get; set; }
    public string EndDateTime { get; set; }
    public bool Instant { get; set; }

}


public class CommandIniUpdate
{
    public string[] KioskIP;

    public string[] MachineSrNo;

    public string CommandCount;
    public string Command { get; set; }
    public bool Instant { get; set; }

}

public class PatchUpdateINI
{
    public string[] KioskIP;

    public string[] MachineID;

    public string patch;
    public string PatchName { get; set; }
    public string StartDateTime { get; set; }
    public string EndDateTime { get; set; }
    public bool Instant { get; set; }

}

public class RegistryIni
{
    public string[] KioskIP;
    public string val;
    public string patchname;
}
public class UserList
{    
    public bool Result { get; set; }    
    public string Error { get; set; }    
    public DataSet DS { get; set; }
}
public class UserDetailsRes
{  
    public bool Result { get; set; }   
    public string Error { get; set; }

}
public class UserDetailsReq
{ 
    public string UserId { get; set; }   
    public string Type { get; set; }
    public string Password { get; set; }
}
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
    public bool Result { get; set; }


    public string Error { get; set; }

    public DataSet DS { get; set; }
}
public class RespAddService
{
    public bool Result { get; set; }
    public string Error { get; set; }
}
public class ReqAddTerminal
{
    public string TerminalID { get; set; }
    public string Type { get; set; }
}
public class RespAddTerminal
{   
    public bool Result { get; set; }
    public string Error { get; set; }
}
public class RespIssueTerminalsList
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
public class ReqIssueToken
{
    public string TerminalID { get; set; }
    public string ServiceType { get; set; }


}
public class RespIssueToken
{
    public bool Result { get; set; }
    public string Error { get; set; }
    public string TokenNo { get; set; }
}
public class ReqFetchNextToken
{
    public string ServiceType { get; set; }
    public string TerminalID { get; set; }
    public string UserName { get; set; }
    public bool IsNeedToSkip { get; set; }
}
public class RespFetchNextToken
{
    public bool Result { get; set; }
    public string Error { get; set; }
    public string TokenNo { get; set; }
}
public class ReqUpdateTokenStatus
{
    public string TokenNo { get; set; }
    public string Status { get; set; }
}
public class RespUpdateTokenStatus
{
    public bool Result { get; set; }
    public string Error { get; set; }
}
public class ReqServiceWithTokenStatus
{
    public string ServiceId { get; set; }
    public string TerminalId { get; set; }

}
public class ReqChartData
{
    public string ToDT { get; set; }

    public string FromDT { get; set; }
}