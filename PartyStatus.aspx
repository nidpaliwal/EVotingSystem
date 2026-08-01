<%@ Page Title="" Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="PartyStatus.aspx.cs" Inherits="EVotingSystem.PartyStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style4 {
        width: 100%;
    }
    .auto-style5 {
        height: 31px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
    <br />
    My Party Status</p>
<table class="auto-style4">
    <tr>
        <td>Party Name :</td>
        <td>
            <asp:Label ID="lblPartyName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Registration Status :</td>
        <td>
            <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr id="trDeclineReason" runat="server">
        <td class="auto-style5">Decline Reason :</td>
        <td class="auto-style5">
            <asp:Label ID="lblDeclineReason" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
</table>
<p>
    &nbsp;</p>
</asp:Content>
