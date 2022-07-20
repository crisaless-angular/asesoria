function soloNumeros(e) {
    let key = window.event ? e.which : e.keyCode;
    if (key < 48 || key > 57) {
        return true;
    }
}

function ValidateIBAN(campo) {

    let IBAN = campo;

    //Se pasa a Mayusculas
    IBAN = IBAN.toUpperCase();
    //Se quita los blancos de principio y final.
    IBAN = IBAN.trim();
    IBAN = IBAN.replace(/\s/g, ""); //Y se quita los espacios en blanco dentro de la cadena

    let letra1, letra2, num1, num2;
    let isbanaux;
    let numeroSustitucion;
    //La longitud debe ser siempre de 24 caracteres
    if (IBAN.length != 24) {
        return false;
    }

    // Se coge las primeras dos letras y se pasan a números
    letra1 = IBAN.substring(0, 1);
    letra2 = IBAN.substring(1, 2);
    num1 = getnumIBAN(letra1);
    num2 = getnumIBAN(letra2);
    //Se sustituye las letras por números.
    isbanaux = String(num1) + String(num2) + IBAN.substring(2);
    // Se mueve los 6 primeros caracteres al final de la cadena.
    isbanaux = isbanaux.substring(6) + isbanaux.substring(0, 6);

    //Se calcula el resto, llamando a la función modulo97, definida más abajo
    resto = modulo97(isbanaux);
    if (resto == 1) {
        return true;
    } else {
        return false;
    }
}

function modulo97(iban) {
    let parts = Math.ceil(iban.length / 7);
    let remainer = "";

    for (let i = 1; i <= parts; i++) {
        remainer = String(parseFloat(remainer + iban.substr((i - 1) * 7, 7)) % 97);
    }

    return remainer;
}

function getnumIBAN(letra) {
    ls_letras = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    return ls_letras.search(letra) + 10;
}


function actualizarFactura() {

    let tabla = $('.datatable').DataTable();
    let data = tabla.row($('.btn-actualizarFactura').parent().data()).data();
    let Nfactura = data[0];

    window.location.href = "/Facturas/VerEditar?NFactura=" + Nfactura;

}




function verFactura(Nfactura) {

    let iframe = "";

    $.ajax({
        type: "POST",
        url: '/Facturas/VerPdf?Nfactura=' + Nfactura,
        beforeSend: function (xhr) {
            
        },
        data: null,
        success: function (response) {
            
            if ($('#visor'))
                $('#visor').remove();
            
            iframe = `<iframe id='visor' src='${response}' style='width:100%; height:700px;' frameborder='0'></iframe>`;
            $('.contentIndexmodal').append(iframe);
            $('#modalIndex').modal('toggle');
           
        },
        failure: function (response) {
            console.log(response);
        }
    });
    

}

function GenerarFactura(IdFactura, iva, formapago, fechaemision, generada) {

    $("#crearFactura").find('button').each(function () {
        $(this).attr('disabled', 'disabled');
    });
    $("#crearFactura").find('select').each(function () {
        $(this).attr('disabled', 'disabled');
    });
    $("#crearFactura").find('input').each(function () {
        $(this).attr('disabled', 'disabled');
    });

    if (!generada) {
        $.get("GenerarFacturaPdf/?Idfactura=" + IdFactura + "&iva=" + iva + "&TipoFormaPago=" + formapago + "&fechaemision=" + fechaemision, function (result) {
            window.location.href = "/Facturas";
            window.open("/Facturas/GenerarFactura/?Idfactura=" + IdFactura);
        });
    } else {
        window.open("/Facturas/GenerarFactura/?Idfactura=" + IdFactura);
    }
   
}

function GenerarFacturaEditada(IdFactura, iva, formapago, Pagada) {

    $.get("GenerarFacturaPdfEditada/?Idfactura=" + IdFactura + "&iva=" + iva + "&TipoFormaPago=" + formapago + "&pagada=" + Pagada, function (result) {
        window.open("/Facturas/GenerarFactura/?Idfactura=" + IdFactura);
    });

}


//correcto('Cliente asociado correctamente a la factura.');


function ObtenerNumeroFactura() {
    let tabla = $('.datatable').DataTable();
    let data = tabla.row($('.btn-ObtenerIdCliente').parent().data()).data();
    return data[0];
}

$(document).ready(function () {

    $("[name='CrearFactura']").on('click', function () {
        if ($('.idcliente').val() === null) {
            alert('Debe seleccionar un cliente');
        }
    });

    $('.datatable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected'))
        {
            $(this).removeClass('selected');
        } else {
            tabla.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#GenerarFactura').on('click', function () {

        Swal.fire({
            title: 'Generar la factrua',
            text: 'Se va a generar la factrua, ¿Los datos son correctos?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Generar'
        }).then((result) => {
            if (result.isConfirmed) {
                GenerarFactura($('#IdFactura').val(), $('#ivaselect').val(), $('#formapagoselect').val(), $('#fechaEmision').val(), false);
            }
        });

    });


});

function IdiomaTabla(clase) {
    
    $(clase).DataTable({
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        responsive: true,
    });
}

function empty(element) {
    while (element.firstElementChild) {
        element.firstElementChild.remove();
    }
}

function correcto(text) {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: text,
        showConfirmButton: false,
        timer: 1500
    })
}

function PuntoPorComa(valor) {
    return valor.replace('.', ',');
}

function UpperCase(valor) {
    return valor.toUpperCase();
}





