
// Khai báo
const token = localStorage.getItem('token');
localStorage.setItem("currentUserPage", 1)
localStorage.setItem("lastUserPage", 'False')
localStorage.setItem("user_status", '2')
localStorage.setItem("user_gender", '2')
localStorage.setItem("searchUserStatus", "False");

const status_admin = '1';
let pageSizeDefault = 5

// ------- User
// Load user data 
async function fetchUserChangePassword(token, phoneNumber, newPassword) {
    const requestBody = {
        phoneNumber: phoneNumber.toString(),
        newPassword: newPassword
    };
    return await axios.put(`https://localhost:7018/api/ManagerAccount/ChangePasswordAdmin?token=${token}`, requestBody);
}

async function fetchUserProfile(token, phoneNumber) {
    return await axios.get(`https://localhost:7018/api/User/GetProfile?token=${token}&phoneNumber=${phoneNumber}`)
}

async function fetchUserRestore(phoneNumber) {
    return await axios.put(`https://localhost:7018/api/ManagerAccount/UnLockAccount?phoneNumber=${phoneNumber}`)
}

async function fetchUserLock(phoneNumber) {
    return await axios.put(`https://localhost:7018/api/ManagerAccount/LockAccount?phoneNumber=${phoneNumber}`)
}

async function fetchUserUnlock(phoneNumber) {
    return await axios.put(`https://localhost:7018/api/ManagerAccount/UnLockAccount?phoneNumber=${phoneNumber}`)
}

async function fetchUserDeleteData(token, phoneNumber) {
    return await axios.delete(`https://localhost:7018/api/ManagerCustomer/DeleteCustomer?token=${token}&phoneNumber=${phoneNumber}`)
}

async function fetchUserSearchData(token, textInput, pageIndex, pageSize) {
    return await axios.get(`https://localhost:7018/api/ManagerCustomer/SearchCustomers?token=${token}&textInput=${textInput}&pageIndex=${pageIndex}&pageSize=${pageSize}`)
}

async function fetchUserData(token, status, gender, pageIndex, pageSize) {
    return await axios.get(`https://localhost:7018/api/ManagerCustomer/GetAllCustomers?token=${token}&status=${status}&gender=${gender}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
}

function renderUserPage(data) {
    const totalPage = Math.ceil(parseInt(data.totalRecord) / pageSizeDefault);
    const tableBody = document.getElementById('userTableBody');
    const paginationLoad = document.getElementById('pagination_user--nav');
    const totalCustomerText = document.getElementById('total_customers');
    var currentUserPage = parseInt(localStorage.getItem("currentUserPage")) || 1;

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
                    ${user.status === 0 ?
                `<i class="fa-solid fa-lock-open text-success" onclick="lockUser('${user.phoneNumber}')"></i>` :
                (user.status === -1 ? `<i class="fa-solid fa-lock text-danger" onclick="unLockUser('${user.phoneNumber}')"></i>` :
                    (user.status === -2 ? `<i class="fa-solid fa-square-minus" id="restoreUserIcon" onclick="restoreUser('${user.phoneNumber}')"></i>` : '')
                )
            }
                </td>
                <td class="popup_trigger--user" onclick="togglePopup(event,'popup_content--user')">
                    <span class="material-icons-sharp">more_horiz</span>
                    <div class="popup_content--user d-flex d-none flex-column">
                        <a href="#" class="popup-option" onclick="detailUser(event,${user.phoneNumber})">Detail</a>
                        ${user.status === -2 ? '' : `<a href="#" class="popup-option" onclick="deleteUser(event,${user.phoneNumber})">Delete</a>`}
                    </div>
                </td>
            </tr>`;
        tableBody.innerHTML += row;
    });
    totalCustomerText.innerHTML = data.totalRecord;
    renderUserPagination(currentUserPage, totalPage, paginationLoad);
}

// Pagination user
function renderUserPagination(currentUserPage, totalPage, paginationLoad) {
    var lastUserPage = localStorage.getItem("lastUserPage") === "True";
    var updateColorPage = localStorage.getItem("updateUserColor");
    var residual = lastUserPage ? (totalPage % 4 > 0 ? totalPage % 4 - 1 : totalPage % 4 + 3) : 0;
    var htmlContent = '';
    let onClickFunction = localStorage.getItem("searchUserStatus") == "True" ? "searchUser" : "handleUsersPage";

    htmlContent += `<li class="page_item--user">
                        <a class="page-link" href="#" onclick="firsUsertPage(${1})">
                            <i class="fa-solid fa-angles-left"></i>
                        </a>
                    </li>
                    <li class="page_item--user">
                        <a class="page-link" href="#" onclick="periousUserPage(${lastUserPage ? currentUserPage - 4 - residual : currentUserPage - 4})">
                            <i class="fa fa-angle-left"></i>
                        </a>
                    </li>`;

    for (let page = lastUserPage ? totalPage - residual : currentUserPage; page <= totalPage && page < currentUserPage + 4; page++) {

        htmlContent += `<li class="page_item--user ${page == updateColorPage ? 'active' : ''}">
                            <a class="page-link" onclick="${onClickFunction}(${page},${pageSizeDefault})" href="#" value="${page}">${page}</a>
                        </li>`;
    }

    htmlContent += `<li class="page_item--user">
                        <a class="page-link" href="#" onclick="nextUserPage(${currentUserPage + 4},${totalPage})">
                            <i class="fa fa-angle-right"></i>
                        </a>
                    </li>
                    <li class="page_item--user">
                        <a class="page-link" href="#" onclick="lastUserPage(${totalPage})">
                            <i class="fa-solid fa-angles-right"></i>
                        </a>
                    </li>`;

    paginationLoad.innerHTML += htmlContent;
    localStorage.setItem("lastUserPage", "False");
    localStorage.setItem("currentUserPage", lastUserPage ? currentUserPage - residual : currentUserPage);
}

function handleUsersPage(pageIndex, pageSize) {
    localStorage.setItem("updateUserColor", pageIndex);
    localStorage.setItem("searchUserStatus", "False");

    const status_user = localStorage.getItem("user_status");
    const status_gender = localStorage.getItem("user_gender");
    fetchUserData(token, status_user, status_gender, pageIndex, pageSize)
        .then(response => {
            renderUserPage(response.data);

            console.log(response.data)
        })
        .catch(error => console.error("Error:", error));
}

function nextUserPage(nextUserPage, totalPage) {
    if (nextUserPage > totalPage) {
        return;
    }
    else {
        localStorage.setItem("currentUserPage", nextUserPage)
        if (localStorage.getItem("searchUserStatus") == "False") {
            handleUsersPage(nextUserPage, pageSizeDefault)
        }
        else {
            searchUser(nextUserPage, pageSizeDefault)
        }
    }
}

function periousUserPage(periousUserPage) {
    if (periousUserPage < 1) {
        return;
    }
    else {
        localStorage.setItem("currentUserPage", periousUserPage)
        if (localStorage.getItem("searchUserStatus") == "False") {
            handleUsersPage(periousUserPage, pageSizeDefault)
        }
        else {
            searchUser(periousUserPage, pageSizeDefault)
        }
    }
}

function lastUserPage(page) {
    localStorage.setItem("lastUserPage", "True")
    localStorage.setItem("currentUserPage", page)

    if (localStorage.getItem("searchUserStatus") == "False") {
        handleUsersPage(page, pageSizeDefault)
    }
    else {
        searchUser(page, pageSizeDefault)
    }
}

function firsUsertPage(page) {
    localStorage.setItem("currentUserPage", page)
    if (localStorage.getItem("searchUserStatus") == "False") {
        handleUsersPage(page, pageSizeDefault)
    }
    else {
        searchUser(page, pageSizeDefault)
    }
}

// Filter user
function getUserByStatus(status) {
    if (status === "lock") {
        localStorage.setItem("user_status", '-1')
    }
    else if (status === "unlock") {
        localStorage.setItem("user_status", '0')
    }
    else if (status === "delete") {
        localStorage.setItem("user_status", '-2')
    }
    else {
        localStorage.setItem("user_status", '2')
    }
    handleUsersPage(1, pageSizeDefault);
}

function getUserByGender(gender) {
    if (gender === "male") {
        localStorage.setItem("user_gender", '0')
    }
    else if (gender === "female") {
        localStorage.setItem("user_gender", '1')
    }
    else {
        localStorage.setItem("user_gender", '2')
    }
    handleUsersPage(1, pageSizeDefault);
}


// Search user
function searchUser(pageIndex, pageSize) {
    localStorage.setItem("updateUserColor", pageIndex);
    localStorage.setItem("searchUserStatus", "True");
    var inputText = document.getElementById("form1").value;
    if (inputText == "") {
        handleUsersPage(1, pageSizeDefault)
    }
    else {
        fetchUserSearchData(token, inputText, pageIndex, pageSize)
            .then(response => {
                renderUserPage(response.data);
            })
            .catch(error => console.error("Error:", error));
    }
}

// Xử lý hiển thị popup
document.addEventListener('click', function (event) {
    const popupsUser = document.querySelectorAll('.popup_content--user');
    const popupsProduct = document.querySelectorAll('.popup_content--product');
    const clickedPopup = event.target.nextElementSibling;

    if (!clickedPopup || !clickedPopup.classList.contains('popup_content--user')) {
        popupsUser.forEach(popup => {
            popup.classList.add('d-none');
        });
    }

    if (!clickedPopup || !clickedPopup.classList.contains('popup_content--product')) {
        popupsProduct.forEach(popup => {
            popup.classList.add('d-none');
        });
    }
});

function hideAllPopups(event, className) {
    const popups = document.querySelectorAll('.' + className);
    const clickedPopup = event.target.nextElementSibling;
    popups.forEach(popup => {
        if (popup !== clickedPopup) {
            popup.classList.add('d-none');
        }
    });
}

function togglePopup(event, className) {
    const popupContent = event.target.nextElementSibling;
    popupContent.classList.toggle('d-none');
    event.stopPropagation();
    hideAllPopups(event, className);
}

// Xử lý action trong popup
function detailUser(event, phoneNumber) {
    openProfile(event, phoneNumber);
    event.stopPropagation();
}

async function deleteUser(event, phoneNumber) {
    await fetchUserDeleteData(token, phoneNumber);
    alert("Delete Successfully !")
    handleUsersPage(1, pageSizeDefault);
    event.stopPropagation();
}

async function lockUser(phoneNumber) {
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

async function unLockUser(phoneNumber) {
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

async function restoreUser(phoneNumber) {
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
function closeProfile() {
    document.querySelector(".user_profile").style.display = "none";
    document.querySelector(".overlay").style.display = "none";
}

function openProfile(event, phoneNumber) {
    event.stopPropagation();
    fetchUserProfile(token, phoneNumber)
        .then(response => {
            console.log(response.data)
            renderUserProfile(response.data);
        })
        .catch(error => console.error("Error:", error));
    document.querySelector(".user_profile").style.display = "block";
}

function renderUserProfile(data) {
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
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Status:</span>${data.status === 0 ? 'Unlock' : (data.status === -1 ? 'Lock' : (data.status === -2 ? 'Delete' : ''))}</li>
                                    <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Registration Date:</span> www.example.com</li>
                                    ${data.status === -2 ? '' : `<li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">New password:</span> 
                                    <input type="password" class="form-control" id="newPasswordField">
                                </li>
                                <li class="mb-2 mb-xl-3 display-28"><span class="display-26 text-secondary me-2 font-weight-600">Repeat password:</span> 
                                    <input type="password" class="form-control" id="repeatPasswordField">
                                </li>`}              
                                </ul>
                                <div class="button_profile d-flex justify-content-end mx-2">
                                    <button type="button" class="btn btn-light mx-2" onclick="closeProfile()">Cancel</button>
                                    <button type="button" class="btn btn-success" onclick="${data.status === -2 ? `restoreUser(${data.phoneNumber})` : `savePassword(${data.phoneNumber})`}">${data.status === -2 ? 'Restore' : 'Save'}</button>
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

function savePassword(phoneNumber) {
    var newPassword = document.getElementById("newPasswordField").value;
    var repeatPassword = document.getElementById("repeatPasswordField").value;

    if (newPassword !== repeatPassword) {
        alert("New password and repeat password do not match");
    }
    else {
        fetchUserChangePassword(token, phoneNumber, newPassword)
            .catch(error => console.error("Error:", error));
        alert("Update password successfully!")
    }
}


// --------------- Dashboard
// Load dashboard data
function fetchAdminData(token) {
    return axios.get(`https://localhost:7018/api/ManagerAccount/GetAdminInfor?token=${token}`)
}

function populateAdmin(data) {
    const tableBody = document.querySelector('.right-section .nav .profile');
    tableBody.innerHTML = '';

    tableBody.innerHTML += `
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

function handleAdminPage() {
    fetchAdminData(token)
        .then(response => {
            populateAdmin(response.data);
        })
        .catch(error => console.error("Error:", error));
}

// -------------------- Seafoods
// Khai báo
localStorage.setItem("currentProductPage", 1)
localStorage.setItem("lastProductPage", 'False')
localStorage.setItem("searchProductStatus", "False");
localStorage.setItem("searchProductByType", "");

async function fetchProductSeachData(nameProduct, pageIndex, pageSize) {
    return await axios.post(`https://localhost:7018/api/SeaFood/SearchSeafood?nameSeaFood=${nameProduct}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
}

async function fetchProductData(pageIndex, pageSize) {
    return await axios.get(`https://localhost:7018/api/SeaFood?pageNumber=${pageIndex}&pageSize=${pageSize}`);
}

async function fetchFiterSeaFoodData(token) {
    return await axios.get(`https://localhost:7018/api/Type/GetTypes?token=${token}`);
}

async function fetchSearchProductByTypeData(nameType, pageIndex, pageSize) {
    return await axios.post(`https://localhost:7018/api/SeaFood/SearchSeafoodByType?nameSeaFood=${nameType}&pageIndex=${pageIndex}&pageSize=${pageSize}`)
}

async function fetchAddProduct(token) {
    var name = document.getElementById("product_detail--name").value
    var instruct = document.getElementById("product_detail--instruct").value
    var unit = document.getElementById("product_detail--unit").value
    var origin = document.getElementById("product_detail--origin").value
    var expirationDate = document.getElementById("product_detail--preserve").value
    var price = document.getElementById("product_detail--price").value
    var quantity = document.getElementById("product_detail--amount").value
    var selectVoucher = document.getElementById("selectVoucher");
    var voucherId = selectVoucher.value;
    var selectType = document.getElementById("selectType");
    var nameType = selectType.value;
    var description = tinymce.activeEditor.getContent("textPalce");

    var primaryImage = document.getElementById("selectedImage").dataset.filename; // Lấy tên ảnh chính từ dataset
    var seafoodImages = [];
    var imageInputs = ['selectedImageFirstChild', 'selectedImageSecondChild', 'selectedImageThirdChild', 'selectedImageFourthChild', 'selectedImageFiveChild'];

    imageInputs.forEach(id => {
        var imageName = document.getElementById(id).dataset.filename; // Lấy tên ảnh từ dataset
        if (imageName) {
            seafoodImages.push({ nameImage: imageName });
        }
    });

    const requestBody = {
        name: name,
        instruct: instruct,
        unit: unit,
        origin: origin,
        expirationDate: expirationDate,
        price: price,
        voucher: voucherId,
        nameType: nameType,
        description: description,
        quantity: quantity,
        primaryImage: primaryImage,
        seafoodImages: seafoodImages
    };
    return await axios.post(`https://localhost:7018/api/ManagerSeaFood/AddSeaFood?token=${token}`, requestBody);
}

async function fetchProductDetail(id) {
    return await axios.get(`https://localhost:7018/api/SeaFood/SeaFoodDetail?id=${id}`);
}

async function fetchUpdateProduct(id){
    var description = tinymce.activeEditor.getContent("textPalce");
    var name = document.getElementById("product_detail--name").value
    var instruct = document.getElementById("product_detail--instruct").value
    var unit = document.getElementById("product_detail--unit").value
    var origin = document.getElementById("product_detail--origin").value
    var expirationDate = document.getElementById("product_detail--preserve").value
    var price = document.getElementById("product_detail--price").value
    var quantity = document.getElementById("product_detail--amount").value
    var selectVoucher = document.getElementById("selectVoucher");
    var voucherId = selectVoucher.value;
    var selectType = document.getElementById("selectType");
    var nameType = selectType.value;

    var primaryImage = document.getElementById("selectedImage").dataset.filename; // Lấy tên ảnh chính từ dataset
    var seafoodImages = [];
    var imageInputs = ['selectedImageFirstChild', 'selectedImageSecondChild', 'selectedImageThirdChild', 'selectedImageFourthChild', 'selectedImageFiveChild'];

    imageInputs.forEach(id => {
        var imageName = document.getElementById(id).dataset.filename; // Lấy tên ảnh từ dataset
        if (imageName) {
            seafoodImages.push({ nameImage: imageName });
        }
    });

    const requestBody = {
        name: name,
        instruct: instruct,
        unit: unit,
        origin: origin,
        expirationDate: expirationDate,
        price: price,
        voucher: voucherId,
        nameType: nameType,
        description: description,
        quantity:  parseInt(quantity),
        primaryImage: primaryImage,
        seafoodImages: seafoodImages
    };
    return await axios.put(`https://localhost:7018/api/ManagerSeaFood/UpdateSeaFood?idSeaFood=${id}&token=${token}`,requestBody)
}

async function fetchDeleteProduct(token,id){
    return await axios.delete(`https://localhost:7018/api/ManagerSeaFood/DeleteSeaFood?token=${token}&seafoodId=${id}`)
}

function renderFiterProductPage(data) {
    const filterProduct = document.getElementById('filter_product');
    filterProduct.innerHTML = ' <option selected>Tất cả</option>';
    console.log(data)
    data.forEach((type) => {
        const select = `<option value="${type.nameType}">${type.nameType}</option>`
        filterProduct.innerHTML += select;
    })
}

function renderProductPage(data) {
    const totalPage = Math.ceil(parseInt(data.totalRecord) / pageSizeDefault);
    const tableBody = document.getElementById('productTableBody');
    const paginationLoad = document.getElementById('pagination_product--nav');
    const totalCustomerText = document.getElementById('total_products');
    var currentProductPage = parseInt(localStorage.getItem("currentProductPage")) || 1;

    tableBody.innerHTML = '';
    paginationLoad.innerHTML = '';

    if (!data || data.listSeaFood.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="7" class="text-center text-danger">Không có dữ liệu</td></tr>';
        totalCustomerText.innerHTML = '0';
        return;
    }

    data.listSeaFood.forEach((product) => {
        const formattedPrice = parseFloat(product.price).toLocaleString('vi-VN');
        const row = `
            <tr>
                <th scope="row">${product.rowNum}</th>
                <td>${product.name}</td>
                <td>${formattedPrice}</td>
                <td>${product.unit}</td>
                <td>${product.nameType}</td>
                <td>${product.idVoucher == null ? '' : product.idVoucher}</td>
                <td class="popup_trigger--product" onclick="togglePopup(event,'popup_content--product')">
                    <span class="material-icons-sharp">more_horiz</span>
                    <div class="popup_content--product d-flex d-none flex-column">
                        <a href="#" class="popup-option" onclick="detailProduct(event,${product.id})">Detail</a>
                        <a href="#" class="popup-option" onclick="deleteProduct(event,${product.id})">Delete</a>
                    </div>
                </td>
            </tr>`;
        tableBody.innerHTML += row;
    });
    totalCustomerText.innerHTML = data.totalRecord;
    renderProductPagination(currentProductPage, totalPage, paginationLoad);

}

function renderProductDetail(productData) {
    console.log(`${productData.description.toString()}`)
    tinymce.get('textPalce').setContent(productData.description);
    document.getElementById('product_detail--name').value = productData.name;
    document.getElementById('product_detail--instruct').value = productData.instruct;
    document.getElementById('product_detail--unit').value = productData.unit;
    document.getElementById('product_detail--amount').value = productData.quantity;
    document.getElementById('product_detail--origin').value = productData.origin;
    document.getElementById('product_detail--preserve').value = productData.expirationDate;
    document.getElementById('product_detail--price').value = productData.price;
    document.getElementById('selectType').value = productData.nameType;
    document.getElementById('selectVoucher').value = productData.idVourcher;
    toggleUpdateProductForm(productData.id);
}

// Pagination Product
function renderProductPagination(currentProductPage, totalPage, paginationLoad) {
    var lastProductPage = localStorage.getItem("lastProductPage") === "True";
    var updateColorPage = localStorage.getItem("updateProductColor");
    var residual = lastProductPage ? (totalPage % 4 > 0 ? totalPage % 4 - 1 : totalPage % 4 + 3) : 0;
    var htmlContent = '';
    let onClickFunction = localStorage.getItem("searchProductStatus") == "True" ? "searchProduct" : (localStorage.getItem("searchProductByType") !== "" ? "getProductByType" : "handleProductsPage");

    htmlContent += `<li class="page_item--Product">
                        <a class="page-link" href="#" onclick="firstProductPage(${1})">
                            <i class="fa-solid fa-angles-left"></i>
                        </a>
                    </li>
                    <li class="page_item--Product">
                        <a class="page-link" href="#" onclick="periousProductPage(${lastProductPage ? currentProductPage - 4 - residual : currentProductPage - 4})">
                            <i class="fa fa-angle-left"></i>
                        </a>
                    </li>`;

    for (let page = lastProductPage ? totalPage - residual : currentProductPage; page <= totalPage && page < currentProductPage + 4; page++) {

        htmlContent += `<li class="page_item--Product ${page == updateColorPage ? 'active' : ''}">
                            <a class="page-link" onclick="${onClickFunction}(${page},${pageSizeDefault})" href="#" value="${page}">${page}</a>
                        </li>`;
    }

    htmlContent += `<li class="page_item--product">
                        <a class="page-link" href="#" onclick="nextProductPage(${currentProductPage + 4},${totalPage})">
                            <i class="fa fa-angle-right"></i>
                        </a>
                    </li>
                    <li class="page_item--product">
                        <a class="page-link" href="#" onclick="lastProductPage(${totalPage})">
                            <i class="fa-solid fa-angles-right"></i>
                        </a>
                    </li>`;

    paginationLoad.innerHTML += htmlContent;
    localStorage.setItem("lastProductPage", "False");
    localStorage.setItem("currentProductPage", lastProductPage ? currentProductPage - residual : currentProductPage);
}

function handleProductsPage(pageIndex, pageSize) {
    localStorage.setItem("updateProductColor", pageIndex);
    localStorage.setItem("searchProductStatus", "False");

    fetchFiterSeaFoodData(token)
        .then(res => {
            renderFiterProductPage(res.data)
        })
        .catch(error => console.error("Error:", error));

    fetchProductData(pageIndex, pageSize)
        .then(response => {
            renderProductPage(response.data);
        })
        .catch(error => console.error("Error:", error));
}

function nextProductPage(page, totalPage) {
    if (page > totalPage) {
        return;
    }
    else {
        localStorage.setItem("currentProductPage", page)
        if (localStorage.getItem("searchProductStatus") == "False") {
            handleProductsPage(page, pageSizeDefault)
        }
        else {
            searchProduct(page, pageSizeDefault)
        }
    }
}

function periousProductPage(page) {
    if (page < 1) {
        return;
    }
    else {
        localStorage.setItem("currentProductPage", page)
        if (localStorage.getItem("searchProductStatus") == "False") {
            handleProductsPage(page, pageSizeDefault)
        }
        else {
            searchProduct(page, pageSizeDefault)
        }
    }
}

function lastProductPage(page) {
    localStorage.setItem("lastProductPage", "True")
    localStorage.setItem("currentProductPage", page)

    if (localStorage.getItem("searchProductStatus") == "False") {
        handleProductsPage(page, pageSizeDefault)
    }
    else {
        searchProduct(page, pageSizeDefault)
    }
}

function firstProductPage(page) {
    localStorage.setItem("currentProductPage", page)
    if (localStorage.getItem("searchProductStatus") == "False") {
        handleProductsPage(page, pageSizeDefault)
    }
    else {
        searchProduct(page, pageSizeDefault)
    }
}

// Search production
function searchProduct(pageIndex, pageSize) {
    localStorage.setItem("updateProductColor", pageIndex);
    localStorage.setItem("searchProductStatus", "True");
    localStorage.setItem("searchProductByType", "")

    var nameProduct = document.getElementById("form_search").value;
    if (nameProduct == "") {
        handleProductsPage(1, pageSizeDefault)
    }
    else {
        fetchProductSeachData(nameProduct, pageIndex, pageSize)
            .then(response => {
                renderProductPage(response.data);
            })
            .catch(error => console.error("Error:", error));
    }
}

// Get product by type
function getProductByType(pageIndex, pageSize) {
    var selectElement = document.getElementById("filter_product");
    var nameType = selectElement.value;

    localStorage.setItem("updateProductColor", pageIndex);
    if (nameType == "Tất cả") {
        localStorage.setItem("searchProductByType", "")
        handleProductsPage(1, pageSizeDefault)
    }
    else {
        localStorage.setItem("searchProductByType", nameType)
        fetchSearchProductByTypeData(nameType, pageIndex, pageSize)
            .then(response => {
                renderProductPage(response.data);
            })
            .catch(error => console.error("Error:", error));
    }
}

// Xử lý action trong popup
function detailProduct(event, idProduct) {
    fetchProductDetail(idProduct)
        .then(response => {
            renderProductDetail(response.data);
        })
        .catch(erro => console.error("Error:", erro));
    event.stopPropagation();
}

function deleteProduct(event, idProduct) {
    if(confirm("Do you want to delete this product ?")){
        fetchDeleteProduct(token,idProduct)
        .then(response=>{
            alert(response.data);
            handleProductsPage(pageIndex, pageSize);
        })
        .catch(error=>{
            console.error('Error: ',error);
            alert("An error occurred while deleting the product.");
        })
    }
    
    event.stopPropagation();
}

// Add product
function addProduct() {
    fetchAddProduct(token);
    alert("Add successfully !")
    handleProductsPage(pageIndex,pageSize)
}


function updateProduct(idProduct){
    fetchUpdateProduct(idProduct);
    alert("Update successfully !")
    handleProductsPage(pageIndex,pageSize)
}

// -------------------- Chart

// -------------------- Blog