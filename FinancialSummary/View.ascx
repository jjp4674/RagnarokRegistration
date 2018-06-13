<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.FinancialSummary.View" %>

<div class="status">
    <asp:Label ID="lblStatus" runat="server" CssClass="error" Visible="false" />
</div>

<div class="row pad-b-10 pad-t-10">
    <h1>Ragnarok XXXII Financial Summary Report</h1>
    <table class="financialTable">
        <tr>
            <td colspan="3"><h2>Previous Year's Balance</h2></td>
        </tr>
        <tr>
            <td class="description"><b>2016 End Balance</b></td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblEndBalance" runat="server" /></b></td>
        </tr>
        <tr>
            <td colspan="3"><h1>Income</h1></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Registrations</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Adult Pre-Registrations</b></td>
            <td class="number"><asp:Label ID="lblAdultPreRegNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblAdultPreRegTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Child Pre-Registrations</b></td>
            <td class="number"><asp:Label ID="lblChildPreRegNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblChildPreRegTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Merchant Pre-Registrations</b></td>
            <td class="number"><asp:Label ID="lblMerchantPreRegNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblMerchantPreRegTotal" runat="server" /></td>
        </tr>
        <tr class="altRow">
            <td class="description"><b>Adult On-Site Registrations</b></td>
            <td class="number"><asp:Label ID="lblAdultRegNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblAdultRegTotal" runat="server" /></td>
        </tr>
        <tr>
            <td class="description"><b>Child On-Site Registrations</b></td>
            <td class="number"><asp:Label ID="lblChildRegNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblChildRegTotal" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2"><b>Subtotal:</b></td>
            <td class="amount"><b><asp:Label ID="lblRegistrationsSubtotal" runat="server" /></b></td>
        </tr>
        <asp:Repeater ID="repIncome" runat="server" OnItemDataBound="repIncome_ItemDataBound">
            <ItemTemplate>
        <tr>
            <td colspan="3"><h2><asp:Literal ID="litCategory" runat="server" /></h2></td>
        </tr>
                <asp:Repeater ID="repIncomeItem" runat="server" OnItemDataBound="repIncomeItem_ItemDataBound">
                    <ItemTemplate>
        <tr>
            <td class="description"><asp:Literal ID="litText" runat="server" /></td>
            <td class="number"><asp:Label ID="lblNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblTotal" runat="server" /></td>
        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
        <tr class="altRow">
            <td class="description"><asp:Literal ID="litText" runat="server" /></td>
            <td class="number"><asp:Label ID="lblNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblTotal" runat="server" /></td>
        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
        <tr>
            <td colspan="2"><b>Subtotal:</b></td>
            <td class="amount"><b><asp:Label ID="lblSubtotal" runat="server" /></b></td>
        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="3"><h1>Expenses</h1></td>
        </tr>
        <asp:Repeater ID="repExpense" runat="server" OnItemDataBound="repExpense_ItemDataBound">
            <ItemTemplate>
        <tr>
            <td colspan="3"><h2><asp:Literal ID="litCategory" runat="server" /></h2></td>
        </tr>
                <asp:Repeater ID="repExpenseItem" runat="server" OnItemDataBound="repExpenseItem_ItemDataBound">
                    <ItemTemplate>
        <tr>
            <td class="description"><asp:Literal ID="litText" runat="server" /></td>
            <td class="number"><asp:Label ID="lblNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblTotal" runat="server" /></td>
        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
        <tr class="altRow">
            <td class="description"><asp:Literal ID="litText" runat="server" /></td>
            <td class="number"><asp:Label ID="lblNo" runat="server" /></td>
            <td class="amount"><asp:Label ID="lblTotal" runat="server" /></td>
        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
        <tr>
            <td colspan="2"><b>Subtotal:</b></td>
            <td class="amount"><b><asp:Label ID="lblSubtotal" runat="server" /></b></td>
        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="3"><h2>Authorize.NET Fees</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Authorize.NET Fees</b><br />
                <span class="small">2.9% + $0.30 per transaction</span>
            </td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblAuthNetFees" runat="server" /></b></td>
        </tr>
        <tr>
            <td colspan="3"><h1>Summary</h1></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Current Total Balance</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Current Total Balance</b><br />
                <span class="small">Not Including Payment to Cooper's Lake</span>
            </td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblCurrentTotalBalance" runat="server" /></b></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Cooper's Lake Payment</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Total Cooper's Lake Payment</b><br />
                <span class="small">Actual Amount Based on Attendee Check-In Dates</span>
            </td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblActualCooperPayment" runat="server" /></b></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Current Available Balance</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Current Available Balance</b><br />
                <span class="small">Cash Available After Accounting for Cooper's Lake Payment</span>
            </td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblCurrentAvailableBalance" runat="server" /></b></td>
        </tr>
        <tr>
            <td colspan="3"><h2>Pre-Event Estimates</h2></td>
        </tr>
        <tr>
            <td class="description"><b>Expected Cooper's Lake Payment</b><br />
                <span class="small">Estimated Based on Pre-Reg Dates (AKA: if everyone shows up on the day they pre-registered)</span>
            </td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblExpectedCooperPayment" runat="server" /></b></td>
        </tr>
        <tr>
            <td class="description"><b>Current Available Estimated Balance</b><br />
                <span class="small">The total we have taken in minus the estimated Cooper's payment</span>
            </td>
            <td class="number"></td>
            <td class="amount"><b><asp:Label ID="lblCurrentEstimatedBalance" runat="server" /></b></td>
        </tr>
    </table>
</div>