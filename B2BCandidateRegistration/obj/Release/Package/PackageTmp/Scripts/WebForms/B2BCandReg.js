var progloadingMsg = "Processing your request.";
var progsavingMsg = "Saving/Updating your request.";
var progLoginMsg = "Validating your credentials.";

function ShowProgress(passmsgcode) {

    // Remove any existing overlay first
    $("#modernloading-overlay").remove();

    var message = "Processing your request.";
    if (passmsgcode == "progloadingMsg") {
        message = progloadingMsg;
    }
    else if (passmsgcode == "progsavingMsg") {
        message = progsavingMsg;
    }
    else if (passmsgcode == "progLoginMsg") {
        message = progLoginMsg;
    }

    // Create overlay with spinner + text inside
    var modernloadingoverlay = $(
        '<div id="modernloading-overlay" class="modernloading-overlay">' +
        '<div class="modernloading-box">' +
        '<div class="modernspinner-container">' +
        '<div class="modernloading-spinner"></div>' +
        '<img src="https://tms.languagetesting.com/TMS/images/lti-logo.png" alt="Company Logo" class="modernspinner-logo" />' +
        '</div>' +
        '<div class="modernloading-title">' +
        'Please Wait...' +
        '</div>' +
        '<div class="modernloading-message">' +
        message +
        '</div>' +
        '</div >' +
        '</div >'
    );

    modernloadingoverlay.appendTo('body');
}

function HideProgress() {    
    $('#modernloading-overlay').fadeOut('slow', function () {
        $(this).remove();
    });
}

function showMessage(Message) {
    $('<div id="LTIgenericoverlay"></div>')
        .css('top', $(document).scrollTop())
        .css('opacity', '0.75')
        .appendTo('body');

    document.getElementById('divPopupMessage').innerHTML = "<br/><br/>" + Message + "<br/><br/>";    
    document.getElementById('xmlPopupMessage').style.display = 'block';

    //use left align if we are displaying a list
    if (Message.indexOf("<ul>") > 0) {
        $('#divPopupMessage').css('textAlign', 'left');
        $('#divPopupMessage').css('padding-left', '10px;');
    }
    else {
        $('#divPopupMessage').css('textAlign', 'center');
    }
}

function hideMessage() {

    $('#LTIgenericoverlay').fadeOut('slow', function () {
        $(this).remove();
    });

    document.getElementById('xmlPopupMessage').style.display = 'none';
}

