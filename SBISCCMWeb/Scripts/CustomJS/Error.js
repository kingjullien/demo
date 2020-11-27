// When Error Occure and it is Popup at that we hide some content of the Page for display proper message in popup
$(document).ready(function (e) {
    
    var w = document.defaultView || document.parentWindow;
    var frames = w.parent.document.getElementsByTagName('iframe');
    if (frames.length > 0)
    {
        for (var i = frames.length; i-- > 0;) {
            var frame = frames[i];
            try {
                var d = frame.contentDocument || frame.contentWindow.document;
                if (d === document)
                {
                    $('header').remove();
                    $("#ribbon").remove();
                    $("#left-panel").remove();
                    $(".page-footer").remove();
                    $(".fixed-header #main").css("margin-top", "0px");
                }
            } catch (e) { }
        }  
    }
   
});