$(document).ready(function () {
    $("#Nascimento").mask("00/00/0000");
    $("#CEP").mask("00.000-000");
    $("#Telefone").mask("(00) 00000-0000");
    $("#CPF").mask('000.000.000-00', { reverse: true });
});