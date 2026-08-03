<%@ Page Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="EVotingSystem.MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">My Profile</h2>
    <div class="card">
        <table class="info-table">
            <tr>
                <td>Party Name</td>
                <td><asp:Label ID="lblPartyName" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Leader Name</td>
                <td><asp:Label ID="lblLeaderName" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Status</td>
                <td><asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr id="trDeclineReason" runat="server">
                <td>Decline Reason</td>
                <td><asp:Label ID="lblDeclineReason" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Objective</td>
                <td><asp:TextBox ID="TextBoxObjective" runat="server" TextMode="MultiLine" MaxLength="1000"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Legal History</td>
                <td><asp:TextBox ID="TextBoxLegalHistory" runat="server" TextMode="MultiLine" MaxLength="1000"></asp:TextBox></td>
            </tr>
        </table>
        <div class="form-actions">
            <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Save Changes" CssClass="btn" />
        </div>
    </div>
</asp:Content>
