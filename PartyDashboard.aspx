<%@ Page Title="" Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="PartyDashboard.aspx.cs" Inherits="EVotingSystem.PartyDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
    <h2>Welcome, <asp:Label ID="lblLeaderName" runat="server"></asp:Label></h2>
    <p>You are logged in as: <asp:Label ID="lblEmail" runat="server"></asp:Label></p></p>
</asp:Content>
