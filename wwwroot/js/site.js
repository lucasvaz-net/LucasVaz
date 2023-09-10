document.addEventListener("DOMContentLoaded", function () {

    function typeWriter(elemento) {
        if (!elemento) return; // Se o elemento for null ou undefined, saia da função

        const texto = elemento.innerHTML;
        elemento.innerHTML = '';

        let index = 0;

        const escrever = () => {
            elemento.innerHTML += texto[index];
            index++;

            if (index < texto.length) {
                setTimeout(escrever, 100);
            }
        };

        escrever();
    }

    const titulo = document.querySelector('h1');
    if (titulo) {
        typeWriter(titulo);
    }
});
