<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RegisterVoter.aspx.cs" Inherits="EVotingSystem.RegisterVoter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            
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
    <p style="font-size: large">
        Voter Registration<table border="1">

        <tr>
        <td><label for="name" >Full Name : </label></td>
        <td>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td><label for="dob">Date of Birth : </label></td>
        <td>
            <asp:TextBox ID="TextBox2" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td><label for="Gender">Select Gender : </label></td>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem>Male</asp:ListItem>
                <asp:ListItem>Female</asp:ListItem>
                <asp:ListItem>Others</asp:ListItem>
            </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><label>Address : </label></td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><label>Voter Aadhar ID Number :</label></td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><label>Email : </label></td>
            <td>
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><label>Password : </label></td>
            <td>
                <asp:TextBox ID="TextBox6" runat="server" TextMode="Password"></asp:TextBox>
            &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" Height="17px" ImageUrl="~/Images/hide.png" OnClick="ImageButton1_Click" OnClientClick="togglePassword(); return false;" Width="18px" />
            </td>
        </tr>
        <tr>
            <td><label>Phone : </label></td>
            <td>
                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><label>Photo</label></td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        

</table>
    </p>
    <p style="font-size: large">
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Register" />
    </p>
    <p style="font-size: large">
        &nbsp;</p>
    <br />
</asp:Content>
