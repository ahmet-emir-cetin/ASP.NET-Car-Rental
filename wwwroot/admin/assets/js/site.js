$(document).ready(function() {

    // Add Row
    $("#add-row").DataTable({
        pageLength: 5,
    });

    $("#add-rowCustomer").DataTable({
        pageLength: 5,
    });
    var action =
        '<td> <div class="form-button-action"> <button type="button" data-bs-toggle="tooltip" title="" class="btn btn-link btn-primary btn-lg" data-original-title="Edit Task"> <i class="fa fa-edit"></i> </button> <button type="button" data-bs-toggle="tooltip" title="" class="btn btn-link btn-danger" data-original-title="Remove"> <i class="fa fa-times"></i> </button> </div> </td>';

    $("#addRowButton").click(function() {
        $("#add-row")
            .dataTable()
            .fnAddData([
                $("#addName").val(),
                $("#addPosition").val(),
                $("#addOffice").val(),
                action,
            ]);
        $("#addRowModal").modal("hide");
    });

});


$("#add-row").on('click', '#editbtnRes', function() {

    var action = $(this).data("action");
    var id = $(this).data("id");
    console.log("deneme " + id + " " + action);
    $.ajax({
        type: "GET",
        url: "/admin/reservation/" + action + "/" + id,
        success: function(data) {
            $("#modalReservationEdit").modal("toggle");
            $("#generalMessage").html(data);
        },
        error: function(data) {
            $("#modalReservationEdit").modal("toggle");
            $("#generalMessage").html(data);
        },
    });
}); // End of use strict

$("#add-rowCar").on('click', '#editbtnCar', function() {

    var action = $(this).data("action");
    var id = $(this).data("id");
    console.log("deneme " + id + " " + action);
    $.ajax({
        type: "GET",
        url: "/admin/car/" + action + "/" + id,
        success: function(data) {
            $("#modalCarEdit").modal("toggle");
            $("#editCarMessage").html(data);
        },
        error: function(data) {
            $("#modalCarEdit").modal("toggle");
            $("#editCarMessage").html(data);
        },
    });
}); // End of use strict

$("#add-rowCampaign").on('click', '#editbtnCampaign', function() {

    var action = $(this).data("action");
    var id = $(this).data("id");
    console.log("deneme " + id + " " + action);
    $.ajax({
        type: "GET",
        url: "/admin/campaign/" + action + "/" + id,
        success: function(data) {
            $("#modalCampaignEdit").modal("toggle");
            $("#editCampaignMessage").html(data);
        },
        error: function(data) {
            $("#modalCampaignEdit").modal("toggle");
            $("#editCampaignMessage").html(data);
        },
    });
}); // End of use strict

$("#add-rowCustomer").on('click', '#editbtnCustomer', function() {

    var action = $(this).data("action");
    var id = $(this).data("id");
    console.log("deneme " + id + " " + action);
    $.ajax({
        type: "GET",
        url: "/admin/customer/" + action + "/" + id,
        success: function(data) {
            $("#modalCustomerEdit").modal("toggle");
            $("#editCustomerMessage").html(data);
        },
        error: function(data) {
            $("#modalCustomerEdit").modal("toggle");
            $("#editCustomerMessage").html(data);
        },
    });
}); // End of use strict

(function($) {
    "use strict";

    $(document).ready(function() {
        var today = new Date();
        var endOfYear = new Date(today.getFullYear(), 11, 31);

        $('#datepickerRes').datepicker({
            autoclose: true,
            todayHighlight: true,
            startDate: today, // Bugünden itibaren tarih seçimine izin ver
            endDate: endOfYear // Yıl sonuna kadar tarih seçimine izin ver
        });
        $('#datepickerResOff').datepicker({
            autoclose: true,
            todayHighlight: true
        });
        $('#datepickerDriverDate').datepicker({
            autoclose: true,
            todayHighlight: true
        });
        $('#datepickerCustomerBDay').datepicker({
            autoclose: true,
            todayHighlight: true
        });

        // Başlangıç tarihi değiştiğinde, diğer tarih seçicilerin tarih aralığını güncelle
        $('#datepickerRes').on('changeDate', function(e) {
            var startDate = e.date;
            var maxEndDate = new Date(startDate);
            maxEndDate.setMonth(maxEndDate.getMonth() + 1); // 1 ay sonrasını hesapla

            // Bitiş tarih seçicisinin tarih aralığını güncelle
            $('#datepickerResOff').datepicker('setStartDate', startDate);
            $('#datepickerResOff').datepicker('setEndDate', maxEndDate);
        });
        // Tarih değiştiğinde butondaki yazıyı güncelle
        $('#datepickerResOff').on('changeDate', function() {
            var startDate = $('#datepickerRes').datepicker('getDate');
            var endDate = $('#datepickerResOff').datepicker('getDate');

            if (startDate && endDate) {
                // Gün farkını hesapla
                var timeDiff = endDate - startDate;
                var daysDiff = Math.floor(timeDiff / (1000 * 60 * 60 * 24) + 1); // Gün cinsinden
                $('#Reservation_RentalDay').val(daysDiff);
                console.log("RentalDay " + daysDiff)
            }
        });
    }); // End Document Ready Function

})(jQuery); // End jQuery