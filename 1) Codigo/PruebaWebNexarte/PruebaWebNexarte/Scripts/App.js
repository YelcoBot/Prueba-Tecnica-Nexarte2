$(document).ready(function () {

    var TableNomina = $('#TableNomina').DataTable();

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    });

    $(document).on("submit", "#FormBusqueda", function () {
        swalWithBootstrapButtons.fire({
            title: 'Usuario',
            html: '<input id="swal-busq-usuario" class="swal2-input">',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Consultar!',
            cancelButtonText: 'No, cancelar!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                let usuario = $("#swal-busq-usuario").val();
                let documento = $("#busq-documento").val();
                if (usuario.length === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Ingrese un usuario!'
                    });
                } else {

                    TableNomina.ajax.url("/Home/Search?documento=" + documento + "&usuario=" + usuario).load();
                    TableNomina.draw();

                }
            }
        });
    });

    $(document).on("click", "#TableNomina #btn-remove", function () {

        let documento = $(this).attr("documento");
        let registro = $(this).attr("registro");

        swalWithBootstrapButtons.fire({
            title: '¿Estas segur@?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, eliminarlo!',
            cancelButtonText: 'No, cancelar!'
        }).then((resultDelete) => {
            if (resultDelete.isConfirmed) {
                swalWithBootstrapButtons.fire({
                    title: 'Usuario',
                    html: '<input id="swal-delete-usuario" class="swal2-input">',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Eliminar!',
                    cancelButtonText: 'No, cancelar!',
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        let usuario = $("#swal-delete-usuario").val();

                        if (usuario.length === 0) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Ingrese un usuario!'
                            });
                        } else {

                            $.ajax({
                                url: "/Home/Delete",
                                dataType: "json",
                                type: "POST",
                                data: { Documento: documento, Registro: registro, Usuario: usuario },
                                success: function (data) {
                                    console.log("Correcto:");
                                    console.log(data);
                                    if (data.Tipo == "OK") {
                                        Swal.fire({
                                            position: 'top-end',
                                            icon: 'success',
                                            title: data.Mensaje,
                                            showConfirmButton: false,
                                            timer: 1500
                                        });

                                        TableNomina.ajax.reload();
                                    } else {
                                        Swal.fire({
                                            icon: 'error',
                                            title: 'Oops...',
                                            text: data.Mensaje
                                        })
                                    }
                                },
                                error: function (data) {
                                    console.log("Error:");
                                    console.log(data);
                                },
                            });
                        }
                    }
                });
            }
        });
    });

    $(document).on("click", "#TableNomina #btn-edit", function () {

        let documento = $(this).attr("documento");
        let registro = $(this).attr("registro");

        $.ajax({
            url: "/Home/Get?Registro=" + registro,
            dataType: "json",
            type: "GET",
            success: function (data) {
                console.log("Correcto:");
                console.log(data);
                if (data.Tipo == "OK") {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: data.Mensaje,
                        showConfirmButton: false,
                        timer: 1500
                    }).then((resultGet) => {
                        $("#ModalEditar #Documento").val(documento).trigger("change");
                        $("#ModalEditar #Registro").val(data.Data.Registro).prop("disabled", true).trigger("change");
                        $("#ModalEditar #IdEmpleado").val(data.Data.IdEmpleado).prop("disabled", true).trigger("change");
                        $("#ModalEditar #IdConcepto").val(data.Data.IdConcepto).trigger("change");
                        $("#ModalEditar #ValorNomina").val(data.Data.ValorNomina).trigger("change");
                        $("#ModalEditar #Cantidad").val(data.Data.Cantidad).trigger("change");
                        $("#ModalEditar").modal("show");
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data.Mensaje
                    })
                }
            },
            error: function (data) {
                console.log("Error:");
                console.log(data);
            },
        });
    });

    $(document).on("submit", "#FormEditar", function () {

        $("#ModalEditar").modal("hide");

        swalWithBootstrapButtons.fire({
            title: 'Usuario',
            html: '<input id="swal-edit-usuario" class="swal2-input">',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Editar!',
            cancelButtonText: 'No, cancelar!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                let usuario = $("#swal-edit-usuario").val();
                let documento = $("#FormEditar #Documento").val();
                let registro = $("#FormEditar #Registro").val();
                let idConcepto = $("#FormEditar #IdConcepto").val();
                let valorNomina = $("#FormEditar #ValorNomina").val();
                let cantidad = $("#FormEditar #Cantidad").val();
                if (usuario.length === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Ingrese un usuario!'
                    });
                } else {

                    $.ajax({
                        url: "/Home/Update",
                        dataType: "json",
                        type: "POST",
                        data: {
                            Documento: documento,
                            Registro: registro,
                            Usuario: usuario,
                            IdConcepto: idConcepto,
                            ValorNomina: valorNomina,
                            Cantidad: cantidad
                        },
                        success: function (data) {
                            console.log("Correcto:");
                            console.log(data);
                            if (data.Tipo == "OK") {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: data.Mensaje,
                                    showConfirmButton: false,
                                    timer: 1500
                                });                                

                                TableNomina.ajax.reload();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: data.Mensaje
                                })
                            }
                        },
                        error: function (data) {
                            console.log("Error:");
                            console.log(data);
                        },
                    });

                }
            }
        });
    });

    $(document).on("click", "#ModalEditar .close", function () {
        $("#ModalEditar").modal("hide");
    });

});