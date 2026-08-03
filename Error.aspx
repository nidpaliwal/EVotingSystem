<%@ Page Title="Error" Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="EVotingSystem.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Something went wrong</title>
    <link href="Styles/site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <main class="container">
        <div class="card login-card">
            <h3>Sorry, something went wrong</h3>
            <p class="form-note">An unexpected error occurred while processing your request. Please try again.</p>
            <div class="form-actions form-actions-center">
                <a class="btn" href="Login.aspx">Back to Login</a>
                <a class="btn btn-secondary" href="VoterInfoPublic.aspx">Home</a>
            </div>
        </div>
    </main>
</body>
</html>