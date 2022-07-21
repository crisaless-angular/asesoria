$(".todosclientes").on('click', function () {
    window.location.href = "/Clientes"
});

function CargarClientes(msgdatanotfound)
{

    $.post("Clientes/ObtenerClientesIndex/", function (data) {

        let BodyTable = document.getElementById("IndexdatosClientes");
        let contenido = "";
        let MsgNodata = msgdatanotfound;

        $(data).each(function (index, items) {

            contenido += "<tr>";
            contenido += "<td>" + (items.codigO_CONTABILIDAD == null ? MsgNodata : items.codigO_CONTABILIDAD) + "</td>";
            contenido += "<td>" + (items.nombrE_FISCAL == null ? MsgNodata : items.nombrE_FISCAL) + "</td>";
            contenido += "<td>" + (items.nombrE_COMERCIAL == null ? MsgNodata : items.nombrE_COMERCIAL) + "</td>";
            contenido += "<td>" + (items.telefono == null ? MsgNodata : items.telefono) + "</td>";
            contenido += "<td>" + (items.fax == null ? MsgNodata : items.fax) + "</td>";
            contenido += "<td>" + (items.movil == null ? MsgNodata : items.movil) + "</td>";
            contenido += "<td>" + (items.emailprincipal == null ? MsgNodata : items.emailprincipal) + "</td>";
            contenido += "<td>" + (items.identificacioN_FISCAL == null ? MsgNodata : items.identificacioN_FISCAL) + "</td>";
            contenido += "<td class='col-md-2'><button class='btn btn-info'>Detalle <span style='font-size: 20px !important';>>>></span></button></td>";
            contenido += "</tr>";

        });

        BodyTable.innerHTML = contenido;
        IdiomaTabla("#datatableIndexClientes");
        $('#datatableIndexClientes').DataTable();

    });

    
}