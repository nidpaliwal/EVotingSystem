<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="EVotingSystem.Logout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card login-card">
        <h3>You have been logged out</h3>
        <p class="form-note">Your session has ended securely. Thank you for using the E-Voting System.</p>
        <div class="form-actions form-actions-center">
            <asp:Button ID="Button1" runat="server" Text="Back to Login" CssClass="btn btn-lg" OnClick="Button1_Click" />
        </div>
    </div>
</asp:Content>