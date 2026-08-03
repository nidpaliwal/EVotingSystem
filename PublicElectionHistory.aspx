<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PublicElectionHistory.aspx.cs" Inherits="EVotingSystem.PublicElectionHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        Election Results</p>
    <p>
        <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <div class="election-history-item">
                <a href='ResultPublic.aspx?id=<%# Eval("ElectionID") %>'>
                    <%# Eval("Title") %> (<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %>to <%# Eval("EndDate", "{0:dd-MMM-yyyy}") %>)
                </a>
                <%# (bool)Eval("IsActive") && Convert.ToDateTime(Eval("EndDate")) > DateTime.Now ? " — Result will be out soon" : "" %>
            </div>
        </ItemTemplate>
    </asp:Repeater>&nbsp;</p>
</asp:Content>
