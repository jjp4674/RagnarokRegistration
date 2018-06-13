<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.CoopersPayment.View" %>

<div class="status">
    <asp:Label ID="lblStatus" runat="server" CssClass="error" Visible="false" />
</div>

<div class="row pad-b-10 pad-t-10">
    <h1>Ragnarok XXXIII Coopers Payment Report</h1>
    <table class="financialTable">
        <tr>
            <td colspan="3"><h1>Arrivals</h1></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Friday (06/15/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblFriday1AdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblFriday1AdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblFriday1ChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblFriday1ChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblFriday1SubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblFriday1SubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Saturday (06/16/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblSaturday1AdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblSaturday1AdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblSaturday1ChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblSaturday1ChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblSaturday1SubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblSaturday1SubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Sunday (06/17/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblSundayAdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblSundayAdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblSundayChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblSundayChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblSundaySubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblSundaySubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Monday (06/18/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblMondayAdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblMondayAdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblMondayChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblMondayChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblMondaySubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblMondaySubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Tuesday (06/19/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblTuesdayAdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblTuesdayAdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblTuesdayChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblTuesdayChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblTuesdaySubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblTuesdaySubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Wednesday (06/20/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblWednesdayAdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblWednesdayAdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblWednesdayChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblWednesdayChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblWednesdaySubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblWednesdaySubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Thursday (06/21/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblThursdayAdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblThursdayAdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblThursdayChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblThursdayChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblThursdaySubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblThursdaySubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Friday (06/22/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblFriday2AdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblFriday2AdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblFriday2ChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblFriday2ChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblFriday2SubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblFriday2SubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Saturday (06/23/2018)</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult</b></td>
            <td class="number"><asp:Label ID="lblSaturday2AdultNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblSaturday2AdultTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child</b></td>
            <td class="number"><asp:Label ID="lblSaturday2ChildNo" runat="server" Text="0" /></td>
            <td class="amount"><asp:Label ID="lblSaturday2ChildTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Subtotal</b></td>
            <td class="number"><asp:Label ID="lblSaturday2SubtotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblSaturday2SubtotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <td colspan="3"><h1>Total Owed to Cooper's Lake</h1></td>
        </tr>
        <tr>
            <td class="description"><b>Total</b></td>
            <td class="number"><asp:Label ID="lblTotalNo" runat="server" Text="0" Font-Bold="true" /></td>
            <td class="amount"><asp:Label ID="lblTotalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
    </table>
</div>