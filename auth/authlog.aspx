<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/account.Master" CodeBehind="authlog.aspx.vb" Inherits="auth.authlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_B" runat="server">
    <div class="row">
         
          <div class="col-md-8" cssclass="col-md-8">
              <div class="card">
                <div class="card-header">
                  <h4>授权日志</h4>
                  <div class="card-header-action">
                      <asp:Button ID="Button1" CssClass="btn btn-danger" runat="server" Text="清除日志" />
                  </div>
                </div>
                <div class="card-body p-0">
                  <div class="table-responsive table-invoice">
                    <table class="table table-striped">
                      <tbody><tr>
                        <th>应用/令牌</th>
                        <th>操作</th>
                        <th>状态</th>
                        <th>时间</th>
                      </tr>
                       <% Response.Write(tabledata) %>
                    </tbody></table>
                  </div>
                </div>
              </div>
            </div>
            
          </div>
</asp:Content>
