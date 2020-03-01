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

function RenderPdfIntoCanvas(dataBytes, canvas, moreThanOnePageWarning) {
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument({ data: dataBytes });
    loadingTask.promise.then(
        function (pdf) {

            if (pdf.numPages > 1)
                $(moreThanOnePageWarning).removeClass("d-none");
            else
                $(moreThanOnePageWarning).addClass("d-none");

            // Load information from the first page.
            pdf.getPage(1).then(function (page) {
                var scale = 1.5;
                var viewport = page.getViewport(scale);

                // Apply page dimensions to the <canvas> element.
                var context = canvas.getContext("2d");
                canvas.height = viewport.height;
                canvas.width = viewport.width;

                // Render the page into the <canvas> element.
                var renderContext = {
                    canvasContext: context,
                    viewport: viewport
                };
                page.render(renderContext).then(function () {
                    console.log("Page rendered!");
                });
            });
        },
        function (reason) {
            console.error(reason);
        }
    );
}