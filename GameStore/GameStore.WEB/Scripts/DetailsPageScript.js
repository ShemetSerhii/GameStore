$("a:last").bind("click", function() {
    var tab = window.open("data:text/html");
    setTimeout(tab.close(), 1000);
})