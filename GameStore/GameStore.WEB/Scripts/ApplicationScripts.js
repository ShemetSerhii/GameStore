$(function() {
    //Listbox setup
    $(".listbox").multiselect({
        includeSelectAllOption: true,
        maxHeight: 200,
        numberDisplayed: 10
    });

    //Sending a modal window for delete
    $.ajaxSetup({ cache: false });
    $(".delete").bind("click",
        function(e) {
            e.preventDefault();

            var href = $(this).attr("href");
            var index = $(this).attr("href").lastIndexOf("/") + 1;
            var id = href.substr(index);

            $.confirm({
                title: "Delete",
                content: "Delete this comment?",
                buttons: {
                    Yes: function() {
                        $.ajax({
                            url: $("#ActionDelete").val(),
                            data: {
                                "commentId": id,
                                "gameId": $("#GameId").val(),
                                "crossProperty": $("#CrossProperty").val()
                            },
                            type: "GET",
                            success: function() {
                                location.reload();
                            }
                        });
                    },

                    No: function() {}
                }
            });
        });

    $(".addForm").bind("click",
        function (e) {
            e.preventDefault();
            $(".form-container").empty();
            var id = $(this).attr("id");
            $("#d" + id + " .btn").css("visibility", "hidden");
            $.ajax({
                url: $("#ActionName").val(),
                success:
                    function(data) { $("." + id).html(data); },
                complete: function() {
                    $("#id").attr("value", id);
                }
            });
        });

    $(".quote").bind("click",
        function() {
            $(".form-container").empty();
            var id = $(this).attr("id");
            $("#d" + id + " .btn").css("visibility", "hidden");
            $.ajax({
                url: $("#ActionName").val(),
                success:
                    function(data) { $("." + id).html(data); },
                complete: function() {
                    $("#Id").attr("value", id);

                    var quoteText = $("#b" + id).text();

                    $("#quote").css({
                        "border-left": "2px solid gray",
                        "margin-left": "15px",
                        "background": "lightgray",
                        "display": "inline-block"
                    });
                    $("#quote").text(quoteText);
                    $("#quoted").val(quoteText);
                }
            });
        });

    //Cancel writing comment
    $("body").on("click",
        ".btn-cancel",
        function(e) {
            e.preventDefault();
            $(".form-container").empty();
            $(".btn").css("visibility", "visible");
        });

    $(".PageSize").bind("change",
        function() {
            var pageSize = $(this).find(":selected").val();
            $(".PageSize").val(pageSize);
            $(".form").submit();
        });

    if (!$("#UserModel_IsBanned").checked) {
        $("#BanExpires").toggle();
    }

    $("#UserModel_IsBanned").bind("change",
        function() {
            $("#BanExpires").toggle();
        });

    $(".add-btn").bind("mouseover",
        function () {
            $(this).find("img").first().attr("src", "/Content/Images/Cart/Add Icon-White.svg");
            $(this).removeClass("add-btn");
            $(this).addClass("product-quantity-control-hover");
        });

    $(".remove-btn").bind("mouseover",
        function() {
            var div1 = $(this).prop("parentNode");
            var div2 = $(div1).prop("parentNode");
            var quantity = $(div2).find(".product-quantity").first().text();

            if (quantity != "1") {
                $(this).removeClass("remove-btn");
                $(this).addClass("product-quantity-control-hover");
            }
            else {
                $(".remove-btn").addClass("disabled");
                $(".remove-btn").find("img").addClass("disabled");
            }
        });

    $(".add-btn").bind("mouseout",
        function () {
            $(this).find("img").first().attr("src", "/Content/Images/Cart/Add Icon-Orange.svg");
            $(this).removeClass("product-quantity-control-hover");
            $(this).addClass("add-btn");
        });

    $(".remove-btn").bind("mouseout",
        function() {
            $(this).addClass("remove-btn");
            $(this).removeClass("product-quantity-control-hover");
            $(".remove-btn").removeClass("disabled");
            $(".remove-btn").find("img").removeClass("disabled");
        });

    $(".Culture-RU").toggle();

    $(".nav-tabs a").bind("click",
        function () {
            $(".Culture-EU").toggle();
            $(".Culture-RU").toggle();
        });

    $(".start").append(CreateDivStart());
    $(".previous").append(CreateDivPrevious());
    $(".next").append(CreateDivNext());
    $(".end").append(CreateDivEnd());

    $(".filter-aside-body input[type='submit']").bind("click",
        function() {
            $(".hidden-part-name").val($("#FilterModel_PartOfName").val());
        });
});

function CreateDivStart() {
    var div = document.createElement("div");
    div.append(CreateLeftArrow());
    div.append(CreateLeftArrow());

    return div;
}

function CreateDivPrevious() {
    var div = document.createElement("div");
    div.append(CreateLeftArrow());

    return div;
}

function CreateDivNext() {
    var div = document.createElement("div");
    div.append(CreateRightArrow());

    return div;
}

function CreateDivEnd() {
    var div = document.createElement("div");
    div.append(CreateRightArrow());
    div.append(CreateRightArrow());

    return div;
}

function CreateLeftArrow() {
    var arrow = document.createElement("img");
    arrow.setAttribute("src", "/Content/Images/GameList/Arrow.svg");
    arrow.setAttribute("class", "rotate");

    return arrow;
}

function CreateRightArrow() {
    var arrow = document.createElement("img");
    arrow.setAttribute("src", "/Content/Images/GameList/Arrow.svg");

    return arrow;
}