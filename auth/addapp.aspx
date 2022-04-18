<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/account.Master" CodeBehind="addapp.aspx.vb" Inherits="auth.addapp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_B" runat="server">

       <div class="row">
         
                   <div class="col-md-8" cssclass="col-md-8">
              <div class="card">
                <div class="card-header">
                  <h4>APP接入申请</h4>
                </div>
                <div class="card-body p-0">
                    <div class="card-body">
                      <div class="form-group">
                        <label>APP名称</label>
                        <asp:TextBox ID="input_appname" runat="server"  CssClass ="form-control" ></asp:TextBox>
                      </div>
                      <div class="form-group">
                        <label>信息请求</label>
                          <asp:CheckBoxList ID="required" runat="server"  >
                               <asp:ListItem>用户昵称/头像</asp:ListItem>
                             <asp:ListItem>用户邮箱</asp:ListItem>
                          </asp:CheckBoxList>
                      </div>
                        <div class="form-group">
                        <label>重定向地址</label>
                        <asp:TextBox ID="redirect" runat="server"  CssClass ="form-control" ></asp:TextBox>
                      </div>
                      <div class="form-group">
                        <label>APP类型</label>
                        <asp:DropDownList  ID="app_calss"  runat="server" CssClass ="form-control">
                            <asp:ListItem>公有APP</asp:ListItem>
                            <asp:ListItem>私有APP</asp:ListItem>
                        </asp:DropDownList>
                      </div>
                    </div>
                    <div class="card-footer text-right">
                        <asp:Button CssClass="btn btn-primary" ID="btn_submit" runat="server" Text="提交" />
                    </div>
                </div>
              </div>
            </div>
            
          </div>
  
</asp:Content>
