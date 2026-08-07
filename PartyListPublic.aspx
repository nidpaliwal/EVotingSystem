<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PartyListPublic.aspx.cs" Inherits="EVotingSystem.PartyListPublic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Registered Parties</h2>
    <div class="card">
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridViewStyle">
                <Columns>
                    <asp:TemplateField HeaderText="Symbol">
                        <ItemTemplate>
                            <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' CssClass="symbol-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Leader Photo">
                        <ItemTemplate>
                            <asp:Image ID="imgLeader" runat="server" ImageUrl='<%# Eval("LeaderPhotoPath") %>' CssClass="symbol-rounded photo-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
                    <asp:BoundField DataField="LeaderName" HeaderText="Leader Name" />
                    <asp:BoundField DataField="Objective" HeaderText="Objective" />
                    <asp:BoundField DataField="LegalHistory" HeaderText="Legal History" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>