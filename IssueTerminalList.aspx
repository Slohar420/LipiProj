<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IssueTerminalList.aspx.cs" Inherits="IssueTerminalList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Dashboard/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Dashboard/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="Dashboard/bower_components/Ionicons/css/ionicons.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Dashboard/dist/css/AdminLTE.min.css" />
    <!-- iCheck -->
    <link rel="stylesheet" href="Dashboard/plugins/iCheck/square/blue.css" />
    <link href="Dashboard/css/Design.css" rel="stylesheet" />
    <%--  custom js--%>
    <script src="Dashboard/js/Custom.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <link rel="stylesheet" href="mystyle.css" />
    <link href="Dashboard/css/Design.css" rel="stylesheet" />
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
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

</head>
<body style="background-color: #E3E1E1;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="col-lg-12 col-sm-12 col-xs-12 col-md-12">
            <div class="card">
                <div class="card-header">
                    <h2>Issue Terminal List</h2>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-1 col-sm-1 col-xs-1 col-md-1">
                        </div>
                        <div class="col-lg-10 col-sm-10 col-xs-10 col-md-10">
                            <div class="card" style="margin: 20px; padding: 20px">
                                <div class="col-lg-4 col-sm-4 col-xs-4 col-md-4 ">
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlIssueTerminalList">
                                        <asp:ListItem>Select Terminal</asp:ListItem>
                                    </asp:DropDownList>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-1 col-sm-1 col-xs-1 col-md-1">
                        </div>
                        <div class="col-lg-10 col-sm-10 col-xs-10 col-md-10">
                            <div class="card" style="margin: 20px; padding: 20px">
                                <div class="card-body" style="max-height: 500px; overflow-y: auto;">
                                    <%--<asp:UpdatePanel ID="updatepnl" runat="server">--%>
                                       <%-- <ContentTemplate>--%>
                                            <asp:GridView ID="GV_ServiceList" runat="server" OnRowDataBound="GV_ServiceList_RowDataBound" OnRowCommand="GV_ServiceList_RowCommand" EnableEventValidation="true" Font-Size="Large" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
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
                                                    <asp:TemplateField HeaderText="Issue Token" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:Button Text="Issue" ID="btnFatchToken" Visible="true" class="btn btn-danger" CommandArgument="<%# Container.DataItemIndex %>" runat="server" CommandName="IssueToken" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Token No" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblTokenNo"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="pk_SM_Id" HeaderText="pk_SM_Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundField>
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
                                       <%-- </ContentTemplate>--%>
                                      <%--  <Triggers>--%>
                                          <%--  <asp:PostBackTrigger ControlID="updatepnl" />--%>
                                       <%-- </Triggers>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
