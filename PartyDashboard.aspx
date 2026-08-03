<%@ Page Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="PartyDashboard.aspx.cs" Inherits="EVotingSystem.PartyDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Welcome, <asp:Label ID="lblLeaderName" runat="server"></asp:Label></h2>
    <div class="card">
        <p>Logged in as: <strong><asp:Label ID="lblEmail" runat="server"></asp:Label></strong></p>
    </div>
    <h3 class="section-title">Quick actions</h3>
    <div class="quick-links">
        <a class="quick-link" href="MyProfile.aspx">My Profile<span>View and update your party details</span></a>
        <a class="quick-link" href="PartyStatus.aspx">Registration Status<span>Check approval status of your party</span></a>
        <a class="quick-link" href="PartyElectionHistory.aspx">Results<span>See election results after voting closes</span></a>
    </div>
</asp:Content>
