<%@ Page Title="" Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterVote.aspx.cs" Inherits="EVotingSystem.VoterVote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        <script>
            function confirmVote() {
                var radios = document.getElementsByName('PartyChoice');
                var selected = null;
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].checked) {
                        selected = radios[i].value;
                    }
                }

                if (selected === null) {
                    alert("Please select a party before voting.");
                    return false;
                }
                var confirm1 = confirm("You selected Party ID " + selected + ". Continue?");
                if (!confirm1) return false;

                var confirm2 = confirm("This action is FINAL and cannot be undone. Submit your vote?");
                if (!confirm2) return false;

                return true;
            }
    </script>
        <p>Cast Your Vote</p>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <br />

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <input type="radio" name="PartyChoice" value='<%# Eval("PartyID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Symbol">
                <ItemTemplate>
                    <asp:Image ID="imgSymbol" runat="server" ImageUrl='<%# Eval("SymbolImagePath") %>' Width="50" Height="50" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PartyName" HeaderText="Party Name" />
        </Columns>
    </asp:GridView>
        <br />
    <asp:Button ID="ButtonVote" runat="server" Text="Vote" OnClientClick="return confirmVote();" OnClick="ButtonVote_Click" />
    </p>
</asp:Content>
