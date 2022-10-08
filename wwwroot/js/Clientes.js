$(".todosclientes").on('click', function () {
    window.location.href = "/Clientes";
});

$(".crearcliente").on('click', function () {
    window.location.href = "/Clientes/Crear";
});

$("#OrderAscIndex").on('click', function () {
    Filtrar();
});

$("#OrderDescIndex").on('click', function () {
    Filtrar();
});

$("#checkAutonomos").on('click', function () {
    $("#checkEmpresa").prop('checked', false); 
    Filtrar();
});

$("#checkEmpresa").on('click', function () {
    $("#checkAutonomos").prop('checked', false); 
    Filtrar();
});

$(".selectindex").on('change', function () {

    if (this.value !== '0') {
        $("#datatableIndexClientes_filter").css("display", "block");
        Filtrar();
    }
    else 
        $("#datatableIndexClientes_filter").css("display", "none");

});

//aQUI NO ENTRA
$("#datatableIndexClientes_filter input").on('keyup', function () {
    Filtrar();
});

async function Filtrar()
{
    let responseData = await ExtraerDatosClientes();

    responseData = FiltrarCampo(responseData);
    responseData = Filtarorden(responseData, $(".selectindex option:selected").text());
   
    CargarTablaIndex(responseData, "No hay datos");
}

function FiltrarCampo(responseData)
{
   
    let autonomo = "#checkAutonomos";
    let empresa = "#checkEmpresa";
    let campo = "#datatableIndexClientes_filter"
    let valor = "";

    if ($(autonomo).prop("checked"))
        valor = "Autónomo";
    else if ($(empresa).prop("checked")) 
        valor = "Empresa";


    if ($(campo).val() !== "") {
        responseData = responseData.filter(function (filtro, i) {
            return filtro.tipO_CLIENTE === campo;
        });
    }

    if (valor != "")
    {
        responseData = responseData.filter(function (filtro, i) {
            return filtro.tipO_CLIENTE === valor;
        });
    }
 
    return responseData;

}

function Filtarorden(responseData, campo)
{

    switch (campo) {
        case "Nombre":
            campo = "nombrE_COMPLETO";
            break;
        case "Contratación TH":
            campo = "fechA_CONTRATACION_TH";
            break;
        case "Móvil":
            campo = "movil";
            break;
        case "E-mail":
            campo = "emailprincipal";
            break;
        case "N.I.F":
            campo = "identificacioN_FISCAL";
            break;
        case "Agente":
            campo = "agente";
            break;
        case "Tipo cliente":
            campo = "tipO_CLIENTE";
            break;
    }
    
    let filter = "nombrE_COMPLETO";
    if (campo !== "Selección")
        filter = campo;
    
    if ($('#OrderDescIndex').is(':checked')) {

        responseData = responseData.sort(function (a, b) {
            return (b[filter] > a[filter]) ? 1 : ((b[filter] < a[filter]) ? -1 : 0);
        });

    } else {
        responseData = responseData.sort(function (a, b) {
            return (a[filter] > b[filter]) ? 1 : ((a[filter] < b[filter]) ? -1 : 0);
        });
    }

    return responseData;
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
    console.log(data);
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

        $("#table-index").append(table);
             
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

        if ($(".selectindex").val() !== '0')
            $("#datatableIndexClientes_filter").css("display", "block");
        else
            $("#datatableIndexClientes_filter").css("display", "none");

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


