<%@ Page Title="" Language="C#" MasterPageFile="~/Site3Party.Master" AutoEventWireup="true" CodeBehind="ResultParty.aspx.cs" Inherits="EVotingSystem.ResultParty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        Election Results</p>
    <p>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

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
</asp:Content>
