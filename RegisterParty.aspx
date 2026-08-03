<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RegisterParty.aspx.cs" Inherits="EVotingSystem.RegisterParty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 100%;
        }
    </style>

    <script>
function togglePassword() {

    var txt = document.getElementById("ContentPlaceHolder1_TextBox6");

    if (txt.type === "password")
        txt.type = "text";
    else
        txt.type = "password";
    }
    </script> 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p style="font-size: large; font-weight: 700">
        Register Party</p>
    <table  border="1">
        <tr>
            <td>Party Name :</td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Leader Name :</td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Leader Photo :</td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Party Symbol :</td>
            <td>
                <asp:FileUpload ID="FileUpload2" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Objective :</td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Legal History :</td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Email :</td>
            <td>
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Password :</td>
            <td>
                <asp:TextBox ID="TextBox6" runat="server" TextMode="Password"></asp:TextBox>
            &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" Height="17px" ImageUrl="~/Images/hide.png" OnClick="ImageButton1_Click" OnClientClick="togglePassword(); return false;" Width="18px" />
            </td>
        </tr>
        <tr>
            <td>Phone :</td>
            <td>
                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Register" OnClick="Button1_Click" />
</asp:Content>
