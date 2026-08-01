<%@ Page Title="" Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="ManageVoters.aspx.cs" Inherits="EVotingSystem.ManageVoters" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        Manage Voters</p>
    <p>
        <asp:Label ID="Label1" runat="server" Text="Enter Voter Name / ID :"></asp:Label>
&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Search" />
    </p>
    <p>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
         DataKeyNames="Email" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:BoundField DataField="VoterID" Visible="false" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="VoterIDNumber" HeaderText="Voter ID Number" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="Status" HeaderText="Status" />

                <asp:TemplateField HeaderText="Photo">
                    <ItemTemplate>
                        <asp:Image ID="imgPhoto" runat="server" ImageUrl='<%# Eval("PhotoPath") %>' Width="50" Height="50" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnApprove" runat="server" Text="Approve" CommandName="Approve" CommandArgument='<%# Eval("Email") %>' />
                        <asp:Button ID="btnDecline" runat="server" Text="Decline" CommandName="Decline" CommandArgument='<%# Eval("Email") %>' 
                            OnClientClick="return setDeclineReason(this);" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:HiddenField ID="hdnDeclineReason" runat="server" ClientIDMode="Static" />
        <script>
function setDeclineReason(btn) {
    var reason = prompt("Please enter a reason for declining:");
    if (reason === null || reason.trim() === "") {
        alert("A decline reason is required.");
        return false;
    }
    document.getElementById("hdnDeclineReason").value = reason;
    return true;
        }
    </script>
    </p>
</asp:Content>
