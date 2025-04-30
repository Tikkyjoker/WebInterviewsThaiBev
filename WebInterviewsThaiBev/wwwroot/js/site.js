$(document).ready(function() {
    var $ = window.$;
    const patten = {
        phone: /^0[689]\d{8}$/,
        email: /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    };
    $("#form1").find("input,select").filter((i, e) => e.id != "phone").on("change",
        (e) => {
            if (e.target.value != "" || e.target.checked)
                $(e.target).closest("div.col-md-6").children().last().hide();
            else
                $(e.target).closest("div.col-md-6").children().last().show();
        })
    $("#form1").on("submit",
        function(e) {
            e.preventDefault();
            const formData = new FormData(this);
            let isValid = true;

            if (!formData.has("Sex")) {
                formData.append("Sex", "");
            }
            [...formData].forEach(e => {
                if (e[1] == "" ||
                    (typeof e[1] == "object" && e[1].name == "" || $(`#${e[0]}Error`).css("display") != "none")) {
                    $(`#${e[0]}Error`).show();
                    isValid = false;
                } else
                    $(`#${e[0]}Error`).hide();
            });
            if (!isValid) return;
            $.ajax({
                url: "/Home/SubmitForm",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function(response) {
                    $("#alertBox").find("#rowid").text(response.id).end().show().fadeTo(4000, 500)
                        .slideUp(500, function() { $("#alertBox").slideUp(500); });
                    $("#clear").click();
                },
                error: function(xhr, status, error) {
                    alert("Error: " + error);
                }
            });
        });
    $("input[name='Phone']").keydown((ev) => {
        if (!(isFinite(ev.key) || ev.keyCode < 65 || ev.keyCode > 90)) {
            ev.preventDefault();
        }
    });
    $("input[name='Phone']").on("input",
        function(e) {
            if (e.target.value.length >= 10 && patten.phone.test(e.target.value.substr(0, 10)))
                $("#PhoneError").hide().text("Please provide a valid Phone");
            else
                $("#PhoneError").show()
                    .text(e.target.value.length === 0 ? "Please provide a valid Phone" : "Invalid mobile number");
        });
    $("input[name='Email']").on("input",
        function(e) {
            const email = e.target.value;
            if (patten.email.test(email)) {
                $("#EmailError").hide().text("Please provide a valid Email");
            } else {
                $("#EmailError").show()
                    .text(e.target.value.length === 0 ? "Please provide a valid Email" : "Invalid email format").show();
            }
        });
});