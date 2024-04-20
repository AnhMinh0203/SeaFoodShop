// Khai báo
const token = localStorage.getItem('token');
const status_user = '0';
const status_admin = '1';

// Load user data 
function fetchUserData(token, status) {
    return axios.get(`https://localhost:7018/api/ManagerCustomer/GetAllCustomers?token=${token}&status=${status}`);
}
function populateUserTable(data) {
    const tableBody = document.getElementById('userTableBody');
    tableBody.innerHTML = '';  // Clear table body

    data.forEach((user, index) => {
        const row = `
            <tr>
                <th scope="row">${index + 1}</th>
                <td>${user.fullName}</td>
                <td>${user.phoneNumber}</td>
                <td>${new Date(user.dob).toLocaleDateString()}</td>
                <td>${user.gender === 0 ? 'Nam' : 'Nữ'}</td>
                <td>
                    <span class="material-icons-sharp">
                        more_horiz
                    </span>
                </td>
            </tr>
        `;
        tableBody.innerHTML += row;
    });
}

function handleUsersPage() {
    fetchUserData(token, status_user)
        .then(response => {
            populateUserTable(response.data);
        })
        .catch(error => console.error("Error:", error));
}


// Load dashboard data
function fetchAdminData(token, status,phoneNumber) {
    return axios.get(`https://localhost:7018/api/ManagerCustomer/SearchCustomers?token=${token}&phoneNumber=${phoneNumber}&status=${status}`);
}
function populateAdmin(data) {
    const tableBody = document.querySelector('.right-section .nav .profile');
    tableBody.innerHTML = '';  // Clear table body

    data.forEach((user) => {
        const row = `
            <div class="profile">
                <div class="info">
                    <b>${user.fullName}</b>
                </div>
                <div class="profile-photo">
                    <img src="../assets/images/${user.avatar}">
                </div>
            </div>
        `;
        tableBody.innerHTML += row;
    });
}

function handleAdminPage(){
    const phoneNumber = '0869819316';
    fetchAdminData(token, status_admin,phoneNumber)
        .then(response => {
            populateAdmin(response.data);
        })
        .catch(error => console.error("Error:", error));
}

