// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("document").ready(function() {

    var checkTcNum = function(value) {
        value = value.toString();
        var isEleven = /^[0-9]{11}$/.test(value);
        var totalX = 0;
        for (var i = 0; i < 10; i++) {
            totalX += Number(value.substr(i, 1));
        }
        var isRuleX = totalX % 10 == value.substr(10, 1);
        var totalY1 = 0;
        var totalY2 = 0;
        for (var i = 0; i < 10; i += 2) {
            totalY1 += Number(value.substr(i, 1));
        }
        for (var i = 1; i < 10; i += 2) {
            totalY2 += Number(value.substr(i, 1));
        }
        var isRuleY = ((totalY1 * 7) - totalY2) % 10 == value.substr(9, 0);
        return isEleven && isRuleX && isRuleY;
    };


    $('#tc').on('keyup focus blur load', function(event) {
        event.preventDefault();
        var isValid = checkTcNum($(this).val());
        console.log('isValid ', isValid);
        if (isValid) {

        } else {
            $('#tcValidate').text("Geçersiz TC numarası").attr('class', 'text-danger');
        }
    });

    $('#birthdayDate').on('input', function(e) {
        var value = $(this).val().replace(/[^0-9]/g, ''); // Sadece rakamları al

        if (value.length > 2 && value.length <= 4) {
            // İlk 2 rakamdan sonra / ekle
            value = value.substring(0, 2) + '-' + value.substring(2);
        }

        if (value.length > 4) {
            // 5. karakterden sonra (yani ayın sonunda) ikinci / ekle
            value = value.substring(0, 2) + '-' + value.substring(2, 4) + '-' + value.substring(4);
        }

        $(this).val(value);
    });

    $("#driverDate").datepicker({
        dateFormat: "dd-mm-yy",
        firstDay: 1
    });
    $("#birthdayDate").datepicker({
        dateFormat: "dd-mm-yy",
        firstDay: 1
    });

    $("#city").off("click");


}); //document.ready

const prevBtns = document.querySelectorAll(".btn-prev");
const nextBtns = document.querySelectorAll(".btn-next");
const progress = document.getElementById("progress");
const formSteps = document.querySelectorAll(".form-step");
const progressSteps = document.querySelectorAll(".progress-step");

let formStepsNum = 0;

nextBtns.forEach((btn) => {
    btn.addEventListener("click", function(event) {
        if ($("#lastForm").valid()) {
            event.preventDefault();
            formStepsNum++;
            updateFormSteps();
            updateProgressbar();
            if (formStepsNum == 1) {
                $(".form-1").removeClass("d-flex").addClass("d-none");
                $(".form-2").removeClass("d-none").addClass("d-flex");
                console.log("2. adım");
            }
            if (formStepsNum == 2) {
                $(".form-2").removeClass("d-flex").addClass("d-none");
                $(".form-3").removeClass("d-none").addClass("d-flex");
                console.log("3. adım");
            }
        }
    });
});

prevBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
        formStepsNum--;
        updateFormSteps();
        updateProgressbar();
        if (formStepsNum == 1) {
            $(".form-3").removeClass("d-flex").addClass("d-none");
            $(".form-2").removeClass("d-none").addClass("d-flex");
            console.log("2. adım");
        }
        if (formStepsNum == 0) {
            $(".form-2").removeClass("d-flex").addClass("d-none");
            $(".form-1").removeClass("d-none").addClass("d-flex");
            console.log("2. adım");
        }
    });
});

function updateFormSteps() {
    formSteps.forEach((formStep) => {
        formStep.classList.contains("form-step-active") &&
            formStep.classList.remove("form-step-active");
    });

    formSteps[formStepsNum].classList.add("form-step-active");
}

function updateProgressbar() {
    progressSteps.forEach((progressStep, idx) => {
        if (idx < formStepsNum + 1) {
            progressStep.classList.add("progress-step-active");
        } else {
            progressStep.classList.remove("progress-step-active");
        }
    });

    const progressActive = document.querySelectorAll(".progress-step-active");

    progress.style.width =
        ((progressActive.length - 1) / (progressSteps.length - 1)) * 100 - 13 + "%";
}

// $('#tcNot').change(function() {
//     $('#pasLabel, #passportNo').toggle(this.checked);
//     // $('#tcLabel, #pasLabel').toggle(this.checked);
// });

$('#tcNot').change(function() {
    $('#tc, #tcLabel').toggle(!this.checked); // Checkbox seçili değilse (false) tcNo göster
    $('#pasLabel,#passportNo').toggle(this.checked) // Checkbox seçiliyse passportNo göster
});

$('#countryOut').change(function() {
    // $('#county, #city').disabled(!this.checked); // Checkbox seçili değilse (false) tcNo göster
    $('#county, #city').prop('disabled', this.checked); // Checkbox seçiliyse passportNo göster
});

$('#nextDriverCheck').click(function() {
    $('#tcCheck').val($('#tc').val());
});

$('#nextDriverCheck').click(function() {
    $('#driverCheck').val($('#DriverNo').val());
});

$('#nextDriverCheck').click(function() {
    $('#birthdayDateCheck').val($('#birthdayDate').val());
});

function setMaxDate() {
    const dateInput = document.getElementById("driverDate");
    dateInput.max = new Date().toISOString().split("T")[0];
}

function cc_format(value) {
    var v = value.replace(/\s+/g, '').replace(/[^0-9]/gi, '')
    var matches = v.match(/\d{4,16}/g);
    var match = matches && matches[0] || ''
    var parts = []
    for (i = 0, len = match.length; i < len; i += 4) {
        parts.push(match.substring(i, i + 4))
    }
    if (parts.length) {
        return parts.join(' ')
    } else {
        return value
    }
}

function formatCardNumber(input) {
    input.value = cc_format(input.value);
}

function setSelectedInsurance(isSelected) {
    document.getElementById('InsuranceSelect').value = isSelected;
}

const decreaseBtn = document.getElementById('decrease');
const increaseBtn = document.getElementById('increase');
const driverCountInput = document.getElementById('driverCount');
const pricePerDay = 100;

decreaseBtn.addEventListener('click', () => {
    let count = parseInt(driverCountInput.value);
    if (count > 0) {
        count--;
        driverCountInput.value = count;
        calculateTotalPrice()
    }
});

increaseBtn.addEventListener('click', () => {
    let count = parseInt(driverCountInput.value);
    count++;
    driverCountInput.value = count;
    calculateTotalPrice()
});

const totalPriceElement = document.querySelector('.card-text:last-child');

function calculateTotalPrice() {
    console.log('calculateTotalPrice function called'); // Fonksiyonun çağrıldığını kontrol et

    const totalDrivers = parseInt(driverCountInput.value);
    console.log('Total drivers:', totalDrivers); // Değişken değerlerini kontrol et

    const totalPrice = totalDrivers * pricePerDay;
    console.log('Total price:', totalPrice);

    totalPriceElement.textContent = `${totalPrice.toFixed(2)} TL / Gün`;
}

// Sayı değiştiğinde toplam fiyatı güncelle
driverCountInput.addEventListener('change', calculateTotalPrice);

setMaxDate();