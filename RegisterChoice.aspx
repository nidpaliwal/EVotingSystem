<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RegisterChoice.aspx.cs" Inherits="EVotingSystem.RegisterChoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        Register as</p>
    <p>
        <asp:Button ID="Button1" runat="server" Text="Register as Voter" OnClick="Button1_Click" />
    </p>
    <p>
        <asp:Button ID="Button2" runat="server" Text="Register as Party" OnClick="Button2_Click1" />
    </p>
</asp:Content>
