<%@ Page Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="PartyStatus.aspx.cs" Inherits="EVotingSystem.PartyStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">My Party Status</h2>
    <div class="card">
        <table class="info-table">
            <tr>
                <td>Party Name</td>
                <td><asp:Label ID="lblPartyName" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Registration Status</td>
                <td><asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr id="trDeclineReason" runat="server">
                <td>Decline Reason</td>
                <td><asp:Label ID="lblDeclineReason" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>
