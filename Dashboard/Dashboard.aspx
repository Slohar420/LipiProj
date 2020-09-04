<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard_Dashboard" %>

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
            <div class="col-md-3">
                <div class="info-box">
                    <span class=" icon-box">
                        <i class="fa fa-clone"></i>
                    </span>
                    <div class="box-content">
                        <b><span class="info-box-number">
                            <asp:Label runat="server" ID="lblTotalTokens" Text="0"></asp:Label>
                            <asp:Label runat="server" ID="lblTotalTokenAdmin" Text="0" Visible="false"></asp:Label>
                        </span></b>
                        <span class="info-box-text">
                            <asp:Label runat="server" ID="lblTotalTokenLabel"></asp:Label>
                        </span>

                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="info-box" style="background-color: #58ADED">
                    <span class=" icon-box">
                        <i class="fa fa-window-restore"></i>
                    </span>
                    <div class="box-content">
                        <b><span class="info-box-number">
                            <asp:Label runat="server" ID="lblComplete" Text="0"></asp:Label>
                            <asp:Label runat="server" ID="lblCompleteAdmin" Text="0" Visible="false"></asp:Label>
                        </span></b>
                        <span class="info-box-text">Complete Tokens</span>

                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="info-box" style="background-color: #0D8386">
                    <span class=" icon-box">
                        <i class="fa fa-share"></i>
                    </span>
                    <div class="box-content">
                        <b><span class="info-box-number">
                            <asp:Label runat="server" ID="lblSkip" Text="0"></asp:Label>
                            <asp:Label runat="server" ID="lblSkipAdmin" Text="0" Visible="false"></asp:Label>
                        </span></b>
                        <span class="info-box-text">Skip Tokens</span>

                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="info-box" style="background-color: #D62D2D">
                    <span class=" icon-box">
                        <i class="fa fa-frown-o"></i>
                    </span>
                    <div class="box-content">
                        <b><span class="info-box-number">
                            <asp:Label runat="server" ID="lblReject" Text="0"></asp:Label>
                            <asp:Label runat="server" ID="lblRejectAdmin" Text="0" Visible="false"></asp:Label>
                        </span></b>
                        <span class="info-box-text">Reject Tokens</span>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12 col-xs-12 col-md-12 col-sm-12">
            <div class="col-lg-6 col-xs-6 col-md-6 col-sm-6 ">
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
                                    <asp:CheckBox runat="server" ID="chkNeedToSkip" OnCheckedChanged="chkNeedToSkip_CheckedChanged" AutoPostBack="true" />
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

            <div class="col-lg-6 col-xs-6 col-md-6 col-sm-6 ">
                <div class="card" id="cardTokenStatus" runat="server">
                    <div class="card-header">
                        <h4>Tokens Status</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 col-xs-12 col-md-12">
                                <asp:GridView ID="GV_ServiceList" runat="server" OnRowDataBound="GV_ServiceList_RowDataBound" EnableEventValidation="true" Font-Size="Large" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                                    <AlternatingRowStyle BackColor="white" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.NO." ItemStyle-Width="100" ItemStyle-Font-Bold="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Service" HeaderText="Service" ItemStyle-Width="150">
                                            <ItemStyle Width="250px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Skip" HeaderText="Skip Token" ItemStyle-Width="150">
                                            <ItemStyle Width="250px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Complete" HeaderText="Complete Token" ItemStyle-Width="150">
                                            <ItemStyle Width="250px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Reject" HeaderText="Reject Token" ItemStyle-Width="150">
                                            <ItemStyle Width="250px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="" HeaderText="pk_SM_Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" />
                                    <HeaderStyle BackColor="#337ab7" Font-Bold="true" ForeColor="white" HorizontalAlign="Center" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="true" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                    <SortedAscendingHeaderStyle BackColor="#848384" />
                                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                    <SortedDescendingHeaderStyle BackColor="#575357" />
                                </asp:GridView>
                            </div>
                        </div>


                        <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                            <div class="row">
                            </div>

                            <div class="row">
                            </div>

                            <div class="row">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-12 col-xs-12 col-md-12 col-sm-12">
            <div class="col-lg-6 col-xs-6 col-md-6 col-sm-6 ">
                <div class="card">
                    <div class="card-header">
                    </div>
                    <div class="card-body">
                   <canvas id="bar-chart-grouped" width="800" height="568"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-xs-6 col-md-6 col-sm-6 ">
                <div class="card" id="cardAdminTokenStatus" runat="server">
                    <div class="card-header">
                        <h4>Tokens Status</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 col-xs-12 col-md-12">
                                <asp:GridView ID="GV_AdminTokenStatus" runat="server" OnRowDataBound="GV_ServiceList_RowDataBound" EnableEventValidation="true" Font-Size="Large" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                                    <AlternatingRowStyle BackColor="white" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.NO." ItemStyle-Width="100" ItemStyle-Font-Bold="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Service" HeaderText="Service" ItemStyle-Width="150">
                                            <ItemStyle Width="350px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Skip" HeaderText="Skip Token" ItemStyle-Width="150">
                                            <ItemStyle Width="300px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Complete" HeaderText="Complete Token" ItemStyle-Width="150">
                                            <ItemStyle Width="300px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Reject" HeaderText="Reject Token" ItemStyle-Width="150">
                                            <ItemStyle Width="300px" Font-Bold="false"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="" HeaderText="pk_SM_Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" />
                                    <HeaderStyle BackColor="#337ab7" Font-Bold="true" ForeColor="white" HorizontalAlign="Center" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="true" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                    <SortedAscendingHeaderStyle BackColor="#848384" />
                                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                    <SortedDescendingHeaderStyle BackColor="#575357" />
                                </asp:GridView>
                            </div>
                        </div>

                          
                        <div class="col-lg-6 col-sm-6 col-xs-6 col-md-6 ">
                            <div class="row">
                            </div>

                            <div class="row">
                            </div>

                            <div class="row">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
     <script type='text/javascript'>
      
             function GetName() {                
                 PageMethods.GetChartData(Success, Failure);
             }
             function Success(result) {
                 var val = result;
             }

             function Failure(error) {
                 alert(error);
             }
         
           
        </script> 
    <script>

               

        window.onload = function () {

            new Chart(document.getElementById("bar-chart-grouped"), {
                type: 'bar',
                data: {
                    labels: ["Monday", "Thusday", "Wensday", "Thrusday", "Friday", "Saturday"],
                    datasets: [
                      {
                          label: "Complete",
                          backgroundColor: "#58ADED",
                          data: [10, 15, 25, 15]
                      }, {
                          label: "Skip",
                          backgroundColor: "#0D8386",
                          data: [13, 14, 2, 6]
                      }, {
                          label: "Reject",
                          backgroundColor: "#D62D2D",
                          data: [5, 6, 9, 10]
                      }
                    ]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Last 7 days Tokens Status'
                    }
                }
            })
        };
    </script>
  
</asp:Content>

