<%@ Page Language="C#" MasterPageFile="~/Site2Admin.Master" AutoEventWireup="true" CodeBehind="ElectionControl.aspx.cs" Inherits="EVotingSystem.ElectionControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Election Control</h2>
    <div class="card">
        <h3>Create a new election</h3>
        <div class="form-grid">
            <label for="ContentPlaceHolder1_TextBoxTitle">Election Title</label>
            <asp:TextBox ID="TextBoxTitle" runat="server" MaxLength="150"></asp:TextBox>

            <label for="ContentPlaceHolder1_TextBoxStart">Start Date</label>
            <asp:TextBox ID="TextBoxStart" runat="server" TextMode="Date"></asp:TextBox>

            <label for="ContentPlaceHolder1_TextBoxEnd">End Date</label>
            <asp:TextBox ID="TextBoxEnd" runat="server" TextMode="Date"></asp:TextBox>
        </div>
        <div class="form-actions">
            <asp:Button ID="Button1" runat="server" Text="Create Election" OnClick="Button1_Click" CssClass="btn" />
        </div>
    </div>
    <div class="card">
        <h3>Existing Elections</h3>
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
                DataKeyNames="ElectionID" OnRowCommand="GridView1_RowCommand" CssClass="GridViewStyle">
                <Columns>
                    <asp:BoundField DataField="ElectionID" Visible="false" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="status-badge"><%# (bool)Eval("IsActive") ? "Active" : "Inactive" %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNewEndDate" runat="server" TextMode="DateTime" style="max-width: 180px; display: inline-block;"></asp:TextBox>
                            <asp:Button ID="btnActivate" runat="server" Text="Set Active" CssClass="btn btn-success" CommandName="Activate" CommandArgument='<%# Eval("ElectionID") %>' />
                            <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CssClass="btn btn-danger" CommandName="Deactivate" CommandArgument='<%# Eval("ElectionID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
