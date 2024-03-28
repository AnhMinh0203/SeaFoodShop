// const container = document.getElementById('container');
// const registerBtn = document.getElementById('register');
// const loginBtn = document.getElementById('login');

// registerBtn.addEventListener('click', () => {
//     container.classList.add("active");
// });

// loginBtn.addEventListener('click', () => {
//     container.classList.remove("active");
// });
// document.getElementById('password').addEventListener('input', function() {
//     var passwordInput = this.value;
//     var maskedPassword = '*'.repeat(passwordInput.length);
//     this.value = maskedPassword;
// });
const container = document.querySelector('.container');
const registerBtn = document.getElementById('register');
const loginBtn = document.querySelector('.toggle-panel.toggle-left button');
const signUpBtn = document.querySelector('.sign-up button');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});


