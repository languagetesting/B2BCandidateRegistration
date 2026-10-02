function registerPasswordToggle(toggle,passinput) {    
    var passwordInput = document.getElementById(passinput); 
    
    if (!toggle || !passwordInput) {
        return false;
    }

    var isHidden = passwordInput.type === 'password';
    passwordInput.type = isHidden ? 'text' : 'password';

    toggle.classList.toggle('fa-eye');
    toggle.classList.toggle('fa-eye-slash');

    toggle.title = isHidden ? 'Hide Password' : 'Show Password';

    return false;
}

function CheckValidationControl() {
    var errMessage = "";    
    Page_ClientValidate();    
    if (Page_IsValid == false) {
        for (i = 0; i < Page_Validators.length; i++) {
            if (!Page_Validators[i].isvalid)
                errMessage += Page_Validators[i].errormessage + "<br />";
        }
        HideProgress();
        showMessage(errMessage);
    }    
}