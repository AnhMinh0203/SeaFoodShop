// Login
async function login() {
  const url = 'https://localhost:7018/api/Account/SignIn';

  const phoneNumber = $('#phoneNumberLogin').val();
  const password = $('#passwordLogin').val();

  const loginData = {
      phoneNumber: phoneNumber,
      password: password
  };

  try {
        const response = await $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(loginData)
        });
        // User
        if (response && response.token && response.status == 0) {
            alert("oce")
            localStorage.setItem('token',response.token);
            window.location.href = '../pages/home.html';
        }
        // Admin
        else if (response && response.token && response.status == 1) {
            localStorage.setItem('token', response.token);
            window.location.href = '../../admin_side/pages/index_admin.html';
        }
    } catch (error) {
        console.error('Unable to get items.', error);
    }
}

// Sign Up
async function SignUp() {
    const url = 'https://localhost:7018/api/Account/SignUp';
    const fullName = document.querySelector('input[type="FullName"]').value;
    const dob = document.querySelector('input[type="date"]').value;
    const gender = document.querySelector('input[name="gender"]:checked').value;
    const phoneNumber = document.querySelector('input[type="phonenumber"]').value;
    const password = document.querySelector('#passwordSignUp').value;
    const repeatPassword = document.querySelector('#passwordSignUpRepeat').value;

    
    // Kiểm tra mật khẩu nhập lại có khớp không
    if (password !== repeatPassword) {
        alert("Mật khẩu nhập lại không khớp");
        return;
    }

    const data = {
        FullName: fullName,
        Dob: dob,
        Gender: gender,
        PhoneNumber: phoneNumber,
        Password: password
    };

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();
        console.log(result); // In kết quả trả về từ API vào console
        // Xử lý kết quả ở đây, có thể là hiển thị thông báo thành công, lỗi, vv.
    } catch (error) {
        console.error('Error:', error);
        // Xử lý lỗi ở đây, có thể là hiển thị thông báo lỗi cho người dùng
    }
}
