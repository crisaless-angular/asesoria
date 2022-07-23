function Getlogo()
{
    $.get("../../Public/GetLogo/", function (data) {

        if (data != undefined || data != null)
        {
            let spanLogo = $("#divlogo");
            let src = `${window.location.origin}/images/${data}`;
            spanLogo.prepend('<a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index"><img id="logo" src="' + src + '" width="120" height="60"></a>');
        }
        
        /*document.getElementById("logo").src = `${window.location.origin}/images/${data}`;*/
    });
}