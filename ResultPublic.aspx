<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ResultPublic.aspx.cs" Inherits="EVotingSystem.ResultPublic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Election Results</h2>
    <div class="card">
        <asp:Label ID="lblMessage" runat="server" CssClass="alert-error"></asp:Label>
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridViewStyle">
                <Columns>
                    <asp:TemplateField HeaderText="Symbol">
                        <ItemTemplate>
                            <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' Width="44" Height="44" Style="object-fit: contain;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
                    <asp:BoundField DataField="VoteCount" HeaderText="Votes" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>