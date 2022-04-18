<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="delete.aspx.vb" Inherits="auth.delete" MasterPageFile="~/account.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_B" runat="server">
   
<html>
<script src="Scripts/zepto.js"></script>
<script type="text/javascript" src="Scripts/dialog.js"></script>

     <script type="text/javascript" language="javascript">
         function ErrorRedirect(a) {
             popup({ type: 'error', msg: a, delay: 2000, width: 350, height: 160, callBack: function () { window.location.href ="<%=redirect%>" }});
         };
     </script>
    <script type="text/javascript" src="Scripts/dialog.js"></script>

     <script type="text/javascript" language="javascript">
         function Error(a) {
             popup({ type: 'error', msg: a, delay: 2000, width: 350, height: 160});
         };
     </script>
 <script type="text/javascript" language="javascript">
     function SuccessRedirect(a) {
         popup({ type: 'success', msg: a, delay: 2000, width: 350, height: 160, callBack: function () { window.location.href = "<%=redirect%>" }});
     };
 </script>
<head>
    <title>销户 - icUniAuth统一认证</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta charset="UTF-8" />
   <link rel="stylesheet" href="css/style.css" type="text/css" media="all" />
    <link rel="stylesheet" type="text/css" href="css/dialog.css">
    <link rel="stylesheet" href="css/font-awesome.min.css" type="text/css" media="all">

</head>
 
<body>
  

     <section class="w3l-hotair-form" style="height:2px">
        <h1 style="margin-top: 20px">icUniAuth统一认证</h1>
        <div class="container">
             <asp:Panel ID="Panel_act" runat="server" Visible ="false" >
                                         <div class="workinghny-form-grid">
                <div class="main-hotair">
                    <div class="content-wthree">
                        <h2>销户</h2>

                     
                            <p>
                                &nbsp;</p>
                            <asp:TextBox ID="input_email" runat="server" placeholder="请输入注册邮箱"></asp:TextBox>
                         <asp:TextBox ID="input_username" runat="server" placeholder="请输入用户UID(在激活邮件中提供)"></asp:TextBox>
                            <button ID="btn_delete" class="btn" type="submit"  runat="server" onserverclick="delete_click">销户</button>
                        <script type="text/javascript">
                            $('#btn_delete').click(function () {
                                popup({
                                    type: 'load', msg: "正在提交请稍等", delay: null
                                });
                            })
                        </script>
                         <p class="account">点错了？<a href="default.aspx">取消</a> 
                        <br />
                    </div>
                   
                </div>
            </div>
                        </asp:Panel>
           
        </div>
        
    </section>
</body>         

</html>
     </asp:Content>