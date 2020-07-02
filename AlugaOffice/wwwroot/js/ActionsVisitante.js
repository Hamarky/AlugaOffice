$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
    QuantidadeProduto();
});
function QuantidadeProduto() {
    $("#order .btn-light").click(function () {

        var pai = $(this).parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().parent();

        if ($(this).hasClass("diminuir")) {
            var id = pai.find("inputProdutoId").val();
            alert("Clicou no botão - :" + id);

        }
        if ($(this).hasClass("aumentar")) {
            var id = pai.find("inputProdutoId").val();
            alert("Clicou no botão + :" + id);
        }

    });
}
function MudarImagemPrincipalProduto() {
    $(".img-small-wrap img").click(function () {
        var Caminho = $(this).attr("src");
        $(".img-big-wrap img").attr("src", Caminho);
        $(".img-big-wrap").attr("href", Caminho);
    });
}

function MoverScrollOrdenacao() {
    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash == "#ordenacao-produto") {
            window.scrollBy(0, 500);
        }
    }
}
function MudarOrdenacao() {
    $("#ordenacao").change(function () {


        var Pagina = 1;
        var Pesquisa = "";
        var Ordenacao = $(this).val();
        var Fragmento = "#ordenacao-produto";

        var QueryString = new URLSearchParams(window.location.search);
        if (QueryString.has("pagina")) {
            Pagina = QueryString.get("pagina");
        }
        if (QueryString.has("pesquisa")) {
            Pesquisa = QueryString.get("pesquisa");
        }
        if ($("#breadcrumb").length > 0) {
            Fragmento = "";
        }

        var URL = window.location.protocol + "//" + window.location.host + window.location.pathname;

        var URLComParametros = URL + "?pagina=" + Pagina + "&pesquisa=" + Pesquisa + "&ordenacao=" + Ordenacao + Fragmento;
        window.location.href = URLComParametros;


    });
}