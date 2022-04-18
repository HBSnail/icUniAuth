<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="register.aspx.vb" Inherits="auth.Register" %>

<html>
    
<script src="Scripts/zepto.js"></script>
<script type="text/javascript" src="Scripts/dialog.js"></script>

     <script type="text/javascript" language="javascript">
         function Error(a) {
             popup({ type: 'error', msg: a, delay: 2000, width: 350, height: 160, callBack: function () { } });
         };
     </script>
 <script type="text/javascript" language="javascript">
     function Success(a,b) {
         popup({ type: 'success', msg: a, delay: 2000, width: 350, height: 160, callBack: function () { window.location.href = b } });
     };
 </script>
<head>
    <title>注册 - icUniAuth统一认证</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta charset="UTF-8" />
   <link rel="stylesheet" href="css/style.css" type="text/css" media="all" />
    <link rel="stylesheet" type="text/css" href="css/dialog.css">
    <link rel="stylesheet" href="css/font-awesome.min.css" type="text/css" media="all">

</head>
 
<body background="images/back.png">
   <form id="form1" runat="server">

    <section class="w3l-hotair-form">
        <h1 style="margin-top: 20px">icUniAuth统一认证</h1>
        <div class="container">
            <div class="workinghny-form-grid">
                <div class="main-hotair">
                    <div class="content-wthree">
                        <h2>注册</h2>
                        <p>&nbsp;</p>
                            <asp:TextBox ID="input_username" runat="server" placeholder="请输入邮箱"></asp:TextBox>
                            <asp:TextBox ID="input_password" runat="server"  TextMode="Password" placeholder="请输入密码"></asp:TextBox>
                            <asp:TextBox ID="input_repassword" runat="server"  TextMode="Password" placeholder="请再次输入密码"></asp:TextBox>
                            <button ID="btn_reg" class="btn" type="submit"  runat="server" onserverclick="reg_click">注册</button>
                        <script type="text/javascript">
                            $('#btn_reg').click(function () {
                                popup({
                                    type: 'load', msg: "正在提交请稍等", delay: null
                                });
                            })
                        </script>

                        <br />
                        <p class="account">已有账号？<a href="login.aspx">登录</a> </p>
                    </div>
                   
                </div>
            </div>
        </div>
         <div class="copyright text-center">
            <p class="copy-footer-29">Copyright © <%: Now.Year.ToString %> icSecLab. All rights reserved.</p>
        </div>
    </section>
        </form>
</body>         

</html>