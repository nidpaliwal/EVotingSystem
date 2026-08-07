<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ResultPublic.aspx.cs" Inherits="EVotingSystem.ResultPublic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Election Results</h2>
    <asp:Label ID="lblMessage" runat="server" CssClass="alert-error"></asp:Label>
    <div class="card" id="divSessionInfo" runat="server">
        <h3>Election Session Details</h3>
        <table class="info-table">
            <tr>
                <td>Election Title</td>
                <td><strong><asp:Label ID="lblElectionTitle" runat="server"></asp:Label></strong></td>
            </tr>
            <tr>
                <td>Election Period</td>
                <td><asp:Label ID="lblElectionPeriod" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Authority Name</td>
                <td><asp:Label ID="lblAuthorityName" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Authority No.</td>
                <td><asp:Label ID="lblAuthorityNumber" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Status</td>
                <td><asp:Label ID="lblElectionStatus" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Total Votes Cast</td>
                <td><asp:Label ID="lblTotalVotes" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Total Approved Voters</td>
                <td><asp:Label ID="lblTotalVoters" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
    <div class="card">
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridViewStyle">
                <Columns>
                    <asp:TemplateField HeaderText="Symbol">
                        <ItemTemplate>
                            <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' CssClass="symbol-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Leader">
                        <ItemTemplate>
                            <asp:Image ID="imgLeader" runat="server" ImageUrl='<%# Eval("LeaderPhotoPath") %>' CssClass="symbol-rounded photo-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LeaderName" HeaderText="Leader Name" />
                    <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
                    <asp:BoundField DataField="VoteCount" HeaderText="Votes" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
