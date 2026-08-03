<%@ Page Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterElectionHistory.aspx.cs" Inherits="EVotingSystem.VoterElectionHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Election History</h2>
    <div class="card">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <div class="election-history-item">
                    <a href='ResultVoter.aspx?id=<%# Eval("ElectionID") %>'>
                        <%# Eval("Title") %> (<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %> to <%# Eval("EndDate", "{0:dd-MMM-yyyy}") %>)
                    </a>
                    <span class="history-meta"><%# (bool)Eval("IsActive") && Convert.ToDateTime(Eval("EndDate")) > DateTime.Now ? "Result will be out soon" : "View results" %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>