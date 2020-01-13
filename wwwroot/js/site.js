// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function ConvertInputsToBeAsArrayItem(elem, arrayName, index) {
    $(elem).find("input, select").each(function (i, e) {
        $(e).attr("id", arrayName + "_" + index + "__" + $(e).attr("id"));
        $(e).attr("name", arrayName + "[" + index + "]." + $(e).attr("name"));
    });
    return elem;
}

function ToggleNearInput(thisObj, name) {
    thisObj.parent().parent().siblings().children("input[name$='" + name + "']").prop('disabled', function (i, v) { return !v; });
}