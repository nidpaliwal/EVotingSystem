<%@ Page Title="" Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="EVotingSystem.MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        Profile</p>
    <table class="auto-style4">
        <tr>
            <td>Party Name :</td>
            <td>
                <asp:Label ID="lblPartyName" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Leader Name :</td>
            <td>
                <asp:Label ID="lblLeaderName" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Status :</td>
            <td>
                <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr id="trDeclineReason" runat="server">
            <td id="tdDeclineReason">Decline Reason :</td>
            <td>
                <asp:Label ID="lblDeclineReason" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Objective :</td>
            <td>
                <asp:TextBox ID="TextBoxObjective" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Legal History :</td>
            <td>
                <asp:TextBox ID="TextBoxLegalHistory" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
<br />
<asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Save Changes" />
</asp:Content>
