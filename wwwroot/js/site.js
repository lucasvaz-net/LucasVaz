function typeWriter(elemento) {
    const texto = elemento.innerHTML;
    elemento.innerHTML = '';

    let index = 0;

    const escrever = () => {
        elemento.innerHTML += texto[index];
        index++;

        if (index < texto.length) {
            setTimeout(escrever, 100);
        } else {
            exibirBotao(); // Chama a função para exibir o botão após a digitação do texto
        }
    };

    escrever();
}

const titulo = document.querySelector('h1');
typeWriter(titulo);


setTimeout(function () {
    document.getElementById("button-container").style.opacity = "1";
}, 13000);