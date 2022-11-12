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
        Filtrar();
    }
      
});

$(document).ready(function () {

    localStorage.removeItem("PersonaContacto");

    let CampoBuscar = "<p id='bloqueBuscarIndex'><label> Buscar: <input type='text' id='BuscarIndex'/></label><button id='BtnBuscarIndex' class='boton_primario'><span>Buscar</span></button></p>";
    $("#table-index").prepend(CampoBuscar);

    $("#BtnBuscarIndex").hide();

    $("#BuscarIndex").keyup(function () {
        $("#BtnBuscarIndex").show();
    });  

    $("#BtnBuscarIndex").on('click', function () {
        Filtrar();
    });

    $("#NuevoTipoCliente").hide();

    $("#btnTipoClienteInput").on('click', function () {

        if ($("#TipoCliente").hasClass("mostrar"))
        {
            $("#TipoCliente").removeClass("mostrar");
            $("#TipoCliente").hide();
            $("#NuevoTipoCliente").show();
        } else
        {
            $("#TipoCliente").addClass("mostrar");
            $("#NuevoTipoCliente").hide();
            $("#TipoCliente").show();
        }
        
    });

    $("#anadirPersona").on('click', function () {

        if ($("#nombrePersona").val() !== "" &&
            $("#telefonoPersona").val() !== "" &&
            $("#emailPersona").val() !== ""
        ) {
            let persona = [];

            persona.push({
                "NombrePersona": $("#nombrePersona").val(),
                "TelefonoPersona": $("#telefonoPersona").val(),
                "EmailPersona": $("#emailPersona").val()
            });

            if (localStorage.getItem("PersonaContacto") !== null) {
                let jsonPersona = JSON.parse(localStorage.getItem("PersonaContacto"));
                jsonPersona.push({
                    "NombrePersona": $("#nombrePersona").val(),
                    "TelefonoPersona": $("#telefonoPersona").val(),
                    "EmailPersona": $("#emailPersona").val()
                });

                localStorage.setItem("PersonaContacto", JSON.stringify(jsonPersona));

            }
            else
            {
                localStorage.setItem("PersonaContacto", JSON.stringify(persona));
            }

            cargarTablepersonas(JSON.parse(localStorage.getItem("PersonaContacto")));

            $("#MesajeError>p").remove();
            $("#nombrePersona").val("");
            $("#telefonoPersona").val("");
            $("#emailPersona").val("");
                
        }
        else {
            $("#MesajeError>p").remove();
            $("#MesajeError").append("<p>Todos los campos son obligatorios</p>");
        }

    });

    $("#btnPersonaContacto").on('click', function () {
        cargarTablepersonas(JSON.parse(localStorage.getItem("PersonaContacto")));
    });

});

function cargarTablepersonas(JsonData)
{
    $("#bodyTable").empty();
    let contenido = "";
    $(JsonData).each(function (index, items) {
        
        contenido += "<tr class='tr'>";
        contenido += "<td class='bigword'>" + items.NombrePersona + "</td>";
        contenido += "<td class='bigword'>" + items.TelefonoPersona + "</td>";
        contenido += "<td>" + items.EmailPersona + "</td>";
        contenido += "<td class='col-md-1 tdPersona'><button type='button' onclick='seleccionPersona(" + `"${items.NombrePersona}"` + ")' class='boton_primario BtnSeleccionarPersona'><span>Seleccionar</span></button></td>";
        contenido += "<td class='col-md-1 tdPersona'><button onclick='quitarPersonaContacto(" + `"${items.TelefonoPersona}"` + ")' class='boton_primario BtnEliminarPersona'><span>X</span></button></td>";
        contenido += "</tr>";

    });

    $("#bodyTable").append(contenido);
}

function seleccionPersona(nombre)
{
    $("#InputPersonaContacto").val(nombre);
    $("#btnCerrarModalPersona").click();
}

function quitarPersonaContacto(telefono)
{
    let jsonPersona = JSON.parse(localStorage.getItem("PersonaContacto"));

    jsonPersona.forEach(function (currentValue, index, arr) {
        if (jsonPersona[index].TelefonoPersona == telefono) {
            jsonPersona.splice(index, index);
        }
    })

    localStorage.setItem("PersonaContacto", JSON.stringify(jsonPersona));
    cargarTablepersonas(JSON.parse(localStorage.getItem("PersonaContacto")));

}


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
    let campo = "#BuscarIndex"
    let valor = "";

    if ($(autonomo).prop("checked"))
        valor = "Autónomo";
    else if ($(empresa).prop("checked")) 
        valor = "Empresa";


    if ($(campo).val() !== "") {

        let busqueda = $(campo).val();
        let expresion = new RegExp(`${busqueda}.*`, "i");

        if ($(".selectindex").val() === "0") {
            responseData = responseData.filter(
                x => expresion.test(x.tipO_CLIENTE)
                    || expresion.test(x.nombrE_COMPLETO)
                    || expresion.test(x.fechA_CONTRATACION_TH)
                    || expresion.test(x.emailprincipal)
                    || expresion.test(x.movil)
                    || expresion.test(x.emailprincipal)
                    || expresion.test(x.identificacioN_FISCAL)
                    || expresion.test(x.agente)

            );
        } else
        {
            let CampoSeleccionado = ReturnCampoBd($(".selectindex option:selected").text());
            responseData = responseData.filter(x => expresion.test(eval("x." + CampoSeleccionado)));
        }

       
        
    }

    if (valor != "")
    {
        responseData = responseData.filter(function (filtro, i) {
            return filtro.tipO_CLIENTE === valor;
        });
    }
 
    return responseData;

}

function ReturnCampoBd(campo)
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

    return campo;

}

function Filtarorden(responseData, campo)
{

    campo = ReturnCampoBd(campo);
    
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
            contenido += "<td class='bigword'>" + (items.fechA_CONTRATACION_TH == null ? MsgNodata : new Date(items.fechA_CONTRATACION_TH).toLocaleDateString("es-ES")) + "</td>";
            contenido += "<td>" + (items.movil == null ? MsgNodata : items.movil) + "</td>";
            contenido += "<td class='bigword'>" + (items.emailprincipal == null ? MsgNodata : items.emailprincipal) + "</td>";
            contenido += "<td>" + (items.identificacioN_FISCAL == null ? MsgNodata : items.identificacioN_FISCAL) + "</td>";
            contenido += "<td>" + (items.agente == null ? MsgNodata : items.agente) + "</td>";
            contenido += "<td>" + (items.tipO_CLIENTE == null ? MsgNodata : items.tipO_CLIENTE) + "</td>";

            contenido += "<td class='col-md-2'><button onclick='Detalle(" + items.codigO_CLIENTE + ")' class='boton_primario'><span>Detalle</span></button></td>";
            contenido += "</tr>";

        });

        IdiomaTabla("#datatableIndexClientes");
        BodyTable.append(contenido);

    }
    
}

function Detalle(element)
{
    window.location = "/Clientes/Detalle?CodigoCliente=" + element;
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
    

    localStorage.removeItem("PersonaContacto");
    $("#form-new-client").submit();
    
    //correcto("Aún sin funcionalidad");

    //SignalIr
    //$.post("mensajeInstantaneo?mensaje=guardar", function (data) {

    //});

    //Ejemplo llamada a Gdrive
    //$.post("GDriveModule", function () {

    //});

    
});

/* Making the button scroll to the top of the page. */
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

$('.selectFilter').on('select2:select', function (e) {
    let input_iae = $("#input_iae");
    let input_cnae = $("#input_cnae");
    let value_select_actividad = e.params.data;
    input_iae.val(value_select_actividad.text.split(" - ")[0]);
    input_cnae.val(value_select_actividad.text.split(" - ")[1]);
  });

/*detalle*/
$("#update-new-cliente").on('click', function () {
    $("#form-update-client").submit();
});

$("#btnEmialsDetalle").on('click', function () {

    if($("#tableEmails").hasClass("hide"))
        $("#tableEmails").removeClass("hide");
    else
        $("#tableEmails").addClass("hide");
        
});

/*detalle*/


