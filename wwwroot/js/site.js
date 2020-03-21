// Basic input validation for line items during transaction recording
function ValidateLineItem() {
    var accountName = document.getElementById("ItemAccountName");
    if (accountName.value == "") {
        window.alert("Error: A line item must contain an account name.");
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
}

// Default values of zero are passed in
function ClearLineItemAmounts() {
    document.getElementById("ItemAmountDebit").value = "";
    document.getElementById("ItemAmountCredit").value = "";
}