<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="EVotingSystem.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Logged Out - E-Voting System</title>
    <link href="~/Styles/site.css" rel="stylesheet" type="text/css" />
    <style>
        body { display: block; }
        .logout-card {
            max-width: 460px;
            margin: 80px auto;
            text-align: center;
            background: var(--card);
            border: 1px solid var(--line);
            border-radius: var(--radius);
            box-shadow: 0 1px 3px rgba(20,30,60,.06);
            padding: 32px;
        }
        .logout-card h1 { font-size: 22px; color: var(--navy); margin: 0 0 10px; }
        .logout-card p { color: var(--muted); margin: 0 0 20px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logout-card">
            <h1>You have successfully logged out</h1>
            <p>Your session has ended securely. You may log in again whenever you are ready.</p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Back to Login" CssClass="btn btn-lg" />
        </div>
    </form>
</body>
</html>
