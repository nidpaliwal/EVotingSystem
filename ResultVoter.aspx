<%@ Page Title="" Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="ResultVoter.aspx.cs" Inherits="EVotingSystem.ResultVoter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
    <br />
    Result</p>
<p>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    </p>
    <p>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
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
