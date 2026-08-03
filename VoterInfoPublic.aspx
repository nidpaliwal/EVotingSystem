<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="VoterInfoPublic.aspx.cs" Inherits="EVotingSystem.VoterInfoPublic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
    Voter Information

    </p>
    <p>
        Any eligible citizen [ age above 18 ] can register as a voter on this platform. To register, you will need: your full name, date of birth, address, a valid Voter ID number, and a photo for identity verification.
    </p>
    <p>
        After submitting your registration, your details are reviewed by an Election Commision (E.C.) Administrator. 
        Your status will show as <strong>Pending</strong> until reviewed, and will change to 
        <strong>Approved</strong> or <strong>Declined</strong> (with a reason) once processed.
    </p>
    <p>
        Once approved, you can log in to vote in any active election, view the list of registered parties, and check results after the election.
    </p>

    <br />
    <table border="1">
        <tr>
            <td>Total Registered Voters :</td>
            <td><asp:Label ID="lblTotalVoters" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Total Approved Voters :</td>
            <td><asp:Label ID="lblApprovedVoters" runat="server"></asp:Label></td>
        </tr>
    </table>
    <br />
</asp:Content>
