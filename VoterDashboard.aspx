<%@ Page Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterDashboard.aspx.cs" Inherits="EVotingSystem.VoterDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Welcome, <asp:Label ID="lblName" runat="server"></asp:Label></h2>
    <div class="card">
        <div class="info-table" style="max-width: 640px;">
            <div style="display: table; width: 100%;">
                <p style="margin: 6px 0;">Logged in as: <strong><asp:Label ID="lblEmail" runat="server"></asp:Label></strong></p>
                <p style="margin: 6px 0;">Voter ID: <strong><asp:Label ID="lblVoterID" runat="server"></asp:Label></strong></p>
            </div>
        </div>
    </div>
    <h3 style="margin: 4px 0 12px; color: var(--navy); font-size: 17px;">Quick actions</h3>
    <div class="quick-links">
        <a class="quick-link" href="VoterVote.aspx">Vote Now<span>Cast your vote in the active election</span></a>
        <a class="quick-link" href="VoterStatus.aspx">My Status<span>Check your registration and voting status</span></a>
        <a class="quick-link" href="PartyListVoter.aspx">Party List<span>View registered parties and symbols</span></a>
        <a class="quick-link" href="VoterElectionHistory.aspx">Results<span>See election results after voting closes</span></a>
    </div>
</asp:Content>
