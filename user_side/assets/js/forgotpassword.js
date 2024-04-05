function isValidPhoneNumber(phoneNumber) {
    var phonePattern = /^\d{11}$/;
    return phonePattern.test(phoneNumber);
}

function showSuccessMessage() {
    alert("Số điện thoại hợp lệ. Đã gửi mật khẩu về số điện thoại!");
}

document.getElementById("continueButton").addEventListener("click", function() {
    var phoneNumber = document.getElementById("phoneInput").value;
    if (isValidPhoneNumber(phoneNumber)) {
        showSuccessMessage();
    } else {
        alert("Số điện thoại không hợp lệ. Vui lòng nhập lại.");
    }
});
function loginsignup() {
    window.location.href = "log in-sign up.html";
}
