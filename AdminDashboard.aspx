<%@ Page Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="EVotingSystem.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Welcome, Admin</h2>
    <div class="card">
        <p>You are logged in as <strong><asp:Label ID="lblEmail" runat="server"></asp:Label></strong>.</p>
    </div>
    <h3 style="margin: 4px 0 12px; color: var(--navy); font-size: 17px;">Quick actions</h3>
    <div class="quick-links">
        <a class="quick-link" href="ManageVoters.aspx">Manage Voters<span>Approve or decline voter registrations</span></a>
        <a class="quick-link" href="ManageParties.aspx">Manage Parties<span>Approve or decline party registrations</span></a>
        <a class="quick-link" href="ElectionControl.aspx">Election Control<span>Create and manage election windows</span></a>
        <a class="quick-link" href="AdminElectionHistory.aspx">View Results<span>See results of past and live elections</span></a>
    </div>
</asp:Content>
