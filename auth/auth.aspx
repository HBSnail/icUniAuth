<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="auth.aspx.vb" Inherits="auth.auth" %>

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
    <title>激活 - icUniAuth统一认证</title>
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
             <asp:Panel ID="Panel_act" runat="server" Visible="False">
                                         <div class="workinghny-form-grid">
                <div class="main-hotair">
                    <div class="content-wthree">
                        <h2>授权申请</h2>

                     
                        <p>
                            <asp:Label ID="userid" runat="server" Text=""></asp:Label>
                        </p>
                            <p>
                                &nbsp;</p>
                            <p>
                                <asp:Label ID="userid0" runat="server" Text=""></asp:Label>
                        </p>
                       
                              <p>
                                  &nbsp;</p>
                        <table style="table-layout:fixed;width:350px;align-content:center">
                            <tr>
                                <td> <button style="background:#63ed7a" ID="btn_act" class="btn" type="submit"  runat="server" onserverclick="act_click">同意授权</button></td>
                                <td> <button style="background:#fc544b" ID="btn_notact" class="btn" type="submit"  runat="server" onserverclick="notact_click">拒绝授权</button></td>
                            </tr>
                        </table>
                       
                             
                         <script type="text/javascript">
                             $('#btn_notact').click(function () {
                                 popup({
                                     type: 'load', msg: "正在提交请稍等", delay: null
                                 });
                             })
                         </script>
                        <script type="text/javascript">
                            $('#btn_act').click(function () {
                                popup({
                                    type: 'load', msg: "正在提交请稍等", delay: null
                                });
                            })
                        </script>

                        <br />
                    </div>
                   
                </div>
            </div>
                        </asp:Panel>
           
        </div>
         <div class="copyright text-center">
            <p class="copy-footer-29">Copyright © <%: Now.Year.ToString %> icSecLab. All rights reserved.</p>
        </div>
    </section>
        </form>
</body>         

</html>