// Khai báo
const token = localStorage.getItem('token');
localStorage.setItem("currentPage",1)
localStorage.setItem("lastPage",'False')
localStorage.setItem("user_status",'2')
localStorage.setItem("user_gender",'2')
localStorage.setItem("searchStatus", "False");

const status_admin = '1';
let pageSizeDefault = 5

// ------- User
// Load user data 
async function fetchUserChangePassword(token, phoneNumber, newPassword) {
    const requestBody = {
        phoneNumber: phoneNumber.toString(),
        newPassword: newPassword
    };
    return await axios.put(`https://localhost:7018/api/ManagerAccount/ChangePasswordAdmin?token=${token}`,requestBody);
}

async function fetchUserProfile(token,phoneNumber){
    return await axios.get(`https://localhost:7018/api/User/GetProfile?token=${token}&phoneNumber=${phoneNumber}`)
}

async function fetchUserRestore(phoneNumber){
    return await axios.put(`https://localhost:7018/api/ManagerAccount/UnLockAccount?phoneNumber=${phoneNumber}`)
}

async function fetchUserLock(phoneNumber){
    return await axios.put(`https://localhost:7018/api/ManagerAccount/LockAccount?phoneNumber=${phoneNumber}`)
}

async function fetchUserUnlock(phoneNumber){
    return await axios.put(`https://localhost:7018/api/ManagerAccount/UnLockAccount?phoneNumber=${phoneNumber}`)
}

async function fetchUserDeleteData(token,phoneNumber){
    return await axios.delete(`https://localhost:7018/api/ManagerCustomer/DeleteCustomer?token=${token}&phoneNumber=${phoneNumber}`)
}

async function fetchUserSearchData(token,textInput, pageIndex, pageSize){
    return await axios.get(`https://localhost:7018/api/ManagerCustomer/SearchCustomers?token=${token}&textInput=${textInput}&pageIndex=${pageIndex}&pageSize=${pageSize}`)
}

async function fetchUserData(token, status,gender, pageIndex, pageSize) {
    return await axios.get(`https://localhost:7018/api/ManagerCustomer/GetAllCustomers?token=${token}&status=${status}&gender=${gender}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
}

function renderUserPage(data) {
    const totalPage =  Math.ceil( parseInt(data.totalRecord) / pageSizeDefault);
    const tableBody = document.getElementById('userTableBody');
    const paginationLoad = document.getElementById('pagination_user--nav');
    const totalCustomerText = document.getElementById('total_customers');
    var currentPage = parseInt(localStorage.getItem("currentPage")) || 1;

    tableBody.innerHTML = '';  
    paginationLoad.innerHTML = '';

    if (!data || data.listCustomerInfor.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="7" class="text-center text-danger">Không có dữ liệu</td></tr>';
        totalCustomerText.innerHTML = '0';
        return;
    }

    data.listCustomerInfor.forEach((user) => {
        const row = `
            <tr>
                <th scope="row">${user.rowNum}</th>
                <td>${user.fullName}</td>
                <td>${user.phoneNumber}</td>
                <td>${new Date(user.dob).toLocaleDateString()}</td>
                <td>${user.gender === 0 ? 'Nam' : 'Nữ'}</td>
                <td>
                    ${
                        user.status === 0 ? 
                        `<i class="fa-solid fa-lock-open text-success" onclick="lockUser('${user.phoneNumber}')"></i>` :
                        (user.status === -1 ? `<i class="fa-solid fa-lock text-danger" onclick="unLockUser('${user.phoneNumber}')"></i>` :
                        (user.status === -2 ? `<i class="fa-solid fa-square-minus" id="restoreUserIcon" onclick="restoreUser('${user.phoneNumber}')"></i>` : '')
                        )
                    }
                </td>
                <td class="popup-trigger" onclick="togglePopup(event)">
                    <span class="material-icons-sharp">more_horiz</span>
                    <div class="popup-content d-flex d-none flex-column">
                        <a href="#" class="popup-option" onclick="detailUser(event,${user.phoneNumber})">Detail</a>
                        ${user.status === -2 ? '': `<a href="#" class="popup-option" onclick="deleteUser(event,${user.phoneNumber})">Delete</a>`}
                    </div>
                </td>
            </tr>`;
        tableBody.innerHTML += row;
    });
    totalCustomerText.innerHTML = data.totalRecord ;
    renderPagination(currentPage, totalPage, paginationLoad);
}

// Pagination user
function renderPagination(currentPage, totalPage, paginationLoad){
    var lastPage = localStorage.getItem("lastPage") === "True";
    var updateColorPage = localStorage.getItem("updateColor");
    var residual = lastPage ? (totalPage % 4 > 0 ? totalPage % 4 - 1 : totalPage % 4 + 3) : 0;
    var htmlContent = '';
    let onClickFunction = localStorage.getItem("searchStatus") == "True" ? "searchUser" : "handleUsersPage";

    htmlContent += `<li class="page-item">
                        <a class="page-link" href="#" onclick="firstPage(${1})">
                            <i class="fa-solid fa-angles-left"></i>
                        </a>
                    </li>
                    <li class="page-item">
                        <a class="page-link" href="#" onclick="periousPage(${lastPage ? currentPage - 4 - residual : currentPage - 4})">
                            <i class="fa fa-angle-left"></i>
                        </a>
                    </li>`;

    for (let page = lastPage ? totalPage - residual : currentPage; page <= totalPage && page < currentPage + 4; page++) {
       
        htmlContent += `<li class="page-item ${page == updateColorPage ? 'active' : ''}">
                            <a class="page-link" onclick="${onClickFunction}(${page},${pageSizeDefault})" href="#" value="${page}">${page}</a>
                        </li>`;
    }

    htmlContent += `<li class="page-item">
                        <a class="page-link" href="#" onclick="nextPage(${currentPage + 4},${totalPage})">
                            <i class="fa fa-angle-right"></i>
                        </a>
                    </li>
                    <li class="page-item">
                        <a class="page-link" href="#" onclick="lastPage(${totalPage})">
                            <i class="fa-solid fa-angles-right"></i>
                        </a>
                    </li>`;

    paginationLoad.innerHTML += htmlContent;
    localStorage.setItem("lastPage", "False");
    localStorage.setItem("currentPage", lastPage ? currentPage - residual : currentPage);
}

function handleUsersPage(pageIndex, pageSize) {
    localStorage.setItem("updateColor", pageIndex);
    localStorage.setItem("searchStatus", "False");

    const status_user = localStorage.getItem("user_status");
    const status_gender = localStorage.getItem("user_gender");
    fetchUserData(token, status_user,status_gender, pageIndex, pageSize)
        .then(response => {
            renderUserPage(response.data);
        })
        .catch(error => console.error("Error:", error));
}

function nextPage(nextPage,totalPage) {
    if(nextPage > totalPage){
        return;
    }
    else{
        localStorage.setItem("currentPage",nextPage)
        if(localStorage.getItem("searchStatus") == "False"){
            handleUsersPage(nextPage,pageSizeDefault)
        }
        else{
            searchUser(nextPage,pageSizeDefault)
        }
    }
}

function periousPage(periousPage){
    if(periousPage < 1){
        return;
    }
    else{
        localStorage.setItem("currentPage",periousPage)
        if(localStorage.getItem("searchStatus") == "False"){
            handleUsersPage(periousPage,pageSizeDefault)
        }
        else{
            searchUser(periousPage,pageSizeDefault)
        }
    }
}

function lastPage(page){
    localStorage.setItem("lastPage","True")
    localStorage.setItem("currentPage",page)

    if(localStorage.getItem("searchStatus") == "False"){
        handleUsersPage(page,pageSizeDefault) 
    }
    else{
        searchUser(page,pageSizeDefault)
    }
}

function firstPage(page){
    localStorage.setItem("currentPage",page)
    if(localStorage.getItem("searchStatus") == "False"){
        handleUsersPage(page,pageSizeDefault)
    }
    else{
        searchUser(page,pageSizeDefault)
    }
}

// Filter user
function getUserByStatus(status){
    if (status === "lock") {
        localStorage.setItem("user_status",'-1')
    }
    else if(status === "unlock"){
        localStorage.setItem("user_status",'0')
    }
    else if(status === "delete"){
        localStorage.setItem("user_status",'-2')
    }
    else {
        localStorage.setItem("user_status",'2')
    }
    handleUsersPage(1,pageSizeDefault);
}

function getUserByGender(gender){
    if (gender === "male") {
        localStorage.setItem("user_gender",'0')
    }
    else if (gender === "female") {
        localStorage.setItem("user_gender",'1')
    }
    else{
        localStorage.setItem("user_gender",'2')
    }
    handleUsersPage(1,pageSizeDefault);
}


// Search user
function searchUser(pageIndex, pageSize) {
    localStorage.setItem("updateColor", pageIndex);
    localStorage.setItem("searchStatus", "True");
    var inputText = document.getElementById("form1").value;
    if (inputText == ""){
        handleUsersPage(pageIndex,pageSize)
    }
    else{
        fetchUserSearchData(token, inputText, pageIndex, pageSize)
        .then(response => {
            renderUserPage(response.data);
        })
        .catch(error => console.error("Error:", error));
    }
}

// Xử lý hiển thị popup
document.addEventListener('click', function(event) {
    const popups = document.querySelectorAll('.popup-content');
    const clickedPopup = event.target.nextElementSibling;
    if (!clickedPopup || !clickedPopup.classList.contains('popup-content')) {
        popups.forEach(popup => {
            popup.classList.add('d-none');
        });
    }
});

function hideAllPopups(event) {
    const popups = document.querySelectorAll('.popup-content');
    const clickedPopup = event.target.nextElementSibling;
    popups.forEach(popup => {
        if (popup !== clickedPopup) {
            popup.classList.add('d-none');
        }
    });
}

function togglePopup(event) {
    const popupContent = event.target.nextElementSibling;
    popupContent.classList.toggle('d-none');
    event.stopPropagation(); 
    hideAllPopups(event);
}

// Xử lý action trong popup
function detailUser(event,phoneNumber){
    openProfile(event,phoneNumber);
    event.stopPropagation();
}

async function deleteUser(event,phoneNumber){
    await fetchUserDeleteData(token,phoneNumber);
    alert("Delete Successfully !")
    handleUsersPage(1,pageSizeDefault);
    event.stopPropagation();
}

async function lockUser(phoneNumber){
    const ok = confirm("Do you want to lock this account?");
    if (ok) {
        try {
            await fetchUserLock(phoneNumber);
            alert("Lock Successfully !");
            handleUsersPage(1, pageSizeDefault);
        } catch (error) {
            console.error("Error unlocking user:", error);
            alert("Failed to lock the user.");
        }
    }
}

async function unLockUser(phoneNumber){
    const ok = confirm("Do you want to unlock this account?");
    if (ok) {
        try {
            await fetchUserUnlock(phoneNumber);
            alert("Unlock Successfully !");
            handleUsersPage(1, pageSizeDefault);
        } catch (error) {
            console.error("Error unlocking user:", error);
            alert("Failed to unlock the user.");
        }
    }
}

async function restoreUser(phoneNumber){
    const ok = confirm("Do you want to restore this account?");
    if (ok) {
        try {
            await fetchUserRestore(phoneNumber);
            alert("Restore Successfully !");
            handleUsersPage(1, pageSizeDefault);
        } catch (error) {
            console.error("Error restore user:", error);
            alert("Failed to restore the user.");
        }
    }
}

// Detail user and change password
function closeProfile(){
    document.querySelector(".user_profile").style.display = "none";
    document.querySelector(".overlay").style.display = "none";
}

function openProfile(event,phoneNumber){
    event.stopPropagation();
    fetchUserProfile(token,phoneNumber)
    .then(response => {
        console.log(response.data)
        renderUserProfile(response.data);       
    })
    .catch(error => console.error("Error:", error));
    document.querySelector(".user_profile").style.display = "block";
}

function renderUserProfile(data){
    const tableBody = document.getElementById('user_profile');
    tableBody.innerHTML = '';

    var result = `
    <div class="user_profile--detail position-absolute top-50 start-50 translate-middle">
        <div class="row">
            <div class="col-lg-12 mb-4 mb-sm-5">
                <div class="card card-style1 border-0">
                    <div class="card-body p-1-9 p-sm-2-3 p-md-6 p-lg-7">
                        <div class="row align-items-center">
                            <div class="close_profile--icon d-flex justify-content-end">
                                <i class="fa-solid fa-square-xmark" id="closeIcon" onclick="closeProfile()"></i>
                            </div>
                            <div class="col-lg-2  mb-4 mb-lg-0">
                                <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="...">
                            </div>
                            
                            <div class="col-lg-10 px-xl-10">
                                
                                <div class=" py-1-9 px-1-9 px-sm-6 mb-1-9 rounded">
                                    <h3 class="h2 mb-0">${data.fullName}</h3>                          
                                </div>
                                <ul class="list-unstyled mb-1-9">
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Phone number:</span> ${data.phoneNumber}</li>
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Date of birth:</span>${new Date(data.dob).toLocaleDateString()}</li>
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Gender:</span>${data.gender === 0 ? 'Nam' : 'Nữ'}</li>
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Status:</span>${data.status === 0 ? 'Unlock' :(data.status === -1 ? 'Lock' :(data.status === -2 ? 'Delete' : ''))}</li>
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Registration Date:</span> www.example.com</li>
                                    ${data.status === -2 ? '': `<li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">New password:</span> 
                                    <input type="password" class="form-control" id="newPasswordField">
                                </li>
                                <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Repeat password:</span> 
                                    <input type="password" class="form-control" id="repeatPasswordField">
                                </li>`}              
                                </ul>
                                <div class="button_profile d-flex justify-content-end mx-2">
                                    <button type="button" class="btn btn-light mx-2" onclick="closeProfile()">Cancel</button>
                                    <button type="button" class="btn btn-success" onclick="${data.status === -2 ? `restoreUser(${data.phoneNumber})` : `savePassword(${data.phoneNumber})`}">${data.status === -2 ? 'Restore': 'Save'}</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="overlay"></div>`
    tableBody.innerHTML += result;
}

function savePassword(phoneNumber){
    var newPassword = document.getElementById("newPasswordField").value;
    var repeatPassword = document.getElementById("repeatPasswordField").value;

    if (newPassword !== repeatPassword) {     
        alert("New password and repeat password do not match");
    }
    else{
        fetchUserChangePassword(token,phoneNumber,newPassword)
        .catch(error => console.error("Error:", error));
        alert("Update password successfully!")
    } 
}


// ------ Dashboard
// Load dashboard data
function fetchAdminData(token) {
    return axios.get(`https://localhost:7018/api/ManagerAccount/GetAdminInfor?token=${token}`)
}

function populateAdmin(data) {
    const tableBody = document.querySelector('.right-section .nav .profile');
    tableBody.innerHTML = '';  // Clear table body

    tableBody.innerHTML +=  `
        <div class="profile">
            <div class="info">
                <b>${data.fullName}</b>
            </div>
            <div class="profile-photo">
                <img src="../assets/images/${data.avatar}">
            </div>
        </div>
    `;
}

function handleAdminPage(){
    fetchAdminData(token)
        .then(response => {
            populateAdmin(response.data);
        })
        .catch(error => console.error("Error:", error));
}

