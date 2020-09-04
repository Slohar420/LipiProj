<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="DashboardUser.aspx.cs" Inherits="Dashboard_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <link href="Chart/css/Chart.css" rel="stylesheet" />
    <link href="Chart/css/Chart.min.css" rel="stylesheet" />

    <script src="Chart/js/Chart.bundle.js"></script>
    <script src="Chart/js/Chart.bundle.min.js"></script>
    <script src="Chart/js/Chart.js"></script>
    <script src="Chart/js/Chart.min.js"></script>

    <script>

        function Success(add) {
            swal({
                title: add,
                icon: "success",
                customClass: 'swal-wide',
                buttons: {
                    confirm: { text: 'Ok', className: 'sweet-warning' },
                    cancel: { text: 'Delete', className: 'sweet-warning' },
                },
            })
        }
        function Failed(add) {
            swal({
                title: add,
                icon: "error",
                customClass: 'swal-wide',
                buttons: {
                    confirm: { text: 'Ok', className: 'sweet-warning' },
                    cancel: { text: 'Delete', className: 'sweet-warning' },
                },
            })
        }
        function Warning(add) {
            swal({
                title: add,
                icon: "warning",
                customClass: 'swal-wide',
                buttons: {
                    confirm: { text: 'Ok', className: 'sweet-warning' },
                    cancel: { text: 'Delete', className: 'sweet-warning' },
                },
            })
        }
    </script>
    <style>
        .button1 {
            background-color: #4CAF50;
            color: white;
            padding: 14px 20px;
            margin: 8px 0;
            border: none;
            cursor: pointer;
            width: 200%;
            opacity: 0.9;
        }

        .box {
            border: 2px solid red;
            width: 100%;
            height: 180px;
            margin-top: 45px;
        }

        .ddl {
            width: 200%;
        }

        .info-box {
            /*box-shadow: 0 0 1px rgba(0,0,0,.125), 0 1px 3px rgba(0,0,0,.2);*/
            /*border-radius: .25rem;*/
            background-color: #F6C34D;
            display: -ms-flexbox;
            display: flex;
            margin-bottom: 1rem;
            min-height: 80px;
            padding: .5rem;
            margin-top: 30%;
            margin: 10px;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite; /* Safari */
            animation: spin 2s linear infinite;
        }

        /* Safari */
        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <script type="text/javascript">
        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }
        }
    </script>

   

    <div class="row">
        <div class="col-lg-12 col-xs-12 col-md-12 col-sm-12">
            <div class="col-lg-12 col-xs-12 col-md-12 col-sm-12 ">
                <div class="card" runat="server" id="CardFatchToken">
                    <div class="card-header">
                        <h4>Fatch Tokens</h4>
                    </div>
                    <div class="card-body">
                        <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                            <div class="row" style="margin-top: 20px">
                                <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                                    <label for="ser"><b>Services</b></label>
                                    <asp:DropDownList runat="server" CssClass="form-control ddl" ID="ddlServices">
                                        <asp:ListItem>Select Services</asp:ListItem>
                                        <asp:ListItem>All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 20px" id="rowNeedtoskip" runat="server">
                                <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                                    <asp:CheckBox runat="server" ID="chkNeedToSkip"  AutoPostBack="true" />
                                    <label>Need To Skip</label>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 20px" id="rowFatchNextTokenbutton" runat="server">
                                <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                                    <asp:Button runat="server" ID="btnFetchNextToken" OnClick="btnFetchNextToken_Click" Text="Fetch Next Token" class="signupbtn button1" Style="background-color: #337ab7" />
                                </div>
                            </div>

                            <div class="row" style="margin-top: 20px" id="rowStatus" runat="server">
                                <div class="col-lg-5 col-sm-5 col-xs-5 col-md-5 ">
                                    <label for="ser"><b>Status</b></label><br />
                                    <asp:CheckBoxList ID="chkStatusList" class="custom-control custom-checkbox " runat="server">
                                        <asp:ListItem Text="Complete" Value="1" onclick="MutExChkList(this);" />
                                        <asp:ListItem Text="Skipped" Value="2" onclick="MutExChkList(this);" />
                                        <asp:ListItem Text="Reject" Value="2" onclick="MutExChkList(this);" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 20px" id="rowStatusbutton" runat="server">
                                <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                                    <asp:Button runat="server" ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click" class="signupbtn button1" Style="background-color: #337ab7" />
                                </div>
                            </div>

                        </div>
                        <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                            <div class="box">
                                <div id="divLblLastTokenFatechNo" style="text-align: center; margin-top: -7%" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblLastFatchTokenNo">0</asp:Label>
                                </div>
                                <div id="divLoader" class="loader" runat="server" visible="true" style="justify-content: center; margin: 2% 0 0 25%">
                                </div>
                                <p id="pMsgLoader" runat="server" style="margin: 2% 0 0 8%">Waiting For to be Next Token Fetch</p>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

           
        </div>
      
    </div>

  
</asp:Content>

