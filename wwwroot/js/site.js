// Basic input validation for line items during transaction recording
function ValidateLineItem() {
    var accountName = document.getElementById("ItemAccountName");
    if (accountName.value == "") {
        window.alert("Error: A line item must contain an account name.");
        accountName.
        return;
    }
    var debit = document.getElementById("ItemAmountDebit");
    var credit = document.getElementById("ItemAmountCredit");
    if (debit.value != "" && credit.value != "") {
        window.alert("Error: A line item can only contain one amount.");
        return;
    }
    if (debit.value == "" && credit.value == "") {
        window.alert("Error: A line item must contain a debit or credit amount.");
        return;
    }
    if (debit.value < 0 || credit.value < 0) {
        window.alert("Error: Amounts cannot be negative.");
        return;
    }
}

// Default values of zero are passed in and should be cleared for ValidateLineItem() to properly function
// Also necessary to clear input after a line item is entered for next input
function ClearLineItemAmounts() {
    document.getElementById("ItemAccountName").value = "";
    document.getElementById("ItemAmountDebit").value = "";
    document.getElementById("ItemAmountCredit").value = "";
}