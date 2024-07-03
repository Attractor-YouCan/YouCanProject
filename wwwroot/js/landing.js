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


