<%@ Page Title="" Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterDashboard.aspx.cs" Inherits="EVotingSystem.VoterDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
    <h2>Welcome, <asp:Label ID="lblName" runat="server"></asp:Label></h2>
    <p>&nbsp;</p>
    <p>You are logged in as: <asp:Label ID="lblEmail" runat="server"></asp:Label></p>
    <p>Your Voter ID: <asp:Label ID="lblVoterID" runat="server"></asp:Label></p>
</asp:Content>
