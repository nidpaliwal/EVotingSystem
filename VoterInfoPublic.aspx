<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="VoterInfoPublic.aspx.cs" Inherits="EVotingSystem.VoterInfoPublic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Voter Information</h2>
    <div class="card">
        <p style="margin-top: 0;">Any eligible citizen (age 18 or above) can register as a voter on this platform. To register, you will need: your full name, date of birth, address, a valid Voter's Aadhaar ID number, and a photo for identity verification.</p>
        <p>After submitting your registration, your details are reviewed by an Election Commission (E.C.) Administrator. Your status will show as <span class="badge badge-warn">Pending</span> until reviewed, and will change to <span class="badge badge-ok">Approved</span> or <span class="badge badge-bad">Declined</span> (with a reason) once processed.</p>
        <p>Once approved, you can log in to vote in any active election, view the list of registered parties, and check results after the election.</p>
    </div>
    <div class="card">
        <h3>Platform statistics</h3>
        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 14px; max-width: 520px;">
            <div style="background: #f4f6fa; border: 1px solid var(--line); border-radius: 6px; padding: 16px;">
                <div style="font-size: 26px; font-weight: 700; color: var(--navy);"><asp:Label ID="lblTotalVoters" runat="server"></asp:Label></div>
                <div style="font-size: 13px; color: var(--muted);">Total Registered Voters</div>
            </div>
            <div style="background: #f4f6fa; border: 1px solid var(--line); border-radius: 6px; padding: 16px;">
                <div style="font-size: 26px; font-weight: 700; color: var(--navy);"><asp:Label ID="lblApprovedVoters" runat="server"></asp:Label></div>
                <div style="font-size: 13px; color: var(--muted);">Total Approved Voters</div>
            </div>
        </div>
    </div>
</asp:Content>