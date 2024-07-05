const starsContainer = document.querySelector('.stars');
const breakpoints = {
    small: window.matchMedia("(max-width: 600px)"),
    medium: window.matchMedia("(min-width: 601px) and (max-width: 1024px)"),
    large: window.matchMedia("(min-width: 1025px)")
};

const starCounts = {
    small: 100,
    medium: 200,
    large: 300
};

const sizes = ['xsmall', 'small', 'medium', 'large'];

function createStars(count) {
    starsContainer.innerHTML = '';
    for (let i = 0; i < count; i++) {
        const star = document.createElement('div');
        star.className = `star ${sizes[Math.floor(Math.random() * sizes.length)]}`;
        star.style.left = `${Math.random() * 100}%`;
        star.style.top = `${Math.random() * 100}%`;
        star.style.animationDelay = `${Math.random() * 2}s`;
        starsContainer.appendChild(star);
    }
}

function updateStars() {
    if (breakpoints.small.matches) {
        createStars(starCounts.small);
    } else if (breakpoints.medium.matches) {
        createStars(starCounts.medium);
    } else if (breakpoints.large.matches) {
        createStars(starCounts.large);
    }
}
Object.values(breakpoints).forEach(breakpoint => {
    breakpoint.addListener(updateStars);
});

updateStars();

document.addEventListener("DOMContentLoaded", function () {
    let toggleBtn1 = document.querySelector("#toggleAboutBtn1");
    let briefContent1 = document.querySelector("#briefContent1");
    let fullContent1 = document.querySelector("#fullContent1");
    let closeBtn1 = document.querySelector("#closeFullContent1");

    toggleBtn1.addEventListener("click", function () {
        if (!briefContent1.classList.contains("collapse")) {
            briefContent1.classList.add("collapse");
        }
    });

    closeBtn1.addEventListener("click", function () {
        if (briefContent1.classList.contains("collapse")) {
            briefContent1.classList.remove("collapse");
        }
    });

    fullContent1.addEventListener("shown.bs.collapse", function () {
        briefContent1.classList.add("collapse");
    });

    fullContent1.addEventListener("hidden.bs.collapse", function () {
        briefContent1.classList.remove("collapse");
    });
});

document.addEventListener("DOMContentLoaded", function () {
    let toggleBtn2 = document.querySelector("#toggleAboutBtn2");
    let briefContent2 = document.querySelector("#briefContent2");
    let fullContent2 = document.querySelector("#fullContent2");
    let closeBtn2 = document.querySelector("#closeFullContent2");

    toggleBtn2.addEventListener("click", function () {
        if (!briefContent2.classList.contains("collapse")) {
            briefContent2.classList.add("collapse");
        }
    });

    closeBtn2.addEventListener("click", function () {
        if (briefContent2.classList.contains("collapse")) {
            briefContent2.classList.remove("collapse");
        }
    });

    fullContent2.addEventListener("shown.bs.collapse", function () {
        briefContent2.classList.add("collapse");
    });

    fullContent2.addEventListener("hidden.bs.collapse", function () {
        briefContent2.classList.remove("collapse");
    });
});

document.addEventListener("DOMContentLoaded", function () {
    let toggleBtn3 = document.querySelector("#toggleAboutBtn3");
    let briefContent3 = document.querySelector("#briefContent3");
    let fullContent3 = document.querySelector("#fullContent3");
    let closeBtn3 = document.querySelector("#closeFullContent3");

    toggleBtn3.addEventListener("click", function () {
        if (!briefContent3.classList.contains("collapse")) {
            briefContent3.classList.add("collapse");
        }
    });

    closeBtn3.addEventListener("click", function () {
        if (briefContent3.classList.contains("collapse")) {
            briefContent3.classList.remove("collapse");
        }
    });

    fullContent3.addEventListener("shown.bs.collapse", function () {
        briefContent3.classList.add("collapse");
    });

    fullContent3.addEventListener("hidden.bs.collapse", function () {
        briefContent3.classList.remove("collapse");
    });
});
