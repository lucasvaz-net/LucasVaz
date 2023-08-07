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

        }
    };

    escrever();
}

const titulo = document.querySelector('h1');
typeWriter(titulo);

