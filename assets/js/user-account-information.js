// Ẩn hiện của thanh sidebar
function displayname(nameClass,color) {
    var boxes = document.querySelectorAll('.displaynamee > div');
    boxes.forEach(function(box) {
        if (!box.classList.contains('d-none')) {
            box.classList.add('d-none');
        }
    });
    var targetBox = document.querySelector(nameClass);
    if (targetBox) {
        targetBox.classList.remove('d-none');
    } else {
        console.error('Element with the provided selector not found: ' + nameClass);
    }
    var colors = document.querySelectorAll('.list-group > button');
        colors.forEach(function(color) {
        if (color.classList.contains('active__buttom')) {
            color.classList.remove('active__buttom');
        }
    });
    var colordisplay = document.querySelector(color);
  
    if (colordisplay) {
        colordisplay.classList.add('active__buttom');
    } else {
        console.error('Element with the provided selector not found: ' + color);
    }
}
// ẩn hiện của history
function displaymove(moveClass, color) {
    // Chọn tất cả các phần tử con trực tiếp của '.move'
    var move = document.querySelectorAll('.move > div');
    // Ẩn tất cả các phần tử này
    move.forEach(function(moves) {
        moves.classList.add('d-none');
    });

    // Hiển thị phần tử có lớp tương ứng với moveClass
    var target = document.querySelector(moveClass);
    if (target) {
        target.classList.remove('d-none');
        console.log('Đã hiển thị phần tử với lớp:', moveClass); // Sử dụng console.log để gỡ lỗi tốt hơn
    } else {
        console.error('Không tìm thấy phần tử với selector đã cung cấp:', moveClass);
    }

    // Xử lý lớp 'active__history' cho các nút
    var colors = document.querySelectorAll('.list-history > button');
    colors.forEach(function(button) {
        if (button.classList.contains('active__history')) {
            button.classList.remove('active__history');
            var hr = button.querySelector('hr');
            if (hr) {
                hr.classList.add('d-none');
            }
        }
    });

    var colordisplay = document.querySelector(color);
    if (colordisplay) {
        colordisplay.classList.add('active__history');
        var hr = colordisplay.querySelector('hr');
        if (hr) {
            hr.classList.remove('d-none');
        }
    } else {
        console.error('Không tìm thấy phần tử với selector đã cung cấp:', color);
    }
}

// ẩn hiện của information 
document.querySelector('.change__name').addEventListener('click', function() {
    var buttonText = this.innerText;
    if (buttonText === "Thay đổi") {
        this.innerText = "Lưu";
        document.querySelectorAll('.change').forEach(function(input) {
            input.removeAttribute('disabled');
        });
    } else {
        this.innerText = "Thay đổi";
        document.querySelectorAll('.change').forEach(function(input) {
            input.setAttribute('disabled', '');
        });

        
    }
});