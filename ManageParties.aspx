<%@ Page Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="ManageParties.aspx.cs" Inherits="EVotingSystem.ManageParties" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Manage Parties</h2>
    <div class="card">
        <div class="search-row">
            <asp:Label ID="Label1" runat="server" Text="Enter Party Name:"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" MaxLength="200"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Search" CssClass="btn" />
        </div>
    </div>
    <div class="card">
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
                DataKeyNames="Email" OnRowCommand="GridView1_RowCommand" CssClass="GridViewStyle">
                <Columns>
                    <asp:BoundField DataField="PartyID" Visible="false" />
                    <asp:TemplateField HeaderText="Symbol">
                        <ItemTemplate>
                            <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' CssClass="symbol-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
                    <asp:TemplateField HeaderText="Leader Photo">
                        <ItemTemplate>
                            <asp:Image ID="imgLeader" runat="server" ImageUrl='<%# Eval("LeaderPhotoPath") %>' CssClass="symbol-rounded photo-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LeaderName" HeaderText="Leader Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:TemplateField HeaderText="Objective">
                        <ItemTemplate>
                            <div class="cell-long" title='<%# Eval("Objective") %>'><%# Eval("Objective") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Legal History">
                        <ItemTemplate>
                            <div class="cell-long" title='<%# Eval("LegalHistory") %>'><%# Eval("LegalHistory") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="status-badge"><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success" CommandName="Approve" CommandArgument='<%# Eval("Email") %>' />
                            <asp:Button ID="btnDecline" runat="server" Text="Decline" CssClass="btn btn-danger" CommandName="Decline" CommandArgument='<%# Eval("Email") %>'
                                OnClientClick="return setDeclineReason(this);" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
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
</asp:Content>
