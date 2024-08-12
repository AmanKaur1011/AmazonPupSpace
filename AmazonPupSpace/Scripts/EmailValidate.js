
document.addEventListener("DOMContentLoaded", function () {
    const emailInput = document.getElementById("Email");
    // Set the maximum date for the input field
    document.getElementById("HireDate").setAttribute("max", today);

    // Add event listener for input events
    emailInput.addEventListener("input", function (event) {
        // Prevent the @ symbol from being typed
        event.target.value = event.target.value.replace(/@/g, '');
    });

    // Optional: To handle pasting text
    emailInput.addEventListener("paste", function (event) {
        event.preventDefault();
        const pastedData = event.clipboardData.getData('text');
        emailInput.value = pastedData.replace(/@/g, '');
    });
});