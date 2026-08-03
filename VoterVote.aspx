<%@ Page Language="C#" MasterPageFile="~/Site4Voter.Master" AutoEventWireup="true" CodeBehind="VoterVote.aspx.cs" Inherits="EVotingSystem.VoterVote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function confirmVote() {
            var radios = document.getElementsByName('PartyChoice');
            var selectedId = null;
            var selectedName = null;

            for (var i = 0; i < radios.length; i++) {
                if (radios[i].checked) {
                    selectedId = radios[i].value;
                    selectedName = radios[i].getAttribute('data-partyname');
                }
            }

            if (selectedId === null) {
                alert("Please select a party before voting.");
                return false;
            }

            var confirm1 = confirm("You selected " + selectedName + ". Continue?");
            if (!confirm1) return false;

            var confirm2 = confirm("This action is FINAL and cannot be undone. Submit your vote for " + selectedName + "?");
            if (!confirm2) return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Cast Your Vote</h2>
    <div class="card">
        <p><strong>Election:</strong> <asp:Label ID="lblElectionInfo" runat="server" Style="font-weight: 600;"></asp:Label></p>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert-error"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="ballot-list"
            ShowHeader="false" GridLines="None">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <label class="ballot-card">
                            <input type="radio" name="PartyChoice" value='<%# Eval("PartyID") %>' data-partyname='<%# Eval("PartyName") %>' />
                            <img class="party-symbol" src='<%# ResolveUrl(Eval("SymbolImagePath").ToString()) %>' alt="Party symbol" />
                            <span class="party-name"><%# Eval("PartyName") %></span>
                        </label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="form-actions">
            <asp:Button ID="ButtonVote" runat="server" Text="Submit My Vote" CssClass="btn btn-lg" OnClientClick="return confirmVote();" OnClick="ButtonVote_Click" />
        </div>
    </div>
</asp:Content>
