<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="EVotingSystem.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <p style="font-weight: 700; text-align: center; font-size: x-large">
        <br />
        You Have Sucessfully Logged Out !!!</p>
    <form id="form1" runat="server">
        <p style="font-weight: 700; text-align: center; font-size: medium">
            To Login again click Login Button&nbsp;&nbsp;&nbsp;&nbsp;
        </p>
        <p style="font-weight: 700; text-align: center; font-size: medium">
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="font-weight: 700" Text="Login" />
        </p>
        <p style="font-weight: 700; text-align: center; font-size: x-large">
            &nbsp;</p>
        <div>
        </div>
    </form>
</body>
</html>
