 <%@ Page Title="" Language="vb" AutoEventWireup="false"  CodeBehind="login.aspx.vb" Inherits="auth.Login" %>   

<html>


    
<script src="Scripts/zepto.js"></script>
<script type="text/javascript" src="Scripts/dialog.js"></script>

     <script type="text/javascript" language="javascript">
         function Error(a) {
             popup({ type: 'error', msg: a, delay: 2000, width: 350, height: 160, callBack: function () { } });
         };
     </script>
 <script type="text/javascript" language="javascript">
     function Success(a) {
         popup({ type: 'success', msg: a, delay: 2000, width: 350, height: 160, callBack: function () { window.location.href ="<%=redirect%>" } });
     };
 </script>
<head>
    <title>登录 - icUniAuth统一认证</title>
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
                        <h2>登录</h2>
                        <p>&nbsp;</p>
                            <asp:TextBox ID="input_username" runat="server" placeholder="请输入邮箱"></asp:TextBox>
&nbsp;                      <asp:TextBox ID="input_password" runat="server"  TextMode="Password" placeholder="请输入密码"></asp:TextBox>
                           <p>&nbsp;</p>
                            <button ID="btn_login" class="btn" type="submit"  runat="server" onserverclick="login_click">登录</button>
                        <script type="text/javascript">
                            $('#btn_login').click(function () {
                                popup({
                                    type: 'load', msg: "正在登录请稍等", delay: null
                                });
                            })
                        </script>

                        <br />
                        <p class="account">没有账号？<a href="register.aspx">注册</a> &nbsp;&nbsp;&nbsp; 忘记密码？<a href="reset.aspx">找回</a> </p>
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