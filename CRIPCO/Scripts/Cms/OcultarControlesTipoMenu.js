$(function () {
        $("#SelectedTipo").change(function () {
        var t = $(this).val();

        if (t == "M") {
            $("#Descripcion").hide();
            $("#CargarImagenes").hide();
            $("#ImagenesCargadas").hide();
            
        }
        else {
            $("#Descripcion").show();
            $("#CargarImagenes").show();
            $("#ImagenesCargadas").show();
        }


        if (t == "A") {
            $("#ImputImages").show()
        }
        else {
            $("#ImputImages").hide()
        }


        if (t == "G") {
            $("#TableImages").show()
        }
        else {
            $("#TableImages").hide()
        }


        if (t == "V") {
            $("#TableVideos").show()
        }
        else {
            $("#TableVideos").hide()
        }
    });
    
});

