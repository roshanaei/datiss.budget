
function numberOnly(id) {
    var element = document.getElementById(id);
    element.value = element.value.replace(/[^0-9]/gi, "");
}

function numberOnly(element) {
    element.value = element.value.replace(/[^0-9]/gi, "");
}

function successMsg(message) {
    Swal.fire({
        title: 'عملیات موفق',
        text: message,
        type: 'info',
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        confirmButtonText: 'بستن',
        confirmButtonClass: 'btn btn-warning my-font',
        buttonsStyling: false,
    });
}

function successMsgThenRedirect(message, redirectUrl) {
    Swal.fire({
        title: 'عملیات موفق',
        text: message,
        type: 'info',
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        confirmButtonText: 'بستن',
        confirmButtonClass: 'btn btn-warning my-font',
        buttonsStyling: false,
    }).then(function (result) {
        document.location.href = redirectUrl;
    });
}

function errorMsg(message) {
    Swal.fire({
        title: 'خطا',
        text: message,
        type: 'error',
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        confirmButtonText: 'بستن',
        confirmButtonClass: 'btn btn-warning my-font',
        buttonsStyling: false,
    });
}

function confirmDelete() {
    Swal.fire({
        title: 'برای حذف اطمینان دارید؟',
        text: "توجه داشته باشید که این عملیات قابل برگشت نیست!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        cancelButtonColor: '#F55252',
        cancelButtonText: 'انصراف',
        confirmButtonText: 'حذف اطلاعات!',

        confirmButtonClass: 'btn btn-warning my-font',
        cancelButtonClass: 'btn btn-danger  my-font ml-1',
        buttonsStyling: false,
    }).then(function (result) {
        if (result.value) {
            return true;
        }
        else {
            return false;
        }
    });
}

function askInfoMsg(title, message, cancelText, confirmText) {
    Swal.fire({
        title: title,
        text: message,
        type: 'info',
        showCancelButton: true,
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        confirmButtonText: confirmText,
        confirmButtonClass: 'btn btn-warning my-font',
        cancelButtonColor: '#F55252',
        cancelButtonText: cancelText,
        buttonsStyling: false,
    }).then(function (result) {
        return result.value;
    });
}

function askWarningMsg(title, message, cancelText, confirmText) {
    Swal.fire({
        title: 'برای حذف اطمینان دارید؟',
        text: "توجه داشته باشید که این عملیات قابل برگشت نیست!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        cancelButtonColor: '#F55252',
        cancelButtonText: cancelText,
        confirmButtonText: confirmText,
        confirmButtonClass: 'btn btn-warning my-font',
        cancelButtonClass: 'btn btn-danger  my-font ml-1',
        buttonsStyling: false,
    }).then(function (result) {
        return result.value;
    });
}

function ErorrPermission() {
    Swal.fire({
        title: '<span class="my-font">خطا</span>',
        html: `<span class="my-font">شما مجاز به انجام این عملیات نمی باشید.</span>`,
        type: 'error',
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        confirmButtonText: 'بستن',
        confirmButtonClass: 'btn btn-outline-danger mr-1 my-font',
        buttonsStyling: false,
    });
}