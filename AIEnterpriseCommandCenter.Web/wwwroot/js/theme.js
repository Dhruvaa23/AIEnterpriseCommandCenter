const themeToggle = document.getElementById("themeToggle");

const currentTheme = localStorage.getItem("theme");

if (currentTheme === "dark") {
    document.body.classList.add("dark-mode");
    themeToggle.innerHTML = '<i class="bi bi-sun"></i>';
}

themeToggle.addEventListener("click", () => {

    document.body.classList.toggle("dark-mode");

    if (document.body.classList.contains("dark-mode")) {

        localStorage.setItem("theme", "dark");

        themeToggle.innerHTML = '<i class="bi bi-sun"></i>';

    }
    else {

        localStorage.setItem("theme", "light");

        themeToggle.innerHTML = '<i class="bi bi-moon-stars"></i>';

    }

});