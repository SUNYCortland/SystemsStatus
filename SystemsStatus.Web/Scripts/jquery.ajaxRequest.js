$.extend({
    ajaxRequest: function (url, type, datagetter, onsuccess, onerror) {
        /// <summary>jQuery extension that executes Ajax calls and handles errors in application specific ways.</summary>
        /// <param name="url" type="String">URL address where to issue this Ajax call.</param>
        /// <param name="type" type="String">HTTP method type (GET, POST, DELETE, PUT, HEAD)</param>
        /// <param name="datagetter" type="Function">This parameterless function will be called to return ajax data.</param>
        /// <param name="onsuccess" type="Function">This optional function(data) will be called after a successful Ajax call.</param>
        /// <param name="onerror" type="Function">This optional function(data) will be called after an erroneous Ajax call.</param>

        var execSuccess = $.isFunction(onsuccess) ? onsuccess : $.noop;
        var execError = $.isFunction(onsuccess) ? onsuccess : $.noop;
        var getData = $.isFunction(datagetter) ? datagetter : function () { return datagetter; };

        $.ajax({
            url: url,
            type: type,
            data: getData(),
            contentType: "application/json",
            error: function (xhr, status, err) {
                $("#modalTitle").html("Error");
                $("#modalBody").html("");

                if (xhr.status == 400) {
                    var status = $.parseJSON(xhr.responseText);
                    $.each(status.errors, function (index, error) {
                        $("#modalBody").append(error.Error);
                    });
                }
                else {
                    $("#modalBody").html("A server error occurred. If this problem persists, please contact a systems administrator.");
                }

                $("#modal").modal("show");

                window.setTimeout(function () {
                    execError(status);
                }, 10);
            },
            success: function (data, status, xhr) {
                if (data.hasOwnProperty("unauthorized"))
                    location.reload();

                window.setTimeout(function () {
                    execSuccess(data);
                }, 10);
            }
        });
    }
});