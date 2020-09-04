<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title>Login | Terminal SecureServer </title>

    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Dashboard/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Dashboard/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="Dashboard/bower_components/Ionicons/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="Dashboard/dist/css/AdminLTE.min.css">
    <!-- iCheck -->
    <link rel="stylesheet" href="Dashboard/plugins/iCheck/square/blue.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <link href="Dashboard/css/Design.css" rel="stylesheet" />
    <%--  custom js--%>
    <script src="Dashboard/js/Custom.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
    <link rel="stylesheet" href="mystyle.css" />

    <script type="text/javascript">
        var isSubmitted = false;
        function GetName() {
            var newDate = new Date();
            if (!isSubmitted) {
                var username = document.getElementById('<%= txtUserName.ClientID%>').value;
                document.getElementById('<%= txtUserName.ClientID %>').value = data(username + "&" + newDate.getTime());
                var password = document.getElementById('<%= txtPassword.ClientID%>').value;
                document.getElementById('<%= txtPassword.ClientID %>').value = data(password + "*" + newDate.getTime());

                isSubmitted = true;

                return true;
            }
            else { return false; }
        }

    </script>

    <script type="text/javascript">
        function data(s) {
            var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
            var encodedString = Base64.encode(s);
            return encodedString;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input").change(function () {               
                var tboxid = $("#txtUserName").val();
                if ($("#txtUserName").val() == "admin") {
                    $("#divddl").css("display", "none");
                } else {
                    $("#divddl").css("display", "block");
                }
            });
        });

    </script>
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
                window.location = "Terminal.aspx";
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


</head>

<body class="hold-transition login-page" style="background-color: #E3E1E1;">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12 col-xs-12">
                <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                </div>
                <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                    <div class="card" style="margin-top: 50%">
                        <div class="card-header">
                            <h1 style="color: #A09A9A">QMS</h1>
                        </div>
                        <div class="card-body">
                            <p class="login-box-msg">Sign in to start your session</p>                          
                            <div class="form-group has-feedback">
                                <label>UserName</label>
                                <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="User Name"></asp:TextBox>
                                <span class="glyphicon glyphicon-user form-control-feedback"></span>
                            </div>
                            <div class="form-group has-feedback" style="margin-top: -20px">
                                <label>Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" class="form-control" oncopy="return false" onpaste="return false" oncut="return false" placeholder="Password" TextMode="Password"></asp:TextBox>
                                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                            </div>
                            <div class="form-group has-feedback" id="divddl">
                                <asp:DropDownList ID="cmbTerminalID" runat="server" class="form-control" placeholder="Terminal ID" AutoCompleteType="Disabled" autocomplete="off"></asp:DropDownList>
                                <span class="glyphicon glyphicon-user form-control-feedback"></span>
                            </div>

                            <div class="row">
                                <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                                </div>
                                <div class="col-xs-6">
                                    <asp:Button ID="btnLogin" runat="server" Text="Sign In" OnClientClick="return GetName();" BackColor="#337ab7" class="btn btn-primary btn-block btn-flat" OnClick="btnLogin_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <%--<div id="viewStage" style="position: relative; right: 0px;" >
        <div id="cube1" class="cube">
            <div class="facefront fb"></div>
            <div class="facebck fb"></div>
            <div class="faceleft lr"></div>
            <div class="faceright lr"></div>
            <div class="facetop tb"></div>
            <div class="facebottom tb"></div>
        </div>
        <div id="cube2" class="cube reverse">
            <div class="facefront fb"></div>
            <div class="facebck fb"></div>
            <div class="faceleft lr"></div>
            <div class="faceright lr"></div>
            <div class="facetop tb"></div>
            <div class="facebottom tb"></div>
        </div>
        <div id="cube3" class="cube">
            <div class="facefront fb"></div>
            <div class="facebck fb"></div>
            <div class="faceleft lr"></div>
            <div class="faceright lr"></div>
            <div class="facetop tb"></div>
            <div class="facebottom tb"></div>
        </div>
        <div id="cube4" class="cube reverse">
            <div class="facefront fb"></div>
            <div class="facebck fb"></div>
            <div class="faceleft lr"></div>
            <div class="faceright lr"></div>
            <div class="facetop tb"></div>
            <div class="facebottom tb"></div>
        </div>
        <div id="cube5" class="cube">
            <div class="facefront fb"></div>
            <div class="facebck fb"></div>
            <div class="faceleft lr"></div>
            <div class="faceright lr"></div>
            <div class="facetop tb"></div>
            <div class="facebottom tb"></div>
        </div>
    </div>--%>
    <!-- /.content-wrapper -->
    <footer class="main-footer" style="margin-left: 0px; color: #03a9f4; position: absolute; bottom: 0; width: 100%;">
        <div class="pull-right  hidden-xs image">
            <img style="height: 38px; width: 40px;" src="Dashboard/images/logo1.png" />
        </div>

        <div style="text-align: center; padding-left: 7%">
            <strong>Copyright &copy; 2020 <u>LIPI DATA SYSTEMS LTD</u>|</strong> All rights
            reserved.
        </div>
    </footer>


    <!-- jQuery 3 -->
    <script src="Dashboard/bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="Dashboard/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="Dashboard/plugins/iCheck/icheck.min.js"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '0%' /* optional */
            });
        });
    </script>
</body>
</html>

