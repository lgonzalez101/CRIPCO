var max_fieldsformation = 10; //maximum input boxes allowed
var wrapperformation = $("#input_fields_wrapJobs"); //Fields wrapper
var add_buttonformation = $("#add_field_buttonJobs"); //Add button ID
var x = $("#input_fields_wrapJobs > tbody").children().length; //initlal text box count
var esta = false;

$(add_buttonformation).click(function (e) { //on add input button click
    e.preventDefault();
    if (x < max_fieldsformation) { //max input box allowed
        x++; //text box increment       

        var gui = GetRandomGUI();
        var htm = '';
        htm += '<tr>\n\n';
        htm += '                                        <td><input id="Files" name="Files" type="file" accept="image/*" value="System.Collections.Generic.List`1[System.Web.HttpPostedFileBase]"></td>\n'
        htm += '                                        <td ><a href="#" class="remove_fieldJobs">      Eliminar</a></td >\n'
        htm += '                                    </tr>'

        
        $(wrapperformation).append(htm); //add input box        
        x++;
    }
});

$(wrapperformation).on("click", ".remove_fieldJobs", function (e) { //user click on remove text
    e.preventDefault(); $(this).closest('tr').remove(); x--;
})


function GetRandomGUI() {
    var val = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    return val;
}
