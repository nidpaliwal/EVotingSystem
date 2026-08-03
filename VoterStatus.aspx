<%@ Page Title="" Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterStatus.aspx.cs" Inherits="EVotingSystem.VoterStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        My Status</p>
    <table class="auto-style4">
        <tr>
            <td>&nbsp;Name :&nbsp;</td>
            <td>
                <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Registration Status :</td>
            <td>
                <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trDeclineReason">
            <td>Decline Reason :</td>
            <td>
                <asp:Label ID="lblDeclineReason" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Voting Status :</td>
            <td>
                <asp:Label ID="lblHasVoted" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
