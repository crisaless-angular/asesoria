$(".todosclientes").on('click', function () {
    window.location.href = "/Clientes";
});

$(".crearcliente").on('click', function () {
    window.location.href = "/Clientes/Crear";
});

$("#OrderAscIndex").on('click', function () {
    Filtrar("nombrE_COMPLETO", true)
});

$("#OrderDescIndex").on('click', function () {
    Filtrar("nombrE_COMPLETO", false)
});

async function Filtrar(campo, asc)
{
    let responseData = await ExtraerDatosClientes();
    
    //responseData = responseData.filter(function (i, campo) {
    //    return campo.nombrE_COMPLETO === 'google';
    //});

    if (asc) {

        responseData = responseData.sort(function (a, b) {
            return (a["nombrE_COMPLETO"] > b["nombrE_COMPLETO"]) ? 1 : ((a["nombrE_COMPLETO"] < b["nombrE_COMPLETO"]) ? -1 : 0);
        });

    } else {

        responseData = responseData.sort(function (a, b) {
            return (b["nombrE_COMPLETO"] > a["nombrE_COMPLETO"]) ? 1 : ((b["nombrE_COMPLETO"] < a["nombrE_COMPLETO"]) ? -1 : 0);
        });

    }
    
    CargarTablaIndex(responseData, "No hay datos");
}

async function CargarClientes() {

    let responseData = await ExtraerDatosClientes();
    CargarTablaIndex(responseData, "No hay datos");
    
}

function ExtraerDatosClientes()
{ 
   return $.post("Clientes/ObtenerClientesIndex/", function (data) { });   
}

function CargarTablaIndex(data, msgdatanotfound)
{
    
    if (data != null)
    {
        let tableId = document.getElementById("datatableIndexClientes_wrapper");

        if (tableId)
            tableId.remove();

        let table = '<table class="table nowrap col-sm-12 col-md-10 col-lg-10 col-xl-10" id="datatableIndexClientes">';
        table += '<thead><tr class="tr"><th scope="col">Nombre</th><th scope="col">Fecha contratación TH</th>';
        table += '<th scope="col">Móvil</th><th scope="col">E-mail</th><th scope="col">N.I.F.</th><th scope="col">Agente</th>';
        table += '<th scope="col">Tipo de Cliente</th><th scope="col"></th></tr></thead>';
        table += '<tbody id="IndexdatosClientes"></tbody></table>';    

        $("#CuerpoIndex").append(table);
             
        BodyTable = $("#IndexdatosClientes");
        let contenido = "";
        let MsgNodata = msgdatanotfound;
        
        $(data).each(function (index, items) {
            
            contenido += "<tr class='tr'>";
            contenido += "<td class='bigword'>" + (items.nombrE_COMPLETO == null ? MsgNodata : items.nombrE_COMPLETO) + "</td>";
            contenido += "<td class='bigword'>" + (items.fechA_CONTRATACION_TH == null ? MsgNodata : items.fechA_CONTRATACION_TH) + "</td>";
            contenido += "<td>" + (items.movil == null ? MsgNodata : items.movil) + "</td>";
            contenido += "<td class='bigword'>" + (items.emailprincipal == null ? MsgNodata : items.emailprincipal) + "</td>";
            contenido += "<td>" + (items.identificacioN_FISCAL == null ? MsgNodata : items.identificacioN_FISCAL) + "</td>";
            contenido += "<td>" + (items.agente == null ? MsgNodata : items.agente) + "</td>";
            contenido += "<td>" + (items.tipO_CLIENTE == null ? MsgNodata : items.tipO_CLIENTE) + "</td>";

            contenido += "<td class='col-md-2'><button onclick='Detalle(" + items.codigO_CONTABILIDAD + ")' class='boton_primario'><span>Detalle</span></button></td>";
            contenido += "</tr>";

        });

        IdiomaTabla("#datatableIndexClientes");
        BodyTable.append(contenido);

    }
    
}

function Detalle(element)
{
    window.location = "/Clientes/Detalle?codigoContabilidad=" + element;
    /*correcto("Aún sin funcionalidad");*/
}

function enviarDatos() {
    
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
    correcto("Aún sin funcionalidad");
    //$("#form-new-client").submit();

    $.post("mensajeInstantaneo?mensaje=guardar", function (data) {

    });

    //Ejemplo llamada a Gdrive
    //$.post("GDriveModule", function () {

    //});

    
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


$("#CodigoPostal").on("keypress", function myfunction(e) {

    if (soloNumeros(e))
        e.preventDefault();
    
});

$('#identificacion_fiscal').keyup(function () {
    this.value = UpperCase(this.value);
});

$('#poblacion').keyup(function () {
    this.value = UpperCase(this.value);
});

$('#provincia').keyup(function () {
    this.value = UpperCase(this.value);
});

$('#email_principal').keyup(function () {
    this.value = LowerCase(this.value);
});

$('#iban').keyup(function () {
    this.value = UpperCase(this.value);
});

$('#banco').keyup(function () {
    this.value = UpperCase(this.value);
});

/*detalle*/

/*detalle*/


