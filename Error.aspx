<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="EVotingSystem.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Something went wrong - E-Voting System</title>
    <link href="~/Styles/site.css" rel="stylesheet" type="text/css" />
    <style>
        body { display: block; }
        .logout-card { text-align: left; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logout-card">
            <h1>Sorry, something went wrong</h1>
            <p>An unexpected error occurred while processing your request.</p>
            <p><a class="btn" href="Login.aspx">Back to Login</a></p>
        </div>
    </form>
</body>
</html>
