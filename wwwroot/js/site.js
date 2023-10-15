document.addEventListener("DOMContentLoaded", function () {

    let isTyping = false;

    function typeWriter(elemento) {
        if (!elemento || !elemento.innerHTML || isTyping) return;

        isTyping = true;

        const texto = elemento.innerHTML.trim();
        if (!texto) return;

        elemento.innerHTML = '';
        elemento.style.visibility = 'visible';

        let index = 0;

        const escrever = () => {
            elemento.innerHTML += texto[index];
            index++;

            if (index < texto.length) {
                setTimeout(escrever, 75);
            } else {
                isTyping = false;
            }
        };

        escrever();
    }

    window.startTyping = function (selector) {
        if (isTyping) return;

        const element = document.querySelector(selector);
        typeWriter(element);
        setTimeout(function () {
            document.getElementById("button-containersobre").style.opacity = "1";
        }, 25500);
    };

    const tituloInicio = document.querySelector('#content-container h1#type');
    typeWriter(tituloInicio);

    window.goToTop = function () {
        window.scroll({ top: 0, left: 0, behavior: 'smooth' });
    }

    window.addEventListener("scroll", function () {
        let backToTopBtn = document.getElementById("backToTop");
        if (window.pageYOffset > 300) {
            backToTopBtn.style.display = "block";
        } else {
            backToTopBtn.style.display = "none";
        }
    });

});
