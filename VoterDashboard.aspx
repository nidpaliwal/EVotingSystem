<%@ Page Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterDashboard.aspx.cs" Inherits="EVotingSystem.VoterDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Welcome, <asp:Label ID="lblName" runat="server"></asp:Label></h2>
    <div class="card">
        <table class="info-table">
            <tr>
                <td>Logged in as</td>
                <td><strong><asp:Label ID="lblEmail" runat="server"></asp:Label></strong></td>
            </tr>
            <tr>
                <td>Voter ID</td>
                <td><strong><asp:Label ID="lblVoterID" runat="server"></asp:Label></strong></td>
            </tr>
        </table>
    </div>
    <h3 class="section-title">Quick actions</h3>
    <div class="quick-links">
        <a class="quick-link" href="VoterVote.aspx">Vote Now<span>Cast your vote in the active election</span></a>
        <a class="quick-link" href="VoterStatus.aspx">My Status<span>Check your registration and voting status</span></a>
        <a class="quick-link" href="PartyListVoter.aspx">Party List<span>View registered parties and symbols</span></a>
        <a class="quick-link" href="VoterElectionHistory.aspx">Results<span>See election results after voting closes</span></a>
    </div>
</asp:Content>
