<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/account.Master" CodeBehind="myapp.aspx.vb" Inherits="auth.myapp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_B" runat="server">
            <div class="row">
            
                   <div class="col-md-8" cssclass="col-md-8">
              <div class="card">
                <div class="card-header">
                  <h4>我的APP</h4>
                </div>
                <div class="card-body p-0">
                  <div class="table-responsive table-invoice">
                    <table class="table table-striped" id="table-2">
                        <thead>
                          <tr>
                            <th>APP名称</th>
                            <th>公有令牌</th>
                            <th>私有令牌</th>
                            <th>请求信息</th>
                            <th>模式</th>
                            <th>操作</th>
                          </tr>
                        </thead>
                        <tbody>
                          <%
                              Response.Write(x)
                              %>
                        </tbody>
                      </table>
                  </div>
                </div>
              </div>
                       </div>
                </div>
</asp:Content>
