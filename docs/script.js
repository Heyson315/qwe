// Contact Form Handling
document.getElementById('contactForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData);
    
    // Option 1: Send to ASP.NET MVC backend
    // When Docker container is running, this will send to your API
    const API_ENDPOINT = 'http://localhost:8080/api';
    
    try {
        // For now, show success message (implement actual API call when backend is ready)
        console.log('Form data:', data);
        
        // Uncomment when API endpoint is ready:
        /*
        const response = await fetch(`${API_ENDPOINT}/contact`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });
        
        if (response.ok) {
            alert('Thank you! We will contact you soon.');
            e.target.reset();
        } else {
            alert('Something went wrong. Please try again.');
        }
        */
        
        // Temporary success message
        alert('Thank you for your interest! We will contact you soon.\n\nNote: To enable form submission, run the Docker container and implement the /api/contact endpoint.');
        e.target.reset();
        
    } catch (error) {
        console.error('Error:', error);
        alert('Unable to send message. Please email us directly at contact@hhrcpa.us');
    }
});

// Smooth scrolling for navigation
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            const headerOffset = 80;
            const elementPosition = target.getBoundingClientRect().top;
            const offsetPosition = elementPosition + window.pageYOffset - headerOffset;

            window.scrollTo({
                top: offsetPosition,
                behavior: 'smooth'
            });
        }
    });
});

// Add animation on scroll
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -100px 0px'
};

const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observe feature and service cards
document.querySelectorAll('.feature-card, .service-card, .tech-item').forEach(el => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(30px)';
    el.style.transition = 'all 0.6s ease-out';
    observer.observe(el);
});

// Check if Docker container is running
async function checkDockerStatus() {
    try {
        const response = await fetch('http://localhost:8080/api/services', {
            method: 'GET',
            mode: 'no-cors'
        });
        console.log('Docker container is running ✓');
        return true;
    } catch (error) {
        console.log('Docker container is not running. Start it with: docker-compose up -d');
        return false;
    }
}

// Check Docker status on page load
checkDockerStatus();

// Add SharePoint link helper
function openSharePoint() {
    window.open('https://rahmanfinanceandaccounting.sharepoint.com/sites/m365appbuilder-infogrid-8856/Shared%20Documents/Forms/AllItems.aspx', '_blank');
}

// Add navbar scroll effect
let lastScroll = 0;
window.addEventListener('scroll', () => {
    const header = document.querySelector('header');
    const currentScroll = window.pageYOffset;
    
    if (currentScroll > 100) {
        header.style.boxShadow = '0 4px 20px rgba(0,0,0,0.15)';
    } else {
        header.style.boxShadow = '0 2px 10px rgba(0,0,0,0.1)';
    }
    
    lastScroll = currentScroll;
});

// Add click tracking for analytics (when implemented)
document.querySelectorAll('a').forEach(link => {
    link.addEventListener('click', function() {
        const href = this.getAttribute('href');
        const text = this.textContent;
        console.log(`Link clicked: ${text} (${href})`);
        // TODO: Send to analytics service
    });
});

// Helper function to format dates
function formatDate(date) {
    return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    }).format(date);
}

// Display current year in footer
const currentYear = new Date().getFullYear();
document.querySelector('.footer-bottom p').textContent = `© ${currentYear} HHR CPA. All rights reserved.`;

// Add loading state to buttons
function setButtonLoading(button, isLoading) {
    if (isLoading) {
        button.disabled = true;
        button.dataset.originalText = button.textContent;
        button.textContent = 'Loading...';
    } else {
        button.disabled = false;
        button.textContent = button.dataset.originalText;
    }
}

// Form validation helper
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Add real-time form validation
const emailInput = document.querySelector('input[name="email"]');
if (emailInput) {
    emailInput.addEventListener('blur', function() {
        if (this.value && !validateEmail(this.value)) {
            this.style.borderColor = 'red';
            this.setCustomValidity('Please enter a valid email address');
        } else {
            this.style.borderColor = '#e0e0e0';
            this.setCustomValidity('');
        }
    });
}

// Console welcome message
console.log('%cHHR CPA Platform', 'color: #0066cc; font-size: 24px; font-weight: bold;');
console.log('%cBuilt with ASP.NET MVC + Docker + SQL Server', 'color: #00b894; font-size: 14px;');
console.log('GitHub: https://github.com/Heyson315/qwe');
console.log('SharePoint: https://rahmanfinanceandaccounting.sharepoint.com');
