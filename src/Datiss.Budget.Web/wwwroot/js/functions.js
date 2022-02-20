
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
    return Swal.fire({
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

function ShowPermissionErorr() {
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

function ShowCommonErorr() {
    Swal.fire({
        title: '<span class="my-font">خطا</span>',
        html: `<span class="my-font">مشکلی به وجود آمده است لطفا مجددا تلاش کنید .</span>`,
        type: 'error',
        confirmButtonColor: '#2F8BE6',
        animation: false,
        customClass: 'animated flipInX',
        confirmButtonText: 'بستن',
        confirmButtonClass: 'btn btn-outline-danger mr-1 my-font',
        buttonsStyling: false,
    });
}

function removeAllTagsAndTrim(html) {
    return !html ? "" : $.trim(html.replace(/(<([^>]+)>)/ig, ""));
}

function waiting(elem, hide) {
    if (hide == 'hide') { $(elem).waitMe(hide); }
    else { $(elem).waitMe({ effect: 'roundBounce', text: '', bg: 'rgba(255,255,255,0.7)', color: '#000', sizeW: '', sizeH: '', source: '' }); }
}

function dataAjaxBegin() {
    waiting('body', 'show');
    return true;
}

function dataAjaxSuccess(data, status, xhr) {
    resultDialog('dataAjaxSuccess', 'نتیجه عملیات درخواستی', 'عملیات درخواستی با موفقیت انجام شد', 'check', 'success');
    waiting('body', 'hide');
}


function resultDialog(id, title, text, icon, type) {
    $('#' + id).remove();

    var str =
        '<div class="modal fade" Id=' + id + '>' +
        '<div class="modal-dialog modal-' + type + ' modal-alert">' +
        '<div class="modal-content">' +
        '<div class="modal-header bg-info">' +
        '<h6 class="modal-title"><i class="fa fa-' + icon + ' mr-1"></i>' + title + '</h6>' +
        '<button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span aria-hidden="true"><i class="ft-x font-medium-2 text-bold-700"></i></span></button>' +
        '</div >' +
        '<div class="modal-body">' +
        '<div>' + text + '</div>' +
        '</div>' +
        '<div class="modal-footer">' +
        '<button type="button" class="btn btn-info mb-1" data-dismiss="modal">تایید</button>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    $('body').append(str);
    $('#' + id).modal();
}

function dataAjaxFailure(xhr, status, error) {
    if ($('#dialogDiv').is(':visible')) {
        $('#dialogDiv').modal('hide');
        $('.modal-backdrop').remove();
    }

    $('#dataAjaxFailure').remove();
    if ((xhr != null || xhr != undefined) && (xhr.status == 401)) {
        //alert('1');
        window.location.href = xhr.getResponseHeader('Location');
        return;
    }

    if ((xhr != null || xhr != undefined) && !isNullOrEmpty(xhr.responseText)) {
        //alert('2');
        ////notification(xhr.responseText, 'error', false);
        resultDialog('dataAjaxFailure', 'نتیجه عملیات درخواستی', xhr.responseText, 'exclamation-triangle', 'danger');
    }
    else {
        //alert('3');
        //notification('عملیات مورد نظر با خطا مواجه شد، با تیم پشتیبانی تماس بگیرید.', 'error', true);
        resultDialog('dataAjaxFailure', 'نتیجه عملیات درخواستی', 'عملیات مورد نظر با خطا مواجه شد، با تیم پشتیبانی تماس بگیرید.', 'exclamation-triangle', 'danger');
    }

    //alert('4');
    waiting('body', 'hide');
}

function dataAjaxNoContent(message) {
    if ($('#dialogDiv').is(':visible')) {
        $('#dialogDiv').modal('hide');
        $('.modal-backdrop').remove();
    }

    $('#dataAjaxFailure').remove();

    if (!isNullOrEmpty(message)) {
        notification(message, 'error', false);
    }
    else {
        notification('اطلاعات درخواستی یافت نشد.', 'error', false);
    }

    waiting('body', 'hide');
}

function linkAjaxBegin(xhr, settings) {
    var token = $('input[name=__RequestVerificationToken]').val();
    settings.data = settings.data + '&__RequestVerificationToken=' + token;
    waiting('body', 'show');
}

function checkRTL(s) {
    var ltrChars = 'A-Za-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02B8\u0300-\u0590\u0800-\u1FFF' + '\u2C00-\uFB1C\uFDFE-\uFE6F\uFEFD-\uFFFF',
        rtlChars = '\u0591-\u07FF\uFB1D-\uFDFD\uFE70-\uFEFC',
        rtlDirCheck = new RegExp('^[^' + ltrChars + ']*[' + rtlChars + ']');
    return rtlDirCheck.test(s);
}

function setDirection(selector) {
    var string = selector.val();
    for (var i = 0; i < string.length; i++) {
        var isRtl = checkRTL(string[i]);
        var dir = isRtl ? 'RTL' : 'LTR';
        if (dir === 'RTL') var finalDirection = 'RTL';
        if (finalDirection == 'RTL') dir = 'RTL';
    }
    if (dir === 'LTR') {
        selector.css("direction", "ltr");
    } else {
        selector.css("direction", "rtl");
    }
}

function GoTo(url) {
    window.location.href = url;
}

function isNullOrEmpty(value) {
    return (value == null || value == undefined || value === '' || value == ' ');
}

function notification(text, type, autoHide) {
    $.notify(text, {
        clickToHide: true,
        autoHide: autoHide,
        autoHideDelay: 5000,
        //globalPosition: 'bottom right',
        style: 'bootstrap',
        globalPosition: 'top right',
        className: type,
        showAnimation: 'slideDown',
        showDuration: 500,
        hideAnimation: 'slideUp',
        hideDuration: 200,
    });
}

function objectifyForm(formArray) {
    //serialize data function
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}
