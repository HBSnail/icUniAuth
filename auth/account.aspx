<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/account.Master" CodeBehind="account.aspx.vb" Inherits="auth.account" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_B" runat="server">
     <div class="row">
         
                   <div class="col-md-8" cssclass="col-md-8">
              <div class="card">
                <div class="card-header">
                  <h4>授权列表</h4>
                </div>
                <div class="card-body p-0">
                     <asp:Panel ID="Panel1" runat="server">
                      </asp:Panel>
                  <div class="table-responsive table-invoice">
                    <table class="table table-striped">
                      <tbody><tr>
                        <th>应用</th>
                        <th>令牌</th>
                        <th>信息</th>
                        <th>时间</th>
                        <th>操作</th>
                      </tr>

                       <% Response.Write(tabledata) %>
                    </tbody></table>
                  </div>
                </div>
              </div>
            </div>
            
          </div>


</asp:Content>
