$(document).ready(function () {
  $("#my-table").Tabledit({
    ordering: true,
    hideIdentifier: true,
    columns: {
      identifier: [0, "id"],
      editable: [
        [1, "نام", "input"],
        [
          2,
          "بیو",
          "textarea",
          '{"rows": "2", "cols": "4", "maxlength": "50", "wrap": "hard"}',
        ],
        [3, "سن", "number"],
        [4, "جنسیت", "select", '{"0":"مرد","1":"زن"}'],
      ],
    },
  //   buttons: {
  //     edit: {
  //         class: 'btn btn-sm btn-default',
  //         html: '<span class="glyphicon glyphicon-pencil"></span>',
  //         action: 'edit'
  //     },
  //     delete: {
  //         class: 'btn btn-sm btn-default',
  //         html: '<span class="glyphicon glyphicon-trash"></span>',
  //         action: 'delete'
  //     },
  //     save: {
  //         class: 'btn btn-sm btn-success',
  //         html: 'Save'
  //     },
  //     restore: {
  //         class: 'btn btn-sm btn-warning',
  //         html: 'Restore',
  //         action: 'restore'
  //     },
  //     confirm: {
  //         class: 'btn btn-sm btn-danger',
  //         html: 'Confirm'
  //     }
  // }
  });
  $("#addRow").on("click", function () {
    var data = " <tr><td></td><td></td><td></td><td></td><td></td></tr>";
    $("#my-table tbody").append(data);
    $("#my-table").Tabledit("update");
    // ajax example
    //     $.ajax({
    //       type: "POST",
    //       url: "url ajax ro bezar",
    //       datatype: "html",
    //       data: {
    //         parameters: Parameters,
    //       },
    //       success: function (data) {
    //         // Add 'html' data to table
    //         $("#example9 tbody").append(data);

    //         // Update Tabledit plugin
    //         $("#example9").Tabledit("update");
    //       },
    //       error: function () {},
    //     });
  });
});
