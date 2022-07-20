function Getlogotipo()
{
    $.get("../../Public/GetLogotipo/", function (data) {
        document.getElementById("logotipo").src = `${window.location.origin}/images/${data}`;
    });
}