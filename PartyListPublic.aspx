<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PartyListPublic.aspx.cs" Inherits="EVotingSystem.PartyListPublic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        Registered Parties</p>
    <p>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Symbol">
                <ItemTemplate>
                    <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' Width="50" Height="50" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
            <asp:BoundField DataField="LeaderName" HeaderText="Leader Name" />
            <asp:BoundField DataField="Objective" HeaderText="Objective" />
            <asp:BoundField DataField="LegalHistory" HeaderText="Legal History" />
        </Columns>
    </asp:GridView>&nbsp;</p>
</asp:Content>
