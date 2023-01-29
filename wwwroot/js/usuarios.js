$(document).ready(function () {


});

function ver_detalle_usuario()
{
    let id_user = event.currentTarget.attributes[1].nodeValue;
    window.location.href = `Usuarios/Editar/${id_user}`;
}