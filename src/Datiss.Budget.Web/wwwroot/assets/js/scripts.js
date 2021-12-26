(function(window, undefined) {
  'use strict';
  /*
  NOTE:
  ------
  PLACE HERE YOUR OWN JAVASCRIPT CODE IF NEEDED
  WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR JAVASCRIPT CODE PLEASE CONSIDER WRITING YOUR SCRIPT HERE.  */

    function showInfoMsg(title, text) {
        Swal.fire({
            title: title,
            text: text,
            type: 'info',
            confirmButtonColor: '#2F8BE6',
            animation: false,
            customClass: 'animated flipInX',
            confirmButtonText: 'بستن',
            confirmButtonClass: 'btn btn-warning my-font',
            buttonsStyling: false,
        });
    };

    function showErrorMsg(text) {
        Swal.fire({
            title: 'خطا',
            text: text,
            type: 'warning',
            confirmButtonColor: '#2F8BE6',
            animation: false,
            customClass: 'animated flipInX',
            confirmButtonText: 'بستن',
            confirmButtonClass: 'btn btn-warning my-font',
            buttonsStyling: false,
        });
    };

})(window);