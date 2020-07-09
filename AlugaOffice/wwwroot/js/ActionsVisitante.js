$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
    MudarQuantidadeProdutoCarrinho();
    AlteracoesVisuaisProdutoCarrinho();
    AtualizarQuantidadeEValor();
    MostrarMensagemDeErros();
    AcaoCalcularFrete();
    AJAXCalcularFrete(false);
    AJAXBuscarCEP();
    AJAXEnderecoEntregaCalcularFrete();
});

function AJAXEnderecoEntregaCalcularFrete() {
    $("input[name=endereco]").change(function () {
        var cep = RemoverMascara($(this).parent().find("input[name=cep]").val());

        $.cookie("Carrinho.Endereco", $(this).val(), { path: "/" });
        
        $.ajax({
            type: "GET",
            url: "/CarrinhoCompra/CalcularFrete?cepDestino=" + cep,
            error: function (data) {
                MostrarMensagemDeErro("Opps! Tivemos um erro ao obter o Frete..." + data.Message);

            },
            success: function (data) {
                for (var i = 0; i < data.listaValores.length; i++) {
                    var tipoFrete = data.listaValores[i].tipoFrete;
                    var valor = data.listaValores[i].valor;
                    var prazo = data.listaValores[i].prazo;

                    $("#titulo")[i].innerHTML = tipoFrete;
                    $("#prazoFrete")[i].innerHTML = "Prazo de " + prazo + " dias.";
                    $("#valorFrete")[i].innerHTML = "<input type=\"radio\" name=\"frete\" value=\" " + tipoFrete + "\" id='" + tipoFrete + "' /> <strong><label for='" + tipoFrete + "'>" + numberToReal(valor) + "</label></strong>";
                }
            }
        });
    });
}

function EnderecoEntregaCardsLoading() {
    for (var i = 0; i < 2; i++) {
        $("#prazoFrete")[i].innerHTML = "<br /> <img src='\\img\\spinner.gif' class='center' style='width: 60; height: 60px;' />";
    }
}
function EnderecoEntregaCardsLimpar() {
    for (var i = 0; i < 2; i++) {
        $("#titulo")[i].innerHTML = "-";
        $("#prazoFrete")[i].innerHTML = "-";
        $("#valorFrete")[i].innerHTML = "-"
    }
}

function AJAXBuscarCEP() {
    $("#CEP").keyup(function () {
        OcultarMensagemDeErro();



        if ($(this).val().length == 10) {

            var cep = RemoverMascara($(this).val());
            $.ajax({
                type: "GET",
                url: "https://viacep.com.br/ws/" + cep + "/json/?callback=callback_name",
                dataType: "jsonp",
                error: function (data) {
                    MostrarMensagemDeErro("Parece que os servidores estão offline!");
                },
                success: function (data) {
                    if (data.erro == undefined) {
                        $("#Estado").val(data.uf);
                        $("#Cidade").val(data.localidade);
                        $("#Endereco").val(data.logradouro);
                        $("#Bairro").val(data.bairro);
                    } else {
                        MostrarMensagemDeErro("O CEP informado não existe!");
                    }

                }
            });
        }
    });
}

function AcaoCalcularFrete() {
    AJAXCalcularFrete(true);
}

function AJAXCalcularFrete(byButtom) {
    $(".btn-continuar").addClass("disabled");
    if (byButtom == false) {
        if ($.cookie('Carrinho.CEP') != undefined) {
            $(".cep").val();
        }
    }


    if ($(".cep").length > 0) {
        var cep = RemoverMascara($(".cep").val());

        $.removeCookie("Carrinho.TipoFrete");

        if (cep.length == 8) {
            $.cookie('Carrinho.CEP', $("#CEP").val());
            $(".container-frete").html("<img src='\\img\\spinner.gif' class='center' style='width: 60; height: 60px;' />");
            $(".frete").text("R$ 0,00");
            $(".total").text("R$ 0,00");

            html = "";
            $.ajax({
                type: "GET",
                url: "/CarrinhoCompra/CalcularFrete?cepDestino=" + cep,
                error: function (data) {
                    MostrarMensagemDeErro("Tivemos um erro ao obter essa informação " + data.Message);
                    console.info(data);
                },
                success: function (data) {

                    for (var i = 0; i < data.listaValores.length; i++) {
                        var tipoFrete = data.listaValores[i].tipoFrete;
                        var valor = data.listaValores[i].valor;
                        var prazo = data.listaValores[i].prazo;

                        html += "<dl class=\"dlist-align\"><dt><input type=\"radio\" name=\"frete\" value=\"" + tipoFrete + "\" /><input type=\"hidden\" name=\"valor\" value=\"" + valor + "\" /></dt><dd>" + tipoFrete + " - " + numberToReal(valor) + " <p>(" + prazo + " dias últeis)</p></dd></dl>";
                    }
                    $(".container-frete").html(html);
                    $(".container-frete").find("input[type=radio]").change(function () {
                        var valorFrete = parseFloat($(this).parent().find("input[type=hidden]").val());
                        $.cookie("Carrinho.TipoFrete", $(this).val());
                        $(".btn-continuar").removeClass("disabled");
                        $(".frete").text(numberToReal(valorFrete));

                        var subtotal = parseFloat($(".subtotal").text().replace("R$", "").replace(".", "").replace(",", "."));
                        console.info("Subtotal: " + subtotal);

                        var total = valorFrete + subtotal;

                        $(".total").text(numberToReal(total))
                    });
                }
            });
        } else {
            if (byButtom == true) {
                $(".container-frete").html("");
                MostrarMensagemDeErro("Campo CEP invalido!")
            }
        }
    }
}

function numberToReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}

function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-light").click(function () {
        if ($(this).hasClass("diminuir")) {
            OrquestradorDeAcoesProduto("diminuir", $(this));
        }
        if ($(this).hasClass("aumentar")) {
            OrquestradorDeAcoesProduto("aumentar", $(this));
        }
    });
}

function OrquestradorDeAcoesProduto(operacao, botao) {
    OcultarMensagemDeErro();
    var pai = botao.parent().parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var quantidadeEstoque = parseInt(pai.find(".inputQuantidadeEstoque").val());
    var valorUnitario = parseFloat(pai.find(".inputValorUnitario").val().replace(",", "."));

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var quantidadeProdutoCarrinhoAntiga = parseInt(campoQuantidadeProdutoCarrinho.val());

    var campoValor = botao.parent().parent().parent().find(".price");

    var produto = new ProdutoQuantidadeEValor(produtoId, quantidadeEstoque, valorUnitario,
        quantidadeProdutoCarrinhoAntiga, 0, campoQuantidadeProdutoCarrinho, campoValor);

    AlteracoesVisuaisProdutoCarrinho(produto, operacao);
}

function AlteracoesVisuaisProdutoCarrinho(produto, operacao) {
    if (operacao == "aumentar") {
        produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga + 1;

        AtualizarQuantidadeEValor(produto);

        AJAXComunicarAuteracaoQuantidadeProduto(produto)

    } else if (operacao == "diminuir") {

        produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga - 1;

        AtualizarQuantidadeEValor(produto);

        AJAXComunicarAuteracaoQuantidadeProduto(produto)
    }
}

function AJAXComunicarAuteracaoQuantidadeProduto(produto) {
    $.ajax({
        type: "GET",
        url: "/CarrinhoCompra/AlterarQuantidade?id=" + produto.produtoId + "&quantidade=" + produto.quantidadeProdutoCarrinhoNova,
        error: function (data) {
            MostrarMensagemDeErro(data.responseJSON.mensagem)
            //Rollback
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga;
            AtualizarQuantidadeEValor(produto)
        },
        success: function () {
            AJAXCalcularFrete();
        }
    });
}

function MostrarMensagemDeErro(mensagem) {
    $(".alertaEstoque").css("display", "block");
    $(".alertaEstoque").text(mensagem);
}

function OcultarMensagemDeErro() {
    $(".alertaEstoque").css("display", "none");
}

function AtualizarQuantidadeEValor(produto) {
    produto.campoQuantidadeProdutoCarrinho.val(produto.quantidadeProdutoCarrinhoNova);
    var resultado = produto.valorUnitario * produto.quantidadeProdutoCarrinhoNova;
    produto.campoValor.text(numberToReal(resultado));

    AtualizarSubtotal();
}

function AtualizarSubtotal() {
    var Subtotal = 0;
    var TagPrice = $(".price");
    TagPrice.each(function () {
        var ValorReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));

        Subtotal += ValorReais;
    });
    $(".subtotal").text(numberToReal(Subtotal));
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

class ProdutoQuantidadeEValor {
    constructor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, quantidadeProdutoCarrinhoNova,
        campoQuantidadeProdutoCarrinho, campoValor) {

        this.produtoId = produtoId;
        this.quantidadeEstoque = quantidadeEstoque;
        this.valorUnitario = valorUnitario;

        this.quantidadeProdutoCarrinhoAntiga = quantidadeProdutoCarrinhoAntiga;
        this.quantidadeProdutoCarrinhoNova = quantidadeProdutoCarrinhoNova;

        this.campoQuantidadeProdutoCarrinho = campoQuantidadeProdutoCarrinho;
        this.campoValor = campoValor;
    }
}

function RemoverMascara(valor) {
    return valor.replace(".", "").replace("-", "");
}