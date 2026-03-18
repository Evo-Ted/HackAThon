// Animation au scroll des cartes
const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry, index) => {
        if (entry.isIntersecting) {
            const delay = entry.target.dataset.delay || 0;
            setTimeout(() => {
                entry.target.classList.add('visible');
            }, delay * 150);
            observer.unobserve(entry.target);
        }
    });
}, {
    threshold: 0.1
});

// Observer les cartes valeurs et membres
document.querySelectorAll('.valeur-card, .membre-card').forEach(card => {
    observer.observe(card);
});

// Animation compteur pour les stats
function animateCounter(element, target, duration = 1500) {
    const isPercent = target.toString().includes('%');
    const isPlus = target.toString().includes('+');
    const num = parseInt(target);
    let start = 0;
    const step = num / (duration / 16);

    const timer = setInterval(() => {
        start += step;
        if (start >= num) {
            start = num;
            clearInterval(timer);
        }
        element.textContent = Math.floor(start) + (isPlus ? '+' : '') + (isPercent ? '%' : '');
    }, 16);
}

// Lancer les compteurs quand les stats sont visibles
const statsObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            const statNumbers = entry.target.querySelectorAll('.stat-number');
            statNumbers.forEach(el => {
                const originalText = el.textContent;
                animateCounter(el, originalText);
            });
            statsObserver.unobserve(entry.target);
        }
    });
}, { threshold: 0.3 });

const statsSection = document.querySelector('.apropos-project-stats');
if (statsSection) statsObserver.observe(statsSection);