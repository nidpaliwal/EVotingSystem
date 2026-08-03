<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RegisterChoice.aspx.cs" Inherits="EVotingSystem.RegisterChoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Create an account</h2>
    <div class="card">
        <p class="form-note" style="font-size: 14px; margin-top: 0;">Choose the type of account you want to register. Voters must be 18 or older with a valid Aadhaar ID. Parties register with their leader details and party symbol.</p>
        <div class="choice-grid">
            <asp:Button ID="Button1" runat="server" Text="Register as Voter" OnClick="Button1_Click" CssClass="btn btn-lg" />
            <asp:Button ID="Button2" runat="server" Text="Register as Party" OnClick="Button2_Click1" CssClass="btn btn-secondary btn-lg" />
        </div>
    </div>
</asp:Content>
