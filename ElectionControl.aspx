<%@ Page Title="" Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="ElectionControl.aspx.cs" Inherits="EVotingSystem.ElectionControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 100%;
        }
        .auto-style5 {
            height: 33px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
    <br />
    Election Control</p>
    <table class="auto-style4">
        <tr>
            <td>Election Title :</td>
            <td>
                <asp:TextBox ID="TextBoxTitle" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Start Date :</td>
            <td>
                <asp:TextBox ID="TextBoxStart" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">End Date :</td>
            <td class="auto-style5">
                <asp:TextBox ID="TextBoxEnd" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Create Election" OnClick="Button1_Click" />
    <br />
    <br />
    Existing Elections<br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
        DataKeyNames="ElectionID" OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:BoundField DataField="ElectionID" Visible="false" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" />
            <asp:BoundField DataField="IsActive" HeaderText="Active" />
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnActivate" runat="server" Text="Set Active" CommandName="Activate" CommandArgument='<%# Eval("ElectionID") %>' />
                    <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CommandName="Deactivate" CommandArgument='<%# Eval("ElectionID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
