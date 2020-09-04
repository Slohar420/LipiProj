<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="Services.aspx.cs" Inherits="Dashboard_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <style>
        /*body {font-family: Arial, Helvetica, sans-serif;}
* {box-sizing: border-box;}*/

        /* Full-width input fields */
        
    </style>
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
            }).then(function () {
                window.location = "Services.aspx";
            });
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
    <script type="text/javascript">
        function fn_validateNumeric() {
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            else return true;
            if (((keycode >= 65) && (keycode <= 90)) || ((keycode >= 97) && (keycode <= 122))) {
                return true;
            }
            else {
                return false;
            }
        }



    </script>
    <script type="text/javascript">
        function RestrictSpace() {
            if (event.keyCode == 32) {
                return false;
            }
        }

    </script>
    <script>
        function Delete(UserId) {
               
            swal({
                title: "Are you sure?",
                text: "Once deleted, you will not be able to recover this Project Type !",
                icon: "warning",
                buttons: true,
                dangerMode: true,               
            })
           .then((willDelete) => {
               if (willDelete) {
                   PageMethods.RegisterUser(UserId, onSucess, onError);
                   function onSucess(result) {
                       swal(result, {
                           icon: "success",
                       }).then(function () {
                           window.location = "Services.aspx";
                       })
                   }
                   function onError(result) {
                       swal(result, {
                           icon: "error",
                       }).then(function () {
                           window.location = "Services.aspx";
                       })
                   }
               } else {
                   swal("Your File is safe!");
               }
           });         
            //document.location.reload();
          }
      </script>
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Servise's
                  
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">User Management</a></li>
            <li class="active">Create User</li>
        </ol>
    </section>
    <!-- Main content -->
    <section class="content">

        <div id="id01" class="modal">
            <span onclick="document.getElementById('id01').style.display='none'" class="close" title="Close Modal">&times;</span>
            <div class="modal-content">
                <div class="container">
                    <div class="row">
                        <div class="card" style="width: 50%; margin: 20px; margin-left: 60px">
                            <div class="card-header">
                                <h4>Create Services</h4>
                            </div>
                            <div class="card-body">
                                <label for="psw"><b>Services</b></label>
                                <asp:TextBox ID="txtServices" class="form-control " placeholder="Enter Service Name" runat="server"   AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>

                                <div class="clearfix">
                                    <asp:Button runat="server" type="submit" ID="btnsubmit" OnClick="btnsubmit_Click" class="signupbtn button1" Style="background-color: #337ab7" Text="Create" />
                                    <button type="button" onclick="document.getElementById('id01').style.display='none'" class="cancelbtn">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <script>
            // Get the modal
            var modal = document.getElementById('id01');

            // When the user clicks anywhere outside of the modal, close it
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.style.display = "none";
                }
            }
        </script>



        <div class="row">
            <div class="col-lg-12 col-sm-12 col-md-12 col-xs-12">     
                <div class="col-lg-2 col-sm-2 col-md-2 col-xs-2"></div>    
                <div class="col-lg-8 col-sm-8 col-md-5 col-xs-8">
                    <div class="card">
                        <div class="card-header">
                            <div class="col-lg-11 col-sm-11 col-md-11 col-xs-11">
                                <h3 style="color:#A09A9A">Service's Details</h3>                            
                            </div>              
                             <div class="col-lg-1 col-sm-1 col-md-1 col-xs-1">
                            <button onclick="document.getElementById('id01').style.display='block'" type="button" class="btn btn-block" style="width: auto; float:right; background-color: #337ab7; font-size: 15px">Add<span class="fa fa-plus" style="margin-left: 5px; font-size: 10px; padding: 5px"></span></button>
                            </div>   
                        </div>
                        <div class="card-body" style="max-height: 700px; overflow-y: auto;">
                            <asp:GridView ID="GV_ServiceList" runat="server" EnableEventValidation="true" font-size= "Large" OnRowCommand="GV_ServiceList_RowCommand"  OnRowDataBound="GV_ServiceList_RowDataBound" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
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
                                     <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150">
                                        <ItemStyle Width="250px" Font-Bold="false"></ItemStyle>
                                    </asp:BoundField>                                      
                                    <asp:TemplateField HeaderText="Change Status"  ItemStyle-Width="200">
                                        <ItemTemplate>
                                            <asp:Button Text="Change" ID="btnChangeStatus" Visible="true"  class="btn btn-danger" CommandArgument="<%# Container.DataItemIndex %>" runat="server" CommandName="ChangeStatus" />
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
                        </div>
                    </div>   
                </div>       
                                 
            </div>
        </div>
    </section>
</asp:Content>

