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

    $("#btnPersonaContactoDetalle").on('click', function () {
        cargarTablepersonasDetalle(JSON.parse(localStorage.getItem("PersonaContacto")));
    });

    if(window.location.href.includes('Detalle'))
    {
        if($('#actividad_filter_detalle').val() !== null)
        {
            let input_iae = $("#input_iae_detalle");
            let input_cnae = $("#input_cnae_detalle");
            let value_select_actividad = $('#actividad_filter_detalle :selected')[0].text;
            input_iae.val(value_select_actividad.split(" - ")[0]);
            input_cnae.val(value_select_actividad.split(" - ")[1]);
        }

        if($("#InputPersonaContactoDetalle").val() !== "")
        {
            let jsonPersona = JSON.parse($("#InputPersonaContactoDetalle").val());
            $("#InputPersonaContactoDetalle").val(jsonPersona.NombrePersona);
            localStorage.setItem("PersonaContacto", JSON.stringify(jsonPersona));
        }

        PintarBodyTablePersona();

    }

    $('#enviar_doc_google').click(function (e) { 

        e.preventDefault();

        if(jQuery('#cargar_fichero').val() !== '')
        {
            let data = new FormData();
            jQuery.each(jQuery('#cargar_fichero')[0].files, function(i, file) {
                data.append('file-'+i, file);
            });

            $.ajax({
                type: "POST",
                url: `cargar_documento?codigoCliente=${$('#codCliente').val()}&tipoDoc=${$('#tiposDoc').val()}`,
                data: data,
                contentType: false,
                processData: false,
                success: function (data) {

                    if(data)
                    {
                        correcto("Documento subido correctamente");
                        $('#cargar_fichero').val('');
                        $("#popup_anadir_doc").modal('hide');
                        $(".btn_subir_doc_sin_datos").remove();
                        CargarDocumentosCliente($("#codCliente").val());
                    }
                    else
                        informacion("No se ha podido subir el documento");
                }

            });
        }
        else
            informacion("No se ha seleccionado ningún archivo"); 

    });  

    //correcto("Aún sin funcionalidad");

    //SignalIr
    //$.post("mensajeInstantaneo?mensaje=guardar", function (data) {

    //});

    //Ejemplo llamada a Gdrive
    // $.post("Clientes/GDriveModule", function () {

    // });

});

function cargaroptionsTiposDoc()
{
    $("#tiposDoc").empty();
    let options = "";
    $.ajax({
        url: "GetTiposDoc",
        type: "POST",
        async: false,
        success: function (data) {
            $(data).each(function (index, items) {
                options += "<option value='" + items.idTipoDocumento + "'>" + items.nombreTipoDocumento + "</option>";
            });
        }
    });

    $("#tiposDoc").append(options);
}

function cargarTablepersonas(JsonData)
{
    $("#bodyTable").empty();
    let contenido = "";
    $(JsonData).each(function (index, items) {
        
        contenido += "<tr class='tr'>";
        contenido += "<td class='bigword'>" + items.NombrePersona + "</td>";
        contenido += "<td class='bigword'>" + items.TelefonoPersona + "</td>";
        contenido += "<td>" + items.EmailPersona + "</td>";
        contenido += "<td class='col-md-1 tdPersona'><button type='button' onclick='seleccionPersona(" + `"${items.NombrePersona}"` + ")' class='boton_primario BtnSeleccionarPersona' data-dismiss='modal'><span>Seleccionar</span></button></td>";
        contenido += "<td class='col-md-1 tdPersona'><button onclick='quitarPersonaContacto(" + `"${items.TelefonoPersona}"` + ")' class='boton_primario BtnEliminarPersona'><span>X</span></button></td>";
        contenido += "</tr>";

    });

    $("#bodyTable").append(contenido);
}

function seleccionPersona(nombre)
{
    $("#InputPersonaContacto").val(nombre);
    //$("#btnCerrarModalPersona").click();
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
        
        $(data).each((index, items) => {

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

    if(localStorage.getItem("PersonaContacto") != null)
    {
        let persona = new Object()
        persona.IdPersonaContacto = 0;
        persona.Nombre = JSON.parse(localStorage.getItem("PersonaContacto"))[0].NombrePersona;
        persona.Telefono = JSON.parse(localStorage.getItem("PersonaContacto"))[0].TelefonoPersona;
        persona.Email = JSON.parse(localStorage.getItem("PersonaContacto"))[0].EmailPersona;
        persona.Clientes = null;

         $("#persona_contacto_crear").val(JSON.stringify(persona));
    }
    

    localStorage.removeItem("PersonaContacto");
    localStorage.removeItem("IdPersonaContacto");
    $("#form-new-client").submit();
    
    
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

$("#btnEmialsDetalle").on('click', function (evt) {
    evt.preventDefault();
    try
    {
        if($("#tableEmails").hasClass("hide"))
            PintarBodyTableEmail();
        else
            $("#tableEmails").addClass("hide");
    }
    catch (e) {
        console.log(e);
    }
    
});

function CambiarEmailCliente(idEmailCliente)
{
    $.post("CambiarEmailCliente?idEmailCliente=" + idEmailCliente, function (data) {
        PintarBodyTableEmail();
        $("#email_principal").val(data);
    });
    
}

function PintarBodyTableEmail()
{
    $.ajax({
        method: "GET",
        url: "ReturnEmailsCliente?codCliente=" + $("#codCliente").val(),
    }).done(function (data) {

        if (data.length > 0) {

            BodyTable = $("#bodyTableDetalleEmails");
            BodyTable.empty();
            let contenido = "";
            let MsgNodata = "No hay datos";
            
            $(data).each(function (index, items) {
                
                contenido += "<tr class='trEmailCliente'>";
                contenido += "<td class='tdEmailCliente'>" + (items.email1 == null ? MsgNodata : items.email1) + "</td>";
                contenido += "<td class='tdEmailCliente'>" + (items.activo == null ? MsgNodata : (items.activo == true ? "Si" : "No")) + "</td>";
                
                if(items.activo != true)
                    contenido += "<td class='col-md-2'> <input type='button' value='usar' onclick='CambiarEmailCliente(" + items.idEmailCliente + ")' class='boton_primario' /></td>";
                
                    contenido += "</tr>";

            });
            BodyTable.append(contenido);
            $("#tableEmails").removeClass("hide");

        }

    }).fail(function (data) {
        console.error(data);
    });
}

$("#btnCuentasDetalle").on('click', function (evt) {
    
    evt.preventDefault();
    try
    {
        if($("#tableCuentas").hasClass("hide"))
            PintarBodyTableCuentas();
        else
            $("#tableCuentas").addClass("hide");
    }
    catch (e) {
        console.log(e);
    }
    
});

function CambiarCuentaCliente(idCuentaCliente)
{
    $.post("CambiarCuentaCliente?idCuentaCliente=" + idCuentaCliente, function (data) {
        PintarBodyTableCuentas();
        $("#iban").val(data[0]);
        $("#bic").val(data[2]);
        $("#banco").val(data[1]);
    });
    
}

function PintarBodyTableCuentas()
{
    $.ajax({
        method: "GET",
        url: "ReturnCuentasCliente?codCliente=" + $("#codCliente").val(),
    }).done(function (data) {

        if (data.length > 0) {

            let BodyTable = $("#bodyTableDetalleCuentas");
            BodyTable.empty();
            let contenido = "";
            let MsgNodata = "No hay datos";
            
            $(data).each(function (index, items) {
                
                contenido += "<tr class='trEmailCliente'>";
                contenido += "<td class='tdEmailCliente'>" + (items.iban == null ? MsgNodata : items.iban) + "</td>";
                contenido += "<td class='tdEmailCliente'>" + (items.bic == null ? MsgNodata : items.bic) + "</td>";
                contenido += "<td class='tdEmailCliente'>" + (items.banco == null ? MsgNodata : items.banco) + "</td>";
                contenido += "<td class='tdEmailCliente'>" + (items.activa == null ? MsgNodata : (items.activa == true ? "Si" : "No")) + "</td>";
                
                if(items.activa != true)
                    contenido += "<td class='col-md-2'> <input type='button' value='usar' onclick='CambiarCuentaCliente(" + items.idCuenta + ")' class='boton_primario' /></td>";
                
                    contenido += "</tr>";

            });
            BodyTable.append(contenido);
            $("#tableCuentas").removeClass("hide");

        }

    }).fail(function (data) {
        console.error(data);
    });
}

function AddCuentaCliente(idCodigoCliente, iban, banco, bic)
{
    $.post("AddCuentaCliente?idCodigoCliente=" + idCodigoCliente + "&iban=" + iban + "&banco=" + banco + "&bic=" + bic, function (data) {
        PintarBodyTableCuentas();
    });
    
}

$("#btnAnadirCuentaDetalle").on('click', function (evt) {
    evt.preventDefault();
    try
    {
        informacionBoton("¿Guardar los cambios?").then((result) => {
            if (result.isConfirmed) 
            {
                AddCuentaCliente($("#codCliente").val(), $("#iban").val(), $("#banco").val(), $("#bic").val());
                Swal.fire('Guardado!', '', 'success');
            }
        });
    }
    catch (e) {
        console.log(e);
    }
    
});

function AddEmailCliente(idCodigoCliente, email)
{
    $.post("AddEmailCliente?idCodigoCliente=" + idCodigoCliente + "&email=" + email, function (data) {
        PintarBodyTableEmail();
    });
    
}

$("#btnAnadirEmailDetalle").on('click', function (evt) {
    evt.preventDefault();
    try
    {
        informacionBoton("¿Guardar los cambios?").then((result) => {
            if (result.isConfirmed) 
            {
                AddEmailCliente($("#codCliente").val(), $("#email_principal").val());
                Swal.fire('Guardado!', '', 'success');
            }
        });
        
    }
    catch (e) {
        console.log(e);
    }
    
});

$('#actividad_filter_detalle').on('select2:select', function (e) {
    let input_iae = $("#input_iae_detalle");
    let input_cnae = $("#input_cnae_detalle");
    let value_select_actividad = e.params.data;
    input_iae.val(value_select_actividad.text.split(" - ")[0]);
    input_cnae.val(value_select_actividad.text.split(" - ")[1]);
});

$("#btnPersonasDetalle").on('click', function (evt) {
    
    evt.preventDefault();
    try
    {
        if($("#tablePersonas").hasClass("hide"))
        {
            PintarBodyTablePersona();
            $("#tablePersonas").removeClass("hide");
        }
        else
            $("#tablePersonas").addClass("hide");
    }
    catch (e) {
        console.log(e);
    }
    
});

function PintarBodyTablePersona()
{
    $.ajax({
        method: "GET",
        url: "ReturnPersonasCliente?codCliente=" + $("#codCliente").val(),
    }).done(function (data) {

        if (data.length > 0) {

            let BodyTable = $("#bodyTableDetallePersonas");
            BodyTable.empty();
            let contenido = "";
            let MsgNodata = "No hay datos";
            
            $(data).each(function (index, items) {
                localStorage.setItem("IdPersonaContacto", items.idPersonaContacto);
                contenido += "<tr class='trEmailCliente'>";
                contenido += "<td class='tdEmailCliente'>" + (items.nombre == null ? MsgNodata : items.nombre) + "</td>";
                contenido += "<td class='tdEmailCliente'>" + (items.telefono == null ? MsgNodata : items.telefono) + "</td>";
                contenido += "<td class='tdEmailCliente'>" + (items.email == null ? MsgNodata : items.email) + "</td>";
                contenido += "</tr>";

            });
            BodyTable.append(contenido);

        }

    }).fail(function (data) {
        console.error(data);
    });
}

function CambiarPersonaContacto(persona)
{
    $.post("CambiarPersonaContacto?objetopersona=" + persona, function (data) {
        PintarBodyTablePersona();
        $("#btnCerrarModalPersonaDetalle").click();
    });
}

$("#anadirPersonaDetalle").on('click', function (evt) {
    evt.preventDefault();
    try
    {
        informacionBoton("¿Guardar los cambios?").then((result) => {
            if (result.isConfirmed) 
            {
                let persona = new Object()
                persona.IdPersonaContacto = localStorage.getItem("IdPersonaContacto");
                persona.Nombre = $("#nombrePersonaDetalle").val();
                persona.Telefono = $("#telefonoPersonaDetalle").val();
                persona.Email = $("#emailPersonaDetalle").val();
                persona.Clientes = null;

                CambiarPersonaContacto(JSON.stringify(persona));
                Swal.fire('Guardado!', '', 'success');
            }
        });
        
    }
    catch (e) {
        console.log(e);
    }
    
});


async function CargarDocumentosCliente(codCliente) {

    let responseData = await ExtraerDocumentosClientes(codCliente);
    CargarTablaDocumentos(responseData, "No hay datos");
    
}

function ExtraerDocumentosClientes(codCliente)
{ 
   return $.post("ObtenerDocumentosCliente?codCliente=" + codCliente, function (data) { });   
}

function CargarTablaDocumentos(data, msgdatanotfound)
{

    let tableId = document.getElementById("datatableDocumentosCliente_wrapper");

    if (data != null && data.length > 0)
    {
        let tableId = document.getElementById("datatableDocumentosCliente_wrapper");

        if (tableId)
            tableId.remove();

        let table = '<table class="table nowrap col-sm-12 col-md-10 col-lg-10 col-xl-10" id="datatableDocumentosCliente">';
        table += '<thead><tr class="tr"><th scope="col">Nombre documento</th><th scope="col">Url documento</th>';
        table += '<th scope="col">Tipo documento</th><th scope="col">Fecha subida</th><th scope="col"></th><th scope="col"></th></tr></thead>';
        table += '<tbody id="DocumentosClientes"></tbody></table>';    

        $("#table-documentos").append(table);
             
        BodyTable = $("#DocumentosClientes");
        let contenido = "";
        let MsgNodata = msgdatanotfound;
        
         $(data).each((index, items) => {
            
            contenido += "<tr class='tr'>";
            contenido += "<td class='bigword'>" + (items.nombreDocumento == null ? MsgNodata : items.nombreDocumento) + "</td>";
            contenido += "<td class='bigword'>" + (items.urlDocumento == null ? MsgNodata : items.urlDocumento) + "</td>";
            contenido += "<td>" + (items.nombreTipoDocumento == null ? MsgNodata : items.nombreTipoDocumento) + "</td>";
            contenido += "<td>" + (items.fechaSubida == null ? MsgNodata : new Date(items.fechaSubida).toLocaleDateString("es-ES")) + "</td>";

            contenido += `<td class='col-md-2'><a href='javascript:void(0)'  class='boton_primario' id='descarga_doc_${items.urlDocumento}' onclick='VerDocumento("${items.urlDocumento}");'>Ver documento</a></td>`;
            
            EsAdmin().then((result) => { 
                if(result)
                    contenido += `<td class='col-md-2'><a href='javascript:void(0)'  class='boton_primario' id='eliminar_doc_${items.urlDocumento}' onclick='EliminarDocumento("${items.urlDocumento}");'>Eliminar</a></td>`;
            })
            
            contenido += "</tr>";

        });

        IdiomaTabla("#datatableDocumentosCliente");
        BodyTable.append(contenido);
        $("#datatableDocumentosCliente_filter").empty();
        $("#datatableDocumentosCliente_filter").append("<input type='button' class='col-sm-12 col-md-2 col-lg-2 col-xl-2 ml-2' id='anadirDocumento' value='Añadir documento' onclick='AbrirModalAnadirDocumento();' />");
        $("#datatableDocumentosCliente_filter").append("<input type='text' class='col-sm-12 col-md-3 col-lg-3 col-xl-3 mr-2' id='busquedadocumentos' placeholder='Buscar...'><input class='col-sm-12 col-md-1 col-lg-1 col-xl-1' id='buscarButtondocumentos' type='button' value='Buscar' onclick='filtrarDocumentos("+JSON.stringify(data)+");'/>");
        $("#datatableDocumentosCliente_filter").removeClass('dataTables_filter');
        $("#datatableDocumentosCliente_filter").addClass('col-sm-12 col-md-12 col-lg-12 col-xl-12');

    }
    else
    {
        if (tableId)
            tableId.remove();

        $("#msg_noHaydatos").remove();
        $("#table-documentos").append("<p id='msg_noHaydatos' class='row'>No hay datos</p>");
        $("#table-documentos").append("<input type='button' class='col-sm-12 col-md-2 col-lg-2 col-xl-2 ml-2 btn_subir_doc_sin_datos' id='anadirDocumento' value='Añadir documento' onclick='AbrirModalAnadirDocumento();' />");
    }
        
    
}

filtrarDocumentos = (data) => {
    
    let responseData = data;

    if ($("#busquedadocumentos").val() !== "") {

        let busqueda = $("#busquedadocumentos").val();
        let expresion = new RegExp(`${busqueda}.*`, "i");

        data = data.filter(
            x => expresion.test(x.nombreDocumento)
                || expresion.test(x.urlDocumento)
                || expresion.test(x.nombreTipoDocumento)
                || expresion.test(x.fechaSubida)
        );

        if(data.length === 0)
            CargarTablaDocumentos(responseData, "No hay datos");
        else
            CargarTablaDocumentos(data, "No hay datos");

    }
    else
        CargarDocumentosCliente($("#codCliente").val());

    

};

function VerDocumento(urlArchivo)
{
    urlArchivoConcatenado = `https://drive.google.com/file/d/${urlArchivo}/view`;
    $('#descarga_doc_' + urlArchivo).attr({target: '_blank', href : urlArchivoConcatenado});
}

function EliminarDocumento(urlArchivo)
{

    $.post("EliminarDocumento", { fileId: urlArchivo }, function (data) {
        
        if (data)
        {
            correcto("Documento eliminado correctamente");
            CargarDocumentosCliente($("#codCliente").val());
        }
        else
        {
            incorrecto("No se ha podido eliminar el documento");
        }
    });
}

function AbrirModalAnadirDocumento()
{
    $("#popup_anadir_doc").modal('show');
    cargaroptionsTiposDoc();
}

function EsAdmin()
{
    return $.post("EsAdmin", function (data) { });  
}

/*detalle*/
