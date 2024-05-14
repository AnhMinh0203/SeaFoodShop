// Mã JavaScript để xử lý sự kiện nhấn nút và tăng giá trị bộ đếm
document.addEventListener("DOMContentLoaded", function() {
    // Lấy tất cả các phần tử có class "nutProfile"
    var nuts = document.querySelectorAll(".nutProfile");

    // Lặp qua từng phần tử nút
    nuts.forEach(function(nut) {
        // Lấy phần tử bộ đếm cho mỗi nút
        var boDem = nut.querySelector("#demSo");

        // Khởi tạo giá trị ban đầu của bộ đếm cho mỗi nút
        var soLan = 0;

        // Thêm trình nghe sự kiện click cho mỗi nút
        nut.addEventListener("click", function(event) {
            // Ngăn chặn hành động mặc định của thẻ a
            event.preventDefault();

            // Tăng giá trị của bộ đếm cho nút này
            soLan++;

            // Cập nhật văn bản của bộ đếm cho nút này
            boDem.textContent = soLan;
        });
    });
});