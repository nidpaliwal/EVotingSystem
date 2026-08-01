<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EVotingSystem.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 100%;
        }
        .auto-style5 {
            width: 229px;
        }
    </style>
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
    <p>
        <strong>
        <br class="auto-style1" />
        </strong><span class="auto-style1"><strong>Login</strong></span></p>
  <center><table class="auto-style4">
        <tr>
            <td class="auto-style5">Enter E-mail&nbsp; :</td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Height="20px" style="margin-left: 0px" Width="233px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">Enter Password : </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" Height="20px" Width="208px" ClientIDMode="Static"></asp:TextBox>
                &nbsp;
                <asp:ImageButton ID="ImageButton1" runat="server" Height="17px" ImageUrl="~/Images/hide.png" OnClientClick="togglePassword(); return false;" style="width: 17px " OnClick="ImageButton1_Click"  />
                </td>
        </tr>
    </table></center>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Login" OnClick="Button1_Click" />
</asp:Content>
