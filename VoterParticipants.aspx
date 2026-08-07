<%@ Page Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterParticipants.aspx.cs" Inherits="EVotingSystem.VoterParticipants" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Participants</h2>
    <div class="card">
        <p style="margin-top: 0;">List of approved voters who are eligible to participate in the ongoing elections.</p>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert-error"></asp:Label>
        <div class="table-wrap">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridViewStyle">
                <Columns>
                    <asp:TemplateField HeaderText="Photo">
                        <ItemTemplate>
                            <asp:Image ID="imgPhoto" runat="server" ImageUrl='<%# Eval("PhotoPath") %>' CssClass="symbol-rounded photo-rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="VoterIDNumber" HeaderText="Voter ID Number" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="DOB" HeaderText="Date of Birth" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>