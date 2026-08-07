<%@ Page Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterVoteHistory.aspx.cs" Inherits="EVotingSystem.VoterVoteHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">My Vote History</h2>
    <div class="card">
        <asp:Label ID="lblMessage" runat="server" CssClass="alert-error"></asp:Label>
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridViewStyle">
                <Columns>
                    <asp:TemplateField HeaderText="Symbol">
                        <ItemTemplate>
                            <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' CssClass="symbol-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ElectionTitle" HeaderText="Election" />
                    <asp:BoundField DataField="ElectionPeriod" HeaderText="Election Period" />
                    <asp:BoundField DataField="PartyName" HeaderText="Voted For" />
                    <asp:BoundField DataField="VotedOn" HeaderText="Voted On" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" HtmlEncode="false" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>