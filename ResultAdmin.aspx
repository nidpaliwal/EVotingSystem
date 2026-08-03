<%@ Page Title="" Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="ResultAdmin.aspx.cs" Inherits="EVotingSystem.Result" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
    <br />
        Election
    Result</p>
    <p>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    </p>
<p>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridViewStyle">
        <Columns>
            <asp:TemplateField HeaderText="Symbol">
                <ItemTemplate>
                    <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' Width="50" Height="50" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
            <asp:BoundField DataField="VoteCount" HeaderText="Votes" />
        </Columns>
    </asp:GridView>&nbsp;</p>
<p>
    &nbsp;</p>
</asp:Content>
