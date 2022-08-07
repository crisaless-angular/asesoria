﻿$(".todosclientes").on('click', function () {
    window.location.href = "/Clientes";
});

$(".crearcliente").on('click', function () {
    window.location.href = "/Clientes/Crear";
});

function CargarClientes(msgdatanotfound) {

    $.post("Clientes/ObtenerClientesIndex/", function (data) {

        let BodyTable = document.getElementById("IndexdatosClientes");
        let contenido = "";
        let MsgNodata = msgdatanotfound;

        $(data).each(function (index, items) {
            debugger;
            contenido += "<tr class='tr'>";
            contenido += "<td>" + (items.codigO_CONTABILIDAD == null ? MsgNodata : items.codigO_CONTABILIDAD) + "</td>";
            contenido += "<td class='bigword'>" + (items.nombrE_FISCAL == null ? MsgNodata : items.nombrE_FISCAL) + "</td>";
            contenido += "<td class='bigword'>" + (items.nombrE_COMERCIAL == null ? MsgNodata : items.nombrE_COMERCIAL) + "</td>";
            contenido += "<td>" + (items.movil == null ? MsgNodata : items.movil) + "</td>";
            contenido += "<td class='bigword'>" + (items.emailprincipal == null ? MsgNodata : items.emailprincipal) + "</td>";
            contenido += "<td>" + (items.identificacioN_FISCAL == null ? MsgNodata : items.identificacioN_FISCAL) + "</td>";
            contenido += "<td>" + (items.agente == null ? MsgNodata : items.agente) + "</td>";
            contenido += "<td>" + (items.tipO_CLIENTE == null ? MsgNodata : items.tipO_CLIENTE) + "</td>";
            
            contenido += "<td class='col-md-2'><button onclick='Detalle(" + items.codigO_CONTABILIDAD +")' class='boton_primario'><span>Detalle</span></button></td>";
            contenido += "</tr>";

        });

        BodyTable.innerHTML = contenido;
        IdiomaTabla("#datatableIndexClientes");
        $('#datatableIndexClientes').DataTable();

    });

}

function Detalle(element)
{
    //console.log(element);
    correcto("Aún sin funcionalidad");
}

function enviarDatos() {
    debugger;
    let input = document.getElementById("inputExcelClientes");

    let formData = new FormData();

    formData.append("ArchivoExcel", input.files[0]);

    let url = "/Clientes/RecibirExcel/";

    try {

        $.ajax({
            method: "POST",
            url: url,
            async: true,
            data: formData,
            processData: false,
            contentType: false
        }).done(function (data) {

        }).fail(function (data) {
            console.error(data);
        });

    }
    catch (e) {
        console.log(e);
    }

}

$("#save-new-cliente").on('click', function () {
    $("#form-new-client").submit();
});

$('#volver_arriba').click(function () {
        $('body, html').animate({
            scrollTop: '0px'
        }, 300);
    });

$(window).scroll(function () {
    if ($(this).scrollTop() > 0) {
        $('.ir-arriba').slideDown(300);
    } else {
        $('.ir-arriba').slideUp(300);
    }
});


