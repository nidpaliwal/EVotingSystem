<%@ Page Title="" Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="AdminElectionHistory.aspx.cs" Inherits="EVotingSystem.AdminElectionHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        Election History</p>
    <p>
        <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <div class="election-history-item">
                <a href='ResultAdmin.aspx?id=<%# Eval("ElectionID") %>'>
                    <%# Eval("Title") %> (<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %>to <%# Eval("EndDate", "{0:dd-MMM-yyyy}") %>)
                </a>
                <%# (bool)Eval("IsActive") ? " — Ongoing (live)" : "" %>
            </div>
        </ItemTemplate>
    </asp:Repeater>&nbsp;</p>
</asp:Content>
