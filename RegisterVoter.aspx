<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RegisterVoter.aspx.cs" Inherits="EVotingSystem.RegisterVoter" %>
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
    <h2 class="page-title">Voter Registration</h2>
    <div class="card">
        <div class="form-grid">
            <label for="ContentPlaceHolder1_TextBox1">Full Name</label>
            <asp:TextBox ID="TextBox1" runat="server" MaxLength="100"></asp:TextBox>

            <label for="ContentPlaceHolder1_TextBox2">Date of Birth</label>
            <asp:TextBox ID="TextBox2" runat="server" TextMode="Date"></asp:TextBox>

            <label for="ContentPlaceHolder1_DropDownList1">Gender</label>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem>Male</asp:ListItem>
                <asp:ListItem>Female</asp:ListItem>
                <asp:ListItem>Others</asp:ListItem>
            </asp:DropDownList>

            <label for="ContentPlaceHolder1_TextBox3">Address</label>
            <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine" MaxLength="300"></asp:TextBox>

            <label for="ContentPlaceHolder1_TextBox4">Aadhaar ID Number</label>
            <asp:TextBox ID="TextBox4" runat="server" MaxLength="12" TextMode="Number" placeholder="12-digit Aadhaar number"></asp:TextBox>

            <label for="ContentPlaceHolder1_TextBox5">Email</label>
            <asp:TextBox ID="TextBox5" runat="server" TextMode="Email" MaxLength="100"></asp:TextBox>

            <label for="ContentPlaceHolder1_TextBox6">Password</label>
            <div style="display: flex; gap: 8px; align-items: center; max-width: 460px; width: 100%;">
                <asp:TextBox ID="TextBox6" runat="server" TextMode="Password" MaxLength="128" style="flex: 1;"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" Height="20px" ImageUrl="~/Images/hide.png" OnClick="ImageButton1_Click" OnClientClick="togglePassword(); return false;" style="width: 20px;" ToolTip="Show / hide password" />
            </div>

            <label for="ContentPlaceHolder1_TextBox7">Phone</label>
            <asp:TextBox ID="TextBox7" runat="server" MaxLength="15"></asp:TextBox>

            <label for="ContentPlaceHolder1_FileUpload1">Photo</label>
            <asp:FileUpload ID="FileUpload1" runat="server" />
        </div>
        <p class="form-note">Photo must be a .jpg, .jpeg, .png or .gif image up to 5 MB.</p>
        <div class="form-actions">
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Register" CssClass="btn" />
        </div>
    </div>
</asp:Content>
