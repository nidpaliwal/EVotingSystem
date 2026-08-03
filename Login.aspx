<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EVotingSystem.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function togglePassword() {
            var txt = document.getElementById("TextBox2");
            if (txt.type === "password")
                txt.type = "text";
            else
                txt.type = "password";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card login-card">
        <h3>Login to your account</h3>
        <p class="form-note">Use the credentials you registered with. Voters, parties and admins all sign in here.</p>
        <div class="form-grid">
            <label for="ContentPlaceHolder1_TextBox1">Email</label>
            <asp:TextBox ID="TextBox1" runat="server" TextMode="Email" MaxLength="100"></asp:TextBox>

            <label for="TextBox2">Password</label>
            <div class="login-password">
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" ClientIDMode="Static" MaxLength="128"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" CssClass="password-toggle" ImageUrl="~/Images/hide.png" OnClientClick="togglePassword(); return false;" OnClick="ImageButton1_Click" ToolTip="Show / hide password" />
            </div>
        </div>
        <asp:Label ID="lblLocked" runat="server" CssClass="lockout-banner" Visible="false"
            Text="Too many failed login attempts. Please try again after 15 minutes."></asp:Label>
        <div class="form-actions">
            <asp:Button ID="Button1" runat="server" Text="Login" OnClick="Button1_Click" CssClass="btn btn-lg" />
        </div>
        <p class="form-note">New here? <a href="RegisterChoice.aspx">Register as a voter or a party</a>.</p>
    </div>
</asp:Content>
