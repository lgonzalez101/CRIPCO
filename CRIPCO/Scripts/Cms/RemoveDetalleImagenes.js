var wrapperformation = $("#input_fields_wrapDetalle"); //Fields wrapper

$(wrapperformation).on("click", ".remove_fieldDetalle", function (e) { //user click on remove text
    e.preventDefault(); $(this).closest('tr').remove(); 
})

